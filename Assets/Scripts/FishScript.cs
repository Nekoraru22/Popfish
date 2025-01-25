using Unity.VisualScripting;
using UnityEngine;

public class FishScript : MonoBehaviour
{

    public Rigidbody2D body;
    public Collider2D Collider;
    public float ChargeRate = 5.0f;
    public float MaxCharge = 20.0f;
    private float Charge = 0;
    public KeyCode keyIzquierda = KeyCode.A;
    public KeyCode KeyDerecha = KeyCode.D;

    private Vector2 movimiento = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Tengo que arreglar la relacion entre altura y anchura para que salte mas que vaya de lados
        //pero que deje hacer la animación de lado a lado
        movimiento.Set(movimiento.x, 1.7f);
        bool izquierda = Input.GetKey(keyIzquierda);
        bool derecha = Input.GetKey(KeyDerecha);

        if (derecha && !izquierda)
        {
            movimiento.x = 1.0f;
        }
        if (izquierda && !derecha)
        {
            movimiento.x = -1.0f;
        }
        if (derecha && izquierda)
        {
            movimiento.x = 0.0f;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Charge = Mathf.Min(Charge + ChargeRate * Time.deltaTime, MaxCharge);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            body.AddForce(movimiento * Charge, ForceMode2D.Impulse);
            Charge = 0f;
        }
    }

    void enableCollision()
    {
        Collider.enabled = true;
    }
}
