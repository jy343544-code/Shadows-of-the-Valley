using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4.2f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private float gravity = -22f;
    [SerializeField] private float jumpHeight = 1.8f;

    [Header("Dodge")]
    [SerializeField] private float dodgeSpeed = 10f;
    [SerializeField] private float dodgeDuration = 0.28f;
    [SerializeField] private float dodgeCooldown = 0.75f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;

    private CharacterController controller;
    private Vector2 moveInput;
    private float verticalVelocity;
    private float dodgeTimer;
    private float nextDodgeTime;
    private Vector3 dodgeDirection;

    public bool IsGrounded => controller != null && controller.isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    public void SetMoveInput(Vector2 value) => moveInput = Vector2.ClampMagnitude(value, 1f);

    // Connect to the mobile Jump button.
    public void Jump()
    {
        if (!controller.isGrounded) return;
        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        if (animator != null) animator.SetTrigger("Jump");
    }

    // Connect to the mobile Dodge button.
    public void Dodge()
    {
        if (Time.time < nextDodgeTime || dodgeTimer > 0f) return;
        Vector3 forward = cameraTransform != null ? cameraTransform.forward : transform.forward;
        Vector3 right = cameraTransform != null ? cameraTransform.right : transform.right;
        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();
        Vector3 inputDirection = forward * moveInput.y + right * moveInput.x;
        dodgeDirection = inputDirection.sqrMagnitude > 0.01f ? inputDirection.normalized : transform.forward;
        dodgeTimer = dodgeDuration;
        nextDodgeTime = Time.time + dodgeCooldown;
        if (animator != null) animator.SetTrigger("Dodge");
    }

    private void Update()
    {
        if (cameraTransform == null) return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();

        Vector3 move = forward * moveInput.y + right * moveInput.x;
        if (move.sqrMagnitude > 1f) move.Normalize();

        if (dodgeTimer > 0f)
        {
            dodgeTimer -= Time.deltaTime;
            move = dodgeDirection * (dodgeSpeed / Mathf.Max(0.01f, moveSpeed));
        }

        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion target = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }

        if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * moveSpeed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        if (animator != null)
        {
            animator.SetFloat("MoveSpeed", moveInput.magnitude, 0.1f, Time.deltaTime);
            animator.SetBool("Grounded", controller.isGrounded);
            animator.SetFloat("VerticalSpeed", verticalVelocity);
        }
    }
}