interface CheckBoxProps {
    label: string;
    checked: boolean;
    indeterminate?: boolean;   // ← NY
    onChange: () => void;
}

const CheckBox: React.FC<CheckBoxProps> = ({
                                               label,
                                               checked,
                                               indeterminate = false,
                                               onChange,
                                           }) => (
    <label className="flex items-center space-x-2 cursor-pointer select-none">
        <input
            type="checkbox"
            checked={checked}
            // nullable indeterminate
            ref={(el) => {
                if (el) el.indeterminate = indeterminate;
            }}
            onChange={onChange}
            className="hidden"
        />
        <div
            className={`w-5 h-5 flex items-center justify-center border-2 rounded-md transition-all ${
                checked || indeterminate ? "border-[#E17044]" : "border-black"
            }`}
        >
            {(checked || indeterminate) && <span className="text-black">✔</span>}
        </div>
        <h4 className="text-black">{label}</h4>
    </label>
);

export default CheckBox;
