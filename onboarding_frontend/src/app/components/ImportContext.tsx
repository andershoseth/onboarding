"use client";
import React, { createContext, useState, useEffect } from "react";
import { systemCoverage } from "./systemCoverage";

interface ImportContextType {
  selectedSystem: string | null;
  setSelectedSystem: (system: string | null) => void;
  selectedFileType: string | null;
  setSelectedFileType: (filetype: string | null) => void;
  fileName: string[];
  setFileName: React.Dispatch<React.SetStateAction<string[]>>;
  removeFileName: (nameToRemove: string) => void;
  selectedColumns: { [key: string]: boolean };
  setSelectedColumns: (columns: { [key: string]: boolean } | ((prev: { [key: string]: boolean }) => { [key: string]: boolean })) => void;
  mappingCompleted: boolean;
  setMappingCompleted: (completed: boolean) => void;
}

const ImportContext = createContext<ImportContextType>({
  selectedSystem: null,
  setSelectedSystem: () => { },
  selectedFileType: null,
  setSelectedFileType: () => { },
  fileName: [],
  setFileName: () => { },
  removeFileName: () => { },
  selectedColumns: {},
  setSelectedColumns: () => { },
  mappingCompleted: false,
  setMappingCompleted: () => { }
});

export function ImportProvider({ children }: { children: React.ReactNode }) {
  const [selectedSystem, setSelectedSystem] = useState<string | null>(null);
  const [selectedFileType, setSelectedFileType] = useState<string | null>(null);
  const [selectedColumns, setSelectedColumns] = useState<{ [key: string]: boolean }>({})
  const [mappingCompleted, setMappingCompleted] = useState<boolean>(false)

  // 1) Start off with null (or empty)
  const [fileName, setFileName] = useState<string[]>([]);


  const removeFileName = (nameToRemove: string) => {
    setFileName((prev) => prev.filter((name) => name !== nameToRemove));
  };

  const updateSelectedColumns = (columns: { [key: string]: boolean } | ((prev: { [key: string]: boolean }) => { [key: string]: boolean })) => {
    setSelectedColumns(columns);
  };

  useEffect(() => {
    if (typeof window !== "undefined") {
      localStorage.setItem("checkboxState", JSON.stringify(selectedColumns));
    }
  }, [selectedColumns]); //local storage for checked boxes so that export-page works

  useEffect(() => { //dette er for importvelgeren (bruker systemCoverage.tsx)

    if (selectedSystem && selectedFileType) {

      const fileTypeMap: Record<string, string> = {
        "SAF-T (.xml)": "safTSubjects",
        "CSV (.csv)": "csvSubjects",
        "Excel (.xlsx)": "csvSubjects" //fjern senere?? Jeg er usikker -Linn
      };

      const correctedFileType = fileTypeMap[selectedFileType] || selectedFileType;

      const safTSubjects = systemCoverage[selectedSystem]?.safTSubjects || [];
      const csvSubjects = systemCoverage[selectedSystem]?.csvSubjects || [];

      const combinedSubjects = correctedFileType === "safTSubjects" //sort the new list in alphabetical order
        ? Array.from(new Set([...safTSubjects, ...csvSubjects])).sort((a, b) => a.localeCompare(b))
        : csvSubjects.sort((a, b) => a.localeCompare(b));

      if (!combinedSubjects.length) {
        console.warn("No subjects found for selected system and file type!");
        return;
      }

      const initialCheckBoxState = combinedSubjects.reduce((acc, subject) => {
        acc[subject] = false;
        return acc;
      }, {} as { [key: string]: boolean });

      setSelectedColumns(initialCheckBoxState);
    }
  }, [selectedSystem, selectedFileType]);


  return (
    <ImportContext.Provider
      value={{
        selectedSystem,
        setSelectedSystem,
        selectedFileType,
        setSelectedFileType,
        fileName,
        setFileName,
        removeFileName,
        selectedColumns,
        setSelectedColumns: updateSelectedColumns,
        mappingCompleted,
        setMappingCompleted
      }}
    >
      {children}
    </ImportContext.Provider>
  );
}

export default ImportContext;
