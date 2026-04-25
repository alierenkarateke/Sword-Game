using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    #region Settings
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [Header("Dodge")]
    [SerializeField] private float dodgeTime;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float dodgeCooldown = 1f;

    #endregion Settings

    #region  Components

    private Animator animator;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction dodgeAction;
    private AudioSource audioSource;
    [Header("Sound")]
    [SerializeField] private AudioClip dodgeClip;
    
    #endregion Components

    #region State
    
    [Header("State")]
    public bool isDodging = false;
    public bool isActive;
    private Vector2 moveInput;
    private Vector3 moveDir;
    private float lastDodgeTime = float.NegativeInfinity;

    #endregion State

    #region  UnityLifecycle

    void Awake()
    {
        isActive = true;
    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();

        moveAction = playerInput.actions["Move"];
        dodgeAction = playerInput.actions["Dodge"];
    }

    void Update()
    {
        if (!isActive) return;
        HandleDodgeInput();
    }

    void FixedUpdate()
    {
        if (!isActive) return;
        ResetVelocity();
        ReadMovementInput();
        ApplyMovement();
        ApplyRotation();
        UpdateAnimator();
    }
  

    #endregion UnityLifecycle

    #region Dodge

    IEnumerator DodgeCoroutine()
    {
        if (moveDir == Vector3.zero) yield break;
        StartDodge();
        yield return new WaitForSeconds(dodgeTime);
        EndDodge();
    }

    #endregion Dodge

    #region Methods

    private void EndDodge()
    {
        rb.linearVelocity = Vector3.zero;
        isDodging = false;
        animator.SetBool("isDodging", false);
    }

    private void StartDodge()
    {
        audioSource.PlayOneShot(dodgeClip);
        lastDodgeTime = Time.time;
        isDodging = true;
        animator.SetBool("isDodging", true);
        rb.AddForce(moveDir * dodgeForce, ForceMode.VelocityChange);
    }

    private void HandleDodgeInput()
    {
        if (dodgeAction.WasPressedThisFrame() && !isDodging && Time.time - lastDodgeTime > dodgeCooldown && moveDir != Vector3.zero)
        {
            StartCoroutine(DodgeCoroutine());
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isMoving", moveDir != Vector3.zero);
    }

    private void ApplyRotation()
    {
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.Slerp(rb.rotation,
            Quaternion.LookRotation(moveDir),
            rotationSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(targetRotation);
        }
    }

    private void ApplyMovement()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    private void ReadMovementInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        moveDir = new Vector3(moveInput.x, 0, moveInput.y);
    }

    private void ResetVelocity()
    {
        if (!isDodging) rb.linearVelocity = Vector3.zero;
    }

    #endregion Methods
}