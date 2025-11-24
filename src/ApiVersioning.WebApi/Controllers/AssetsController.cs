using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ApiVersioning.WebApi.Controllers;

[ApiVersion("1", Deprecated = true)]
[ApiController]
[Route("api/v{v:apiVersion}/assets")]
public class AssetsV1Controller : ControllerBase
{
    [HttpPatch()]
    public IActionResult Update( string id, [FromBody] Dtos.V1.Models.UpdateAssetRequest dto)
    {
        var response = new Dtos.V1.Models.AssetResponse
        {
            Id = new Dtos.AssetId(id),
            Status = dto.Status,
            Notes = dto.Notes,
            Value = dto.Value
        };
        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id) => Ok(new Dtos.V1.Models.AssetResponse { 
        Id = new Dtos.AssetId(id) 
    });

}


[ApiVersion("2")]
[ApiController]
[Route("api/v{v:apiVersion}/assets")]
[Route("api/assets")]
public class AssetsV2Controller : ControllerBase
{
    [HttpPatch()]
    public IActionResult Update(string id, [FromBody] Dtos.V2.Models.UpdateAssetRequest dto)
    {
        var response = new Dtos.V2.Models.AssetResponse
        {
            AssetId = new Dtos.AssetId(id),
            Status = dto.Status,
            Notes = dto.Notes,
            TotalValue = dto.TotalValue,
            RetiredOn = dto.RetiredOn
        };
        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id) => Ok(new Dtos.V2.Models.AssetResponse { 
        AssetId = new Dtos.AssetId(id) 
    });
}
