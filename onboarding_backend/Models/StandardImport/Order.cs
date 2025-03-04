namespace onboarding_backend.Models.StandardImport
{
    public class Order
    {
        // Overordnede ordrefelter
            public int? OrderNo { get; set; }                  // Numerisk, Ja
            public string OrderDate { get; set; }             // DDMMYYYY
            public int? RecurringInvoiceActive { get; set; }   // 0= false,1= true
            public int? RecurringInvoiceRepeatTimes { get; set; }
            public string RecurringInvoiceEndDate { get; set; } // DDMMYYYY
            public int? RecurringInvoiceSendMethod { get; set; }
            public int? RecurringInvoiceSendFrequency { get; set; }
            public int? RecurringInvoiceSendFrequencyUnit { get; set; } // 0=Uke,1=Måned,2=År, etc.
            public string NextRecurringInvoiceDate { get; set; } // DDMMYYYY

            // Selger/Prosjekt
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
            public int? ProjectStatus { get; set; }   // se egen tabell
            public string ProjectContactPerson { get; set; }

            // Avdeling
            public string DepartmentCode { get; set; }
            public string DepartmentName { get; set; }
            public int? DepartmentManagerCode { get; set; }
            public string DepartmentManagerName { get; set; }

            // Reskontro / kunde
            public int? CustomerNo { get; set; }
            public string ContactName { get; set; }
            public string ContactGroup { get; set; }
            public string CustomerSince { get; set; }  // DDMMYYYY
            public int? IsVatFree { get; set; }        // 0= false,1= true
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

            // Bank
            public string BankAccount { get; set; }
            public string IBAN { get; set; }
            public string SWIFT { get; set; }

            // Fakturaoppsett
            public int? InvoiceDelivery { get; set; }     // 1=E-post,2=Utskrift,3=EHF, ...
            public string ContactPersonFirstName { get; set; }
            public string ContactPersonLastName { get; set; }
            public string ContactPersonPhone { get; set; }
            public string ContactPersonEmail { get; set; }

            // Ekstra
            public string Reference { get; set; }
            public int? PaymentTerms { get; set; }  // Netto dager
            public int? MergeWithPreviousOrder { get; set; } // 0= false,1= true
            public string Currency { get; set; }  // 3-tegn

            // Produkt & linjer (hvis du vil ha EN-linjers 
            // import i stedet for et eget lines-objekt)
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
            public string Description { get; set; } // fritekstlinje
            public decimal? OrderLineUnitPrice { get; set; }
            public int? SortOrder { get; set; }     // Linjerekkefølge
            public string VATReturnSpecification { get; set; }
        

    }
}
