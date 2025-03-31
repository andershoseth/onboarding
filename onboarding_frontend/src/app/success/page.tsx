"use client";

import Image from 'next/image';
import { useContext, useState } from 'react';
import ImportContext from '../components/ImportContext';
import { useSearchParams } from "next/navigation";
import { Button } from 'primereact/button';

function Success() {
  const { selectedColumns, fileName, setMappingCompleted } = useContext(ImportContext);
  const [downloadClicked, setDownloadClicked] = useState(false)
  const searchParams = useSearchParams();
  const id = searchParams.get("id"); // e.g., "/success?id=abcdef123..."

  const handleDownload = () => {
    setDownloadClicked(true);
    setMappingCompleted(true);
  }

  // Get the selected checkboxes (only the ones that are checked)
  const checkedBoxes = Object.keys(selectedColumns)
    .filter((key) => selectedColumns[key]) // Filter out only checked boxes
    .map((key) => key.charAt(0).toUpperCase() + key.slice(1)); // e.g. "Name" -> "Name"

  return (
    <div className="flex flex-col items-center text-center mt-10 w-full">
      <h1 className="text-5xl font-bold mt-10">
        <span className="text-black">Suksess!</span>
      </h1>

      <div className="mt-10 w-full max-w-6xl">
        <div className="flex flex-col sm:flex-row w-full mb-8">
          <div className="sm:w-1/2 w-full bg-white text-blue-600 mr-6 rounded-3xl flex flex-col items-center justify-center p-6 text-2xl font-semibold shadow-lg min-h-80">
            Du er nå i mål med importen!
            <div className="mt-4 text-xl text-gray-700 w-full">
              <p>Du har importert følgende filer:</p>
              {fileName.length > 0 ? (
                <ul className="font-bold text-lg text-gray-800 flex flex-col items-center gap-2">
                  {fileName.map((name, index) => (
                    <li key={index} className="flex items-center justify-center gap-2">
                      <span>- {name}</span>
                    </li>
                  ))}
                </ul>
              ) : (
                <p className="font-bold text-lg text-gray-800">
                  Ingen filer lastet opp
                </p>
              )}

              <p>Med disse valgene fra importvelgeren:</p>
              <ul className="mt-2">
                {checkedBoxes.length > 0 ? (
                  checkedBoxes.map((box, index) => (
                    <li key={index} className="text-lg text-gray-700">
                      - {box}
                    </li>
                  ))
                ) : (
                  <li className="text-lg text-gray-700">Ingen bokser valgt.</li>
                )}
              </ul>

              {/* 
                If 'id' is present, show a button/link to download the mapped CSV. 
                The backend minimal API is at "/api/download/{id}" returning text/csv.
              */}
              {id && (
                <div className="mt-6">
                  <a href={`http://localhost:5116/api/download/${id}`} download>
                    <Button
                      rounded
                      label="Last ned mappet CSV"
                      className="inline-block px-4 py-2 text-white bg-green-600 hover:bg-green-700"
                      onClick={handleDownload}
                    />
                  </a>
                </div>
              )}
            </div>
          </div>

          <div className="flex flex-col w-full sm:w-1/2">
            <div className="h-40 bg-white text-blue-600 mb-6 rounded-3xl flex items-center justify-center p-6 text-2xl font-semibold shadow-lg">
              Sjekk ut kom-i-gang-artiklene våre!
            </div>
            <div className="h-40 bg-white text-blue-600 rounded-3xl flex items-center justify-between p-6 text-2xl font-semibold shadow-lg">
              <div className="relative w-full h-full p-4 flex items-start">
                <Image
                  src="/global-news-4305.png"
                  alt="global news"
                  width={300}
                  height={300}
                  className="w-full h-full object-contain"
                />
              </div>
              <div className="flex items-center justify-center text-center w-full">
                Les de siste<br />nyhetene våre
              </div>
            </div>
          </div>
        </div>

        <div className="flex flex-col sm:flex-row w-full">
          <div className="sm:w-1/2 w-full h-40 bg-white text-blue-600 mr-6 rounded-3xl flex items-center justify-center p-6 text-2xl font-semibold shadow-lg">
            Importer fra tidligere<br />lønnssystem
          </div>
          <div className="sm:w-1/2 w-full h-40 bg-white text-blue-600 rounded-3xl flex items-center justify-center p-6 text-2xl font-semibold shadow-lg">
            Bruker du andre system sammen med Go?
          </div>
        </div>
      </div>
    </div>
  );
}

export default Success;
