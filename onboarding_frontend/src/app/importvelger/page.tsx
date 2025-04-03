"use client";
import React, { useContext } from "react";
import ImportContext from "../components/ImportContext";
import CheckBox from "./CheckBox";
import Link from "next/link";
import MenuContainer from "./MenuContainer";
import { Button } from "primereact/button";

const ImportVelger: React.FC = () => {
  const { selectedColumns, setSelectedColumns } = useContext(ImportContext);
  const isDisabled = !Object.values(selectedColumns || {}).some(Boolean);


  const handleCheckboxChange = (column: string) => {
    setSelectedColumns((prev) => ({
      ...prev,
      [column]: !prev[column],
    }));
  };

  return (
    <div className="flex flex-col items-center min-h-screen p-10 relative">
      {/* menucontainer for den dynamiske importvelgeren */}
      <div className="flex flex-col items-center text-center">
        <h1 className="text-4xl font-bold">Hva ønsker du å importere fra filene?</h1>
      </div>

      <div className="flex flex-col items-center text-center mt-5">
        <h2 className="text-xl">Bruk listen til å huke av svarene dine.</h2>
      </div>

      <div className="flex items-start justify-center gap-10 w-full">
        <MenuContainer>
          <div className="p-3 bg-white rounded-lg">
            <div className="space-y-3 capitalize">
              {selectedColumns && Object.keys(selectedColumns).length > 0 ? (
                Object.keys(selectedColumns).map((subject) => (
                  <CheckBox
                    key={subject}
                    label={subject}
                    checked={selectedColumns[subject]}
                    onChange={() => handleCheckboxChange(subject)}
                  />
                ))
              ) : (
                <p className="text-gray-500">No options available. Please select a system and file type.</p>
              )}
            </div>
          </div>
        </MenuContainer>
      </div>

      {/* navigasjonsknapper med implementert figma design */}
      <div className="absolue bottom-10 w-full px-10 flex justify-between">
        <Link href="/filtype">
          <Button
            rounded
            label="Forrige"
            className="bg-[#EAEAEA] text-black hover:bg-[#D0D0D0] active:bg-[#9D9D9D] px-4 py-2 shadow-md w-[100px] h-[32px]"
          />
        </Link>

        <Link href={isDisabled ? "#" : "/export"}>
          <Button
            rounded
            label="Neste"
            className={`px-4 py-2 shadow-md transition ${isDisabled
              ? "bg-[#DAF0DA] text-white cursor-not-allowed px-4 py-2 shadow-md w-[100px] h-[32px]"
              : "bg-[#1E721E] text-white hover:bg-[#449844] active:bg-[#075607] px-4 py-2 shadow-md w-[100px] h-[32px]"}`}
            aria-disabled={isDisabled}
          />
        </Link>
      </div>

      <footer className="mt-auto py-6">
        <Link className="text-white hover:underline" href="/home">
          Hjem
        </Link>
      </footer>
    </div>
  );
};

export default ImportVelger;