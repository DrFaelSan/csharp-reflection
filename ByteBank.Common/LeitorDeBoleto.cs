using System.Reflection;

namespace ByteBank.Common
{
    public class LeitorDeBoleto
    {
        public List<Boleto> LerBoletos(string caminhoArquivo)
        {
            try
            {
                // montar lista de boletos
                var boletos = new List<Boleto>();

                // ler arquivo de boletos
                using (var reader = new StreamReader(caminhoArquivo))
                {
                    string linha = reader.ReadLine();
                    // ler cabeçalho do arquivo CSV
                    string[] cabecalho = linha.Split(',');

                    // para cada linha do arquivo CSV
                    while (!reader.EndOfStream)
                    {
                        // ler dados
                        linha = reader.ReadLine();
                        string[] dados = linha.Split(',');

                        // carregar objeto Boleto
                        Boleto boleto = MapearTextoParaObjeto<Boleto>(cabecalho, dados);

                        // adicionar boleto à lista
                        boletos.Add(boleto);
                    }
                }

                // retornar lista de boletos
                return boletos;
            }
            catch
            {
                return new List<Boleto>();
            }
        }

        private static T MapearTextoParaObjeto<T>(string[] nomesPropriedades, string[] valoresPropriedades)
        {
            T instancia = Activator.CreateInstance<T>();

            for(int i = 0; i < nomesPropriedades.Length; i++)
            {
                string nomePropriedade = nomesPropriedades[i];

                //Obtém a propriedade atual através do nome.
                PropertyInfo propertyInfo = instancia.GetType().GetProperty(nomePropriedade);

                //Verifica se a propriedade foi encontrada
                if(propertyInfo is not null)
                {
                    //Obtém o tipo da propriedade
                    Type propertyType = propertyInfo.PropertyType;

                    //Obtém o valor da propriedade
                    string valor = valoresPropriedades[i];

                    //Converte o valor da propriedade para o tipo.
                    object valorConvertido = Convert.ChangeType(valor,propertyType);

                    //Guarda o valor convertido na propriedade.
                    propertyInfo.SetValue(instancia, valorConvertido);
                }
            }            
            
            return instancia;
        }

    }
}
