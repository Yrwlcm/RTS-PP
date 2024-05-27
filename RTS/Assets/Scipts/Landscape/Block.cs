using Unity.AI.Navigation;
using UnityEngine;


public class Block : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    private int infectScore;
    public GameObject normalBlockPrefab;
    public GameObject infectedBlockPrefab;
    public int CollisionsCountToInfected;
    public bool IsInfected;
    

    void Awake()
    {
        // navMeshSurface = GetComponent<NavMeshSurface>();
        // navMeshSurface.BuildNavMesh();
        infectScore = 0;
    }

    void OnCollisionStay(Collision collision)
    {
        if (!IsInfected)
        {
            var collisionObject = collision.gameObject;
            var block = collisionObject.GetComponent<Block>();
            if (block != null && block.IsInfected)
            {
                var infectedMode = AddInfectionAndReturnInfectedMode();
                if (infectedMode)
                {
                    ChangeBlockToInfected();
                    block.ChangeBlockToNormal();
                }
            }
        }
    }


    public bool AddInfectionAndReturnInfectedMode()
    {
        infectScore++;
        IsInfected = infectScore >= CollisionsCountToInfected;
        return IsInfected;
    }

    private void ChangeBlockToInfected()
    {
        Instantiate(infectedBlockPrefab, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }

    private void ChangeBlockToNormal()
    {
        Instantiate(normalBlockPrefab, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
