"use client"
import React, { createContext, useState, useEffect } from 'react';

interface ImportContextType {
  selectedSystem: string | null;
  setSelectedSystem: (system: string | null) => void;
}

const ImportContext = createContext<ImportContextType>({
  selectedSystem: null,
  setSelectedSystem: () => {},
});

export function ImportProvider({ children }: { children: React.ReactNode }) {
  const [selectedSystem, setSelectedSystem] = useState<string | null>(() => {
    if (typeof window !== 'undefined') {
      return localStorage.getItem('selectedSystem');
    }
    return null;
  });

  useEffect(() => {
    if (selectedSystem) {
      localStorage.setItem('selectedSystem', selectedSystem);
    } else {
      localStorage.removeItem('selectedSystem');
    }
  }, [selectedSystem]);

  return (
    <ImportContext.Provider value={{ selectedSystem, setSelectedSystem }}>
      {children}
    </ImportContext.Provider>
  );
}

export default ImportContext;
