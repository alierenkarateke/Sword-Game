using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    Rigidbody rb;
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction attackAction;

    [SerializeField] float moveSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        attackAction = playerInput.actions.FindAction("Attack");

        attackAction.performed += onAttack;
    }

     void OnEnable()
    {
        moveAction.Enable();
        attackAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        attackAction.Disable();
    }

    void FixedUpdate()
    {
        movePlayer();

    }

    void movePlayer()
    {
        Vector2 moveDirection = moveAction.ReadValue<Vector2>();
        rb.transform.position += new Vector3(moveDirection.x * moveSpeed * Time.fixedDeltaTime, 0, moveDirection.y * moveSpeed * Time.fixedDeltaTime);
    }

     private void onAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack");
    }
}
