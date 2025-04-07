import Card from '../card';
import Link from 'next/link';

function Home() {
    return (
        <>
            <div className="flex flex-col items-center text-center mt-10">
                <h1 className="text-6xl font-bold">
                    <span className="text-[#FF8D04]">VELKOMMEN</span>
                </h1>
                <h2 className="text-3xl mt-2 text-white">
                    til PowerOffice konverteringsverktøy!
                </h2>
            </div>

            <div className="flex flex-col items-center text-center mt-10">
                <h2 className="text-xl mt-2 text-white">
                    Her kan du konvertere filene fra ditt gamle regnskapssystem over til PowerOffice Go!
                </h2>
                <h2 className="text-xl mt-2 text-white">
                    La oss starte prosessen med å trykke "Begynn importen" nedenfor:
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
            </div>
        </>
    );
}

export default Home;