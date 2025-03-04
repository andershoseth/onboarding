'use client';
import React, { useContext, useEffect, useState } from 'react';
import Link from "next/link";
import FileUploader from '../components/FileUploader';
import ImportContext from '../components/ImportContext';



export default function UploadPage() {
  const { selectedSystem } = useContext(ImportContext);
  const [hasMounted, setHasMounted] = useState(false);
  const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]); //array for the checked boxes


  useEffect(() => {
    setHasMounted(true);

    const savedCheckBoxes = localStorage.getItem("checkboxState"); //saves the checked boxes in the array
    if (savedCheckBoxes) {
      const parsedCheckBoxes = JSON.parse(savedCheckBoxes);
      const selectedLabels = Object.entries(parsedCheckBoxes)
        .filter(([_, value]) => value)
        .map(([key]) => key);
      setCheckedBoxes(selectedLabels)
    }

  }, []);

  if (!hasMounted) {
    return null;
  }

  return (
    <div className="grid grid-rows-[20px_1fr_20px] items-center justify-items-center min-h-screen p-8 pb-20 gap-16 sm:p-20 font-[family-name:var(--font-geist-sans)]">
      <main className="flex flex-col gap-8 row-start-2 items-center sm:items-center">
        <p>
          {selectedSystem ? `You selected ${selectedSystem}` : "No system selected"}
        </p>
        <p>
          {`You selected: ${checkedBoxes.join(", ")}`} {/* simple string to display the boxes checked */}
        </p>
        <h1 className="text-3xl sm:text-4xl font-bold text-center">
          Upload your files
        </h1>
        <h1><Link href="/ResultTable">Gå til ResultTable</Link></h1>
        <p className="text-center">
          Get started by uploading your files to our server.
        </p>

        {/* Render the FileUploader component here */}
        <div className="mx-auto">
          <FileUploader />
        </div>

        <div className="mt-6 flex justify-center">
          <Link
            href="/importvelger"
            className="bg-white text-black px-4 py-2 rounded-md shadow-md hover:bg-[#c85b34] transition"
          >
            Previous
          </Link>
        </div>
      </main>
      <footer className="row-start-3 flex gap-6 flex-wrap items-center justify-center">
        <a
          className="flex items-center gap-2 hover:underline hover:underline-offset-4"
          href="/home"
        >
          Home
        </a>
      </footer>
    </div>
  );
}
