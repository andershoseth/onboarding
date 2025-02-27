export function flattenObject(
    obj: Record<string, unknown>,
    prefix = ""
  ): Record<string, unknown> {
    const result: Record<string, unknown> = {};
  
    for (const [key, value] of Object.entries(obj)) {
    
      const newKey = prefix ? `${prefix}.${key}` : key;
  
      if (
        value &&
        typeof value === "object" &&
        !Array.isArray(value)
      ) {
        // Rekursiv kall for å flate ut videre
        Object.assign(result, flattenObject(value as Record<string, unknown>, newKey));
      } else {
        result[newKey] = value;
      }
    }
  
    return result;
  }