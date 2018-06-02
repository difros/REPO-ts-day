using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;

namespace GQ.Compiler
{
    public class CompilerCSharpRoslyn : CompilerCSharp
    {
        public EmitResult emitResult { get; private set; } = null;

        public CompilerCSharpRoslyn()
        {
            Reference = new List<string>();
        }

        public override void GenerateCode(CompilerCSharp code)
        {
            List<MetadataReference> referencias = new List<MetadataReference>();

            var assemblys = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var item in assemblys)
            {
                try
                {
                    referencias.Add(MetadataReference.CreateFromFile(item.Location));
                }
                catch
                {

                }
            }

            foreach (string s in code.Reference)
            {
                if (s != "")
                    referencias.Add(MetadataReference.CreateFromFile(s));
            }

            var assemblyName = Guid.NewGuid().ToString();

            List<SyntaxTree> tree = new List<SyntaxTree>();
            switch (code.SourceType)
            {
                case SourceTypeEnum.File:
                    {

                        break;
                    }
                case SourceTypeEnum.Text:
                    {
                        tree.Add(CSharpSyntaxTree.ParseText(code.Source));
                        break;
                    }
            }

            var cc = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: tree,
                references: referencias,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
              );

            using (MemoryStream ms = new MemoryStream())
            {
                ((CompilerCSharpRoslyn)code).emitResult = cc.Emit(ms);
                ms.Position = 0;
                code.CompiledAssembly = DllLoader.LoadDll(ms).assembly;
            }
        }
    }
}
