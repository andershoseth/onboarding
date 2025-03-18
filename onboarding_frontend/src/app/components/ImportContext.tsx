"use client";
import React, { createContext, useState, useEffect } from "react";
import { systemCoverage } from "../export/systemCoverage";

interface ImportContextType {
  selectedSystem: string | null;
  setSelectedSystem: (system: string | null) => void;
  selectedFileType: string | null;
  setSelectedFileType: (filetype: string | null) => void;
  fileName: string | null;
  setFileName: (name: string | null) => void;
  selectedColumns: { [key: string]: boolean };
  setSelectedColumns: (columns: { [key: string]: boolean } | ((prev: { [key: string]: boolean }) => { [key: string]: boolean })) => void;
}

const ImportContext = createContext<ImportContextType>({
  selectedSystem: null,
  setSelectedSystem: () => { },
  selectedFileType: null,
  setSelectedFileType: () => { },
  fileName: null,
  setFileName: () => { },
  selectedColumns: {},
  setSelectedColumns: () => { },
});

export function ImportProvider({ children }: { children: React.ReactNode }) {
  const [selectedSystem, setSelectedSystem] = useState<string | null>(null);
  const [selectedFileType, setSelectedFileType] = useState<string | null>(null);
  const [selectedColumns, setSelectedColumns] = useState<{ [key: string]: boolean }>({})

  // 1) Start off with null (or empty)
  const [fileName, setFileName] = useState<string | null>(null);

  // 2) In a useEffect, read sessionStorage on the client
  useEffect(() => {
    if (typeof window !== "undefined") {
      const storedFileName = sessionStorage.getItem("fileName");
      if (storedFileName) {
        setFileName(storedFileName);
      }
    }
  }, []);

  // 3) Whenever fileName changes on the client, store it
  useEffect(() => {
    if (typeof window !== "undefined" && fileName) {
      sessionStorage.setItem("fileName", fileName);
    }
  }, [fileName]);

  const updateSelectedColumns = (columns: { [key: string]: boolean } | ((prev: { [key: string]: boolean }) => { [key: string]: boolean })) => {
    setSelectedColumns(columns);
  };

  useEffect(() => {

    if (selectedSystem && selectedFileType) {

      const fileTypeMap: Record<string, string> = {
        "SAF-T (.xml)": "safTSubjects",
        "CSV (.csv)": "csvSubjects",
        "Excel (.xlsx)": "excelSubjects"
      };

      const correctedFileType = fileTypeMap[selectedFileType] || selectedFileType;
      const availableSubjects = systemCoverage[selectedSystem]?.[correctedFileType];

      if (!availableSubjects || availableSubjects.length === 0) {
        console.warn("No subjects found for selected system and file type!");
        return;
      }

      const initialCheckBoxState = availableSubjects.reduce((acc, subject) => {
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
        selectedColumns,
        setSelectedColumns: updateSelectedColumns,
      }}
    >
      {children}
    </ImportContext.Provider>
  );
}

export default ImportContext;
