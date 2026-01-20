
using UnityEngine;

public class PhantomMove : MonoBehaviour
{

    // goal: obxecto destino ao que se move o porco.
    // direction: vector de dirección cara ao destino.
    // speed: velocidade de desprazamento.

    // El GameObject goal ,hace referencia al objeto "Patient" o "Luigi" en esta escena
    public GameObject goal;

    // Obtenemos gameObject del aldeano 
    // public GameObject luigi;



    Vector3 direction;

    // Vectores3 donde almacenaremos la orientación de la vista de los fantasmas y luigi
    Vector3 phantom_view;
    Vector3 luigi_view;
    public float speed = 5f;

    // LateUpdate chámase despois de Update en cada fotograma.
    // Calcula a dirección cara ao obxecto goal e orienta o porco cara a el.
    // Se a distancia ao destino é maior que 2:
    //   - Desprázase cara ao destino segundo a velocidade indicada.
    private void LateUpdate()
    {
        // Obtenemos  la dirección a la que "miran" los fantasmas
        phantom_view = transform.forward;
        Debug.Log("Dirección a la que miran los fantasmas " + phantom_view);
        // Obtenemos la dirección a la que luigi "mira"
        luigi_view = goal.transform.forward;
        Debug.Log("Dirección a la que mira luigi" + luigi_view);

        // Obtengo el Vector3 de Luigi hasta los Fantasmas
        Vector3 toPhantom = (transform.position - goal.transform.position).normalized;

        // Calculamos el producto punto entre la dirección de la vista de Luigi y la dirección hacia los fantasmas
        float dot = Vector3.Dot(luigi_view, toPhantom);

        //  Calculamos el umbral de detección 
        // Si el fantasma esta en el radio de visión frontal de luigi, interrumpimos la funcion
        // El umbral es 0.9f , es decir, un ángulo de visión de aproximadamente 25 grados
        if (dot > 0.9f)
        {
            Debug.Log("Luigi mira fijamente a los fantasmas , estos se han congelado");
            return;
        }

        // Si no, los fantasmas se moverán hacia Luigi
        direction = goal.transform.position - this.transform.position;
        // Orientamos los fantasmas hacia Luigi
        this.transform.LookAt(goal.transform.position);

        // Si la distancia entre los fantasmas y Luigi es mayor que 2 unidades, los fantasmas se moverán hacia él
        if (direction.magnitude > 2)
        {

            Vector3 velocity = direction.normalized * speed * Time.deltaTime;
            this.transform.position = this.transform.position + velocity;
        }
    }


}
