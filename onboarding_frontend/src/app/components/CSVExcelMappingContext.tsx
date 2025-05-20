"use client";
import React, { createContext, useState, useContext, ReactNode } from "react";

/**
 * Storing all CSV/Excel mappings as an object:
 * {
 *   [tableName]: { [columnName]: string }
 * }
 * so that each bracketed table has its own mapping dictionary.
 */

export interface CSVExcelMapping {
  [tableName: string]: {
    [columnName: string]: string;
  };
}

interface CSVExcelMappingContextType {
  csvMapping: CSVExcelMapping;
  setCSVMapping: React.Dispatch<React.SetStateAction<CSVExcelMapping>>;
}

const CSVExcelMappingContext = createContext<CSVExcelMappingContextType | null>(null);

export function CSVExcelMappingProvider({ children }: { children: ReactNode }) {
  const [csvMapping, setCSVMapping] = useState<CSVExcelMapping>({});

  return (
    <CSVExcelMappingContext.Provider value={{ csvMapping, setCSVMapping }}>
      {children}
    </CSVExcelMappingContext.Provider>
  );
}

export function useCSVExcelMapping() {
  const context = useContext(CSVExcelMappingContext);
  if (!context) {
    throw new Error("useCSVExcelMapping must be used within a CSVExcelMappingProvider");
  }
  return context;
}
