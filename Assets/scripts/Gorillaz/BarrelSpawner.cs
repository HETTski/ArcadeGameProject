using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [Header("Ustawienia Generatora")]
    public GameObject barrelPrefab;
    public float spawnInterval = 3f; // Czas między kolejnymi beczkami

    private void Start()
    {
        // InvokeRepeating cyklicznie wywołuje podaną funkcję
        InvokeRepeating(nameof(SpawnBarrel), 1f, spawnInterval);
    }

    private void SpawnBarrel()
    {
        if (barrelPrefab != null)
        {
            Instantiate(barrelPrefab, transform.position, Quaternion.identity);
        }
    }
}