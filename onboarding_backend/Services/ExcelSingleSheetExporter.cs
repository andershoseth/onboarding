using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using onboarding_backend.Models.StandardImport;

namespace onboarding_backend.Services;

public static class ExcelSingleSheetExporter
{
    /// <summary>
    ///     Creates one .xlsx with a single worksheet, appending data from all 15 lists.
    ///     The "decorators" like [Contacts], [Products], etc. appear inline in the rows.
    ///     Then returns the .xlsx as a byte[].
    /// </summary>
    public static byte[] CreateSingleSheet(Standardimport data)
    {
        // Basic checks
        if (data == null) throw new ArgumentNullException(nameof(data));

        // Create workbook and single sheet
        IWorkbook workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet("FullData");

        // We'll keep a row index to know where to write next
        var rowIndex = 0;

        // 1) Contacts
        rowIndex = WriteContactsBlock(sheet, data.Contact, rowIndex);

        // 2) Products
        rowIndex = WriteProductsBlock(sheet, data.Product, rowIndex);

        // 3) Projects
        rowIndex = WriteProjectsBlock(sheet, data.Project, rowIndex);

        // 4) Project Team
        rowIndex = WriteProjectTeamBlock(sheet, data.ProjectTeamMember, rowIndex);

        // 5) Project Activities
        rowIndex = WriteProjectActivitiesBlock(sheet, data.ProjectActivitie, rowIndex);

        // 6) Departments
        rowIndex = WriteDepartmentsBlock(sheet, data.Department, rowIndex);

        // 7) Vouchers
        rowIndex = WriteVouchersBlock(sheet, data.Voucher, rowIndex);

        // 8) Orders
        rowIndex = WriteOrdersBlock(sheet, data.Order, rowIndex);

        // 9) Quotes
        rowIndex = WriteQuotesBlock(sheet, data.Quote, rowIndex);

        // 10) InvoiceCid
        rowIndex = WriteInvoiceCidBlock(sheet, data.InvoiceCid, rowIndex);

        // 11) ChartOfAccounts
        rowIndex = WriteChartOfAccountsBlock(sheet, data.ChartOfAccount, rowIndex);

        // 12) FixedAssets
        rowIndex = WriteFixedAssetsBlock(sheet, data.FixedAsset, rowIndex);

        // 13) YTD Payroll
        rowIndex = WriteYtdPayrollBlock(sheet, data.YTDPayrollBalance, rowIndex);

        // 14) SalaryBasis
        rowIndex = WriteSalaryBasisBlock(sheet, data.SalaryBasis, rowIndex);

        // 15) SalaryAdjustments
        rowIndex = WriteSalaryAdjustmentsBlock(sheet, data.SalaryAdjustments, rowIndex);

        // Convert workbook to byte[]
        using var ms = new MemoryStream();
        workbook.Write(ms);
        return ms.ToArray();
    }

    #region "Block" Writers

    /// <summary>
    ///     Writes a block for [Contacts] in the single sheet, returning the updated row index
    /// </summary>
    private static int WriteContactsBlock(ISheet sheet, IEnumerable<Contacts> contacts, int startRow)
    {
        // Decorator row: "[Contacts]"
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[Contacts]");

        // Header row (use the exact columns your importer expects)
        var headerRow = sheet.CreateRow(startRow++);
        var headers = new[]
        {
            "CustomerNo", "SupplierNo", "EmployeeNo", "ContactName", "ContactGroup", "CustomerSince", "SupplierSince",
            "EmployeeSince",
            "IsVatFree", "Phone", "Email", "Web", "OrganizationNo", "MailAddress1", "MailAddress2", "MailPostCode",
            "MailCity", "MailCountry", "DeliveryAddress1", "DeliveryAddress2", "DeliveryPostcode", "DeliveryCity",
            "DeliveryCountry", "BankAccount", "IBAN", "SWIFT", "ContactPersonFirstName", "ContactPersonLastName",
            "ContactPersonPhone", "ContactPersonEmail", "InvoiceDelivery", "InvoiceEmailAddress", "IsPerson",
            "SocialSecurityNumber",
            "InternationalIdNumber", "InternationalIdCountryCode", "InternationalIdType", "DateOfBirth", "JobTitle",
            "DepartmentCode",
            "DepartmentName", "PayslipEmail", "OwedHolidayPayForLastYear", "OwedHolidayPayForLastYearOver60",
            "IncludeInFnoReport", "RemunerationPeriod", "CustomerDiscount", "SupplierStandardAccount",
            "SupplierStandardAccountAgricultureDepartment",
            "SupplierEHFCoding", "SupplierApprover", "SubmitAutomaticallyForApproval"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        // Data rows
        foreach (var c in contacts)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(c.CustomerNo?.ToString() ?? "");
            row.CreateCell(1).SetCellValue(c.SupplierNo?.ToString() ?? "");
            row.CreateCell(2).SetCellValue(c.EmployeeNo?.ToString() ?? "");
            row.CreateCell(3).SetCellValue(c.ContactName ?? "");
            row.CreateCell(4).SetCellValue(c.ContactGroup ?? "");
            row.CreateCell(5).SetCellValue(c.CustomerSince ?? "");
            row.CreateCell(6).SetCellValue(c.SupplierSince ?? "");
            row.CreateCell(7).SetCellValue(c.EmployeeSince ?? "");
            row.CreateCell(8).SetCellValue(c.IsVatFree?.ToString() ?? "");
            row.CreateCell(9).SetCellValue(c.Phone ?? "");
            row.CreateCell(10).SetCellValue(c.Email ?? "");
            row.CreateCell(11).SetCellValue(c.Web ?? "");
            row.CreateCell(12).SetCellValue(c.OrganizationNo ?? "");
            row.CreateCell(13).SetCellValue(c.MailAddress1 ?? "");
            row.CreateCell(14).SetCellValue(c.MailAddress2 ?? "");
            row.CreateCell(15).SetCellValue(c.MailPostcode ?? "");
            row.CreateCell(16).SetCellValue(c.MailCity ?? "");
            row.CreateCell(17).SetCellValue(c.MailCountry ?? "");
            row.CreateCell(18).SetCellValue(c.DeliveryAddress1 ?? "");
            row.CreateCell(19).SetCellValue(c.DeliveryAddress2 ?? "");
            row.CreateCell(20).SetCellValue(c.DeliveryPostcode ?? "");
            row.CreateCell(21).SetCellValue(c.DeliveryCity ?? "");
            row.CreateCell(22).SetCellValue(c.DeliveryCountry ?? "");
            row.CreateCell(23).SetCellValue(c.BankAccount ?? "");
            row.CreateCell(24).SetCellValue(c.IBAN ?? "");
            row.CreateCell(25).SetCellValue(c.SWIFT ?? "");
            row.CreateCell(26).SetCellValue(c.ContactPersonFirstName ?? "");
            row.CreateCell(27).SetCellValue(c.ContactPersonLastName ?? "");
            row.CreateCell(28).SetCellValue(c.ContactPersonPhone ?? "");
            row.CreateCell(29).SetCellValue(c.ContactPersonEmail ?? "");
            row.CreateCell(30).SetCellValue((double?)c.InvoiceDelivery ?? 0.0);
            row.CreateCell(31).SetCellValue(c.InvoiceEmailAddress ?? "");
            row.CreateCell(32).SetCellValue((double?)c.IsPerson ?? 0.0);
            row.CreateCell(33).SetCellValue(c.SocialSecurityNumber ?? "");
            row.CreateCell(34).SetCellValue(c.InternationalIdNumber ?? "");
            row.CreateCell(35).SetCellValue(c.InternationalIdCountryCode ?? "");
            row.CreateCell(36).SetCellValue((double?)c.InternationalIdType ?? 0.0);
            row.CreateCell(37).SetCellValue(c.DateOfBirth ?? "");
            row.CreateCell(38).SetCellValue(c.JobTitle ?? "");
            row.CreateCell(39).SetCellValue(c.DepartmentCode ?? "");
            row.CreateCell(40).SetCellValue(c.DepartmentName ?? "");
            row.CreateCell(41).SetCellValue(c.PayslipEmail ?? "");
            row.CreateCell(42).SetCellValue((double?)c.OwedHolidayPayForLastYear ?? 0.0);
            row.CreateCell(43).SetCellValue((double?)c.OwedHolidayPayForLastYearOver60 ?? 0.0);
            row.CreateCell(44).SetCellValue((double?)c.IncludeInFnoReport ?? 0.0);
            row.CreateCell(45).SetCellValue(c.RemunerationPeriod ?? "");
            row.CreateCell(46).SetCellValue((double?)c.CustomerDiscount ?? 0.0);
            row.CreateCell(47).SetCellValue((double?)c.SupplierStandardAccount ?? 0.0);
            row.CreateCell(48).SetCellValue((double?)c.SupplierStandardAccountAgricultureDepartment ?? 0.0);
            row.CreateCell(49).SetCellValue(c.SupplierEHFCoding ?? "");
            row.CreateCell(50).SetCellValue(c.SupplierApprover ?? "");
            row.CreateCell(51).SetCellValue((double?)c.SubmitAutomaticallyForApproval ?? 0.0);
        }

        // Optionally add one blank row to separate this block from the next
        startRow++;

        return startRow;
    }

