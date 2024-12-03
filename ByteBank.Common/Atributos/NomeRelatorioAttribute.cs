namespace ByteBank.Common.Atributos;

[AttributeUsage(AttributeTargets.Class)]
public class NomeRelatorioAttribute : Attribute
{
    public string Nome { get; }

    public NomeRelatorioAttribute(string nome)
        =>  Nome = nome;
}