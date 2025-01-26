using UnityEngine;

public class SpikesScript : MonoBehaviour
{


    private GameObject FishScriptObj;
    private GameObject FishControllerObj;

    private FishScript fishscript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FishScriptObj = GameObject.Find("MutantFish");
        Debug.Log(FishControllerObj);
        fishscript = FishScriptObj.GetComponent<FishScript>();
        Debug.Log(fishscript);

        FishControllerObj = GameObject.Find("FishController");
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

    void hit()
    {
        float random = UnityEngine.Random.Range(-1f, 1f);
        Vector2 direction = new Vector2(random, 1.7f);
        fishscript.body.AddForce(direction*3 , ForceMode2D.Impulse);
        fishscript.RemoveBubble(1);
    }
}
