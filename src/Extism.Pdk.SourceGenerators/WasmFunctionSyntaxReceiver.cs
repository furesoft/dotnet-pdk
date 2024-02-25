using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Extism.SourceGenerators;

internal class WasmFunctionSyntaxReceiver : ISyntaxContextReceiver
{
    public List<(MethodDeclarationSyntax method, AttributeSyntax attribute)> CandidateMethods { get; } = new();

    /// <summary>
    ///     Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for
    ///     generation
    /// </summary>
    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        var syntaxNode = context.Node;

        if (!(syntaxNode is MethodDeclarationSyntax method)) return;

        var attribute = method.AttributeLists
            .SelectMany(l => l.Attributes)
            .FirstOrDefault(a =>
                context.SemanticModel.GetTypeInfo(a).Type?.ToDisplayString()?.Contains("WasmFunction") == true);

        if (attribute is null) return;

        CandidateMethods.Add((method, attribute));
    }
}