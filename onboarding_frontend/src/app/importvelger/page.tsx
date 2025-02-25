'use client'
import React from "react";
import MenuContainer, { useBoxState } from "../components/menuContainer";
import CheckBox from "../components/checkBox";

const ImportVelger: React.FC = () => {
    const { selected, handleBoxChange } = useBoxState();
    return (
        <MenuContainer>
            <div className="p-6 bg-gray-100 rounded-lg shadow-sm w-96">
                <h2 className="text-xl font-semibold text-[#E17044]"> Hva ønsker du å importere? </h2>
                <p className="text-gray-600 text-sm mb-4"> Huk av hva du vil importere fra det gamle regnskapssystemet </p>

                <div className="space-y-3">
                    <CheckBox
                        label="Kunder"
                        checked={selected.kunder}
                        onChange={() => handleBoxChange("kunder")}
                    />
                    <CheckBox
                        label="Bilag"
                        checked={selected.bilag}
                        onChange={() => handleBoxChange("bilag")}
                    />
                    <CheckBox
                        label="Saldobalanse"
                        checked={selected.saldobalanse}
                        onChange={() => handleBoxChange("saldobalanse")}
                    />
                </div>
            </div>
        </MenuContainer>
    );
};

export default ImportVelger;