using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BatmanSignalController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform batSignalPivot; 
    [SerializeField] private Light2D batSignalLight;   

    void Update()
    {
        HandleBatSignal();
    }

    /// <summary>
    /// Toggles the Bat-Signal light and rotates it.
    /// </summary>
    private void HandleBatSignal()
    {
        if (batSignalPivot == null || batSignalLight == null) return;

        // Toggle On/Off (We toggle the Light component, not the whole anchor)
        if (Input.GetKeyDown(KeyCode.B))
        {
            batSignalLight.enabled = !batSignalLight.enabled;

            // Also show/hide the beam triangle if you have a sprite renderer on it
            batSignalPivot.GetComponentInChildren<SpriteRenderer>().enabled = batSignalLight.enabled;
        }

        // Rotate the ANCHOR (The Hinge)
        if (batSignalLight.enabled)
        {
            // This rotates the Anchor. Because the Triangle and Logo are children,
            // they will "sweep" across the sky like a windshield wiper.
            float angle = Mathf.Sin(Time.time * 1.5f) * 30f + 40;
            batSignalPivot.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
