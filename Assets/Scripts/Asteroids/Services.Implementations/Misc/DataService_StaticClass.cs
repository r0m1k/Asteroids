using Infrastructure;

namespace Asteroids.Services
{
    public class DataService_StaticClass : DataService
    {
        public DataService_StaticClass(DataServiceParameters parameters)
            : base(new StaticTypeList<DataService.DataContainer>(), parameters) { }
    }
}