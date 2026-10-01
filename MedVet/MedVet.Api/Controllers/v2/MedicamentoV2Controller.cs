using Asp.Versioning;
using MedVet.Application.DTOs;
using MedVet.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MedVet.Api.Controllers.v2;

/// <summary>
/// Endpoints para gerenciamento de medicamentos (Contrato v2 - Atual com suporte a Paginacao e Rate Limiting).
/// </summary>
[ApiController]
[ApiVersion("2.0")]
[Route("api/medicamento")]
[Produces("application/json")]
public class MedicamentoV2Controller(
    IMedicamentoService medicamentoService,
    ILogger<MedicamentoV2Controller> logger) : ControllerBase
{
    /// <summary>
    /// Lista medicamentos de forma paginada com envelope de metadados.
    /// </summary>
    /// <param name="query">Parametros de paginacao (page maior ou igual a 1, pageSize entre 1 e 100).</param>
    /// <response code="200">Envelope paginado contendo os medicamentos e totais.</response>
    /// <response code="400">Parametros de paginacao invalidos.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<MedicamentoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult GetAllPaginated([FromQuery] PaginationQuery query)
    {
        if (query.TryGetError(out var message))
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Parametros de paginacao invalidos",
                Detail = message
            });
        }

        var (items, total) = medicamentoService.GetPaged(query.Page, query.PageSize);

        return Ok(new PagedResponse<MedicamentoResponse>(
            items,
            query.Page,
            query.PageSize,
            total));
    }

    /// <summary>
    /// Obtem um medicamento pelo identificador unico na versao 2.
    /// </summary>
    /// <param name="id">Identificador unico do medicamento.</param>
    /// <response code="200">Medicamento localizado com sucesso.</response>
    /// <response code="404">Medicamento nao encontrado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MedicamentoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var medicamento = medicamentoService.GetById(id);
        if (medicamento is null)
            return NotFound();

        return Ok(medicamento);
    }

    /// <summary>
    /// Cadastra um novo medicamento com restricao de taxa (Rate Limiting - Fixed Window).
    /// </summary>
    /// <param name="request">Dados do medicamento.</param>
    /// <response code="201">Medicamento cadastrado com sucesso.</response>
    /// <response code="400">Dados invalidos para cadastro.</response>
    /// <response code="429">Limite de requisicoes excedido.</response>
    [HttpPost]
    [EnableRateLimiting("fixed")]
    [ProducesResponseType(typeof(MedicamentoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public IActionResult Create([FromBody] MedicamentoRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;
        logger.LogInformation("Iniciando criacao de medicamento v2: {NomeMedicamento}, TraceId: {TraceId}", request.NomeMedicamento, traceId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = medicamentoService.Create(request);
        logger.LogInformation("Finalizando criacao de medicamento v2: {NomeMedicamento}, Id: {Id}, TraceId: {TraceId}", created.NomeMedicamento, created.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Remove um medicamento pelo identificador unico na versao 2.
    /// </summary>
    /// <param name="id">Identificador unico do medicamento.</param>
    /// <response code="204">Medicamento removido com sucesso.</response>
    /// <response code="404">Medicamento nao encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return medicamentoService.Delete(id) ? NoContent() : NotFound();
    }
}
