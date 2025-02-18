using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;

namespace onboarding_backend
{
    public class SaftParser
    {
        /* <summary>
         Leser en SAF-T XML-fil og mapper dataene til StandardImport
         med lister for Contacts, Departments, Products, Projects, m.m.
         </summary>*/
        public string DetectDefaultNamespace(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Ingen fil opplastet.", nameof(file));

            // Les XML
            XDocument xdoc;
            using (var stream = file.OpenReadStream())
            {
                xdoc = XDocument.Load(stream);
            }

            // Hent root-elementet
            XElement root = xdoc.Root;

            // Hvis root er null (veldig uvanlig), returner tom streng
            return root?.Name.NamespaceName ?? string.Empty;
        }
        public async Task<StandardImport> ProcessSaftFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Ingen fil opplastet.", nameof(file));

            return await Task.Run(() =>
            {
                string detectedNs = DetectDefaultNamespace(file);
                XNamespace ns = string.IsNullOrEmpty(detectedNs) ? XNamespace.None : detectedNs; 

                XDocument xdoc;
                using (var stream = file.OpenReadStream())
                {
                    xdoc = XDocument.Load(stream);
                }

                var stdImport = new StandardImport();

                // =================== PARSE CONTACTS ===================
                var contactNodes = xdoc.Descendants(ns+"Contact");
                foreach (var c in contactNodes)
                {
                    var contactPerson = c.Element(ns + "ContactPerson");

                    var contact = new Contacts
                    {
                        // Identifikasjon
                        CustomerNo = (int?)c.Element(ns+"CustomerNo"),
                        SupplierNo = (int?)c.Element(ns + "SupplierNo"),
                        EmployeeNo = (int?)c.Element(ns + "EmployeeNo"),

                        // Navn/gruppe
                        ContactName = contactPerson != null
            ? $"{(string)contactPerson.Element(ns + "FirstName")} {(string)contactPerson.Element(ns + "LastName")}".Trim()
            : (string)c.Element(ns + "ContactName") ?? string.Empty,
                        ContactGroup = (string)c.Element(ns + "ContactGroup"),

                        // Tidsstempler
                        CustomerSince = (string)c.Element(ns + "CustomerSince"),
                        SupplierSince = (string)c.Element(ns + "SupplierSince"),
                        EmployeeSince = (string)c.Element(ns + "EmployeeSince"),

                        // MVA
                        IsVatFree = (int?)c.Element(ns + "IsVatFree"),

                        // Generell kontaktinfo
                        Phone = (string)c.Element(ns + "Phone"),
                        Email = (string)c.Element(ns + "Email"),
                        Web = (string)c.Element(ns + "Web"),

                        // Organisasjonsinfo
                        OrganizationNo = (string)c.Element(ns + "OrganizationNo"),

                        // Postadresse
                        MailAddress1 = (string)c.Element(ns + "MailAddress1"),
                        MailAddress2 = (string)c.Element(ns + "MailAddress2"),
                        MailPostcode = (string)c.Element(ns+"MailPostcode"),
                        MailCity = (string)c.Element(ns + "MailCity"),
                        MailCountry = (string)c.Element(ns +"MailCountry"),

                        // Leveringsadresse
                        DeliveryAddress1 = (string)c.Element(ns + "DeliveryAddress1"),
                        DeliveryAddress2 = (string)c.Element(ns +"DeliveryAddress2"),
                        DeliveryPostcode = (string)c.Element(ns+"DeliveryPostcode"),
                        DeliveryCity = (string)c.Element(ns+"DeliveryCity"),
                        DeliveryCountry = (string)c.Element(ns +"DeliveryCountry"),

                        // Bank
                        BankAccount = (string)c.Element(ns +"BankAccount"),
                        IBAN = (string)c.Element(ns +"IBAN"),
                        SWIFT = (string)c.Element(ns +"SWIFT"),

                        // Kontaktperson
                        ContactPersonFirstName = (string)c.Element(ns +"ContactPersonFirstName"),
                        ContactPersonLastName = (string)c.Element(ns +"ContactPersonLastName"),
                        ContactPersonPhone = (string)c.Element(ns + "ContactPersonPhone"),
                        ContactPersonEmail = (string)c.Element(ns + "ContactPersonEmail"),

                        // Faktura
                        InvoiceDelivery = (int?)c.Element(ns + "InvoiceDelivery"),
                        InvoiceEmailAddress = (string)c.Element(ns + "InvoiceEmailAddress"),

                        // Privatperson / ansatt
                        IsPerson = (int?)c.Element("IsPerson"),
                        SocialSecurityNumber = (string)c.Element(ns + "SocialSecurityNumber"),
                        InternationalIdNumber = (string)c.Element(ns +"InternationalIdNumber"),
                        InternationalIdCountryCode = (string)c.Element(ns + "InternationalIdCountryCode"),
                        InternationalIdType = (int?)c.Element(ns + "InternationalIdType"),
                        DateOfBirth = (string)c.Element( ns+ "DateOfBirth"),

                        // Ansatt-detaljer
                        JobTitle = (string)c.Element(ns+     "JobTitle"),
                        DepartmentCode = (string)c.Element(ns + "DepartmentCode"),
                        DepartmentName = (string)c.Element(ns + "DepartmentName"),
                        PayslipEmail = (string)c.Element(ns + "PayslipEmail"),
                        OwedHolidayPayForLastYear = (decimal?)c.Element(ns + "OwedHolidayPayForLastYear"),
                        OwedHolidayPayForLastYearOver60 = (decimal?)c.Element(ns + "OwedHolidayPayForLastYearOver60"),

                        // FNO
                        IncludeInFnoReport = (int?)c.Element(ns + "IncludeInFnoReport"),
                        RemunerationPeriod = (string)c.Element(ns + "RemunerationPeriod"),

                        // Kunde-rabatt
                        CustomerDiscount = (int?)c.Element(ns + "CustomerDiscount"),

                        // Leverandørspesifikke
                        SupplierStandardAccount = (int?)c.Element(ns + "SupplierStandardAccount"),
                        SupplierStandardAccountAgricultureDepartment = (int?)c.Element( ns+"SupplierStandardAccountAgricultureDepartment"),
                        SupplierEHFCoding = (string)c.Element(ns + "SupplierEHFCoding"),
                        SupplierApprover = (string)c.Element(ns + "SupplierApprover"),
                        SubmitAutomaticallyForApproval = (int?)c.Element(ns + "SubmitAutomaticallyForApproval")
                    };

                    stdImport.Contacts.Add(contact);
                }

                // =================== PARSE DEPARTMENTS ===================
                var deptNodes = xdoc.Descendants(ns+   "Department");
                foreach (var d in deptNodes)
                {
                    var dept = new Departments
                    {
                        DepartmentCode = (string)d.Element(ns+ "DepartmentCode"),
                        DepartmentName = (string)d.Element(ns + "DepartmentName"),
                        DepartmentManagerCode = (int?)d.Element(ns+"DepartmentManagerCode"),
                        DepartmentManagerName = (string)d.Element(ns+"DepartmentManagerName")
                    };

                    stdImport.Departments.Add(dept);
                }

                // =================== PARSE PRODUCTS ===================
                var productNodes = xdoc.Descendants(ns+"Product");
                foreach (var p in productNodes)
                {
                    var product = new Products
                    {
                        ProductCode = (string)p.Element(ns + "ProductCode"),
                        ProductName = (string)p.Element(ns + "ProductName"),
                        ProductGroup = (string)p.Element(ns +"ProductGroup"),
                        ProductDescription = (string)p.Element(ns + "ProductDescription"),
                        ProductType = (int?)p.Element(ns +"ProductType"),
                        ProductUnit = (string)p.Element(ns + "ProductUnit"),
                        ProductSalesPrice = (decimal?)p.Element(ns + "ProductSalesPrice"),
                        ProductCostPrice = (decimal?)p.Element(ns + "ProductCostPrice"),
                        ProductSalesAccount = (int?)(p.Element(ns + "ProductSalesAccount")) ?? 0,
                        ProductSalesAccountName = (string)p.Element(ns + "ProductSalesAccountName"),
                        ProductAltSalesAccount = (int?)p.Element(ns + "ProductAltSalesAccount"),
                        ProductAltSalesAccountName = (string)p.Element(ns + "ProductAltSalesAccountName"),
                        ProductGTIN = (string)p.Element(ns + "ProductGTIN"),

                        // Supplier/EHF
                        SupplierStandardAccount = (string)p.Element(ns + "SupplierStandardAccount"),
                        SupplierStandardAccountAgricultureDepartment = (string)p.Element(ns + "SupplierStandardAccountAgricultureDepartment"),
                        SupplierEHFCoding = (string)p.Element(ns + "SupplierEHFCoding"),
                        SupplierApprover = (string)p.Element(ns + "SupplierApprover"),
                        SubmitAutomaticallyForApproval = (int?)p.Element(ns + "SubmitAutomaticallyForApproval"),

                        // Lager
                        ProductsOnHand = (decimal?)p.Element(ns + "ProductsOnHand"),
                        // IsActive i PDF “True/False” → parse:
                        IsActive = ((string)p.Element(ns + "IsActive"))?.ToLower() == "false" ? false : true
                    };

                    stdImport.Products.Add(product);
                }

                // =================== PARSE PROJECTS ===================
                var projectNodes = xdoc.Descendants(ns + "Project");
                foreach (var prj in projectNodes)
                {
                    var project = new Projects
                    {
                        ProjectCode = (string)prj.Element(ns + "ProjectCode"),
                        ProjectName = (string)prj.Element(ns + "ProjectName"),
                        SubprojectCode = (string)prj.Element(ns + "SubprojectCode"),

                        ProjectManagerCode = (string)prj.Element(ns + "ProjectManagerCode"),
                        ProjectManagerName = (string)prj.Element(ns + "ProjectManagerName"),
                        ProjectContactPerson = (string)prj.Element(ns + "ProjectContactPerson"),
                        ContactPerson = (string)prj.Element(ns + "ContactPerson"),
                        ProjectBillable = (int?)prj.Element(ns + "ProjectBillable"),
                        CustomerNo = (int?)prj.Element(ns + "CustomerNo"),
                        ContactName = (string)prj.Element(ns + "ContactName"),
                        ProjectStartDate = (string)prj.Element(ns + "ProjectStartDate"),
                        ProjectEndDate = (string)prj.Element(ns + "ProjectEndDate"),
                        ProjectStatus = (int?)prj.Element(ns + "ProjectStatus"),
                        ProjectBrandingThemeCode = (string)prj.Element(ns + "ProjectBrandingThemeCode"),
                        FixedPrice = (decimal?)prj.Element(ns + "FixedPrice"),
                        Progress = (int?)prj.Element(ns + "Progress"),
                        ProjectBillingMethod = (string)prj.Element(ns + "ProjectBillingMethod"),
                        DepartmentCode = (string)prj.Element(ns + "DepartmentCode"),
                        DepartmentName = (string)prj.Element(ns + "DepartmentName"),
                        ProjectHourlyRateSpecification = (string)prj.Element(ns + "ProjectHourlyRateSpecification"),
                        ProjectHourlyRate = (decimal?)prj.Element(ns + "ProjectHourlyRate"),
                        AttachVouchersToInvoice = (int?)prj.Element(ns + "AttachVouchersToInvoice"),
                        AllowAllEmployees = (int?)prj.Element(ns + "AllowAllEmployees"),
                        AllowAllActivities = (int?)prj.Element(ns + "AllowAllActivities"),
                        Hours = (decimal?)prj.Element(ns + "Hours"),
                        HourlyRate = (decimal?)prj.Element(ns + "HourlyRate"),
                        TimeRevenues = (decimal?)prj.Element(ns + "TimeRevenues"),
                        Revenues = (decimal?)prj.Element(ns + "Revenues"),
                        CostOfGoods = (decimal?)prj.Element(ns + "CostOfGoods"),
                        PayrollExpenses = (decimal?)prj.Element(ns + "PayrollExpenses"),
                        OtherExpenses = (decimal?)prj.Element(ns + "OtherExpenses"),
                        IsExpenseMarkupEnabled = (int?)prj.Element(ns + "IsExpenseMarkupEnabled"),
                        MarkupExpensesByFactor = (decimal?)prj.Element(ns + "MarkupExpensesByFactor"),
                        ExpenseMarkupDescription = (string)prj.Element(ns + "ExpenseMarkupDescription"),
                        IsFeeMarkupEnabled = (int?)prj.Element(ns + "IsFeeMarkupEnabled"),
                        MarkupFeesByFactor = (decimal?)prj.Element(ns + "MarkupFeesByFactor"),
                        FeeMarkupDescription = (string)prj.Element(ns + "FeeMarkupDescription"),
                        LocationCode = (string)prj.Element(ns + "LocationCode"),
                        LocationName = (string)prj.Element(ns + "LocationName"),
                        IsInternal = (string)prj.Element(ns + "IsInternal")
                    };

                    stdImport.Projects.Add(project);
                }

                // =================== PARSE PROJECT TEAM MEMBERS ===================
                var ptmNodes = xdoc.Descendants(ns + "ProjectTeamMember");
                foreach (var ptm in ptmNodes)
                {
                    var member = new ProjectTeamMembers
                    {
                        ProjectCode = (string)ptm.Element(ns + "ProjectCode"),
                        SubprojectCode = (string)ptm.Element(ns + "SubprojectCode"),
                        ProjectName = (string)ptm.Element(ns + "ProjectName"),
                        EmployeeNo = (int)ptm.Element(ns + "EmployeeNo"),
                        ContactName = (string)ptm.Element(ns + "ContactName"),
                        Hours = (decimal?)ptm.Element(ns + "Hours"),
                        HourlyRate = (decimal?)ptm.Element(ns + "HourlyRate")
                    };

                    stdImport.ProjectTeamMembers.Add(member);
                }

                // =================== PARSE PROJECT ACTIVITIES ===================
                var paNodes = xdoc.Descendants(ns + "ProjectActivity");
                foreach (var a in paNodes)
                {
                    var activity = new ProjectActivity
                    {
                        ProjectCode = (string)a.Element(ns + "ProjectCode"),
                        SubprojectCode = (string)a.Element(ns + "SubprojectCode"),
                        ProjectName = (string)a.Element(ns + "ProjectName"),
                        ActivityCode = (string)a.Element(ns + "ActivityCode"),
                        ActivityName = (string)a.Element(ns + "ActivityName"),
                        ProjectBillable = (int?)a.Element(ns + "ProjectBillable"),
                        HourlyRate = (decimal?)a.Element(ns + "HourlyRate")
                    };

                    stdImport.ProjectActivities.Add(activity);
                }

                // =================== PARSE VOUCHERS ===================
                var voucherNodes = xdoc.Descendants(ns + "Voucher");
                foreach (var v in voucherNodes)
                {
                    var voucher = new Voucher
                    {
                        VoucherNo = (int?)v.Element(ns + "VoucherNo") ?? 0,
                        SaftBatchId = (string)v.Element(ns + "SaftBatchId"),
                        SaftSourceId = (string)v.Element(ns + "SaftSourceId"),
                        DocumentDate = (string)v.Element(ns + "DocumentDate"),
                        PostingDate = (string)v.Element(ns + "PostingDate"),
                        VoucherType = (int?)v.Element(ns + "VoucherType") ?? 0
                    };

                    // parse lines
                    var lineNodes = v.Descendants(ns + "Line");
                    foreach (var ln in lineNodes)
                    {
                        var line = new VoucherLine
                        {
                            Account = (int?)ln.Element(ns + "Account"),
                            AccountName = (string)ln.Element(ns + "AccountName"),
                            AccountAgricultureDepartment = (string)ln.Element(ns + "AccountAgricultureDepartment"),
                            VAT = (string)ln.Element(ns + "VAT"),
                            VATReturnSpecification = (string)ln.Element(ns + "VATReturnSpecification"),

                            Amount = (decimal?)ln.Element(ns + "Amount"),
                            Currency = (string)ln.Element(ns + "Currency"),
                            CurrencyAmount = (decimal?)ln.Element(ns + "CurrencyAmount"),
                            Discount = (decimal?)ln.Element(ns + "Discount"),
                            Quantity = (decimal?)ln.Element(ns + "Quantity"),

                            InvoiceNo = (int?)ln.Element(ns + "InvoiceNo"),
                            CID = (string)ln.Element(ns + "CID"),
                            DueDate = (string)ln.Element(ns + "DueDate"),
                            Description = (string)ln.Element(ns + "Description"),
                            Reference = (string)ln.Element(ns + "Reference"),

                            SalesPersonEmployeeNo = (int?)ln.Element(ns + "SalesPersonEmployeeNo"),
                            SalesPersonName = (string)ln.Element(ns + "SalesPersonName"),
                            Accrual = (int?)ln.Element(ns + "Accrual"),

                            CustomerNo = (int?)ln.Element(ns + "CustomerNo"),
                            SupplierNo = (int?)ln.Element(ns + "SupplierNo"),
                            EmployeeNo = (int?)ln.Element(ns + "EmployeeNo"),
                            ContactName = (string)ln.Element(ns + "ContactName"),
                            ContactGroup = (string)ln.Element(ns + "ContactGroup"),
                            CustomerSince = (string)ln.Element(ns + "CustomerSince"),
                            SupplierSince = (string)ln.Element(ns + "SupplierSince"),
                            EmployeeSince = (string)ln.Element(ns + "EmployeeSince"),
                            IsVatFree = (int?)ln.Element(ns + "IsVatFree"),

                            Phone = (string)ln.Element(ns + "Phone"),
                            Email = (string)ln.Element(ns + "Email"),
                            Web = (string)ln.Element(ns + "Web"),
                            OrganizationNo = (string)ln.Element(ns + "OrganizationNo"),

                            MailAddress1 = (string)ln.Element(ns + "MailAddress1"),
                            MailAddress2 = (string)ln.Element(ns + "MailAddress2"),
                            MailPostcode = (string)ln.Element(ns + "MailPostcode"),
                            MailCity = (string)ln.Element(ns + "MailCity"),
                            MailCountry = (string)ln.Element(ns + "MailCountry"),

                            DeliveryAddress1 = (string)ln.Element(ns + "DeliveryAddress1"),
                            DeliveryAddress2 = (string)ln.Element(ns + "DeliveryAddress2"),
                            DeliveryPostcode = (string)ln.Element(ns + "DeliveryPostcode"),
                            DeliveryCity = (string)ln.Element(ns + "DeliveryCity"),
                            DeliveryCountry = (string)ln.Element(ns + "DeliveryCountry"),

                            BankAccount = (string)ln.Element(ns + "BankAccount"),
                            IBAN = (string)ln.Element(ns + "IBAN"),
                            SWIFT = (string)ln.Element(ns + "SWIFT"),

                            ContactPersonFirstName = (string)ln.Element(ns + "ContactPersonFirstName"),
                            ContactPersonLastName = (string)ln.Element(ns + "ContactPersonLastName"),
                            ContactPersonPhone = (string)ln.Element(ns + "ContactPersonPhone"),
                            ContactPersonEmail = (string)ln.Element(ns + "ContactPersonEmail"),

                            ProductCode = (string)ln.Element(ns + "ProductCode"),
                            ProductName = (string)ln.Element(ns + "ProductName"),
                            ProductGroup = (string)ln.Element(ns + "ProductGroup"),
                            ProductDescription = (string)ln.Element(ns + "ProductDescription"),
                            ProductType = (int?)ln.Element(ns + "ProductType"),
                            ProductUnit = (string)ln.Element(ns + "ProductUnit"),
                            ProductSalesPrice = (decimal?)ln.Element(ns + "ProductSalesPrice"),
                            ProductCostPrice = (decimal?)ln.Element(ns + "ProductCostPrice"),
                            ProductSalesAccount = (int?)ln.Element(ns + "ProductSalesAccount"),
                            ProductSalesAccountName = (string)ln.Element(ns + "ProductSalesAccountName"),
                            ProductAltSalesAccount = (int?)ln.Element(ns + "ProductAltSalesAccount"),
                            ProductAltSalesAccountName = (string)ln.Element(ns + "ProductAltSalesAccountName"),
                            ProductGTIN = (string)ln.Element(ns + "ProductGTIN"),

                            ProjectCode = (string)ln.Element(ns + "ProjectCode"),
                            SubprojectCode = (string)ln.Element(ns + "SubprojectCode"),
                            ProjectName = (string)ln.Element(ns + "ProjectName"),
                            ProjectManagerCode = (string)ln.Element(ns + "ProjectManagerCode"),
                            ProjectManagerName = (string)ln.Element(ns + "ProjectManagerName"),
                            ProjectBillable = (int?)ln.Element(ns + "ProjectBillable"),
                            ProjectStartDate = (string)ln.Element(ns + "ProjectStartDate"),
                            ProjectEndDate = (string)ln.Element(ns + "ProjectEndDate"),
                            ProjectStatus = (int?)ln.Element(ns + "ProjectStatus"),
                            DepartmentCode = (string)ln.Element(ns + "DepartmentCode"),
                            DepartmentName = (string)ln.Element(ns + "DepartmentName"),
                            InvoiceDelivery = (int?)ln.Element(ns + "InvoiceDelivery"),

                            CustomerDiscount = (int?)ln.Element(ns + "CustomerDiscount"),
                            CustomMatchingReference = (string)ln.Element(ns + "CustomMatchingReference")
                        };

                        voucher.Lines.Add(line);
                    }

                    stdImport.Vouchers.Add(voucher);
                }

                // ===================== ORDERS =====================
                var orderNodes = xdoc.Descendants(ns + "Order");
                foreach (var o in orderNodes)
                {
                    var order = new Order
                    {
                        OrderNo = (int?)o.Element(ns + "OrderNo"),
                        OrderDate = (string)o.Element(ns + "OrderDate"),
                        RecurringInvoiceActive = (int?)o.Element(ns + "RecurringInvoiceActive"),
                        RecurringInvoiceRepeatTimes = (int?)o.Element(ns + "RecurringInvoiceRepeatTimes"),
                        RecurringInvoiceEndDate = (string)o.Element(ns + "RecurringInvoiceEndDate"),
                        RecurringInvoiceSendMethod = (int?)o.Element(ns + "RecurringInvoiceSendMethod"),
                        RecurringInvoiceSendFrequency = (int?)o.Element(ns + "RecurringInvoiceSendFrequency"),
                        RecurringInvoiceSendFrequencyUnit = (int?)o.Element(ns + "RecurringInvoiceSendFrequencyUnit"),
                        NextRecurringInvoiceDate = (string)o.Element(ns + "NextRecurringInvoiceDate"),

                        // Selger / Prosjekt
                        SalesPersonEmployeeNo = (int?)o.Element(ns + "SalesPersonEmployeeNo"),
                        SalesPersonName = (string)o.Element(ns + "SalesPersonName"),
                        ProjectCode = (string)o.Element(ns + "ProjectCode"),
                        SubprojectCode = (string)o.Element(ns + "SubprojectCode"),
                        ProjectName = (string)o.Element(ns + "ProjectName"),
                        ProjectManagerCode = (string)o.Element(ns + "ProjectManagerCode"),
                        ProjectManagerName = (string)o.Element(ns + "ProjectManagerName"),
                        ProjectBillable = (int?)o.Element(ns + "ProjectBillable"),
                        ProjectStartDate = (string)o.Element(ns + "ProjectStartDate"),
                        ProjectEndDate = (string)o.Element(ns + "ProjectEndDate"),
                        ProjectStatus = (int?)o.Element(ns + "ProjectStatus"),
                        ProjectContactPerson = (string)o.Element(ns + "ProjectContactPerson"),

                        // Avdeling
                        DepartmentCode = (string)o.Element(ns + "DepartmentCode"),
                        DepartmentName = (string)o.Element(ns + "DepartmentName"),
                        DepartmentManagerCode = (int?)o.Element(ns + "DepartmentManagerCode"),
                        DepartmentManagerName = (string)o.Element(ns + "DepartmentManagerName"),

                        // Reskontro / kunde
                        CustomerNo = (int?)o.Element(ns + "CustomerNo"),
                        ContactName = (string)o.Element(ns + "ContactName"),
                        ContactGroup = (string)o.Element(ns + "ContactGroup"),
                        CustomerSince = (string)o.Element(ns + "CustomerSince"),
                        IsVatFree = (int?)o.Element(ns + "IsVatFree"),
                        Phone = (string)o.Element(ns + "Phone"),
                        Email = (string)o.Element(ns + "Email"),
                        Web = (string)o.Element(ns + "Web"),
                        OrganizationNo = (string)o.Element(ns + "OrganizationNo"),

                        // Postadresse
                        MailAddress1 = (string)o.Element(ns + "MailAddress1"),
                        MailAddress2 = (string)o.Element(ns + "MailAddress2"),
                        MailPostcode = (string)o.Element(ns + "MailPostcode"),
                        MailCity = (string)o.Element(ns + "MailCity"),
                        MailCountry = (string)o.Element(ns + "MailCountry"),

                        // Leveringsadresse
                        DeliveryAddress1 = (string)o.Element(ns + "DeliveryAddress1"),
                        DeliveryAddress2 = (string)o.Element(ns + "DeliveryAddress2"),
                        DeliveryPostcode = (string)o.Element(ns + "DeliveryPostcode"),
                        DeliveryCity = (string)o.Element(ns + "DeliveryCity"),
                        DeliveryCountry = (string)o.Element(ns + "DeliveryCountry"),

                        // Bank
                        BankAccount = (string)o.Element(ns + "BankAccount"),
                        IBAN = (string)o.Element(ns + "IBAN"),
                        SWIFT = (string)o.Element(ns + "SWIFT"),

                        // Faktura
                        InvoiceDelivery = (int?)o.Element(ns + "InvoiceDelivery"),
                        ContactPersonFirstName = (string)o.Element(ns + "ContactPersonFirstName"),
                        ContactPersonLastName = (string)o.Element(ns + "ContactPersonLastName"),
                        ContactPersonPhone = (string)o.Element(ns + "ContactPersonPhone"),
                        ContactPersonEmail = (string)o.Element(ns + "ContactPersonEmail"),

                        // Ekstra
                        Reference = (string)o.Element(ns + "Reference"),
                        PaymentTerms = (int?)o.Element(ns + "PaymentTerms"),
                        MergeWithPreviousOrder = (int?)o.Element(ns + "MergeWithPreviousOrder"),
                        Currency = (string)o.Element(ns + "Currency"),

                        // Produkt & linjer
                        ProductCode = (string)o.Element(ns + "ProductCode"),
                        ProductName = (string)o.Element(ns + "ProductName"),
                        ProductGroup = (string)o.Element(ns + "ProductGroup"),
                        ProductDescription = (string)o.Element(ns + "ProductDescription"),
                        ProductType = (int?)o.Element(ns + "ProductType"),
                        ProductUnit = (string)o.Element(ns + "ProductUnit"),
                        ProductSalesPrice = (decimal?)o.Element(ns + "ProductSalesPrice"),
                        ProductCostPrice = (decimal?)o.Element(ns + "ProductCostPrice"),
                        ProductSalesAccount = (int?)o.Element(ns + "ProductSalesAccount"),
                        ProductSalesAccountName = (string)o.Element(ns + "ProductSalesAccountName"),
                        ProductAltSalesAccount = (int?)o.Element(ns + "ProductAltSalesAccount"),
                        ProductAltSalesAccountName = (string)o.Element(ns + "ProductAltSalesAccountName"),
                        ProductGTIN = (string)o.Element(ns + "ProductGTIN"),
                        Discount = (decimal?)o.Element(ns + "Discount"),
                        Quantity = (decimal?)o.Element(ns + "Quantity"),
                        Description = (string)o.Element(ns + "Description"),
                        OrderLineUnitPrice = (decimal?)o.Element(ns + "OrderLineUnitPrice"),
                        SortOrder = (int?)o.Element(ns + "SortOrder"),
                        VATReturnSpecification = (string)o.Element(ns + "VATReturnSpecification")
                    };
                    stdImport.Orders.Add(order);
                }

                // ===================== QUOTES =====================
                var quoteNodes = xdoc.Descendants(ns + "Quote");
                foreach (var q in quoteNodes)
                {
                    var quote = new Quote
                    {
                        QuoteNo = (int?)q.Element(ns + "QuoteNo"),
                        QuoteDate = (string)q.Element(ns + "QuoteDate"),
                        QuoteExpiryDate = (string)q.Element(ns + "QuoteExpiryDate"),

                        // Selger / prosjekt
                        SalesPersonEmployeeNo = (int?)q.Element(ns + "SalesPersonEmployeeNo"),
                        SalesPersonName = (string)q.Element(ns + "SalesPersonName"),
                        ProjectCode = (string)q.Element(ns + "ProjectCode"),
                        SubprojectCode = (string)q.Element(ns + "SubprojectCode"),
                        ProjectName = (string)q.Element(ns + "ProjectName"),
                        ProjectManagerCode = (string)q.Element(ns + "ProjectManagerCode"),
                        ProjectManagerName = (string)q.Element(ns + "ProjectManagerName"),
                        ProjectBillable = (int?)q.Element(ns + "ProjectBillable"),
                        ProjectStartDate = (string)q.Element(ns + "ProjectStartDate"),
                        ProjectEndDate = (string)q.Element(ns + "ProjectEndDate"),
                        ProjectStatus = (int?)q.Element(ns + "ProjectStatus"),
                        ProjectContactPerson = (string)q.Element(ns + "ProjectContactPerson"),

                        // Avdeling
                        DepartmentCode = (string)q.Element(ns + "DepartmentCode"),
                        DepartmentName = (string)q.Element(ns + "DepartmentName"),
                        DepartmentManagerCode = (int?)q.Element(ns + "DepartmentManagerCode"),
                        DepartmentManagerName = (string)q.Element(ns + "DepartmentManagerName"),

                        // Kundesiden
                        CustomerNo = (int?)q.Element(ns + "CustomerNo"),
                        ContactName = (string)q.Element(ns + "ContactName"),
                        ContactGroup = (string)q.Element(ns + "ContactGroup"),
                        CustomerSince = (string)q.Element(ns + "CustomerSince"),
                        IsVatFree = (int?)q.Element(ns + "IsVatFree"),
                        Phone = (string)q.Element(ns + "Phone"),
                        Email = (string)q.Element(ns + "Email"),
                        Web = (string)q.Element(ns + "Web"),
                        OrganizationNo = (string)q.Element(ns + "OrganizationNo"),

                        // Postadresse
                        MailAddress1 = (string)q.Element(ns + "MailAddress1"),
                        MailAddress2 = (string)q.Element(ns + "MailAddress2"),
                        MailPostcode = (string)q.Element(ns + "MailPostcode"),
                        MailCity = (string)q.Element(ns + "MailCity"),
                        MailCountry = (string)q.Element(ns + "MailCountry"),

                        // Leveringsadresse
                        DeliveryAddress1 = (string)q.Element(ns + "DeliveryAddress1"),
                        DeliveryAddress2 = (string)q.Element(ns + "DeliveryAddress2"),
                        DeliveryPostcode = (string)q.Element(ns + "DeliveryPostcode"),
                        DeliveryCity = (string)q.Element(ns + "DeliveryCity"),
                        DeliveryCountry = (string)q.Element(ns + "DeliveryCountry"),

                        // Bank
                        BankAccount = (string)q.Element(ns + "BankAccount"),
                        IBAN = (string)q.Element(ns + "IBAN"),
                        SWIFT = (string)q.Element(ns + "SWIFT"),

                        InvoiceDelivery = (int?)q.Element(ns + "InvoiceDelivery"),

                        // Kontaktperson
                        ContactPersonFirstName = (string)q.Element(ns + "ContactPersonFirstName"),
                        ContactPersonLastName = (string)q.Element(ns + "ContactPersonLastName"),
                        ContactPersonPhone = (string)q.Element(ns + "ContactPersonPhone"),
                        ContactPersonEmail = (string)q.Element(ns + "ContactPersonEmail"),

                        // Produktlinjer
                        ProductCode = (string)q.Element(ns + "ProductCode"),
                        ProductName = (string)q.Element(ns + "ProductName"),
                        ProductGroup = (string)q.Element(ns + "ProductGroup"),
                        ProductDescription = (string)q.Element(ns + "ProductDescription"),
                        ProductType = (int?)q.Element(ns + "ProductType"),
                        ProductUnit = (string)q.Element(ns + "ProductUnit"),
                        ProductSalesPrice = (decimal?)q.Element(ns + "ProductSalesPrice"),
                        ProductCostPrice = (decimal?)q.Element(ns + "ProductCostPrice"),
                        ProductSalesAccount = (int?)q.Element(ns + "ProductSalesAccount"),
                        ProductSalesAccountName = (string)q.Element(ns + "ProductSalesAccountName"),
                        ProductAltSalesAccount = (int?)q.Element(ns + "ProductAltSalesAccount"),
                        ProductAltSalesAccountName = (string)q.Element(ns + "ProductAltSalesAccountName"),
                        ProductGTIN = (string)q.Element(ns + "ProductGTIN"),

                        Discount = (decimal?)q.Element(ns + "Discount"),
                        Quantity = (decimal?)q.Element(ns + "Quantity"),
                        Description = (string)q.Element(ns + "Description"),
                        QuoteLineUnitPrice = (decimal?)q.Element(ns + "QuoteLineUnitPrice"),
                        VATReturnSpecification = (string)q.Element(ns + "VATReturnSpecification")
                    };
                    stdImport.Quotes.Add(quote);
                }

                // ===================== INVOICECIDS =====================
                var cidNodes = xdoc.Descendants(ns + "InvoiceCid");
                foreach (var cNode in cidNodes)
                {
                    var cid = new InvoiceCid
                    {
                        InvoiceNo = (string)cNode.Element(ns + "InvoiceNo"),
                        CID = (string)cNode.Element(ns + "CID")
                    };
                    stdImport.InvoiceCids.Add(cid);
                }

                // ===================== CHARTOFACCOUNTS =====================
                var coaNodes = xdoc.Descendants(ns + "ChartOfAccounts");
                foreach (var co in coaNodes)
                {
                    var chart = new ChartOfAccount
                    {
                        Account = (int)co.Element(ns + "Account"),
                        AccountName = (string)co.Element(ns + "AccountName"),
                        AccountAgricultureDepartment = (string)co.Element(ns + "AccountAgricultureDepartment"),
                        VAT = (string)co.Element(ns + "VAT"),
                        VATReturnSpecification = (string)co.Element(ns + "VATReturnSpecification"),
                        BankAccount = (string)co.Element(ns + "BankAccount"),
                        IsProjectRequired = (int?)co.Element(ns + "IsProjectRequired"),
                        IsDepartmentRequired = (int?)co.Element(ns + "IsDepartmentRequired"),
                        IsLocationRequired = (int?)co.Element(ns + "IsLocationRequired"),
                        IsFixedAssetsRequired = (int?)co.Element(ns + "IsFixedAssetsRequired"),
                        IsEnterpriseRequired = (int?)co.Element(ns + "IsEnterpriseRequired"),
                        IsActivityRequired = (int?)co.Element(ns + "IsActivityRequired"),
                        IsDim1Required = (int?)co.Element(ns + "IsDim1Required"),
                        IsDim2Required = (int?)co.Element(ns + "IsDim2Required"),
                        IsDim3Required = (int?)co.Element(ns + "IsDim3Required"),
                        IsQuantityRequired = (int?)co.Element(ns + "IsQuantityRequired"),
                        IsQuantity2Required = (int?)co.Element(ns + "IsQuantity2Required"),
                        IsProductRequired = (int?)co.Element(ns + "IsProductRequired"),
                        IsAgricultureProductRequired = (int?)co.Element(ns + "IsAgricultureProductRequired"),
                        StandardProjectCode = (string)co.Element(ns + "StandardProjectCode"),
                        StandardDepartmentCode = (string)co.Element(ns + "StandardDepartmentCode"),
                        LockVatCode = (int?)co.Element(ns + "LockVatCode"),
                        IsActive = (int?)co.Element(ns + "IsActive")
                    };
                    stdImport.ChartOfAccounts.Add(chart);
                }

                // ===================== FIXEDASSET =====================
                var faNodes = xdoc.Descendants(ns + "FixedAsset");
                foreach (var fA in faNodes)
                {
                    var asset = new FixedAsset
                    {
                        AssetCode = (string)fA.Element(ns + "AssetCode"),
                        AssetName = (string)fA.Element(ns + "AssetName"),
                        AssetTypeName = (string)fA.Element(ns + "AssetTypeName"),
                        PurchaseDate = (string)fA.Element(ns + "PurchaseDate"),
                        PurchasePrice = (decimal?)fA.Element(ns + "PurchasePrice"),
                        DepreciationMethod = (string)fA.Element(ns + "DepreciationMethod"),
                        Rate = (decimal?)fA.Element(ns + "Rate"),
                        EconomicLife = (int?)fA.Element(ns + "EconomicLife"),
                        Deprecation0101 = (decimal?)fA.Element(ns + "Deprecation0101"),
                        YTDDepreciation = (decimal?)fA.Element(ns + "YTDDepreciation"),
                        LastDepreciation = (string)fA.Element(ns + "LastDepreciation"),
                        DepartmentCode = (string)fA.Element(ns + "DepartmentCode"),
                        ProjectCode = (string)fA.Element(ns + "ProjectCode"),
                        LocationCode = (string)fA.Element(ns + "LocationCode"),
                        SerialNumber = (string)fA.Element(ns + "SerialNumber")
                    };
                    stdImport.FixedAssets.Add(asset);
                }

                // ===================== YTD PAYROLL BALANCES =====================
                var ytdNodes = xdoc.Descendants(ns + "YTDPayrollBalance");
                foreach (var y in ytdNodes)
                {
                    var ytd = new YTDPayrollBalance
                    {
                        SocialSecurityNumber = (string)y.Element(ns + "SocialSecurityNumber"),
                        InternationalIdNumber = (string)y.Element(ns + "InternationalIdNumber"),
                        EmploymentRelationshipId = (string)y.Element(ns + "EmploymentRelationshipId"),
                        YtdPayrollBalancesLineType = (string)y.Element(ns + "YtdPayrollBalancesLineType"),
                        PayItemCode = (string)y.Element(ns + "PayItemCode"),
                        Amount = (decimal?)y.Element(ns + "Amount"),
                        Quantity = (decimal?)y.Element(ns + "Quantity"),
                        PrivateDrivenKilometers = (decimal?)y.Element(ns + "PrivateDrivenKilometers"),
                        HomeWorkKilometers = (decimal?)y.Element(ns + "HomeWorkKilometers"),
                        Year = (int?)y.Element(ns + "Year")
                    };
                    stdImport.YTDPayrollBalances.Add(ytd);
                }

                // ===================== SALARYBASIS =====================
                var sbNodes = xdoc.Descendants(ns + "SalaryBasis");
                foreach (var sB in sbNodes)
                {
                    var basis = new SalaryBasis
                    {
                        EmployeeNo = (int?)sB.Element(ns + "EmployeeNo"),
                        DepartmentCode = (string)sB.Element(ns + "DepartmentCode"),
                        ProjectCode = (string)sB.Element(ns + "ProjectCode"),
                        PayItemCode = (string)sB.Element(ns + "PayItemCode"),
                        Rate = (decimal?)sB.Element(ns + "Rate"),
                        Amount = (decimal?)sB.Element(ns + "Amount"),
                        Quantity = (decimal?)sB.Element(ns + "Quantity"),
                        Comment = (string)sB.Element(ns + "Comment"),
                        PersonType = (string)sB.Element(ns + "PersonType")
                    };
                    stdImport.SalaryBasis.Add(basis);
                }

                // ===================== SALARYADJUSTMENTS =====================
                var saNodes = xdoc.Descendants(ns + "SalaryAdjustment");
                foreach (var sA in saNodes)
                {
                    var adjust = new SalaryAdjustment
                    {
                        EmployeeNo = (int?)sA.Element(ns + "EmployeeNo"),
                        EmploymentRelationshipId = (string)sA.Element(ns + "EmploymentRelationshipId"),
                        RemunerationType = (string)sA.Element(ns + "RemunerationType"),
                        AnnualSalary = (decimal?)sA.Element(ns + "AnnualSalary"),
                        HourlyRate = (decimal?)sA.Element(ns + "HourlyRate"),
                        AdjustAnnualSalaryBy = (decimal?)sA.Element(ns + "AdjustAnnualSalaryBy"),
                        AdjustHourlyRateBy = (decimal?)sA.Element(ns + "AdjustHourlyRateBy"),
                        LastSalaryChangeDate = (string)sA.Element(ns + "LastSalaryChangeDate")
                    };
                    stdImport.SalaryAdjustments.Add(adjust);
                }

                // Returner det ferdige StandardImport-objektet
                return stdImport;
            });
        }
    }
}
