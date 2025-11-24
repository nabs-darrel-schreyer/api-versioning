namespace ApiVersioning.Dtos;

public record AssetId(string Value);
public enum AssetStatus {  New, Active, Inactive, Retired }