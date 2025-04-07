"use client";
import React from "react";
import { useUploadContext } from "../components/UploadContext";
import SaftData, { GroupedSaftEntries } from "../components/SaftData";

export default function SaftTablePage() {

  const { uploadedFiles } = useUploadContext();
  const subject = "safTExport";

  // Retrieve the SAF-T data for the subject 
  const data: GroupedSaftEntries[] = uploadedFiles[subject]?.data || [];

  if (data.length === 0) {
    return <div className="p-4">Ingen SAF-T data tilgjengelig.</div>;
  }

  return (
    <div className="p-6 min-h-screen pt-16 bg-white text-black">
      <SaftData data={data} />
    </div>
  );
}



