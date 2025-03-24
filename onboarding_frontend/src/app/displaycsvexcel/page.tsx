"use client";

import React, { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import { useReactTable, getCoreRowModel, ColumnDef } from "@tanstack/react-table";
import { useUploadContext } from "../components/UploadContext";
import Link from "next/link";

export default function FileDisplayPage() {
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

    return (
        <div className="p-6 min-h-screen">
            <h2 className="text-2xl font-bold mb-4">Uploaded File Data</h2>
            <p className="mb-4">Subjects you selected: {checkedBoxes.length > 0 ? checkedBoxes.join(", ") : "No subjects selected."}</p>

            {/* Buttons for each checked subject */}
            <div className="flex flex-wrap gap-4 mb-6">
                {checkedBoxes.map((subject) => (
                    <button
                        key={subject}
                        onClick={() => setSelectedSubject(subject)}
                        className="bg-blue-500 text-white px-4 py-2 rounded shadow hover:bg-blue-600"
                    >
                        {subject}
                    </button>
                ))}
                <button
                    onClick={() => setSelectedSubject("safTExport")}
                    className="bg-green-500 text-white px-4 py-2 rounded shadow hover:bg-green-600"
                >
                    SAF-T
                </button>
                <Link
                    className="px-4 py-2 rounded-md shadow-md transition bg-[#E17044] text-white hover:bg-[#c85b34]"
                    href="/success"
                >
                    Next
                </Link>
            </div>

            {/* If a subject is selected, show the uploaded data if it exists */}
            {selectedSubject && (
                <div className="mt-6">
                    <h2 className="text-xl font-semibold mb-2">Data for: {selectedSubject}</h2>
                    {uploadedFiles[selectedSubject] ? (
                        <>
                            <p className="mb-2">File Name: <strong>{uploadedFiles[selectedSubject].fileName}</strong></p>
                            <div className="overflow-x-auto border border-gray-500 rounded-lg shadow-md bg-white">
                                <table className="min-w-full">
                                <thead className="bg-gray-600 text-white">
                                    <tr>
                                        {Object.keys(uploadedFiles[selectedSubject]?.data?.[0] || {}).map((header, index) => (
                                            <th key={index} className="border border-gray-400 px-4 py-2 text-left">
                                                {header} {/* Display the actual column name without swapping */}
                                            </th>
                                        ))}
                                    </tr>
                                </thead>
                                <tbody>
                                    {uploadedFiles[selectedSubject]?.data?.map((row, rowIndex) => (
                                        <tr key={rowIndex} className={rowIndex % 2 === 0 ? "bg-gray-300" : "bg-gray-100"}>
                                            {Object.keys(uploadedFiles[selectedSubject]?.data?.[0] || {}).map((key, cellIndex) => (
                                                <td key={cellIndex} className="border border-gray-400 px-4 py-2 text-gray-900">
                                                    {row[key]} {/* Ensures values match their headers */}
                                                </td>
                                            ))}
                                        </tr>
                                    ))}
                                </tbody>
                                </table>
                            </div>
                        </>
                    ) : (
                        <p className="text-gray-500">No data uploaded yet for "{selectedSubject}"</p>
                    )}
                </div>
            )}
        </div>
    );
}