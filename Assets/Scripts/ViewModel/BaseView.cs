using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class BaseView : MonoBehaviour {

    public Canvas ViewCanvasComponent;
    public GameObject GameControllerObject;
    public GameObject PlayerObject;
    public Text SystemMessage;

    void Start ()
    {
	
	}
	

	void Update ()
    {
	
	}

    public void ViewOn()
    {
        ViewCanvasComponent.enabled = true;
    }
    // Включение канваса.
    public void ViewOff()
    {
        ViewCanvasComponent.enabled = false;
    }
    // Выключение канваса.

    public void DisableButton(Button Button1)
    {
        Button1.enabled = false;
        Button1.image.enabled = false;
    }
    // Выключение кнопки.

    public void EnableButton(Button Button1)
    {
        Button1.enabled = true;
        Button1.image.enabled = true;
    }
    // Включение кнопки.
}
