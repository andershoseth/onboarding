import React from "react";

interface CardInfo {
    logoSrc: string;
    title: string;
    description: string;
    link: string;
}

const Card: React.FC<CardInfo> = ({ logoSrc, title, description, link }) => {
    return (
        <div className="w-80 h-64 p-10 bg-gradient-to-b from-blue-500 to-purple-700 rounded-lg shadow-lg text-center flex flex-col justify-between">
            <img src={logoSrc} alt="Logo" className="w-33 h-20 mx-auto bg-transparent" />
            <h2 className="text-white text-xl font-bold">
                <a href={link} className="text-white underline hover:text-gray-300 transition"> {title} </a>
            </h2>
            <p className="text-white text-sm overflow-hidden text-ellipsis">{description}</p>
        </div>
    );
};

export default Card;
