import Card from '../card';

function Resources() {
    return (
        <>
            <div className="flex flex-col items-center text-center mt-10">
                <h1 className="text-4xl font-bold">
                    <span className="text-blue-600">Ressurser</span>
                </h1>
            </div>

            <div className="flex flex-wrap justify-center gap-8 p-10">
                {/*import*/}
                <Card
                    logoSrc="import.png"
                    title="Begynn importen"
                    description="Få inn nøkkeltall fra systemet du kommer fra"
                    link="/upload"
                />
            </div>
        </>
    );
}

export default Resources;