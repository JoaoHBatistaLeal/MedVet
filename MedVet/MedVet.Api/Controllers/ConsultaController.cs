using MedVet.Application.DTOs;
using MedVet.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedVet.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de consultas veterinarias.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ConsultaController(IConsultaService consultaService, ILogger<ConsultaController> logger) : ControllerBase
{
    /// <summary>
    /// Lista todas as consultas realizadas.
    /// </summary>
    /// <response code="200">Lista de consultas retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ConsultaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(consultaService.GetAll());
    }

    /// <summary>
    /// Obtem uma consulta pelo identificador unico.
    /// </summary>
    /// <param name="id">Identificador unico da consulta.</param>
    /// <response code="200">Consulta localizada com sucesso.</response>
    /// <response code="404">Consulta nao encontrada.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ConsultaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var consulta = consultaService.GetById(id);
        if (consulta is null)
            return NotFound();

        return Ok(consulta);
    }

    /// <summary>
    /// Cadastra uma nova consulta veterinaria.
    /// </summary>
    /// <param name="request">Dados da consulta vinculando pet e veterinario.</param>
    /// <response code="201">Consulta agendada/criada com sucesso.</response>
    /// <response code="400">Dados invalidos ou referencias nao localizadas.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ConsultaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] ConsultaRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;
        logger.LogInformation("Iniciando criacao de consulta para Pet: {PetId}, Vet: {VetId}, TraceId: {TraceId}", request.IdPet, request.IdVeterinario, traceId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = consultaService.Create(request);
        logger.LogInformation("Finalizando criacao de consulta, Id: {Id}, TraceId: {TraceId}", created.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Remove uma consulta pelo identificador unico.
    /// </summary>
    /// <param name="id">Identificador unico da consulta.</param>
    /// <response code="204">Consulta removida com sucesso.</response>
    /// <response code="404">Consulta nao encontrada.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return consultaService.Delete(id) ? NoContent() : NotFound();
    }
}
