"use client";

import React, { useState, useEffect, useContext } from "react";
import ImportContext from "../components/ImportContext";
import Instructions from "../components/Instructions";
import { instructionConfig } from "./InstructionConfig";
import { systemCoverage } from "./systemCoverage";
import Link from "next/link";

export default function ExportPage() {
  const { selectedSystem } = useContext(ImportContext);

  // To be retrieved from localStorage later
  const fileType = "SAF-T";
  // const fileType = "CSV";

  // We'll store the user-checked subjects:
  const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]);

  // Prevent hydration issues by waiting until mount to read from localStorage
  const [hasMounted, setHasMounted] = useState(false);

  useEffect(() => {
    // 1) Read from localStorage
    const savedCheckBoxes = localStorage.getItem("checkboxState");
    if (savedCheckBoxes) {
      const parsedCheckBoxes = JSON.parse(savedCheckBoxes);
      const selectedLabels = Object.entries(parsedCheckBoxes)
        .filter(([_, value]) => value)
        .map(([key]) => key);
      setCheckedBoxes(selectedLabels);
    }
    // 2) Mark as mounted so we can safely render
    setHasMounted(true);
  }, []);

  if (!hasMounted) {
    // Can return a loading spinner here
    return null;
  }

  if (!selectedSystem) {
    return <p>Please pick a system first.</p>;
  }

  // If we’re in SAF-T mode, show the SAF-T instructions approach
  if (fileType === "SAF-T") {
    return <SaftModeInstructions system={selectedSystem} checkedBoxes={checkedBoxes} />;
  }
  // Otherwise, CSV mode
  else {
    return <CsvModeInstructions system={selectedSystem} checkedBoxes={checkedBoxes} />;
  }
}

/** 
 * SAF-T Mode 
 * - Finds which subjects the user selected are “covered” by SAF‐T 
 * - Everything else is leftover that must use CSV 
 * - Renders a row of buttons: one for the SAF‐T export (if coverage > 0),
 *   and one button for each leftover subject. 
 * - Clicking a button shows the appropriate instructions below.
 */
function SaftModeInstructions({
  system,
  checkedBoxes,
}: {
  system: string;
  checkedBoxes: string[];
}) {
  // 1) Which subjects does this system cover with SAF‐T?
  const coverage = systemCoverage[system]?.safTSubjects || [];

  // 2) Filter the user’s checked subjects into “covered” vs. leftover
  const covered = checkedBoxes.filter((subject) => coverage.includes(subject));
  const leftover = checkedBoxes.filter((subject) => !coverage.includes(subject));

  // 3) SAF‐T instructions array from the config
  const safTSteps = instructionConfig[system]?.["SAF-T"] || [];

  // 4) Local UI state for “which button was clicked?”
  //    - e.g. "safTExport" or a leftover subject name
  const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

  // If we have at least 1 covered subject, we can show a “SAF‐T Export” button
  const hasSaftCoverage = covered.length > 0;

  return (
    <div className="p-6 space-y-4">
      <h2 className="text-2xl font-bold">
        {system} – SAF‐T Export
      </h2>

      {covered.length > 0 ? (
        <p>SAF‐T covers these subjects: {covered.join(", ")}</p>
      ) : (
        <p>No covered subjects. (You might not have selected anything that SAF‐T can export.)</p>
      )}

      {leftover.length > 0 && (
        <p>Leftover subjects (not covered by SAF‐T): {leftover.join(", ")}</p>
      )}

      {/* Buttons row */}
      <div className="flex flex-wrap gap-2 mt-4">
        {/* If there's coverage, show a SAF‐T Export button */}
        {hasSaftCoverage && (
          <button
            onClick={() => setSelectedSubject("safTExport")}
            className="bg-teal-500 text-white px-4 py-2 rounded"
          >
            SAF‐T Export
          </button>
        )}

        {/* For each leftover subject, show a CSV button */}
        {leftover.map((subject) => (
          <button
            key={subject}
            onClick={() => setSelectedSubject(subject)}
            className="bg-blue-500 text-white px-4 py-2 rounded"
          >
            {subject}
          </button>
        ))}
      </div>

      {/* Render instructions based on selectedSubject */}
      {selectedSubject === null && (
        <p className="mt-6">Please pick a button above to see instructions.</p>
      )}

      {/* If user clicked “SAF‐T Export” */}
      {selectedSubject === "safTExport" && hasSaftCoverage && (
        <Instructions
          title={`${system} - SAF‐T Export`}
          steps={safTSteps}
        />
      )}

      {/* If user clicked a leftover subject => CSV instructions */}
      {selectedSubject &&
        selectedSubject !== "safTExport" &&
        leftover.includes(selectedSubject) && (
          <CsvInstructionsForSubject system={system} subject={selectedSubject} />
        )}

      <div className="mt-6 flex justify-end absolute bottom-4 right-4">
        <Link
          className={`px-4 py-2 rounded-md shadow-md transition bg-[#E17044] text-white hover:bg-[#c85b34]`} href="/upload"
        >
          Next
        </Link>
      </div>
      <div className="mt-6 flex justify-end absolute bottom-4 left-4">
        <Link
          className={`px-4 py-2 rounded-md shadow-md transition bg-[#E17044] text-white hover:bg-[#c85b34]`} href="/importvelger"
        >
          Previous
        </Link>
      </div>
    </div>
  );
}

/**
 * CSV Mode
 * - Simply list all CSV subjects for the system that the user *did* check 
 * - Render a button for each. Clicking one shows that subject’s CSV instructions.
 */
function CsvModeInstructions({
  system,
  checkedBoxes,
}: {
  system: string;
  checkedBoxes: string[];
}) {
  // We’ll show only the CSV keys that match user-checked subjects
  const allCsvSubjects = Object.keys(instructionConfig[system]?.CSV || {});
  const relevantSubjects = allCsvSubjects.filter((sub) =>
    checkedBoxes.map((s) => s.toLowerCase()).includes(sub.toLowerCase())
  );

  const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

  return (
    <div className="p-6 space-y-4">
      <h2 className="text-2xl font-bold">
        {system} – CSV Export
      </h2>
      <div className="flex flex-wrap gap-2 mt-4">
        {relevantSubjects.map((subject) => (
          <button
            key={subject}
            onClick={() => setSelectedSubject(subject)}
            className="bg-blue-500 text-white px-4 py-2 rounded"
          >
            {subject}
          </button>
        ))}
      </div>

      {selectedSubject ? (
        <CsvInstructionsForSubject system={system} subject={selectedSubject} />
      ) : (
        <p className="mt-4">Please pick a subject above to see instructions.</p>
      )}
    </div>
  );
}

/** Helper to render CSV instructions for exactly one subject. */
function CsvInstructionsForSubject({
  system,
  subject,
}: {
  system: string;
  subject: string;
}) {
  // For CSV instructions, we have `instructionConfig[system]["CSV"][subject]`
  const steps = instructionConfig[system]?.CSV?.[subject] || null;

  if (!steps) {
    return <p className="mt-4">No CSV instructions found for {subject}</p>;
  }
  return (
    <Instructions
      title={`${system} – CSV – ${subject}`}
      steps={steps}
    />
  );
}
