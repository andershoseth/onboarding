namespace onboarding_backend; // ✅ Ensure this namespace matches your project

using System.Text;
using System.Globalization;
using ExcelDataReader;

public static class FileProcessor // ✅ Add this class wrapper
{
    public static List<Dictionary<string, string>> ProcessCsv(Stream stream)
    {
        var results = new List<Dictionary<string, string>>();

        using var reader = new StreamReader(stream);
        var firstLine = reader.ReadLine();
        if (firstLine == null) return results;

        // 🔍 Detect delimiter: Use semicolon (;) if found, otherwise fallback to comma (,)
        char delimiter = firstLine.Contains(";") ? ';' : ',';
        var headers = firstLine.Split(delimiter);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line == null) continue;

            var values = line.Split(delimiter);
            var row = new Dictionary<string, string>();

            for (int i = 0; i < headers.Length; i++)
            {
                string cellValue = (i < values.Length) ? values[i].Trim() : "";

                // 🛠 Fix: Remove leading/trailing quotes if present
                if (cellValue.StartsWith("\"") && cellValue.EndsWith("\""))
                {
                    cellValue = cellValue.Substring(1, cellValue.Length - 2);
                }

                row[headers[i]] = cellValue;
            }

            results.Add(row);
        }

        return results;
    }


    public static List<Dictionary<string, string>> ProcessExcel(Stream stream)
    {
        var results = new List<Dictionary<string, string>>();

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        using var reader = ExcelReaderFactory.CreateReader(stream);

        var headerRow = new List<string>();

        while (reader.Read())
        {
            if (headerRow.Count == 0)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    headerRow.Add(reader.GetValue(i)?.ToString() ?? $"Column{i}");
                }
            }
            else
            {
                var row = new Dictionary<string, string>();
                for (int i = 0; i < headerRow.Count; i++)
                {
                    object value = reader.GetValue(i);
                    string cellValue = value?.ToString()?.Trim() ?? "";

                    // ✅ Fix: Convert Excel Dates Correctly
                    if (value is DateTime dt)
                    {
                        // If Excel added "1899-12-31" to a time, remove the date
                        if (dt.Year == 1899 && dt.Month == 12 && dt.Day == 31)
                        {
                            cellValue = dt.ToString("HH:mm:ss"); // Keep only time
                        }
                        // If it's a full date with no explicit time, remove "00:00:00"
                        else if (dt.TimeOfDay == TimeSpan.Zero)
                        {
                            cellValue = dt.ToString("yyyy-MM-dd"); // Keep only date
                        }
                    }

                    row[headerRow[i]] = cellValue;
                }
                results.Add(row);
            }
        }

        return results;
    }
}
