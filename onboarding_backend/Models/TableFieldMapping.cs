namespace onboarding_backend.Models
{
    public class TableFieldMapping
    {
        public string TableName { get; set; }
        public List<StandardImportField> Fields { get; set; }
    }
}