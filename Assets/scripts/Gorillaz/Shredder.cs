using UnityEngine;

public class Shredder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiedy cokolwiek dotknie tego obiektu, niszczymy to, żeby zwolnić pamięć!
        Destroy(other.gameObject);
    }
}