import React from "react";

interface CardInfo {
    logoSrc: string;
    title: string;
    description: string;
    link: string;
}

const Card: React.FC<CardInfo> = ({ logoSrc, title, description, link }) => {
    return (
        <div className="w-80 h-64 p-10 bg-gradient-to-b from-[#E17044] to-[#54155C] rounded-lg shadow-lg text-center flex flex-col justify-between">
            <img src={logoSrc} alt="Logo" className="w-33 h-20 mx-auto bg-transparent" />
            <h2 className="text-white text-xl font-bold">
                {title}
            </h2>
            <p className="text-white text-sm overflow-hidden text-ellipsis">{description}</p>
        </div>
    );
};

export default Card;
