using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
//[RequireComponent(typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] public float moveSpeed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public float dodgeTime;
    [SerializeField] public float dodgeForce;

    
    private Animator animator;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction dodgeAction;
    private Vector2 moveInput;
    public bool isDodging = false;

    Vector3 moveDir;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();

        moveAction = playerInput.actions["Move"];
        dodgeAction = playerInput.actions["Dodge"];
    }

    void Update()
    {
         if (dodgeAction.WasPressedThisFrame() && !isDodging)
        {
            StartCoroutine(DodgeCoroutine());
        }
    }

    void FixedUpdate()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        moveDir = new Vector3(moveInput.x, 0, moveInput.y);

        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        if(moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.Slerp(rb.rotation, 
            Quaternion.LookRotation(moveDir), 
            rotationSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(targetRotation);
        }
        animator.SetBool("isMoving", moveDir != Vector3.zero);
    }

    IEnumerator DodgeCoroutine()
    {
        //animator.SetTrigger("dodgeTrigger");
        isDodging = true;
        if(moveDir != Vector3.zero)
        {
            rb.AddForce(moveDir * dodgeForce, ForceMode.VelocityChange);    
        }
        /*
        else
        {
            rb.AddForce(-transform.forward * dodgeForce, ForceMode.VelocityChange);
        }
        */
        yield return new WaitForSeconds(dodgeTime);
        rb.linearVelocity = Vector3.zero;
        isDodging = false;
    }

}