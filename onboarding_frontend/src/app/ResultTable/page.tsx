"use client";
import React from "react";
import ResultTable from '../components/ResultsTable';
import { useUploadContext } from "../components/UploadContext"; 



export default function ResultPage() {
  const { uploadedData } = useUploadContext(); // 📌 Bruk kontekst

  console.log("🔍 Data i ResultPage:", uploadedData); // 📌 Sjekk hva som blir hentet

  if (!uploadedData) {
    return <div className="p-4">Ingen data lastet opp ennå.</div>;
  }

  return (
    <main className="p-4">
      <h1 className="text-2xl font-bold mb-4">API data i ResultTable</h1>
      <ResultTable data={uploadedData} />
    </main>
  );
}
