using System.IO;
using System.Collections.Generic;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using onboarding_backend.Models.StandardImport;

namespace onboarding_backend.Services
{
    public static class ExcelSingleSheetExporter
    {
        /// <summary>
        /// Creates one .xlsx with a single worksheet, appending data from all 15 lists.
        /// The "decorators" like [Contacts], [Products], etc. appear inline in the rows.
        /// Then returns the .xlsx as a byte[].
        /// </summary>
        public static byte[] CreateSingleSheet(Standardimport data)
        {
            // Basic checks
            if (data == null) throw new ArgumentNullException(nameof(data));

            // Create workbook and single sheet
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("FullData");

            // We'll keep a row index to know where to write next
            int rowIndex = 0;

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
        /// Writes a block for [Contacts] in the single sheet, returning the updated row index
        /// </summary>
        private static int WriteContactsBlock(ISheet sheet, IEnumerable<Contacts> contacts, int startRow)
        {
            // Decorator row: "[Contacts]"
            IRow decRow = sheet.CreateRow(startRow++);
            decRow.CreateCell(0).SetCellValue("[Contacts]");

            // Header row (use the exact columns your importer expects)
            var headerRow = sheet.CreateRow(startRow++);
            string[] headers = new [] {
                "CustomerNo", "SupplierNo", "EmployeeNo", "ContactName", "Phone", "Email", "MailCountry"
            };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            // Data rows
            foreach(var c in contacts)
            {
                IRow row = sheet.CreateRow(startRow++);
                row.CreateCell(0).SetCellValue(c.CustomerNo?.ToString() ?? "");
                row.CreateCell(1).SetCellValue(c.SupplierNo?.ToString() ?? "");
                row.CreateCell(2).SetCellValue(c.EmployeeNo?.ToString() ?? "");
                row.CreateCell(3).SetCellValue(c.ContactName ?? "");
                row.CreateCell(4).SetCellValue(c.Phone ?? "");
                row.CreateCell(5).SetCellValue(c.Email ?? "");
                row.CreateCell(6).SetCellValue(c.MailCountry ?? "");
            }

            // Optionally add one blank row to separate this block from the next
            startRow++;

            return startRow;
        }

        private static int WriteProductsBlock(ISheet sheet, IEnumerable<Products> products, int startRow)
        {
            IRow decRow = sheet.CreateRow(startRow++);
            decRow.CreateCell(0).SetCellValue("[Products]");

            var headerRow = sheet.CreateRow(startRow++);
            string[] headers = new [] {
                "ProductCode","ProductName","ProductGroup","ProductSalesPrice","ProductSalesAccount"
            };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var p in products)
            {
                IRow row = sheet.CreateRow(startRow++);
                row.CreateCell(0).SetCellValue(p.ProductCode ?? "");
                row.CreateCell(1).SetCellValue(p.ProductName ?? "");
                row.CreateCell(2).SetCellValue(p.ProductGroup ?? "");
                row.CreateCell(3).SetCellValue((double?)p.ProductSalesPrice ?? 0.0);
                row.CreateCell(4).SetCellValue(p.ProductSalesAccount.ToString());
            }

            startRow++;
            return startRow;
        }

        private static int WriteProjectsBlock(ISheet sheet, IEnumerable<Projects> projects, int startRow)
        {
            var decRow = sheet.CreateRow(startRow++);
            decRow.CreateCell(0).SetCellValue("[Projects]");

            var headerRow = sheet.CreateRow(startRow++);
            string[] headers = {
                "ProjectCode","ProjectName","ProjectManagerCode","ProjectStartDate","ProjectEndDate"
            };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var prj in projects)
            {
                var row = sheet.CreateRow(startRow++);
                row.CreateCell(0).SetCellValue(prj.ProjectCode ?? "");
                row.CreateCell(1).SetCellValue(prj.ProjectName ?? "");
                row.CreateCell(2).SetCellValue(prj.ProjectManagerCode ?? "");
                row.CreateCell(3).SetCellValue(prj.ProjectStartDate ?? "");
                row.CreateCell(4).SetCellValue(prj.ProjectEndDate ?? "");
            }

            startRow++;
            return startRow;
        }

        private static int WriteProjectTeamBlock(ISheet sheet, IEnumerable<ProjectTeamMembers> team, int startRow)
        {
            var decRow = sheet.CreateRow(startRow++);
            decRow.CreateCell(0).SetCellValue("[ProjectTeamMembers]");

            var headerRow = sheet.CreateRow(startRow++);
            string[] headers = { "ProjectCode", "EmployeeNo", "Hours", "HourlyRate" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var m in team)
            {
                var row = sheet.CreateRow(startRow++);
                row.CreateCell(0).SetCellValue(m.ProjectCode ?? "");
                row.CreateCell(1).SetCellValue(m.EmployeeNo.ToString());
                row.CreateCell(2).SetCellValue((double?)m.Hours ?? 0.0);
                row.CreateCell(3).SetCellValue((double?)m.HourlyRate ?? 0.0);
            }

            startRow++;
            return startRow;
        }

