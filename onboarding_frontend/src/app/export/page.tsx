"use client";
import React, { useState, useContext, useEffect } from "react";
import ImportContext from "../components/ImportContext";
import { instructionConfig } from "./InstructionConfig";
import { systemCoverage } from "./systemCoverage";
import Instructions from "../components/Instructions";

export default function ExportPage() {
  const { selectedSystem } = useContext(ImportContext);

  // const fileType = "CSV"; // or "SAF-T"
  const fileType = "SAF-T"; // or "CSV"
  const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]);
  const [hasMounted, setHasMounted] = useState(false);

  useEffect(() => {
    // Read from localStorage in client only
    const savedCheckBoxes = localStorage.getItem("checkboxState");
    if (savedCheckBoxes) {
      const parsedCheckBoxes = JSON.parse(savedCheckBoxes);
      const selectedLabels = Object.entries(parsedCheckBoxes)
        .filter(([_, value]) => value)
        .map(([key]) => key);
      setCheckedBoxes(selectedLabels);
    }

    setHasMounted(true);
  }, []);

  // Avoid rendering until localStorage is read, prevents hydration mismatch
  if (!hasMounted) {
    // Return a loading placeholder or just null
    return null;
  }

  if (!selectedSystem) {
    return <p>Please pick a system first.</p>;
  }

  // Render instructions based on the selected system and file type
  if (fileType === "SAF-T") {
    return <SaftModeInstructions system={selectedSystem} checkedBoxes={checkedBoxes} />;
  } else {
    return <CsvModeInstructions system={selectedSystem} checkedBoxes={checkedBoxes} />;
  }
}

function SaftModeInstructions({ system, checkedBoxes }: { system: string; checkedBoxes: string[] }) {
  const coverage = systemCoverage[system]?.safTSubjects || [];
  const covered = checkedBoxes.filter(cb => coverage.includes(cb));
  const leftover = checkedBoxes.filter(cb => !coverage.includes(cb));
  // SAF-T needs to be an array, even if there's only one step
  const safTSteps = instructionConfig[system]?.["SAF-T"] || [];

  return (
    <div className="p-10">
      <Instructions title={`${system} - SAF-T Export`} steps={safTSteps} />
      <p>SAF-T covers the following subjects automatically: {covered.join(", ")}</p>
      {leftover.length > 0 && (
        <div>
          <p>These subjects are not covered by SAF-T: {leftover.join(", ")}</p>
          <p>You must do a CSV export for them.</p>
          {leftover.map(subject => {
            const steps = instructionConfig[system]?.["CSV"]?.[subject] || null;
            if (!steps) {
              return <p key={subject}>No CSV instructions found for {subject}</p>;
            }
            return (
              <Instructions
                key={subject}
                title={`${system} - CSV Export - ${subject}`}
                steps={steps}
              />
            );
          })}
        </div>
      )}
    </div>
  );
}

function CsvModeInstructions({ system, checkedBoxes }: { system: string; checkedBoxes: string[] }) {
  return (
    <div>
      {checkedBoxes.map(subject => {
        const steps = instructionConfig[system]?.["CSV"]?.[subject] || null;
        if (!steps) {
          return <p key={subject}>No CSV instructions for {subject}</p>;
        }
        return (
          <Instructions
            key={subject}
            title={`${system} - CSV Export - ${subject}`}
            steps={steps}
          />
        );
      })}
    </div>
  );
}
