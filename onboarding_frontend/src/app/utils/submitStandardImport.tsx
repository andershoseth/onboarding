"use client"
import { buildStandardImport } from "./BuildStandardImport";
import { GroupedSaftEntries } from "../components/SaftData";

export async function submitStandardImport(groupedRows: GroupedSaftEntries[], mapping: any) {
    const stdImport = buildStandardImport(groupedRows, mapping);
    console.log("Constructed StandardImport:", stdImport);

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
