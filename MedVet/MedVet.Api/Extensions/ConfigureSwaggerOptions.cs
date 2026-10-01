using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MedVet.Api.Extensions;

/// <summary>
/// Gera um SwaggerDoc por versao descoberta pelo IApiVersionDescriptionProvider.
/// Permite alternar entre versoes v1 (deprecada) e v2 (atual) no Swagger UI.
/// </summary>
public sealed class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = "MedVet API",
                    Version = description.ApiVersion.ToString(),
                    Description = description.IsDeprecated
                        ? "Esta versao esta deprecada. Utilize a versao mais recente (v2.0) com suporte a paginacao."
                        : "API REST para gerenciamento da clinica veterinaria MedVet.",
                    Contact = new OpenApiContact
                    {
                        Name = "Equipe MedVet"
                    }
                });
        }
    }
}
