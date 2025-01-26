using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

public class FishScript : MonoBehaviour
{
    public GameObject bubbleControler;
    private BubbleScript bubbleScript;

    private bool sueloSlowFalling = true;

    private bool slowFalling = true;

    private int numMaxSaltos = 2;
    private int currentSaltos = 0;

    private GameObject prefabBomba;
    private GameObject prefabDobleSaltoAnimacion;

    public Rigidbody2D body;
    public float ChargeRate = 15.0f;
    public float MaxCharge = 15.0f;
    private float Charge = 0;
    
    public AudioSource[] jumps;
    public AudioSource splat;
    
    private bool isPoisoned = false;
    private float poisonedTimer = 0.0f;

    public int maxTimeWater = 20;
    public bool underWater = false;
    private float water = 0.0f;
    public GameObject oxygenController;
    private OxygenScript oxygenScript;

    public BoxCollider2D PoisonCollider2D;

    // Controles por default
    public KeyCode keyIzquierda = KeyCode.A;
    public KeyCode KeyDerecha = KeyCode.D;
    public KeyCode KeyPlanear = KeyCode.W;
    public KeyCode KeyCheckPoint = KeyCode.R;
    public KeyCode KeyBomba = KeyCode.S;
    public KeyCode KeyGancho = KeyCode.LeftShift;
    public KeyCode KeySalto = KeyCode.Space;
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

    private bool splatPlayed = false;
    void Start()
    {
        prefabBomba = Resources.Load<GameObject>("Prefabs/Game/BombPowerUp");
        prefabDobleSaltoAnimacion = Resources.Load<GameObject>("Prefabs/Game/SaltoDoble-Sheet_0");

        oxygenScript = oxygenController.GetComponent<OxygenScript>();
        bubbleScript = bubbleControler.GetComponent<BubbleScript>();

        water = maxTimeWater;
        oxygenScript.SetMaxOxygen(maxTimeWater);

        body.gravityScale = 5.0f;
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
        if (isStuned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer < 0.0f)
            {
                isStuned = false;
            }
        }
    }

    void Update()
    {
        isMovingHorizontally = body.linearVelocity.magnitude > 0.1f;
        if (isStuned)
        {
            return;
        }
        movimiento.Set(movimiento.x, 1.7f);
        bool izquierda = Input.GetKey(keyIzquierda);
        bool derecha = Input.GetKey(KeyDerecha);

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 newPosition = transform.position;
        bool needsWrapping = false;
        

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

            if (Mathf.Abs(currentYRotation) != 90 && Mathf.Abs(currentYRotation) != 270)
            {
                transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
            }
        }

        if (body.IsTouching(ContactFilter))
        {
            sueloSlowFalling = true;
            if (!splatPlayed)
            {
                splat.Play();
                splatPlayed = true;
            }

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

            float hHeight = Camera.main.orthographicSize;
            float hWidth = hHeight * Camera.main.aspect;
            if (this.transform.position.x < Camera.main.transform.position.x - hWidth - 10)
            {
                this.transform.position = Camera.main.transform.position + new Vector3(0, hWidth, 0);
            }

            // Charge and jump logic
            if (Input.GetKey(KeySalto))
            {
                if (isPoisoned)
                    Charge = Mathf.Min(Charge + ChargeRate * Time.deltaTime, MaxCharge / 2);
                else
                    Charge = Mathf.Min(Charge + ChargeRate * Time.deltaTime, MaxCharge);

            }
            if (Input.GetKeyUp(KeySalto))
            {
                
                body.AddForce(movimiento * Charge, ForceMode2D.Impulse);
                currentSaltos = 1;
                Charge = 0f;
                JumpEffect();
            }
        }
        else if (currentSaltos == 1 && currentSaltos < numMaxSaltos && Input.GetKeyDown(KeySalto) && bubbleScript.HasEnoughBubbles(1))
        {

            body.AddForce(movimiento * MaxCharge / 2, ForceMode2D.Impulse);
            currentSaltos = 0;
            JumpEffect();
            bubbleScript.RemoveBubbles(1);
            StartCoroutine(AnimacionSalto());

        }
        else if (slowFalling && bubbleScript.HasEnoughBubbles(1))
        {
            if (Input.GetKey(KeyPlanear))
            {
                if (sueloSlowFalling)
                {
                    bubbleScript.RemoveBubbles(1);
                    sueloSlowFalling = false;
                }
                body.linearVelocityY = -3f;
                if (Input.GetKeyDown(KeyDerecha))
                    body.linearVelocityX = 5.0f;
                if (Input.GetKeyDown(keyIzquierda))
                    body.linearVelocityX = -5.0f;
            }
        }
        else
        {
            splatPlayed = false;
        }
        
        if (Input.GetKeyDown(KeyBomba) && bubbleScript.HasEnoughBubbles(1))
        {
            bubbleScript.RemoveBubbles(1);
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
            SetPoison();
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
    public void SetStunned()
    {
        isStuned = true;
        stunTimer = 2.0f;
    }

    public void SetPoison()
    {
        isPoisoned = true;
        poisonedTimer = 5f;
    }

    public void SetWater()
    {
        underWater = false;
        water = maxTimeWater;
        SetNormalControls();
    }

    public void SetNumSalots(int newnumSaltos)
    {
        numMaxSaltos = newnumSaltos;
    }
    public void RefillBubbles(int bubbles)
    {
        // Sumar burbujas
        underWater = true;
        SetNormalControls();
        bubbleScript.AddBubbles(bubbles);
    }

    public void LoseBubbles(int bubbles)
    {
        // Restar burbujas
        bubbleScript.RemoveBubbles(bubbles);
    }

    private void JumpEffect()
    {
        // Make it play a jump sound effect here
        // Play random jump sound
        if (jumps != null && jumps.Length > 0)
        {
            // Get random index from jumps array
            int randomIndex = UnityEngine.Random.Range(0, jumps.Length);

            // Make sure the selected AudioSource exists
            if (jumps[randomIndex] != null)
            {
                // Stop the current sound if it's playing (optional)
                jumps[randomIndex].Stop();
    
                // Play the random jump sound
                jumps[randomIndex].Play();
            }
        }
    }


    IEnumerator AnimacionSalto()
    {
        Vector3 posicion = body.transform.position;
        Quaternion rotation = Quaternion.identity;

        // Instanciar la bomba
        GameObject instanceWithTransform = Instantiate(prefabDobleSaltoAnimacion, posicion, rotation);
        Debug.Log("ªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªª");

        // Esperar 0.4 segundos
        yield return new WaitForSeconds(0.4f);

        // Destruir el objeto de la bomba
        Destroy(instanceWithTransform);
    }

    void AnimacionSalto2()
    {
        Vector3 posicion = body.transform.position;
        posicion.y += 1;
        Quaternion rotation = Quaternion.identity;

        // Instanciar la bomba
        GameObject instanceWithTransform = Instantiate(prefabDobleSaltoAnimacion, posicion, rotation);
        Debug.Log("Saltando");

        // Esperar 0.4 segundos
        Thread.Sleep(400);

        // Destruir el objeto de la bomba
        Destroy(instanceWithTransform);
    }

}