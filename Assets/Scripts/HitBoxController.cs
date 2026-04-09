using NUnit.Framework;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    [SerializeField] private string ownerTag;
    [SerializeField] private float damage;
    [SerializeField] public bool hasDamaged = false;

    void OnTriggerStay(Collider other)
    {
    Debug.Log("Trigger algiladi: " + other.tag + " / " + other.name);
    }
    /*
    void OnTriggerStay(Collider other)
    {
        if(!hasDamaged && other.tag != ownerTag && (other.tag == "Player1" || other.tag == "Player2"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            PlayerController playerController = other.GetComponent<PlayerController>();
            if(stats != null && !playerController.isDodging)
            {
                Debug.Log("Hit !!!");
                stats.TakeDamge(damage,stats.eTag);
                hasDamaged = true;
            }
            
        }
    }
    */
}
