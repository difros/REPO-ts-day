using GQ.Compiler.exception;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace GQ.Compiler
{
    public class CompilerCSharpNetCore : CompilerCSharp
    {
        public CompilerResults compilerResults { get; private set; } = null;

        public CompilerCSharpNetCore()
        {
            Reference = new List<string>();
        }

        public override void GenerateCode(CompilerCSharp code)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();

            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            cp.IncludeDebugInformation = true;

            foreach (string s in code.Reference)
            {
                if (s != "")
                    cp.ReferencedAssemblies.Add(s);
            }

            switch (code.SourceType)
            {
                case SourceTypeEnum.File:
                    {
                        ((CompilerCSharpNetCore)code).compilerResults = provider.CompileAssemblyFromFile(cp, code.Source);
                        break;
                    }
                case SourceTypeEnum.Text:
                    {
                        ((CompilerCSharpNetCore)code).compilerResults = provider.CompileAssemblyFromSource(cp, code.Source);
                        break;
                    }
            }

            if (((CompilerCSharpNetCore)code).compilerResults.Errors.HasErrors == true)
            {
                var ex = new ExceptionCompiler();
                ex.CompilerResults = ((CompilerCSharpNetCore)code).compilerResults;
                throw ex;
            }

            code.CompiledAssembly = ((CompilerCSharpNetCore)code).compilerResults.CompiledAssembly;
        }
    }
}
