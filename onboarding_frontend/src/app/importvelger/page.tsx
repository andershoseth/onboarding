'use client'
import React from "react";
import Link from "next/link";
import MenuContainer, { useBoxState } from "../components/MenuContainer";
import CheckBox from "../components/CheckBox";

const ImportVelger: React.FC = () => {
    const { selected, handleBoxChange } = useBoxState(); /* for the checkboxes */
    const isDisabled = !Object.values(selected).some(Boolean) /* for the next button */
    return (
        <>
            <MenuContainer>
                <div className="p-6 bg-gray-100 rounded-lg shadow-sm w-96">
                    <h2 className="text-xl font-semibold text-[#E17044]"> Hva ønsker du å importere? </h2>
                    <p className="text-gray-600 text-sm mb-4"> Huk av hva du vil importere fra filene dine </p>

                    <div className="space-y-3">
                        <CheckBox
                            label="Kunder"
                            checked={selected.kontakter}
                            onChange={() => handleBoxChange("kontakter")}
                        />
                        <CheckBox
                            label="Bilag"
                            checked={selected.avdeling}
                            onChange={() => handleBoxChange("avdeling")}
                        />
                        <CheckBox
                            label="Saldobalanse"
                            checked={selected.faktura}
                            onChange={() => handleBoxChange("faktura")}
                        />
                    </div>

                    <div className="mt-6 flex justify-end">
                        {/* next button to the upload page */}
                        <Link
                            href={isDisabled ? "#" : "/upload"}
                            className={`px-4 py-2 rounded-md shadow-md transition ${isDisabled
                                ? "bg-gray-400 text-gray-200 cursor-not-allowed" /* no button is checked */
                                : "bg-[#E17044] text-white hover:bg-[c85b34]" /* button is checked */
                                }`}
                            aria-disabled={isDisabled}
                        >
                            Next
                        </Link>
                    </div>
                </div>
            </MenuContainer>
            <footer className="row-start-3 flex gap-6 flex-wrap items-center justify-center">
                <Link
                    className="flex items-center gap-2 hover:underline hover:underline-offset-4"
                    href="/home"
                >
                    Home
                </Link>
            </footer>
        </>
    );
};

export default ImportVelger;