export const systemCoverage: { [key: string]: { [key: string]: string[] } } = {
  "Visma": {
    safTSubjects: ["avdelinger", "bilag", "bilagslinjer", "kontoplan", "kontakter", "prosjekter", "prosjektaktiviteter",
      "prosjektfaktureringsmetode", "prosjektmedlemmer", "prosjektstatus"],

    csvSubjects: ["anleggsmidler", "bestillinger", "faktura CID", "lønnsgrunnlag", "lønnsjustering",
      "lønnssaldo hittil i år", "produkter", "timeprisspesifikasjon", "tilbud"],
  },

  "Tripletex": {
    safTSubjects: ["anleggsmidler", "bilag", "bestillinger", "kontoplan", "lønnsgrunnlag",
      "lønnsjustering", "produkter", "timeprisspesifikasjon", "tilbud"],

    csvSubjects: ["avdelinger", "bilagslinjer", "faktura CID", "kontakter", "lønnssaldo hittil i år", "prosjekter",
      "prosjektaktiviteter", "prosjektfaktureringsmetode", "prosjektmedlemmer", "prosjektstatus"],
  }
}; //elementene i listene er tatt fra standardimport-mappa på backenden