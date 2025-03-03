"use client";
import React, { createContext, useState, useEffect } from "react";

interface ImportContextType {
  selectedSystem: string | null;
  setSelectedSystem: (system: string | null) => void;
  fileName: string | null;
  setFileName: (name: string | null) => void;
}

const ImportContext = createContext<ImportContextType>({
  selectedSystem: null,
  setSelectedSystem: () => {},
  fileName: null,
  setFileName: () => {},
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

  return (
    <ImportContext.Provider value={{ selectedSystem, setSelectedSystem, fileName, setFileName }}>
      {children}
    </ImportContext.Provider>
  );
}

export default ImportContext;
