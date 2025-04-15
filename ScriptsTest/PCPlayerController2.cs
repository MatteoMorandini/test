using System;

public class PCPlayerController2 : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public Animator animator;

    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float sprintDuration = 5f;
    public float sprintCooldown = 3f;
    public float turnSmoothTime = 0.1f;

    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    private Vector3 velocity;

    private float turnSmoothVelocity;
    private float sprintTimer;
    private bool canSprint = true;

    private void update()
    {
        Move();
        ApplyGravity();
    }
    
    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && canSprint;
        float speed = isSprinting ? runSpeed : walkSpeed;

        if (isSprinting)
        {
            Sprint();

        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
           
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Update animator
        animator.SetFloat("Speed", direction.magnitude * (isSprinting ? 2f : 1f));
    }

    //it used to force the playr's sprint when player trigger the enemy

    void Sprint()
    {

        sprintTimer += Time.deltaTime;
        if (sprintTimer >= sprintDuration)
        {
            canSprint = false;
            sprintTimer = 0f;
            Invoke(nameof(ResetSprint), sprintCooldown);
        }
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void ResetSprint()
    {
        canSprint = true;
    }