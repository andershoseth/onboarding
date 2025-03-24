"use client";
import React, { useState, useEffect } from "react";
import { useUploadContext } from "../components/UploadContext";
import Link from "next/link";
export default function TestPage() {
    const { uploadedFiles } = useUploadContext();

    const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]);
    const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

    useEffect(() => {
        // Read user’s selected checkboxes from localStorage
        const savedCheckBoxes = localStorage.getItem("checkboxState");
        if (savedCheckBoxes) {
            const parsedCheckBoxes = JSON.parse(savedCheckBoxes);
            const selectedLabels = Object.entries(parsedCheckBoxes)
                .filter(([_, value]) => value === true)
                .map(([key]) => key);
            setCheckedBoxes(selectedLabels);
        }
    }, []);

    const fileKeys = Object.keys(uploadedFiles);

    return (
        <div style={{ padding: "1rem" }}>
            <h1>Test Page: Check Uploaded Data by Subject</h1>

            {/* List out which subjects the user selected in importvelger */}
            {checkedBoxes.length === 0 ? (
                <p>No subjects selected in importvelger (or none found in localStorage).</p>
            ) : (
                <p>Subjects you selected: {checkedBoxes.join(", ")}</p>
            )}

            {/* Buttons for each *checked* subject */}
            <div style={{ margin: "1rem 0" }}>
                {checkedBoxes.map((subject) => (
                    <button
                        key={subject}
                        onClick={() => setSelectedSubject(subject)}
                        style={{ marginRight: "0.5rem" }}
                    >
                        {subject}
                    </button>
                ))}

                {/* SAF-T button — we unconditionally show it, but you could conditionally show it if you prefer */}
                <button
                    onClick={() => setSelectedSubject("safTExport")}
                    style={{ marginRight: "0.5rem" }}
                >
                    SAF-T
                </button>
                
            </div>
            {/* Link for å navigere til en egen side for SAF-T data */}
      <div style={{ marginBottom: "1rem" }}>
      <Link 
  href="/SaftTable" 
  style={{ color: "blue", textDecoration: "underline" }}
>
  Gå til SAF-T Page
</Link>
      </div>
            {/* If a subject is selected, show the uploaded data if it exists */}
            {selectedSubject && (
                <div style={{ marginTop: "2rem" }}>
                    <h2>Data for: {selectedSubject}</h2>
                    {uploadedFiles[selectedSubject] ? (
                        <>
                            <p>
                                File Name: <strong>{uploadedFiles[selectedSubject].fileName}</strong>
                            </p>
                            <pre>{JSON.stringify(uploadedFiles[selectedSubject].data, null, 2)}</pre>
                        </>
                    ) : (
                        <p style={{ color: "gray" }}>
                            No data uploaded yet for "{selectedSubject}"
                        </p>
                    )}
                </div>
            )}
        </div>
    );


}
