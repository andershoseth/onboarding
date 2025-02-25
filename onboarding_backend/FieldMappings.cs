namespace onboarding_backend
{
    public class FieldMappings
    {
        public static readonly Dictionary<string, string[]> Mappings = new Dictionary<string, string[]>
    {
        { "Email", new[] { "Email", "Epost", "ContactEmail", "Mail" } },
        { "Phone", new[] { "Phone", "Telephone", "Mobil", "MobilePhone" } },
        { "FirstName", new[] { "FirstName", "Fornavn" } },
        { "LastName", new[] { "LastName", "Etternavn" } },
        { "OrganizationNo", new[] { "OrganizationNo", "OrgNumber", "CompanyID" } },
        { "ContactName", new[] { "ContactName", "Name", "FullName" } },
            
        // Legg til flere felt etter behov
    };
    }
}