    private static int WriteProductsBlock(ISheet sheet, IEnumerable<Products> products, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[Products]");

        var headerRow = sheet.CreateRow(startRow++);
        var headers = new[]
        {
            "ProductCode", "ProductName", "ProductGroup", "ProductDescription", "ProductType", "ProductUnit",
            "ProductSalesPrice", "ProductSalesAccount", "ProductAltSalesAccount",
            "ProductAltSalesAccountName", "AccountAgricultureDepartment", "ProductGTIN", "ProductsOnHand", "IsActive"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var p in products)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(p.ProductCode ?? "");
            row.CreateCell(1).SetCellValue(p.ProductName ?? "");
            row.CreateCell(2).SetCellValue(p.ProductGroup ?? "");
            row.CreateCell(3).SetCellValue(p.ProductDescription ?? "");
            row.CreateCell(4).SetCellValue((double?)p.ProductType ?? 0.0);
            row.CreateCell(5).SetCellValue(p.ProductUnit ?? "");
            row.CreateCell(6).SetCellValue((double?)p.ProductSalesPrice ?? 0.0);
            row.CreateCell(7).SetCellValue((double?)p.ProductSalesAccount ?? 0.0);
            row.CreateCell(8).SetCellValue(p.ProductSalesAccountName ?? "");
            row.CreateCell(9).SetCellValue(p.ProductAltSalesAccount ?? 0.0);
            row.CreateCell(10).SetCellValue(p.ProductAltSalesAccountName ?? "");
            row.CreateCell(11).SetCellValue(p.SupplierStandardAccountAgricultureDepartment);
            row.CreateCell(12).SetCellValue(p.ProductGTIN ?? "");
            row.CreateCell(13).SetCellValue((double?)p.ProductsOnHand ?? 0.0);
            row.CreateCell(14).SetCellValue(p.IsActive ?? false);
        }

