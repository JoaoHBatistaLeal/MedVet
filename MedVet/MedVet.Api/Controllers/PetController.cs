using MedVet.Application.DTOs;
using MedVet.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedVet.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de animais de estimacao.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class PetController(IPetService petService, ILogger<PetController> logger) : ControllerBase
{
    /// <summary>
    /// Lista todos os animais cadastrados.
    /// </summary>
    /// <response code="200">Lista de animais retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(petService.GetAll());
    }

    /// <summary>
    /// Obtem um animal pelo identificador unico.
    /// </summary>
    /// <param name="id">Identificador unico do animal.</param>
    /// <response code="200">Animal localizado com sucesso.</response>
    /// <response code="404">Animal nao encontrado.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var pet = petService.GetById(id);
        if (pet is null)
            return NotFound();

        return Ok(pet);
    }

    /// <summary>
    /// Cadastra um novo animal vinculado a um proprietario.
    /// </summary>
    /// <param name="request">Dados do animal e identificador do proprietario.</param>
    /// <response code="201">Animal cadastrado com sucesso.</response>
    /// <response code="400">Dados invalidos ou proprietario nao encontrado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] PetRequest request)
    {
        var traceId = HttpContext.TraceIdentifier;
        logger.LogInformation("Iniciando criacao de pet: {NomePet}, DonoId: {DonoId}, TraceId: {TraceId}", request.Nome, request.IdDono, traceId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = petService.Create(request);
        logger.LogInformation("Finalizando criacao de pet: {NomePet}, PetId: {PetId}, TraceId: {TraceId}", created.Nome, created.Id, traceId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Remove um animal pelo identificador unico.
    /// </summary>
    /// <param name="id">Identificador unico do animal.</param>
    /// <response code="204">Animal removido com sucesso.</response>
    /// <response code="404">Animal nao encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return petService.Delete(id) ? NoContent() : NotFound();
    }
}
