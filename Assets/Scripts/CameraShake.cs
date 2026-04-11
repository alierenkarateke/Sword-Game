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

        if (GameManager.Instance.matchOver)    
        yield break;

        float elapsed = 0f;
        

        while(elapsed < duration)
        {
            if (GameManager.Instance.matchOver)
            {
                orgPosition = transform.localPosition; 
                yield break;
            }
            
            float x = Random.Range(-1f,1f) * magnitude;
            float y = Random.Range(-1f,1f) * magnitude;

            transform.localPosition = new Vector3(orgPosition.x + x, orgPosition.y + y, orgPosition.z);

            elapsed += Time.unscaledDeltaTime;
            yield return null;

        }

        transform.localPosition = orgPosition;
    }
}
