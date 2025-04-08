"use client";

import React, { useState, useEffect } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useUploadContext } from "../components/UploadContext";
import { useCSVExcelMapping } from "../components/CSVExcelMappingContext";
import MappingHeader from "../utils/MappingHeader";
import { TableFieldMapping } from "../components/SaftData";
import { CSVExcelMapping } from "../components/CSVExcelMappingContext";

// Helper function to group rows by tablename
function groupByTableName(rows: any[]) {
  const groups: Record<string, any[]> = {};
  for (const row of rows) {
    const tableName = row.TableName || ""; // Fall back to empty string
    if (!groups[tableName]) {
      groups[tableName] = [];
    }
    groups[tableName].push(row);
  }
  return groups;
}

export default function FileDisplayPage() {
  const router = useRouter();

  const { uploadedFiles } = useUploadContext();
  const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]);
  const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

  const { csvMapping, setCSVMapping } = useCSVExcelMapping();

  // Table/field definitions from backend
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

    const csvRows = uploadedFiles[selectedSubject]?.data;
    if (!csvRows) {
      alert("No CSV data found for this subject.");
      return;
    }

    // Flatten nested table mapping (if you use per-table mapping)
    const flattenMapping = (nestedMapping: CSVExcelMapping) => {
      const flat: Record<string, string> = {};
      for (const table in nestedMapping) {
        for (const col in nestedMapping[table]) {
          flat[col] = nestedMapping[table][col];
        }
      }
      return flat;
    };

    // Build the payload
    const payload = {
      Mapping: flattenMapping(csvMapping),
      Data: csvRows,
    };

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

  useEffect(() => {
    if (selectedSubject && uploadedFiles[selectedSubject]?.data) {
      console.log("Raw uploaded data:", uploadedFiles[selectedSubject].data.slice(0, 5));
    }
  }, [selectedSubject, uploadedFiles]);

  return (
    <div className="p-6 min-h-screen pt-16">
      <h2 className="text-2xl font-bold mb-4">Uploaded File Data</h2>
      <p className="mb-4">
        Subjects you selected:{" "}
        {checkedBoxes.length > 0 ? checkedBoxes.join(", ") : "No subjects selected."}
      </p>

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

        <Link
          href="/SaftTable"
          className="bg-green-500 text-white px-4 py-2 rounded shadow hover:bg-green-600"
        >
          SAF-T
        </Link>

        <button
          onClick={handleCompleteMapping}
          className="px-4 py-2 rounded-md shadow-md transition bg-[#E17044] text-white hover:bg-[#c85b34]"
        >
          Complete Mapping
        </button>
      </div>

      {selectedSubject && (
        <div className="mt-6">
          <h2 className="text-xl font-semibold mb-2">Data for: {selectedSubject}</h2>

          {uploadedFiles[selectedSubject] ? (
            <>
              <p className="mb-2">
                File Name: <strong>{uploadedFiles[selectedSubject].fileName}</strong>
              </p>

              {/* NEW: group all rows by TableName */}
              {(() => {
                // allRows is the entire array from the backend
                const allRows = uploadedFiles[selectedSubject].data as any[];

                if (!allRows || allRows.length === 0) {
                  return <p>No rows found</p>;
                }

                // Group them
                const grouped = groupByTableName(allRows);

                // We'll create a table for each distinct TableName
                return Object.keys(grouped).map((tableName, index) => {
                  // If tableName is empty, skip or handle
                  if (!tableName) return null;

                  const rowsForTable = grouped[tableName];
                  if (!rowsForTable || rowsForTable.length === 0) {
                    return null;
                  }

                  // The column headers come from the keys of the first row
                  // except we might skip "TableName"
                  const rowKeys = Object.keys(rowsForTable[0] ?? {}).filter(
                    (k) => k !== "TableName"
                  );

                  return (
                    <div key={index} className="mb-8">
                      <h3 className="text-lg font-semibold mb-2">Table: {tableName}</h3>

                      <div className="max-h-[calc(80vh-150px)] border border-gray-500 rounded-lg shadow-md bg-white overflow-auto">
                        <table className="min-w-full">
                          <thead className="bg-gray-600 text-white sticky top-0 z-10">
                            <tr>
                              {rowKeys.map((header, hIndex) => {
                                const currentMapping =
                                  csvMapping[tableName]?.[header] || "";

                                return (
                                  <th
                                    key={hIndex}
                                    className="border border-gray-400 px-4 py-2 text-left"
                                  >
                                    <MappingHeader
                                      columnLabel={header}
                                      tableFieldMappings={tableFieldMappings}
                                      currentMapping={currentMapping}
                                      onMappingSelect={(selected) => {
                                        setCSVMapping((prev) => ({
                                          ...prev,
                                          [tableName]: {
                                            ...(prev[tableName] || {}),
                                            [header]: selected,
                                          },
                                        }));
                                      }}
                                    />
                                  </th>
                                );
                              })}
                            </tr>
                          </thead>
                          <tbody>
                            {rowsForTable.map((row, rowIndex) => (
                              <tr
                                key={rowIndex}
                                className={
                                  rowIndex % 2 === 0 ? "bg-gray-300" : "bg-gray-100"
                                }
                              >
                                {rowKeys.map((key, cellIndex) => (
                                  <td
                                    key={cellIndex}
                                    className="border border-gray-400 px-4 py-2 text-gray-900"
                                  >
                                    {row[key]}
                                  </td>
                                ))}
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </div>
                    </div>
                  );
                });
              })()}
            </>
          ) : (
            <p className="text-gray-500">
              No data uploaded yet for "{selectedSubject}"
            </p>
          )}
        </div>
      )}
    </div>
  );
}

// END
