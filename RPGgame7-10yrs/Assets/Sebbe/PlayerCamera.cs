using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothSpeed = 0.125f;

    [Header("Zoom Settings")]
    public float minSize = 5f;      // Closest zoom
    public float maxSize = 10f;     // Furthest zoom
    public float zoomSpeed = 50f; 

    private Camera cam;

    IEnumerator Start()
    {
        cam = GetComponent<Camera>();

        yield return null; // wait 1 frame

        player1 = GameObject.Find("Player Spawnpoint").transform.GetChild(0);
        if (GameManagerScript.instance.playerCount == 2)
        {
            player2 = GameObject.Find("Player2 Spawnpoint").transform.GetChild(0);
        }
    }

    void LateUpdate()
    {
        if (player1 == null && player2 == null) return;

        // Calculate Position 
        Vector3 centerPoint = GetCenterPoint();
        transform.position = Vector3.Lerp(transform.position, centerPoint + offset, smoothSpeed);

        // Calculate Zoom
        if (player1 != null && player2 != null)
        {
            float newZoom = Mathf.Lerp(minSize, maxSize, GetGreatestDistance() / zoomSpeed);

            if (cam.orthographic)
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, smoothSpeed);
            else
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom * 10, smoothSpeed); 
        }
    }

    float GetGreatestDistance()
    {
        return Vector2.Distance(player1.position, player2.position);
    }

    Vector3 GetCenterPoint()
    {
        if (player1 != null && player2 != null) return (player1.position + player2.position) / 2f;
        if (player1 != null) return player1.position;
        return player2.position;
    }
}