namespace onboarding_backend
{
    public class ProjectTeamMembers
    {
        public string ProjectCode { get; set; }   // Påkrevd
        public string SubprojectCode { get; set; }
        public string ProjectName { get; set; }
        public int EmployeeNo { get; set; }       // Påkrevd
        public string ContactName { get; set; }
        public decimal? Hours { get; set; }
        public decimal? HourlyRate { get; set; }
    }
}
