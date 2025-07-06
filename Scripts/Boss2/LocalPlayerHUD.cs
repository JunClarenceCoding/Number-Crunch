using UnityEngine;
using UnityEngine.UI;

public class LocalPlayerHUD : MonoBehaviour
{
    public Slider localHealthSlider;

    // Method to initialize the health slider with the given max health
    public void Initialize(int health)
    {
        // Ensure this is executed on the main thread
        UnityMainThreadDispatcher.Enqueue(() =>
        {
            localHealthSlider.maxValue = health; // Set the maximum value for the health slider
            localHealthSlider.value = health;     // Set the initial value to max health
        });
    }

    // Method to update the health slider
    public void UpdateHealth(int health)
    {
        // Ensure this is executed on the main thread
        UnityMainThreadDispatcher.Enqueue(() =>
        {
            localHealthSlider.value = health; // Update the current health value
        });
    }
}
