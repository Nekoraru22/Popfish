using UnityEngine;
using System;

public class SneakScript : MonoBehaviour
{
    private GameObject FishScriptObj;
    private GameObject FishControllerObj;

    private float lenguaPosicion = 0f; // Posici�n actual de la lengua
    private float velocidadLengua = 0.02f; // Velocidad de movimiento
    private float limiteLengua = 2f; // L�mite m�ximo
    private bool direccionCreciente = true; // Direcci�n actual (true = creciendo, false = disminuyendo)
    private FishScript fishscript;

    Transform hijoTransform;
    GameObject lengua;
    private Vector3 lenguaPosicionInicial;
    private float indiceX,indiceY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FishScriptObj = GameObject.Find("MutantFish");
        Debug.Log(FishControllerObj);
        fishscript = FishScriptObj.GetComponent<FishScript>();
        Debug.Log(fishscript);
        Debug.Log(FishControllerObj);

        FishControllerObj = GameObject.Find("FishController");

        hijoTransform = transform.GetChild(0);
        lengua = hijoTransform.gameObject;
        lenguaPosicionInicial = lengua.transform.position;
        indiceX = (float)System.Math.Sin(transform.eulerAngles.z * System.Math.PI / 180);
        indiceY = (float)System.Math.Cos(transform.eulerAngles.z * System.Math.PI / 180);
        movimientoLengua();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos si la colisi�n es con el objeto que queremos (MutantFish)
        if (collision.gameObject.name == "MutantFish")
        {
            Debug.Log("Colisi�n detectada con MutantFish");

            // Llama a la funci�n `hit` cuando ocurre la colisi�n
            hit();
        }
    }

    void Update()
    {
        movimientoLengua();
    }

    private void hit()
    {
        float random = UnityEngine.Random.Range(-1f, 1f);
        Vector2 direction = new Vector2(random, 1.7f);
        fishscript.body.AddForce(direction * 3, ForceMode2D.Impulse);
        fishscript.setStunned();
    }

    

    private void movimientoLengua()
    {
        if (direccionCreciente)
        {
            // Incrementa la posici�n de la lengua
            lenguaPosicion += velocidadLengua;

            // Si alcanza el l�mite, cambia de direcci�n
            if (lenguaPosicion >= limiteLengua)
            {
                direccionCreciente = false;
            }
        }
        else
        {
            // Decrementa la posici�n de la lengua
            lenguaPosicion -= velocidadLengua;

            // Si llega a 0, cambia de direcci�n
            if (lenguaPosicion <= 0)
            {
                direccionCreciente = true;
            }
        }

        lengua.transform.position = new Vector3(lenguaPosicionInicial.x - lenguaPosicion * indiceX, lenguaPosicionInicial.y + lenguaPosicion*indiceY , lengua.transform.position.z);

        // Aqu� puedes usar `lenguaPosicion` para aplicar el movimiento a un objeto
        Debug.Log("Posici�n de la lengua: " + lenguaPosicion);
    }
}
