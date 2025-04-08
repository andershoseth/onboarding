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
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[Orders]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            "OrderNo", "OrderDate", "CustomerNo", "ContactName", "ProductCode", "ProductName", "Quantity",
            "OrderLineUnitPrice"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var o in orders)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(o.OrderNo?.ToString() ?? "");
            row.CreateCell(1).SetCellValue(o.OrderDate ?? "");
            row.CreateCell(2).SetCellValue(o.CustomerNo?.ToString() ?? "");
            row.CreateCell(3).SetCellValue(o.ContactName ?? "");
            row.CreateCell(4).SetCellValue(o.ProductCode ?? "");
            row.CreateCell(5).SetCellValue(o.ProductName ?? "");
            row.CreateCell(6).SetCellValue((double?)o.Quantity ?? 0.0);
            row.CreateCell(7).SetCellValue((double?)o.OrderLineUnitPrice ?? 0.0);
        }

        startRow++;
        return startRow;
    }

    private static int WriteQuotesBlock(ISheet sheet, IEnumerable<Quote> quotes, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[Quotes]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
        {
            "QuoteNo", "QuoteDate", "CustomerNo", "ContactName", "ProductCode", "ProductName", "Quantity",
            "QuoteLineUnitPrice"
        };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var q in quotes)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(q.QuoteNo?.ToString() ?? "");
            row.CreateCell(1).SetCellValue(q.QuoteDate ?? "");
            row.CreateCell(2).SetCellValue(q.CustomerNo?.ToString() ?? "");
            row.CreateCell(3).SetCellValue(q.ContactName ?? "");
            row.CreateCell(4).SetCellValue(q.ProductCode ?? "");
            row.CreateCell(5).SetCellValue(q.ProductName ?? "");
            row.CreateCell(6).SetCellValue((double?)q.Quantity ?? 0.0);
            row.CreateCell(7).SetCellValue((double?)q.QuoteLineUnitPrice ?? 0.0);
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
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[ChartOfAccounts]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers = { "Account", "AccountName", "VAT", "AccountAgricultureDepartment", "IsActive" };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var c in charts)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(c.Account.ToString());
            row.CreateCell(1).SetCellValue(c.AccountName ?? "");
            row.CreateCell(2).SetCellValue(c.VAT ?? "");
            row.CreateCell(3).SetCellValue(c.AccountAgricultureDepartment ?? "");
            row.CreateCell(4).SetCellValue(c.IsActive?.ToString() ?? "");
        }

        startRow++;
        return startRow;
    }

    private static int WriteFixedAssetsBlock(ISheet sheet, IEnumerable<FixedAsset> assets, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[FixedAssets]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers =
            { "AssetCode", "AssetName", "AssetTypeName", "PurchaseDate", "PurchasePrice", "DepreciationMethod" };
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
        }

        startRow++;
        return startRow;
    }

    private static int WriteYtdPayrollBlock(ISheet sheet, IEnumerable<YTDPayrollBalance> ytds, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[YTDPayrollBalances]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers = { "SocialSecurityNumber", "EmploymentRelationshipId", "PayItemCode", "Amount", "Year" };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var y in ytds)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(y.SocialSecurityNumber ?? "");
            row.CreateCell(1).SetCellValue(y.EmploymentRelationshipId ?? "");
            row.CreateCell(2).SetCellValue(y.PayItemCode ?? "");
            row.CreateCell(3).SetCellValue((double?)y.Amount ?? 0.0);
            row.CreateCell(4).SetCellValue(y.Year?.ToString() ?? "");
        }

        startRow++;
        return startRow;
    }

    private static int WriteSalaryBasisBlock(ISheet sheet, IEnumerable<SalaryBasis> salaries, int startRow)
    {
        var decRow = sheet.CreateRow(startRow++);
        decRow.CreateCell(0).SetCellValue("[SalaryBasis]");

        var headerRow = sheet.CreateRow(startRow++);
        string[] headers = { "EmployeeNo", "PayItemCode", "Rate", "Amount", "Quantity", "PersonType" };
        for (var i = 0; i < headers.Length; i++)
            headerRow.CreateCell(i).SetCellValue(headers[i]);

        foreach (var s in salaries)
        {
            var row = sheet.CreateRow(startRow++);
            row.CreateCell(0).SetCellValue(s.EmployeeNo?.ToString() ?? "");
            row.CreateCell(1).SetCellValue(s.PayItemCode ?? "");
            row.CreateCell(2).SetCellValue((double?)s.Rate ?? 0.0);
            row.CreateCell(3).SetCellValue((double?)s.Amount ?? 0.0);
            row.CreateCell(4).SetCellValue((double?)s.Quantity ?? 0.0);
            row.CreateCell(5).SetCellValue(s.PersonType ?? "");
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
            "LastSalaryChangeDate"
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
            row.CreateCell(5).SetCellValue(a.LastSalaryChangeDate ?? "");
        }

        startRow++;
        return startRow;
    }

    #endregion
}