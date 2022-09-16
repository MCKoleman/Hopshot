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
    [SerializeField]
    private Vector2 moveDelta;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Time.fixedDeltaTime * moveSpeed * moveDelta);
        //rb.MovePosition(Vector2.Lerp(rb.position, targetPos, moveLerpSpeed * Time.deltaTime));
    }

    #region Input Actions
    // Sets the target movement of the player
    public void HandleMove(Vector2 delta)
    {
        moveDelta = delta.normalized;
    }

    //
    public void HandleAttack()
    {
        // TODO: Handle attacking
    }

    //
    private void HandlePause()
    {

    }
    #endregion

    #region Input Context Handlers
    public void HandleMoveContext(InputAction.CallbackContext context)
    {
        if (!CanTakeInput())
            return;

        HandleMove(context.ReadValue<Vector2>());
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

    #region Helper Functions
    private bool CanTakeInput()
    {
        return GameManager.Instance.IsGameActive && !GameManager.Instance.IsGamePaused();
    }
    #endregion
}
