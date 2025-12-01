using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NyanTypeReference.Editor.Utilities
{
    public static class AssemblyFilterUtility
    {
        public static IEnumerable<Assembly> GetRelevantAssemblies(Type baseType)
        {
            if (baseType == null)
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                    if (!asm.IsDynamic) yield return asm;
                yield break;
            }

            var baseAsm = baseType.Assembly;
            yield return baseAsm;

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm == baseAsm) continue;
                if (asm.IsDynamic) continue;

                var refs = asm.GetReferencedAssemblies();
                if (refs.Any(r => r.FullName == baseAsm.FullName)) yield return asm;
            }
        }
    }
}