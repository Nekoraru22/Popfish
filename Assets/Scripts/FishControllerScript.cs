using Unity.VisualScripting;
using UnityEngine;

public class FishControllerScript : MonoBehaviour
{
    public GameObject fish;
    private FishScript fishScript;

    private void Start()
    {
        fishScript = fish.GetComponent<FishScript>();
    }
    //fish = GameObject.Find("fish").GetComponent<FishScript>();

    public void SetStun() {
        // Rebotar e inmovilizar al pez durante 2 segundos
        fishScript.setStunned();
    }

    public void SetPoison() {
        // Menos salto y/o velocidad durante 5 segundos
        fishScript.setPosion();
    }

    public void SetDrowned() {
        // Invertir controles hasta recargar el oxígeno
        fishScript.SetInverseControls();
    }

    public void RestoreDrowned() {
        // Restaurar controles
        fishScript.SetNormalControls();
    }

    public void LoseBubbles(int bubbles) {
        // Restar burbujas
    }

    public void RefillBubbles(int bubbles) {
        // Sumar burbujas
        // Si -1, sumar todas las burbujas
    }

    public void DisableColliders() {
        // Desactivar colisiones pero no triggers
        fish.GetComponent<Collider2D>().isTrigger = true;
    }

    public void EnableColliders() {
        // Activar colisiones
        fish.GetComponent<Collider2D>().isTrigger = false;
    }
}
