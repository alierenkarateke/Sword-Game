using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;


    void Awake()
    {
        Instance = this;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orgPosition = transform.localPosition;

        float elapsed = 0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f,1f) * magnitude;
            float y = Random.Range(-1f,1f) * magnitude;

            transform.localPosition = new Vector3(orgPosition.x + x, orgPosition.y + y, orgPosition.z);

            elapsed += Time.deltaTime;
            yield return null;

        }

        transform.localPosition = orgPosition;
    }
}
