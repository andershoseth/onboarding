import React from "react";

interface CardInfo {
    logoSrc: string;
    title: string;
    description: string;
    link: string;
}

const Card: React.FC<CardInfo> = ({ logoSrc, title, description }) => {
    return (
        <div className="w-80 h-64 p-10 bg-[#FAFAFA] rounded-lg shadow-[#6A48B5] border border-black text-center flex flex-col justify-between">
            <img src={logoSrc} alt="Logo" className="w-33 h-20 mx-auto bg-transparent" />
            <h2 className="text-black">
                {title}
            </h2>
            <h4 className="text-black overflow-hidden">{description}</h4>
        </div>
    );
};

export default Card;
