using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(2, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;                       // Adds headers: api-supported-versions, api-deprecated-versions
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),                   // /api/v1/jobs
        new HeaderApiVersionReader("x-api-version")         // Fallback: header
    );
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";                     // Swagger groups: v1, v2
    options.SubstituteApiVersionInUrl = true;               // Replaces {v:apiVersion} in docs
});

string[] versions = { "v1", "v2" };
foreach (var version in versions)
{
    builder.Services.AddOpenApi(version, options =>
    {
        options.AddDocumentTransformer((document, context, _) =>
        {
            var descriptionProvider = context.ApplicationServices
                .GetRequiredService<IApiVersionDescriptionProvider>();
            var versionDescription = descriptionProvider.ApiVersionDescriptions
                .FirstOrDefault(x => x.GroupName == version);
            document.Info.Version = versionDescription?.ApiVersion.ToString();

            // Add deprecation metadata for the document.
            if (versionDescription?.IsDeprecated == true)
            {
                document.Info?.Description += "\n\n**Deprecated**: This API version is deprecated. Please migrate to the latest version.";

                // Mark all operations as deprecated (Scalar will show warnings per endpoint)
                var operations = document.Paths
                    .Where(p => p.Value.Operations != null)
                    .SelectMany(p => p.Value.Operations!.Values);

                foreach (var operation in operations)
                {
                    operation.Deprecated = true;
                    operation.Description = (operation.Description ?? "") + "\n\n**Deprecated**: Use latest version instead.";
                }
            }

            return Task.CompletedTask;
        });
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddDocuments(versions);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
