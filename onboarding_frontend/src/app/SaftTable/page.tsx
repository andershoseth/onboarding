"use client";
import React, { useEffect, useState } from "react";
import { useUploadContext } from "../components/UploadContext";
import SaftData ,{ GroupedSaftEntries } from "../components/SaftData";

export default function ResultPage() {
  const [saftData, setSaftData] = useState<GroupedSaftEntries[]>([]);
  const [loading, setLoading] = useState(true);
  const { selectedFile } = useUploadContext()
  useEffect(() => {
    // Hvis ingen fil er valgt, gjør ingenting
    if (!selectedFile) {
      setLoading(false);
      return;
    }
    const formData = new FormData();
    formData.append("file", selectedFile);

    fetch("http://localhost:5116/api/upload", {
      method: "POST",
      body: formData,
    })
      .then((res) => {
        if (!res.ok) {
          throw new Error(`Feil: ${res.status} ${res.statusText}`);
        }
        return res.json();
      })
      .then((fetchedData: GroupedSaftEntries[]) => {
        console.log("Hentet SafT-data:", fetchedData);
        setSaftData(fetchedData);
      })
      .catch((error) => {
        console.error("Feil under henting:", error);
      })
      .finally(() => {
        setLoading(false);
      });
  }, [selectedFile]); // useEffect kjører når selectedFile endres


  if (loading) {
    return <div className="p-4">Laster...</div>;
  }

  if (!saftData) {
    return <div className="p-4">Ingen data hentet .</div>;
  }

  return (
    <main className="p-4">
      <h1 className="text-2xl font-bold mb-4">Saf-t fil</h1>
      <SaftData data={saftData} />
    </main>
  );
}