"use client";
import React from "react";
import { useUploadContext } from "../components/UploadContext";
import SaftData, { GroupedSaftEntries } from "../components/SaftData";
import Link from "next/link";
import { Button } from "primereact/button";

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
      <Link href="/displaycsvexcel">
        <Button
          rounded
          label="Gå tilbake"
          className="bg-[#EAEAEA] text-black hover:bg-[#D0D0D0] active:bg-[#9D9D9D] px-4 py-2 shadow-md w-[120px] h-[32px]"
        />
      </Link>
      <SaftData data={data} />
    </div>
  );
}



