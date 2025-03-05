
"use client";
import React, { createContext, useState, useContext, ReactNode } from "react";

interface MappingRow {
  field: string;
  mapping: string;
}

interface Mapping {
  [tableName: string]: MappingRow[];
}

interface MappingContextType {
  mappingData: Mapping;
  setMappingData: React.Dispatch<React.SetStateAction<Mapping>>;
}

const MappingContext = createContext<MappingContextType | undefined>(undefined);

export const MappingProvider = ({ children }: { children: ReactNode }) => {
  const [mappingData, setMappingData] = useState<Mapping>({});
  return (
    <MappingContext.Provider value={{ mappingData, setMappingData }}>
      {children}
    </MappingContext.Provider>
  );
};

export const useMapping = () => {
  const context = useContext(MappingContext);
  if (!context) {
    throw new Error("useMapping må brukes innenfor MappingProvider");
  }
  return context;
};
