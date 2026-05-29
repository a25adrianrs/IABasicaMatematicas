using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float rotationSpeed = 100;

    Rigidbody rb;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        rb.MovePosition(rb.position + transform.forward * move);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotation, 0));
    }


}
