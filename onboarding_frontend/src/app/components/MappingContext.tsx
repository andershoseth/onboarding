
"use client";
import React, { createContext, useState, useContext, ReactNode } from "react";

export interface Mapping {
  [columnName: string]: string;
}


export interface RowData {
  [fieldName: string]: any;
}


export interface GroupedRowsDict {
  [groupKey: string]: RowData[];
}

interface MappingContextType {

  mapping: Mapping;
  setMapping: React.Dispatch<React.SetStateAction<Mapping>>;

  // groupKey → array of pivoted row objects
  groupedRows: GroupedRowsDict;
  setGroupedRows: React.Dispatch<React.SetStateAction<GroupedRowsDict>>;
}

const MappingContext = createContext<MappingContextType | null>(null);

export function MappingProvider({ children }: { children: ReactNode }) {
  const [mapping, setMapping] = useState<Mapping>({});
  const [groupedRows, setGroupedRows] = useState<GroupedRowsDict>({});

  return (
    <MappingContext.Provider value={{ mapping, setMapping, groupedRows, setGroupedRows }}>
      {children}
    </MappingContext.Provider>
  );
}

export function useMapping() {
  const context = useContext(MappingContext);
  if (!context) {
    throw new Error("useMapping must be used within a MappingProvider");
  }
  return context;
}
