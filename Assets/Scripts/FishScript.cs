using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class FishScript : MonoBehaviour
{
    public GameObject prefabBomba;

    public Rigidbody2D body;
    public float ChargeRate = 15.0f;
    public float MaxCharge = 15.0f;
    private float Charge = 0;

    private bool isPoisoned = false;
    private float poisonedTimer = 0.0f;

    public int maxTimeWater = 20;
    public bool underWater = false;
    private float water = 0.0f;
    public GameObject oxygenController;
    private OxygenScript oxygenScript;

    public BoxCollider2D PoisonCollider2D;

    private bool isOnPlatform = true;

    // Controles por default
    public KeyCode keyIzquierda = KeyCode.A;
    public KeyCode KeyDerecha = KeyCode.D;
    public KeyCode KeyPlanear = KeyCode.W;
    public KeyCode KeyCheckPoint = KeyCode.R;
    public KeyCode KeyBomba = KeyCode.S;
    public KeyCode KeyGancho = KeyCode.LeftShift;
    public bool isReversed = false;

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
    public ContactFilter2D ContactFilter;

    private float animationTimer = 0f;
    private bool isMovingHorizontally = false;

    private float objectWidth;

    private float currentYRotation = 0f;
    private float rotationProgress = 0f;
    private bool isRotating = false;
    private float startRotation;
    private float targetRotation;

    void Start()
    {
        oxygenScript = oxygenController.GetComponent<OxygenScript>();
        water = maxTimeWater;
        oxygenScript.SetMaxOxygen(maxTimeWater);
    }

    private void FixedUpdate() {
        if (isPoisoned) {
            poisonedTimer -= Time.deltaTime;
            if (poisonedTimer < 0.0f) {
                isPoisoned = false;
            }
        }
        underWater = false;
        if (!underWater) {
            water = Mathf.Max(water - Time.deltaTime, 0.0f);
            oxygenScript.SetOxygen(water);
            if (water <= 0.0f && !isReversed) {
                SetInverseControls();
            }
        }
    }

    void Update()
    {
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

        if (body.IsTouching(ContactFilter))
        {
            if (derecha && !izquierda)
            {
                movimiento.x = 1.0f;
                if (currentYRotation > 90f && !isRotating) // If facing left-ish and not already rotating
                {
                    StartRotation(180f, 0f);
                }
            }
            else if (izquierda && !derecha)
            {
                movimiento.x = -1.0f;
                if (currentYRotation < 90f && !isRotating) // If facing right-ish and not already rotating
                {
                    StartRotation(0f, 180f);
                }
            }
            else if (derecha && izquierda)
            {
                movimiento.x = 0.0f;
            }
            else movimiento.x = 0.0f;

            if (isRotating)
            {
                rotationProgress += Time.deltaTime * 3f; // Adjust speed by changing 3f
                if (rotationProgress >= 1f)
                {
                    rotationProgress = 1f;
                    isRotating = false;
                    currentYRotation = targetRotation;
                }
                else
                {
                    // Smooth easing curve
                    float t = EaseInOutCubic(rotationProgress);
                    currentYRotation = Mathf.Lerp(startRotation, targetRotation, t);
                }
            }

            transform.rotation = Quaternion.Euler(0, currentYRotation, 0);

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
        
        if (Input.GetKeyDown(KeyBomba))
        {
            Vector3 posicion = body.transform.position;
            Quaternion rotation = Quaternion.identity;
            GameObject instanceWithTransform = Instantiate(prefabBomba, posicion, rotation);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PoisonSpill"))  // Make sure to set this tag on your poison prefab
        {
            Debug.Log("ESTOY ENVENENADO");
            setPosion();
        }
    }
    
    private void StartRotation(float start, float target)
    {
        startRotation = start;
        targetRotation = target;
        rotationProgress = 0f;
        isRotating = true;
    }

    private float EaseInOutCubic(float t)
    {
        return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
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
        isReversed = true;
    }

    public void SetNormalControls()
    {
        KeyCode auxiliar = keyIzquierda;
        keyIzquierda = KeyDerecha;
        KeyDerecha = auxiliar;
        isReversed = false;
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

}