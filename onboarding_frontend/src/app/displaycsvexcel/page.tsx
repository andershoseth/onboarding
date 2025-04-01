"use client";
import React, { useState, useEffect } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useUploadContext } from "../components/UploadContext";
import { useMapping } from "../components/MappingContext";
import MappingHeader from "../utils/MappingHeader";
import { TableFieldMapping } from "../components/SaftData";
import { Button } from "primereact/button";

export default function FileDisplayPage() {
  const router = useRouter();

  const { uploadedFiles } = useUploadContext();
  const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]);
  const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

  // From mappingcontext. Mapping is { [columnName: string]: string }
  const { mapping, setMapping } = useMapping();

  // table/field definitions from backend
  const [tableFieldMappings, setTableFieldMappings] = useState<TableFieldMapping[]>([]);

  useEffect(() => {
    fetch("http://localhost:5116/api/standard-import-mapping")
      .then((res) => res.json())
      .then((data: TableFieldMapping[]) => {
        setTableFieldMappings(data);
      })
      .catch((error) => console.error("Error fetching standard import mapping:", error));
  }, []);

  useEffect(() => {
    const savedCheckBoxes = localStorage.getItem("checkboxState");
    if (savedCheckBoxes) {
      const parsedCheckBoxes = JSON.parse(savedCheckBoxes);
      const selectedLabels = Object.entries(parsedCheckBoxes)
        .filter(([_, value]) => value === true)
        .map(([key]) => key);
      setCheckedBoxes(selectedLabels);
    }
  }, []);


  // The function to call on "Next" or "Complete Mapping"
  const handleCompleteMapping = async () => {
    if (!selectedSubject) {
      alert("No subject selected!");
      return;
    }

    // 1) Get the CSV data for the selected subject
    const csvRows = uploadedFiles[selectedSubject]?.data;
    if (!csvRows) {
      alert("No CSV data found for this subject.");
      return;
    }

    // 2) Build the payload
    const payload = {
      Mapping: mapping,
      Data: csvRows
    };

    // 3) POST to the .NET backend
    try {
      const response = await fetch("http://localhost:5116/api/perform-mapping", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });

      if (!response.ok) {
        alert("Failed to map the CSV on the server.");
        return;
      }

      const result = await response.json();

      if (result.success) {
        // 4) Navigate to success page with the ID
        router.push(`/success?id=${result.id}`);
      } else {
        alert("Server reported failure, no ID returned.");
      }
    } catch (err) {
      console.error("Error calling /api/perform-mapping:", err);
      alert("Error performing mapping. Check console.");
    }
  };

  return (
    <div className="p-6 min-h-screen pt-16">
      <h2 className="text-2xl font-bold mb-4">Uploaded File Data</h2>
      <p className="mb-4">
        Subjects you selected: {checkedBoxes.length > 0 ? checkedBoxes.join(", ") : "No subjects selected."}
      </p>

      <div className="flex flex-wrap gap-4 mb-6">
        {checkedBoxes.map((subject) => (
          <Button
            rounded
            key={subject}
            onClick={() => setSelectedSubject(subject)}
            className="bg-blue-500 text-white px-4 py-2 rounded shadow hover:bg-blue-600"
          >
            {subject}
          </Button>
        ))}

        <Link href="/SaftTable">
          <Button
            rounded
            label="Saf-T"
            className="bg-green-500 text-white px-4 py-2 rounded shadow hover:bg-green-600"
          />
        </Link>

        {/* Instead of Link to /success, we do a button that triggers handleCompleteMapping */}
        <Button
          rounded
          label="Complete mapping"
          onClick={handleCompleteMapping}
          className="px-4 py-2 rounded-md shadow-md transition bg-[#E17044] text-white hover:bg-[#c85b34]"
        >
        </Button>
      </div>

      {selectedSubject && (
        <div className="mt-6">
          <h2 className="text-xl font-semibold mb-2">Data for: {selectedSubject}</h2>
          {uploadedFiles[selectedSubject] ? (
            <>
              <p className="mb-2">
                File Name: <strong>{uploadedFiles[selectedSubject].fileName}</strong>
              </p>
              <div className="overflow-y-auto max-h-[calc(80vh-150px)] border border-gray-500 rounded-lg shadow-md bg-white">
                <table className="min-w-full">
                  <thead className="bg-gray-600 text-white sticky top-0 z-10">
                    <tr>
                      {Object.keys(uploadedFiles[selectedSubject].data[0] || {}).map((header, index) => (
                        <th key={index} className="border border-gray-400 px-4 py-2 text-left">
                          <MappingHeader
                            columnLabel={header}
                            tableFieldMappings={tableFieldMappings}
                            currentMapping={mapping[header] || ""}
                            onMappingSelect={(selected) => {
                              setMapping((prev) => ({
                                ...prev,
                                [header]: selected,
                              }));
                            }}
                          />
                        </th>
                      ))}
                    </tr>
                  </thead>
                  <tbody>
                    {uploadedFiles[selectedSubject].data.map((row: any, rowIndex: number) => (
                      <tr
                        key={rowIndex}
                        className={rowIndex % 2 === 0 ? "bg-gray-300" : "bg-gray-100"}
                      >
                        {Object.keys(uploadedFiles[selectedSubject].data[0] || {}).map((key, cellIndex) => (
                          <td key={cellIndex} className="border border-gray-400 px-4 py-2 text-gray-900">
                            {row[key]}
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