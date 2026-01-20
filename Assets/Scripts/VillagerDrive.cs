using UnityEngine;
using System.Collections;

// Clase que permite controlar o movemento dun aldeán na escena 3D usando as teclas de dirección.
// O aldeán pode avanzar/retroceder e rotar segundo a entrada do usuario.
// Inclúe parámetros para controlar a velocidade de desprazamento e de rotación.


public class VillagerDrive : MonoBehaviour
{
    // speed: velocidade de desprazamento.
    // rotationSpeed: velocidade de rotación.
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    // Update chámase unha vez por cada fotograma.
    // Recolle a entrada do usuario (teclas de dirección) para avanzar/retroceder e rotar.
    // Aplica a velocidade e o tempo para un movemento suave.
    // Move e rota o aldeán segundo a entrada recibida.
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        // Move o aldeán cara adiante/atrás e rótao segundo a entrada do usuario.
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
    }
}