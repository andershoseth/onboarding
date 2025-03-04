"use client";

import Image from 'next/image';
import { useBoxState } from '../components/MenuContainer';
import ImportContext from '../components/ImportContext';
import { useContext, useEffect, useState } from 'react';

function Success() {
  const { selected } = useBoxState();
  const { fileName } = useContext(ImportContext);

  const [clientFileName, setClientFileName] = useState<string | null>(null);

  useEffect(() => {
    setClientFileName(fileName);
  }, [fileName]);

  const checkedBoxes = Object.keys(selected)
    .filter((key) => selected[key as keyof typeof selected]) 
    .map((key) => key.charAt(0).toUpperCase() + key.slice(1));

  return (
    <div className="flex flex-col items-center text-center mt-10 w-full ">
      <h1 className="text-5xl font-bold mt-10">
        <span className="text-black">Suksess!</span>
      </h1>

      <div className="mt-10 w-full max-w-6xl">
        <div className="flex flex-col sm:flex-row w-full mb-8">
          <div className="sm:w-1/2 w-full h-80 bg-white text-blue-600 mr-6 rounded-3xl flex flex-col items-center justify-center p-6 text-2xl font-semibold shadow-lg">
            Du er nå i mål med importen!
            <div className="mt-4 text-xl text-gray-700 w-full">
              <p>Du har importert følgende:</p>
              <p className="font-bold text-lg text-gray-800">
                {clientFileName ? clientFileName : "Ingen fil lastet opp"}
              </p>
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
            </div>
          </div>

          <div className="flex flex-col w-full sm:w-1/2">
            <div className="h-40 bg-white text-blue-600 mb-6 rounded-3xl flex items-center justify-center p-6 text-2xl font-semibold shadow-lg">
              Sjekk ut kom-i-gang-artiklene våre!
            </div>
            <div className="h-40 bg-white text-blue-600 rounded-3xl flex items-center justify-between p-6 text-2xl font-semibold shadow-lg">
              <div className="flex items-start">
                <Image 
                  src="/global-news-4305.png" 
                  alt="global news" 
                  width={300} 
                  height={300} 
                  className="w-auto h-auto"
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
