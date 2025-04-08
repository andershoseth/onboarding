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
            <div className={`w-5 h-5 flex items-center justify-center border-2 rounded-md transition-all ${checked ? "border-[#E17044]" : "border-black"}`}>
                {checked && <span className="text-black">✔</span>}
            </div>
            <h4><span className="text-black mt-6"> {label} </span></h4>
        </label>
    );
};

export default CheckBox;
