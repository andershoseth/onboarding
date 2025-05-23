﻿namespace onboarding_backend.Models.StandardImport
{
    public class VoucherLine
    {
        // Konto / Mva
        public int? Account  { get; set; }                   
        public string? AccountName { get; set; }             // Tekst
        public string? AccountAgricultureDepartment   { get; set; } // Se egen tabell (Landbruksavdeling)
        public string? VAT { get; set; }                     // (se egen tabell, f.eks. "0", "1", "3", "14", "K1", etc.)
        public string? VATReturnSpecification { get; set; }  // (se egen tabell for mva-spesifikasjon)

        // Beløp og valuta
        public decimal? Amount { get; set; }                // Desimaltall i lokal valuta
        public string? Currency { get; set; }             
        public decimal? CurrencyAmount { get; set; }        // Desimaltall
        public decimal? Discount { get; set; }              // Desimaltall (prosent)
        public decimal? Quantity { get; set; }              // Numerisk (antall)

        // Faktura, KID, forfallsdato, beskrivelse
        public int? InvoiceNo { get; set; }                 // Numerisk (fakturanr)
        public string? CID { get; set; }                     // Tekst (Kidnr)
        public string? DueDate { get; set; }                 // DDMMYYYY
        public string? Description { get; set; }             // Tekst (fritekst på faktura / bokføring)
        public string? Reference { get; set; }               // Tekst (referanse på faktura/kreditnota)

        // Selger og periodisering
        public int? SalesPersonEmployeeNo { get; set; }     // Numerisk
        public string? SalesPersonName { get; set; }         // Tekst
        public int? Accrual { get; set; }                   // Numerisk (periodisering av bilagslinje)

        // Reskontro
        public int? CustomerNo { get; set; }                
        public int? SupplierNo { get; set; }                
        public int? EmployeeNo { get; set; }               
        public string? ContactName { get; set; }             
        public string? ContactGroup { get; set; }           
        public string? CustomerSince { get; set; }           
        public string? SupplierSince { get; set; }           
        public string? EmployeeSince { get; set; }           
        public int? IsVatFree { get; set; }             

        // Kontaktdata
        public string? Phone { get; set; }                   
        public string? Email { get; set; }                   
        public string? Web { get; set; }                    
        public string? OrganizationNo { get; set; }          

        // Postadresse
        public string? MailAddress1 { get; set; }
        public string? MailAddress2 { get; set; }
        public string? MailPostcode { get; set; }
        public string? MailCity { get; set; }
        public string? MailCountry { get; set; }             

        // Leveringsadresse
        public string? DeliveryAddress1 { get; set; }
        public string? DeliveryAddress2 { get; set; }
        public string? DeliveryPostcode { get; set; }
        public string? DeliveryCity { get; set; }
        public string? DeliveryCountry { get; set; }         // (2 tegn) Landkode ISO 3166-1

        // Bank
        public string? BankAccount { get; set; }
        public string? IBAN { get; set; }
        public string? SWIFT { get; set; }

        // Kontaktperson
        public string? ContactPersonFirstName { get; set; }
        public string? ContactPersonLastName { get; set; }
        public string? ContactPersonPhone { get; set; }
        public string? ContactPersonEmail { get; set; }

        // Produktoppdatering
        public string? ProductCode { get; set; }             
        public string? ProductName { get; set; }             
        public string? ProductGroup { get; set; }            
        public string? ProductDescription { get; set; }      // Tekst (overskrives av "Description" i noen tilfeller)
        public int? ProductType { get; set; }               // Se egen tabell (1=Vare,2=Tjeneste)
        public string?   ProductUnit { get; set; }             // Se egen tabell (EA,HUR, etc.)
        public decimal? ProductSalesPrice { get; set; }
        public decimal? ProductCostPrice { get; set; }
        public int? ProductSalesAccount { get; set; }
        public string? ProductSalesAccountName { get; set; }
        public int? ProductAltSalesAccount { get; set; }
        public string? ProductAltSalesAccountName { get; set; }
        public string? ProductGTIN { get; set; }

        // Prosjekt
        public string? ProjectCode { get; set; }
        public string? SubprojectCode { get; set; }
        public string? ProjectName { get; set; }
        public int? ProjectManagerCode { get; set; }
        public string? ProjectManagerName { get; set; }
        public int? ProjectBillable { get; set; }         // 0= false,1= true
        public string? ProjectStartDate { get; set; }      // DDMMYYYY
        public string? ProjectEndDate { get; set; }        // DDMMYYYY
        public int? ProjectStatus { get; set; }           // Se egen tabell
        public string? DepartmentCode { get; set; }
        public string? DepartmentName { get; set; }
        public int? InvoiceDelivery { get; set; }         // se egen tabell (f.eks. 1=E-post)

        // Kunde-rabatt
        public int? CustomerDiscount { get; set; }

        // Egendefinert matching
        public string? CustomMatchingReference { get; set; }
    }
}
