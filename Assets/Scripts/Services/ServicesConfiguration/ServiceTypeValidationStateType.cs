namespace Services
{
    public enum ServiceTypeValidationStateType
    {
        UnknownType,
        Duplicated,
        KnownType,
        ConstructorCount,
        ConstructorRequireUnknownService,
        ConstructorRequireUnityObject,
        ConstructorRequireUnityObjectInstance,
        Valid,
    }
}