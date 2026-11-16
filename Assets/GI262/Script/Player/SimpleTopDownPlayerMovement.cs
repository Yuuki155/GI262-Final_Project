using UnityEngine;

public class SimpleTopDownPlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public PlayerInputReaderSo PlayerInputReader;
    public float MoveSpeed = 5f;

    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    #region Engine Lifecycle
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        PlayerInputReader.MoveEvent += OnMove;
        var asset = PlayerInputReader.GetInputActionAsset();
        OnMove(asset.FindAction("Move", true).ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        PlayerInputReader.MoveEvent -= OnMove;

        _moveInput = Vector2.zero;
        _rb.linearVelocity = Vector2.zero;
    }
    #endregion

    private void OnMove(Vector2 direction)
    {
        _moveInput = direction.normalized;
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _moveInput * MoveSpeed;
    }
}
