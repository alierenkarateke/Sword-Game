using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    [SerializeField] private string ownerTag;
    [SerializeField] private float damage;
    [SerializeField] private float hitStopDuration;
    /*
    void OnTriggerEnter(Collider other)
    {
    Debug.Log("Trigger algiladi: " + other.tag + " / " + other.name);
    }
    */
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag != ownerTag && (other.tag == "Player1" || other.tag == "Player2"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            PlayerController playerController = other.GetComponent<PlayerController>();
            if(stats != null && !playerController.isDodging)
            {
                //Debug.Log("Hit !!!");
                StartCoroutine(doHitStop(hitStopDuration));
                StartCoroutine(CameraShake.Instance.Shake(0.1f, 0.05f));
                stats.TakeDamge(damage,stats.eTag);
            }
            
        }
    }

    IEnumerator doHitStop(float duration)
    {
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(duration);
        if (!GameManager.Instance.matchOver)
        {
            Time.timeScale = 1.0f;
        }  
    }
}
