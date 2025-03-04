namespace onboarding_backend.Models.StandardImport
{
    public class SalaryBasis
    {
     
            public int? EmployeeNo { get; set; }      // Påkrevd
            public string DepartmentCode { get; set; }
            public string ProjectCode { get; set; }
            public string PayItemCode { get; set; }   // Lønnsart (påkrevd)
            public decimal? Rate { get; set; }
            public decimal? Amount { get; set; }
            public decimal? Quantity { get; set; }
            public string Comment { get; set; }
            public string PersonType { get; set; } // f.eks. "norskPendler", "sokkelarbeider", etc.
    }

    
}
