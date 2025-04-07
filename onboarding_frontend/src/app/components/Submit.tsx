// SubmitStandardImportButton.tsx
"use client";

import React from "react";
import { useMapping } from "../components/MappingContext";
import { buildStandardImport } from "../utils/BuildStandardImport"

export default function SubmitStandardImportButton() {
  const { groupedRows, mapping } = useMapping();

  async function handleSubmit() {

    const stdImport = buildStandardImport(groupedRows, mapping);
    console.log("Constructed StandardImport:", stdImport);

    const body = JSON.stringify(stdImport);
    console.log("Sending JSON:", body);
    const response = await fetch("http://localhost:5116/api/standard-import-object", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(stdImport),
    });

    if (!response.ok) {
      console.error("Failed to POST standard import");
      return;
    }

    const result = await response.json();
    console.log("Success result:", result);
    alert("Import success!");
  }

  return (
    <button onClick={handleSubmit} className="bg-blue-500 text-white px-4 py-2 rounded">
      Submit Standard Import
    </button>
  );
}
