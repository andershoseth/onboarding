using System.IO.Compression;
using System.Text;
using onboarding_backend.Models.StandardImport;
namespace onboarding_backend.Services;


public static class CsvExporter
{
    /// <summary>
    /// Top-level method that:
    ///  1) Creates a CSV for each entity type in Standardimport
    ///  2) Puts them all into a single .zip in-memory
    ///  3) Returns the zip as byte[]
    /// 
    /// You can then return that byte[] from an API endpoint or save it to disk, etc.
    /// </summary>
    public static byte[] CreateAllCsvsAsZip(Standardimport standardImport)
    {
        if (standardImport == null)
            throw new ArgumentNullException(nameof(standardImport));

        using var memStream = new MemoryStream();
        using (var archive = new ZipArchive(memStream, ZipArchiveMode.Create, true))
        {
            // 1) Contacts
            var contactsCsv = CreateContactsCsv(standardImport.Contact);
            var contactsEntry = archive.CreateEntry("Contacts.csv");
            WriteBytesToZipEntry(contactsEntry, contactsCsv);

            // 2) Products
            var productsCsv = CreateProductsCsv(standardImport.Product);
            var productsEntry = archive.CreateEntry("Products.csv");
            WriteBytesToZipEntry(productsEntry, productsCsv);

            // 3) Projects
            var projectsCsv = CreateProjectsCsv(standardImport.Project);
            var projectsEntry = archive.CreateEntry("Projects.csv");
            WriteBytesToZipEntry(projectsEntry, projectsCsv);

            // 4) Project Team Members
            var projectTeamCsv = CreateProjectTeamCsv(standardImport.ProjectTeamMember);
            var projectTeamEntry = archive.CreateEntry("ProjectTeamMembers.csv");
            WriteBytesToZipEntry(projectTeamEntry, projectTeamCsv);

            // 5) Project Activities
            var projectActivitiesCsv = CreateProjectActivitiesCsv(standardImport.ProjectActivitie);
            var projectActivitiesEntry = archive.CreateEntry("ProjectActivities.csv");
            WriteBytesToZipEntry(projectActivitiesEntry, projectActivitiesCsv);

            // 6) Departments
            var departmentsCsv = CreateDepartmentsCsv(standardImport.Department);
            var departmentsEntry = archive.CreateEntry("Departments.csv");
            WriteBytesToZipEntry(departmentsEntry, departmentsCsv);

            // 7) Vouchers
            var vouchersCsv = CreateVouchersCsv(standardImport.Voucher);
            var vouchersEntry = archive.CreateEntry("Vouchers.csv");
            WriteBytesToZipEntry(vouchersEntry, vouchersCsv);

            // 8) Orders
            var ordersCsv = CreateOrdersCsv(standardImport.Order);
            var ordersEntry = archive.CreateEntry("Orders.csv");
            WriteBytesToZipEntry(ordersEntry, ordersCsv);

            // 9) Quotes
            var quotesCsv = CreateQuotesCsv(standardImport.Quote);
            var quotesEntry = archive.CreateEntry("Quotes.csv");
            WriteBytesToZipEntry(quotesEntry, quotesCsv);

            // 10) InvoiceCid
            var invoiceCidCsv = CreateInvoiceCidCsv(standardImport.InvoiceCid);
            var invoiceCidEntry = archive.CreateEntry("InvoiceCids.csv");
            WriteBytesToZipEntry(invoiceCidEntry, invoiceCidCsv);

            // 11) Chart of Accounts
            var chartCsv = CreateChartOfAccountsCsv(standardImport.ChartOfAccount);
            var chartEntry = archive.CreateEntry("ChartOfAccounts.csv");
            WriteBytesToZipEntry(chartEntry, chartCsv);

            // 12) Fixed Assets
            var fixedAssetCsv = CreateFixedAssetsCsv(standardImport.FixedAsset);
            var fixedAssetEntry = archive.CreateEntry("FixedAssets.csv");
            WriteBytesToZipEntry(fixedAssetEntry, fixedAssetCsv);

            // 13) YTD Payroll Balance
            var ytdCsv = CreateYtdPayrollBalancesCsv(standardImport.YTDPayrollBalance);
            var ytdEntry = archive.CreateEntry("YTDPayrollBalances.csv");
            WriteBytesToZipEntry(ytdEntry, ytdCsv);

            // 14) Salary Basis
            var salaryBasisCsv = CreateSalaryBasisCsv(standardImport.SalaryBasis);
            var salaryBasisEntry = archive.CreateEntry("SalaryBasis.csv");
            WriteBytesToZipEntry(salaryBasisEntry, salaryBasisCsv);

            // 15) Salary Adjustments
            var salaryAdjustmentCsv = CreateSalaryAdjustmentsCsv(standardImport.SalaryAdjustments);
            var salaryAdjustmentEntry = archive.CreateEntry("SalaryAdjustments.csv");
            WriteBytesToZipEntry(salaryAdjustmentEntry, salaryAdjustmentCsv);
        }

        // Return the entire zip as a byte[]
        return memStream.ToArray();
    }

