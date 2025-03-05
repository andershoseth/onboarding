'use client'
import React, { useContext } from "react";
import { useBoxState } from "../components/MenuContainer";
import CheckBox from "../components/CheckBox";
import ImportContext from "../components/ImportContext";
import Link from "next/link";


interface BoxState {
  kontakter: boolean;
  avdeling: boolean;
  saldobalanse: boolean;
}

const ImportVelger: React.FC = () => {
  const { selected, handleBoxChange } = useBoxState(); 
  const isDisabled = !Object.values(selected).some(Boolean);
  const { setSelectedColumns } = useContext(ImportContext); // Using context for sidebar recomposition

  // Updated this with correct typing to avoid "any" error
  const handleCheckboxChange = (column: keyof BoxState) => {
    // checking column name in boxstate
    const updatedColumns = { ...selected, [column]: !selected[column] }; //creating a copy to avoid triggering any false renders

    handleBoxChange(column);  // Updating local state
    setSelectedColumns(updatedColumns); // Updating global context state too 
  };

  return (
    <div className="flex flex-col items-center min-h-screen p-10">
      <div className="flex items-start justify-center gap-10">
        <div className="p-6 bg-gray-100 rounded-lg shadow-sm w-96">
          <h2 className="text-xl font-semibold text-[#E17044]">Hva ønsker du å importere?</h2>
          <p className="text-gray-600 text-sm mb-4">Huk av hva du vil importere fra filene dine.</p>

          <div className="space-y-3">
            <CheckBox label="Kontakter" checked={selected.kontakter} onChange={() => handleCheckboxChange("kontakter")} />
            <CheckBox label="Avdeling" checked={selected.avdeling} onChange={() => handleCheckboxChange("avdeling")} />
            <CheckBox label="Saldobalanse" checked={selected.saldobalanse} onChange={() => handleCheckboxChange("saldobalanse")} />
          </div>

          <div className="mt-6 flex justify-end">
            <Link
              href={isDisabled ? "#" : "/export"}
              className={`px-4 py-2 rounded-md shadow-md transition ${isDisabled ? "bg-gray-400 text-gray-200 cursor-not-allowed" : "bg-[#E17044] text-white hover:bg-[#c85b34]"} `}
              aria-disabled={isDisabled}
            >
              Next
            </Link>
          </div>
        </div>
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
