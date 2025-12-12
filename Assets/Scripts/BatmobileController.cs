using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Controls the Batmobile's movement, state management (Normal, Stealth, Alert),
/// </summary>
public class BatmobileController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float stealthSpeed = 2f;
    [SerializeField] private float boostSpeed = 10f; // Shift key boost
    [SerializeField] private float rotationSpeed = 150f;

    [Header("Components")]
    [SerializeField] private Light2D headLight; // Reference to the car's light
    [SerializeField] private AudioSource sirenAudio; // Reference for alarm sound

    public enum BatmanState { Normal, Stealth, Alert }
    private BatmanState currentState = BatmanState.Normal;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float currentSpeed;

    private float flashTimer;
    private bool isLightOn;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SetState(BatmanState.Normal);
    }

    private void Update()
    {
        HandleInput();
        UpdateStateBehavior();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    /// <summary>
    /// Reads user input for movement and state switching.
    /// </summary>
    private void HandleInput()
    {
        // Movement Input (W/S or Up/Down arrows)
        float drive = Input.GetAxis("Vertical");
        // Rotation Input (A/D or Left/Right arrows)
        float turn = Input.GetAxis("Horizontal");

        moveInput = new Vector2(turn, drive);

        if (Input.GetKeyDown(KeyCode.N)) SetState(BatmanState.Normal);
        if (Input.GetKeyDown(KeyCode.C)) SetState(BatmanState.Stealth);
        if (Input.GetKeyDown(KeyCode.Space)) SetState(BatmanState.Alert);
    }

    /// <summary>
    /// Applies physics-based movement and rotation to the Batmobile.
    /// </summary>
    private void ApplyMovement()
    {
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : currentSpeed;

        Vector2 velocity = transform.up * moveInput.y * targetSpeed;
        rb.linearVelocity = velocity;

        if (moveInput.y != 0 || moveInput.x != 0) 
        {
            float rotation = -moveInput.x * rotationSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation + rotation);
        }
    }

    /// <summary>
    /// Updates the logic that needs to run every frame based on the current state.
    /// </summary>
    private void UpdateStateBehavior()
    {
        if (currentState == BatmanState.Alert)
        {
            flashTimer += Time.deltaTime;
            if (flashTimer >= 0.2f) // Flash every 0.2 seconds
            {
                flashTimer = 0;
                isLightOn = !isLightOn;

                // Toggle light color between Red and Blue for police effect
                headLight.color = isLightOn ? Color.red : Color.blue;
                headLight.enabled = true;
            }
        }
    }

    /// <summary>
    /// Sets the current state and applies entry logic (color changes, speed changes).
    /// </summary>
    private void SetState(BatmanState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case BatmanState.Normal:
                currentSpeed = normalSpeed;
                headLight.color = Color.white;
                headLight.intensity = 2f;
                headLight.enabled = true;
                if (sirenAudio) sirenAudio.Stop(); // Stop alarm
                break;

            case BatmanState.Stealth:
                currentSpeed = stealthSpeed;
                headLight.intensity = 0.5f; // Dim lights
                headLight.color = new Color(1f, 1f, 1f, 0.75f);
                if (sirenAudio) sirenAudio.Stop();
                break;

            case BatmanState.Alert:
                currentSpeed = normalSpeed;
                headLight.intensity = 2f;
                if (sirenAudio) sirenAudio.Play(); // Play alarm
                break;
        }
    }
}