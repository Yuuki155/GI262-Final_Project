using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Collider2D InteractCollider;
    [SerializeField] private PlayerInputReaderSo PlayerInputReader;
    [Header("Component")]
    [SerializeField] private SpriteRenderer HeldItemSpriteRenderer;

    private ItemSo _heldItem;
    private readonly List<Collider2D> _interactHits = new List<Collider2D>();

    #region Engine Lifecycle
    private void OnEnable()
    {
        PlayerInputReader.InteractEvent += OnInteract;
        RefreshHeldItemSprite();
    }

    private void OnDisable()
    {
        PlayerInputReader.InteractEvent -= OnInteract;
    }
    #endregion

    public ItemSo GetHeldItem() => _heldItem;

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

    private void RefreshHeldItemSprite()
    {
        HeldItemSpriteRenderer.gameObject.SetActive(_heldItem != null);
        HeldItemSpriteRenderer.sprite = _heldItem?.GetItemSprite();
    }

    public void TryPickUpItem(ItemSo item)
    {
        if (_heldItem != null) return;
        _heldItem = item;
        RefreshHeldItemSprite();
    }

    public void RemoveCurrentItem()
    {
        if (_heldItem == null) return;
        _heldItem = null;
        RefreshHeldItemSprite();
    }

}
