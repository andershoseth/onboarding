"use client";

import React, { useState, useEffect, useContext, useRef } from "react";
import ImportContext from "../components/ImportContext";
import Instructions from "../components/Instructions";
import { instructionConfig } from "./InstructionConfig";
import { systemCoverage } from "./systemCoverage";
import Link from "next/link";
import { Toast } from "primereact/toast";
import FileUploader from "@/app/components/FileUploader";
import { Button } from "primereact/button";

function SaftModeInstructions({
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
        <div className="p-6 space-y-4 relative capitalize">
            {/* Row of subject buttons */}
            <div className="flex flex-wrap gap-4 mt-4 justify-center py-10 capitalize">
                {subjectList.map((sub) => {
                    const label = sub === "safTExport" ? "SAF‐T Export" : sub;
                    return (
                        <Button // implementert figma design
                            key={sub}
                            onClick={() => setSelectedSubject(sub)}
                            className="bg-[#1E721E] text-white hover:bg-[#449844] active:bg-[#075607] px-6 py-3 rounded-full text-lg font-semibold capitalize"
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
                        <div className="flex flex-col items-center space-y-4 mt-4 py-10">
                            {instructionsSteps.length > 0 ? (
                                <Instructions title={instructionsTitle} steps={instructionsSteps} />
                            ) : (
                                <p>No instructions found for {sub}.</p>
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

            {/* Nav buttons */}
            <div className="mt-6 flex justify-end absolute bottom-4 right-4"> {/* implementert figma design */}
                <Link href="/displaycsvexcel"
                >
                    <Button
                        rounded
                        label="Next"
                        className="bg-[#1E721E] text-white hover:bg-[#449844] active:bg-[#075607] px-4 py-2 shadow-md transition"
                    />
                </Link>
            </div>
            <div className="mt-6 flex justify-end absolute bottom-4 left-4">
                <Link href="/importvelger"
                >
                    <Button
                        rounded
                        label="Previous"
                        className="bg-[#EAEAEA] text-black hover:bg-[#D0D0D0] active:bg-[#9D9D9D] px-4 py-2 shadow-md transition"
                    />
                </Link>
            </div>
        </div>
    );
}

function CsvModeInstructions({
    system,
    checkedBoxes,
    showErrorToast,
}: {
    system: string;
    checkedBoxes: string[];
    showErrorToast: (msg: string) => void;
}) {
    const allCsvSubjects = Object.keys(instructionConfig[system]?.CSV || {});
    const relevantSubjects = allCsvSubjects.filter((sub) =>
        checkedBoxes.map((s) => s.toLowerCase()).includes(sub.toLowerCase())
    );

    const [selectedSubject, setSelectedSubject] = useState<string | null>(null);

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

            {/* Always render a pane for each relevant CSV subject. */}
            {relevantSubjects.map((sub) => {
                const isActive = sub === selectedSubject;
                const steps = instructionConfig[system]?.CSV?.[sub];

                return (
                    <div key={sub} style={{ display: isActive ? "block" : "none" }}>
                        <div className="flex flex-col items-center space-y-4 mt-4 py-10">
                            {steps ? (
                                <Instructions title={`${system} – CSV – ${sub}`} steps={steps} />
                            ) : (
                                <p>No CSV instructions found for {sub}</p>
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

export default function ExportPage() {
    const { selectedSystem } = useContext(ImportContext);

    // 1) Create a ref for the Toast
    const toastRef = useRef<Toast>(null);

    // 2) Function to show error messages
    const showErrorToast = (msg: string) => {
        toastRef.current?.show({
            severity: "error",
            summary: "Upload Error",
            detail: msg,
            life: 10000,
        });
    };

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

    return (
        <div>
            {/* 3) Render the Toast once at the top level */}
            <Toast ref={toastRef} />

            {fileType === "SAF-T" ? (
                <SaftModeInstructions
                    system={selectedSystem}
                    checkedBoxes={checkedBoxes}
                    showErrorToast={showErrorToast} // pass callback down
                />
            ) : (
                <CsvModeInstructions
                    system={selectedSystem}
                    checkedBoxes={checkedBoxes}
                    showErrorToast={showErrorToast} // pass callback down
                />
            )}
        </div>
    );
}
