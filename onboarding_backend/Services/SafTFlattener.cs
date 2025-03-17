using System.Collections.Generic;
using System.Xml.Linq;
using onboarding_backend.Models;
namespace onboarding_backend.Services;


    public class FlattenedEntry
    {
        public string Path { get; set; }
        public string Value { get; set; }
    }

    public static class SafTFlattener
    {
   
    public static List<FlattenedEntry> FlattenSafTAsList(XElement rootElement)
    {
        var results = new List<FlattenedEntry>();

        if (rootElement != null)
        {
            FlattenElementToList(rootElement, "", results);
        }

        return results;
    }
    public static List<GroupedSafTEntries> GroupSafTEntries(List<FlattenedEntry> flattenedEntries)
    {
        var groups = flattenedEntries
            .GroupBy(e =>
            {
                var segments = e.Path.Split('.');
                return segments.Length >= 2 ? $"{segments[0]}.{segments[1]}" : e.Path;
            })
            .Select(g => new GroupedSafTEntries
            {
                GroupKey = g.Key,
                Entries = g.ToList()
            })
            .ToList();

        return groups;
    }
  


    /// <summary>
    /// Flattens a SAF-T XML file and returns a Dictionary of (Path -> Value).
    /// Includes [index] for repeated sibling elements to avoid collisions.
    /// </summary>
    public static Dictionary<string, string> FlattenSafTAsDictionary(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var dict = new Dictionary<string, string>();

            if (doc.Root != null)
            {
                FlattenElementToDictionary(doc.Root, parentPath: "", dict);
            }

            return dict;
        }

        #region Private List-based Implementation

        private static void FlattenElementToList(
            XElement element,
            string parentPath,
            List<FlattenedEntry> results,
            int siblingIndex = -1)
        {
            // Build this element's name with optional sibling index (e.g. "Supplier[2]")
            var baseName = siblingIndex >= 0
                ? $"{element.Name.LocalName}[{siblingIndex}]"
                : element.Name.LocalName;

            // If there's a parent path, append; otherwise just use baseName
            string currentPath = string.IsNullOrEmpty(parentPath)
                ? baseName
                : $"{parentPath}.{baseName}";

            // If the element is a leaf (no children) and has text, record it
            if (!element.HasElements && !string.IsNullOrWhiteSpace(element.Value))
            {
                results.Add(new FlattenedEntry
                {
                    Path = currentPath,
                    Value = element.Value
                });
            }

            // Also record attributes
            foreach (var attr in element.Attributes())
            {
                string attrPath = $"{currentPath}@{attr.Name.LocalName}";
                results.Add(new FlattenedEntry
                {
                    Path = attrPath,
                    Value = attr.Value
                });
            }

            // Group children by name, so we can index repeated siblings
            var groupedChildren = element.Elements()
                                         .GroupBy(e => e.Name.LocalName);

            foreach (var group in groupedChildren)
            {
                var sameNameChildren = group.ToList();
                if (sameNameChildren.Count > 1)
                {
                    // For multiple siblings, index each
                    for (int i = 0; i < sameNameChildren.Count; i++)
                    {
                        FlattenElementToList(sameNameChildren[i], currentPath, results, i + 1);
                    }
                }
                else
                {
                    // Only one child, no need for index
                    FlattenElementToList(sameNameChildren[0], currentPath, results);
                }
            }
        }

        #endregion

        #region Private Dictionary-based Implementation

        private static void FlattenElementToDictionary(
            XElement element,
            string parentPath,
            Dictionary<string, string> dict,
            int siblingIndex = -1)
        {
            var baseName = siblingIndex >= 0
                ? $"{element.Name.LocalName}[{siblingIndex}]"
                : element.Name.LocalName;

            string currentPath = string.IsNullOrEmpty(parentPath)
                ? baseName
                : $"{parentPath}.{baseName}";

            if (!element.HasElements && !string.IsNullOrWhiteSpace(element.Value))
            {
                dict[currentPath] = element.Value;
            }

            // Attributes
            foreach (var attr in element.Attributes())
            {
                string attrPath = $"{currentPath}@{attr.Name.LocalName}";
                dict[attrPath] = attr.Value;
            }

            // Group child elements by name to handle repeats
            var groupedChildren = element.Elements()
                .GroupBy(e => e.Name.LocalName);

            foreach (var group in groupedChildren)
            {
                var sameNameChildren = group.ToList();
                if (sameNameChildren.Count > 1)
                {
                    for (int i = 0; i < sameNameChildren.Count; i++)
                    {
                        FlattenElementToDictionary(sameNameChildren[i], currentPath, dict, i + 1);
                    }
                }
                else
                {
                    FlattenElementToDictionary(sameNameChildren[0], currentPath, dict);
                }
            }
        }

        #endregion
    }