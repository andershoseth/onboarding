namespace onboarding_backend.Models.StandardImport
{
    public class YTDPayrollBalance
    {

        public string SocialSecurityNumber { get; set; }       // Personnummer (11 tegn)
        public string InternationalIdNumber { get; set; }      // Brukes ved ikke-unikt FNr
        public string EmploymentRelationshipId { get; set; }   // Must exist
        public string YtdPayrollBalancesLineType { get; set; } // f.eks. PayItem, TaxTable, NetPayout, etc.
        public string PayItemCode { get; set; }                // lønnsartkode (krever "PayItem")
        public decimal? Amount { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? PrivateDrivenKilometers { get; set; }
        public decimal? HomeWorkKilometers { get; set; }
        public int? Year { get; set; } // Påkrevd

    }
}
