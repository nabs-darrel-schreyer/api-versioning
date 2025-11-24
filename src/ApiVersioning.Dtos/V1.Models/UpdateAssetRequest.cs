namespace ApiVersioning.Dtos.V1.Models;

public class UpdateAssetRequest
{
    public AssetStatus Status { get; set; }
    public string? Notes { get; set; }
    public decimal? Value { get; set; }
}
