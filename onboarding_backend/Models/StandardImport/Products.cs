namespace onboarding_backend.Models.StandardImport
{
    public class Products
    {
    
            // Obligatoriske felt
            public string ProductCode { get; set; }        // Tekst (Påkrevd)
            public string ProductName { get; set; }        // Tekst (Påkrevd)
            public int ProductSalesAccount { get; set; }   // Numerisk, påkrevd

            // Valgfrie felt
            public string ProductGroup { get; set; }
            public string ProductDescription { get; set; }
            public int? ProductType { get; set; }          // (Se egen tabell: 1=Vare, 2=Tjeneste)
            public string ProductUnit { get; set; }        // (Se egen tabell: EA, HUR, MTR, etc.)
            public decimal? ProductSalesPrice { get; set; }
            public decimal? ProductCostPrice { get; set; }
            public int? ProductAltSalesAccount { get; set; }
            public string ProductAltSalesAccountName { get; set; }
            public string ProductSalesAccountName { get; set; }

            // Landbruk m.m.
            public string SupplierStandardAccount { get; set; }
            public string SupplierStandardAccountAgricultureDepartment { get; set; }
            public string SupplierEHFCoding { get; set; }
            public string SupplierApprover { get; set; }
            public int? SubmitAutomaticallyForApproval { get; set; } // 0/1

            // Lagerbeholdning / GTIN
            public string ProductGTIN { get; set; }
            public decimal? ProductsOnHand { get; set; }
            public bool? IsActive { get; set; } // True / False
        

    }
}
