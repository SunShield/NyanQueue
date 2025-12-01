using System;
using UnityEngine;

namespace NyanTypeReference.Attribs
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class TypeOptionsAttribute : PropertyAttribute
    {
        public Type BaseType          { get; }
        public bool IncludeAbstract   { get; }
        public bool IncludeInterfaces { get; }
        public bool AllowNone         { get; }

        public TypeOptionsAttribute(Type baseType = null, bool includeAbstract = false, bool includeInterfaces = false,
            bool allowNone = true)
        {
            BaseType = baseType;
            IncludeAbstract = includeAbstract;
            IncludeInterfaces = includeInterfaces;
            AllowNone = allowNone;
        }
    }
}