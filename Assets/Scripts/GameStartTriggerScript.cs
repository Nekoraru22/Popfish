using Unity.VisualScripting;
using UnityEngine;

public class GameStartTriggerScript : MonoBehaviour
{
    public GameObject fishController;

    private void Start() {
        fishController.GetComponent<FishControllerScript>().DisableColliders();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Fish")) {
            Debug.Log("Player entered the trigger");
            fishController.GetComponent<FishControllerScript>().EnableColliders();
        }
    }
}
