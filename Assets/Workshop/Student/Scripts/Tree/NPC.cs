using Solution;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class NPC : Identity
{
    public DialogueUI dialogueUI;
    public DialogueSequen dialogueSeauen;
    public bool canTalk = true;
    public PatienceBarUI patienceBarUI;

    private void Update()
    {
        if(patienceBarUI.isTimeUp == true)
        {
            ReceiveFood();
            ResetCustomer();
        }
    }
    public override bool Hit()
    {
        // ตรวจสอบว่าผู้เล่นมีไอเท็มที่ต้องการหรือไม่
        if (canTalk)
        {
            dialogueUI.Setup(dialogueSeauen);
            dialogueSeauen.dialogueUI = dialogueUI;
            ResetCustomer();
            return false;
        }
        else
        {
            Debug.Log("I not neet to talk to you");
            return false;
        }
    }
    public void ReceiveFood()
    {
        if (mapGenerator != null)
        {
            mapGenerator.mapdata[positionX, positionY] = null;
        }
        gameObject.SetActive(false);
        if (mapGenerator != null)
        {
            mapGenerator.CheckWaveComplete();
        }
    }
    public void ResetCustomer()
    {
        // ใส่โค้ดสุ่มเมนูอาหารใหม่ของลูกค้าตรงนี้
        // เช่น currentOrder = Random.Range(0, 3);
        patienceBarUI.ResetcurrPatience();
        patienceBarUI.isTimeUp = false;
    }
}
