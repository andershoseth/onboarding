using System.Text;
using System.Globalization;
using ExcelDataReader;

public static class FileProcessor
{
    public static List<Dictionary<string, string>> ProcessCsv(Stream stream)
    {
        var results = new List<Dictionary<string, string>>();

        using var reader = new StreamReader(stream);
        var headers = reader.ReadLine()?.Split(',');

        if (headers == null) return results;

        while (!reader.EndOfStream)
        {
            var values = reader.ReadLine()?.Split(',');
            if (values != null)
            {
                var row = new Dictionary<string, string>();
                for (int i = 0; i < headers.Length; i++)
                {
                    row[headers[i]] = values[i];
                }
                results.Add(row);
            }
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
                    row[headerRow[i]] = reader.GetValue(i)?.ToString() ?? "";
                }
                results.Add(row);
            }
        }

        return results;
    }
}
