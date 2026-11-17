using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingStation : MonoBehaviour, IInteractable
{
    [SerializeField] private List<CookingRecipeSo> CookingRecipe;
    [SerializeField] private List<Image> ItemImage;

    private List<ItemSo> _containingItem = new();

    
    #region Engine Lifecycle
    void Awake()
    {
        RefreshContainingItemSprite();
    }
    #endregion

    public void Interact(Player interactor)
    {
        if(!interactor) return;
        if (interactor.GetHeldItem())
        {
            _containingItem.Add(interactor.GetHeldItem());
            interactor.RemoveCurrentItem();
        }
        else
        {
            if (_containingItem.Count <= 0) return;
            int lastIndex = _containingItem.Count - 1;
            interactor.TryPickUpItem(_containingItem[lastIndex]);
            _containingItem.RemoveAt(lastIndex);

        }
        RefreshContainingItemSprite();
    }


    private void RefreshContainingItemSprite()
    {
        for (int i = 0; i < ItemImage.Count; i++)
        {
            bool shouldActive = _containingItem.Count > i;
            ItemImage[i].gameObject.SetActive(shouldActive);

            if(shouldActive) ItemImage[i].sprite = _containingItem[i].GetItemSprite();
        }
    }
}
