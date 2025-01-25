using Unity.VisualScripting;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    public Rigidbody2D body;
    public Collider2D Collider;
    public float ChargeRate = 5.0f;
    public float MaxCharge = 20.0f;
    private float Charge = 0;

    //Controles por default
    public KeyCode keyIzquierda = KeyCode.A;
    public KeyCode KeyDerecha = KeyCode.D;
    public KeyCode KeyPlanear = KeyCode.W;
    public KeyCode KeyCheckPoint = KeyCode.R;
    public KeyCode KeyBomba = KeyCode.S;
    public KeyCode KeyGancho = KeyCode.LeftShift;


    public int CurrentBubbles = 0;
    public int MaxBubbles = 0;

    private Vector2 movimiento = Vector2.zero;

    [Header("Leg Sprites")]
    public SpriteRenderer leftLeg;
    public SpriteRenderer rightLeg;

    [Header("Animation Settings")]
    [Tooltip("Time in seconds for one complete step")]
    public float stepDuration = 0.5f;

    [Tooltip("Maximum rotation angle for legs")]
    public float maxRotationAngle = 15f;

    private float animationTimer = 0f;
    private bool isMovingHorizontally = false;


    void Start()
    {

    }

    void Update()
    {
        //Tengo que arreglar la relacion entre altura y anchura para que salte mas que vaya de lados
        //pero que deje hacer la animaci�n de lado a lado
        movimiento.Set(movimiento.x, 1.7f);
        bool izquierda = Input.GetKey(keyIzquierda);
        bool derecha = Input.GetKey(KeyDerecha);

        isMovingHorizontally = body.linearVelocity.magnitude > 0.1f;

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

        // Charge and jump logic
        if (Input.GetKey(KeyCode.Space))
        {
            Charge = Mathf.Min(Charge + ChargeRate * Time.deltaTime, MaxCharge);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            body.AddForce(movimiento * Charge, ForceMode2D.Impulse);
            Charge = 0f;
        }

        // Legs animation
        if (isMovingHorizontally)
        {
            UpdateLegAnimation();
        }
        else
        {
            ResetLegs();
        }

    }

    void UpdateLegAnimation()
    {
        animationTimer += Time.deltaTime;
        if (animationTimer >= stepDuration)
        {
            animationTimer = 0f;
        }

        float progress = (animationTimer / stepDuration) * Mathf.PI * 2;

        float leftAngle = Mathf.Sin(progress) * maxRotationAngle;
        float rightAngle = Mathf.Sin(progress + Mathf.PI) * maxRotationAngle; // Opposite phase

        if (leftLeg != null)
        {
            leftLeg.transform.parent.localRotation = Quaternion.Euler(0, 0, leftAngle);
        }
        if (rightLeg != null)
        {
            rightLeg.transform.parent.localRotation = Quaternion.Euler(0, 0, rightAngle);
        }
    }

    void ResetLegs()
    {
        if (leftLeg != null)
        {
            leftLeg.transform.parent.localRotation = Quaternion.identity;
        }
        if (rightLeg != null)
        {
            rightLeg.transform.parent.localRotation = Quaternion.identity;
        }
        animationTimer = 0f;
    }

    void enableCollision()
    {
        Collider.enabled = true;
    }
}