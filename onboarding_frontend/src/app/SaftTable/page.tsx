"use client";
import React from "react";
import { useUploadContext } from "../components/UploadContext";
import SaftData, { GroupedSaftEntries } from "../components/SaftData";
import Link from "next/link";
import { Button } from "primereact/button";
import { useMapping } from "../components/MappingContext";
import { buildStandardImport } from "../utils/BuildStandardImport";

export default function SaftTablePage() {
  const { groupedRows, mapping } = useMapping();
  const { uploadedFiles } = useUploadContext();
  const subject = "safTExport";

  // Retrieve the SAF-T data for the subject 
  const data: GroupedSaftEntries[] = uploadedFiles[subject]?.data || [];

  if (data.length === 0) {
    return <div className="p-4">Ingen SAF-T data tilgjengelig.</div>;
  }

  async function handleSubmit() {
    try {
      const stdImport = buildStandardImport(groupedRows, mapping);
      console.log("Constructed StandardImport:", stdImport);

      const response = await fetch(
        "http://localhost:5116/api/StandardImport/import",
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(stdImport),
        }
      );

      if (!response.ok) {
        const errText = await response.text();
        console.error("Failed to POST standard import:", errText);
        alert("Import failed: " + errText);
        return;
      }

      const blob = await response.blob();
      const url = URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = "StandardImport.xlsx";
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);

      alert("Import success! Excel downloaded.");
    } catch (error: any) {
      console.error(error);
      alert("An error occurred: " + error.message);
    }
  }
  return (
    <div className="p-6 min-h-screen pt-16 bg-white text-black">
      <Link href="/displaycsvexcel">
        <Button
          rounded
          label="Gå tilbake"
          className="bg-[#EAEAEA] text-black hover:bg-[#D0D0D0] active:bg-[#9D9D9D] px-4 py-2 shadow-md w-[120px] h-[32px]"
        />
      </Link>
      <Button
        rounded
        label="Send inn standardimporten"
        onClick={handleSubmit}
        className="bg-[#1E721E] text-white hover:bg-[#449844] active:bg-[#075607] px-4 py-2 shadow-md w-[250px] h-[32px] mx-4"
      />
      <SaftData data={data} />
    </div>
  );
}