using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSo ItemData;

    [Header("Component")]
    [SerializeField] private SpriteRenderer ItemSpriteRenderer;

    private bool _hasInitialized;

    #region Engine Lifecycle
    private void Awake()
    {
        InitializeItem(ItemData);
    }
    #endregion

    public void InitializeItem(ItemSo itemData)
    {
        if (itemData == null || _hasInitialized) return;

        ItemData = itemData;
        RefreshHeldItemSprite();
    }


    private void RefreshHeldItemSprite()
    {
        ItemSpriteRenderer.gameObject.SetActive(ItemData != null);
        ItemSpriteRenderer.sprite = ItemData?.GetItemSprite();
    }

    public void Interact(Player interactor)
    {
        if (interactor.GetHeldItem()) return;

        interactor.TryPickUpItem(ItemData);
        Destroy(gameObject);
    }
}
