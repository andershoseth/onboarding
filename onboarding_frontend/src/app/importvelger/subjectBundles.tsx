// app/subjectBundles.ts
export type BundlesPerSystem = {
    [system: string]: {
        [bundleNavn: string]: string[];   // hvilke gamle emner som ligger i pakken
    };
};

export const subjectBundles: BundlesPerSystem = {
    Visma: {
        Bilagsimport: ["bilag", "bilagslinjer"],
        Prosjekt: [
            "prosjekter",
            "prosjektaktiviteter",
            "prosjektmedlemmer",
            "prosjektstatus",
            "prosjektfaktureringsmetode",
        ],
        "Produkter & priser": ["produkter", "timeprisspesifikasjon", "tilbud"],
        Lønn: [
            "lønnsgrunnlag",
            "lønnsjustering",
            "lønnssaldo hittil i år",
        ],
        "Kontoplan & dimensjoner": ["kontoplan", "avdelinger", "kontakter"],
    },

    Tripletex: {
        Bilagsimport: ["bilag", "bilagslinjer"],
        Prosjekt: [
            "prosjekter",
            "prosjektaktiviteter",
            "prosjektmedlemmer",
            "prosjektstatus",
            "prosjektfaktureringsmetode",
        ],
        "Produkter & priser": ["produkter", "timeprisspesifikasjon", "tilbud"],
        Lønn: [
            "lønnsgrunnlag",
            "lønnsjustering",
            "lønnssaldo hittil i år",
        ],
        "Kontoplan & dimensjoner": ["kontoplan", "avdelinger", "kontakter"],
    },
};