    #region Private CSV creation methods

    // ----------------------------------
    // CONTACTS
    // ----------------------------------
    private static byte[] CreateContactsCsv(IEnumerable<Contacts> contacts)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[Contacts]");
        
        // A minimal example with just a handful of fields:
        sb.AppendLine("CustomerNo,SupplierNo,EmployeeNo,ContactName,Phone,Email,MailCountry");  // header
        foreach (var c in contacts)
        {
            // Convert fields to CSV-safe
            var row = string.Join(",", new[]
            {
                CsvValue(c.CustomerNo),
                CsvValue(c.SupplierNo),
                CsvValue(c.EmployeeNo),
                CsvValue(c.ContactName),
                CsvValue(c.Phone),
                CsvValue(c.Email),
                CsvValue(c.MailCountry)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // PRODUCTS
    // ----------------------------------
    private static byte[] CreateProductsCsv(IEnumerable<Products> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[Products]");
        sb.AppendLine("ProductCode,ProductName,ProductGroup,ProductSalesPrice,ProductSalesAccount");
        foreach (var p in products)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(p.ProductCode),
                CsvValue(p.ProductName),
                CsvValue(p.ProductGroup),
                CsvValue(p.ProductSalesPrice),
                CsvValue(p.ProductSalesAccount)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // PROJECTS
    // ----------------------------------
    private static byte[] CreateProjectsCsv(IEnumerable<Projects> projects)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[Projects]");
        sb.AppendLine("ProjectCode,ProjectName,ProjectManagerCode,ProjectStartDate,ProjectEndDate");
        foreach (var prj in projects)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(prj.ProjectCode),
                CsvValue(prj.ProjectName),
                CsvValue(prj.ProjectManagerCode),
                CsvValue(prj.ProjectStartDate),
                CsvValue(prj.ProjectEndDate)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // PROJECT TEAM MEMBERS
    // ----------------------------------
    private static byte[] CreateProjectTeamCsv(IEnumerable<ProjectTeamMembers> team)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[ProjectTeamMembers]");
        sb.AppendLine("ProjectCode,EmployeeNo,Hours,HourlyRate");
        foreach (var member in team)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(member.ProjectCode),
                CsvValue(member.EmployeeNo),
                CsvValue(member.Hours),
                CsvValue(member.HourlyRate)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // PROJECT ACTIVITIES
    // ----------------------------------
    private static byte[] CreateProjectActivitiesCsv(IEnumerable<ProjectActivity> activities)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[ProjectActivity]");
        sb.AppendLine("ProjectCode,ActivityCode,ActivityName,ProjectBillable,HourlyRate");
        foreach (var a in activities)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(a.ProjectCode),
                CsvValue(a.ActivityCode),
                CsvValue(a.ActivityName),
                CsvValue(a.ProjectBillable),
                CsvValue(a.HourlyRate)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // DEPARTMENTS
    // ----------------------------------
    private static byte[] CreateDepartmentsCsv(IEnumerable<Departments> depts)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[Departments]");
        
        sb.AppendLine("DepartmentCode,DepartmentName,DepartmentManagerCode");
        foreach (var d in depts)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(d.DepartmentCode),
                CsvValue(d.DepartmentName),
                CsvValue(d.DepartmentManagerCode)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // VOUCHERS
    // (Similar to earlier example, but
    //  we now return a single CSV for
    //  all lines of all vouchers)
    // ----------------------------------
    private static byte[] CreateVouchersCsv(IEnumerable<Voucher> vouchers)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[Vouchers]");
        sb.AppendLine("VoucherNo,DocumentDate,PostingDate,VoucherType,Account,Amount,Description");
        foreach (var v in vouchers)
        {
            foreach (var line in v.Lines)
            {
                var row = string.Join(",", new[]
                {
                    CsvValue(v.VoucherNo),
                    CsvValue(v.DocumentDate),
                    CsvValue(v.PostingDate),
                    CsvValue(v.VoucherType),
                    CsvValue(line.Account),
                    CsvValue(line.Amount),
                    CsvValue(line.Description)
                });
                sb.AppendLine(row);
            }
            sb.AppendLine();

        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // ORDERS
    // ----------------------------------
    private static byte[] CreateOrdersCsv(IEnumerable<Order> orders)
    {
        var sb = new StringBuilder();

        sb.AppendLine("[Orders]");
        sb.AppendLine("OrderNo,OrderDate,CustomerNo,ContactName,ProductCode,ProductName,Quantity,OrderLineUnitPrice");
        foreach (var o in orders)
        {

            var row = string.Join(",", new[]
            {
                CsvValue(o.OrderNo),
                CsvValue(o.OrderDate),
                CsvValue(o.CustomerNo),
                CsvValue(o.ContactName),
                CsvValue(o.ProductCode),
                CsvValue(o.ProductName),
                CsvValue(o.Quantity),
                CsvValue(o.OrderLineUnitPrice)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // QUOTES
    // ----------------------------------
    private static byte[] CreateQuotesCsv(IEnumerable<Quote> quotes)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[Quotes]");

        sb.AppendLine("QuoteNo,QuoteDate,CustomerNo,ContactName,ProductCode,ProductName,Quantity,QuoteLineUnitPrice");
        foreach (var q in quotes)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(q.QuoteNo),
                CsvValue(q.QuoteDate),
                CsvValue(q.CustomerNo),
                CsvValue(q.ContactName),
                CsvValue(q.ProductCode),
                CsvValue(q.ProductName),
                CsvValue(q.Quantity),
                CsvValue(q.QuoteLineUnitPrice)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // INVOICE CID
    // ----------------------------------
    private static byte[] CreateInvoiceCidCsv(IEnumerable<InvoiceCid> invoiceCid)
    {
        var sb = new StringBuilder();
        sb.AppendLine("InvoiceNo,CID");
        foreach (var i in invoiceCid)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(i.InvoiceNo),
                CsvValue(i.CID)
            });
            sb.AppendLine(row);
        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // CHART OF ACCOUNTS
    // ----------------------------------
    private static byte[] CreateChartOfAccountsCsv(IEnumerable<ChartOfAccount> charts)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[ChartOfAccounts]");
        sb.AppendLine("Account,AccountName,VAT,AccountAgricultureDepartment,IsActive");
        foreach (var c in charts)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(c.Account),
                CsvValue(c.AccountName),
                CsvValue(c.VAT),
                CsvValue(c.AccountAgricultureDepartment),
                CsvValue(c.IsActive)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // FIXED ASSETS
    // ----------------------------------
    private static byte[] CreateFixedAssetsCsv(IEnumerable<FixedAsset> assets)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[FixedAssets]");
        sb.AppendLine("AssetCode,AssetName,AssetTypeName,PurchaseDate,PurchasePrice,DepreciationMethod");
        foreach (var fa in assets)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(fa.AssetCode),
                CsvValue(fa.AssetName),
                CsvValue(fa.AssetTypeName),
                CsvValue(fa.PurchaseDate),
                CsvValue(fa.PurchasePrice),
                CsvValue(fa.DepreciationMethod)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // YTD PAYROLL BALANCES
    // ----------------------------------
    private static byte[] CreateYtdPayrollBalancesCsv(IEnumerable<YTDPayrollBalance> ytds)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[YTDPayrollBalances]");
        sb.AppendLine("SocialSecurityNumber,EmploymentRelationshipId,PayItemCode,Amount,Year");
        foreach (var y in ytds)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(y.SocialSecurityNumber),
                CsvValue(y.EmploymentRelationshipId),
                CsvValue(y.PayItemCode),
                CsvValue(y.Amount),
                CsvValue(y.Year)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // SALARY BASIS
    // ----------------------------------
    private static byte[] CreateSalaryBasisCsv(IEnumerable<SalaryBasis> salaries)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[SalaryBasis]");
        sb.AppendLine("EmployeeNo,PayItemCode,Rate,Amount,Quantity,PersonType");
        foreach (var s in salaries)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(s.EmployeeNo),
                CsvValue(s.PayItemCode),
                CsvValue(s.Rate),
                CsvValue(s.Amount),
                CsvValue(s.Quantity),
                CsvValue(s.PersonType)
            });
            sb.AppendLine(row);
        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // ----------------------------------
    // SALARY ADJUSTMENTS
    // ----------------------------------
    private static byte[] CreateSalaryAdjustmentsCsv(IEnumerable<SalaryAdjustment> adjustments)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[SalaryAdjustments]");
        sb.AppendLine("EmployeeNo,EmploymentRelationshipId,RemunerationType,AnnualSalary,HourlyRate,LastSalaryChangeDate");
        foreach (var a in adjustments)
        {
            var row = string.Join(",", new[]
            {
                CsvValue(a.EmployeeNo),
                CsvValue(a.EmploymentRelationshipId),
                CsvValue(a.RemunerationType),
                CsvValue(a.AnnualSalary),
                CsvValue(a.HourlyRate),
                CsvValue(a.LastSalaryChangeDate)
            });
            sb.AppendLine(row);
        }
        sb.AppendLine();
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    #endregion

    #region Private Helpers

    private static void WriteBytesToZipEntry(ZipArchiveEntry entry, byte[] bytes)
    {
        using var stream = entry.Open();
        stream.Write(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// CSV-escapes a value by wrapping quotes and escaping any existing quotes
    /// Example output: "Some ""quoted"" text"
    /// </summary>
    private static string CsvValue(object val)
    {
        if (val == null) return "\"\"";
        string s = val.ToString() ?? "";
        s = s.Replace("\"", "\"\"");
        return $"\"{s}\"";
    }

    #endregion
}
