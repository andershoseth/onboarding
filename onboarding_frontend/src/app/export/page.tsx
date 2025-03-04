"use client";

import React, { useContext, useState, useEffect } from "react";
import ImportContext from "../components/ImportContext";
import { instructionConfig } from "./InstructionConfig";
import Instructions from "../components/Instructions";

export default function ExportPage() {
  const { selectedSystem } = useContext(ImportContext);

  const [hasMounted, setHasMounted] = useState(false);
  const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]);

  // Hard-coded for demonstration
  const fileType = "SAF-T";

  const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

  useEffect(() => {
    setHasMounted(true);

    // const userChecked = ["Kontakter", "Avdeling", "Saldobalanse"];
    // setCheckedBoxes(userChecked);


    // Get checkbox state, this is all lowercase
    const savedCheckBoxes = localStorage.getItem("checkboxState"); //saves the checked boxes in the array
    if (savedCheckBoxes) {
      const parsedCheckBoxes = JSON.parse(savedCheckBoxes);
      const selectedLabels = Object.entries(parsedCheckBoxes)
        .filter(([_, value]) => value)
        .map(([key]) => key);
      setCheckedBoxes(selectedLabels);
    }

  }, []);

  if (!hasMounted) {
    return null; // loading spinner could be added here
  }

  return (
    <div>
      <div className="py-20">
        <p>Selected system: {selectedSystem}</p>
        <p>Selected checkboxes: {checkedBoxes.join(", ")}</p>
      </div>

      <div className="my-4 flex flex-wrap gap-2 justify-center top-40">
        {checkedBoxes.map((checkboxValue) => (
          <button
            key={checkboxValue}
            onClick={() => setSelectedSubject(checkboxValue)}
            className="px-4 py-2 bg-blue-500 text-white rounded"
          >
            {checkboxValue}
          </button>
        ))}
      </div>

      <InstructionsRenderer
        system={selectedSystem}
        fileType={fileType}
        subject={selectedSubject}
      />
    </div>
  );
}


function InstructionsRenderer({
  system,
  fileType,
  subject
}: {
  system: string | null;
  fileType: string;
  subject: string | null;
}) {
  // If we don't have a system or subject selected, show a prompt or fallback:
  if (!system) {
    return <p>No system selected.</p>;
  }
  if (!subject) {
    return <p>Please click one of the subject buttons above.</p>;
  }

  // 1) get top-level instructions for the system
  const systemConfig = instructionConfig[system];

  if (!systemConfig) {
    return <p>No instructions exist for system: {system}</p>;
  }

  // 2) get instructions for the chosen fileType
  // systemConfig[fileType] could be an array (if SAF-T) or an object (if CSV)
  const fileTypeConfig = systemConfig[fileType];
  if (!fileTypeConfig) {
    return (
      <p>
        No instructions for fileType "{fileType}" in {system}
      </p>
    );
  }

  // If it's an array (like for SAF-T in your config), that means there's
  // not a sub-key for subject (the user might not need to pick "Hovedbok...").
  // But if it's an object (CSV scenario), we do a second-level lookup for subject.
  if (Array.isArray(fileTypeConfig)) {
    // Means SAF-T scenario
    // Just show the entire array
    return (
      <Instructions
        title={`${system} - ${fileType}`}
        steps={fileTypeConfig}
      />
    );
  } else {
    // Means CSV scenario (or anything where we have sub-keys)
    // So fileTypeConfig might look like { "Kontakter": { heading: "...", ... }, "Hovedbok...": {...} }
    // We do fileTypeConfig[subject] to see if there's a match.
    const subjectInstructions = fileTypeConfig[subject];
    if (!subjectInstructions) {
      return (
        <p>No instructions for subject "{subject}" in {system} - {fileType}</p>
      );
    }

    // subjectInstructions in your config is an object, but <Instructions /> wants an array.
    // So if you only have a single step object, you can wrap it in an array.
    // Or if you want multiple steps, your config can just be an array in the first place.
    const steps = Array.isArray(subjectInstructions)
      ? subjectInstructions
      : [subjectInstructions];

    return (
      <Instructions
        title={`${system} - ${fileType} - ${subject}`}
        steps={steps}
      />
    );
  }
}