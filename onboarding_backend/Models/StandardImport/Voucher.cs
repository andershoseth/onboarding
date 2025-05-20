namespace onboarding_backend.Models.StandardImport
{
    public class Voucher
    {
        public int VoucherNo { get; set; }          
        public string? SaftBatchId { get; set; }
        public string? SaftSourceId { get; set; }
        public string? DocumentDate { get; set; }    
        public string? PostingDate { get; set; }    
        public string? VoucherType { get; set; }        

        // Linjer
        public List<VoucherLine> Lines { get; set; } = new();
    }
}
