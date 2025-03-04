namespace onboarding_backend.Models.StandardImport
{
    public class Contacts
    {
        // Identifikasjon av kontakt
        public int? CustomerNo { get; set; }         // Numerisk (kunde)
        public int? SupplierNo { get; set; }         // Numerisk (leverandør)
        public int? EmployeeNo { get; set; }         // Numerisk (ansatt)

        // Kontaktens navn/gruppe
        public string ContactName { get; set; }      // Tekst (Påkrevd)
        public string ContactGroup { get; set; }     // Tekst

        // Kunde-, leverandør- og ansatt-siden
        public string CustomerSince { get; set; }    // DDMMYYYY
        public string SupplierSince { get; set; }    // DDMMYYYY
        public string EmployeeSince { get; set; }    // DDMMYYYY

        // MVA-relatert
        public int? IsVatFree { get; set; }          // 0 = False, 1 = True

        // Generell kontaktinfo
        public string Phone { get; set; }            // Tekst
        public string Email { get; set; }            // Tekst
        public string Web { get; set; }              // Tekst

        // Organisasjonsinfo
        public string OrganizationNo { get; set; }   // Tekst

        // Postadresse (MailAddress)
        public string MailAddress1 { get; set; }     // Tekst
        public string MailAddress2 { get; set; }     // Tekst
        public string MailPostcode { get; set; }     // Tekst
        public string MailCity { get; set; }         // Tekst
        public string MailCountry { get; set; }      // (2 tegn) Postadresse landskode (ISO 3166-1)

        // Leveringsadresse (DeliveryAddress)
        public string DeliveryAddress1 { get; set; } // Tekst
        public string DeliveryAddress2 { get; set; } // Tekst
        public string DeliveryPostcode { get; set; } // Tekst
        public string DeliveryCity { get; set; }     // Tekst
        public string DeliveryCountry { get; set; }  // (2 tegn) ISO 3166-1

        // Bankopplysninger
        public string BankAccount { get; set; }
        public string IBAN { get; set; }
        public string SWIFT { get; set; }

        // Kontaktperson-detaljer
        public string ContactPersonFirstName { get; set; } // Tekst
        public string ContactPersonLastName { get; set; }  // Tekst
        public string ContactPersonPhone { get; set; }     // Tekst
        public string ContactPersonEmail { get; set; }     // Tekst

        // Fakturalevering
        public int? InvoiceDelivery { get; set; }     // (Se egen tabell) Standard leveringsmåte (1=E-post,2=Utskrift,3=EHF)
        public string InvoiceEmailAddress { get; set; }

        // Privatperson / ansattinfo
        public int? IsPerson { get; set; }            // 0 = false, 1 = true
        public string SocialSecurityNumber { get; set; } // Numerisk (11 tegn)
        public string InternationalIdNumber { get; set; }
        public string InternationalIdCountryCode { get; set; } // Tekst (2 tegn)
        public int? InternationalIdType { get; set; } // Numerisk
        public string DateOfBirth { get; set; }       // DDMMYYYY

        // Ansatt-detaljer
        public string JobTitle { get; set; }          // Tekst
        public string DepartmentCode { get; set; }    // Tekst
        public string DepartmentName { get; set; }    // Tekst
        public string PayslipEmail { get; set; }      // Tekst
        public decimal? OwedHolidayPayForLastYear { get; set; } // Desimaltall
        public decimal? OwedHolidayPayForLastYearOver60 { get; set; } // Desimaltall

        // FNO-rapportering
        public int? IncludeInFnoReport { get; set; }  // 0/false/nei, 1/true/ja
        public string RemunerationPeriod { get; set; } // Se egen tabell (avlønningsperiode)

        // Kunde-rabatt
        public int? CustomerDiscount { get; set; }    // Numerisk (prosent)

        // Leverandørspesifikke felter (om de ønskes her)
        public int? SupplierStandardAccount { get; set; }                     // Numerisk
        public int? SupplierStandardAccountAgricultureDepartment { get; set; } // Numerisk (Landbruksavdeling)
        public string SupplierEHFCoding { get; set; }   // Tekst (Se egen tabell)
        public string SupplierApprover { get; set; }    // Brukerens e-postadresse eller en “standard leder”
        public int? SubmitAutomaticallyForApproval { get; set; } // 0= false,1= true
    }
}
