using ProvaDiogo.Models;
using Microsoft.AspNetCore.Mvc;
using ProvaDiogo.Data;

namespace ProvaDiogo.Controllers;
[Route("api/folha")]
[ApiController]
public class FolhaPagamentoController : ControllerBase
{
    private readonly AppDbContext _context;

    public FolhaPagamentoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("cadastrar")]
    public IActionResult CadastrarFolhaPagamento([FromBody] FolhaPagamento folhaPagamento)
    {
        if (folhaPagamento == null)
        {
            return BadRequest("Dados da folha de pagamento inválidos.");
        }

        var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Id == folhaPagamento.FuncionarioId);

        if (funcionario == null)
        {
            return NotFound("Funcionário não encontrado.");
        }

        // Realize os cálculos necessários antes de salvar a folha de pagamento
        folhaPagamento.Valor = folhaPagamento.Quantidade * funcionario.ValorHora; // Cálculo do salário bruto

        // Cálculo do desconto de INSS
        decimal descontoInss = 0;
        if (folhaPagamento.Valor <= 1693.72M)
        {
            descontoInss = folhaPagamento.Valor * 0.08M;
        }
        else if (folhaPagamento.Valor <= 2822.90M)
        {
            descontoInss = folhaPagamento.Valor * 0.09M;
        }
        else if (folhaPagamento.Valor <= 5645.80M)
        {
            descontoInss = folhaPagamento.Valor * 0.11M;
        }
        else
        {
            descontoInss = 5645.80M * 0.11M;
        }

        // Cálculo do desconto de IR
        decimal descontoIr = 0;
        if (folhaPagamento.Valor <= 1903.98M)
        {
            descontoIr = 0;
        }
        else if (folhaPagamento.Valor <= 2826.65M)
        {
            descontoIr = (folhaPagamento.Valor * 0.075M) - 142.80M;
        }
        else if (folhaPagamento.Valor <= 3751.05M)
        {
            descontoIr = (folhaPagamento.Valor * 0.15M) - 354.80M;
        }
        else if (folhaPagamento.Valor <= 4664.68M)
        {
            descontoIr = (folhaPagamento.Valor * 0.225M) - 636.13M;
        }
        else
        {
            descontoIr = (folhaPagamento.Valor * 0.275M) - 869.36M;
        }

        // Cálculo do salário líquido
        folhaPagamento.SalarioLiquido = folhaPagamento.Valor - descontoInss - descontoIr;

        _context.FolhasPagamento.Add(folhaPagamento);
        _context.SaveChanges();

        return CreatedAtAction(nameof(CadastrarFolhaPagamento), new { id = folhaPagamento.Id }, folhaPagamento);
    }

    [HttpPost("listar")]
    public IActionResult ListarFolhasPagamento()
    {
        var folhasPagamento = _context.FolhasPagamento.ToList();

        if (folhasPagamento.Count == 0)
        {
            return NotFound("Nenhuma folha de pagamento encontrada.");
        }

        return Ok(folhasPagamento);
    }

    [HttpPost("buscar/{cpf}/{mes}/{ano}")]
public IActionResult BuscarFolhaPagamento(string cpf, int mes, int ano)
{
    var funcionario = _context.Funcionarios.FirstOrDefault(f => f.CPF == cpf);
    if (funcionario == null)
    {
        return NotFound("Funcionário não encontrado.");
    }

    var folhaPagamento = _context.FolhasPagamento.FirstOrDefault(fp =>
        fp.FuncionarioId == funcionario.Id && fp.Mes == mes && fp.Ano == ano);

    if (folhaPagamento == null)
    {
        return NotFound("Folha de pagamento não encontrada.");
    }

    // Monta o JSON de resposta com base no esquema fornecido
    var resposta = new
    {
        valor = folhaPagamento.Valor,
        quantidade = folhaPagamento.Quantidade,
        mes = folhaPagamento.Mes,
        ano = folhaPagamento.Ano,
        funcionarioId = folhaPagamento.FuncionarioId
    };

    return Ok(resposta);
}
}
