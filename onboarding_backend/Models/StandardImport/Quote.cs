namespace onboarding_backend.Models.StandardImport
{
    public class Quote
    {
        
            public int? QuoteNo { get; set; }       // Numerisk, Påkrevd
            public string QuoteDate { get; set; }   // DDMMYYYY (f.eks. når tilbudet ble sendt)
            public string QuoteExpiryDate { get; set; } // DDMMYYYY

            // Selger/prosjekt
            public int? SalesPersonEmployeeNo { get; set; }
            public string SalesPersonName { get; set; }
            public string ProjectCode { get; set; }
            public string SubprojectCode { get; set; }
            public string ProjectName { get; set; }
            public string ProjectManagerCode { get; set; }
            public string ProjectManagerName { get; set; }
            public int? ProjectBillable { get; set; } // 0= false,1= true
            public string ProjectStartDate { get; set; }
            public string ProjectEndDate { get; set; }
            public int? ProjectStatus { get; set; }
            public string ProjectContactPerson { get; set; }

            // Avdelingsinfo
            public string DepartmentCode { get; set; }
            public string DepartmentName { get; set; }
            public int? DepartmentManagerCode { get; set; }
            public string DepartmentManagerName { get; set; }

            // Kundesiden
            public int? CustomerNo { get; set; }  // Påkrevd på "tilbudhodet"
            public string ContactName { get; set; }
            public string ContactGroup { get; set; }
            public string CustomerSince { get; set; }
            public int? IsVatFree { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Web { get; set; }
            public string OrganizationNo { get; set; }

            // Postadresse
            public string MailAddress1 { get; set; }
            public string MailAddress2 { get; set; }
            public string MailPostcode { get; set; }
            public string MailCity { get; set; }
            public string MailCountry { get; set; }

            // Leveringsadresse
            public string DeliveryAddress1 { get; set; }
            public string DeliveryAddress2 { get; set; }
            public string DeliveryPostcode { get; set; }
            public string DeliveryCity { get; set; }
            public string DeliveryCountry { get; set; }

            // Bank / Levering
            public string BankAccount { get; set; }
            public string IBAN { get; set; }
            public string SWIFT { get; set; }
            public int? InvoiceDelivery { get; set; }

            // Kontaktperson
            public string ContactPersonFirstName { get; set; }
            public string ContactPersonLastName { get; set; }
            public string ContactPersonPhone { get; set; }
            public string ContactPersonEmail { get; set; }

            // Produktlinjer (hvis alt i én post)
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string ProductGroup { get; set; }
            public string ProductDescription { get; set; }
            public int? ProductType { get; set; }
            public string ProductUnit { get; set; }
            public decimal? ProductSalesPrice { get; set; }
            public decimal? ProductCostPrice { get; set; }
            public int? ProductSalesAccount { get; set; }
            public string ProductSalesAccountName { get; set; }
            public int? ProductAltSalesAccount { get; set; }
            public string ProductAltSalesAccountName { get; set; }
            public string ProductGTIN { get; set; }

            public decimal? Discount { get; set; }
            public decimal? Quantity { get; set; }
            public string Description { get; set; }   // fritekstlinje
            public decimal? QuoteLineUnitPrice { get; set; }
            public string VATReturnSpecification { get; set; }
        

    }
}
