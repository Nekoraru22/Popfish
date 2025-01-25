using Unity.VisualScripting;
using UnityEngine;

public class GameStartTriggerScript : MonoBehaviour
{
    public GameObject fishController;

    private void Start() {
        Debug.Log("GameStartTriggerScript started");
        fishController.GetComponent<FishControllerScript>().DisableColliders();
    }

    // on colision
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Fish")) {
            Debug.Log("Player entered the trigger");
            fishController.GetComponent<FishControllerScript>().EnableColliders();
        }
    }
}
