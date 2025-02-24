import React, { useState } from "react";

export interface BoxState {
    kunder: boolean;
    bilag: boolean;
    saldobalanse: boolean;
}

interface MenuContainerProps {
    children: (state: BoxState, handleBoxChange: (name: keyof BoxState) => void) => React.ReactNode;
}

const MenuContainer: React.FC<MenuContainerProps> = ({ children }) => {
    const [selected, setSelected] = useState<BoxState>({
        kunder: true,
        bilag: false,
        saldobalanse: true
    });

    const handleBoxChange = (name: keyof BoxState) => {
        setSelected((prev) => ({ ...prev, [name]: !prev[name] }));
    };

    return <>{children(selected, handleBoxChange)}</>;
};

export default MenuContainer;