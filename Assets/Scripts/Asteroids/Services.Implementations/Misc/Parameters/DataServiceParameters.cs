using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Services
{
    [CreateAssetMenu(fileName = "DataService Parameters", menuName = "Asteroids Infrastructure/DataService Parameters")]
    public class DataServiceParameters : ScriptableObject
    {
        public List<ScriptableObject> Scriptables;
    }
}