using UnityEngine;

public class FishControllerScript : MonoBehaviour
{
    public GameObject fish;
    
    public void SetStun() {
        // Rebotar e inmovilizar al pez durante 2 segundos
    }

    public void SetPoison() {
        // Menos salto y/o velocidad durante 5 segundos
    }

    public void SetDrowned() {
        // Invertir controles hasta recargar el ox�geno
    }

    public void RestoreDrowned() {
        // Restaurar controles
    }

    public void LoseBubbles(int bubbles) {
        // Restar burbujas
    }

    public void RefillBubbles(int bubbles) {
        // Sumar burbujas
        // Si -1, sumar todas las burbujas
    }

    public void DisableColliders() {
        // Desactivar colisiones
    }

    public void EnableColliders() {
        // Activar colisiones
    }
}
