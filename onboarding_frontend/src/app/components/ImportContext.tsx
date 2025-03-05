"use client";
import React, { createContext, useState, useEffect } from "react";

interface ImportContextType {
  selectedSystem: string | null;
  setSelectedSystem: (system: string | null) => void;
  fileName: string | null;
  setFileName: (name: string | null) => void;
  selectedColumns: { kontakter: boolean; avdeling: boolean; saldobalanse: boolean };
  setSelectedColumns: (columns: { kontakter: boolean; avdeling: boolean; saldobalanse: boolean }) => void;
}

const ImportContext = createContext<ImportContextType>({
  selectedSystem: null,
  setSelectedSystem: () => {},
  fileName: null,
  setFileName: () => {},
  selectedColumns: { kontakter: false, avdeling: false, saldobalanse: false },
  setSelectedColumns: () => {},
});

export function ImportProvider({ children }: { children: React.ReactNode }) {
  const [selectedSystem, setSelectedSystem] = useState<string | null>(() => {
    if (typeof window !== "undefined") {
      return localStorage.getItem("selectedSystem");
    }
    return null;
  });

  const [fileName, setFileName] = useState<string | null>(() => {
    if (typeof window !== "undefined") {
      return localStorage.getItem("uploadedFileName");
    }
    return null;
  });

  const [selectedColumns, setSelectedColumns] = useState<{ kontakter: boolean; avdeling: boolean; saldobalanse: boolean }>({
    kontakter: false,
    avdeling: false,
    saldobalanse: false,
  });

  // Update context and localStorage whenever system or fileName changes
  useEffect(() => {
    if (selectedSystem) {
      localStorage.setItem("selectedSystem", selectedSystem);
    } else {
      localStorage.removeItem("selectedSystem");
    }
  }, [selectedSystem]);

  useEffect(() => {
    if (fileName) {
      localStorage.setItem("uploadedFileName", fileName);
    } else {
      localStorage.removeItem("uploadedFileName");
    }
  }, [fileName]);

  // Update the selectedColumns in localStorage whenever it changes
  useEffect(() => {
    localStorage.setItem("checkboxState", JSON.stringify(selectedColumns));
  }, [selectedColumns]);

  return (
    <ImportContext.Provider value={{ selectedSystem, setSelectedSystem, fileName, setFileName, selectedColumns, setSelectedColumns }}>
      {children}
    </ImportContext.Provider>
  );
}

export default ImportContext;
