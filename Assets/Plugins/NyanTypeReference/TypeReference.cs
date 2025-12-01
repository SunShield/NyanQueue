using System;
using UnityEngine;

namespace NyanTypeReference
{
    [Serializable]
    public sealed class TypeReference : ISerializationCallbackReceiver
    {
        [SerializeField] private string _typeName;
        [NonSerialized] private Type _type;

        public Type Type
        {
            get
            {
                if (_type == null && !string.IsNullOrEmpty(_typeName))
                    _type = System.Type.GetType(_typeName);

                return _type;
            }
            set
            {
                _type = value;
                _typeName = value != null ? value.AssemblyQualifiedName : null;
            }
        }

        public bool IsValid => Type != null;

        public string TypeName => Type != null ? Type.FullName : "(None)";

        public TypeReference() { }

        public TypeReference(Type t)
        {
            Type = t;
        }

        public static implicit operator Type(TypeReference r) => r?.Type;
        public static implicit operator TypeReference(Type t) => new TypeReference(t);

        public void OnBeforeSerialize()
        {
            if (_type != null)
                _typeName = _type.AssemblyQualifiedName;
        }

        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(_typeName))
                _type = System.Type.GetType(_typeName);
            else
                _type = null;
        }

        public override string ToString() => TypeName;
    }
}