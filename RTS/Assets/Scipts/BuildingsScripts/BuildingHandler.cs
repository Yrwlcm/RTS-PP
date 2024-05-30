using Scipts;
using UnityEngine;

namespace Scipts
{
    public class BuildingHandler : MonoBehaviour
    {
        public static BuildingHandler instance;

        [SerializeField]
        private BasicBuilding basicBuilding;

        private void Awake()
        {
            instance = this;
        }

        public BuildingStatTypes.Base GetBasicBuildingStats(string type)
        {
            BasicBuilding building;
            switch (type)
            {
                case "basicbuilding":
                    building = basicBuilding;
                    break;
                default:
                    Debug.Log("Type does not exist");
                    return null;
            }
            return building.baseStats;
        }
    }
}

