using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 8 yönlü karakter hareketi — Rigidbody tabanlı.
/// Gerekli componentler: Rigidbody, PlayerInput
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class AI_PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotationSpeed = 20f;

    [Header("Player Info")]
    [SerializeField] private int playerIndex = 0;

    // Components
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;

    // State
    private Vector3 moveDirection = Vector3.zero;

    #region Unity Lifecycle

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    #endregion

    #region Input

    private void ReadInput()
    {
        Vector2 raw = moveAction.ReadValue<Vector2>();

        if (raw.magnitude < 0.2f)
        {
            moveDirection = Vector3.zero;
            return;
        }

        Vector2 snapped = SnapToEightDirections(raw);
        moveDirection = new Vector3(snapped.x, 0f, snapped.y);
    }

    /// <summary>
    /// Ham input'u en yakın 45°'ye kilitler → 8 yön.
    /// </summary>
    private Vector2 SnapToEightDirections(Vector2 input)
    {
        // Açıyı hesapla (kuzey = 0°, saat yönü artar)
        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;

        // En yakın 45°'ye yuvarla
        float snapped = Mathf.Round(angle / 45f) * 45f;

        float rad = snapped * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
    }

    #endregion

    #region Physics

    private void Move()
    {
        if (moveDirection == Vector3.zero) return;

        Vector3 newPos = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    private void Rotate()
    {
        if (moveDirection == Vector3.zero) return;

        Quaternion targetRot = Quaternion.LookRotation(moveDirection);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
    }

    #endregion

    #region Public API

    public void SetPlayerIndex(int index) => playerIndex = index;
    public int GetPlayerIndex() => playerIndex;

    #endregion
}
