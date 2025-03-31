"use client";

import React, { useState, useEffect, useContext } from "react";
import ImportContext from "../components/ImportContext";
import Instructions from "../components/Instructions";
import { instructionConfig } from "./InstructionConfig";
import { systemCoverage } from "./systemCoverage";
import Link from "next/link";
import FileUploader from "@/app/components/FileUploader";

/**
 * SAF-T Mode:
 * 1) Figure out which subjects are “covered by SAF-T” => we represent them with a single subject “safTExport”
 * 2) Whatever is leftover is CSV
 * 3) We show a row of “buttons” to pick the active subject, but we actually mount all subject sections.
 */
function SaftModeInstructions({
                                  system,
                                  checkedBoxes,
                              }: {
    system: string;
    checkedBoxes: string[];
}) {
    // Which subjects are covered by SAF-T in this system?
    const coverage = systemCoverage[system]?.safTSubjects || [];
    const covered = checkedBoxes.filter((subject) => coverage.includes(subject));
    const leftover = checkedBoxes.filter((subject) => !coverage.includes(subject));

    // We consider “safTExport” if coverage has anything
    const subjectList = [...leftover];
    if (covered.length > 0) {
        subjectList.unshift("safTExport");
    }

    // Let user pick which subject is “active” on screen
    const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

    // On first load, auto-select something
    useEffect(() => {
        if (!selectedSubject) {
            if (covered.length > 0) {
                setSelectedSubject("safTExport");
            } else if (leftover.length > 0) {
                setSelectedSubject(leftover[0]);
            }
        }
    }, [covered, leftover, selectedSubject]);

    // Get the instructions for SAF-T.
    // We’ll just do “safTSteps” = instructionConfig[system]?.["SAF-T"]
    const safTSteps = instructionConfig[system]?.["SAF-T"] || [];

    return (
        <div className="p-6 space-y-4 relative capitalize">
            {/* Row of subject buttons */}
            <div className="flex flex-wrap gap-4 mt-4 justify-center py-10 capitalize">
                {subjectList.map((sub) => {
                    const label = sub === "safTExport" ? "SAF‐T Export" : sub;
                    return (
                        <button
                            key={sub}
                            onClick={() => setSelectedSubject(sub)}
                            className="bg-orange-600 text-white px-6 py-3 rounded-full text-lg font-semibold hover:bg-orange-700 capitalize"
                        >
                            {label}
                        </button>
                    );
                })}
            </div>
            {/* Render each page, but only show the actiave one */}
            {subjectList.map((sub) => {
                const isActive = sub === selectedSubject;

                // Get instructions for the active subject
                let instructionsTitle = "";
                let instructionsSteps: any[] = [];
                let accept = ".csv,.xlsx"; // default
                // Edgecase for SAF-T
                if (sub === "safTExport") {
                    instructionsTitle = `${system} – SAF‐T Export`;
                    instructionsSteps = safTSteps;
                    accept = ".xml";
                } else {
                    // Not SAF-T so it's CSV/Excel
                    instructionsTitle = `${system} – CSV – ${sub}`;
                    instructionsSteps =
                        instructionConfig[system]?.CSV?.[sub] ?? [];
                    accept = ".csv,.xlsx";
                }

                return (
                    <div key={sub} style={{ display: isActive ? "block" : "none" }}>
                        <div className="flex flex-col items-center space-y-4 mt-4 py-10">
                            {/* Instructions */}
                            {instructionsSteps.length > 0 ? (
                                <Instructions title={instructionsTitle} steps={instructionsSteps} />
                            ) : (
                                <p>No instructions found for {sub}.</p>
                            )}

                            {/* Each subject has its own FileUploader instance in advanced mode */}
                            <FileUploader subject={sub} accept={accept} />
                        </div>
                    </div>
                );
            })}

            {/* Nav buttons */}
            <div className="mt-6 flex justify-end absolute bottom-4 right-4">
                <Link
                    className="px-4 py-2 rounded-md shadow-md transition bg-[#E17044] text-white hover:bg-[#c85b34]"
                    href="/displaycsvexcel"
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
        </div>
    );
}

/**
 * CSV Mode Example
 * You can do the exact same approach:
 * Always mount all leftover subjects, hide with display:none,
 * so each subject’s queue remains.
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
        checkedBoxes
            .map((s) => s.toLowerCase())
            .includes(sub.toLowerCase())
    );

    const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

    // Pick a default
    useEffect(() => {
        if (!selectedSubject && relevantSubjects.length > 0) {
            setSelectedSubject(relevantSubjects[0]);
        }
    }, [selectedSubject, relevantSubjects]);

    return (
        <div className="p-6 space-y-4">
            <h2 className="text-2xl font-bold">{system} – CSV Export</h2>

            {/* Row of CSV subjects */}
            <div className="flex flex-wrap gap-4 mt-4 justify-center">
                {relevantSubjects.map((subject) => (
                    <button
                        key={subject}
                        onClick={() => setSelectedSubject(subject)}
                        className="bg-blue-500 text-white px-6 py-3 rounded-full text-lg font-semibold hover:bg-blue-600"
                    >
                        {subject}
                    </button>
                ))}
            </div>

            {/* Always render a pane for each relevant CSV subject. Hide with style if not selected. */}
            {relevantSubjects.map((sub) => {
                const isActive = sub === selectedSubject;
                const steps = instructionConfig[system]?.CSV?.[sub];

                return (
                    <div key={sub} style={{ display: isActive ? "block" : "none" }}>
                        <div className="flex flex-col items-center space-y-4 mt-4 py-10">
                            {steps ? (
                                <Instructions
                                    title={`${system} – CSV – ${sub}`}
                                    steps={steps}
                                />
                            ) : (
                                <p>No CSV instructions found for {sub}</p>
                            )}

                            <FileUploader subject={sub} accept=".csv,.xlsx" />
                        </div>
                    </div>
                );
            })}
        </div>
    );
}

export default function ExportPage() {
    const { selectedSystem } = useContext(ImportContext);

    // Hard coded for demonstration
    const fileType = "SAF-T";
    // const fileType = "CSV";

    const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]);
    const [hasMounted, setHasMounted] = useState(false);

    useEffect(() => {
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

    if (!hasMounted) return null;
    if (!selectedSystem) {
        return <p>Please pick a system first.</p>;
    }

    return fileType === "SAF-T" ? (
        <SaftModeInstructions
            system={selectedSystem}
            checkedBoxes={checkedBoxes}
        />
    ) : (
        <CsvModeInstructions
            system={selectedSystem}
            checkedBoxes={checkedBoxes}
        />
    );
}
