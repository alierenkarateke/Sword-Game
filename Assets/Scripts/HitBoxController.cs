using System.Collections;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    #region Settings

    [Header("Hit Box Values")]
    [SerializeField] private float damage;
    [SerializeField] private float hitStopDuration;
    [SerializeField] private float hitStopScale;

    #endregion Settings

    #region UnityLifecycle

    void OnTriggerEnter(Collider other)
    {
        if (IsValidTarget(other))
        {
            HandleHit(other);
        }
    }

    #endregion UnityLifecycle

    #region Methods

    IEnumerator DoHitStop(float duration)
    {
        if (GetMatchOver()) yield break;
        HitStopStart();
        yield return new WaitForSecondsRealtime(duration);
        HitStopEnd();
    }

    private void HandleHit(Collider other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (stats != null && !playerController.isDodging)
        {
            StartCoroutine(DoHitStop(hitStopDuration));
            StartCoroutine(CameraShake.Instance.Shake(0.1f, 0.05f));
            stats.TakeDamage(damage);
        }
    }

    private bool IsValidTarget(Collider other)
    {
        return !other.CompareTag(gameObject.tag) && (other.CompareTag("Player1") || other.CompareTag("Player2"));
    }

    private static bool GetMatchOver()
    {
        return GameManager.Instance.matchOver;
    }

    private static void HitStopEnd()
    {
        if (!GameManager.Instance.matchOver)
            Time.timeScale = 1.0f;
    }

    private void HitStopStart()
    {
        Time.timeScale = hitStopScale;
    }

    #endregion Methods
}
