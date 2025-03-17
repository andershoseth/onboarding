"use client";
import React, { useEffect, useState } from "react";

import SaftData ,{ GroupedSaftEntries } from "../components/SaftData";

export default function ResultPage() {
  const [saftData, setSaftData] = useState<GroupedSaftEntries[]>([]);
  const [loading, setLoading] = useState(true);
 
  useEffect(() => {
    const formData = new FormData();
    

  fetch("http://localhost:5116/api/upload", {
    method: "POST",
    body: formData,
  })
      .then((res) => res.json())
      .then((fetchedData: GroupedSaftEntries[]) => {
        console.log("Hentet saf-t fil data:", fetchedData);
        setSaftData(fetchedData);
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