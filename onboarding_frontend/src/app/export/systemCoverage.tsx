export const systemCoverage: { [key: string]: { [key: string]: string[] } } = {
  "Visma": {
    safTSubjects: ["avdelinger", "bilag", "bilagslinjer", "kontoplan", "kontakter",
      "lønnsgrunnlag", "lønnsjustering", "prosjekter", "prosjektaktiviteter",
      "prosjektfaktureringsmetode", "prosjektmedlemmer", "prosjektstatus"],

    csvSubjects: ["anleggsmidler", "bestillinger", "faktura CID", "lønnsgrunnlag", "lønnsjustering",
      "lønnssaldo hittil i år", "produkter", "timeprisspesifikasjon", "tilbud"],
  },

  "Tripletex": {
    safTSubjects: ["anleggsmidler", "bestillinger", "bilag", "faktura CID", "kontakter", "kontoplan", "lønnsgrunnlag",
      "lønnsjustering", "lønnssaldo hittil i år", "produkter", "timeprisspesifikasjon", "tilbud"],

    csvSubjects: ["avdelinger", "bilag", "bilagslinjer", "kontoplan", "kontakter", "prosjekter",
      "prosjektaktiviteter", "prosjektfaktureringsmetode", "prosjektmedlemmer", "prosjektstatus"],
  }
}; //elementene i listene er tatt fra standardimport-mappa på backenden