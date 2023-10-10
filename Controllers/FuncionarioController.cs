using ProvaDiogo.Models;
using Microsoft.AspNetCore.Mvc;
using ProvaDiogo.Data;

namespace ProvaDiogo.Controllers;

[Route("api/funcionario")]
[ApiController]
public class FuncionarioController : ControllerBase
{
    private readonly AppDbContext _context;

    public FuncionarioController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("cadastrar")]
    public IActionResult CadastrarFuncionario([FromBody] Funcionario funcionario)
    {
        if (funcionario == null)
        {
            return BadRequest("Dados do funcionário inválidos.");
        }

        _context.Funcionarios.Add(funcionario);
        _context.SaveChanges();

        return CreatedAtAction(nameof(CadastrarFuncionario), new { id = funcionario.Id }, funcionario);
    }

   [HttpGet("listar")]
    public IActionResult ListarFuncionarios()
    {
    var funcionarios = _context.Funcionarios.ToList();
    return Ok(funcionarios);
    }

}