        private static int WriteProjectActivitiesBlock(ISheet sheet, IEnumerable<ProjectActivity> activities, int startRow)
        {
            var decRow = sheet.CreateRow(startRow++);
            decRow.CreateCell(0).SetCellValue("[ProjectActivity]");

            var headerRow = sheet.CreateRow(startRow++);
            string[] headers = { "ProjectCode","ActivityCode","ActivityName","ProjectBillable","HourlyRate" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var a in activities)
            {
                var row = sheet.CreateRow(startRow++);
                row.CreateCell(0).SetCellValue(a.ProjectCode ?? "");
                row.CreateCell(1).SetCellValue(a.ActivityCode ?? "");
                row.CreateCell(2).SetCellValue(a.ActivityName ?? "");
                row.CreateCell(3).SetCellValue(a.ProjectBillable?.ToString() ?? "");
                row.CreateCell(4).SetCellValue((double?)a.HourlyRate ?? 0.0);
            }

            startRow++;
            return startRow;
        }

        private static int WriteDepartmentsBlock(ISheet sheet, IEnumerable<Departments> departments, int startRow)
        {
            var decRow = sheet.CreateRow(startRow++);
            decRow.CreateCell(0).SetCellValue("[Departments]");

            var headerRow = sheet.CreateRow(startRow++);
            string[] headers = { "DepartmentCode","DepartmentName","DepartmentManagerCode" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var d in departments)
            {
                var row = sheet.CreateRow(startRow++);
                row.CreateCell(0).SetCellValue(d.DepartmentCode ?? "");
                row.CreateCell(1).SetCellValue(d.DepartmentName ?? "");
                row.CreateCell(2).SetCellValue(d.DepartmentManagerCode?.ToString() ?? "");
            }

            startRow++;
            return startRow;
        }

        private static int WriteVouchersBlock(ISheet sheet, IEnumerable<Voucher> vouchers, int startRow)
        {
            var decRow = sheet.CreateRow(startRow++);
            decRow.CreateCell(0).SetCellValue("[Vouchers]");

            var headerRow = sheet.CreateRow(startRow++);
            string[] headers = { "VoucherNo","DocumentDate","PostingDate","VoucherType","Account","Amount","Description" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var v in vouchers)
            {
                // multiple lines per voucher
                foreach(var line in v.Lines)
                {
                    var row = sheet.CreateRow(startRow++);
                    row.CreateCell(0).SetCellValue(v.VoucherNo);
                    row.CreateCell(1).SetCellValue(v.DocumentDate ?? "");
                    row.CreateCell(2).SetCellValue(v.PostingDate ?? "");
                    row.CreateCell(3).SetCellValue(v.VoucherType.ToString());
                    row.CreateCell(4).SetCellValue(line.Account?.ToString() ?? "");
                    row.CreateCell(5).SetCellValue((double?)line.Amount ?? 0.0);
                    row.CreateCell(6).SetCellValue(line.Description ?? "");
                }
            }

            startRow++;
            return startRow;
        }

        private static int WriteOrdersBlock(ISheet sheet, IEnumerable<Order> orders, int startRow)
        {
            var decRow = sheet.CreateRow(startRow++);
            decRow.CreateCell(0).SetCellValue("[Orders]");

            var headerRow = sheet.CreateRow(startRow++);
            string[] headers = { "OrderNo","OrderDate","CustomerNo","ContactName","ProductCode","ProductName","Quantity","OrderLineUnitPrice" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var o in orders)
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
            string[] headers = { "QuoteNo","QuoteDate","CustomerNo","ContactName","ProductCode","ProductName","Quantity","QuoteLineUnitPrice" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var q in quotes)
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
            string[] headers = { "InvoiceNo","CID" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var ic in invoiceCids)
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
            string[] headers = { "Account","AccountName","VAT","AccountAgricultureDepartment","IsActive" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var c in charts)
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
            string[] headers = { "AssetCode","AssetName","AssetTypeName","PurchaseDate","PurchasePrice","DepreciationMethod" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var fa in assets)
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
            string[] headers = { "SocialSecurityNumber","EmploymentRelationshipId","PayItemCode","Amount","Year" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var y in ytds)
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
            string[] headers = { "EmployeeNo","PayItemCode","Rate","Amount","Quantity","PersonType" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var s in salaries)
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

        private static int WriteSalaryAdjustmentsBlock(ISheet sheet, IEnumerable<SalaryAdjustment> adjustments, int startRow)
        {
            var decRow = sheet.CreateRow(startRow++);
            decRow.CreateCell(0).SetCellValue("[SalaryAdjustments]");

            var headerRow = sheet.CreateRow(startRow++);
            string[] headers = { "EmployeeNo","EmploymentRelationshipId","RemunerationType","AnnualSalary","HourlyRate","LastSalaryChangeDate" };
            for (int i=0; i<headers.Length; i++)
                headerRow.CreateCell(i).SetCellValue(headers[i]);

            foreach(var a in adjustments)
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
}
