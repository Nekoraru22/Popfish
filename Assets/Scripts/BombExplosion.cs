using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public float ExplosionPower = 1;

    public int timer;
    private float elapsedTime;

    public GameObject FishScriptObj;
    public GameObject FishControllerObj;

    private FishScript fishscript;
    private FishControllerScript fishController;

    void Start()
    {
        timer = 2;
        elapsedTime = 0f;
        fishscript = FishScriptObj.GetComponent<FishScript>();
        fishController = FishControllerObj.GetComponent<FishControllerScript>();
    }

    private void FixedUpdate()
    {
        // Acumula el tiempo transcurrido
        elapsedTime += Time.fixedDeltaTime;

        // Si ha pasado un segundo, reduce el timer y reinicia el contador de tiempo
        if (elapsedTime >= 1f)
        {
            timer--;
            elapsedTime = 0f; // Reinicia el contador de tiempo

            // Aquí puedes agregar cualquier lógica adicional que desees ejecutar cada segundo
            Debug.Log("Timer: " + timer);

            // Si el timer llega a 0, puedes hacer algo (como explotar la bomba)
            if (timer <= 0)
            {
                Explode();
            }
        }
    }

    private void Explode() // FALTA HACER QUE TENGA ANIMACIÓN
    {
        // Lógica para manejar la explosión de la bomba
        Debug.Log("La bomba ha explotado");
        float distance = Vector2.Distance(transform.position, FishScriptObj.transform.position);

        Vector2 direction = (FishScriptObj.transform.position - transform.position).normalized;

        // Función exponencial
        //float forceMultiplier = Mathf.Clamp((1/Mathf.Pow(2, distance)), 0.1f, 1f); // Asegura que el multiplicador esté entre 0.1 y 1
        
        // Función lineal
        float forceMultiplier = Mathf.Clamp(1 / distance, 0.1f, 1f);

        fishscript.body.AddForce(direction * ExplosionPower * forceMultiplier, ForceMode2D.Impulse);
        Destroy(gameObject); // Destruye el objeto de la bomba
    }
}