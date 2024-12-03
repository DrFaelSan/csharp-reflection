using ByteBank.Common.Atributos;
using ByteBank.Common.Interfaces;
using System.Reflection;

namespace ByteBank.Common;

[NomeRelatorio("Relatório de boletos agrupados por cedentes")]
public class RelatorioCSV : RelatorioDeBoletoBase
{
    protected readonly string nomeArquivoSaida = "BoletosPorCedente.csv";

    public RelatorioCSV(DateTime dataRelatorio) : base(dataRelatorio) {}

    public RelatorioCSV(string nomeArquivoSaida) : base(nomeArquivoSaida) {}

    public RelatorioCSV(string nomeArquivoSaida, DateTime dataRelatorio) : base(nomeArquivoSaida, dataRelatorio) {}

    public RelatorioCSV() {}

    protected override void GravarArquivo(List<BoletosPorCedente> grupos)
    {
        // Obter tipo da classe
        Type tipo = typeof(BoletosPorCedente);

        // Usar Reflection para obter propriedades
        PropertyInfo[] propriedades = tipo.GetProperties();

        // Escrever os dados no arquivo CSV
        using (var sw = new StreamWriter(nomeArquivoSaida))
        {
            // Escrever cabeçalho
            var cabecalho = propriedades
                .Select(p => p.GetCustomAttribute<NomeColunaAttribute>()?.Header
                ?? p.Name);

            sw.WriteLine(string.Join(',', cabecalho));

            // Escrever linhas do relatório
            foreach (var grupo in grupos)
            {
                var valores = propriedades.Select(p => p.GetValue(grupo));
                sw.WriteLine(string.Join(',', valores));
            }
        }

        Console.WriteLine($"Arquivo '{nomeArquivoSaida}' criado com sucesso!");
    }
}
