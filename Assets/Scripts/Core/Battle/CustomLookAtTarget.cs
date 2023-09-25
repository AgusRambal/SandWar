using System.Collections;
using UnityEngine;

public class CustomLookAtTarget : MonoBehaviour
{
    public float speed = 1f;

    public Coroutine LookCoroutine { get; private set; }

    public void StartRotating(Transform target)
    {
        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt(target));
    }

    private IEnumerator LookAt(Transform target)
    { 
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * speed;
            yield return null;
        }
    }
}
