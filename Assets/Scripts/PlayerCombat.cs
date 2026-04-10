using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float hitBoxDuration;
    [SerializeField] float attackCoolDown;

    [SerializeField] GameObject hitBoxTrigger;

    HitBoxController hitBoxController;

    private Animator animator;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction attackAction;
    private bool isAttacking = false;

    void Awake()
    {
        hitBoxTrigger.SetActive(false);
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
        if (attackAction.WasPressedThisFrame() && !isAttacking)
        {
            StartCoroutine(ActivateHitBox());
        }
    }

    IEnumerator ActivateHitBox()
    {
        isAttacking = true;
        hitBoxTrigger.SetActive(true);
        animator.SetTrigger("attackTrigger");
        yield return new WaitForSeconds(hitBoxDuration);
        hitBoxTrigger.SetActive(false);
        yield return new WaitForSeconds(attackCoolDown);
        isAttacking = false;
    }
}
