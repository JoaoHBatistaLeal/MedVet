using MedVet.Application.DTOs;
using MedVet.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedVet.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de proprietarios de animais.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class DonoController(IDonoService donoService, ILogger<DonoController> logger) : ControllerBase
{
    /// <summary>
    /// Lista todos os proprietarios cadastrados.
    /// </summary>
    /// <response code="200">Lista de proprietarios retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DonoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(donoService.GetAll());
    }

    /// <summary>
    /// Obtem um proprietario pelo identificador unico.
    /// </summary>
    /// <param name="id">Identificador unico do proprietario.</param>
    /// <response code="200">Proprietario localizado com sucesso.</response>
    /// <response code="404">Proprietario nao encontrado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DonoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var dono = donoService.GetById(id);
        if (dono is null)
            return NotFound();

        return Ok(dono);
    }

    /// <summary>
    /// Cadastra um novo proprietario.
    /// </summary>
    /// <param name="request">Dados para criacao do proprietario.</param>
    /// <response code="201">Proprietario cadastrado com sucesso.</response>
    /// <response code="400">Dados invalidos para criacao.</response>
    [HttpPost]
    [ProducesResponseType(typeof(DonoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] DonoRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;
        logger.LogInformation("Iniciando criacao de proprietario: {NomeDono}, TraceId: {TraceId}", request.Nome, traceId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = donoService.Create(request);
        logger.LogInformation("Finalizando criacao de proprietario: {NomeDono}, DonoId: {DonoId}, TraceId: {TraceId}", created.Nome, created.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Remove um proprietario pelo identificador unico.
    /// </summary>
    /// <param name="id">Identificador unico do proprietario.</param>
    /// <response code="204">Proprietario removido com sucesso.</response>
    /// <response code="404">Proprietario nao encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return donoService.Delete(id) ? NoContent() : NotFound();
    }
}
