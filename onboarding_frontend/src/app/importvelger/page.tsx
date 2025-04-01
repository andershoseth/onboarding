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
    <div className="flex flex-col items-center min-h-screen p-10">
      <div className="flex items-start justify-center gap-10">
        <MenuContainer>
          <div className="p-6 bg-white rounded-lg">
            <h2 className="text-xl font-semibold text-[#E17044]">Hva ønsker du å importere?</h2>
            <p className="text-gray-600 text-sm mb-4">Huk av hva du vil importere fra filene dine.</p>

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

            <div className="mt-6 flex justify-between"> {/* implementert figma design */}
              <Link href="/filtype">
                <Button
                  rounded
                  label="Previous"
                  className="px-4 py-2 shadow-md transition bg-[#EAEAEA] text-black hover:bg-[#D0D0D0] active:bg-[#9D9D9D]"
                />
              </Link>

              <Link href={isDisabled ? "#" : "/export"}>
                <Button
                  rounded
                  label="Next"
                  className={`px-4 py-2 shadow-md transition ${isDisabled
                    ? "bg-[#DAF0DA] text-white cursor-not-allowed"
                    : "bg-[#1E721E] text-white hover:bg-[#449844] active:bg-[#075607]"}`}
                  aria-disabled={isDisabled}
                />
              </Link>
            </div>
          </div>
        </MenuContainer>
      </div>

      <footer className="mt-auto py-6">
        <Link className="text-white hover:underline" href="/home">
          Home
        </Link>
      </footer>
    </div>
  );
};

export default ImportVelger;