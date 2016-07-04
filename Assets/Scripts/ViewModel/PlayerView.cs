using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerView : BaseView{

    public Button LvlUpSkill1;
    public Button LvlUpSkill2;

    public Text ReusedTime1;
    public Text ReusedTime2;

    public Button Button1;
    Image ComponentImageButton1;
    Color ColorButtonSkill1;
    public Button Button2;
    Image ComponentImageButton2;
    Color ColorButtonSkill2;


    PlayerCharacter PlayerCharacterComponent;

	void Start ()
    {
        DisableButton(LvlUpSkill1);
        DisableButton(LvlUpSkill2);

        ViewCanvasComponent = transform.GetComponent<Canvas>();
        PlayerCharacterComponent = PlayerObject.GetComponent<PlayerCharacter>();
        GameControllerObject.GetComponent<GameController>().ControlPlayer.AddListener(ViewOn);
        PlayerCharacterComponent.LvlUp.AddListener(EnableButtonsOnLvlUp);

        PlayerCharacterComponent.Skill1.ActivateSkill.AddListener(ShadowButtonSkill1);
        PlayerCharacterComponent.Skill2.ActivateSkill.AddListener(ShadowButtonSkill2);
        PlayerCharacterComponent.Skill1.DeactivateSkill.AddListener(UnshadowShadowButtonSkill1);
        PlayerCharacterComponent.Skill2.DeactivateSkill.AddListener(UnshadowShadowButtonSkill2);

        ComponentImageButton1 = Button1.GetComponent<Image>();
        ComponentImageButton2 = Button2.GetComponent<Image>();

        ColorButtonSkill1 = ComponentImageButton1.color;
        ColorButtonSkill2 = ComponentImageButton2.color;

        NonSelectedState.CancelAllControl += ViewOff;
    }

	void Update ()
    {
        ShowReusedTime();
        HotKeys();
    }

    public void ButtonSkill1()
    {
        if (!PlayerCharacterComponent.Skill1.GetRefreshSkillBool())
        {
            PlayerCharacterComponent.Skill1.ActivateSkill.Invoke();
        }
    }
    // Кнопка активации первого скилла.

    public void ButtonEnchantSkill1()
    {
        PlayerCharacterComponent.Skill1.SkillLvlUp();
        DisableButton(LvlUpSkill1);
        DisableButton(LvlUpSkill2);
    }
    // Кнопка прокачки первого скилла.

    public void ButtonSkill2()
    {
        if (!PlayerCharacterComponent.Skill2.GetRefreshSkillBool())
        {
            PlayerCharacterComponent.Skill2.ActivateSkill.Invoke();
        }
    }
    // Кнопка активации второго скилла.

    public void ButtonEnchantSkill2()
    {
        PlayerCharacterComponent.Skill2.SkillLvlUp();
        DisableButton(LvlUpSkill1);
        DisableButton(LvlUpSkill2);
    }
    // Кнопка прокачки второго скилла.

    void ShowReusedTime()
    {
        if (PlayerCharacterComponent.Skill1.GetRefreshSkillBool())
        {
            ReusedTime1.text = "" + (int)PlayerCharacterComponent.Skill1.ReusedTime;
            ReusedTime1.text = "" + (int)PlayerCharacterComponent.Skill1.ReusedTime;
        }
        else
        {
            ReusedTime1.text = "";
        }

        if (PlayerCharacterComponent.Skill2.GetRefreshSkillBool())
        {
            ReusedTime2.text = "" + (int)PlayerCharacterComponent.Skill2.ReusedTime;
            ReusedTime2.text = "" + (int)PlayerCharacterComponent.Skill2.ReusedTime;
        }
        else
        {
            ReusedTime2.text = "";
        }
    }
    // Вывод кулдауна скиллов.

    void EnableButtonsOnLvlUp()
    {
        EnableButton(LvlUpSkill1);
        EnableButton(LvlUpSkill2);
    }

    void HotKeys()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            {
                PlayerCharacterComponent.Skill2.ActivateSkill.Invoke();
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            {
                PlayerCharacterComponent.Skill1.ActivateSkill.Invoke();
            }
        }
    }
    // Обработка хоткеев сикллбара.


    void ShadowButtonSkill1()
    {
        ComponentImageButton1.color = new Color(0, 0, 0);
    }
    // Затемнение скилла1.
    void ShadowButtonSkill2()
    {
        ComponentImageButton2.color = new Color(0, 0, 0);
    }
    // Затемнение скилла2.


    void UnshadowShadowButtonSkill1()
    {
        ComponentImageButton1.color = ColorButtonSkill1;
    }
    // Возвращение цвета скилла1.
    void UnshadowShadowButtonSkill2()
    {
        ComponentImageButton2.color = ColorButtonSkill2;
    }
    // Возвращение цвета скилла1.
}
