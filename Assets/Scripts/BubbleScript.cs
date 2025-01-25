using UnityEngine;
using UnityEngine.UI;

public class BubbleScript : MonoBehaviour
{
    public Image[] bubbles;
    public int bubblesCount = 6;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AddBubbles(-1);
    }

    // Add bubbles
    public void AddBubbles(int bubbles) {
        // Add all bubbles
        if (bubbles == -1) {
            for (int i = 0; i <= bubblesCount; i++) {
                this.bubbles[i].enabled = true;
            }
        }
        // Add bubbles to the current count
        else {
            int count = CountBubbles();
            if (count + bubbles > this.bubblesCount) {
                bubbles = this.bubblesCount - count;
                count = 0;
            }
            for (int i = 0; i <= count + bubbles - 1; i++) {
                this.bubbles[i].enabled = true;
            }
        }
    }

    // Remove bubbles
    public void RemoveBubbles(int bubbles) {
        if (bubbles < 0) {
            bubbles = 0;
        }
        if (bubbles > this.bubblesCount) {
            bubbles = this.bubblesCount;
        }
        int count = CountBubbles();
        if (count - bubbles < 0) {
            bubbles = CountBubbles();
        }
        // Borrar las ultimas burbujas
        for (int i = count - 1; i >= count - bubbles; i--) {
            this.bubbles[i].enabled = false;
        }
    }

    // Count active bubbles
    public int CountBubbles() {
        int count = 0;
        for (int i = 0; i < this.bubblesCount; i++) {
            if (this.bubbles[i].enabled) {
                count++;
            }
        }
        return count;
    }

    // Remove 1 bubble (menu method for testing)
    [ContextMenu("Remove Bubble")]
    public void RemoveBubble() {
        RemoveBubbles(1);
    }

    // Add 1 bubble (menu method for testing)
    [ContextMenu("Add Bubble")]
    public void AddBubble() {
        AddBubbles(1);
    }
}
