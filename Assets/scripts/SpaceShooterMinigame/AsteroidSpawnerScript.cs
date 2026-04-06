using UnityEngine;

public class AsteroidSpawnerScript : MonoBehaviour
{

    public GameObject largeAsteroidPrefab;
    public int startingAsteroids = 4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < startingAsteroids; i++)
        {
            Vector2 spawnPos = new Vector2(Random.Range(-8f, 8f), Random.Range(-5f, 5f));
            while (Vector2.Distance(spawnPos, Vector2.zero) < 3f)
            {
                spawnPos = new Vector2(Random.Range(-8f, 8f), Random.Range(-5f, 5f));
            }

            Instantiate(largeAsteroidPrefab, spawnPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
