using Unity.VisualScripting;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    public Rigidbody2D body;
    public Collider2D Collider;
    public float ChargeRate = 5.0f;
    public float MaxCharge = 20.0f;
    private float Charge = 0;

    public Camera camera;

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

    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        if (GetComponent<Renderer>() != null)
        {
            objectWidth = GetComponent<Renderer>().bounds.size.x / 2;
            objectHeight = GetComponent<Renderer>().bounds.size.y / 2;
        }
        else
        {
            objectWidth = 0.5f; // Default size if no renderer
            objectHeight = 0.5f;
        }
    }

    void Update()
    {
        //Tengo que arreglar la relacion entre altura y anchura para que salte mas que vaya de lados
        //pero que deje hacer la animaci�n de lado a lado
        movimiento.Set(movimiento.x, 1.7f);
        bool izquierda = Input.GetKey(keyIzquierda);
        bool derecha = Input.GetKey(KeyDerecha);

        Vector3 viewportPosition = camera.WorldToViewportPoint(transform.position);
        Vector3 newPosition = transform.position;
        bool needsWrapping = false;

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

        float hHeight = camera.orthographicSize;
        float hWidth = hHeight * camera.aspect;
        if (this.transform.position.x < camera.transform.position.x - hWidth - 10)
        {
            this.transform.position = camera.transform.position + new Vector3(0, hWidth, 0);
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

        // Check horizontal bounds
        if (viewportPosition.x < 0 - (objectWidth / camera.orthographicSize))
        {
            newPosition.x = camera.ViewportToWorldPoint(new Vector3(1, viewportPosition.y, viewportPosition.z)).x;
            needsWrapping = true;
        }
        else if (viewportPosition.x > 1 + (objectWidth / camera.orthographicSize))
        {
            newPosition.x = camera.ViewportToWorldPoint(new Vector3(0, viewportPosition.y, viewportPosition.z)).x;
            needsWrapping = true;
        }

        // Check vertical bounds
        if (viewportPosition.y < 0 - (objectHeight / camera.orthographicSize))
        {
            newPosition.y = camera.ViewportToWorldPoint(new Vector3(viewportPosition.x, 1, viewportPosition.z)).y;
            needsWrapping = true;
        }
        else if (viewportPosition.y > 1 + (objectHeight / camera.orthographicSize))
        {
            newPosition.y = camera.ViewportToWorldPoint(new Vector3(viewportPosition.x, 0, viewportPosition.z)).y;
            needsWrapping = true;
        }

        // Update position if wrapping is needed
        if (needsWrapping)
        {
            transform.position = newPosition;
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