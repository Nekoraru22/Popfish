using Unity.VisualScripting;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    public Rigidbody2D body;
    public Collider2D Collider;
    public float ChargeRate = 5.0f;
    public float MaxCharge = 20.0f;
    private float Charge = 0;

    [Header("Leg Sprites")]
    public SpriteRenderer leftLeg;
    public SpriteRenderer rightLeg;

    [Header("Animation Settings")]
    [Tooltip("Time in seconds for one complete step")]
    public float stepDuration = 0.5f;

    [Tooltip("Maximum rotation angle for legs")]
    public float maxRotationAngle = 15f;

    private float animationTimer = 0f;
    private Vector2 movimiento = Vector2.zero;
    private bool isMovingHorizontally = false;

    void Start()
    {

    }

    void Update()
    {
        movimiento.Set(0, 1.7f);
        isMovingHorizontally = body.linearVelocity.magnitude > 0.1f;

        if (Input.GetKey(KeyCode.A))
        {
            movimiento.x -= 0.5f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movimiento.x += 0.5f;
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