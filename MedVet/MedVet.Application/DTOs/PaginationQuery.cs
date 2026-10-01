namespace MedVet.Application.DTOs;

/// <summary>
/// Parametros de paginacao por pagina e tamanho de pagina (offset pagination).
/// </summary>
public class PaginationQuery
{
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    /// <summary>Numero da pagina (1-based).</summary>
    public int Page { get; set; } = 1;

    /// <summary>Quantidade de itens por pagina (1 a 100).</summary>
    public int PageSize { get; set; } = DefaultPageSize;

    /// <summary>Valida os limites de page e pageSize.</summary>
    public bool TryGetError(out string? message)
    {
        if (Page < 1)
        {
            message = "page deve ser >= 1.";
            return true;
        }

        if (PageSize is < 1 or > MaxPageSize)
        {
            message = $"pageSize deve estar entre 1 e {MaxPageSize}.";
            return true;
        }

        message = null;
        return false;
    }
}
