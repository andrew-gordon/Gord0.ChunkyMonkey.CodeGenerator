using System.ComponentModel;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Runtime.CompilerServices
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Workaround for the error: Predefined type 'System.Runtime.CompilerServices.IsExternalInit' is not defined or imported Gord0.ChunkyMonkey.CodeGenerator
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit { }
}