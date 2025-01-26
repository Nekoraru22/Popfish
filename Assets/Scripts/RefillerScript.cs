using UnityEngine;

public class RefillerScript : MonoBehaviour
{
    private GameObject FishScriptObj;
    private GameObject FishControllerObj;

    private FishScript fishscript;
    private FishControllerScript fishcontrollerscript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FishScriptObj = GameObject.Find("MutantFish");
        Debug.Log(FishControllerObj);
        fishscript = FishScriptObj.GetComponent<FishScript>();
        Debug.Log(fishscript);

        FishControllerObj = GameObject.Find("FishController");
        fishcontrollerscript = FishControllerObj.GetComponent<FishControllerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si la colisi�n es con el objeto que queremos (MutantFish)
        if (collision.gameObject.name == "MutantFish")
        {
            Debug.Log("Colisi�n detectada con MutantFish");

            fishscript.RefillBubbles(-1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Verificamos si la colisi�n es con el objeto que queremos (MutantFish)
        if (collision.gameObject.name == "MutantFish")
        {
            Debug.Log("Colisi�n salida con MutantFish");

            // Llama a la funci�n `hit` cuando ocurre la colisi�n
            fishcontrollerscript.EndRefill();
        }
    }


}
