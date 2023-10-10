
namespace ProvaDiogo.Models;

public class FolhaPagamento
{
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public int Quantidade { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public int FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }
    public decimal SalarioLiquido { get; internal set; }
}



