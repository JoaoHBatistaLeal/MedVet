using MedVet.Application.DTOs;
using MedVet.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedVet.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de medicos veterinarios.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class VeterinarioController(IVeterinarioService veterinarioService, ILogger<VeterinarioController> logger) : ControllerBase
{
    /// <summary>
    /// Lista todos os veterinarios cadastrados.
    /// </summary>
    /// <response code="200">Lista de veterinarios retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<VeterinarioResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(veterinarioService.GetAll());
    }

    /// <summary>
    /// Obtem um veterinario pelo identificador unico.
    /// </summary>
    /// <param name="id">Identificador unico do veterinario.</param>
    /// <response code="200">Veterinario localizado com sucesso.</response>
    /// <response code="404">Veterinario nao encontrado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VeterinarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var vet = veterinarioService.GetById(id);
        if (vet is null)
            return NotFound();

        return Ok(vet);
    }

    /// <summary>
    /// Cadastra um novo veterinario.
    /// </summary>
    /// <param name="request">Dados do veterinario.</param>
    /// <response code="201">Veterinario cadastrado com sucesso.</response>
    /// <response code="400">Dados invalidos para cadastro.</response>
    [HttpPost]
    [ProducesResponseType(typeof(VeterinarioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] VeterinarioRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;
        logger.LogInformation("Iniciando criacao de veterinario: {NomeVet}, CRMV: {Crmv}, TraceId: {TraceId}", request.Nome, request.Crmv, traceId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = veterinarioService.Create(request);
        logger.LogInformation("Finalizando criacao de veterinario: {NomeVet}, Id: {Id}, TraceId: {TraceId}", created.Nome, created.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Remove um veterinario pelo identificador unico.
    /// </summary>
    /// <param name="id">Identificador unico do veterinario.</param>
    /// <response code="204">Veterinario removido com sucesso.</response>
    /// <response code="404">Veterinario nao encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return veterinarioService.Delete(id) ? NoContent() : NotFound();
    }
}
