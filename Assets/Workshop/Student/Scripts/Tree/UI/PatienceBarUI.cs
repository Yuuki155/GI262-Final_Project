using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PatienceBarUI : MonoBehaviour
{
    public Slider patienceSlider;
    public float maxPatience = 100f;
    public float currPatience;
    public float decreaseRate = 10f;
    public bool isTimeUp = false;

    [Header("color Slider")]
    public Image fillImage;
    public Color normalColor = Color.green;    // สีปกติ
    public Color midColor = Color.yellow; // ได้รับความเสียหายระดับ 1 (เช่น HP เหลือ 66%)
    public Color lowColor = Color.red;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currPatience = maxPatience;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateColor();
        if (currPatience >= 0)
        {
            currPatience -= Time.deltaTime * decreaseRate;
        }
        if (currPatience < 0)
        {
            currPatience = 0;
            isTimeUp = true;
        }
            

        patienceSlider.value = currPatience;
        patienceSlider.maxValue = maxPatience;
    }
    void UpdateColor()
    {
        if (currPatience > 70)
            fillImage.color = normalColor;
        else if (currPatience > 50) 
            fillImage.color = midColor;
        else
            fillImage.color = lowColor;
    }
    public void ResetcurrPatience()
    {
        currPatience = maxPatience;
    }
}
