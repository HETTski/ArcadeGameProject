using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    private Camera mainCamera;

    // Margines okreœla, jak daleko za ekran musi wylecieæ obiekt, ¿eby go przeteleportowaæ.
    // Dziêki temu obiekt nie znika nagle, gdy tylko dotknie krawêdzi.
    private float margin = 0.05f;

    void Start()
    {
        mainCamera = Camera.main; // Pobieramy g³ówn¹ kamerê na starcie
    }

    void Update()
    {
        // 1. Zamieniamy prawdziw¹ pozycjê 3D na pozycjê wzglêdem kamery (0 do 1)
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
        bool isWrapping = false;

        // 2. Sprawdzamy krawêdzie poziome (Prawo / Lewo)
        if (viewportPos.x > 1 + margin)
        {
            viewportPos.x = -margin;
            isWrapping = true;
        }
        else if (viewportPos.x < -margin)
        {
            viewportPos.x = 1 + margin;
            isWrapping = true;
        }

        // 3. Sprawdzamy krawêdzie pionowe (Góra / Dó³)
        if (viewportPos.y > 1 + margin)
        {
            viewportPos.y = -margin;
            isWrapping = true;
        }
        else if (viewportPos.y < -margin)
        {
            viewportPos.y = 1 + margin;
            isWrapping = true;
        }

        // 4. Jeœli wylecieliœmy, przypisujemy now¹ pozycjê
        if (isWrapping)
        {
            transform.position = mainCamera.ViewportToWorldPoint(viewportPos);
        }
    }
}