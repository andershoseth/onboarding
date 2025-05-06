"use client";
import React, { useContext, useState } from "react";
import ImportContext from "../components/ImportContext";
import { subjectBundles } from "./subjectBundles";
import CheckBox from "./CheckBox";
import Link from "next/link";
import MenuContainer from "./MenuContainer";
import { Button } from "primereact/button";

const ImportVelger: React.FC = () => {
    const { selectedColumns, setSelectedColumns, selectedSystem } =
        useContext(ImportContext);

    const [advanced, setAdvanced] = useState(false);
    const bundlesForSystem =
        subjectBundles[selectedSystem as keyof typeof subjectBundles] || {};

    // ---------- helpers --------------------------------------------------
    const toggleSingle = (subject: string) =>
        setSelectedColumns((prev) => ({ ...prev, [subject]: !prev[subject] }));

    const toggleBundle = (bundleName: string) => {
        const subjects = bundlesForSystem[bundleName];

        setSelectedColumns(prev => {
            // Is this bundle already fully selected?
            const alreadySelected = subjects.every(s => prev[s]);

            // Start everything as unchecked
            const next: Record<string, boolean> = Object.fromEntries(
                Object.keys(prev).map(k => [k, false])
            );

            // Tick only the subjects in this bundle
            if (!alreadySelected) {
                subjects.forEach(s => (next[s] = true));
            }
            return next;
        });
    };

    const bundleState = (bundleName: string) => {
        const subs = bundlesForSystem[bundleName];                  // subjects in *this* bundle
        const selected = Object.entries(selectedColumns)            // every subject currently selected
            .filter(([, v]) => v)
            .map(([s]) => s);

        const onlyMineSelected = selected.every(s => subs.includes(s));
        const allMine = subs.every(s => selectedColumns[s]);

        const checked = allMine && onlyMineSelected;

        const indeterminate =
            !checked &&
            selected.some(s => subs.includes(s)) &&
            onlyMineSelected;

        return { checked, indeterminate };
    };

    const isDisabled = !Object.values(selectedColumns || {}).some(Boolean);
    // ---------------------------------------------------------------------

    return (
        <div className="flex flex-col min-h-screen p-6 md:p-10">
            <div className="felx flex-col items-center flex-grow">
                {/* ---------- heading ------------------------------------------------ */}
                <h1 className="text-4xl font-bold text-center">
                    Hva ønsker du å importere fra filene?
                </h1>
                <h2 className="text-xl text-center mt-5">
                    {advanced
                        ? "Avansert: Velg enkelt-emner."
                        : "Enkel: Velg en pakke som dekker flere emner."}
                </h2>

                {/* ---------- checkbox block ---------------------------------------- */}
                <div className="flex items-start justify-center gap-10 w-full">
                    <MenuContainer>
                        <div className="p-3 bg-white rounded-lg space-y-3 capitalize">
                            {advanced
                                ? // ------ avansert: original enkeltemner ---------------
                                Object.keys(selectedColumns).map((subject) => (
                                    <CheckBox
                                        key={subject}
                                        label={subject}
                                        checked={selectedColumns[subject]}
                                        onChange={() => toggleSingle(subject)}
                                    />
                                ))
                                : // ------ enkel: pakker -------------------------------
                                Object.keys(bundlesForSystem).map((bundle) => {
                                    const { checked, indeterminate } = bundleState(bundle);
                                    return (
                                        <CheckBox
                                            key={bundle}
                                            label={bundle}
                                            checked={checked}
                                            indeterminate={indeterminate}
                                            onChange={() => toggleBundle(bundle)}
                                        />
                                    );
                                })}
                            {/* ---------- toggle between modes ---------------------------------- */}
                            <div className="flex justify-center mt-4 mx-16">
                                <Button
                                    rounded
                                    label={advanced ? "← Enkel visning" : "Avansert visning →"}
                                    onClick={() => setAdvanced((p) => !p)}
                                    className="bg-[#EAEAEA] text-black hover:bg-[#D0D0D0] active:bg-[#9D9D9D] w-[160px] h-[32px]"
                                />
                            </div>
                        </div>
                    </MenuContainer>
                </div>
            </div>

            <div className="w-full px-10 flex justify-between">
                {/* ---------- navigation buttons ------------------------------------ */}
                <Link href="/filtype">
                    <Button
                        rounded
                        label="Forrige"
                        className="bg-[#EAEAEA] text-black hover:bg-[#D0D0D0] active:bg-[#9D9D9D] w-[100px] h-[32px]"
                    />
                </Link>

                <Link href={isDisabled ? "#" : "/export"}>
                    <Button
                        rounded
                        label="Neste"
                        className={`w-[100px] h-[32px] ${isDisabled
                            ? "bg-[#DAF0DA] text-white cursor-not-allowed"
                            : "bg-[#1E721E] text-white hover:bg-[#449844] active:bg-[#075607]"
                            }`}
                        aria-disabled={isDisabled}
                    />
                </Link>
            </div>

            <footer className="mt-auto py-6 text-center">
                <Link className="text-white hover:underline" href="/home">
                    Hjem
                </Link>
            </footer>
        </div>
    );
};

export default ImportVelger;