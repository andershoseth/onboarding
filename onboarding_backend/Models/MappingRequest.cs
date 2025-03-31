public class MappingRequest
{
    public Dictionary<string, string> Mapping { get; set; } = new();
    public List<Dictionary<string, string>> Data { get; set; } = new();
}