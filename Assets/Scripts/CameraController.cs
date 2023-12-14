using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements;
using TMPro;

public class CameraController : MonoBehaviour
{
    public Transform transformPlayer;
    public Vector2 margin = new (1, 1);
    public Vector2 smoothed = new (3, 3);
    public Collider limitsForeground;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (transformPlayer == null)
        {
            transformPlayer = GameObject.FindGameObjectWithTag("Rodolfo").transform;

            if (transformPlayer == null)
            {
                Debug.LogError("No hay Player");
            }
        }

        if (limitsForeground == null)
        {
            Debug.LogError("No hay Foreground");
        }
    }

    void LateUpdate()
    {
        if (transformPlayer == null || limitsForeground == null)
            return;

        float clampedX = Mathf.Clamp(transformPlayer.position.x, limitsForeground.bounds.min.x + margin.x, limitsForeground.bounds.max.x - margin.x);
        float clampedY = Mathf.Clamp(transformPlayer.position.y, limitsForeground.bounds.min.y + margin.y, limitsForeground.bounds.max.y - margin.y);

        // Calcula la posición objetivo de la cámara centrada en el área del jugador
        Vector3 targetPosition = new (clampedX, clampedY, transform.position.z);

        // Calcula el desplazamiento adicional de la cámara en la dirección del jugador
        Vector3 playerViewportPosition = Camera.main.WorldToViewportPoint(transformPlayer.position);
        Vector3 additionalOffset = Vector3.zero;

        if (playerViewportPosition.x < margin.x)
            additionalOffset.x = -margin.x;
        else if (playerViewportPosition.x > 1 - margin.x)
            additionalOffset.x = margin.x;

        if (playerViewportPosition.y < margin.y)
            additionalOffset.y = -margin.y;
        else if (playerViewportPosition.y > 1 - margin.y)
            additionalOffset.y = margin.y;

        targetPosition += additionalOffset;

        // Aplica suavizado al movimiento de la cámara
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothed.x, Mathf.Infinity, Time.deltaTime);

        // Calcula los límites del Foreground
        Vector2 minBound = limitsForeground.bounds.min;
        Vector2 maxBound = limitsForeground.bounds.max;

        // Limita la cámara dentro de los límites del Foreground
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBound.x + margin.x, maxBound.x - margin.x),
            Mathf.Clamp(transform.position.y, minBound.y + margin.y, maxBound.y - margin.y),
            transform.position.z);
    }
}

