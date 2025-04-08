"use client";
import React, { useState, useEffect, useContext, useRef } from "react";
import { Toast } from "primereact/toast";
import { SaftModeInstructions } from "./SaftModeInstructions";
import { CsvModeInstructions } from "./CsvModeInstructions";
import { Button } from "primereact/button";
import ImportContext from "../components/ImportContext";
import Link from "next/link";

export default function ExportPage() {
    const { selectedSystem } = useContext(ImportContext);
    const [checkedBoxes, setCheckedBoxes] = useState<string[]>([]);
    const fileType = "SAF-T";
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

    useEffect(() => {
        const savedCheckBoxes = localStorage.getItem("checkboxState");
        if (savedCheckBoxes) {
            const parsedCheckBoxes = JSON.parse(savedCheckBoxes);
            const selectedLabels = Object.entries(parsedCheckBoxes)
                .filter(([_, value]) => value)
                .map(([key]) => key);
            setCheckedBoxes(selectedLabels);
        }
    }, []);

    if (!selectedSystem) {
        return <h4>Vennligst velg et system først</h4>;
    }

    return (
        <div>
            <div className="flex flex-col items-center text-center mt-10">
                <h1 className="text-4xl font-bold">Last opp filene dine!</h1>
            </div>
            <div className="flex flex-col items-center text-center mt-10">
                <h2 className="text-xl">Trykk på knappene laget av importvelgeren fra trinn 3, og last opp filene helt nederst på siden.</h2>
            </div>
            <div className="flex flex-col items-center text-center mt-10">
                <h4 className="text-xl">Veiledning vil også vise hvordan du eksporterer filene fra ditt gamle regnskapssystem når du trykker på knappene.</h4>
            </div>

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

            {/* Nav buttons */}
            <div className="flex justify-between mt-10 px-10 pb-10">
                <Link href="/importvelger">
                    <Button
                        rounded
                        label="Forrige"
                        className="bg-[#EAEAEA] text-black hover:bg-[#D0D0D0] active:bg-[#9D9D9D] px-4 py-2 shadow-md w-[100px] h-[32px]"
                    />
                </Link>

                <Link href="/displaycsvexcel">
                    <Button
                        rounded
                        label="Neste"
                        className="bg-[#1E721E] text-white hover:bg-[#449844] active:bg-[#075607] px-4 py-2 shadow-md w-[100px] h-[32px]"
                    />
                </Link>
            </div>
        </div>
    );
}
