using UnityEngine;

public class ControlsScript : MonoBehaviour {
    [SerializeField] private GameObject[] controlButtons;
    private FishScript fishScript;

    void Start() {
        var fish = GameObject.Find("MutantFish");
        if (!fish || !(fishScript = fish.GetComponent<FishScript>())) {
            Debug.LogError("Failed to initialize ControlsScript - missing references");
            return;
        }

        UpdateAllButtonTexts();
    }

    private void UpdateAllButtonTexts() {
        var controls = new[] {
            fishScript.keyIzquierda,
            fishScript.KeyDerecha,
            fishScript.KeyPlanear,
            fishScript.KeyCheckPoint,
            fishScript.KeyBomba,
            fishScript.KeySalto
        };

        for (int i = 0; i < controlButtons.Length; i++) {
            UpdateButtonText(i, controls[i]);
        }
    }

    private void UpdateButtonText(int index, KeyCode key) {
        var text = controlButtons[index]?.GetComponentInChildren<UnityEngine.UI.Text>();
        if (text) text.text = key.ToString();
    }

    public void ChangeControl(int index, KeyCode newKey) {
        if (index < 0 || index >= controlButtons.Length) return;

        var property = typeof(FishScript).GetProperty($"Key{index}");
        if (property != null) {
            property.SetValue(fishScript, newKey);
            UpdateButtonText(index, newKey);
        }
    }
}