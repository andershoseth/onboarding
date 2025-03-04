using System.Collections.Generic;
using System.Xml.Linq;
using onboarding_backend.Models;
using onboarding_backend.Models.StandardImport;

namespace onboarding_backend.Services
{
    public class FieldMappingHelper
    {
        public static List<TableFieldMapping> GetStandardImportGroupedFields()
        {
            List<TableFieldMapping> groupedMappings = new();

            // Hent typen for Standardimport (merk: her bruker vi "Standardimport" slik det er definert)
            Type standardImportType = typeof(Standardimport);
            var properties = standardImportType.GetProperties();

            foreach (var prop in properties)
            {
                // Sjekk at property er en List<>
                if (prop.PropertyType.IsGenericType &&
                    prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // Hent elementtypen for listen
                    Type elementType = prop.PropertyType.GetGenericArguments()[0];

                    // Hent alle subegenskaper for elementtypen
                    var subProperties = elementType.GetProperties();
                    List<StandardImportField> fields = new();

                    foreach (var subProp in subProperties)
                    {
                        // Legg til hvert felt som et objekt med property "Field"
                        fields.Add(new StandardImportField { Field = subProp.Name });
                    }

                    groupedMappings.Add(new TableFieldMapping
                    {
                        TableName = prop.Name,
                        Fields = fields
                    });
                }
            }

            return groupedMappings;
        }

    }

}

