namespace onboarding_backend
{
    public class StandardImport
    {

        public List<Contacts> Contacts { get; set; } = new ();


        public List<Products> Products { get; set; } = new ();


        public List<Projects> Projects { get; set; } = new ();

   
        public List<ProjectTeamMembers> ProjectTeamMembers { get; set; } = new ();

      
        public List<ProjectActivity> ProjectActivities { get; set; } = new ();


        public List<Departments> Departments { get; set; } = new ();

  
        public List<Voucher> Vouchers { get; set; } = new ();
        public List<Order> Orders { get; set; } = new();
        public List<Quote> Quotes { get; set; } = new();
        public List<InvoiceCid> InvoiceCids { get; set; } = new();
        public List<ChartOfAccount> ChartOfAccounts { get; set; } = new();
        public List<FixedAsset> FixedAssets { get; set; } = new();
        public List<YTDPayrollBalance> YTDPayrollBalances { get; set; } = new();
        public List<SalaryBasis> SalaryBasis { get; set; } = new();
        public List<SalaryAdjustment> SalaryAdjustments { get; set; } = new();


    }
}

