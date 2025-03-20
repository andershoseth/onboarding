"use client";
import React from "react";
import { useUploadContext } from "../components/UploadContext";
import SaftData, { GroupedSaftEntries } from "../components/SaftData";

export default function SaftTablePage() {

  const { uploadedFiles } = useUploadContext();
  const subject = "safTExport";

  // Hent dataen for det gitte subjectet
  const data: GroupedSaftEntries[] = uploadedFiles[subject]?.data || [];

  if (data.length === 0) {
    return <div className="p-4">Ingen SAF-T data tilgjengelig.</div>;
  }

  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold mb-4">SAF-T Table</h1>
      <SaftData data={data} />
    </div>
  );
}