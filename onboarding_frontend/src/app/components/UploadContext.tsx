"use client";
import React, { createContext, useContext, useState } from "react";

// Riktig type for API-responsen
interface FlattenedEntry {
  path: string;
  value: string;
}

interface UploadContextType {
  uploadedData: FlattenedEntry[] | null;
  setUploadedData: React.Dispatch<React.SetStateAction<FlattenedEntry[] | null>>;
}

const UploadContext = createContext<UploadContextType | null>(null);

export function UploadProvider({ children }: { children: React.ReactNode }) {
  const [uploadedData, setUploadedData] = useState<FlattenedEntry[] | null>(null);

  return (
    <UploadContext.Provider value={{ uploadedData, setUploadedData }}>
      {children}
    </UploadContext.Provider>
  );
}

export function useUploadContext() {
  const context = useContext(UploadContext);
  if (!context) {
    throw new Error("useUploadContext must be used within an UploadProvider");
  }
  return context;
}