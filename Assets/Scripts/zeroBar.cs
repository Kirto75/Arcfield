using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class zeroBar : MonoBehaviour
{
    public Slider slider;
    public RectTransform rectTransform;
    Image g;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        g = rectTransform.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustColor();
        OnZero();
    }

    void OnZero(){
        if (slider.value == 0){
            g.enabled = false;
        }
    }

    void AdjustColor(){
        if (slider.value > 70){
            g.color = new Color32(0, 183, 15, 255);
        }
        else if (slider.value > 40){
            g.color = new Color32(255, 216, 0, 255);
        }
        else{
            g.color = new Color32(255, 23, 0, 255);
        }
    }

}
