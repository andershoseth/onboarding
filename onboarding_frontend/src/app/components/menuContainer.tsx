'use client';
import React, { useEffect, useState } from "react";

export interface BoxState {
    kontakter: boolean;
    avdeling: boolean;
    saldobalanse: boolean;
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
        saldobalanse: false
    });

    const [isMounted, setIsMounted] = useState(false); //makes sure the boxes stays checked when going back

    useEffect(() => { //fixes hydration error
        if (typeof window !== "undefined") {
            const savedState = localStorage.getItem("checkboxState");
            if (savedState) {
                setSelected(JSON.parse(savedState))
            }
            setIsMounted(true)
        }
    }, []);

    useEffect(() => {
        if (isMounted) {
            localStorage.setItem("checkboxState", JSON.stringify(selected))
        }
    }, [selected, isMounted]);

    const handleBoxChange = (name: keyof BoxState) => {
        setSelected((prev) => ({ ...prev, [name]: !prev[name] }));
    };

    return { selected, handleBoxChange };
}

export default MenuContainer;