using Infrastructure;

namespace Asteroids.Services
{
    public class DataService_TypeDictionary : DataService
    {
        public DataService_TypeDictionary(DataServiceParameters parameters)
            : base(new TypeList<DataService.DataContainer>(), parameters) { }
    }
}