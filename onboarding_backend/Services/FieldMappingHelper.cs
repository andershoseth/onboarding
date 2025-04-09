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

               
                Type standardImportType = typeof(Standardimport);
                var properties = standardImportType.GetProperties();

                foreach (var prop in properties)
                {
                 
                    if (prop.PropertyType.IsGenericType &&
                        prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                      
                        Type elementType = prop.PropertyType.GetGenericArguments()[0];
                        List<StandardImportField> fields = new();

                        // Hent alle properties for elementtypen
                        var subProperties = elementType.GetProperties();
                        foreach (var subProp in subProperties)
                        {
                           
                            // hent feltene fra den nested typen.
                            if (subProp.PropertyType.IsGenericType &&
                                subProp.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                
                                Type nestedElementType = subProp.PropertyType.GetGenericArguments()[0];
                                var nestedProperties = nestedElementType.GetProperties();
                                foreach (var nestedProp in nestedProperties)
                                {
                                    // Legg til et felt med navnet "Lines.<nestedPropName>"
                                    fields.Add(new StandardImportField
                                    {
                                        Field = $"{subProp.Name}.{nestedProp.Name}"
                                    });
                                }
                            }
                            else
                            {
                               
                                fields.Add(new StandardImportField { Field = subProp.Name });
                            }
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