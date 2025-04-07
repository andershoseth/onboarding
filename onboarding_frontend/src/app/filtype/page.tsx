'use client';
import Card from '../card';
import Link from "next/link";
import ImportContext from '../components/ImportContext';
import React, { useContext } from 'react';

function FileTypeChoice() {
    const { setSelectedFileType } = useContext(ImportContext);

    const cards = [
        {
            logoSrc: "import.png",
            title: "Excel (.xlsx)",
            description: "Last opp filer i Excel-format",
            link: "/importvelger"
        },
        {
            logoSrc: "import.png",
            title: "CSV (.csv)",
            description: "Last opp filer i CSV-format",
            link: "/importvelger"
        },
        {
            logoSrc: "import.png",
            title: "SAF-T (.xml)",
            description: "Last opp filer i SAF-T-format",
            link: "/importvelger"
        }
    ];

    const handleClick = (title: string) => {
        // Save the selected card's title in the context
        setSelectedFileType(title);
    };

    return (
        <>
            <div className="flex flex-col items-center text-center mt-10">
                <h1 className="text-4xl font-bold">Hvilken filtype skal lastes opp?</h1>
            </div>

            <div className="flex flex-col items-center text-center mt-5">
                <h2 className="text-xl">Dette trinnet har betydning senere i prosessen.</h2>
            </div>

            <div className="flex flex-wrap justify-center gap-8 p-10">
                {/* genererer kort for hver item i lista */}
                {cards.map((card, index) => (
                    <Link key={index} href={card.link} passHref>
                        <div
                            onClick={() => handleClick(card.title)}
                            className="cursor-pointer"
                        >
                            <Card
                                logoSrc={card.logoSrc}
                                title={card.title}
                                description={card.description}
                                link={card.link}
                            />
                        </div>
                    </Link>
                ))}
            </div>
            <footer className="row-start-3 flex gap-6 flex-wrap items-center justify-center">
                <Link
                    className="flex items-center gap-2 hover:underline hover:underline-offset-4"
                    href="/home"
                >
                    Hjem
                </Link>
            </footer>
        </>
    );
}

export default FileTypeChoice;