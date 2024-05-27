using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public enum RadiationLevels
{
    Low,
    Medium,
    High
}

//public enum RadiationStates
//{
//    Stay,
//    GoDown,
//    GoUp
//}


public class Block : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    public double infectScore;
    //public GameObject LowRadiationFloorPrefab;
    //public GameObject MediumRadiationFloorPrefab;
    //public GameObject HighRadiationFloorPrefab;
    public RadiationLevels RadiationLevel;
    //private bool isStateChanging = false;
    private GameObject cubeDisplay;
    private Camera mainCamera;
    private Color[] colors;


    void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
        infectScore = RadiationLevel == RadiationLevels.Low 
            ? 0
            : RadiationLevel == RadiationLevels.Medium ? 21: 41;
    }

    private void Start()
    {
        mainCamera = Camera.main;        
        colors = new Color[3] { new(1.0f, 0f, 0f, 0.1f), new(1.0f, 0.8f, 0f, 0.1f), new(0f, 1.0f, 0f, 0.1f) };
    }

    private void Update()
    {
        //if (RadiationLevel == RadiationLevels.High || RadiationLevel == RadiationLevels.Medium)
        //{
        //    var changeMode = ChangeRadiationScore(-0.0005);
        //    if (!isStateChanging) ChangeRadiationLevel(changeMode);
        //}

        if (Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt))
        {
            if (IsObjectVisibleInCamera())
            {
                if (cubeDisplay == null)
                    CreatePlaneDisplay();
                else
                {
                    UpdatePlaneColor();
                }
            }
        }
        else
        {
            if (cubeDisplay != null)
            {
                Destroy(cubeDisplay);
            }
        }
    }


    void OnCollisionStay(Collision collision)
    {
        var collisionObject = collision.gameObject;
        var block = collisionObject.GetComponent<Block>();
        if (block != null)
        {
            if (block.RadiationLevel == RadiationLevels.Medium || block.RadiationLevel == RadiationLevels.High)
            {
                var delta = block.RadiationLevel == RadiationLevels.High ? 0.1 : 0.05;
                var deltaMinus = -(delta - 0.000001);
                ChangeRadiationScore(delta);
                block.ChangeRadiationScore(deltaMinus);
            }

            //var changeMode = ChangeRadiationScore(delta);
            //if (!isStateChanging) ChangeRadiationLevel(changeMode);
        }
    }

    public void ChangeRadiationScore(double delta)
    {
        infectScore += delta;
        RadiationLevel = infectScore < 20 
            ? RadiationLevels.Low
            : infectScore > 40 ? RadiationLevels.High : RadiationLevels.Medium;
        //return RadiationLevel == RadiationLevels.Low && infectScore > 20
        //    || RadiationLevel == RadiationLevels.Medium && infectScore > 40
        //        ? RadiationStates.GoUp
        //        : RadiationLevel == RadiationLevels.High && infectScore < 40
        //            || RadiationLevel == RadiationLevels.Medium && infectScore < 20
        //                ? RadiationStates.GoDown
        //                : RadiationStates.Stay;
    }

    private void CreatePlaneDisplay()
    {
        cubeDisplay = GameObject.CreatePrimitive(PrimitiveType.Plane);
        cubeDisplay.transform.localScale = new Vector3(transform.localScale.x, 0.1f, transform.localScale.z);
        cubeDisplay.transform.position = transform.position + new Vector3(0, 0.1f, 0);
        cubeDisplay.transform.rotation = Quaternion.identity;
        cubeDisplay.transform.parent = transform;

        var meshCollider = cubeDisplay.GetComponent<MeshCollider>();
        meshCollider.convex = true;

        var color = RadiationLevel == RadiationLevels.Low
                    ? colors[2]
                    : RadiationLevel == RadiationLevels.High ? colors[0] : colors[1];

        var renderer = cubeDisplay.GetComponent<Renderer>();
        var material = new Material(Shader.Find("UI/Unlit/Transparent")) { color = color };
        renderer.material = material;

        //var plateRenderer = cubeDisplay.GetComponent<Renderer>();
        //var plateColor = infectScore < 20
        //        ? Color.green
        //        ? Color.green
        //        : infectScore > 40 ? Color.red : Color.yellow;
        //plateColor.a = 0.01f;
        //plateRenderer.material.color = plateColor;
    }

    private void UpdatePlaneColor()
    {
        var color = RadiationLevel == RadiationLevels.Low
            ? colors[2]
            : RadiationLevel == RadiationLevels.High ? colors[0] : colors[1];
        cubeDisplay.GetComponent<Renderer>().material.color = color;
    }

    private bool IsObjectVisibleInCamera()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        return screenPos.x >= 0 && screenPos.x <= Screen.width &&
            screenPos.y >= 0 && screenPos.y <= Screen.height &&
            screenPos.z > 0;
    }

    //private void ChangeRadiationLevel(RadiationStates radiationState)
    //{
    //    if (radiationState == RadiationStates.Stay) return;
    //    isStateChanging = true;
    //    var prefab = radiationState == RadiationStates.GoUp && RadiationLevel == RadiationLevels.Low
    //        ? MediumRadiationFloorPrefab
    //        : radiationState == RadiationStates.GoUp && RadiationLevel == RadiationLevels.Medium
    //            ? HighRadiationFloorPrefab
    //            : radiationState == RadiationStates.GoDown && RadiationLevel == RadiationLevels.High
    //                ? MediumRadiationFloorPrefab
    //                : LowRadiationFloorPrefab;

    //    Instantiate(prefab, transform.position, transform.rotation, transform.parent);
    //    Destroy(this.gameObject);
    //}
}