        startRow++;
        return startRow;
    }

    private static int WriteProjectsBlock(ISheet sheet, IEnumerable<Projects> projects, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[Projects]");


        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            "ProjectCode", "SubprojectCode", "ProjectName", "ProjectManagerCode", "ProjectManagerName",
            "ProjectContactPerson", "ContactPerson", "ProjectBillable", "CustomerNo", "ContactName", "ProjectStartDate",
            "ProjectEndDate", "ProjectStatus", "ProjectBrandingThemeCode", "FixedPrice", "Progress",
            "ProjectBillingMethod", "DepartmentCode", "DepartmentName", "ProjectHourlyRateSpecification",
            "ProjectHourlyRate", "AttachVouchersToInvoice", "AllowAllEmployees", "AllowAllActivities", "Hours",
            "HourlyRate", "TimeRevenues", "Revenues", "CostOfGoods", "PayrollExpenses", "OtherExpenses",
            "IsExpenseMarkupEnabled", "MarkupExpensesByFactor",
            "ExpenseMarkupDescription", "IsFeeMarkupEnabled", "MarkupFeesByFactor",
            "FeeMarkUpDescription", "LocationCode", "LocationName", "IsInternal"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var prj in projects)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(prj.ProjectCode ?? "");
            row.CreateCell(1).SetCellValue(prj.SubprojectCode ?? "");
            row.CreateCell(2).SetCellValue(prj.ProjectName ?? "");
            row.CreateCell(3).SetCellValue(prj.ProjectManagerCode ?? 0.0);
            row.CreateCell(4).SetCellValue(prj.ProjectManagerName ?? "");
            row.CreateCell(5).SetCellValue(prj.ProjectContactPerson ?? "");
            row.CreateCell(6).SetCellValue(prj.ContactPerson ?? "");
            row.CreateCell(7).SetCellValue(prj.ProjectBillable ?? 0.0);
            row.CreateCell(8).SetCellValue(prj.CustomerNo ?? 0.0);
            row.CreateCell(9).SetCellValue(prj.ContactName ?? "");
            row.CreateCell(10).SetCellValue(prj.ProjectStartDate);
            row.CreateCell(11).SetCellValue(prj.ProjectEndDate);
            row.CreateCell(12).SetCellValue(prj.ProjectStatus ?? 0.0);
            row.CreateCell(13).SetCellValue(prj.ProjectBrandingThemeCode ?? "");
            row.CreateCell(14).SetCellValue((double?)prj.FixedPrice ?? 0.0);
            row.CreateCell(15).SetCellValue((double?)prj.Progress ?? 0.0);
            row.CreateCell(16).SetCellValue(prj.ProjectBillingMethod ?? "");
            row.CreateCell(17).SetCellValue(prj.DepartmentCode ?? "");
            row.CreateCell(18).SetCellValue(prj.DepartmentName ?? "");
            row.CreateCell(19).SetCellValue(prj.ProjectHourlyRateSpecification ?? "");
            row.CreateCell(20).SetCellValue((double?)prj.ProjectHourlyRate ?? 0.0);
            row.CreateCell(21).SetCellValue(prj.AttachVouchersToInvoice ?? 0.0);
            row.CreateCell(22).SetCellValue(prj.AllowAllEmployees ?? 0.0);
            row.CreateCell(23).SetCellValue(prj.AllowAllActivities ?? 0.0);
            row.CreateCell(24).SetCellValue((double?)prj.Hours ?? 0.0);
            row.CreateCell(25).SetCellValue((double?)prj.HourlyRate ?? 0.0);
            row.CreateCell(26).SetCellValue((double?)prj.TimeRevenues ?? 0.0);
            row.CreateCell(27).SetCellValue((double?)prj.Revenues ?? 0.0);
            row.CreateCell(28).SetCellValue((double?)prj.CostOfGoods ?? 0.0);
            row.CreateCell(29).SetCellValue((double?)prj.PayrollExpenses ?? 0.0);
            row.CreateCell(30).SetCellValue((double?)prj.OtherExpenses ?? 0.0);
            row.CreateCell(31).SetCellValue(prj.IsExpenseMarkupEnabled ?? 0.0);
            row.CreateCell(32).SetCellValue((double?)prj.MarkupExpensesByFactor ?? 0.0);
            row.CreateCell(33).SetCellValue(prj.ExpenseMarkupDescription ?? "");
            row.CreateCell(34).SetCellValue(prj.IsFeeMarkupEnabled ?? 0.0);
            row.CreateCell(35).SetCellValue((double?)prj.MarkupFeesByFactor ?? 0.0);
            row.CreateCell(36).SetCellValue(prj.FeeMarkupDescription ?? "");
            row.CreateCell(37).SetCellValue(prj.LocationCode ?? "");
            row.CreateCell(38).SetCellValue(prj.LocationName ?? "");
            row.CreateCell(39).SetCellValue(prj.IsInternal ?? "");
        }

        startRow++;
        return startRow;
    }

    private static int WriteProjectTeamBlock(ISheet sheet, IEnumerable<ProjectTeamMembers> team, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[ProjectTeamMembers]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            "ProjectCode", "SubprojectCode", "ProjectName", "EmployeeNo", "ContactName", "Hours", "HourlyRate"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var m in team)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(m.ProjectCode ?? "");
            row.CreateCell(1).SetCellValue(m.SubprojectCode ?? "");
            row.CreateCell(2).SetCellValue(m.ProjectName ?? "");
            row.CreateCell(3).SetCellValue((int?)m.EmployeeNo ?? 0);
            row.CreateCell(4).SetCellValue(m.ContactName ?? "");
            row.CreateCell(5).SetCellValue((int?)m.Hours ?? 0);
            row.CreateCell(6).SetCellValue((int?)m.HourlyRate ?? 0);
        }

        ;

        startRow++;
        return startRow;
    }

    private static int WriteProjectActivitiesBlock(ISheet sheet, IEnumerable<ProjectActivity> activities, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[ProjectActivity]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            "ProjectCode", "SubprojectCode", "ProjectName", "ActivityCode", "ActivityName",
            "ProjectBillable", "HourlyRate"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var a in activities)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(a.ProjectCode ?? "");
            row.CreateCell(1).SetCellValue(a.SubprojectCode ?? "");
            row.CreateCell(2).SetCellValue(a.ProjectName ?? "");
            row.CreateCell(3).SetCellValue(a.ActivityCode ?? "");
            row.CreateCell(4).SetCellValue(a.ActivityName ?? "");
            row.CreateCell(5).SetCellValue(a.ProjectBillable ?? 0);
            row.CreateCell(6).SetCellValue((double?)a.HourlyRate ?? 0.0);
        }

        ;

        startRow++;
        return startRow;
    }

    private static int WriteDepartmentsBlock(ISheet sheet, IEnumerable<Departments> departments, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[Departments]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers = { "DepartmentCode", "DepartmentName", "DepartmentManagerCode", "DepartmentManagerName" };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var d in departments)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(d.DepartmentCode ?? "");
            row.CreateCell(1).SetCellValue(d.DepartmentName ?? "");
            row.CreateCell(2).SetCellValue(d.DepartmentManagerCode?.ToString() ?? "");
            row.CreateCell(3).SetCellValue(d.DepartmentManagerName ?? "");
        }

        startRow++;
        return startRow;
    }

    private static int WriteVouchersBlock(ISheet sheet, IEnumerable<Voucher> vouchers, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[Vouchers]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            // --- Parent Voucher fields ---
            "VoucherNo", "SaftBatchId", "SaftSourceId",
            "DocumentDate", "PostingDate", "VoucherType",

            // --- VoucherLine fields ---
            "Account", "AccountName", "AccountAgricultureDepartment",
            "VAT", "VATReturnSpecification",

            "Amount", "Currency", "CurrencyAmount", "Discount", "Quantity",
            "InvoiceNo", "CID", "DueDate", "Description", "Reference",

            "SalesPersonEmployeeNo", "SalesPersonName", "Accrual",

            "CustomerNo", "SupplierNo", "EmployeeNo",
            "ContactName", "ContactGroup",
            "CustomerSince", "SupplierSince", "EmployeeSince", "IsVatFree",

            "Phone", "Email", "Web", "OrganizationNo",

            "MailAddress1", "MailAddress2", "MailPostcode", "MailCity", "MailCountry",
            "DeliveryAddress1", "DeliveryAddress2", "DeliveryPostcode", "DeliveryCity", "DeliveryCountry",

            "BankAccount", "IBAN", "SWIFT",

            "ContactPersonFirstName", "ContactPersonLastName", "ContactPersonPhone", "ContactPersonEmail",

            "ProductCode", "ProductName", "ProductGroup", "ProductDescription", "ProductType",
            "ProductUnit", "ProductSalesPrice", "ProductCostPrice",
            "ProductSalesAccount", "ProductSalesAccountName",
            "ProductAltSalesAccount", "ProductAltSalesAccountName",
            "ProductGTIN",

            "ProjectCode", "SubprojectCode", "ProjectName",
            "ProjectManagerCode", "ProjectManagerName",
            "ProjectBillable", "ProjectStartDate", "ProjectEndDate", "ProjectStatus",

            "DepartmentCode", "DepartmentName", "InvoiceDelivery",

            "CustomerDiscount",
            "CustomMatchingReference"
        };

        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        // 3) For each Voucher, write one row per VoucherLine
        foreach (var v in vouchers)
        {
            // If you store lines in the Voucher class, ensure you do:
            // public List<VoucherLine> Lines { get; set; } = new();
            // then loop over them:
            var lines = v.Lines ?? new List<VoucherLine>();

            foreach (var line in lines)
            {
                var row = sheet.CreateRow(startRow++);
                var col = 0;

                // --- Parent Voucher fields ---
                row.CreateCell(col++).SetCellValue(v.VoucherNo);
                row.CreateCell(col++).SetCellValue(v.SaftBatchId ?? "");
                row.CreateCell(col++).SetCellValue(v.SaftSourceId ?? "");
                row.CreateCell(col++).SetCellValue(v.DocumentDate ?? "");
                row.CreateCell(col++).SetCellValue(v.PostingDate ?? "");
                row.CreateCell(col++).SetCellValue(v.VoucherType.ToString());

                // --- VoucherLine fields ---
                row.CreateCell(col++).SetCellValue(line.Account?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.AccountName ?? "");
                row.CreateCell(col++).SetCellValue(line.AccountAgricultureDepartment ?? "");
                row.CreateCell(col++).SetCellValue(line.VAT ?? "");
                row.CreateCell(col++).SetCellValue(line.VATReturnSpecification ?? "");

                row.CreateCell(col++).SetCellValue((double?)line.Amount ?? 0.0);
                row.CreateCell(col++).SetCellValue(line.Currency ?? "");
                row.CreateCell(col++).SetCellValue((double?)line.CurrencyAmount ?? 0.0);
                row.CreateCell(col++).SetCellValue((double?)line.Discount ?? 0.0);
                row.CreateCell(col++).SetCellValue((double?)line.Quantity ?? 0.0);

                row.CreateCell(col++).SetCellValue(line.InvoiceNo?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.CID ?? "");
                row.CreateCell(col++).SetCellValue(line.DueDate ?? "");
                row.CreateCell(col++).SetCellValue(line.Description ?? "");
                row.CreateCell(col++).SetCellValue(line.Reference ?? "");

                row.CreateCell(col++).SetCellValue(line.SalesPersonEmployeeNo?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.SalesPersonName ?? "");
                row.CreateCell(col++).SetCellValue(line.Accrual?.ToString() ?? "");

                row.CreateCell(col++).SetCellValue(line.CustomerNo?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.SupplierNo?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.EmployeeNo?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.ContactName ?? "");
                row.CreateCell(col++).SetCellValue(line.ContactGroup ?? "");
                row.CreateCell(col++).SetCellValue(line.CustomerSince ?? "");
                row.CreateCell(col++).SetCellValue(line.SupplierSince ?? "");
                row.CreateCell(col++).SetCellValue(line.EmployeeSince ?? "");
                row.CreateCell(col++).SetCellValue(line.IsVatFree?.ToString() ?? "");

                row.CreateCell(col++).SetCellValue(line.Phone ?? "");
                row.CreateCell(col++).SetCellValue(line.Email ?? "");
                row.CreateCell(col++).SetCellValue(line.Web ?? "");
                row.CreateCell(col++).SetCellValue(line.OrganizationNo ?? "");

                row.CreateCell(col++).SetCellValue(line.MailAddress1 ?? "");
                row.CreateCell(col++).SetCellValue(line.MailAddress2 ?? "");
                row.CreateCell(col++).SetCellValue(line.MailPostcode ?? "");
                row.CreateCell(col++).SetCellValue(line.MailCity ?? "");
                row.CreateCell(col++).SetCellValue(line.MailCountry ?? "");

                row.CreateCell(col++).SetCellValue(line.DeliveryAddress1 ?? "");
                row.CreateCell(col++).SetCellValue(line.DeliveryAddress2 ?? "");
                row.CreateCell(col++).SetCellValue(line.DeliveryPostcode ?? "");
                row.CreateCell(col++).SetCellValue(line.DeliveryCity ?? "");
                row.CreateCell(col++).SetCellValue(line.DeliveryCountry ?? "");

                row.CreateCell(col++).SetCellValue(line.BankAccount ?? "");
                row.CreateCell(col++).SetCellValue(line.IBAN ?? "");
                row.CreateCell(col++).SetCellValue(line.SWIFT ?? "");

                row.CreateCell(col++).SetCellValue(line.ContactPersonFirstName ?? "");
                row.CreateCell(col++).SetCellValue(line.ContactPersonLastName ?? "");
                row.CreateCell(col++).SetCellValue(line.ContactPersonPhone ?? "");
                row.CreateCell(col++).SetCellValue(line.ContactPersonEmail ?? "");

                row.CreateCell(col++).SetCellValue(line.ProductCode ?? "");
                row.CreateCell(col++).SetCellValue(line.ProductName ?? "");
                row.CreateCell(col++).SetCellValue(line.ProductGroup ?? "");
                row.CreateCell(col++).SetCellValue(line.ProductDescription ?? "");
                row.CreateCell(col++).SetCellValue(line.ProductType?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.ProductUnit ?? "");
                row.CreateCell(col++).SetCellValue((double?)line.ProductSalesPrice ?? 0.0);
                row.CreateCell(col++).SetCellValue((double?)line.ProductCostPrice ?? 0.0);
                row.CreateCell(col++).SetCellValue(line.ProductSalesAccount?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.ProductSalesAccountName ?? "");
                row.CreateCell(col++).SetCellValue(line.ProductAltSalesAccount?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.ProductAltSalesAccountName ?? "");
                row.CreateCell(col++).SetCellValue(line.ProductGTIN ?? "");

                row.CreateCell(col++).SetCellValue(line.ProjectCode ?? "");
                row.CreateCell(col++).SetCellValue(line.SubprojectCode ?? "");
                row.CreateCell(col++).SetCellValue(line.ProjectName ?? "");
                row.CreateCell(col++).SetCellValue(line.ProjectManagerCode?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.ProjectManagerName ?? "");
                row.CreateCell(col++).SetCellValue(line.ProjectBillable?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.ProjectStartDate ?? "");
                row.CreateCell(col++).SetCellValue(line.ProjectEndDate ?? "");
                row.CreateCell(col++).SetCellValue(line.ProjectStatus?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.DepartmentCode ?? "");
                row.CreateCell(col++).SetCellValue(line.DepartmentName ?? "");
                row.CreateCell(col++).SetCellValue(line.InvoiceDelivery?.ToString() ?? "");

                row.CreateCell(col++).SetCellValue(line.CustomerDiscount?.ToString() ?? "");
                row.CreateCell(col++).SetCellValue(line.CustomMatchingReference ?? "");
            }
        }

        // 4) Optionally leave one blank row at the end
        startRow++;
        return startRow;
    }


    private static int WriteOrdersBlock(ISheet sheet, IEnumerable<Order> orders, int startRow)
    {
        // Denote the Orders block
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[Orders]");

        // Create a header row
        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            //  0
            "OrderNo",
            //  1
            "OrderDate",
            //  2
            "RecurringInvoiceActive",
            //  3
            "RecurringInvoiceRepeatTimes",
            //  4
            "RecurringInvoiceEndDate",
            //  5
            "RecurringInvoiceSendMethod",
            //  6
            "RecurringInvoiceSendFrequency",
            //  7
            "RecurringInvoiceSendFrequencyUnit",
            //  8
            "NextRecurringInvoiceDate",
            //  9
            "SalesPersonEmployeeNo",
            // 10
            "SalesPersonName",
            // 11
            "ProjectCode",
            // 12
            "SubprojectCode",
            // 13
            "ProjectName",
            // 14
            "ProjectManagerCode",
            // 15
            "ProjectManagerName",
            // 16
            "ProjectBillable",
            // 17
            "ProjectStartDate",
            // 18
            "ProjectEndDate",
            // 19
            "ProjectStatus",
            // 20
            "ProjectContactPerson",
            // 21
            "DepartmentCode",
            // 22
            "DepartmentName",
            // 23
            "DepartmentManagerCode",
            // 24
            "DepartmentManagerName",
            // 25
            "CustomerNo",
            // 26
            "ContactName",
            // 27
            "ContactGroup",
            // 28
            "CustomerSince",
            // 29
            "IsVatFree",
            // 30
            "Phone",
            // 31
            "Email",
            // 32
            "Web",
            // 33
            "OrganizationNo",
            // 34
            "MailAddress1",
            // 35
            "MailAddress2",
            // 36
            "MailPostcode",
            // 37
            "MailCity",
            // 38
            "MailCountry",
            // 39
            "DeliveryAddress1",
            // 40
            "DeliveryAddress2",
            // 41
            "DeliveryPostcode",
            // 42
            "DeliveryCity",
            // 43
            "DeliveryCountry",
            // 44
            "BankAccount",
            // 45
            "IBAN",
            // 46
            "SWIFT",
            // 47
            "InvoiceDelivery",
            // 48
            "ContactPersonFirstName",
            // 49
            "ContactPersonLastName",
            // 50
            "ContactPersonPhone",
            // 51
            "ContactPersonEmail",
            // 52
            "Reference",
            // 53
            "PaymentTerms",
            // 54
            "MergeWithPreviousOrder",
            // 55
            "Currency",
            // 56
            "ProductCode",
            // 57
            "ProductName",
            // 58
            "ProductGroup",
            // 59
            "ProductDescription",
            // 60
            "ProductType",
            // 61
            "ProductUnit",
            // 62
            "ProductSalesPrice",
            // 63
            "ProductCostPrice",
            // 64
            "ProductSalesAccount",
            // 65
            "ProductSalesAccountName",
            // 66
            "ProductAltSalesAccount",
            // 67
            "ProductAltSalesAccountName",
            // 68
            "ProductGTIN",
            // 69
            "Discount",
            // 70
            "Quantity",
            // 71
            "Description",
            // 72
            "OrderLineUnitPrice",
            // 73
            "SortOrder",
            // 74
            "VATReturnSpecification"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        // Write each order's data row
        foreach (var o in orders)
        {
            var row = sheet.CreateRow(startRow++);

            //  0: OrderNo (Numeric or string? Adjust as needed)
            row.CreateCell(0).SetCellValue(o.OrderNo?.ToString() ?? "");

            //  1: OrderDate (Numeric, string, or date? Adjust as needed)
            row.CreateCell(1).SetCellValue(o.OrderDate ?? "");

            //  2: RecurringInvoiceActive (0=F, 1=T). If bool? => row.CreateCell(2).SetCellValue(o.RecurringInvoiceActive == true ? 1.0 : 0.0)
            row.CreateCell(2).SetCellValue(o.RecurringInvoiceActive ?? 0.0);

            //  3: RecurringInvoiceRepeatTimes (Numeric)
            row.CreateCell(3).SetCellValue(o.RecurringInvoiceRepeatTimes ?? 0.0);

            //  4: RecurringInvoiceEndDate (DDMMYYYY => string or date)
            row.CreateCell(4).SetCellValue(o.RecurringInvoiceEndDate ?? "");

            //  5: RecurringInvoiceSendMethod (Numeric)
            row.CreateCell(5).SetCellValue(o.RecurringInvoiceSendMethod ?? 0.0);

            //  6: RecurringInvoiceSendFrequency (Numeric)
            row.CreateCell(6).SetCellValue(o.RecurringInvoiceSendFrequency ?? 0.0);

            //  7: RecurringInvoiceSendFrequencyUnit (0,1,2,3)
            row.CreateCell(7).SetCellValue(o.RecurringInvoiceSendFrequencyUnit ?? 0.0);

            //  8: NextRecurringInvoiceDate
            row.CreateCell(8).SetCellValue(o.NextRecurringInvoiceDate ?? "");

            //  9: SalesPersonEmployeeNo (Numeric)
            row.CreateCell(9).SetCellValue(o.SalesPersonEmployeeNo ?? 0.0);

            // 10: SalesPersonName (Text)
            row.CreateCell(10).SetCellValue(o.SalesPersonName ?? "");

            // 11: ProjectCode
            row.CreateCell(11).SetCellValue(o.ProjectCode ?? "");

            // 12: SubprojectCode
            row.CreateCell(12).SetCellValue(o.SubprojectCode ?? "");

            // 13: ProjectName
            row.CreateCell(13).SetCellValue(o.ProjectName ?? "");

            // 14: ProjectManagerCode (Numeric)
            row.CreateCell(14).SetCellValue(o.ProjectManagerCode ?? 0.0);

            // 15: ProjectManagerName
            row.CreateCell(15).SetCellValue(o.ProjectManagerName ?? "");

            // 16: ProjectBillable (0=F, 1=T)
            row.CreateCell(16).SetCellValue(o.ProjectBillable ?? 0.0);

            // 17: ProjectStartDate
            row.CreateCell(17).SetCellValue(o.ProjectStartDate ?? "");

            // 18: ProjectEndDate
            row.CreateCell(18).SetCellValue(o.ProjectEndDate ?? "");

            // 19: ProjectStatus
            row.CreateCell(19).SetCellValue(o.ProjectStatus ?? 0);

            // 20: ProjectContactPerson
            row.CreateCell(20).SetCellValue(o.ProjectContactPerson ?? "");

            // 21: DepartmentCode
            row.CreateCell(21).SetCellValue(o.DepartmentCode ?? "");

            // 22: DepartmentName
            row.CreateCell(22).SetCellValue(o.DepartmentName ?? "");

            // 23: DepartmentManagerCode (Numeric)
            row.CreateCell(23).SetCellValue(o.DepartmentManagerCode ?? 0.0);

            // 24: DepartmentManagerName
            row.CreateCell(24).SetCellValue(o.DepartmentManagerName ?? "");

            // 25: CustomerNo (Numeric)
            row.CreateCell(25).SetCellValue(o.CustomerNo ?? 0.0);

            // 26: ContactName
            row.CreateCell(26).SetCellValue(o.ContactName ?? "");

            // 27: ContactGroup
            row.CreateCell(27).SetCellValue(o.ContactGroup ?? "");

            // 28: CustomerSince (DDMMYYYY)
            row.CreateCell(28).SetCellValue(o.CustomerSince ?? "");

            // 29: IsVatFree (0=F, 1=T)
            row.CreateCell(29).SetCellValue(o.IsVatFree ?? 0.0);

            // 30: Phone
            row.CreateCell(30).SetCellValue(o.Phone ?? "");

            // 31: Email
            row.CreateCell(31).SetCellValue(o.Email ?? "");

            // 32: Web
            row.CreateCell(32).SetCellValue(o.Web ?? "");

            // 33: OrganizationNo
            row.CreateCell(33).SetCellValue(o.OrganizationNo ?? "");

            // 34: MailAddress1
            row.CreateCell(34).SetCellValue(o.MailAddress1 ?? "");

            // 35: MailAddress2
            row.CreateCell(35).SetCellValue(o.MailAddress2 ?? "");

            // 36: MailPostcode
            row.CreateCell(36).SetCellValue(o.MailPostcode ?? "");

            // 37: MailCity
            row.CreateCell(37).SetCellValue(o.MailCity ?? "");

            // 38: MailCountry (2 characters)
            row.CreateCell(38).SetCellValue(o.MailCountry ?? "");

            // 39: DeliveryAddress1
            row.CreateCell(39).SetCellValue(o.DeliveryAddress1 ?? "");

            // 40: DeliveryAddress2
            row.CreateCell(40).SetCellValue(o.DeliveryAddress2 ?? "");

            // 41: DeliveryPostcode
            row.CreateCell(41).SetCellValue(o.DeliveryPostcode ?? "");

            // 42: DeliveryCity
            row.CreateCell(42).SetCellValue(o.DeliveryCity ?? "");

            // 43: DeliveryCountry (2 characters)
            row.CreateCell(43).SetCellValue(o.DeliveryCountry ?? "");

            // 44: BankAccount
            row.CreateCell(44).SetCellValue(o.BankAccount ?? "");

            // 45: IBAN
            row.CreateCell(45).SetCellValue(o.IBAN ?? "");

            // 46: SWIFT
            row.CreateCell(46).SetCellValue(o.SWIFT ?? "");

            // 47: InvoiceDelivery
            row.CreateCell(47).SetCellValue(o.InvoiceDelivery ?? 0);

            // 48: ContactPersonFirstName
            row.CreateCell(48).SetCellValue(o.ContactPersonFirstName ?? "");

            // 49: ContactPersonLastName
            row.CreateCell(49).SetCellValue(o.ContactPersonLastName ?? "");

            // 50: ContactPersonPhone
            row.CreateCell(50).SetCellValue(o.ContactPersonPhone ?? "");

            // 51: ContactPersonEmail
            row.CreateCell(51).SetCellValue(o.ContactPersonEmail ?? "");

            // 52: Reference
            row.CreateCell(52).SetCellValue(o.Reference ?? "");

            // 53: PaymentTerms (Numeric)
            row.CreateCell(53).SetCellValue(o.PaymentTerms ?? 0.0);

            // 54: MergeWithPreviousOrder (0=F, 1=T)
            row.CreateCell(54).SetCellValue(o.MergeWithPreviousOrder ?? 0.0);

            // 55: Currency
            row.CreateCell(55).SetCellValue(o.Currency ?? "");

            // 56: ProductCode
            row.CreateCell(56).SetCellValue(o.ProductCode ?? "");

            // 57: ProductName
            row.CreateCell(57).SetCellValue(o.ProductName ?? "");

            // 58: ProductGroup
            row.CreateCell(58).SetCellValue(o.ProductGroup ?? "");

            // 59: ProductDescription
            row.CreateCell(59).SetCellValue(o.ProductDescription ?? "");

            // 60: ProductType
            row.CreateCell(60).SetCellValue(o.ProductType ?? 0);

            // 61: ProductUnit
            row.CreateCell(61).SetCellValue(o.ProductUnit ?? "");

            // 62: ProductSalesPrice (decimal)
            row.CreateCell(62).SetCellValue((double?)o.ProductSalesPrice ?? 0.0);

            // 63: ProductCostPrice (decimal)
            row.CreateCell(63).SetCellValue((double?)o.ProductCostPrice ?? 0.0);

            // 64: ProductSalesAccount (numeric)
            row.CreateCell(64).SetCellValue(o.ProductSalesAccount ?? 0.0);

            // 65: ProductSalesAccountName
            row.CreateCell(65).SetCellValue(o.ProductSalesAccountName ?? "");

            // 66: ProductAltSalesAccount (numeric)
            row.CreateCell(66).SetCellValue(o.ProductAltSalesAccount ?? 0.0);

            // 67: ProductAltSalesAccountName
            row.CreateCell(67).SetCellValue(o.ProductAltSalesAccountName ?? "");

            // 68: ProductGTIN
            row.CreateCell(68).SetCellValue(o.ProductGTIN ?? "");

            // 69: Discount (decimal)
            row.CreateCell(69).SetCellValue((double?)o.Discount ?? 0.0);

            // 70: Quantity (decimal)
            row.CreateCell(70).SetCellValue((double?)o.Quantity ?? 0.0);

            // 71: Description (Freetext line if ProductCode is empty)
            row.CreateCell(71).SetCellValue(o.Description ?? "");

            // 72: OrderLineUnitPrice (decimal)
            row.CreateCell(72).SetCellValue((double?)o.OrderLineUnitPrice ?? 0.0);

            // 73: SortOrder (numeric)
            row.CreateCell(73).SetCellValue(o.SortOrder ?? 0.0);

            // 74: VATReturnSpecification
            row.CreateCell(74).SetCellValue(o.VATReturnSpecification ?? "");
        }

        // Return the next available row index after finishing
        startRow++;
        return startRow;
    }


    private static int WriteQuotesBlock(ISheet sheet, IEnumerable<Quote> quotes, int startRow)
    {
        // Denote the Quotes block
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[Quotes]");

        // Create header row
        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            //  0
            "QuoteNo",
            //  1
            "QuoteDate",
            //  2
            "QuoteExpiryDate",
            //  3
            "SalesPersonEmployeeNo",
            //  4
            "SalesPersonName",
            //  5
            "ProjectCode",
            //  6
            "SubprojectCode",
            //  7
            "ProjectName",
            //  8
            "ProjectManagerCode",
            //  9
            "ProjectManagerName",
            // 10
            "ProjectBillable",
            // 11
            "ProjectStartDate",
            // 12
            "ProjectEndDate",
            // 13
            "ProjectStatus",
            // 14
            "ProjectContactPerson",
            // 15
            "DepartmentCode",
            // 16
            "DepartmentName",
            // 17
            "DepartmentManagerCode",
            // 18
            "DepartmentManagerName",
            // 19
            "CustomerNo",
            // 20
            "ContactName",
            // 21
            "ContactGroup",
            // 22
            "CustomerSince",
            // 23
            "IsVatFree",
            // 24
            "Phone",
            // 25
            "Email",
            // 26
            "Web",
            // 27
            "OrganizationNo",
            // 28
            "MailAddress1",
            // 29
            "MailAddress2",
            // 30
            "MailPostcode",
            // 31
            "MailCity",
            // 32
            "MailCountry",
            // 33
            "DeliveryAddress1",
            // 34
            "DeliveryAddress2",
            // 35
            "DeliveryPostcode",
            // 36
            "DeliveryCity",
            // 37
            "DeliveryCountry",
            // 38
            "BankAccount",
            // 39
            "IBAN",
            // 40
            "SWIFT",
            // 41
            "InvoiceDelivery",
            // 42
            "ContactPersonFirstName",
            // 43
            "ContactPersonLastName",
            // 44
            "ContactPersonPhone",
            // 45
            "ContactPersonEmail",
            // 46
            "ProductCode",
            // 47
            "ProductName",
            // 48
            "ProductGroup",
            // 49
            "ProductDescription",
            // 50
            "ProductType",
            // 51
            "ProductUnit",
            // 52
            "ProductSalesPrice",
            // 53
            "ProductCostPrice",
            // 54
            "ProductSalesAccount",
            // 55
            "ProductSalesAccountName",
            // 56
            "ProductAltSalesAccount",
            // 57
            "ProductAltSalesAccountName",
            // 58
            "ProductGTIN",
            // 59
            "Discount",
            // 60
            "Quantity",
            // 61
            "Description",
            // 62
            "QuoteLineUnitPrice",
            // 63
            "VATReturnSpecification"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        // Write each quote's data
        foreach (var q in quotes)
        {
            var row = sheet.CreateRow(startRow++);

            //  0: QuoteNo (Numeric or string; adjust to your data model)
            row.CreateCell(0).SetCellValue(q.QuoteNo?.ToString() ?? "");

            //  1: QuoteDate (DDMMYYYY or date; adjust as needed)
            row.CreateCell(1).SetCellValue(q.QuoteDate ?? "");

            //  2: QuoteExpiryDate
            row.CreateCell(2).SetCellValue(q.QuoteExpiryDate ?? "");

            //  3: SalesPersonEmployeeNo (numeric)
            row.CreateCell(3).SetCellValue(q.SalesPersonEmployeeNo ?? 0.0);

            //  4: SalesPersonName
            row.CreateCell(4).SetCellValue(q.SalesPersonName ?? "");

            //  5: ProjectCode
            row.CreateCell(5).SetCellValue((double?)q.ProjectCode ?? 0);

            //  6: SubprojectCode
            row.CreateCell(6).SetCellValue(q.SubprojectCode ?? "");

            //  7: ProjectName
            row.CreateCell(7).SetCellValue(q.ProjectName ?? "");

            //  8: ProjectManagerCode (numeric)
            row.CreateCell(8).SetCellValue(q.ProjectManagerCode ?? 0.0);

            //  9: ProjectManagerName
            row.CreateCell(9).SetCellValue(q.ProjectManagerName ?? "");

            // 10: ProjectBillable (0=F, 1=T)
            row.CreateCell(10).SetCellValue(q.ProjectBillable ?? 0.0);

            // 11: ProjectStartDate
            row.CreateCell(11).SetCellValue(q.ProjectStartDate ?? "");

            // 12: ProjectEndDate
            row.CreateCell(12).SetCellValue(q.ProjectEndDate ?? "");

            // 13: ProjectStatus
            row.CreateCell(13).SetCellValue(q.ProjectStatus ?? 0);

            // 14: ProjectContactPerson
            row.CreateCell(14).SetCellValue(q.ProjectContactPerson ?? "");

            // 15: DepartmentCode
            row.CreateCell(15).SetCellValue(q.DepartmentCode ?? "");

            // 16: DepartmentName
            row.CreateCell(16).SetCellValue(q.DepartmentName ?? "");

            // 17: DepartmentManagerCode (numeric)
            row.CreateCell(17).SetCellValue(q.DepartmentManagerCode ?? 0.0);

            // 18: DepartmentManagerName
            row.CreateCell(18).SetCellValue(q.DepartmentManagerName ?? "");

            // 19: CustomerNo (numeric)
            row.CreateCell(19).SetCellValue(q.CustomerNo ?? 0.0);

            // 20: ContactName
            row.CreateCell(20).SetCellValue(q.ContactName ?? "");

            // 21: ContactGroup
            row.CreateCell(21).SetCellValue(q.ContactGroup ?? "");

            // 22: CustomerSince (DDMMYYYY)
            row.CreateCell(22).SetCellValue(q.CustomerSince ?? "");

            // 23: IsVatFree (0=F, 1=T)
            row.CreateCell(23).SetCellValue(q.IsVatFree ?? 0.0);

            // 24: Phone
            row.CreateCell(24).SetCellValue(q.Phone ?? "");

            // 25: Email
            row.CreateCell(25).SetCellValue(q.Email ?? "");

            // 26: Web
            row.CreateCell(26).SetCellValue(q.Web ?? "");

            // 27: OrganizationNo
            row.CreateCell(27).SetCellValue(q.OrganizationNo ?? "");

            // 28: MailAddress1
            row.CreateCell(28).SetCellValue(q.MailAddress1 ?? "");

            // 29: MailAddress2
            row.CreateCell(29).SetCellValue(q.MailAddress2 ?? "");

            // 30: MailPostcode
            row.CreateCell(30).SetCellValue(q.MailPostcode ?? "");

            // 31: MailCity
            row.CreateCell(31).SetCellValue(q.MailCity ?? "");

            // 32: MailCountry (2 chars if needed)
            row.CreateCell(32).SetCellValue(q.MailCountry ?? "");

            // 33: DeliveryAddress1
            row.CreateCell(33).SetCellValue(q.DeliveryAddress1 ?? "");

            // 34: DeliveryAddress2
            row.CreateCell(34).SetCellValue(q.DeliveryAddress2 ?? "");

            // 35: DeliveryPostcode
            row.CreateCell(35).SetCellValue(q.DeliveryPostcode ?? "");

            // 36: DeliveryCity
            row.CreateCell(36).SetCellValue(q.DeliveryCity ?? "");

            // 37: DeliveryCountry (2 chars if needed)
            row.CreateCell(37).SetCellValue(q.DeliveryCountry ?? "");

            // 38: BankAccount
            row.CreateCell(38).SetCellValue(q.BankAccount ?? "");

            // 39: IBAN
            row.CreateCell(39).SetCellValue(q.IBAN ?? "");

            // 40: SWIFT
            row.CreateCell(40).SetCellValue(q.SWIFT ?? "");

            // 41: InvoiceDelivery
            row.CreateCell(41).SetCellValue(q.InvoiceDelivery ?? 0);

            // 42: ContactPersonFirstName
            row.CreateCell(42).SetCellValue(q.ContactPersonFirstName ?? "");

            // 43: ContactPersonLastName
            row.CreateCell(43).SetCellValue(q.ContactPersonLastName ?? "");

            // 44: ContactPersonPhone
            row.CreateCell(44).SetCellValue(q.ContactPersonPhone ?? "");

            // 45: ContactPersonEmail
            row.CreateCell(45).SetCellValue(q.ContactPersonEmail ?? "");

            // 46: ProductCode
            row.CreateCell(46).SetCellValue(q.ProductCode ?? "");

            // 47: ProductName
            row.CreateCell(47).SetCellValue(q.ProductName ?? "");

            // 48: ProductGroup
            row.CreateCell(48).SetCellValue(q.ProductGroup ?? "");

            // 49: ProductDescription
            row.CreateCell(49).SetCellValue(q.ProductDescription ?? "");

            // 50: ProductType
            row.CreateCell(50).SetCellValue(q.ProductType ?? 0);

            // 51: ProductUnit
            row.CreateCell(51).SetCellValue(q.ProductUnit ?? "");

            // 52: ProductSalesPrice (decimal)
            row.CreateCell(52).SetCellValue((double?)q.ProductSalesPrice ?? 0.0);

            // 53: ProductCostPrice (decimal)
            row.CreateCell(53).SetCellValue((double?)q.ProductCostPrice ?? 0.0);

            // 54: ProductSalesAccount (numeric)
            row.CreateCell(54).SetCellValue(q.ProductSalesAccount ?? 0.0);

            // 55: ProductSalesAccountName
            row.CreateCell(55).SetCellValue(q.ProductSalesAccountName ?? "");

            // 56: ProductAltSalesAccount (numeric)
            row.CreateCell(56).SetCellValue(q.ProductAltSalesAccount ?? 0.0);

            // 57: ProductAltSalesAccountName
            row.CreateCell(57).SetCellValue(q.ProductAltSalesAccountName ?? "");

            // 58: ProductGTIN
            row.CreateCell(58).SetCellValue(q.ProductGTIN ?? "");

            // 59: Discount (decimal)
            row.CreateCell(59).SetCellValue((double?)q.Discount ?? 0.0);

            // 60: Quantity (decimal)
            row.CreateCell(60).SetCellValue((double?)q.Quantity ?? 0.0);

            // 61: Description (only used if ProductCode is empty => freetext line)
            row.CreateCell(61).SetCellValue(q.Description ?? "");

            // 62: QuoteLineUnitPrice (decimal)
            row.CreateCell(62).SetCellValue((double?)q.QuoteLineUnitPrice ?? 0.0);

            // 63: VATReturnSpecification
            row.CreateCell(63).SetCellValue(q.VATReturnSpecification ?? "");
        }

        startRow++;
        return startRow;
    }


    private static int WriteInvoiceCidBlock(ISheet sheet, IEnumerable<InvoiceCid> invoiceCids, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[InvoiceCid]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers = { "InvoiceNo", "CID" };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var ic in invoiceCids)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(ic.InvoiceNo ?? "");
            row.CreateCell(1).SetCellValue(ic.CID ?? "");
        }

        startRow++;
        return startRow;
    }

    private static int WriteChartOfAccountsBlock(ISheet sheet, IEnumerable<ChartOfAccount> charts, int startRow)
    {
        // Write the block label
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[ChartOfAccounts]");

        // Write header row with all columns as specified by the table
        var headerRow = sheet.CreateRow(startRow++);
        var headers = new[]
        {
            "Account",
            "AccountName",
            "AccountAgricultureDepartment",
            "VAT",
            "VATReturnSpecification",
            "BankAccount",
            "IsProjectRequired",
            "IsDepartmentRequired",
            "IsLocationRequired",
            "IsFixedAssetsRequired",
            "IsEnterpriseRequired",
            "IsActivityRequired",
            "IsDim1Required",
            "IsDim2Required",
            "IsDim3Required",
            "IsQuantityRequired",
            "IsQuantity2Required",
            "IsProductRequired",
            "IsAgricultureProductRequired",
            "StandardProjectCode",
            "StandardDepartmentCode",
            "LockVatCode",
            "IsActive"
        };

        for (var i = 0; i < headers.Length; i++) headerRow.CreateCell(i).SetCellValue(headers[i]);

        // Write each ChartOfAccount record in its own row
        foreach (var c in charts)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(c.Account.ToString());
            row.CreateCell(1).SetCellValue(c.AccountName ?? "");
            row.CreateCell(2).SetCellValue(c.AccountAgricultureDepartment ?? "");
            row.CreateCell(3).SetCellValue(c.VAT ?? "");
            row.CreateCell(4).SetCellValue(c.VATReturnSpecification ?? "");
            row.CreateCell(5).SetCellValue(c.BankAccount ?? "");
            row.CreateCell(6).SetCellValue(c.IsProjectRequired?.ToString() ?? "");
            row.CreateCell(7).SetCellValue(c.IsDepartmentRequired?.ToString() ?? "");
            row.CreateCell(8).SetCellValue(c.IsLocationRequired?.ToString() ?? "");
            row.CreateCell(9).SetCellValue(c.IsFixedAssetsRequired?.ToString() ?? "");
            row.CreateCell(10).SetCellValue(c.IsEnterpriseRequired?.ToString() ?? "");
            row.CreateCell(11).SetCellValue(c.IsActivityRequired?.ToString() ?? "");
            row.CreateCell(12).SetCellValue(c.IsDim1Required?.ToString() ?? "");
            row.CreateCell(13).SetCellValue(c.IsDim2Required?.ToString() ?? "");
            row.CreateCell(14).SetCellValue(c.IsDim3Required?.ToString() ?? "");
            row.CreateCell(15).SetCellValue(c.IsQuantityRequired?.ToString() ?? "");
            row.CreateCell(16).SetCellValue(c.IsQuantity2Required?.ToString() ?? "");
            row.CreateCell(17).SetCellValue(c.IsProductRequired?.ToString() ?? "");
            row.CreateCell(18).SetCellValue(c.IsAgricultureProductRequired?.ToString() ?? "");
            row.CreateCell(19).SetCellValue(c.StandardProjectCode ?? "");
            row.CreateCell(20).SetCellValue(c.StandardDepartmentCode ?? "");
            row.CreateCell(21).SetCellValue(c.LockVatCode?.ToString() ?? "");
            row.CreateCell(22).SetCellValue(c.IsActive?.ToString() ?? "");
        }

        // Add an extra row for spacing if needed
        startRow++;
        return startRow;
    }

    private static int WriteFixedAssetsBlock(ISheet sheet, IEnumerable<FixedAsset> assets, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[FixedAssets]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            "AssetCode", "AssetName", "AssetTypeName", "PurchaseDate", "PurchasePrice", "DepreciationMethod",
            "Rate", "EconomicLife", "Deprecation0101", "YTDDeprecation", "LastDeprecation", "DepartmentCode",
            "ProjectCode", "LocationCode",
            "SerialNumber"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var fa in assets)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(fa.AssetCode ?? "");
            row.CreateCell(1).SetCellValue(fa.AssetName ?? "");
            row.CreateCell(2).SetCellValue(fa.AssetTypeName ?? "");
            row.CreateCell(3).SetCellValue(fa.PurchaseDate ?? "");
            row.CreateCell(4).SetCellValue((double?)fa.PurchasePrice ?? 0.0);
            row.CreateCell(5).SetCellValue(fa.DepreciationMethod ?? "");
            row.CreateCell(6).SetCellValue((double?)fa.Rate ?? 0.0);
            row.CreateCell(7).SetCellValue((double?)fa.EconomicLife ?? 0.0);
            row.CreateCell(8).SetCellValue((double?)fa.Deprecation0101 ?? 0.0);
            row.CreateCell(9).SetCellValue((double?)fa.YTDDepreciation ?? 0.0);
            row.CreateCell(10).SetCellValue(fa.DepartmentCode ?? "");
            row.CreateCell(11).SetCellValue(fa.ProjectCode ?? "");
            row.CreateCell(12).SetCellValue(fa.LocationCode ?? "");
            row.CreateCell(13).SetCellValue(fa.SerialNumber ?? "");
        }

        startRow++;
        return startRow;
    }

    private static int WriteYtdPayrollBlock(ISheet sheet, IEnumerable<YTDPayrollBalance> ytds, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[YTDPayrollBalances]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            "SocialSecurityNumber", "InternationalIdNumber", "EmploymentRelationshipId", "YtdPayrollBalancesLineType",
            "PayItemCode",
            "Amount", "Quantity", "PrivateDrivenKilometers", "HomeWorkKilometers", "Year"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var y in ytds)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(y.SocialSecurityNumber ?? "");
            row.CreateCell(1).SetCellValue(y.InternationalIdNumber ?? "");
            row.CreateCell(2).SetCellValue(y.EmploymentRelationshipId ?? "");
            row.CreateCell(3).SetCellValue(y.YtdPayrollBalancesLineType ?? "");
            row.CreateCell(4).SetCellValue(y.PayItemCode ?? "");
            row.CreateCell(5).SetCellValue((double?)y.Amount ?? 0);
            row.CreateCell(6).SetCellValue((double?)y.Quantity ?? 0);
            row.CreateCell(7).SetCellValue((double?)y.PrivateDrivenKilometers ?? 0);
            row.CreateCell(8).SetCellValue((double?)y.HomeWorkKilometers ?? 0);
            row.CreateCell(9).SetCellValue(y.Year ?? 0);
        }

        startRow++;
        return startRow;
    }

    private static int WriteSalaryBasisBlock(ISheet sheet, IEnumerable<SalaryBasis> salaries, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[SalaryBasis]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            "EmployeeNo", "DepartmentCode", "ProjectCode", "PayItemCode", "Rate", "Amount", "Quantity", "Comment",
            "PersonType"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var s in salaries)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(s.EmployeeNo ?? 0);
            row.CreateCell(1).SetCellValue(s.DepartmentCode ?? "");
            row.CreateCell(2).SetCellValue(s.ProjectCode ?? "");
            row.CreateCell(3).SetCellValue(s.PayItemCode ?? "");
            row.CreateCell(4).SetCellValue((double?)s.Rate ?? 0.0);
            row.CreateCell(5).SetCellValue((double?)s.Amount ?? 0);
            row.CreateCell(6).SetCellValue((double?)s.Quantity ?? 0);
            row.CreateCell(7).SetCellValue(s.Comment ?? "");
            row.CreateCell(8).SetCellValue(s.PersonType ?? "");
        }

        startRow++;
        return startRow;
    }

    private static int WriteSalaryAdjustmentsBlock(ISheet sheet, IEnumerable<SalaryAdjustment> adjustments,
        int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[SalaryAdjustments]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            "EmployeeNo", "EmploymentRelationshipId", "RemunerationType", "AnnualSalary", "HourlyRate",
            "AdjustAnnualSalaryBy", "AdjustHourlyRateBy", "LastSalaryChangeDate"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var a in adjustments)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(a.EmployeeNo?.ToString() ?? "");
            row.CreateCell(1).SetCellValue(a.EmploymentRelationshipId ?? "");
            row.CreateCell(2).SetCellValue(a.RemunerationType ?? "");
            row.CreateCell(3).SetCellValue((double?)a.AnnualSalary ?? 0.0);
            row.CreateCell(4).SetCellValue((double?)a.HourlyRate ?? 0.0);
            row.CreateCell(5).SetCellValue((double?)a.AdjustAnnualSalaryBy ?? 0);
            row.CreateCell(6).SetCellValue((double?)a.AdjustHourlyRateBy ?? 0);
            row.CreateCell(7).SetCellValue(a.LastSalaryChangeDate ?? "");
        }

        startRow++;
        return startRow;
    }

    #endregion
}