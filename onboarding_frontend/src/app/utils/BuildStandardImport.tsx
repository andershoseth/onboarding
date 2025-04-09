export function buildStandardImport(
  flatData: any,
  mapping: { [columnName: string]: string }
): Record<string, any[]> {
 
  const flatRows: any[] = Array.isArray(flatData)
    ? flatData
    : (flatData.Transactions || []);
  console.log("Input flatRows:", JSON.stringify(flatRows, null, 2));
  console.log("Input mapping:", JSON.stringify(mapping, null, 2));

  const vouchers: any[] = [];
  let currentVoucher: any = null;


  for (const row of flatRows) {
    const rowKey: string = row.rowKey || "";
    const isHeaderRow = !rowKey.includes(".Line[");

    if (isHeaderRow) {

      currentVoucher = { Lines: [] };

  
      for (const colName in row) {
        const mapStr = mapping[colName];
        if (!mapStr) continue;
        const parts = mapStr.split(".");
       
        if (parts[0] === "Voucher" && (!parts[1] || parts[1] !== "Lines")) {
          assignNestedValue(currentVoucher, parts.slice(1), row[colName]);
        }
      }
      vouchers.push(currentVoucher);
      console.log("Opprettet voucher:", JSON.stringify(currentVoucher, null, 2));
    } else {

      if (!currentVoucher) {
        console.warn("Voucher line rad uten tilknyttet header:", row);
        continue;
      }
      const voucherLine: any = {};
      let hasLineData = false;
      for (const colName in row) {
        const mapStr = mapping[colName];
        if (!mapStr) continue;
        const parts = mapStr.split(".");
      
        if (parts[0] === "Voucher" && parts[1] === "Lines") {
    
          assignNestedValue(voucherLine, parts.slice(2), row[colName]);
          hasLineData = true;
        }
      }
      if (hasLineData) {
        currentVoucher.Lines.push(voucherLine);
        console.log("Lagt til voucherLine:", JSON.stringify(voucherLine, null, 2));
      }
    }
  }

  const standardImport = { Voucher: vouchers };
  console.log("Constructed StandardImport:", JSON.stringify(standardImport, null, 2));
  return standardImport;
}

/**
 * Rekursiv hjelpefunksjon for å tilordne en verdi til et objekt basert på en liste med nøkler.
 *
 */
function assignNestedValue(obj: any, keys: string[], value: any) {
  if (keys.length === 0) return;
  if (keys.length === 1) {
    obj[keys[0]] = value;
  } else {
    const key = keys[0];
    if (!obj[key]) {
      obj[key] = {};
    }
    assignNestedValue(obj[key], keys.slice(1), value);
  }
}