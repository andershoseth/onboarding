using System.Collections.Generic;
using System.Xml.Linq;
namespace onboarding_backend.Services;


public class SafTFlattener
{
    public static Dictionary<string, string> FlattenSafT(string filePath)
    {
        var doc = XDocument.Load(filePath);
        var result = new Dictionary<string, string>();
        
        if (doc.Root != null)
        {
            FlattenElement(doc.Root, "", result);
        }

        return result;
    }

    private static void FlattenElement(XElement element, string parentPath, Dictionary<string, string> result)
    {
        // Build a dot-path like AuditFile.MasterFiles.GeneralLedgerAccounts.Account
        string currentPath = string.IsNullOrEmpty(parentPath)
            ? element.Name.LocalName
            : $"{parentPath}.{element.Name.LocalName}";

        // If the element has text content (and not just children)
        // (You might want to check element.HasElements if you only want leaf text.)
        if (!string.IsNullOrWhiteSpace(element.Value) && !element.HasElements)
        {
            result[currentPath] = element.Value;
        }

        // Also record attributes (if any)
        foreach (var attr in element.Attributes())
        {
            var attrPath = $"{currentPath}@{attr.Name.LocalName}";
            result[attrPath] = attr.Value;
        }

        // Recurse through child elements
        foreach (var child in element.Elements())
        {
            FlattenElement(child, currentPath, result);
        }
    }
}
