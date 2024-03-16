using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptSizeToSpawner : MonoBehaviour
{
    public BoidSpawner spawner;

    public Vector3 position;
    public float height;

    private void OnDrawGizmos()
    {
        if (spawner == null)
        {
            return;
        }


        transform.position = position * -spawner.spawnRadius;
        transform.localScale = new Vector3(spawner.spawnRadius * 2, height, spawner.spawnRadius * 2);
    }
}
