using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Collider2D InteractCollider;
    [SerializeField] private PlayerInputReaderSo PlayerInputReader;
    
    public ItemSo HeldItem;
    
    private readonly List<Collider2D> _interactHits = new List<Collider2D>();

    #region Engine Lifecycle
    private void OnEnable()
    {
        PlayerInputReader.InteractEvent += OnInteract;
    }

    private void OnDisable()
    {
        PlayerInputReader.InteractEvent -= OnInteract;
    }
    #endregion

    private void OnInteract()
    {
        if (InteractCollider == null || !InteractCollider.isActiveAndEnabled) return;
        InteractCollider.Overlap(ContactFilter2D.noFilter, _interactHits);
        foreach (var hit in _interactHits)
        {
            if (hit.transform.IsChildOf(transform)) continue;

            var interactable = hit.GetComponentInParent<IInteractable>();
            if (interactable == null) continue;

            interactable.Interact(this);
            return;
        }
    }
}
