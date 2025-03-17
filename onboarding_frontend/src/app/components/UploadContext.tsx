// UploadContext.tsx (example)
"use client";
import React, { createContext, useContext, useState } from "react";

type UploadedFileData = {
  fileName: string;
  data: unknown;  // This will hold whatever .NET returns (object, array, etc.)
};

type UploadContextValue = {
  // Old single-file data (optional to keep):
  uploadedData: unknown;
  setUploadedData: (data: unknown) => void;

  // The new dictionary keyed by subject:
  uploadedFiles: Record<string, UploadedFileData>;
  setUploadedFiles: React.Dispatch<React.SetStateAction<Record<string, UploadedFileData>>>;

  // Progress:
  uploadProgress: number;
  setUploadProgress: (n: number) => void;
};

const UploadContext = createContext<UploadContextValue>(null!);

export function UploadProvider({ children }: { children: React.ReactNode }) {
  const [uploadedData, setUploadedData] = useState<any>(null);

  // New dictionary for multiple uploaded files by subject:
  const [uploadedFiles, setUploadedFiles] = useState<Record<string, UploadedFileData>>({});

  const [uploadProgress, setUploadProgress] = useState<number>(0);

  return (
      <UploadContext.Provider
          value={{
            uploadedData,
            setUploadedData,
            uploadedFiles,
            setUploadedFiles,
            uploadProgress,
            setUploadProgress
          }}
      >
        {children}
      </UploadContext.Provider>
  );
}

export const useUploadContext = () => useContext(UploadContext);
