"use client";
import React, { createContext, useState, useEffect } from "react";

interface ImportContextType {
  selectedSystem: string | null;
  setSelectedSystem: (system: string | null) => void;
  selectedFileType: string | null;
  setSelectedFileType: (filetype: string | null) => void;
  fileName: string | null;
  setFileName: (name: string | null) => void;
  selectedColumns: { kontakter: boolean; avdeling: boolean; saldobalanse: boolean };
  setSelectedColumns: (columns: { kontakter: boolean; avdeling: boolean; saldobalanse: boolean }) => void;
}

const ImportContext = createContext<ImportContextType>({
  selectedSystem: null,
  setSelectedSystem: () => { },
  selectedFileType: null,
  setSelectedFileType: () => { },
  fileName: null,
  setFileName: () => { },
  selectedColumns: { kontakter: false, avdeling: false, saldobalanse: false },
  setSelectedColumns: () => { },
});

export function ImportProvider({ children }: { children: React.ReactNode }) {
  const [selectedSystem, setSelectedSystem] = useState<string | null>(null);
  const [selectedFileType, setSelectedFileType] = useState<string | null>(null);
  const [fileName, setFileName] = useState<string | null>(null);


  const [selectedColumns, setSelectedColumns] = useState<{ kontakter: boolean; avdeling: boolean; saldobalanse: boolean }>({
    kontakter: false,
    avdeling: false,
    saldobalanse: false,
  });

  return (
    <ImportContext.Provider value={{ selectedSystem, setSelectedSystem, selectedFileType, setSelectedFileType, fileName, setFileName, selectedColumns, setSelectedColumns }}>
      {children}
    </ImportContext.Provider>
  );
}

export default ImportContext;
