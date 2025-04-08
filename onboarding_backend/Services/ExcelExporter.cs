using NPOI.XSSF.UserModel;  // for XSSFWorkbook
using NPOI.SS.UserModel;    // for IWorkbook, ISheet, IRow, etc.
using System.IO;
using System.Collections.Generic;
using onboarding_backend.Models.StandardImport;

namespace onboarding_backend.Services
{
    public static class ExcelExporter
    {
        /// <summary>
        /// Creates a single .xlsx workbook in memory with multiple sheets,
        /// one for each main entity in Standardimport, and returns the file as byte[].
        /// </summary>
        public static byte[] CreateStandardImportXlsx(Standardimport standardImport)
        {
            if (standardImport == null)
                throw new ArgumentNullException(nameof(standardImport));

            // 1) Create a new workbook
            IWorkbook workbook = new XSSFWorkbook();

            // 2) Create each sheet
            CreateContactsSheet(workbook, standardImport.Contact);
            CreateProductsSheet(workbook, standardImport.Product);
            CreateProjectsSheet(workbook, standardImport.Project);
            CreateProjectTeamSheet(workbook, standardImport.ProjectTeamMember);
            CreateProjectActivitySheet(workbook, standardImport.ProjectActivitie);
            CreateDepartmentsSheet(workbook, standardImport.Department);
            CreateVouchersSheet(workbook, standardImport.Voucher);
            CreateOrdersSheet(workbook, standardImport.Order);
            CreateQuotesSheet(workbook, standardImport.Quote);
            CreateInvoiceCidSheet(workbook, standardImport.InvoiceCid);
            CreateChartOfAccountsSheet(workbook, standardImport.ChartOfAccount);
            CreateFixedAssetsSheet(workbook, standardImport.FixedAsset);
            CreateYtdPayrollBalancesSheet(workbook, standardImport.YTDPayrollBalance);
            CreateSalaryBasisSheet(workbook, standardImport.SalaryBasis);
            CreateSalaryAdjustmentsSheet(workbook, standardImport.SalaryAdjustments);

            // 3) Convert the workbook into a byte[] (in memory)
            using var ms = new MemoryStream();
            workbook.Write(ms);
            return ms.ToArray();
        }

        // ----------------------------------------------------------------
        // Each method below: 
        //  - creates a new worksheet,
        //  - writes a header row,
        //  - iterates the relevant list to create data rows.
        // 
        // Adjust columns, header text, and data fields as needed to match
        // exactly what your import template expects.
        // ----------------------------------------------------------------

        private static void CreateContactsSheet(IWorkbook workbook, IEnumerable<Contacts> contacts)
        {
            ISheet sheet = workbook.CreateSheet("Contacts");

            // Header row
            var header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("[Contacts]");  // If you want a "decorator" in the first cell
                                                              // or you could split the "decorator" from the column headers
            // Example columns: 
            // Row 1: actual column headers
            var columns = new string[]
            {
                "CustomerNo","SupplierNo","EmployeeNo","ContactName","Phone","Email","MailCountry"
            };
            var header2 = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header2.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var c in contacts)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(c.CustomerNo?.ToString() ?? "");
                row.CreateCell(1).SetCellValue(c.SupplierNo?.ToString() ?? "");
                row.CreateCell(2).SetCellValue(c.EmployeeNo?.ToString() ?? "");
                row.CreateCell(3).SetCellValue(c.ContactName ?? "");
                row.CreateCell(4).SetCellValue(c.Phone ?? "");
                row.CreateCell(5).SetCellValue(c.Email ?? "");
                row.CreateCell(6).SetCellValue(c.MailCountry ?? "");
            }

