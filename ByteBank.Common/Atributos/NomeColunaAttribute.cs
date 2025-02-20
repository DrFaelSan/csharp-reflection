﻿namespace ByteBank.Common.Atributos;

[AttributeUsage(AttributeTargets.Property)]
public class NomeColunaAttribute : Attribute
{
    public string Header { get; }

    public NomeColunaAttribute(string header)
        =>  Header = header;
}
