using Unity.VisualScripting;
using UnityEngine;

public class FishControllerScript : MonoBehaviour
{
    public GameObject fish;
    private FishScript fishScript;
    public GameObject bubbleControler;
    private BubbleScript bubbleScript;

    private void Start()
    {

    }
    //fish = GameObject.Find("fish").GetComponent<FishScript>();

    public void SetStun() {
        // Rebotar e inmovilizar al pez durante 2 segundos
        fishScript.SetStunned();
    }

    public void SetPoison() {
        // Menos salto y/o velocidad durante 5 segundos
        fishScript.SetPoison();
    }

    public void SetDrowned() {
        // Invertir controles hasta recargar el oxígeno
        fishScript.SetInverseControls();
    }

    public void RestoreDrowned() {
        // Restaurar controles
        fishScript.SetNormalControls();
    }

    public void EndRefill()
    {
        fishScript.SetWater();
    }

    public void DisableColliders() {
        // Desactivar colisiones pero no triggers
        fish.GetComponent<Collider2D>().isTrigger = true;
    }

    public void EnableColliders() {
        // Activar colisiones
        fish.GetComponent<Collider2D>().isTrigger = false;
    }

    public void EnableSlowFalling()
    {
        // Activar caida lenta / planeo
    }
    public void DisableSlowFalling()
    {

    }

    public void EnableDobleJump()
    {
        fishScript.SetNumSalots(2);
    }
    public void DisableDobleJump() { 
        fishScript.SetNumSalots(1);
    }

}
