using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocityPlayer;        // variable = velocidad del player para moverse
    public float jumpForcePlayer;       // variable = fuerza del salto
    public float jumpMaxPlayer;         // máxima = cantidad de saltos
    public LayerMask layerForeground;   // variable para la capa del Foreground = suelo

    private Rigidbody rigidBodyPlayer;        //variable Rigidbody del Player
    private BoxCollider boxColliderPlayer;    //variable Collider del player
    private float jumpNotMadePlayer;            //saltos restantes que no se hicieron

    private void Start()
    {
        // inicializa el Rigidbody2D, el BoxCollider del Player
        // inicializa los saltos no realizados por el Player = a la cantidad máxima de saltos asignados que puede realizar 
        rigidBodyPlayer = GetComponent<Rigidbody>();
        boxColliderPlayer = GetComponent<BoxCollider>();
        jumpNotMadePlayer = jumpMaxPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        // llama a los métodos que gestionan el movimiento y salto del Player
        ProcessMovement();
        ProcessJump();
    }

    bool OnTheForegronud()
    {
        // Calcula el Raycast del jugador desde el centro del boxColliderPlayer hacia abajo en Y para verificar si el jugador está en el suelo o en un objeto del layer especificado (layerForeground)
             
        bool isHit = Physics.BoxCast(boxColliderPlayer.bounds.center, new Vector3(boxColliderPlayer.bounds.size.x / 2, boxColliderPlayer.bounds.size.y / 2f, boxColliderPlayer.bounds.size.z / 2), Vector3.down, Quaternion.identity, 0.2f, layerForeground);
        // Retorna true cuando el Raycast golpea algo, indicando que el jugador está en el suelo, sino devuelve false
        return isHit;
    }

    void ProcessJump()
    {
        // si el Player está en el piso => la cantidad de saltos que no se hicieron = cantidad máxima de saltos asignados
        if (OnTheForegronud())
        {
            jumpNotMadePlayer = jumpMaxPlayer;
        }

        // si se hace clic en la tecla space y la cantida de saltos no realizados >0 => se resta 1 saltop a los no ralizados, se mueve el Rigibody de Player a la velocidad vertical en cero y aplica una fuerza hacia arriba
        if (Input.GetKeyDown(KeyCode.Space) && jumpNotMadePlayer > 0)
        {
           
            // Resta 1 salto a los no realizados
            jumpNotMadePlayer--;

            // Establece la velocidad vertical del Rigidbody del jugador en cero
            rigidBodyPlayer.velocity = new Vector3(rigidBodyPlayer.velocity.x, 0f, 0f);

            // Aplica una fuerza hacia arriba
            rigidBodyPlayer.AddForce(Vector3.up * jumpForcePlayer, ForceMode.Impulse);
        }
    
    }

    void ProcessMovement()
    {
        // obtiene la entrada del eje horizontal (Input.GetAxis("Horizontal")) para determinar la dirección del movimiento del Player 
        float inputMovimiento = Input.GetAxis("Horizontal");

        // establece la velocidad horizontal del rigidBodyPlayer según la entrada del jugador y la velocidad máxima (velocityPlayer)
        rigidBodyPlayer.velocity = new Vector3(inputMovimiento * velocityPlayer, rigidBodyPlayer.velocity.y, 0f);
    }


}
