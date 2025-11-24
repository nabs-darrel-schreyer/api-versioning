namespace ApiVersioning.Dtos.V1.Models;

public class AssetResponse
{
    public AssetId Id { get; set; } = default!;
    public AssetStatus Status { get; set; }
    public string? Notes { get; set; }
    public decimal? Value { get; set; }
}