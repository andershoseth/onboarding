namespace onboarding_backend.Models.StandardImport
{
    public class Departments
    {
        public string DepartmentCode { get; set; }  // Påkrevd
        public string DepartmentName { get; set; }
        public int? DepartmentManagerCode { get; set; }
        public string DepartmentManagerName { get; set; }
    }
}
