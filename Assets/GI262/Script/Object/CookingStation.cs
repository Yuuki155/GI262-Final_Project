using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingStation : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField] private List<CookingRecipeSo> CookingRecipe;
    [SerializeField] private List<Image> ItemImage;
    [SerializeField] private TMP_Text StationTextComponent;
    [SerializeField] private Slider CookingTimeSlider;
    
    [Header("Data")]
    [SerializeField] private string StationWaitingText;
    [SerializeField] private string StationCookingText;
    [SerializeField] private float CookingTime = 4f;
    
    private List<ItemSo> _containingItem = new();
    private float _cookingTimeLeft;
    private bool _isCooking;
    private ItemSo _cookingProduct;
    
    #region Engine Lifecycle
    void Awake()
    {
        RefreshContainingItemSprite();
        RefreshCookingUI();
    }

    void Update()
    {
        if (!_isCooking) return;

        _cookingTimeLeft = Mathf.Max(0f, _cookingTimeLeft - Time.deltaTime);
        if (_cookingTimeLeft <= 0f)
        {
            FinishCooking();
            return;
        }

        RefreshCookingUI();
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
        CheckRecipeForCooking();
        RefreshContainingItemSprite();
    }
    
    private void CheckRecipeForCooking()
    {
        foreach (CookingRecipeSo recipe in CookingRecipe)
        {
            if (!recipe) continue;

            int requiredCount = 0;
            bool matches = true;
            foreach (var ingredient in recipe.GetIngredients())
            {
                int actualCount = 0;
                foreach (ItemSo item in _containingItem)
                {
                    if (item == ingredient.Key) actualCount++;
                }

                if (actualCount != ingredient.Value)
                {
                    matches = false;
                    break;
                }

                requiredCount += ingredient.Value;
            }

            if (matches && requiredCount > 0 && requiredCount == _containingItem.Count)
            {
                StartCooking(recipe.GetProduct());
                return;
            }
        }
        StopCooking();
    }
    
    private void StartCooking(ItemSo product)
    {
        _cookingTimeLeft = Mathf.Max(0f, CookingTime) * (_isCooking ? 0.5f : 1f);
        _cookingProduct = product;
        _isCooking = true;
        RefreshCookingUI();
    }

    private void StopCooking()
    {
        _isCooking = false;
        _cookingTimeLeft = 0f;
        _cookingProduct = null;
        RefreshCookingUI();
    }

    private void RefreshCookingUI()
    {
        StationTextComponent.text = _isCooking ? StationCookingText : StationWaitingText;
        CookingTimeSlider.gameObject.SetActive(_isCooking);
        CookingTimeSlider.minValue = 0f;
        CookingTimeSlider.maxValue = 1f;
        CookingTimeSlider.wholeNumbers = false;
        CookingTimeSlider.value = _isCooking && CookingTime > 0f
            ? 1f - _cookingTimeLeft / CookingTime
            : 0f;
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
    
    private void FinishCooking()
    {
        _containingItem = new();
        _containingItem.Add(_cookingProduct);
        StopCooking();
        RefreshContainingItemSprite();
    }
    
}
