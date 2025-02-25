import React from "react";

interface CheckBoxProps {
    label: string;
    checked: boolean;
    onChange: () => void;
}

const CheckBox: React.FC<CheckBoxProps> = ({ label, checked, onChange }) => {
    return (
        <label className="flex items-center space-x-2 cursor-pointer">
            <input type="checkbox" checked={checked} onChange={onChange} className="hidden" />
            <div className={`w-5 h-5 flex items-center justify-center border-2 rounded-md transition-all ${checked ? "border-[#E17044]" : "border-gray-400"}`}>
                {checked && <span className="text-black text-lg">✔</span>}
            </div>
            <span className="text-gray-700"> {label} </span>
        </label>
    );
};

export default CheckBox;