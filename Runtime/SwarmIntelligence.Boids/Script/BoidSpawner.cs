using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [Header("Boid Settings")]
    public int boidCount = 10;
    public List<GameObject> boidPrefabs = new List<GameObject>();

    [Header("Spawn Settings")]
    public float spawnRadius = 10.0f;

    private void Start()
    {
        foreach (GameObject boid in boidPrefabs)
        {
            if (boid.GetComponent<BaseBoid>() == null)
            {
                Debug.LogError("BoidSpawner: Boid prefab does not have a Boid component!");
                return;
            }
        }

        for (int i = 0; i < boidCount; i++)
        {
            Vector3 position = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));

            GameObject boid = Instantiate(boidPrefabs[Random.Range(0, boidPrefabs.Count)], position, Quaternion.identity);
            boid.GetComponent<BaseBoid>().Boundaries = Boundaries();
        }
    }

    private void OnDrawGizmos()
    {
        Vector3[] boundaries = Boundaries();

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((boundaries[0] + boundaries[1]) / 2, boundaries[1] - boundaries[0]);

    }

    public Vector3[] Boundaries()
    {
        Vector3[] boundaries = new Vector3[2];

        boundaries[0] = transform.position - new Vector3(spawnRadius, spawnRadius, spawnRadius);
        boundaries[1] = transform.position + new Vector3(spawnRadius, spawnRadius, spawnRadius);

        return boundaries;
    }
}
