using UnityEngine;

public class PoisonousWaterScript : MonoBehaviour
{
    public FishControllerScript fishControllerScript;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Fish")) {
            Debug.Log("Player entered the poisonous water");
            fishControllerScript.SetPoison();
        }
    }
}
