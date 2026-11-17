using UnityEngine;

//[CreateAssetMenu(fileName = "ItemSo", menuName = "Scriptable Objects/Item")]
public abstract class ItemSo : ScriptableObject
{
    [Header("Item Data")]
    [SerializeField] string ItemName;
    [SerializeField] Sprite ItemSprite;

    public string GetItemName() => ItemName;
    public Sprite GetItemSprite() => ItemSprite;



}
