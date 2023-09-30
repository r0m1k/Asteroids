using System;

namespace Services
{
    [Serializable]
    public class ServiceTypeConstructorParameterData
    {
        public ServiceTypeConstructorParameterType Type;
        public UnityEngine.Object Object;
        public string TypeAssemblyQualifiedName;
    }
}