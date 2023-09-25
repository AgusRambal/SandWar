using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        Vector3.MoveTowards(transform.position, target.position, 10 * Time.deltaTime);
    }
}
