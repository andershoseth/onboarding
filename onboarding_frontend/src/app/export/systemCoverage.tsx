export const systemCoverage: { [key: string]: { [key: string]: string[] } } = {
  "Visma": {

    safTSubjects: ["hovedboktransaksjoner", "avdeling"],
    csvSubjects: ["hovedboktransaksjoner", "kontakter", "avdeling", "tilbudimport"],
    excelSubjects: ["hovedboktransaksjoner", "kontakter", "tilbudimport"]

  },
  "Tripletex": {
    safTSubjects: ["hovedboktransaksjoner", "kontakter", "avdeling"],
    csvSubjects: ["hovedboktransaksjoner", "kontakter", "avdeling", "tilbudimport"],
    excelSubjects: ["hovedboktransaksjoner", "kontakter", "tilbudimport"]
  }
};