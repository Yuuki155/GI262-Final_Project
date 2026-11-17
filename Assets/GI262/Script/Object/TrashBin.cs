using UnityEngine;

public class TrashBin : MonoBehaviour,IInteractable
{
    public void Interact(Player interactor)
    {
        if (!interactor.GetHeldItem()) return;

        interactor.RemoveCurrentItem();
    }
}
