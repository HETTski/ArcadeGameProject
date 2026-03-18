using UnityEngine;
using UnityEngine.SceneManagement;

public class InvaderGame : MonoBehaviour
{
    public Transform alien;
    public Transform laser;

    public float alienSpeed = 4f;
    private bool isShooting = false;

   private string hubSceneName = "SampleScene";

    void Update()
    {

        float x = Mathf.PingPong(Time.time * alienSpeed, 8f) - 4f;
        alien.position = new Vector3(x, 3f, 0);


        if (Input.GetKeyDown(KeyCode.Space) && !isShooting)
        {
            isShooting = true;
        }

        if (isShooting)
        {

            laser.Translate(Vector3.up * 15f * Time.deltaTime);

           
            if (Vector3.Distance(laser.position, alien.position) < 1.0f)
            {
                Debug.Log("TRAFIONY! Wygrywasz 20 biletów!");
                GameManager.Instance.tickets += 20;
                SceneManager.LoadScene(hubSceneName); 
            }
            
            else if (laser.position.y > 6f)
            {
                Debug.Log("PUD£O! Tracisz kasê!");
                SceneManager.LoadScene(hubSceneName);
            }
        }
    }
}