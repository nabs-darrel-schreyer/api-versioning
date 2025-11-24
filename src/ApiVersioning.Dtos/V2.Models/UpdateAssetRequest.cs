namespace ApiVersioning.Dtos.V2.Models;

public class UpdateAssetRequest
{
    public AssetStatus Status { get; set; }
    public string? Notes { get; set; }

    // Breaking changes: Renamed from 'Value' to 'TotalValue'
    public decimal? TotalValue { get; set; }

    // New field added in V2
    public DateTime? RetiredOn { get; set; }
}
