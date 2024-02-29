using DominoAPI.GameObjects;
using DominoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DominoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DominoController : ControllerBase
{
    private readonly DominoService _dominoService;

    public DominoController(DominoService dominoService) =>
        _dominoService = dominoService;

    [HttpGet]
    public async Task<List<Domino>> Get() =>
        await _dominoService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Domino>> Get(string id)
    {
        var domino = await _dominoService.GetAsync(id);

        if (domino is null)
        {
            return NotFound();
        }

        return domino;
    }
}