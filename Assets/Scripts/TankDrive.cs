using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Clase que permite controlar o movemento dun tanque na escena 3D usando teclas de dirección e modo autopiloto.
// O tanque pode avanzar, rotar e activar o autopiloto para seguir un obxecto de combustible.
// Inclúe métodos para calcular ángulos, distancias e controlar o movemento automático cara ao obxecto fuel.

public class TankDrive : MonoBehaviour
{
    // speed: velocidade de desprazamento manual.
    // rotationSpeed: velocidade de rotación manual.
    // fuel: obxecto de combustible ao que pode seguir o tanque.
    // autopilot: activa/desactiva o modo automático.
    // tspeed: velocidade de desprazamento en autopiloto.
    // rspeed: velocidade de rotación en autopiloto.
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public GameObject fuel;
    bool autopilot = false;
    float tspeed = 2f;
    float rspeed = 0.2f;

    // AutoPilot move o tanque automaticamente cara ao obxecto fuel.
    // Calcula o ángulo e avanza na dirección actual.
    void AutoPilot()
    {
        CalculateAngle();
        this.transform.position += this.transform.up * tspeed * Time.deltaTime;
    }

    // CalculateAngle calcula o ángulo entre o tanque e o obxecto fuel e rota o tanque cara a el se é necesario.
    void CalculateAngle()
    {
        // Vector3 tankForward: vector que indica a dirección cara adiante do tanque.
        Vector3 tankForward = transform.up;
        // Vector3 fuelDirection: vector que indica a dirección cara ao obxecto fuel desde o tanque.
        Vector3 fuelDirection = fuel.transform.position - transform.position;

        // Debuxar os vectores na escena para visualización.
        Debug.DrawRay(this.transform.position, tankForward * 10, Color.green, 5);
        Debug.DrawRay(this.transform.position, fuelDirection, Color.red, 5);

        // Calcular o ángulo entre os dous vectores usando o produto escalar.
        float dot = tankForward.x * fuelDirection.x + tankForward.y * fuelDirection.y;
        // Mathf.Acos devolve o ángulo en radianes.
        float angle = Mathf.Acos(dot / (tankForward.magnitude * fuelDirection.magnitude));

        Debug.Log("Angle: " + angle * Mathf.Rad2Deg);
        Debug.Log("Unity Angle: " + Vector3.Angle(tankForward, fuelDirection));

        // Determinar a dirección de rotación usando o produto vectorial.
        int clockwise = 1;
        // Se o compoñente z do produto vectorial é negativo, xira en sentido horario.
        if (Cross(tankForward, fuelDirection).z < 0)
            clockwise = -1;
        // Rotar o tanque se o ángulo é maior que 10 graos.
        if ((angle * Mathf.Rad2Deg) > 10)
            this.transform.Rotate(0, 0, angle * Mathf.Rad2Deg * clockwise * rspeed);
    }

    // Cross calcula o produto vectorial entre dous vectores.
    Vector3 Cross(Vector3 v, Vector3 w)
    {
        float xMult = v.y * w.z - v.z * w.y;
        float yMult = v.x * w.z - v.z * w.x;
        float zMult = v.x * w.y - v.y * w.x;
        return (new Vector3(xMult, yMult, zMult));
    }

    // CalculateDistance calcula a distancia entre o tanque e o obxecto fuel e amosa información na consola.
    float CalculateDistance()
    {
        // Cálculo da distancia euclidiana en 2D (x,z).
        float distance = Mathf.Sqrt(Mathf.Pow(fuel.transform.position.x - transform.position.x, 2) +
                                    Mathf.Pow(fuel.transform.position.z - transform.position.z, 2));
        // Cálculo da distancia usando Vector3.Distance en 2D (x,z).
        Vector3 fuelPos = new Vector3(fuel.transform.position.x, 0, fuel.transform.position.z);
        Vector3 tankPos = new Vector3(transform.position.x, 0, transform.position.z);

        float uDistance = Vector3.Distance(fuelPos, tankPos);
        // Cálculo da distancia usando magnitude e sqrMagnitude.
        Vector3 tankToFuel = fuelPos - tankPos;

        Debug.Log("Distance: " + distance);
        Debug.Log("U Distance: " + uDistance);
        Debug.Log("V Magnitude: " + tankToFuel.magnitude);
        Debug.Log("V SqMagnitude: " + tankToFuel.sqrMagnitude);

        return distance;
    }

    // LateUpdate chámase despois de Update en cada fotograma.
    // Controla o movemento manual do tanque segundo a entrada do usuario.
    // Permite activar o modo autopiloto (tecla T) e calcula distancia/ángulo (tecla Espazo).
    // Se o tanque está preto do fuel, desactiva o autopiloto.
    // Se está activo, move o tanque automaticamente cara ao fuel.
    void LateUpdate()
    {
        // Movemento manual do tanque.
        float translation = Input.GetAxis("Vertical") * speed;
        // Rotación manual do tanque.
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Aplicar movemento e rotación ao tanque.
        transform.Translate(0, translation, 0);
        transform.Rotate(0, 0, -rotation);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CalculateDistance();
            CalculateAngle();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            autopilot = !autopilot;
        }

        if (CalculateDistance() < 3)
            autopilot = false;

        if (autopilot)
            AutoPilot();
    }
}