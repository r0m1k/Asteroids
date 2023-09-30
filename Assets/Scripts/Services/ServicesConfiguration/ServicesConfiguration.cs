using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    [CreateAssetMenu(fileName = "Services Configuration", menuName = "Asteroids Infrastructure/Services Configuration")]
    public class ServicesConfiguration : ScriptableObject
    {
        public List<ServiceTypeContainer> Services = new List<ServiceTypeContainer>();
    }
}