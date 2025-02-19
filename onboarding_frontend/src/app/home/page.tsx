import Card from '../card';
import Link from 'next/link';

function Home() {
    return (
        <>
            <div className="flex flex-col items-center text-center mt-10">
                <h1 className="text-4xl font-bold">
                    <span className="text-[#E17044]">VELKOMMEN</span>
                </h1>
                <h2 className="text-2xl mt-2 text-gray-700 dark:text-gray-300">
                    til PowerOffice konverteringsverktøy
                </h2>
            </div>

            <div className="flex flex-wrap justify-center gap-8 p-10">
                {/*import*/}
                <Link href="/systemvalg">
                <Card
                    logoSrc="import.png"
                    title="Begynn importen"
                    description="Få inn nøkkeltall fra systemet du kommer fra"
                    link="/systemvalg"
                />
                </Link>
                {/*links*/}
                <Card
                    logoSrc="link.png"
                    title="Nyttige linker"
                    description="Finn linker til ressursene du trenger i oppstarten"
                    link="/ressurser"
                />
                {/*task list*/}
                <Card
                    logoSrc="task_list.png"
                    title="Oppgaveliste"
                    description="Det som gjenstår for å komme i gang"
                    link="/oppgaveliste"
                />
            </div>
        </>
    );
}

export default Home;