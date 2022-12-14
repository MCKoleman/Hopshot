using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move Speed Mods")]
    [SerializeField]
    private PlayerSpeedMods speedMods;
    [SerializeField]
    private float boopRecoil;

    [Header("Cooldowns")]
    [SerializeField]
    private float maxJumpCooldown = 0.2f;
    private float curJumpCooldown = 0.0f;
    [SerializeField]
    private float maxCoyoteTime = 0.1f;
    private float curCoyoteTime = 0.0f;
    [SerializeField]
    private float maxBoopCooldown = 0.1f;
    private float curBoopCooldown = 0.0f;
    [SerializeField]
    private float maxBounceTime = 0.1f;
    private float curBounceTime = 0.0f;
    private float groundedTime = 0.0f;

    // State data
    private float moveDelta;
    [SerializeField]
    private bool isGrounded = true;
    private bool isFacingForward = true;
    private List<GameObject> touchingGroundObjs;
    public bool IsTouchingGround { get { return touchingGroundObjs.Count != 0; } }

    // Constants
    private const float MOUSE_AIM_ERROR_ZONE = 0.5f;
    private const float MIN_LOOK_THRESHOLD = 0.1f;
    private const float JUMP_THRESHOLD = 0.3f;
    private const float MAX_SLOPE_ANGLE = 45.0f;
    private const float MINIMUM_GROUNDED_TIME = 0.1f;
    private const float MINIMUM_GROUNDED_DROP = -0.3f;
    private const float ANIM_POW_MOD = 0.3678795f;

    // Components
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerCharacter character;
    private BoopGun boopGun;
    private PlayerAudioController audioController;

    private void OnEnable()
    {
        if(character == null)
            character = this.GetComponent<PlayerCharacter>();
        character.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        character.OnDeath -= HandleDeath;
    }

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        boopGun = this.GetComponentInChildren<BoopGun>();
        audioController = this.GetComponentInChildren<PlayerAudioController>();
        touchingGroundObjs = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        // Move the player
        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(moveDelta, rb.velocity.y), GetAccelerationMod(moveDelta != 0.0f) * Time.fixedDeltaTime);
        SetIsHeavy(rb.velocity.y < 0.0f);
        SetAnimationStates();

        // Reset grounded status
        if (isGrounded && rb.velocity.y < MINIMUM_GROUNDED_DROP && !IsTouchingGround)
            SetIsGrounded(false);
        else if (!isGrounded && MathUtils.AlmostZero(rb.velocity.y, 2) && groundedTime >= MINIMUM_GROUNDED_TIME)
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
        if (curBoopCooldown > 0.0f)
        {
            curBoopCooldown = Mathf.Max(curBoopCooldown - Time.fixedDeltaTime, 0.0f);
            UIManager.Instance.UpdateBoopCooldown(curBoopCooldown / maxBoopCooldown);

            // If the cooldown is almost zero, treat it as zero
            if(Mathf.Approximately(curBoopCooldown, 0.0f))
            {
                curBoopCooldown = 0.0f;
                boopGun.Reload();
                UIManager.Instance.UpdateBoopCooldown(0.0f);
            }
        }
        if (curBounceTime > 0.0f)
            curBounceTime = Mathf.Max(curBounceTime - Time.fixedDeltaTime, 0.0f);
        if (isGrounded)
            groundedTime += Time.deltaTime;
    }

    // Returns the acceleration mod for the player's movement
    private float GetAccelerationMod(bool isAccelerating)
    {
        return (isAccelerating ? speedMods.MOVE_ACC_MOD : speedMods.MOVE_DEC_MOD) * (isGrounded ? 1.0f : (isAccelerating ? speedMods.AIR_ACC_MOD : speedMods.AIR_DEC_MOD));
    }

    #region Input Handles
    // Sets the target movement of the player
    public void HandleMove(Vector2 delta)
    {
        moveDelta = delta.x * speedMods.MOVE_SPEED;

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
        anim?.SetTrigger("Jump");
        audioController.Jump();

        SetIsHeavy(false);
        rb.velocity = new Vector2(rb.velocity.x, speedMods.JUMP_FORCE);
        curJumpCooldown = maxJumpCooldown;
        SetIsGrounded(false);
    }

    // Handles player attacking
    public void HandleAttack()
    {
        // TODO: Handle attacking
        if(curBoopCooldown <= 0.0f)
        {
            Vector2 tempDirection = -1.0f * boopGun.FireProjectile();
            rb.AddForce(boopRecoil * tempDirection);
            curBoopCooldown = maxBoopCooldown;
            UIManager.Instance.UpdateBoopCooldown(1.0f);
        }
    }

    // Handles looking using a joystick
    public void HandleLookDelta(Vector2 delta)
    {
        // Only change facing direction if the delta is greater than the minimum threshold
        if(Mathf.Abs(delta.x) > MIN_LOOK_THRESHOLD)
        {
            isFacingForward = (delta.x * this.transform.localScale.x) >= 0.0f;
            SetFacingDir();
        }
    }

    // Handles looking using a pointer position (mouse/touch)
    public void HandleLookPos(Vector2 pos)
    {
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(pos);
        if(Mathf.Abs(lookPos.x - this.transform.position.x) > MOUSE_AIM_ERROR_ZONE)
        {
            isFacingForward = (lookPos.x - this.transform.position.x) * this.transform.localScale.x >= 0.0f;
            SetFacingDir();
        }
    }

    // Plays a footstep sound
    public void HandleFootstep()
    {
        audioController.Footstep();
    }

    // Handles the player death
    private void HandleDeath()
    {
        audioController.Die();
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
            audioController.Land();
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

    // Sets the animation states to reflect the player's current status
    private void SetAnimationStates()
    {
        anim?.SetFloat("velocityX", GetModifiedAnimScale(rb.velocity.x));
        anim?.SetFloat("velocityY", GetModifiedAnimScale(rb.velocity.y));
        anim?.SetFloat("absVelocityY", GetModifiedAnimScale(Mathf.Abs(rb.velocity.y)));
        anim?.SetBool("hasVelocityX", (Mathf.Abs(rb.velocity.x) > speedMods.MOVE_ANIM_THRESHOLD));
        anim?.SetBool("hasVelocityY", !Mathf.Approximately(rb.velocity.y, 0.0f));
        SetFacingDir();
    }

    // Returns the modified animation scale of the given value
    private float GetModifiedAnimScale(float value)
    {
        float sign = (value >= 0.0f) ? 1.0f : -1.0f;
        return sign * Mathf.Pow(Mathf.Abs(value * speedMods.MOVE_ANIM_MULTIPLIER), ANIM_POW_MOD);
    }

    // Sets the player's facing direction
    private void SetFacingDir()
    {
        // Swap looking direction
        bool isMovingForward = rb.velocity.x > 0.0f;
        if (Mathf.Abs(rb.velocity.x) > MIN_LOOK_THRESHOLD)
        {
            // If swapping moving direction, swap facing direction too
            if ((isMovingForward && this.transform.localScale.x < 0.0f) || (!isMovingForward && this.transform.localScale.x > 0.0f))
                isFacingForward = !isFacingForward;
            this.transform.localScale = new Vector3(isMovingForward ? 1.0f : -1.0f, 1.0f, 1.0f);
        }
        anim?.SetBool("facingForward", isFacingForward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only check ground collisions
        if (!collision.collider.CompareTag("Ground") && !collision.collider.CompareTag("Friend") && !collision.collider.CompareTag("Enemy"))
            return;

        //Debug.Log($"Player collided with [{collision.collider.name}]");
        // Only count grounding if colliding on angle smaller than MAX_SLOPE_ANGLE
        if (Vector2.Angle(Vector2.up, collision.GetContact(0).normal) >= MAX_SLOPE_ANGLE)
            return;

        //Debug.Log($"Angle threshold passed");
        // Remove touching object when leaving its collider
        if (!touchingGroundObjs.Contains(collision.gameObject))
            touchingGroundObjs.Add(collision.gameObject);
        anim?.SetTrigger("HitGround");
        SetIsGrounded(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Ground") && !collision.collider.CompareTag("Friend") && !collision.collider.CompareTag("Enemy"))
            return;
        // Remove touching object when leaving its collider
        if(touchingGroundObjs.Contains(collision.gameObject))
            touchingGroundObjs.Remove(collision.gameObject);
    }



    #region Helper Functions
    private bool CanTakeInput()
    {
        return GameManager.Instance.IsGameActive && !UIManager.Instance.IsPaused && !character.IsDead;
    }

    // Sets the players mass as either heavy or normal
    private void SetIsHeavy(bool isHeavy)
    {
        if ((rb.gravityScale == speedMods.GRAVITY_HEAVY && isHeavy) || (rb.gravityScale == speedMods.GRAVITY_NORMAL && !isHeavy))
            return;

        if (isHeavy)
        {
            rb.gravityScale = speedMods.GRAVITY_HEAVY;
            anim?.SetTrigger("Falling");
        }
        else
        {
            rb.gravityScale = speedMods.GRAVITY_NORMAL;
        }
    }

    // Returns the player's position
    public Vector2 GetPos() { return this.transform.position; }
    #endregion



    #region Input Context Handlers
    public void HandleMoveContext(InputAction.CallbackContext context)
    {
        if (CanTakeInput() || context.ReadValue<Vector2>() == Vector2.zero)
            HandleMove(context.ReadValue<Vector2>());
    }

    public void HandleLookPosContext(InputAction.CallbackContext context)
    {
        if (CanTakeInput())
            HandleLookPos(context.ReadValue<Vector2>());
    }

    public void HandleLookDeltaContext(InputAction.CallbackContext context)
    {
        if (CanTakeInput())
            HandleLookDelta(context.ReadValue<Vector2>());
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
