using MedVet.Application.DTOs;

namespace MedVet.Application.Tests.DTOs;

public class PaginationTests
{
    [Theory]
    [InlineData(0, 20)]
    [InlineData(-1, 20)]
    [InlineData(-10, 50)]
    [InlineData(1, 0)]
    [InlineData(1, -5)]
    [InlineData(1, 101)]
    [InlineData(1, 999)]
    public void PaginationQuery_ComParametrosInvalidos_DeveRetornarErro(int page, int pageSize)
    {
        // Arrange
        var query = new PaginationQuery { Page = page, PageSize = pageSize };

        // Act
        var hasError = query.TryGetError(out var message);

        // Assert
        Assert.True(hasError);
        Assert.NotNull(message);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 20)]
    [InlineData(2, 50)]
    [InlineData(10, 100)]
    public void PaginationQuery_ComParametrosValidos_NaoDeveRetornarErro(int page, int pageSize)
    {
        // Arrange
        var query = new PaginationQuery { Page = page, PageSize = pageSize };

        // Act
        var hasError = query.TryGetError(out var message);

        // Assert
        Assert.False(hasError);
        Assert.Null(message);
    }

    [Fact]
    public void PagedResponse_CalculaTotalPagesENavegacaoCorretamente()
    {
        // Arrange
        var items = new List<string> { "item1", "item2" };

        // Act
        var response = new PagedResponse<string>(items, Page: 2, PageSize: 20, TotalItems: 45);

        // Assert
        Assert.Equal(3, response.TotalPages); // ceil(45 / 20) = 3
        Assert.True(response.HasPrevious);   // Page 2 > 1
        Assert.True(response.HasNext);       // Page 2 < 3
        Assert.Equal(2, response.Items.Count);
    }
}
