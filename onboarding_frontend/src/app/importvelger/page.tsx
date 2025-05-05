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

            // ① start with everybody *unselected*
            const next: Record<string, boolean> = Object.fromEntries(
                Object.keys(prev).map(k => [k, false])
            );

            // ② If we’re **activating** this bundle, tick just its subjects
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
        const allMine          = subs.every(s => selectedColumns[s]);

        // ✔  “checked”  = all my own subjects selected, and *nothing outside* selected
        const checked = allMine && onlyMineSelected;

        // ➖ “indeterminate” = some—but not all—of my own subjects selected,
        //                      and still *nothing outside* selected
        const indeterminate =
            !checked &&
            selected.some(s => subs.includes(s)) &&
            onlyMineSelected;

        return { checked, indeterminate };
    };

    const isDisabled = !Object.values(selectedColumns || {}).some(Boolean);
    // ---------------------------------------------------------------------

    return (
        <div className="flex flex-col items-center min-h-screen p-10 relative">
            {/* ---------- heading ------------------------------------------------ */}
            <h1 className="text-4xl font-bold text-center">
                Hva ønsker du å importere fra filene?
            </h1>
            <h2 className="text-xl text-center mt-5">
                {advanced
                    ? "Avansert: Velg enkelt-emner."
                    : "Enkel: Velg en pakke som dekker flere emner."}
            </h2>

            {/* ---------- toggle between modes ---------------------------------- */}
            <button
                onClick={() => setAdvanced((p) => !p)}
                className="mt-4 self-end text-sm underline text-blue-600"
            >
                {advanced ? "← Enkel visning" : "Avansert visning →"}
            </button>

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
                    </div>
                </MenuContainer>
            </div>

            {/* ---------- navigation buttons ------------------------------------ */}
            <div className="absolute bottom-10 w-full px-10 flex justify-between">
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
                        className={`w-[100px] h-[32px] ${
                            isDisabled
                                ? "bg-[#DAF0DA] text-white cursor-not-allowed"
                                : "bg-[#1E721E] text-white hover:bg-[#449844] active:bg-[#075607]"
                        }`}
                        aria-disabled={isDisabled}
                    />
                </Link>
            </div>

            <footer className="mt-auto py-6">
                <Link className="text-white hover:underline" href="/home">
                    Hjem
                </Link>
            </footer>
        </div>
    );
};

export default ImportVelger;
