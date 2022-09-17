using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private float moveLerpSpeed = 6.0f;

    [Header("Jump")]
    [SerializeField]
    private float jumpForce = 5.0f;
    [SerializeField]
    private float massNormal = 1.0f;
    [SerializeField]
    private float massHeavy = 3.0f;

    private float moveDeltaX;
    private int jumpCount;
    private Rigidbody2D rb;

    private const float JUMP_THRESHOLD = 0.3f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //SetIsHeavy(rb.velocity.y < 0.0f);
        
        if(moveDeltaX != 0.0f)
            rb.AddForce(moveSpeed * moveDeltaX * Vector2.right);
        //rb.MovePosition(rb.position + Time.fixedDeltaTime * moveSpeed * moveDelta);
        //rb.MovePosition(Vector2.Lerp(rb.position, targetPos, moveLerpSpeed * Time.deltaTime));
    }

    #region Input Actions
    // Sets the target movement of the player
    public void HandleMove(Vector2 delta)
    {
        moveDeltaX = delta.x;
        //Debug.Log("I'm Jumping");
    }

    // Handles jumping
    public void HandleJump()
    {
        //SetIsHeavy(false);
        rb.AddForce(jumpForce * Vector2.up);
        //Debug.Log("I'm Jumping");
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

    #region Helper Functions
    private bool CanTakeInput()
    {
        return GameManager.Instance.IsGameActive && !UIManager.Instance.IsPaused;
    }

    // Sets the players mass as either heavy or normal
    private void SetIsHeavy(bool isHeavy)
    {
        if ((rb.mass == massHeavy && isHeavy) || (rb.mass == massNormal && !isHeavy))
            return;

        if (isHeavy)
            rb.mass = massHeavy;
        else
            rb.mass = massNormal;
    }
    #endregion

    #region Input Context Handlers
    public void HandleMoveContext(InputAction.CallbackContext context)
    {
        if (!CanTakeInput() && context.ReadValue<Vector2>() != Vector2.zero)
            return;

        if (context.performed && context.ReadValue<Vector2>().y > JUMP_THRESHOLD)
        {
            HandleJump();
            jumpCount = 0;
        }
        if (context.canceled && context.ReadValue<Vector2>().y > JUMP_THRESHOLD)
            jumpCount = 1;

        HandleMove(context.ReadValue<Vector2>());
    }

    public void HandleJumpContext(InputAction.CallbackContext context)
    {
        if (!CanTakeInput())
            return;

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
