namespace onboarding_backend;

using System.Text;
using ExcelDataReader;


public static class FileProcessor
{
    public static List<Dictionary<string, string>> ProcessCsv(Stream stream)
    {
        stream.Position = 0;
        using var reader = new StreamReader(stream, Encoding.UTF8);

        var lines = new List<string>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? "";
            lines.Add(line);
        }

        // No lines? Return empty
        if (lines.Count == 0)
            return new List<Dictionary<string, string>>();

        // First line must always be a bracket line, and the seoncd line always a header line
        string? headerLine = lines.Count > 1 ? lines[1] : null;
        if (string.IsNullOrEmpty(headerLine))
        {
            // if empty
            return new List<Dictionary<string, string>>();
        }

        // Detect delimiter from the header line. not bracket line
        char delimiter = headerLine.Contains(';') ? ';' : ',';

        var results = new List<Dictionary<string, string>>();

        string? currentTableName = null;
        string[]? currentHeaders = null;
        bool awaitingHeader = false;
        bool inTable = false;


        foreach (var rawLine in lines)
        {
            var trimmedLine = rawLine.Trim();
            bool isEmptyLine = string.IsNullOrEmpty(trimmedLine);

            // If line is empty → end the current table (if any)
            if (isEmptyLine)
            {
                if (inTable)
                {
                    inTable = false;
                    awaitingHeader = false;
                    currentHeaders = null;
                    currentTableName = null;
                }
                continue;
            }

            var parts = rawLine.Split(delimiter);

            // If bracket line then enter a new table
            if (IsBracketLine(trimmedLine))
            {
                currentTableName = trimmedLine.Trim('[', ']');
                awaitingHeader = true;
                inTable = false;
                currentHeaders = null;
                continue;
            }

            // If we just saw a bracket line the next line must be a header row
            if (awaitingHeader)
            {
                currentHeaders = parts;
                awaitingHeader = false;
                inTable = true;
                continue;
            }


            // If we're in a table and have headers then parse the data row
            if (inTable && currentHeaders != null)
            {
                var rowDict = new Dictionary<string, string>
                {
                    ["TableName"] = currentTableName ?? ""
                };

                for (int i = 0; i < currentHeaders.Length; i++)
                {
                    string header = currentHeaders[i].Trim();
                    string cellVal = i < parts.Length ? parts[i].Trim() : "";

                    // If wrapped in quotes, remove them
                    if (cellVal.StartsWith("\"") && cellVal.EndsWith("\""))
                    {
                        cellVal = cellVal.Substring(1, cellVal.Length - 2);
                    }

                    rowDict[header] = cellVal;
                }

                results.Add(rowDict);

            }
            else
            {
                // handle line outside of table that isn't empty. this will hopefully never be necessary
            }
        }

        return results;
    }


    // The parsing expects a bracket title: [title] as the first row, a header row to follow it, and then data until it hits empty rows
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

        int rowNumber = 0; // Logging row index

        while (reader.Read())
        {
            rowNumber++;

            // Array of the cell values
            var rowValues = new string[reader.FieldCount];
            for (int c = 0; c < reader.FieldCount; c++)
            {
                var value = reader.GetValue(c);
                string cellValue = "";

                if (value is DateTime dt)
                {
                    if (dt.Year == 1899 && dt.Month == 12 && dt.Day == 31)
                    {
                        cellValue = dt.ToString("HH:mm");
                    }
                    else if (dt.TimeOfDay == TimeSpan.Zero)
                    {
                        cellValue = dt.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        cellValue = dt.ToString("dd-MM-yyyy HH:mm");
                    }
                }
                else
                {
                    cellValue = value?.ToString()?.Trim() ?? "";
                }

                rowValues[c] = cellValue;
            }

            bool isEmpty = IsEmptyRow(rowValues);

            if (isEmpty)
            {
                // If currently in a table, an empty row ends it( ergo no completely empty rows allowed in a table)
                if (inTable)
                {
                    inTable = false;
                    currentHeaders = null;
                    currentTableName = null;
                    awaitingHeader = false;
                }
                else
                {
                    // if empty cell and not in table, then logic can be added here. Not necessary currently
                }
                continue;
            }

            // If first column is bracketed
            if (IsBracketLine(rowValues[0]))
            {
                currentTableName = rowValues[0].Trim('[', ']');
                awaitingHeader = true;
                inTable = false;
                currentHeaders = null;
                continue;
            }

            // If awaiting the header row
            if (awaitingHeader)
            {
                currentHeaders = rowValues;
                awaitingHeader = false;
                inTable = true;
                continue;
            }

            // If in a table & have headers then a data row will follow
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
                // Non-empty row outside a table. Not necessary for us
            }
        }

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