            // Auto-size columns (optional)
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateProductsSheet(IWorkbook workbook, IEnumerable<Products> products)
        {
            ISheet sheet = workbook.CreateSheet("Products");

            // Row 0: Decorator
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[Products]");

            // Row 1: column headers
            var columns = new string[] {
                "ProductCode","ProductName","ProductGroup","ProductSalesPrice","ProductSalesAccount"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var p in products)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(p.ProductCode ?? "");
                row.CreateCell(1).SetCellValue(p.ProductName ?? "");
                row.CreateCell(2).SetCellValue(p.ProductGroup ?? "");
                row.CreateCell(3).SetCellValue((double?)p.ProductSalesPrice ?? 0.0);
                row.CreateCell(4).SetCellValue(p.ProductSalesAccount.ToString());
            }

            // Optional auto-size
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateProjectsSheet(IWorkbook workbook, IEnumerable<Projects> projects)
        {
            ISheet sheet = workbook.CreateSheet("Projects");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[Projects]");
            var columns = new string[]
            {
                "ProjectCode","ProjectName","ProjectManagerCode","ProjectStartDate","ProjectEndDate"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var prj in projects)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(prj.ProjectCode ?? "");
                row.CreateCell(1).SetCellValue(prj.ProjectName ?? "");
                row.CreateCell(2).SetCellValue((double?) prj.ProjectManagerCode ?? 0.0);
                row.CreateCell(3).SetCellValue(prj.ProjectStartDate ?? "");
                row.CreateCell(4).SetCellValue(prj.ProjectEndDate ?? "");
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateProjectTeamSheet(IWorkbook workbook, IEnumerable<ProjectTeamMembers> team)
        {
            ISheet sheet = workbook.CreateSheet("ProjectTeamMembers");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[ProjectTeamMembers]");
            var columns = new string[]
            {
                "ProjectCode","EmployeeNo","Hours","HourlyRate"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var member in team)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(member.ProjectCode ?? "");
                row.CreateCell(1).SetCellValue(member.EmployeeNo.ToString());
                row.CreateCell(2).SetCellValue((double?)member.Hours ?? 0.0);
                row.CreateCell(3).SetCellValue((double?)member.HourlyRate ?? 0.0);
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateProjectActivitySheet(IWorkbook workbook, IEnumerable<ProjectActivity> activities)
        {
            ISheet sheet = workbook.CreateSheet("ProjectActivities");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[ProjectActivity]");
            var columns = new string[]
            {
                "ProjectCode","ActivityCode","ActivityName","ProjectBillable","HourlyRate"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var a in activities)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(a.ProjectCode ?? "");
                row.CreateCell(1).SetCellValue(a.ActivityCode ?? "");
                row.CreateCell(2).SetCellValue(a.ActivityName ?? "");
                row.CreateCell(3).SetCellValue(a.ProjectBillable?.ToString() ?? "");
                row.CreateCell(4).SetCellValue((double?)a.HourlyRate ?? 0.0);
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateDepartmentsSheet(IWorkbook workbook, IEnumerable<Departments> depts)
        {
            ISheet sheet = workbook.CreateSheet("Departments");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[Departments]");
            var columns = new string[]
            {
                "DepartmentCode","DepartmentName","DepartmentManagerCode"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var d in depts)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(d.DepartmentCode ?? "");
                row.CreateCell(1).SetCellValue(d.DepartmentName ?? "");
                row.CreateCell(2).SetCellValue(d.DepartmentManagerCode?.ToString() ?? "");
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateVouchersSheet(IWorkbook workbook, IEnumerable<Voucher> vouchers)
        {
            ISheet sheet = workbook.CreateSheet("Vouchers");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[Vouchers]");

            // This sheet might have to handle multiple lines per voucher
            // if you want one row per line, for instance:
            var columns = new string[]
            {
                "VoucherNo","DocumentDate","PostingDate","VoucherType","Account","Amount","Description"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var v in vouchers)
            {
                // If you want a new row per line:
                foreach (var line in v.Lines)
                {
                    IRow row = sheet.CreateRow(rowIndex++);
                    row.CreateCell(0).SetCellValue(v.VoucherNo);
                    row.CreateCell(1).SetCellValue(v.DocumentDate ?? "");
                    row.CreateCell(2).SetCellValue(v.PostingDate ?? "");
                    row.CreateCell(3).SetCellValue(v.VoucherType.ToString());
                    row.CreateCell(4).SetCellValue(line.Account?.ToString() ?? "");
                    row.CreateCell(5).SetCellValue((double?)line.Amount ?? 0.0);
                    row.CreateCell(6).SetCellValue(line.Description ?? "");
                }
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateOrdersSheet(IWorkbook workbook, IEnumerable<Order> orders)
        {
            ISheet sheet = workbook.CreateSheet("Orders");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[Orders]");
            var columns = new string[]
            {
                "OrderNo","OrderDate","CustomerNo","ContactName","ProductCode","ProductName","Quantity","OrderLineUnitPrice"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var o in orders)
            {
                // If you only have "one-liner" order data, just do one row per 'Order'
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(o.OrderNo?.ToString() ?? "");
                row.CreateCell(1).SetCellValue(o.OrderDate ?? "");
                row.CreateCell(2).SetCellValue(o.CustomerNo?.ToString() ?? "");
                row.CreateCell(3).SetCellValue(o.ContactName ?? "");
                row.CreateCell(4).SetCellValue(o.ProductCode ?? "");
                row.CreateCell(5).SetCellValue(o.ProductName ?? "");
                row.CreateCell(6).SetCellValue((double?)o.Quantity ?? 0.0);
                row.CreateCell(7).SetCellValue((double?)o.OrderLineUnitPrice ?? 0.0);
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateQuotesSheet(IWorkbook workbook, IEnumerable<Quote> quotes)
        {
            ISheet sheet = workbook.CreateSheet("Quotes");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[Quotes]");
            var columns = new string[]
            {
                "QuoteNo","QuoteDate","CustomerNo","ContactName","ProductCode","ProductName","Quantity","QuoteLineUnitPrice"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var q in quotes)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(q.QuoteNo?.ToString() ?? "");
                row.CreateCell(1).SetCellValue(q.QuoteDate ?? "");
                row.CreateCell(2).SetCellValue(q.CustomerNo?.ToString() ?? "");
                row.CreateCell(3).SetCellValue(q.ContactName ?? "");
                row.CreateCell(4).SetCellValue(q.ProductCode ?? "");
                row.CreateCell(5).SetCellValue(q.ProductName ?? "");
                row.CreateCell(6).SetCellValue((double?)q.Quantity ?? 0.0);
                row.CreateCell(7).SetCellValue((double?)q.QuoteLineUnitPrice ?? 0.0);
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateInvoiceCidSheet(IWorkbook workbook, IEnumerable<InvoiceCid> invoiceCids)
        {
            ISheet sheet = workbook.CreateSheet("InvoiceCids");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[InvoiceCid]");
            var columns = new string[] { "InvoiceNo","CID" };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var i in invoiceCids)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(i.InvoiceNo ?? "");
                row.CreateCell(1).SetCellValue(i.CID ?? "");
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateChartOfAccountsSheet(IWorkbook workbook, IEnumerable<ChartOfAccount> charts)
        {
            ISheet sheet = workbook.CreateSheet("ChartOfAccounts");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[ChartOfAccounts]");
            var columns = new string[]
            {
                "Account","AccountName","VAT","AccountAgricultureDepartment","IsActive"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var c in charts)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(c.Account.ToString());
                row.CreateCell(1).SetCellValue(c.AccountName ?? "");
                row.CreateCell(2).SetCellValue(c.VAT ?? "");
                row.CreateCell(3).SetCellValue(c.AccountAgricultureDepartment ?? "");
                row.CreateCell(4).SetCellValue(c.IsActive?.ToString() ?? "");
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateFixedAssetsSheet(IWorkbook workbook, IEnumerable<FixedAsset> assets)
        {
            ISheet sheet = workbook.CreateSheet("FixedAssets");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[FixedAssets]");
            var columns = new string[]
            {
                "AssetCode","AssetName","AssetTypeName","PurchaseDate","PurchasePrice","DepreciationMethod"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var fa in assets)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(fa.AssetCode ?? "");
                row.CreateCell(1).SetCellValue(fa.AssetName ?? "");
                row.CreateCell(2).SetCellValue(fa.AssetTypeName ?? "");
                row.CreateCell(3).SetCellValue(fa.PurchaseDate ?? "");
                row.CreateCell(4).SetCellValue((double?)fa.PurchasePrice ?? 0.0);
                row.CreateCell(5).SetCellValue(fa.DepreciationMethod ?? "");
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateYtdPayrollBalancesSheet(IWorkbook workbook, IEnumerable<YTDPayrollBalance> ytds)
        {
            ISheet sheet = workbook.CreateSheet("YTDPayrollBalances");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[YTDPayrollBalances]");
            var columns = new string[]
            {
                "SocialSecurityNumber","EmploymentRelationshipId","PayItemCode","Amount","Year"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var y in ytds)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(y.SocialSecurityNumber ?? "");
                row.CreateCell(1).SetCellValue(y.EmploymentRelationshipId ?? "");
                row.CreateCell(2).SetCellValue(y.PayItemCode ?? "");
                row.CreateCell(3).SetCellValue((double?)y.Amount ?? 0.0);
                row.CreateCell(4).SetCellValue(y.Year?.ToString() ?? "");
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateSalaryBasisSheet(IWorkbook workbook, IEnumerable<SalaryBasis> salaries)
        {
            ISheet sheet = workbook.CreateSheet("SalaryBasis");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[SalaryBasis]");
            var columns = new string[]
            {
                "EmployeeNo","PayItemCode","Rate","Amount","Quantity","PersonType"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var s in salaries)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(s.EmployeeNo?.ToString() ?? "");
                row.CreateCell(1).SetCellValue(s.PayItemCode ?? "");
                row.CreateCell(2).SetCellValue((double?)s.Rate ?? 0.0);
                row.CreateCell(3).SetCellValue((double?)s.Amount ?? 0.0);
                row.CreateCell(4).SetCellValue((double?)s.Quantity ?? 0.0);
                row.CreateCell(5).SetCellValue(s.PersonType ?? "");
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }

        private static void CreateSalaryAdjustmentsSheet(IWorkbook workbook, IEnumerable<SalaryAdjustment> adjustments)
        {
            ISheet sheet = workbook.CreateSheet("SalaryAdjustments");
            sheet.CreateRow(0).CreateCell(0).SetCellValue("[SalaryAdjustments]");
            var columns = new string[]
            {
                "EmployeeNo","EmploymentRelationshipId","RemunerationType",
                "AnnualSalary","HourlyRate","LastSalaryChangeDate"
            };
            var header = sheet.CreateRow(1);
            for (int i = 0; i < columns.Length; i++)
                header.CreateCell(i).SetCellValue(columns[i]);

            int rowIndex = 2;
            foreach (var a in adjustments)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                row.CreateCell(0).SetCellValue(a.EmployeeNo?.ToString() ?? "");
                row.CreateCell(1).SetCellValue(a.EmploymentRelationshipId ?? "");
                row.CreateCell(2).SetCellValue(a.RemunerationType ?? "");
                row.CreateCell(3).SetCellValue((double?)a.AnnualSalary ?? 0.0);
                row.CreateCell(4).SetCellValue((double?)a.HourlyRate ?? 0.0);
                row.CreateCell(5).SetCellValue(a.LastSalaryChangeDate ?? "");
            }
            for (int i = 0; i < columns.Length; i++)
                sheet.AutoSizeColumn(i);
        }
    }
}
