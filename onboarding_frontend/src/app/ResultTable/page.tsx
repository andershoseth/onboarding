"use client";
import React from "react";
import ResultTable from '../components/ResultsTable';

export default function ResultPage() {
  // Dummy-data
  const dummyData = [
    {
      firstName: "Ola",
      lastName: "Nordmann",
      age: "30",
      Surname: {
        underniv2: "Test1",
        undernivTo: "test2",
        underniv3 : {
               undernv3 : "Test3",
               under3 : "Test4",
         } }
    },
    { firstName: "Kari", lastName: "Hansen", age: '25' },
    { firstName: "Jon", lastName: "Olsen", age: '40' },
  ];

  return (
    <main className="p-4">
      <h1 className="text-2xl font-bold mb-4">Testing Dummy Data</h1>
      <ResultTable data={dummyData} />
    </main>
  );
}