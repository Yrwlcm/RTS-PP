using Scipts;
using UnityEngine;

namespace Scipts
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;

        public Transform playerBuildings;

        private void Awake()
        {
            instance = this;
            SetBasicStats(playerBuildings);
        }

        private void Update()
        {

        }

        public void SetBasicStats(Transform type)
        {
            foreach (Transform child in type)
            {
                foreach (Transform tf in child)
                {
                    string name = child.name.Substring(0, child.name.Length - 1).ToLower();
                    if (type == playerBuildings)
                    {
                        PlayerBuilding pB = tf.GetComponent<PlayerBuilding>();
                        pB.baseStats = BuildingHandler.instance.GetBasicBuildingStats(name);
                    }
                }
            }
        }
    }
}

