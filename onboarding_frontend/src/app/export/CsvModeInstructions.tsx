"use client";
import React, { useState, useEffect } from "react";
import Instructions from "../components/Instructions";
import { instructionConfig } from "./InstructionConfig";
import FileUploader from "@/app/components/FileUploader";
import { systemCoverage } from "../components/systemCoverage";

export function CsvModeInstructions({
                                        system,
                                        checkedBoxes,
                                        showErrorToast,
                                    }: {
    system: string;
    checkedBoxes: string[];
    showErrorToast: (msg: string) => void;
}) {
    const coverageSubjects = systemCoverage[system]?.csvSubjects || [];
    const instructionSubjects = Object.keys(instructionConfig[system]?.CSV || {});

    const allCsvSubjects = Array.from(
        new Set([...coverageSubjects, ...instructionSubjects])
    );

    const relevantSubjects = allCsvSubjects.filter((sub) =>
        checkedBoxes
            .map((s) => s.toLowerCase())
            .includes(sub.toLowerCase())
    );

    const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

    useEffect(() => {
        if (!selectedSubject && relevantSubjects.length > 0) {
            setSelectedSubject(relevantSubjects[0]);
        }
    }, [selectedSubject, relevantSubjects]);


    return (
        <div className="p-6 space-y-4">
            <h2>{system} – CSV Export</h2>

            {/* row of subject‑buttons */}
            <div className="flex flex-wrap gap-4 mt-4 justify-center capitalize">
                {relevantSubjects.map((subject) => (
                    <button
                        key={subject}
                        onClick={() => setSelectedSubject(subject)}
                        className="bg-blue-500 text-white px-6 py-3 rounded-full hover:bg-blue-600"
                    >
                        {subject}
                    </button>
                ))}
            </div>

            {/* instruction + uploader panes */}
            {relevantSubjects.map((sub) => {
                const isActive = sub === selectedSubject;
                const steps = instructionConfig[system]?.CSV?.[sub] ?? [];

                return (
                    <div key={sub} style={{ display: isActive ? "block" : "none" }}>
                        <div className="flex flex-col items-center space-y-4 mt-4 py-10">
                            {steps.length > 0 ? (
                                <Instructions
                                    title={`${system} – CSV – ${sub}`}
                                    steps={steps}
                                />
                            ) : (
                                <h4>
                                    Ingen CSV‑instruksjoner funnet for <em>{sub}</em> ennå.
                                    Last gjerne opp filen likevel.
                                </h4>
                            )}

                            <FileUploader
                                subject={sub}
                                accept=".csv,.xlsx"
                                onShowErrorToast={showErrorToast}
                            />
                        </div>
                    </div>
                );
            })}
        </div>
    );
}
