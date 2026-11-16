using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//[CreateAssetMenu(fileName = "PlayerInputReaderSo", menuName = "Scriptable Objects/PlayerInputReader")]
public class PlayerInputReaderSo : ScriptableObject
{
    [SerializeField] InputActionAsset InputActionAsset;
    public InputActionAsset GetInputActionAsset() => InputActionAsset;
    
    public event UnityAction<Vector2> MoveEvent;
    public event UnityAction InteractEvent;
    
    
    private InputAction _moveAction;
    private InputAction _interactAction;
    

    private void OnEnable()
    {
        if (InputActionAsset == null) return;

        _moveAction = InputActionAsset.FindAction("Move", true);
        _interactAction = InputActionAsset.FindAction("Interact", true);

        _moveAction.performed += OnMoveAction;
        _moveAction.canceled += OnMoveAction;
        _interactAction.started += OnInteractAction;

        _moveAction.Enable();
        _interactAction.Enable();
    }

    private void OnDisable()
    {
        if (_moveAction != null)
        {
            _moveAction.Disable();
            _moveAction.performed -= OnMoveAction;
            _moveAction.canceled -= OnMoveAction;
        }

        if (_interactAction != null)
        {
            _interactAction.Disable();
            _interactAction.started -= OnInteractAction;
        }
    }

    private void OnMoveAction(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        MoveEvent?.Invoke(direction);
    }

    private void OnInteractAction(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
    }
    
}
