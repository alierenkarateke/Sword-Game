using UnityEngine;
using UnityEngine.InputSystem;

public class AI_PlayerSpawner : MonoBehaviour
{
    [Header("Oyuncu Ayarları")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint0;
    [SerializeField] private Transform spawnPoint1;

    [Header("Renkler")]
    [SerializeField] private Color[] playerColors = { Color.blue, Color.red };

    [Header("Gamepad Desteği")]
    [SerializeField] private bool autoAssignGamepads = true;

    private readonly string[] controlSchemes = { "Keyboard Left", "Keyboard Right" };

    private void Start()
    {
        SpawnPlayer(0, spawnPoint0);
        SpawnPlayer(1, spawnPoint1);
    }

    private void SpawnPlayer(int index, Transform spawnPoint)
    {
        if (playerPrefab == null)
        {
            Debug.LogError("[PlayerSpawner] Player Prefab atanmamış!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError($"[PlayerSpawner] SpawnPoint {index} atanmamış!");
            return;
        }

        // Cihaz ve scheme belirle
        var gamepads = Gamepad.all;
        InputDevice device;
        string scheme;

        if (autoAssignGamepads && index < gamepads.Count)
        {
            device = gamepads[index];
            scheme = "Gamepad";
        }
        else
        {
            device = Keyboard.current;
            scheme = controlSchemes[index];
        }

        // Spawn noktasında instantiate et
        var playerInput = PlayerInput.Instantiate(
            playerPrefab,
            playerIndex: index,
            controlScheme: scheme,
            pairWithDevice: device
        );

        // Rigidbody varsa fizik motorunu bypass ederek pozisyonu set et
        if (playerInput.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.Sleep();                              // fiziği durdur
            rb.position = spawnPoint.position;       // rb üzerinden set et
            rb.rotation = spawnPoint.rotation;
            rb.linearVelocity = Vector3.zero;               // kalan hızı sıfırla
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            playerInput.transform.position = spawnPoint.position;
            playerInput.transform.rotation = spawnPoint.rotation;
        }

        // PlayerController'a index ver
        if (playerInput.TryGetComponent<PlayerController>(out var controller))
           //controller.SetPlayerIndex(index);

        // Renk uygula
        ApplyColor(playerInput.gameObject, index);

        Debug.Log($"[PlayerSpawner] Oyuncu {index} → {spawnPoint.position} | Scheme: {scheme}");
    }

    private void ApplyColor(GameObject player, int index)
    {
        if (playerColors == null || index >= playerColors.Length) return;
        var rend = player.GetComponentInChildren<Renderer>();
        if (rend == null) return;
        rend.material = new Material(rend.sharedMaterial) { color = playerColors[index] };
    }
}
