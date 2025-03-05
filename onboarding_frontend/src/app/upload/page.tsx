'use client';
import React, { useContext, useEffect, useState } from 'react';
import { useRouter } from "next/navigation";
import Link from "next/link";
import FileUploader from '../components/FileUploader';
import ImportContext from '../components/ImportContext';
import { useUploadContext } from "../components/UploadContext";

export default function UploadPage() {
  const { selectedSystem } = useContext(ImportContext);
  const { setUploadProgress } = useUploadContext();
  const [hasMounted, setHasMounted] = useState(false);
  const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]); //array for the checked boxes
  const [isTableLoading, setIsTableLoading] = useState(false);
  const [loadingProgress, setLoadingProgress] = useState(0);
  const router = useRouter();


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

    setUploadProgress(0);
  }, []);

  //PROGRESSBAR FOR RESULTTABLE
  const startLoading = () => {
    setIsTableLoading(true);
    setLoadingProgress(0)

    let progress = 0;
    const interval = setInterval(() => {
      progress += 10;
      setLoadingProgress(progress)

      if (progress >= 100) {
        clearInterval(interval);
        setTimeout(() => {
          setIsTableLoading(false);
          router.push("/ResultTable");
        }, 500)
      }
    }, 500)
  }

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

        <div className="mx-auto">
          <FileUploader />
        </div>

        {!isTableLoading && (
          <button
            onClick={startLoading}
            className="bg-white text-black px-4 py-2 rounded shadow hover:bg-[#c85b34] transtition"
          >
            Gå til ResultTable
          </button>
        )}

        {isTableLoading && (
          <div className="w-full bg-gray-200 h-4 rounded-full overflow-hidden mt-4">
            <div className="bg-blue-500 h-4 rounded-full transition-all duration-500" style={{ width: `${loadingProgress}%` }}>
            </div>
          </div>
        )}

        {isTableLoading && (
          <div className="w-full text-white mt-4">
            {loadingProgress}%
          </div>
        )}

        <div className="mt-6 flex justify-center">
          <Link
            href="/export"
            className="px-4 py-2 rounded-md shadow-md transition bg-[#E17044] text-white hover:bg-[#c85b34]"
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