using UnityEngine;

public class Heals : MonoBehaviour
{
    public int HealsCount;

    void Update()
    {
        
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Block>().IsInfected)
            HealsCount--;
    }
}
