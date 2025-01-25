using Unity.VisualScripting;
using UnityEngine;

public class FishScript : MonoBehaviour
{

    public Rigidbody2D body;
    public Collider2D Collider;
    public float ChargeRate = 5.0f;
    public float MaxCharge = 20.0f;
    private float Charge = 0;

    private Vector2 movimiento = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        movimiento.Set(0, 1.7f);
        if ( Input.GetKey(KeyCode.A))
        {
            movimiento.x -= 0.5f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movimiento.x += 0.5f;
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
