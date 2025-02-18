using onboarding_backend.Models;
namespace onboarding_backend.Services;

public static class Converter
{
    public static (List<Customer> Customers, List<Supplier> Suppliers) ConvertFromSaft(AuditFile saft)
    {

        var customers = saft.MasterFiles?.Customers?
        .Select(c => new Customer
        {
            CustomerNo = c.CustomerId ?? string.Empty,
            Name = c.Name ?? string.Empty,
            Phone = c.Contact?.FirstOrDefault()?.Telephone ?? string.Empty,
            Email = c.Contact?.FirstOrDefault()?.Email ?? string.Empty,
            OrganizationNo = c.RegistrationNumber ?? string.Empty
        }).ToList() ?? new List<Customer>();

        var suppliers = saft.MasterFiles?.Suppliers?
        .Select(c => new Supplier
        {
            SupplierNo = c.SupplierId ?? string.Empty,
            Name = c.Name ?? string.Empty,
            Phone = c.Contact?.FirstOrDefault()?.Telephone ?? string.Empty,
            Email = c.Contact?.FirstOrDefault()?.Email ?? string.Empty,
            OrganizationNo = c.RegistrationNumber ?? string.Empty
        }).ToList() ?? new List<Supplier>();

        var contacts = saft.MasterFiles?.Customers?
        .Select(c => new Contact
        {
            CustomerNo = c.CustomerId ?? string.Empty,
            ContactName = c.Name ?? string.Empty,
            Phone = c.Contact?.FirstOrDefault()?.Telephone ?? string.Empty,
            Email = c.Contact?.FirstOrDefault()?.Email ?? string.Empty,
            OrganizationNo = c.RegistrationNumber ?? string.Empty
        }).ToList() ?? new List<Contact>();

        return (customers, suppliers);
    }
}
