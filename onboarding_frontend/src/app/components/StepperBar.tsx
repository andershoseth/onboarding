"use client";
import React, { useContext } from "react";
import ImportContext from "../components/ImportContext";


const Sidebar: React.FC = () => {
  // Access context values
  const { selectedSystem, fileName, selectedColumns } = useContext(ImportContext);

  // Check if any columns are selected
  const isColumnsSelected = Object.values(selectedColumns).some(Boolean); // Check if any value is true

  // Utility function to render numbered circles
  const renderCircle = (stepNumber: number, conditionMet: boolean) => {
    return (
      <div
        className={`w-8 h-8 rounded-full flex items-center justify-center text-white font-bold ${
          conditionMet ? "bg-green-500" : "bg-white border-2 border-gray-500"
        }`}
      >
        <span className={`text-lg ${conditionMet ? "text-white" : "text-black"}`}>
          {stepNumber}
        </span>
      </div>
    );
  };

  return (
    <div className="fixed top-0 left-0 w-64 h-full bg-gray-800 text-white pt-10">
      <ul className="space-y-4">
        <div className="sidebar-text text-center font-bold text-xl">
          Onboarding veiledning
        </div>
        <li className="flex items-center space-x-4">
          {renderCircle(1, selectedSystem !== null)} {/* Check if a system is selected */}
          <a
            href="/systemvalg"
            className="block px-4 py-2 text-lg hover:bg-gray-700 text-center"
          >
            Systemvalg
          </a>
        </li>
        <li className="flex items-center space-x-4">
          {renderCircle(2, isColumnsSelected)} {/* Check if a system is selected and any columns are selected */}
          <a
            href="/importvelger"
            className="block px-4 py-2 text-lg hover:bg-gray-700 text-center"
          >
            Importvelger
          </a>
        </li>
        <li className="flex items-center space-x-4">
          {renderCircle(3, fileName !== null)} {/* Check if a file is uploaded */}
          <a
            href="/upload"
            className="block px-4 py-2 text-lg hover:bg-gray-700 text-center"
          >
            Last opp
          </a>
        </li>
        <li className="flex items-center space-x-4">
          {renderCircle(4, fileName !== null && isColumnsSelected)} {/* Check if file is uploaded and columns are selected */}
          <a
            href="/export"
            className="block px-4 py-2 text-lg hover:bg-gray-700 text-center"
          >
            Export
          </a>
        </li>
      </ul>
    </div>
  );
};

export default Sidebar;
