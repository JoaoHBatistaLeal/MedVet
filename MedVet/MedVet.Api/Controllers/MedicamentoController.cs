using MedVet.Application.DTOs;
using MedVet.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedVet.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de medicamentos via repositorio generico.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class MedicamentoController(IMedicamentoService medicamentoService, ILogger<MedicamentoController> logger) : ControllerBase
{
    /// <summary>
    /// Lista todos os medicamentos disponiveis.
    /// </summary>
    /// <response code="200">Lista de medicamentos retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MedicamentoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(medicamentoService.GetAll());
    }

    /// <summary>
    /// Obtem um medicamento pelo identificador unico.
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
    /// Cadastra um novo medicamento utilizando o repositorio generico.
    /// </summary>
    /// <param name="request">Dados do medicamento.</param>
    /// <response code="201">Medicamento cadastrado com sucesso.</response>
    /// <response code="400">Dados invalidos para cadastro.</response>
    [HttpPost]
    [ProducesResponseType(typeof(MedicamentoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] MedicamentoRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;
        logger.LogInformation("Iniciando criacao de medicamento: {NomeMedicamento}, TraceId: {TraceId}", request.NomeMedicamento, traceId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = medicamentoService.Create(request);
        logger.LogInformation("Finalizando criacao de medicamento: {NomeMedicamento}, Id: {Id}, TraceId: {TraceId}", created.NomeMedicamento, created.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Remove um medicamento pelo identificador unico.
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
