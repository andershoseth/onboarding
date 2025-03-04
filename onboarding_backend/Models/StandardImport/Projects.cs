namespace onboarding_backend.Models.StandardImport
{
    public class Projects
    {
        // Obligatoriske felt
        public string ProjectCode { get; set; }   // Tekst (Påkrevd)
        public string ProjectName { get; set; }   // Tekst (Påkrevd)

        // Delprosjekt
        public string SubprojectCode { get; set; }

        // Prosjektleder, kontaktinfo, kunde
        public string ProjectManagerCode { get; set; }
        public string ProjectManagerName { get; set; }
        public string ProjectContactPerson { get; set; }
        public string ContactPerson { get; set; }
        public int? ProjectBillable { get; set; }    // 0/1
        public int? CustomerNo { get; set; }         // Numerisk
        public string ContactName { get; set; }      // Tekst

        // Datoer
        public string ProjectStartDate { get; set; } // DDMMYYYY
        public string ProjectEndDate { get; set; }   // DDMMYYYY

        // Status, branding, fastpris
        public int? ProjectStatus { get; set; }         // (Se egen tabell: 1=OnHold,2=InProgress,...)
        public string ProjectBrandingThemeCode { get; set; }
        public decimal? FixedPrice { get; set; }
        public int? Progress { get; set; }             // 0 – 100

        // Faktura og avdeling
        public string ProjectBillingMethod { get; set; } // (FixedPrice, Time, TimeAndExpenses)
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }

        // Timepris
        public string ProjectHourlyRateSpecification { get; set; } // Activity, Employee, Project
        public decimal? ProjectHourlyRate { get; set; }
        public int? AttachVouchersToInvoice { get; set; }   // 0/1
        public int? AllowAllEmployees { get; set; }         // 0/1
        public int? AllowAllActivities { get; set; }        // 0/1

        // Budsjetter
        public decimal? Hours { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? TimeRevenues { get; set; }
        public decimal? Revenues { get; set; }
        public decimal? CostOfGoods { get; set; }
        public decimal? PayrollExpenses { get; set; }
        public decimal? OtherExpenses { get; set; }

        // Markup
        public int? IsExpenseMarkupEnabled { get; set; } // 0/1
        public decimal? MarkupExpensesByFactor { get; set; }
        public string ExpenseMarkupDescription { get; set; }
        public int? IsFeeMarkupEnabled { get; set; }     // 0/1
        public decimal? MarkupFeesByFactor { get; set; }
        public string FeeMarkupDescription { get; set; }

        // Lokasjon
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string IsInternal { get; set; } // "Yes"/"No"
    }
}

