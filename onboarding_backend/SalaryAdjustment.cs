namespace onboarding_backend
{
    public class SalaryAdjustment
    {
     
            public int? EmployeeNo { get; set; }               // Påkrevd
            public string EmploymentRelationshipId { get; set; } // hvis flere aktive
            public string RemunerationType { get; set; }         // "fastlonn", "timelonn", "provisjonslonn"
            public decimal? AnnualSalary { get; set; }
            public decimal? HourlyRate { get; set; }
            public decimal? AdjustAnnualSalaryBy { get; set; }
            public decimal? AdjustHourlyRateBy { get; set; }
            public string LastSalaryChangeDate { get; set; }     // DDMMYYYY
        

    }
}
