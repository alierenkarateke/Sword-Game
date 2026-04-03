using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    [SerializeField] private string ownerTag;
    [SerializeField] private float damage;


    void OnTriggerEnter(Collider other)
    {
        if(other.tag != ownerTag && other.tag == "Player")
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            PlayerController playerController = other.GetComponent<PlayerController>();
            if(stats != null && !playerController.isDodging)
            {
                Debug.Log("Hit !!!");
                stats.TakeDamge(damage);
            }
            
        }
    }
}
