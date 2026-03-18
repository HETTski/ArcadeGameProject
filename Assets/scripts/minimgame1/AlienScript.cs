using UnityEngine;
using UnityEngine.SceneManagement;

public class AlienScript : MonoBehaviour
{
    public float alienSpeed = 4f;

    
    private string hubSceneName = "SampleScene";

    void Update()
    {
        
        float x = Mathf.PingPong(Time.time * alienSpeed, 8f) - 4f;
        transform.position = new Vector3(x, 3f, 0);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Laser"))
        {
            Debug.Log("TRAFIONY! Fizyka dzia³a!");

            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.tickets += 20;
            }

            
            SceneManager.LoadScene(hubSceneName);
        }
    }
}