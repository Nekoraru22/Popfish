using UnityEngine;
using UnityEngine.UI;

public class StateEffectsScript : MonoBehaviour
{
    // Get children of Empty GameObject
    public Image[] buffs;
    public Image[] debuffs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < buffs.Length; i++) {
            buffs[i].enabled = false;
        }
        for (int i = 0; i < debuffs.Length; i++) {
            debuffs[i].enabled = false;
        }
        enableBuff(0);
    }

    public void enableBuff(int index) {
        buffs[index].enabled = true;
    }

    public void disableBuff(int index) {
        buffs[index].enabled = false;
    }

    public void enableDebuff(int index) {
        debuffs[index].enabled = true;
    }

    public void disableDebuff(int index) {
        debuffs[index].enabled = false;
    }
}
