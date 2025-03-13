"use client";
import React, { useContext } from "react";
import ImportContext from "../components/ImportContext";

const Sidebar: React.FC = () => {
  const { selectedSystem, fileName, selectedColumns, selectedFileType } = useContext(ImportContext);

  // Might change this, since the circle is not "completed" until an action is taken, NOT if anything is saved
  const isColumnsSelected = Object.values(selectedColumns).some(Boolean);

  const renderCircle = (stepNumber: number, conditionMet: boolean) => {
    return (
      <div
        className={`w-10 h-10 rounded-full flex items-center justify-center text-white font-bold ${
          conditionMet ? "bg-green-500" : "bg-white border-2 border-gray-500 text-black"
        }`}
      >
        <span className={`text-lg ${conditionMet ? "text-white" : "text-black"}`}>
          {stepNumber}
        </span>
      </div>
    );
  };

  return (
    <div className="fixed top-0 left-0 w-64 h-full bg-gray-800 text-white pt-10 flex flex-col items-start pl-6">
      <div className="text-center font-bold text-xl mb-6">
        Onboarding veiledning
      </div>
      <ul className="space-y-6 w-full">
        {/* Step 1 */}
        <li className="flex items-center space-x-4">
          {renderCircle(1, selectedSystem !== null)}
          <a href="/systemvalg" className="text-lg hover:underline">
            Systemvalg
          </a>
        </li>

        {/* Step 2 */}
        <li className="flex items-center space-x-4">
          {renderCircle(2, selectedFileType !== null)}
          <a href="/filtype" className="text-lg hover:underline">
            Filtype
          </a>
        </li>

        {/* Step 3 */}
        <li className="flex items-center space-x-4">
          {renderCircle(3, isColumnsSelected)}
          <a href="/importvelger" className="text-lg hover:underline">
            Importvelger
          </a>
        </li>

        {/* Step 4 */}
        <li className="flex items-center space-x-4">
          {renderCircle(4, fileName !== null && isColumnsSelected)}
          <a href="/export" className="text-lg hover:underline">
            Instrukser
          </a>
        </li>

        {/* Step 5 */}
        <li className="flex items-center space-x-4">
          {renderCircle(5, fileName !== null)}
          <a href="/upload" className="text-lg hover:underline">
            Last opp
          </a>
        </li>
      </ul>
    </div>
  );
};

export default Sidebar;
