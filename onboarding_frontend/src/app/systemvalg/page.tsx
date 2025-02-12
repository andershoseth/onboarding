import Card from '../card';

function SystemChoice() {
    const cards = [
        {
            logoSrc: "import.png",
            title: "Visma",
            description: "Importer data fra Visma",
            link: "/upload"
        },
        {
            logoSrc: "import.png",
            title: "Tripletex",
            description: "Importer data fra Tripletex",
            link: "/upload"
        },
        {
            logoSrc: "import.png",
            title: "Xledger",
            description: "Importer data fra Xledger",
            link: "/upload"
        },
        {
            logoSrc: "import.png",
            title: "Bank + regnskap",
            description: "Importer data fra Bank + regnskap",
            link: "/upload"
        },
        {
            logoSrc: "import.png",
            title: "Fiken",
            description: "Importer data fra Fiken",
            link: "/upload"
        },
        {
            logoSrc: "import.png",
            title: "Egendefinert",
            description: "Importer data fra et egendefinert regnskapssystem",
            link: "/upload"
        },
    ]

    return (
        <>
            <div className="flex flex-col items-center text-center mt-10">
                <h1 className="text-4xl font-bold">
                    <span className="text-blue-600">Hvilket system kommer du fra?</span>
                </h1>
            </div>

            <div className="flex flex-wrap justify-center gap-8 p-10">
                {cards.map((card, index) => (
                    <Card
                        key={index}
                        logoSrc={card.logoSrc}
                        title={card.title}
                        description={card.description}
                        link={card.link}
                    />
                ))}

            </div>
        </>
    );
}

export default SystemChoice;