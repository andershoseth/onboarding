
"use client";
import React, { createContext, useState, useContext, ReactNode } from "react";

export interface Mapping {
  [columnName: string]: string;
}

interface MappingContextType {
  mapping: Mapping;
  setMapping: React.Dispatch<React.SetStateAction<Mapping>>;
}

const MappingContext = createContext<MappingContextType | null>(null);

export function MappingProvider({ children }: { children: React.ReactNode }) {
  const [mapping, setMapping] = useState<Mapping>({});

  return (
    <MappingContext.Provider value={{ mapping, setMapping }}>
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
