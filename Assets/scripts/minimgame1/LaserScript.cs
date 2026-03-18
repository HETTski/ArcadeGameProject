using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserScript : MonoBehaviour
{
    public float laserSpeed = 15f;
    private Rigidbody2D rb;
    private bool isFired = false;

    
    private string hubSceneName = "SampleScene";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && !isFired)
        {
            isFired = true;
            
            rb.linearVelocity = transform.up * laserSpeed;
        }

        
        if (transform.position.y > 6f)
        {
            Debug.Log("PUD�O! Tracisz kas�!");
            SceneManager.LoadScene(hubSceneName);
        }
    }
}