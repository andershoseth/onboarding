using System.Xml.Linq;
using onboarding_backend.Models;
namespace onboarding_backend.Services;

public static class Converter
{
    public static List<Contact> ExtractCustomers(XDocument saftXml)
    {
        var customers = saftXml.Root?
        .Element(XName.Get("Masterfiles", saftXml.Root.Name.NamespaceName))?
        .Element(XName.Get("Customers", saftXml.Root.Name.NamespaceName));

        return customers?.Elements(XName.Get("Customer", saftXml.Root.Name.NamespaceName))
        .Select(c => new Contact
        {
            CustomerNo = c.Element(XName.Get("CustomerID", saftXml.Root.Name.NamespaceName))?.Value ?? string.Empty,
            ContactName = c.Element(XName.Get("Name", saftXml.Root.Name.NamespaceName))?.Value ?? string.Empty,
            Phone = c.Element(XName.Get("Contact", saftXml.Root.Name.NamespaceName))?
                        .Element(XName.Get("Telephone", saftXml.Root.Name.NamespaceName))?.Value ?? string.Empty,
            Email = c.Element(XName.Get("Contact", saftXml.Root.Name.NamespaceName))?
                        .Element(XName.Get("Email", saftXml.Root.Name.NamespaceName))?.Value ?? string.Empty,
            OrganizationNo = c.Element(XName.Get("RegistrationNumber", saftXml.Root.Name.NamespaceName))?.Value ?? string.Empty
        }).ToList() ?? new List<Contact>();
    }
    public static List<Contact> ExtractSuppliers(XDocument saftXml)
    {
        var suppliers = saftXml.Root?
            .Element(XName.Get("MasterFiles", saftXml.Root.Name.NamespaceName))?
            .Element(XName.Get("Suppliers", saftXml.Root.Name.NamespaceName));

        return suppliers?.Elements(XName.Get("Supplier", saftXml.Root.Name.NamespaceName))
            .Select(s => new Contact
            {
                CustomerNo = s.Element(XName.Get("SupplierID", saftXml.Root.Name.NamespaceName))?.Value ?? string.Empty,
                ContactName = s.Element(XName.Get("Name", saftXml.Root.Name.NamespaceName))?.Value ?? string.Empty,
                Phone = s.Element(XName.Get("Contact", saftXml.Root.Name.NamespaceName))?
                            .Element(XName.Get("Telephone", saftXml.Root.Name.NamespaceName))?.Value ?? string.Empty,
                Email = s.Element(XName.Get("Contact", saftXml.Root.Name.NamespaceName))?
                            .Element(XName.Get("Email", saftXml.Root.Name.NamespaceName))?.Value ?? string.Empty,
                OrganizationNo = s.Element(XName.Get("RegistrationNumber", saftXml.Root.Name.NamespaceName))?.Value ?? string.Empty
            }).ToList() ?? new List<Contact>();
    }
}
