export const systemCoverage: { [key: string]: { [key: string]: string[] } } = {
  "Visma": {

    safTSubjects: ["hovedboktransaksjoner", "avdeling"],
    csvSubjects: ["hovedboktransaksjoner", "kontakter", "avdeling", "tilbudimport"],
  },
  "Tripletex": {
    safTSubjects: ["hovedboktransaksjoner", "kontakter"],
    csvSubjects: ["hovedboktransaksjoner", "kontakter", "avdeling", "tilbudimport"],
  }
};