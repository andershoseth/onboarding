"use client";

import React, { useState, useEffect, useContext } from "react";
import ImportContext from "../components/ImportContext";
import Instructions from "../components/Instructions";
import { instructionConfig } from "./InstructionConfig";
import { systemCoverage } from "./systemCoverage";
import Link from "next/link";
import FileUploader from "@/app/components/FileUploader";

// The data for the instructions is stored in instructionConfig.tsx at the same level as this page
// systemCoverage.tsx is used to determine which subjects are covered by SAF-T for each system
// eg. for Visma, if safTSubjects is set to "hovedboktransaksjoner", "avdeling"
// the page will not render the buttons for these pages, as they are covered by SAF-T
// subjects are retrieved from the checked boxes in importvelger

export default function ExportPage() {
    const { selectedSystem } = useContext(ImportContext);

    // Hard coded, get from state/localStorage when the filetype selector is implemented
    const fileType = "SAF-T";
    // const fileType = "CSV";

    const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]);
    const [hasMounted, setHasMounted] = useState(false);

    useEffect(() => {
        // 1) Read the user's checked boxes from localStorage (client only)
        const savedCheckBoxes = localStorage.getItem("checkboxState");
        if (savedCheckBoxes) {
            const parsedCheckBoxes = JSON.parse(savedCheckBoxes);
            const selectedLabels = Object.entries(parsedCheckBoxes)
                .filter(([_, value]) => value)
                .map(([key]) => key);
            setCheckedBoxes(selectedLabels);
        }
        // 2) Mark that we've mounted so we can safely render
        setHasMounted(true);
    }, []);

    // Avoid rendering until mounted (to prevent hydration mismatch)
    if (!hasMounted) return null;

    // If user hasn't selected a system at all, show a fallback
    if (!selectedSystem) {
        return <p>Please pick a system first.</p>;
    }

    // Show either SAF-T mode or CSV mode instructions
    return fileType === "SAF-T" ? (
        <SaftModeInstructions system={selectedSystem} checkedBoxes={checkedBoxes} />
    ) : (
        <CsvModeInstructions system={selectedSystem} checkedBoxes={checkedBoxes} />
    );
}

/**
 * SAF-T Mode
 * 1. Figures out which checked subjects are automatically covered by SAF-T
 * 2. Anything leftover must be CSV
 * 3. Renders a row of buttons: one for "SAF-T Export" (if coverage > 0), plus leftover CSV subjects
 * 4. Auto-selects "SAF-T Export" if available; otherwise the first leftover
 */
function SaftModeInstructions({
                                  system,
                                  checkedBoxes,
                              }: {
    system: string;
    checkedBoxes: string[];
}) {
    // Which subjects are covered?
    const coverage = systemCoverage[system]?.safTSubjects || [];
    const covered = checkedBoxes.filter((subject) => coverage.includes(subject));
    const leftover = checkedBoxes.filter((subject) => !coverage.includes(subject));

    // SAF-T steps (one array, typically)
    const safTSteps = instructionConfig[system]?.["SAF-T"] || [];
    const hasSaftCoverage = covered.length > 0;

    // Which "subject" is currently selected (could be "safTExport" or a leftover subject name)
    const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

    // Auto-select a default subject once we know coverage/leftover
    useEffect(() => {
        if (selectedSubject == null) {
            if (hasSaftCoverage) {
                // If there's any coverage, auto-select the SAF-T button
                setSelectedSubject("safTExport");
            } else if (leftover.length > 0) {
                // Otherwise, pick the first leftover subject if it exists
                setSelectedSubject(leftover[0]);
            }
        }
    }, [selectedSubject, hasSaftCoverage, leftover]);

    return (
        <div className="p-6 space-y-4 relative capitalize">
            {/*
        Centered row of buttons:
          - One "SAF-T Export" if we have coverage
          - Buttons for leftover CSV subjects
      */}
            <SubjectButtonRow
                hasSaftCoverage={hasSaftCoverage}
                leftover={leftover}
                onSubjectSelect={setSelectedSubject}
            />

            {/* Render instructions depending on the selectedSubject */}
            {selectedSubject === null && (
                <p className="mt-6">Please pick a button above to see instructions.</p>
            )}

            {/* SAF-T instructions if we clicked "SAF-T Export" */}
            {selectedSubject === "safTExport" && hasSaftCoverage && (
                <div className="flex flex-col items-center space-y-4 mt-4 py-10">
                    <Instructions title={`${system} – SAF‐T Export`} steps={safTSteps} />

                    {/* SAF‐T File Uploader */}
                    <FileUploader subject="safTExport" accept=".xml" />
                </div>
            )}

            {/* If the user clicked a leftover subject => show CSV instructions */}
            {selectedSubject &&
                selectedSubject !== "safTExport" &&
                leftover.includes(selectedSubject) && (
                    <CsvInstructionsForSubject system={system} subject={selectedSubject} />
                )}

            {/* Bottom Nav Buttons */}
            <div className="mt-6 flex justify-end absolute bottom-4 right-4">
                <Link
                    className="px-4 py-2 rounded-md shadow-md transition bg-[#E17044] text-white hover:bg-[#c85b34]"
                    href="/upload"
                >
                    Next
                </Link>
            </div>
            <div className="mt-6 flex justify-end absolute bottom-4 right-28">
                <Link
                    className="px-4 py-2 rounded-md shadow-md transition bg-[#E17044] text-white hover:bg-[#c85b34]"
                    href="/uploadtest"
                >
                    Test
                </Link>
            </div>
            <div className="mt-6 flex justify-end absolute bottom-4 left-4">
                <Link
                    className="px-4 py-2 rounded-md shadow-md transition bg-[#E17044] text-white hover:bg-[#c85b34]"
                    href="/importvelger"
                >
                    Previous
                </Link>
            </div>
            <div className="mt-6 flex justify-end absolute bottom-4 right-52">
                <Link
                    className="px-4 py-2 rounded-md shadow-md transition bg-[#E17044] text-white hover:bg-[#c85b34]"
                    href="/displaycsvexcel"
                >
                    View Uploaded Data
                </Link>
            </div>

        </div>
    );
}

