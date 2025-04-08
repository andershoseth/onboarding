"use client";

import Image from 'next/image';
import { useContext } from 'react';
import ImportContext from '../components/ImportContext';
import { useSearchParams } from "next/navigation";
import { Button } from 'primereact/button';

function Success() {
  const { selectedColumns, fileName, setMappingCompleted } = useContext(ImportContext);
  const searchParams = useSearchParams();
  const id = searchParams.get("id"); // e.g., "/success?id=abcdef123..."

  // Get the selected checkboxes (only the ones that are checked)
  const checkedBoxes = Object.keys(selectedColumns)
    .filter((key) => selectedColumns[key]) // Filter out only checked boxes
    .map((key) => key.charAt(0).toUpperCase() + key.slice(1)); // e.g. "Name" -> "Name"

  return (
    <div className="flex flex-col items-center text-center w-full">
      <h1 className="text-5xl font-bold mt-8">
        <span className="text-white">Suksess!</span>
      </h1>

      <div className="mt-10 w-full max-w-6xl">
        <div className="flex flex-col sm:flex-row w-full mb-4">
          <div className="sm:w-1/2 w-full bg-white text-[#1867DD] mr-6 rounded-3xl flex flex-col items-center justify-center p-6 shadow-lg min-h-80">
            <h1>Du er nå i mål med importen! </h1>
            <div className="mt-4 text-xl text-black w-full">
              <h2 className="text-[#AE74EB]">Du har importert følgende filer:</h2>
              {fileName.length > 0 ? (
                <ul className="text-black flex flex-col items-center mt-2 gap-2">
                  {fileName.map((name, index) => (
                    <li key={index} className="flex items-center justify-center gap-2">
                      <h4>- {name}</h4>
                    </li>
                  ))}
                </ul>
              ) : (
                <h4 className="text-black"> Ingen filer lastet opp </h4>
              )}
              <br />
              <h2 className="text-[#AE74EB]">Med disse valgene fra importvelgeren:</h2>
              <ul className="text-black flex flex-col items-center mt-2 gap-2">
                {checkedBoxes.length > 0 ? (
                  checkedBoxes.map((box, index) => (
                    <li key={index} className="text-black">
                      <h4>- {box} </h4>
                    </li>
                  ))
                ) : (
                  <li className="text-lg text-black"> <h4>Ingen bokser valgt.</h4> </li>
                )}
              </ul>
            </div>
          </div>

          <div className="flex flex-col w-full sm:w-1/2">
            <div className="h-40 bg-white flex flex-col mb-6 justify-center p-6 rounded-3xl items-center text-center mt-5 shadow-lg">
              <h1 className="text-[#1867DD] font-semibold">Last ned de konverterte filene ved å trykke på knappen nedenfor: </h1>
              {/* 
                If 'id' is present, show a button/link to download the mapped CSV. 
                The backend minimal API is at "/api/download/{id}" returning text/csv.
              */}
              {id && (
                <div className="mt-3"> {/* implementert figma design */}
                  <a href={`http://localhost:5116/api/download/${id}`} download>
                    <Button
                      rounded
                      label="Last ned"
                      className="bg-[#1E721E] text-white hover:bg-[#449844] active:bg-[#075607] px-4 py-2 h-[45px] shadow-md inline-block text-xl"
                      onClick={() => { setMappingCompleted(true) }}
                    />
                  </a>
                </div>
              )}
            </div>
            <div className="h-40 bg-white text-[#1867DD] rounded-3xl flex items-center justify-between p-6 text-2xl font-semibold shadow-lg">
              <div className="flex items-center justify-center text-center w-full">
                <h1>Start prosessen på nytt? Klikk&nbsp;<a href="/home" className="text-[#FF8D04] underline">her</a> for å bli sendt til start! </h1> {/* &nsbp; er mellomrom i html. Refresher siden og tar deg tilbake til start */}
              </div>
            </div>
          </div>
        </div>

        <div className="flex flex-col sm:flex-row w-full">

          <div className="sm:w-1/2 w-full h-40 bg-white text-[#1867DD] mr-6 rounded-3xl flex items-center justify-start p-6 text-2xl font-semibold shadow-lg">
            <div className="flex items-center w-full h-full">
              <div className="w-1/3 h-full flex items-center justify-center">
                <Image
                  src="/global-news-4305.png"
                  alt="global news"
                  width={300}
                  height={300}
                  className="w-full h-full object-contain"
                />
              </div>
              <div className="w-2/3 pl-4">
                <h1>Les de siste nyhetene våre!</h1>
              </div>
            </div>
          </div>

          <div className="sm:w-1/2 w-full h-40 bg-white text-[#1867DD] rounded-3xl flex items-center justify-center p-6 text-2xl font-semibold shadow-lg">
            <h1>Bruker du andre system sammen med Go?</h1>
          </div>

        </div>
      </div>
    </div>
  );
}

export default Success;
