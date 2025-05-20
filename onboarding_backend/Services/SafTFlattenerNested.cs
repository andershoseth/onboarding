using System.Xml.Linq;

namespace onboarding_backend.Services;

public static class SafTNestedFlattener
    {

        // Loads the SAF-T XML, then returns a nested Dictionary-of-Dictionaries
        // structure. Repeated sibling elements become a List<object>.
        // Attributes are stored with an @ prefix (e.g. "@id").
        // Leaf text nodes are stored under "#text" by default.

        public static Dictionary<string, object> FlattenSafTAsNested(string filePath)
        {
            var doc = XDocument.Load(filePath);

            // If there's no root, return an empty dictionary.
            if (doc.Root == null)
                return new Dictionary<string, object>();


            var result = new Dictionary<string, object>
            {
                [doc.Root.Name.LocalName] = ElementToNestedStructure(doc.Root)
            };

            return result;
        }


        // Recursively converts an XElement into a nested C# structure:
        //  - A Dictionary for a single element's children
        //   - A List if there are multiple siblings with the same name
        //   - String for leaf text
        //   - Special keys for attributes and text

        private static object ElementToNestedStructure(XElement element)
        {
            // This will store all data for 'element' in a dictionary
            var nodeObj = new Dictionary<string, object>();

            // 1. Put attributes in the dictionary
            foreach (var attr in element.Attributes())
            {
                // We store attributes with an '@' prefix for clarity
                nodeObj["@" + attr.Name.LocalName] = attr.Value;
            }

            // 2. Check if element has child elements
            if (element.HasElements)
            {
                // Group children by their name to handle repeated elements
                var groupedChildren = element.Elements()
                                             .GroupBy(e => e.Name.LocalName);

                foreach (var group in groupedChildren)
                {
                    var childName = group.Key;
                    var sameNameChildren = group.ToList();

                    if (sameNameChildren.Count == 1)
                    {
                        // Only one child with this name => store a single object
                        nodeObj[childName] = ElementToNestedStructure(sameNameChildren[0]);
                    }
                    else
                    {
                        // Multiple siblings => store them as a list
                        var listOfChildren = new List<object>();
                        foreach (var child in sameNameChildren)
                        {
                            listOfChildren.Add(ElementToNestedStructure(child));
                        }

                        nodeObj[childName] = listOfChildren;
                    }
                }
            }
            else
            {
                // 3. If no child elements, store the text content (if any) as "#text"
                nodeObj[""] = element.Value ?? "";
            }

            return nodeObj;
        }
    }
