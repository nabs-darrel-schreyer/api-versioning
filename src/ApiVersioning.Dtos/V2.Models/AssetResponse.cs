namespace ApiVersioning.Dtos.V2.Models;

public class AssetResponse
{
    public AssetId AssetId { get; set; } = default!;
    public AssetStatus Status { get; set; }
    public string? Notes { get; set; }
    public decimal? TotalValue { get; set; }
    public DateTime? RetiredOn { get; set; }
}