using System;
using UnityEngine;

namespace Infrastructure
{
    public class TypeOfAttribute : PropertyAttribute
    {
        public Type BaseType;

        public TypeOfAttribute(Type baseType)
        {
            BaseType = baseType;
        }
    }
}