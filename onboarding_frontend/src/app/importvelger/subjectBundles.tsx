// app/subjectBundles.ts

export type BundlesPerSystem = {
    [system: string]: Record<string, readonly string[]>;
};

export const defaultBundles: Record<string, readonly string[]> = {
    Bilagsimport: [
        "kontakter",
        "produkter",
        "prosjekter",
        "avdelinger",
        "bilag",
        "bilagslinjer",
    ],
    Tilbudsimport: [
        "kontakter",
        "prosjekter",
        "avdelinger",
        "tilbud",
    ],
    Ordreimport: [
        "kontakter",
        "produkter",
        "avdelinger",
        "bestillinger",
    ],
    Registerimport: [
        "avdelinger",
        "kontakter",
        "produkter",
        "prosjekter",
        "aktiviteter",
        "kontoplan",
        "anleggsmidler",
    ],


} as const;

export const subjectBundles: BundlesPerSystem = {
    Visma: {
        ...defaultBundles,
    },
    Tripletex: {
        ...defaultBundles,
        //Bilagsimport: ["bilag", "bilagslinjer"],
    },
};


export const getBundlesFor = (system: string) =>
    subjectBundles[system] ?? defaultBundles;
