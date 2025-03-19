"use client";
import React, { useEffect, useState } from "react";

export default function DisplayCsvExcel() {
  const [data, setData] = useState<any[]>([]);

  useEffect(() => {
    const storedData = localStorage.getItem("uploadedData");
    if (storedData) {
      setData(JSON.parse(storedData));
    }
  }, []);

  if (!data || data.length === 0) {
    return <p>No data available to display.</p>;
  }

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">CSV/Excel Data</h1>
      <table className="min-w-full border-collapse">
        <thead>
          <tr>
            {Object.keys(data[0]).map((key) => (
              <th key={key} className="border p-2">{key}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.map((row, rowIndex) => (
            <tr key={rowIndex}>
              {Object.values(row).map((value, colIndex) => (
                <td key={colIndex} className="border p-2">{value}</td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
