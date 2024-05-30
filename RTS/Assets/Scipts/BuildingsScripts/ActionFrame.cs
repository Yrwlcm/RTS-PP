using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scipts
{
    public class ActionFrame : MonoBehaviour
    {
        public static ActionFrame instance = null;

        [SerializeField] private Button actionButton = null;
        [SerializeField] private Transform layoutGroup = null;
        [SerializeField] private GameObject actionsFrame = null;

        private List<Button> buttons = new List<Button>();
        private PlayerActions actionsList = null;

        public List<float> spawnQueue = new List<float>(); 
        public List<GameObject> spawnOrder = new List<GameObject> ();
        public GameObject spawnPoint = null;

        private void Awake()
        {
            instance = this;
            actionsFrame.SetActive(false);
        }

        public void SetActionButtons(PlayerActions actions, GameObject spawnLocation)
        {
            actionsList = actions;
            spawnPoint = spawnLocation;

            actionsFrame.SetActive(true);

            if (actions.basicUnits.Count > 0)
            {
                foreach(GameObject unit in actions.basicUnits)
                {
                    Button btn = Instantiate(actionButton, layoutGroup);
                    btn.name = unit.name;
                    buttons.Add(btn);
                }
            }

            if (actions.basicBuildings.Count > 0)
            {
                foreach (BasicBuilding building in actions.basicBuildings)
                {
                    Button btn = Instantiate(actionButton, layoutGroup);
                    btn.name = building.name;
                    buttons.Add(btn);
                }
            }
        }

        public void ClearActions()
        {
            actionsFrame.SetActive(false);

            foreach(Button btn in buttons)
            {
                Destroy(btn.gameObject);
            }
            buttons.Clear();
        }

        public void StartSpawnTimer(string objectToSpawn)
        {
            if (IsUnit(objectToSpawn))
            {
                GameObject unit = IsUnit(objectToSpawn);
                spawnQueue.Add(unit.GetComponent<Unit>().spawnTime);
                spawnOrder.Add(unit);
            }
            else if (IsBuilding(objectToSpawn))
            {
                BasicBuilding building = IsBuilding(objectToSpawn);
                spawnQueue.Add(building.buildTime);
                spawnOrder.Add(building.buildingPrefab);
            }
            else
            {
                Debug.Log($"{objectToSpawn} is not a spawnable object");
            }

            if (spawnQueue.Count == 1)
            {
                ActionTimer.instance.StartCoroutine(ActionTimer.instance.SpawnQueueTimer());
            }
            else if (spawnQueue.Count == 0)
            {
                ActionTimer.instance.StopAllCoroutines();
            }
        }

        private GameObject IsUnit(string name)
        {
            if (actionsList.basicUnits.Count > 0)
            {
                foreach (GameObject unit in actionsList.basicUnits)
                {
                    if (unit.name == name)
                    {
                        return unit;
                    }
                }
            }
            return null;
        }

        private BasicBuilding IsBuilding(string name)
        {
            if (actionsList.basicBuildings.Count > 0)
            {
                foreach(BasicBuilding building in actionsList.basicBuildings)
                {
                    if (building.name == name)
                    {
                        return building;
                    }
                }
            }

            return null;
        }

        public void SpawnObject()
        {

            GameObject spawnedObject = Instantiate(spawnOrder[0], new Vector3(spawnPoint.transform.position.x,
                spawnPoint.transform.position.y + 1, spawnPoint.transform.position.z), Quaternion.identity);
        }
    }
}