/**
 * Button row for SAF-T mode:
 * - If coverage, show one "SAF-T Export" button
 * - Then one button per leftover CSV subject
 * - All are pill-shaped and wrap to new lines if needed
 */
function SubjectButtonRow({
                              hasSaftCoverage,
                              leftover,
                              onSubjectSelect,
                          }: {
    hasSaftCoverage: boolean;
    leftover: string[];
    onSubjectSelect: (subjectKey: string) => void;
}) {
    return (
        // py to avoid topbar, change it when topbar is fixed
        <div className="flex flex-wrap gap-4 mt-4 justify-center py-10 capitalize">
            {/* SAF‐T button */}
            {hasSaftCoverage && (
                <button
                    onClick={() => onSubjectSelect("safTExport")}
                    className="
            bg-orange-600 text-white
            px-6 py-3
            rounded-full
            text-lg font-semibold
            hover:bg-orange-700
          "
                >
                    SAF‐T Export
                </button>
            )}

            {/* Leftover CSV subjects */}
            {leftover.map((subject) => (
                <button
                    key={subject}
                    onClick={() => onSubjectSelect(subject)}
                    className="
            bg-orange-600 text-white
            px-6 py-3
            rounded-full
            text-lg font-semibold
            hover:bg-orange-700
            capitalize
          "
                >
                    {subject}
                </button>
            ))}
        </div>
    );
}

/**
 * CSV Mode:
 * - Builds a list of relevant CSV subjects (intersection of what's in the config + what's checked)
 * - Renders a row of pill-shaped buttons
 * - Auto-selects the first subject on load
 */
function CsvModeInstructions({
                                 system,
                                 checkedBoxes,
                             }: {
    system: string;
    checkedBoxes: string[];
}) {
    const allCsvSubjects = Object.keys(instructionConfig[system]?.CSV || {});
    const relevantSubjects = allCsvSubjects.filter((sub) =>
        checkedBoxes.map((s) => s.toLowerCase()).includes(sub.toLowerCase())
    );

    const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

    // Auto-select the first relevant CSV subject if any
    useEffect(() => {
        if (!selectedSubject && relevantSubjects.length > 0) {
            setSelectedSubject(relevantSubjects[0]);
        }
    }, [selectedSubject, relevantSubjects]);

    return (
        <div className="p-6 space-y-4">
            <h2 className="text-2xl font-bold">{system} – CSV Export</h2>

            {/* Centered row of CSV subjects */}
            <div className="flex flex-wrap gap-4 mt-4 justify-center">
                {relevantSubjects.map((subject) => (
                    <button
                        key={subject}
                        onClick={() => setSelectedSubject(subject)}
                        className="
              bg-blue-500 text-white
              px-6 py-3
              rounded-full
              text-lg font-semibold
              hover:bg-blue-600
            "
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
function CsvInstructionsForSubject({ system, subject }: { system: string; subject: string }) {
    const steps = instructionConfig[system]?.CSV?.[subject];

    return (
        <div className="flex flex-col items-center space-y-4 mt-4 py-10">
            {steps ? (
                <Instructions title={`${system} – CSV – ${subject}`} steps={steps} />
            ) : (
                <p>No CSV instructions found for {subject}</p>
            )}

            {/* File Uploader will render regardless of whether we have instructions */}
            <FileUploader subject={subject} accept=".csv,.xlsx" />
        </div>
    );
}
