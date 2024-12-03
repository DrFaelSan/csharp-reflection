using ByteBank.Common;
using ByteBank.Common.Interfaces;
using Newtonsoft.Json;

namespace ConsultoriaAbacaxi.Plugins;
public class RelatorioJson : IRelatorio<Boleto>
{
    private const string PastaDestino = @"C:\Plugins";

    public RelatorioJson() {}

    public void Processar(List<Boleto> boletos)
    {
        var boletosPorCedenteList = PegaBoletosAgrupados(boletos);

        GravarArquivo(boletosPorCedenteList);
    }

    private void GravarArquivo(List<BoletosPorCedente> grupos)
    {
        string nomeArquivoSaida = Path.Combine(PastaDestino, $"{typeof(BoletosPorCedente).Name}.json");
        File.WriteAllText(nomeArquivoSaida, JsonConvert.SerializeObject(grupos));
        Console.WriteLine($"Arquivo '{nomeArquivoSaida}' criado com sucesso!");
    }
    private List<BoletosPorCedente> PegaBoletosAgrupados(List<Boleto> boletos)
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
        List<BoletosPorCedente> boletosPorCedenteList = new List<BoletosPorCedente>();

        // Iterar sobre os grupos de boletos por cedente
        foreach (var grupo in boletosAgrupados)
        {
            // Criar instância de BoletosPorCedente
            BoletosPorCedente boletosPorCedente = new BoletosPorCedente
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
