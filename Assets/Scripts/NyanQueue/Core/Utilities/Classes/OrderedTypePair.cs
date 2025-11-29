using System;
using System.Collections.Generic;
using UnityEngine;

namespace NyanQueue.Core.Utilities.Classes
{
    [Serializable]
    public class OrderedTypePair : IEquatable<OrderedTypePair>, IEqualityComparer<OrderedTypePair>
    {
        [SerializeField] private Type _firstType;
        [SerializeField] private Type _secondType;
        
        public Type First => _firstType;
        public Type Second => _secondType;

        public OrderedTypePair(Type firstType, Type secondType)
        {
            _firstType = firstType;
            _secondType = secondType;
        }
        
        public bool Equals(OrderedTypePair other)
        {
            if (other is null) return false;
            return _firstType == other._firstType && _secondType == other._secondType;
        }

        public override bool Equals(object obj) => Equals(obj as OrderedTypePair);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + _firstType.GetHashCode();
                hash = hash * 31 + _secondType.GetHashCode();
                return hash;
            }
        }

        public bool Equals(OrderedTypePair x, OrderedTypePair y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return x._firstType == y._firstType && x._secondType == y._secondType;
        }

        public int GetHashCode(OrderedTypePair obj) => obj.GetHashCode();
    }
}