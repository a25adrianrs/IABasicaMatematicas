using UnityEngine;
using UnityEngine.AI;


// Obliga a que el objeto tenga un NavMeshAgent
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NPCController : MonoBehaviour
{

    // Estados posibles del NPC
    enum State { Patrol, Evade };

    // Estado inicial del NPC
    State currentState = State.Patrol;

    [Header("Patrol Settings")]
    // Distancia máxima a la que puede patrullar
    public float PatrolDistance = 10.0f;

    // Tiempo que espera antes de elegir otro destino
    public float patrolWait = 5.0f;

    // Temporizador para contar cuánto tiempo lleva esperando
    float patrolTimePassed = 0;

    [Header("Evade Settings")]
    public Transform threat; // Amenaza de la que los npc deben escapar (player)

    // Tiempo usado para predecir dónde estará el jugador
    public float predictionTime = 0.5f;

    // Distancia que intentará recorrer al huir
    public float fleeDistance = 10f;

    // Temporizador para no recalcular la huida cada frame
    public float evadeTimer;

    // Componente NavMeshAgent del NPC
    private NavMeshAgent agent;

    // RigidBody de la amenaza (player)
    private Rigidbody threatRB;


    // Posición inicial del NPC
    Vector3 startPosition;

    void Start()
    {
        // Hace que el NPC empiece patrullando inmediatamente
        patrolTimePassed = patrolWait;
        // Guardamos referencia al NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        // Guardamos Rigidbody del jugador
        threatRB = threat.GetComponent<Rigidbody>();

        // Guardamos posición inicial para patrullar alrededor
        startPosition = transform.position;

    }


    void Update()
    {
        // Guardamos estado anterior
        State tempState = currentState;

        //-------------------------------------------------
        // COMPROBAR SI HAY QUE CAMBIAR DE ESTADO
        //-------------------------------------------------
        float distance = Vector3.Distance(transform.position, threat.position);


        // Si está patrullando y jugador cerca -> huir
        if (currentState == State.Patrol && distance < 10f)
        {
            currentState = State.Evade;
        }
        // Si está huyendo y jugador lejos -> volver a patrullar
        else if (currentState == State.Evade && distance > 15f)
        {
            currentState = State.Patrol;
        }

        //-------------------------------------------------
        // SI EL ESTADO CAMBIÓ
        //-------------------------------------------------
        if (tempState != currentState)
        {
            Debug.Log("Cambio de estado: " + tempState + " -> " + currentState);

            // Reinicia temporizador de huida
            if (currentState == State.Evade)
            {
                evadeTimer = 0;
            }
            // Hace que patrulle enseguida
            if (currentState == State.Patrol)
            {
                patrolTimePassed = patrolWait;
            }

        }
        //-------------------------------------------------
        // EJECUTAR COMPORTAMIENTO SEGÚN ESTADO
        //-------------------------------------------------
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Evade:
                Evade();
                break;
        }



        //-----------------------------------------
        // COMPORTAMIENTO: PATRULLAR
        //-----------------------------------------

        void Patrol()
        {
            // Suma tiempo
            patrolTimePassed += Time.deltaTime;

            // Cuando pasa suficiente tiempo
            if (patrolTimePassed > patrolWait)
            {
                patrolTimePassed = 0;
                // Punto inicial
                Vector3 patrollingPoint = startPosition;

                // Añade posición aleatoria
                patrollingPoint += new Vector3(Random.Range(-PatrolDistance, PatrolDistance), 0, Random.Range(-PatrolDistance, PatrolDistance));

                // Mover NPC hacia punto
                agent.SetDestination(patrollingPoint);
            }
        }


        //-----------------------------------------
        // COMPORTAMIENTO: HUIR
        //-----------------------------------------

        void Evade()
        {
            // Contar tiempo
            evadeTimer += Time.deltaTime;

            // Obtener velocidad actual del jugador
            Vector3 threatVelocity = threatRB.linearVelocity;

            // Recalcular huida cada 0.5 segundos
            if (evadeTimer > 0.5f)
            {
                evadeTimer = 0;

                // Predecir dónde estará el jugador
                Vector3 posicionFutura = threat.position + threatVelocity * predictionTime;

                // Dirección opuesta al jugador
                Vector3 fleeDirection = (transform.position - posicionFutura).normalized;

                // Punto al que escapar
                Vector3 targetPosition = transform.position + fleeDirection * fleeDistance;

                // Mandar NPC a ese punto
                agent.SetDestination(targetPosition);
            }

        }
    }
}
