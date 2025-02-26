import MenuContainer from "../components/MenuContainer";

export default function InfoPage() {
    return (
        <MenuContainer>
            <div className="text-center">
                <h2 className="text-2xl font-semibold text-[#E17044]">Hva skjer videre?</h2>
                <p className="text-gray-600">Du vil få en guide som viser steg for steg eksporten fra det gamle systemet du migrerer fra, i tillegg hvordan du laster inn filenene én og én.</p>
            </div>
        </MenuContainer>
    );
}
