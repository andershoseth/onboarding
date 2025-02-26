'use client'
import React from "react";
import Link from "next/link";
import MenuContainer, { useBoxState } from "../components/MenuContainer";
import InfoPage from "../components/InfoContainer";
import CheckBox from "../components/CheckBox";

const ImportVelger: React.FC = () => {
    const { selected, handleBoxChange } = useBoxState(); /* for the checkboxes */
    const isDisabled = !Object.values(selected).some(Boolean); /* for the next button */

    return (
        <div className="flex flex-col items-center min-h-screen p-10">
            <div className="flex items-start justify-center gap-10">
                <MenuContainer>
                    <div className="p-6 bg-gray-100 rounded-lg shadow-sm w-96">
                        <h2 className="text-xl font-semibold text-[#E17044]">Hva ønsker du å importere?</h2>
                        <p className="text-gray-600 text-sm mb-4">Huk av hva du vil importere fra filene dine.</p>

                        <div className="space-y-3">
                            <CheckBox label="Kontakter" checked={selected.kontakter} onChange={() => handleBoxChange("kontakter")} />
                            <CheckBox label="Avdeling" checked={selected.avdeling} onChange={() => handleBoxChange("avdeling")} />
                            <CheckBox label="Saldobalanse" checked={selected.saldobalanse} onChange={() => handleBoxChange("saldobalanse")} />
                        </div>

                        <div className="mt-6 flex justify-end">
                            <Link
                                href={isDisabled ? "#" : "/upload"}
                                className={`px-4 py-2 rounded-md shadow-md transition ${isDisabled ? "bg-gray-400 text-gray-200 cursor-not-allowed" : "bg-[#E17044] text-white hover:bg-[#c85b34]"
                                    }`}
                                aria-disabled={isDisabled}
                            >
                                Next
                            </Link>
                        </div>
                    </div>
                </MenuContainer>
                <InfoPage />
            </div>
            <footer className="mt-auto py-6">
                <Link className="text-[#E17044] hover:underline" href="/home">
                    Home
                </Link>
            </footer>
        </div>
    );
};

export default ImportVelger;