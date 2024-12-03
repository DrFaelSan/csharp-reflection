using Newtonsoft.Json;

namespace ByteBank.Common;
public class RelatorioJson : RelatorioDeBoletoBase
{
    protected readonly string PastaDestino = @"C:\Plugins";
    protected readonly string nomeArquivo = "BoletosPorCedente.json";

    public RelatorioJson() {}

    public RelatorioJson(DateTime dataRelatorio) : base(dataRelatorio) { }

    public RelatorioJson(string nomeArquivoSaida) : base(nomeArquivoSaida) { }

    public RelatorioJson(string nomeArquivoSaida, DateTime dataRelatorio) : base(nomeArquivoSaida, dataRelatorio) { }

    protected override void GravarArquivo(List<BoletosPorCedente> grupos)
    {
        string nomeArquivoSaida = Path.Combine(PastaDestino, nomeArquivo);
        File.WriteAllText(nomeArquivoSaida, JsonConvert.SerializeObject(grupos));
        Console.WriteLine($"Arquivo '{nomeArquivoSaida}' criado com sucesso!");
    }
}