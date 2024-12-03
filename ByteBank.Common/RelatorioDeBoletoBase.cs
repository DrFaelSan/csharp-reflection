using ByteBank.Common.Atributos;
using ByteBank.Common.Interfaces;
using System.Reflection;

namespace ByteBank.Common;
public abstract class RelatorioDeBoletoBase : IRelatorio<Boleto>
{
    protected readonly string _nomeArquivoSaida;
    protected readonly DateTime _dataRelatorio = DateTime.Now;

    public RelatorioDeBoletoBase(string nomeArquivoSaida, DateTime dataRelatorio)
    {
        _nomeArquivoSaida = nomeArquivoSaida;
        _dataRelatorio = dataRelatorio;
    }

    public RelatorioDeBoletoBase(DateTime dataRelatorio)
        =>  _dataRelatorio = dataRelatorio;
    

    public RelatorioDeBoletoBase(string nomeArquivoSaida)
        =>  _nomeArquivoSaida = nomeArquivoSaida;

    public RelatorioDeBoletoBase()
    {
    }

    public void Processar(List<Boleto> boletos)
    {
        var atributoNomeRelatorio = this.GetType().GetCustomAttribute<NomeRelatorioAttribute>();

        var boletosPorCedente = PegaBoletosAgrupados(boletos);

        GravarArquivo(boletosPorCedente);
    }


    protected abstract void GravarArquivo(List<BoletosPorCedente> grupos);

    private List<BoletosPorCedente> PegaBoletosAgrupados(IList<Boleto> boletos)
    {
        // Agrupar boletos por cedente
        var boletosAgrupados = boletos.GroupBy(b => new
        {
            b.CedenteNome,
            b.CedenteCpfCnpj,
            b.CedenteAgencia,
            b.CedenteConta
        });

        // Lista para armazenar instâncias de BoletosPorCedente
        List<BoletosPorCedente> boletosPorCedenteList = new();

        // Iterar sobre os grupos de boletos por cedente
        foreach (var grupo in boletosAgrupados)
        {
            // Criar instância de BoletosPorCedente
            BoletosPorCedente boletosPorCedente = new()
            {
                CedenteNome = grupo.Key.CedenteNome,
                CedenteCpfCnpj = grupo.Key.CedenteCpfCnpj,
                CedenteAgencia = grupo.Key.CedenteAgencia,
                CedenteConta = grupo.Key.CedenteConta,
                Valor = grupo.Sum(b => b.Valor),
                Quantidade = grupo.Count()
            };

            // Adicionar à lista
            boletosPorCedenteList.Add(boletosPorCedente);
        }

        return boletosPorCedenteList;
    }


}
