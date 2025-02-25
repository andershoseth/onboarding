namespace onboarding_backend
{
    public class ChartOfAccount
    {
       
            public int Account { get; set; }         // Numerisk, påkrevd
            public string AccountName { get; set; }  // Tekst, påkrevd

            public string AccountAgricultureDepartment { get; set; } // se egen tabell (10=Finans,20=Jordbruk,...)
            public string VAT { get; set; }                  // standard mva-kode
            public string VATReturnSpecification { get; set; }
            public string BankAccount { get; set; }          // hvis dette er en bankkonto

            public int? IsProjectRequired { get; set; }      // Blank/0=false, 1=true
            public int? IsDepartmentRequired { get; set; }
            public int? IsLocationRequired { get; set; }
            public int? IsFixedAssetsRequired { get; set; }
            public int? IsEnterpriseRequired { get; set; }
            public int? IsActivityRequired { get; set; }
            public int? IsDim1Required { get; set; }
            public int? IsDim2Required { get; set; }
            public int? IsDim3Required { get; set; }
            public int? IsQuantityRequired { get; set; }
            public int? IsQuantity2Required { get; set; }
            public int? IsProductRequired { get; set; }
            public int? IsAgricultureProductRequired { get; set; }

            public string StandardProjectCode { get; set; }
            public string StandardDepartmentCode { get; set; }
            public int? LockVatCode { get; set; }          // Blank/0=false,1=true
            public int? IsActive { get; set; }            // 0=false,1= true
        }

    }

