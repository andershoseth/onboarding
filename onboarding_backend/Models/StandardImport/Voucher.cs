namespace onboarding_backend
{
    public class Voucher
    {
        public int VoucherNo { get; set; }          // Ja (skille bilag)
        public string SaftBatchId { get; set; }
        public string SaftSourceId { get; set; }
        public string DocumentDate { get; set; }    // DDMMYYYY (Påkrevd)
        public string PostingDate { get; set; }     // DDMMYYYY (valgfritt)
        public int VoucherType { get; set; }        // Se egen tabell: 1,2,3,4...

        // Linjer
        public List<VoucherLine> Lines { get; set; } = new();
    }
}
