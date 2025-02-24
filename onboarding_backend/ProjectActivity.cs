namespace onboarding_backend
{
    public class ProjectActivity
    {
        public string ProjectCode { get; set; }   // Påkrevd
        public string SubprojectCode { get; set; }
        public string ProjectName { get; set; }
        public string ActivityCode { get; set; }  // Påkrevd
        public string ActivityName { get; set; }
        public int? ProjectBillable { get; set; } // 0/1
        public decimal? HourlyRate { get; set; }
    }
}
