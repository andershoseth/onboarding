"use client";
import React, { createContext, useContext, useState } from "react";

// interface for the flattened data returned fomr the server
interface FlattenedEntry {
    path: string;
    value: string;
}

interface UploadedFileEntry {
    fileName: string;
    data: FlattenedEntry[];
}

interface UploadContextType {
    uploadedData: FlattenedEntry[] | null;
    setUploadedData: React.Dispatch<React.SetStateAction<FlattenedEntry[] | null>>;
    uploadedFiles: Record<string, UploadedFileEntry>;
    setUploadedFiles: React.Dispatch<React.SetStateAction<Record<string, UploadedFileEntry>>>;

}

const UploadContext = createContext<UploadContextType | null>(null);

export function UploadProvider({ children }: { children: React.ReactNode }) {
    const [uploadedData, setUploadedData] = useState<FlattenedEntry[] | null>(null);
    const [uploadedFiles, setUploadedFiles] = useState<Record<string, UploadedFileEntry>>({});

    return (
        <UploadContext.Provider
            value={{
                uploadedData,
                setUploadedData,
                uploadedFiles,
                setUploadedFiles
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
