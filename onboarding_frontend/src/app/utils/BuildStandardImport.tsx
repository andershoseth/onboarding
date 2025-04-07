// buildStandardImport.ts
export function buildStandardImport(
    groupedRows: { [groupKey: string]: any[] },
    mapping: { [columnName: string]: string }
  ) {
   
    const standardImport: Record<string, any[]> = {};
  
  
    for (const groupKey of Object.keys(groupedRows)) {
      const rows = groupedRows[groupKey];
  
  
      for (const row of rows) {
    
        const tableObjects: Record<string, any> = {};
  
     
        for (const colName of Object.keys(row)) {
          const cellValue = row[colName];
 
          const mappedField = mapping[colName];
          if (!mappedField) continue; 
  
          const [tableName, fieldName] = mappedField.split(".");
          if (!tableObjects[tableName]) {
            tableObjects[tableName] = {};
          }
          tableObjects[tableName][fieldName] = cellValue;
        }
  
        for (const tName of Object.keys(tableObjects)) {
          if (!standardImport[tName]) {
            standardImport[tName] = [];
          }
          standardImport[tName].push(tableObjects[tName]);
        }
      }
    }
  
    return standardImport;
  }