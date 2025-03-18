"use client";
import React, { useEffect, useState } from "react";
import StandardImportTables, { TableGroup } from "../components/StandardImportTables";

export default function ResultPage() {
  const [data, setData] = useState<TableGroup[] | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch("http://localhost:5116/api/upload")
      .then((res) => res.json())
      .then((fetchedData: TableGroup[]) => {
        console.log("Hentet standard import mapping:", fetchedData);
        setData(fetchedData);
        setLoading(false);
      })
      .catch((error) => {
        console.error("Feil under henting:", error);
        setLoading(false);
      });
  }, []);

  if (loading) {
    return <div className="p-4">Laster...</div>;
  }

  if (!data) {
    return <div className="p-4">Ingen data hentet .</div>;
  }

  return (
    <main className="p-4">
      <h1 className="text-2xl font-bold mb-4">Standard Import Felter</h1>
      <StandardImportTables data={data} />
    </main>
  );
}