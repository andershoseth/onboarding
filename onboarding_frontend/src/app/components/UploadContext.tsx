"use client";
import React, { createContext, useContext, useState } from "react";

// Riktig type for API-responsen
interface FlattenedEntry {
  path: string;
  value: string;
}

// For multiple files, define an interface describing the data you store per subject
interface UploadedFileEntry {
  fileName: string;
  data: FlattenedEntry[];   // or 'any' if it’s an array of objects, etc.
}

interface UploadContextType {
  uploadedData: FlattenedEntry[] | null; // single-file approach if you still want it
  setUploadedData: React.Dispatch<React.SetStateAction<FlattenedEntry[] | null>>;

  uploadProgress: number;
  setUploadProgress: React.Dispatch<React.SetStateAction<number>>;

  // New multi-file approach:
  uploadedFiles: Record<string, UploadedFileEntry>;
  setUploadedFiles: React.Dispatch<
    React.SetStateAction<Record<string, UploadedFileEntry>>
  >;
}

const UploadContext = createContext<UploadContextType | null>(null);

export function UploadProvider({ children }: { children: React.ReactNode }) {
  const [uploadedData, setUploadedData] = useState<FlattenedEntry[] | null>(null);
  const [uploadProgress, setUploadProgress] = useState(0);

  // Here’s the new multi-file dictionary:
  const [uploadedFiles, setUploadedFiles] = useState<Record<string, UploadedFileEntry>>({});

  return (
    <UploadContext.Provider
      value={{
        uploadedData,
        setUploadedData,
        uploadProgress,
        setUploadProgress,
        uploadedFiles,
        setUploadedFiles,
      }}
    >
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
