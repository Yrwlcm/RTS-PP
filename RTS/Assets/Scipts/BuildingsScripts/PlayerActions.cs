using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scipts
{
    [CreateAssetMenu(fileName = "NewPlayerActions", menuName = "PlayerActions")]
    public class PlayerActions : ScriptableObject
    {
        [Header("Units")]
        public List<GameObject> basicUnits = new List<GameObject>();

        [Header("Buildings")]
        public List<BasicBuilding> basicBuildings = new List<BasicBuilding>();
    }
}

