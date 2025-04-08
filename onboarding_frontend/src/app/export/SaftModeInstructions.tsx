"use client";
import { systemCoverage } from "../components/systemCoverage";
import { Button } from "primereact/button";
import React, { useState, useEffect } from "react";
import Instructions from "../components/Instructions";
import { instructionConfig } from "./InstructionConfig";
import FileUploader from "@/app/components/FileUploader";

export function SaftModeInstructions({
    system,
    checkedBoxes,
    showErrorToast, // <-- accept the callback as a prop
}: {
    system: string;
    checkedBoxes: string[];
    showErrorToast: (msg: string) => void;
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

    const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

    useEffect(() => {
        if (!selectedSubject) {
            if (covered.length > 0) {
                setSelectedSubject("safTExport");
            } else if (leftover.length > 0) {
                setSelectedSubject(leftover[0]);
            }
        }
    }, [covered, leftover, selectedSubject]);

    const safTSteps = instructionConfig[system]?.["SAF-T"] || [];

    return (
        <div className="p-6 space-y-4 relative">
            {/* Row of subject buttons */}
            <div className="flex flex-wrap gap-4 mt-4 justify-center py-5 capitalize">
                {subjectList.map((sub) => {
                    const label = sub === "safTExport" ? "SAF‐T Export" : sub;
                    return (
                        <Button // implementert figma design
                            key={sub}
                            onClick={() => setSelectedSubject(sub)}
                            className="bg-[#1E721E] text-white hover:bg-[#449844] active:bg-[#075607] px-4 py-2 h-[55px] shadow-md rounded-full text-lg font-semibold capitalize"
                        >
                            {label}
                        </Button>
                    );
                })}
            </div>

            {/* Render each subject's instructions + FileUploader */}
            {subjectList.map((sub) => {
                const isActive = sub === selectedSubject;

                let instructionsTitle = "";
                let instructionsSteps: any[] = [];
                let accept = ".csv,.xlsx"; // default

                if (sub === "safTExport") {
                    instructionsTitle = `${system} – SAF‐T Export`;
                    instructionsSteps = safTSteps;
                    accept = ".xml";
                } else {
                    instructionsTitle = `${system} – CSV – ${sub}`;
                    instructionsSteps = instructionConfig[system]?.CSV?.[sub] ?? [];
                    accept = ".csv,.xlsx";
                }

                return (
                    <div key={sub} style={{ display: isActive ? "block" : "none" }}>
                        <div className="flex flex-col items-center space-y-4 mt-4 py-1">
                            {instructionsSteps.length > 0 ? (
                                <Instructions title={instructionsTitle} steps={instructionsSteps} />
                            ) : (
                                <h4>Ingen instruksjoner funnet for {sub}.</h4>
                            )}

                            {/* Pass showErrorToast down to FileUploader */}
                            <FileUploader
                                subject={sub}
                                accept={accept}
                                onShowErrorToast={showErrorToast}
                            />
                        </div>
                    </div>
                );
            })}
        </div>
    );
}