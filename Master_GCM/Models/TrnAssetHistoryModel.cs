using System.ComponentModel.DataAnnotations.Schema;

public class TRNASSETHISTORYMODEL{
    public int IDASSETHISTORY { get; set; }
    public required string ASSETCODE { get; set; }
    public int? NIPP { get; set; }
    public required string STATUS { get; set; }
    public required string PICADDED { get; set; }
    public DateOnly DATEADDED { get; set; }

    // [ForeignKey("NIPP")]
    // public MSTEMPLOYEEMODEL? EMPLOYEE { get; set; }

    [ForeignKey("ASSETCODE")]
    public TRNASSETMODEL? TRNASSET { get; set; }
}