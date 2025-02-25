'use client'
import React from "react";
import Link from "next/link";
import MenuContainer, { useBoxState } from "../components/menuContainer";
import CheckBox from "../components/checkBox";

const ImportVelger: React.FC = () => {
    const { selected, handleBoxChange } = useBoxState();
    return (
        <>
            <MenuContainer>
                <div className="p-6 bg-gray-100 rounded-lg shadow-sm w-96">
                    <h2 className="text-xl font-semibold text-[#E17044]"> Hva ønsker du å importere? </h2>
                    <p className="text-gray-600 text-sm mb-4"> Huk av hva du vil importere fra det gamle regnskapssystemet </p>

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