namespace onboarding_backend.Models.StandardImport
{
    public class Standardimport
    {

        public List<Contacts> Contact { get; set; } = new ();


        public List<Products> Product { get; set; } = new ();


        public List<Projects> Project { get; set; } = new ();

   
        public List<ProjectTeamMembers> ProjectTeamMember { get; set; } = new ();

      
        public List<ProjectActivity> ProjectActivitie { get; set; } = new ();


        public List<Departments> Department { get; set; } = new ();

  
        public List<Voucher> Voucher { get; set; } = new ();
        
        public List<Order> Order { get; set; } = new();
        public List<Quote> Quote { get; set; } = new();
        public List<InvoiceCid> InvoiceCid { get; set; } = new();
        public List<ChartOfAccount> ChartOfAccount { get; set; } = new();
        public List<FixedAsset> FixedAsset { get; set; } = new();
        public List<YTDPayrollBalance> YTDPayrollBalance { get; set; } = new();
        public List<SalaryBasis> SalaryBasis { get; set; } = new();
        public List<SalaryAdjustment> SalaryAdjustments { get; set; } = new();


    }
}

