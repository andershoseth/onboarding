'use client';
import React, { useState } from "react";

export interface BoxState {
    kontakter: boolean;
    avdeling: boolean;
    faktura: boolean;
}

interface MenuContainerProps {
    children: React.ReactNode;
}

const MenuContainer: React.FC<MenuContainerProps> = ({ children }) => {
    return (
        <div className="flex items-center justify-center min-h-[90vh]">
            <div className=" flex items-center p-6 bg-white rounded-lg shadow-md w-96">{children}</div>
        </div>
    )
};

export const useBoxState = () => {
    const [selected, setSelected] = useState<BoxState>({
        kontakter: false,
        avdeling: false,
        faktura: false
    });

    const handleBoxChange = (name: keyof BoxState) => {
        setSelected((prev) => ({ ...prev, [name]: !prev[name] }));
    };

    return { selected, handleBoxChange };
}

export default MenuContainer;