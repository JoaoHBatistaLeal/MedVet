namespace MedVet.Application.DTOs;

/// <summary>
/// Envelope padrao para respostas de listagem paginada (v2).
/// </summary>
/// <typeparam name="T">Tipo do item contido na pagina.</typeparam>
public record PagedResponse<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalItems)
{
    /// <summary>Total de paginas calculado com base em TotalItems e PageSize.</summary>
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalItems / (double)PageSize);

    /// <summary>Indica se existe uma pagina anterior.</summary>
    public bool HasPrevious => Page > 1;

    /// <summary>Indica se existe uma proxima pagina.</summary>
    public bool HasNext => Page < TotalPages;
}
