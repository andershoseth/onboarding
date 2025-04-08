namespace onboarding_backend;

using System.Text;
using System.Globalization;
using ExcelDataReader;

public static class FileProcessor
{
    public static List<Dictionary<string, string>> ProcessCsv(Stream stream)
    {
        var results = new List<Dictionary<string, string>>();

        // We'll read line-by-line manually
        stream.Position = 0;
        using var reader = new StreamReader(stream, Encoding.UTF8);

        // 1) Detect delimiter based on the *first* line with actual content
        //    If you want to handle "empty lines" at the top, skip them first
        //    But let’s keep it simple: read the first non-null line for delimiter detection
        string? firstNonEmptyLine = null;
        while (!reader.EndOfStream)
        {
            var peekLine = reader.ReadLine();
            if (!string.IsNullOrWhiteSpace(peekLine))
            {
                firstNonEmptyLine = peekLine;
                break;
            }
        }
        if (firstNonEmptyLine == null)
        {
            // Entire file empty
            return results;
        }

        char delimiter = firstNonEmptyLine.Contains(";") ? ';' : ',';

        // We'll store that line for further parsing, so let's create a lines list:
        var lines = new List<string> { firstNonEmptyLine };

        // 2) Read the rest of the file
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line != null)
            {
                lines.Add(line);
            }
        }

        // Now we have all lines in memory
        // We'll parse them table by table

        string? currentTableName = null;
        string[]? currentHeaders = null;
        bool awaitingHeader = false;
        bool inTable = false;

        foreach (var rawLine in lines)
        {
            var trimmedLine = rawLine.Trim();
            // Check if line is empty
            if (string.IsNullOrEmpty(trimmedLine))
            {
                // If we were in a table, end it
                if (inTable)
                {
                    inTable = false;
                    currentHeaders = null;
                    currentTableName = null;
                    awaitingHeader = false;
                }
                // If not in a table, just skip
                continue;
            }

            // Split this line by our delimiter
            var parts = trimmedLine.Split(delimiter);

            // 3) Check if first "cell" is bracketed => new table
            if (IsBracketLine(parts[0]))
            {
                // Start a new table
                currentTableName = parts[0].Trim('[', ']');
                awaitingHeader = true;
                inTable = false;
                currentHeaders = null;
                continue;
            }

            // 4) If awaiting header => this line is the header row
            if (awaitingHeader)
            {
                currentHeaders = parts;
                awaitingHeader = false;
                inTable = true;
                continue;
            }

            // 5) If in a table & have headers => data row
            if (inTable && currentHeaders != null)
            {
                var rowDict = new Dictionary<string, string>
                {
                    ["TableName"] = currentTableName ?? ""
                };

                for (int i = 0; i < currentHeaders.Length; i++)
                {
                    string header = currentHeaders[i];
                    // Safely get the cell value
                    string cellValue = (i < parts.Length) ? parts[i].Trim() : "";

                    // If wrapped in quotes, remove them
                    if (cellValue.StartsWith("\"") && cellValue.EndsWith("\""))
                    {
                        cellValue = cellValue.Substring(1, cellValue.Length - 2);
                    }

                    rowDict[header] = cellValue;
                }

                results.Add(rowDict);
            }
            else
            {
                // Non-empty line outside a table => skip or handle differently
            }
        }

        return results;
    }


    // The excel parsing expects a bracket title: [title] as the first row, a header row to follow it, and then data until it hits empty rows
    public static List<Dictionary<string, string>> ProcessExcel(Stream stream)
    {
        // Required by ExcelDataReader for xls/xlsx
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using var reader = ExcelReaderFactory.CreateReader(stream);

        var results = new List<Dictionary<string, string>>();

        // State variables
        string? currentTableName = null;
        string[]? currentHeaders = null;
        bool awaitingHeader = false;
        bool inTable = false;

        int rowNumber = 0; // We'll increment this to log each row's index

        while (reader.Read())
        {
            rowNumber++;

            // Build an array of cell values
            var rowValues = new string[reader.FieldCount];
            for (int c = 0; c < reader.FieldCount; c++)
            {
                rowValues[c] = reader.GetValue(c)?.ToString()?.Trim() ?? "";
            }

            bool isEmpty = IsEmptyRow(rowValues);

            // DEBUG: Print row info
            Console.WriteLine($"Row {rowNumber} → FirstCol='{(rowValues.Length > 0 ? rowValues[0] : "")}', " +
                              $"Empty={isEmpty}, InTable={inTable}, AwaitingHeader={awaitingHeader}, Table='{currentTableName}'");

            if (isEmpty)
            {
                // If we're currently in a table, an empty row ends it
                if (inTable)
                {
                    Console.WriteLine($"    Row {rowNumber} is empty → Ending table '{currentTableName}'");
                    inTable = false;
                    currentHeaders = null;
                    currentTableName = null;
                    awaitingHeader = false;
                }
                else
                {
                    Console.WriteLine($"    Row {rowNumber} is empty → Skipping (not in a table)");
                }
                continue;
            }

            // If first column is bracketed
            if (IsBracketLine(rowValues[0]))
            {
                currentTableName = rowValues[0].Trim('[', ']');
                Console.WriteLine($"    Found bracket line: '{currentTableName}' → Next row is header");
                awaitingHeader = true;
                inTable = false;
                currentHeaders = null;
                continue;
            }

            // If we are awaiting the header row
            if (awaitingHeader)
            {
                currentHeaders = rowValues;
                awaitingHeader = false;
                inTable = true;
                Console.WriteLine($"    Row {rowNumber} set as HEADER → {string.Join(", ", currentHeaders)}");
                continue;
            }

            // If in a table & have headers => data row
            if (inTable && currentHeaders != null)
            {
                var rowDict = new Dictionary<string, string>
                {
                    ["TableName"] = currentTableName ?? ""
                };

                for (int i = 0; i < currentHeaders.Length; i++)
                {
                    string header = currentHeaders[i];
                    string cellVal = (i < rowValues.Length) ? rowValues[i] : "";
                    rowDict[header] = cellVal;
                }
                Console.WriteLine($"    Row {rowNumber} is DATA → Table={currentTableName}, Values=({string.Join(", ", rowDict)})");

                results.Add(rowDict);
            }
            else
            {
                // Non-empty row outside a table
                Console.WriteLine($"    Row {rowNumber} is non-empty but not in a table → skipping");
            }
        }

        Console.WriteLine("Finished reading. Total rows in final 'results': " + results.Count);
        return results;
    }

    private static bool IsEmptyRow(string[] rowValues)
    {
        foreach (var val in rowValues)
        {
            if (!string.IsNullOrEmpty(val)) return false;
        }
        return true;
    }

    private static bool IsBracketLine(string cellValue)
    {
        return !string.IsNullOrEmpty(cellValue) &&
               cellValue.StartsWith("[") &&
               cellValue.EndsWith("]");
    }
}

