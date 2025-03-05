'use client';
import Card from '../card';
import Link from "next/link";
import ImportContext from '../components/ImportContext';
import React from 'react';

function SystemChoice() {
    const { setSelectedSystem } = React.useContext(ImportContext);

    const cards = [
        {
            logoSrc: "import.png",
            title: "Excel (.xlxs)",
            description: "Importer data fra Visma",
            link: "/importvelger"
        },
        {
            logoSrc: "import.png",
            title: "CSV (.csv)",
            description: "Importer data fra Tripletex",
            link: "/importvelger"
        },
        {
            logoSrc: "import.png",
            title: "SAF-T (.xml)",
            description: "Importer data fra Xledger",
            link: "/importvelger"
        }
    ];

    const handleClick = (title: string) => {
        // Save the selected card's title in the context
        setSelectedSystem(title);
    };

    return (
        <>
            <div className="flex flex-col items-center text-center mt-10">
                <h1 className="text-4xl font-bold">Hvilken filtype vil du laste opp?</h1>
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
                    Home
                </Link>
            </footer>
        </>
    );
}

export default SystemChoice;