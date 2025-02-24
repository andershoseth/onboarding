namespace onboarding_backend
{
    public class FixedAsset
    {
            public string AssetCode { get; set; }  // Kode på anleggsmiddelet, påkrevd
            public string AssetName { get; set; }  // Navn på anleggsmiddel, påkrevd
            public string AssetTypeName { get; set; } // Navn på anleggsmiddelgruppe (må finnes)
            public string PurchaseDate { get; set; }   // DDMMYYYY
            public decimal? PurchasePrice { get; set; } // anskaffelseskost
            public string DepreciationMethod { get; set; } // f.eks. "None", "StraightLine", "DecliningBalance"
            public decimal? Rate { get; set; }          // sats for saldoavskrivning
            public int? EconomicLife { get; set; }      // levetid i antall måneder
            public decimal? Deprecation0101 { get; set; }
            public decimal? YTDDepreciation { get; set; }
            public string LastDepreciation { get; set; } // DDMMYYYY
            public string DepartmentCode { get; set; }
            public string ProjectCode { get; set; }
            public string LocationCode { get; set; }
            public string SerialNumber { get; set; }
    }

}

