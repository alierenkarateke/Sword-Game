using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerCombat : MonoBehaviour
{
    #region Settings
    [Header("Attack")]
    [SerializeField] float hitBoxDuration;
    [SerializeField] float attackCoolDown;
    [SerializeField] GameObject hitBoxTrigger;

    #endregion Settings

    #region State

    public bool isActive;
    private bool isAttacking = false;

    #endregion State

    #region Components
    private HitBoxController hitBoxController;
    private Animator animator;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction attackAction;
    
    #endregion Components

    #region UnityLifecycle

    void Awake()
    {
        hitBoxTrigger.SetActive(false);
        isActive = true;
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        
        hitBoxController = hitBoxTrigger.GetComponent<HitBoxController>();

        attackAction = playerInput.actions["Attack"];
    }

    void Update()
    {
        if (!isActive) return;
        HandleAttackInput();
    }

    #endregion UnityLifecycle

    #region ActiveHitBox

    IEnumerator ActivateHitBox()
    {
        StartAttack();
        yield return new WaitForSeconds(hitBoxDuration);
        EndHitBox();
        yield return new WaitForSeconds(attackCoolDown);
        EndAttack();
    }

    #endregion ActiveHitBox

    #region Methods

    private void EndAttack()
    {
        isAttacking = false;
    }

    private void EndHitBox()
    {
        hitBoxTrigger.SetActive(false);
    }

    private void StartAttack()
    {
        animator.SetTrigger("attackTrigger");
        isAttacking = true;
        hitBoxTrigger.SetActive(true);
    }

    private void HandleAttackInput()
    {
        if (attackAction.WasPressedThisFrame() && !isAttacking && !GetComponent<PlayerController>().isDodging)
        {
            StartCoroutine(ActivateHitBox());
        }
    }
    #endregion Methods
}
