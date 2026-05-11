using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMinu : MonoBehaviour
{
    public CanvasGroup OptionPanel;

    void Start(){

    }

    void Update(){
        if (Keyboard.current.escapeKey.wasPressedThisFrame){
            Option();
        }
    }
    public void Back(){
        OptionPanel.alpha = 0;
        OptionPanel.blocksRaycasts = false;
    }

    public void Option(){
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
    }

    public void QuitGame(){
        Application.Quit();
    }
}
