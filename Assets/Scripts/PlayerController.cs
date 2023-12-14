using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject objective;        // variable del tipo GameObject para el Objetive
    public float coverageThreshold;     // variable para controlar el encimamiento del Player con Objective
    public TextMeshProUGUI messageText; // variable que asignar el Message


    private void Update()
    {
        // verifica el encimamiento
        CheckObjectiveVisibility();
    }

    private void CheckObjectiveVisibility()
    {
        // verifica si el Objective no es nulo y si está parcialmente encimado al Player
        if (objective != null && IsAboveObjective(objective, coverageThreshold))
        {
            // muestra 
            Debug.Log("Player está sobre el Objective");
            
            // actualizar el texto en el TextMeshPro = Message
            messageText.text = "Nivel 1 completo";
        }
        else
        {
            // limpia la consola
            System.Console.Clear();
            
            // limpia el texto
            messageText.text = "";
        }
    }

    private bool IsAboveObjective(GameObject objective, float coverageThreshold)
    {

        // verifica si el Player está un 90% o más por encima del Objective en ambas direcciones
        Bounds playerBounds = GetComponent<Collider>().bounds;
        Bounds objectiveBounds = objective.GetComponent<Collider>().bounds;

        float verticalOverlap = Mathf.Min(playerBounds.max.y, objectiveBounds.max.y) - Mathf.Max(playerBounds.min.y, objectiveBounds.min.y);
        float horizontalOverlap = Mathf.Min(playerBounds.max.x, objectiveBounds.max.x) - Mathf.Max(playerBounds.min.x, objectiveBounds.min.x);

        float verticalCoverage = verticalOverlap / objectiveBounds.size.y;
        float horizontalCoverage = horizontalOverlap / objectiveBounds.size.x;

        //Debug.Log($"horizontalCoverage: {horizontalCoverage * 100}%");
        //Debug.Log($"Vertical Coverage: {verticalCoverage * 100}%");

        return verticalCoverage >= coverageThreshold && horizontalCoverage >= coverageThreshold;


    }
}
