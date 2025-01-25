using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class FishScript : MonoBehaviour
{
    public Rigidbody2D body;
    public Collider2D Collider;
    public float ChargeRate = 15.0f;
    public float MaxCharge = 15.0f;
    private float Charge = 0;

    private bool isPoisoned = false;
    private float poisonedTimer = 0.0f;

    public float maxTimeWater = 20.0f;
    public bool underWater = true;
    private float water = 0.0f;

    private bool isOnPlatform = true;

    //Controles por default
    public KeyCode keyIzquierda = KeyCode.A;
    public KeyCode KeyDerecha = KeyCode.D;
    public KeyCode KeyPlanear = KeyCode.W;
    public KeyCode KeyCheckPoint = KeyCode.R;
    public KeyCode KeyBomba = KeyCode.S;
    public KeyCode KeyGancho = KeyCode.LeftShift;

    private float stunTimer = 0.0f;
    public bool isStuned = false;

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

    }
    void Update()
    {
        if (isPoisoned)
        {
            poisonedTimer -= Time.deltaTime;
            if (poisonedTimer < 0.0f)
            {
                isPoisoned = false;
            }
        }
        if (!underWater)
        {
            water -= Time.deltaTime;
            if (water < 0.0f)
            {
                SetInverseControls();
            }
        }
        //Tengo que arreglar la relacion entre altura y anchura para que salte mas que vaya de lados
        //pero que deje hacer la animaci�n de lado a lado
        isMovingHorizontally = body.linearVelocity.magnitude > 0.1f;
        if (isStuned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer < 0.0f)
            {
                isStuned = false;
            }
            return;
        }
        movimiento.Set(movimiento.x, 1.7f);
        bool izquierda = Input.GetKey(keyIzquierda);
        bool derecha = Input.GetKey(KeyDerecha);

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 newPosition = transform.position;
        bool needsWrapping = false;

        if (isOnPlatform)
        {
            if (derecha && !izquierda)
            {
                movimiento.x = 1.0f;
            }
            else if (izquierda && !derecha)
            {
                movimiento.x = -1.0f;
            }
            else if (derecha && izquierda)
            {
                movimiento.x = 0.0f;
            }
            else movimiento.x = 0.0f;
            float hHeight = Camera.main.orthographicSize;
            float hWidth = hHeight * Camera.main.aspect;
            if (this.transform.position.x < Camera.main.transform.position.x - hWidth - 10)
            {
                this.transform.position = Camera.main.transform.position + new Vector3(0, hWidth, 0);
            }

            // Charge and jump logic
            if (Input.GetKey(KeyCode.Space))
            {
                if (isPoisoned)
                Charge = Mathf.Min(Charge + ChargeRate * Time.deltaTime, MaxCharge/2);
                else
                Charge = Mathf.Min(Charge + ChargeRate * Time.deltaTime, MaxCharge);

            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                body.AddForce(movimiento * Charge, ForceMode2D.Impulse);
                Charge = 0f;
            }
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
        if (viewportPosition.x < 0 - (objectWidth / Camera.main.orthographicSize))
        {
            newPosition.x = Camera.main.ViewportToWorldPoint(new Vector3(1, viewportPosition.y, viewportPosition.z)).x;
            needsWrapping = true;
        }
        else if (viewportPosition.x > 1 + (objectWidth / Camera.main.orthographicSize))
        {
            newPosition.x = Camera.main.ViewportToWorldPoint(new Vector3(0, viewportPosition.y, viewportPosition.z)).x;
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

    public void SetInverseControls()
    {
        KeyCode auxiliar = keyIzquierda;
        keyIzquierda = KeyDerecha;
        KeyDerecha = auxiliar;
    }

    public void SetNormalControls()
    {
        KeyCode auxiliar = keyIzquierda;
        keyIzquierda = KeyDerecha;
        KeyDerecha = auxiliar;
    }
    public void setStunned()
    {
        isStuned = true;
        stunTimer = 2.0f;
    }

    public void setPosion()
    {
        isPoisoned = true;
        poisonedTimer = 5f;
    }

    public void SetWater()
    {
        underWater = false;
        water = maxTimeWater;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Comprueba si el jugador está por encima de la plataforma
            if (transform.position.y > collision.transform.position.y)
            {
                isOnPlatform = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false;
        }
    }

}