"use client";
import React, { useState, useEffect, useContext } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useUploadContext } from "../components/UploadContext";
import ImportContext from "../components/ImportContext";
import MappingHeader from "../utils/MappingHeader";
import { TableFieldMapping } from "../components/SaftData";
import { Button } from "primereact/button";
import { useCSVExcelMapping } from "../components/CSVExcelMappingContext";
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
  const { selectedFileType } = useContext(ImportContext);
  const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]);
  const [selectedSubject, setSelectedSubject] = useState<string | null>(null);
  const isDisabled = !selectedSubject || checkedBoxes.length === 1; //kun én kategori kan være valgt for å sende inn

  const showSaftButton = selectedFileType === "SAF-T (.xml)"; //vise saft-knapp hvis saf-t er valgt filtype

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
      alert("Ingen subjekt valgt!");
      return;
    }

    const csvRows = uploadedFiles[selectedSubject]?.data;
    if (!csvRows) {
      alert("Ingen CSV-data funnet for dette subjektet.");
      return;
    }

    // Flatten nested table mapping
    const flattenMapping = (nestedMapping: CSVExcelMapping) => {
      const flat: Record<string, string> = {};
      for (const table in nestedMapping) {
        for (const col in nestedMapping[table]) {
          flat[col] = nestedMapping[table][col];
        }
      }
      return flat;
    };

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
    <div className="p-6 min-h-screen pt-8">
      <div className="flex flex-col items-center text-center mt-5">
        <h1 className="text-4xl font-bold">Visning av filene.</h1>
      </div>

      <div className="flex flex-col items-center text-center mt-5">
        <h2 className="text-xl">Trykk på knappene for å få en visning av filene du lastet opp.</h2>
      </div>

      <div className="flex flex-col items-center text-center mt-5">
        <h4 className="text-xl">Klikk på kolonnene for å endre navn med hjelp av dropdown-menyen. </h4>
      </div>

      <div className="flex flex-col items-center text-center mt-5">
        <h4 className="text-xl">Ved å klikke på "Send inn"-knappen blir du tatt videre til neste side hvor du kan laste ned de konverterte filene. </h4>
      </div>

      <div className="flex flex-wrap gap-6 mb-6 mt-10">
        {checkedBoxes.map((subject) => (
          <Button
            rounded
            key={subject}
            onClick={() => setSelectedSubject(subject)}
            className="bg-[#EAEAEA] text-black hover:bg-[#D0D0D0] active:bg-[#9D9D9D] px-4 py-2 shadow-md h-[32px] capitalize"
          >
            {subject}
          </Button>
        ))}

        {showSaftButton && (
          <Link href="/SaftTable">
            <Button
              rounded
              label="Saf-T"
              className="bg-[#EAEAEA] text-black hover:bg-[#D0D0D0] active:bg-[#9D9D9D] px-4 py-2 h-[32px] shadow-md w-[100px]"
            />
          </Link>
        )}

        <Button
          rounded
          label="Send inn"
          onClick={handleCompleteMapping}
          className={`px-4 py-2 w-[100px] h-[32px] shadow-md transition ${isDisabled
            ? "bg-[#DAF0DA] text-white cursor-not-allowed px-4 py-2 shadow-md"
            : "bg-[#1E721E] text-white hover:bg-[#449844] active:bg-[#075607] px-4 py-2 shadow-md"
            }`}
        />
      </div>

      {selectedSubject && (
        <div className="mt-6">
          <h2 className="text-xl font-semibold mb-2 capitalize">Data for: {selectedSubject}</h2>
          {uploadedFiles[selectedSubject] ? (
            <>
              <h4 className="mb-2">
                Filnavn: <strong>{uploadedFiles[selectedSubject].fileName}</strong>
              </h4>

              {/*  group all rows by TableName */}
              {(() => {
                // allRows is the entire array from the backend
                const allRows = uploadedFiles[selectedSubject].data as any[];

                if (!allRows || allRows.length === 0) {
                  return <p>No rows found</p>;
                }

                // extracts the table names using a record type, for the html
                const grouped = groupByTableName(allRows);

                // Create a table for each distinct TableName
                return Object.keys(grouped).map((tableName, index) => {

                  if (!tableName) return null;

                  const rowsForTable = grouped[tableName];
                  if (!rowsForTable || rowsForTable.length === 0) {
                    return null;
                  }

                  // The column headers come from the keys of the first row (in the record type)

                  const rowKeys = Object.keys(rowsForTable[0] ?? {}).filter(
                    (k) => k && k.trim() !== "" && k !== "TableName"
                  );
                  return (
                    <div key={index}>
                      <h2 className="mb-2 mt-5">Table: {tableName}</h2>
                      <div className="max-w-full overflow-x-auto" style={{ overflowX: "auto" }}> {/* horisontal scroll */}
                        <div className="min-w-max" style={{ maxHeight: "550px" }}> {/* vertikal scroll */}
                          <table className="table-auto w-full">
                            <thead className="bg-gray-600 text-white sticky top-0 z-10">
                              <tr>
                                {rowKeys.map((header, hIndex) => {
                                  const currentMapping = csvMapping[tableName]?.[header] || "";
                                  return (
                                    <th
                                      key={hIndex}
                                      className="border border-gray-400 px-4 py-4 text-left min-w-[12rem] bg-gray-600"
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
                                  className={rowIndex % 2 === 0 ? "bg-gray-300" : "bg-gray-100"}
                                >
                                  {rowKeys.map((key, cellIndex) => (
                                    <td
                                      key={cellIndex}
                                      className="border border-gray-400 px-4 py-2 text-gray-900 min-w-[12rem]"
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
                    </div>

                  );

                });
              })()}
            </>
          ) : (
            <h4 className="text-white">Ingen data lastet opp for "{selectedSubject}"</h4>
          )}
        </div>
      )}
    </div>
  );
}