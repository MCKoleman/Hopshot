using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move Speed Mods")]
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private float MOVE_ACC_MOD = 5.0f;
    [SerializeField]
    private float MOVE_DEC_MOD = 5.0f;
    [SerializeField]
    private float AIR_ACC_MOD = 5.0f;
    [SerializeField]
    private float AIR_DEC_MOD = 5.0f;

    [Header("Jump")]
    [SerializeField]
    private float jumpForce = 5.0f;
    [SerializeField]
    private float massNormal = 1.0f;
    [SerializeField]
    private float massHeavy = 3.0f;

    [Header("Cooldowns")]
    [SerializeField]
    private float maxJumpCooldown = 0.2f;
    private float curJumpCooldown = 0.0f;
    [SerializeField]
    private float maxCoyoteTime = 0.1f;
    private float curCoyoteTime = 0.0f;
    [SerializeField]
    private float maxBounceTime = 0.1f;
    private float curBounceTime = 0.0f;
    private float groundedTime = 0.0f;
    private float maxAirMovementCooldown = 0.1f;
    private float curAirMovementCooldown = 0.0f;

    private float moveDelta;
    private bool isGrounded = true;

    private const float JUMP_THRESHOLD = 0.3f;
    private const float MAX_SLOPE_ANGLE = 45.0f;
    private const float MINIMUM_GROUNDED_TIME = 0.1f;
    private const float MINIMUM_GROUNDED_DROP = -0.3f;

    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Move the player
        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(moveDelta, rb.velocity.y), GetAccelerationMod(moveDelta != 0.0f) * Time.fixedDeltaTime);
        SetIsHeavy(rb.velocity.y < 0.0f);

        // Reset grounded status
        if (isGrounded && rb.velocity.y < MINIMUM_GROUNDED_DROP)
            SetIsGrounded(false);
        else if (!isGrounded && MathUtils.AlmostZero(rb.velocity.y, 2) && curAirMovementCooldown <= 0.0f && groundedTime >= MINIMUM_GROUNDED_TIME)
            SetIsGrounded(true);

        HandleCooldowns();
    }

    // Handles all cooldowns in FixedUpdate
    private void HandleCooldowns()
    {
        if (curJumpCooldown > 0.0f)
            curJumpCooldown = Mathf.Max(curJumpCooldown - Time.fixedDeltaTime, 0.0f);
        if (curCoyoteTime > 0.0f && !isGrounded)
            curCoyoteTime = Mathf.Max(curCoyoteTime - Time.fixedDeltaTime, 0.0f);
        if (curBounceTime > 0.0f)
            curBounceTime = Mathf.Max(curBounceTime - Time.fixedDeltaTime, 0.0f);
        if (isGrounded)
            groundedTime += Time.deltaTime;
    }

    // Returns the acceleration mod for the player's movement
    private float GetAccelerationMod(bool isAccelerating)
    {
        return (isAccelerating ? MOVE_ACC_MOD : MOVE_DEC_MOD) * (isGrounded ? 1.0f : (isAccelerating ? AIR_ACC_MOD : AIR_DEC_MOD));
    }

    #region Movement
    // Sets the target movement of the player
    public void HandleMove(Vector2 delta)
    {
        moveDelta = delta.x * moveSpeed;

        if (delta.y > JUMP_THRESHOLD)
            HandleJump();
    }

    // Handles jumping
    public void HandleJump()
    {
        // If the player jumped while off the ground, allow bounce time
        if (curBounceTime <= 0.0f && !isGrounded)
            curBounceTime = maxBounceTime;

        // Don't allow the player to jump if not on the ground, while on cooldown
        if (curJumpCooldown > 0.0f || !(isGrounded || curCoyoteTime > 0.0f))
            return;

        // TODO: Play jump sound

        SetIsHeavy(false);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        curJumpCooldown = maxJumpCooldown;
        SetIsGrounded(false);
    }

    //
    public void HandleAttack()
    {
        // TODO: Handle attacking
    }

    //
    private void HandlePause()
    {
        UIManager.Instance.TogglePause();
    }
    #endregion

    // Sets the state of the grounded variable
    private void SetIsGrounded(bool _isGrounded)
    {
        isGrounded = _isGrounded;
        anim?.SetBool("IsGrounded", isGrounded);
        if(isGrounded)
        {
            SetIsHeavy(false);
            curCoyoteTime = maxCoyoteTime;
            if (curBounceTime > 0.0f)
                HandleJump();
        }
        else
        {
            groundedTime = 0.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only check ground collisions
        if (!collision.gameObject.CompareTag("Ground"))
            return;

        // Only count grounding if colliding on angle smaller than MAX_SLOPE_ANGLE
        if (Vector2.Angle(Vector2.up, collision.GetContact(0).normal) >= MAX_SLOPE_ANGLE)
            return;

        SetIsGrounded(true);
    }

    #region Helper Functions
    private bool CanTakeInput()
    {
        return GameManager.Instance.IsGameActive && !UIManager.Instance.IsPaused;
    }

    // Sets the players mass as either heavy or normal
    private void SetIsHeavy(bool isHeavy)
    {
        if ((rb.gravityScale == massHeavy && isHeavy) || (rb.gravityScale == massNormal && !isHeavy))
            return;

        if (isHeavy)
        {
            rb.gravityScale = massHeavy;
            anim?.SetTrigger("Falling");
        }
        else
        {
            rb.gravityScale = massNormal;
        }
    }

    // Returns the player's position
    public Vector2 GetPos() { return rb.position; }
    #endregion

    #region Input Context Handlers
    public void HandleMoveContext(InputAction.CallbackContext context)
    {
        if (CanTakeInput() || context.ReadValue<Vector2>() == Vector2.zero)
            HandleMove(context.ReadValue<Vector2>());
    }

    public void HandleJumpContext(InputAction.CallbackContext context)
    {
        if (!CanTakeInput())
            return;

        if(context.performed)
            HandleJump();
    }

    public void HandleAttackContext(InputAction.CallbackContext context)
    {
        if (!CanTakeInput())
            return;

        if (context.performed)
            HandleAttack();
    }

    public void HandleMenuContext(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.IsGameActive)
            return;

        if (context.performed)
            HandlePause();
    }
    #endregion
}
