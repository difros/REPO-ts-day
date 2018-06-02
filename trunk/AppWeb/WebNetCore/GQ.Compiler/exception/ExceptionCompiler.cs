using System;
using System.CodeDom.Compiler;

namespace GQ.Compiler.exception
{
    public class ExceptionCompiler : Exception
    {
        public CompilerResults CompilerResults { get; internal set; }
    }
}