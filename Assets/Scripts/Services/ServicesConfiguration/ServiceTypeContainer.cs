using System;
using System.Collections.Generic;

namespace Services
{
    [Serializable]
    public class ServiceTypeContainer
    {
        public string TypeName;
        public List<ServiceTypeConstructorParameterData> ConstructorParameters;

        public List<ImplementedInterfaceData> ImplementedInterfaces;

        public ServiceTypeValidationStateType ValidationState;
    }
}