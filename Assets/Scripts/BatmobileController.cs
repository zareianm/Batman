using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BatmobileController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float stealthSpeed = 2f;
    [SerializeField] private float boostSpeed = 10f;
    [SerializeField] private float rotationSpeed = 150f;
 

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float currentSpeed;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleInput()
    {
        float drive = Input.GetAxis("Vertical");

        float turn = Input.GetAxis("Horizontal");

        moveInput = new Vector2(turn, drive);
    }

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
}