using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarracksView : BaseView {

    public Sprite MnionTrainigImage;

    public Barrack ConcreteBarrack;
    
    // Конкретная казарма.
    public Text CountTraining;
    // Вывод очереди.
    public Text ShowLvlBarrack;
    // Вывод уровня казармы.
    public Text ShowPriceUpBarack;
    // Вывод цены поднятия лвла казармы.
    public Text ShowPriceUnit1;
    // Вывод цены перовго миньона.

    PlayerCharacter PlayerCharacterComponent;
    // Кешировнаие компонентов.

    void Start ()
    {
        PlayerCharacterComponent = PlayerObject.GetComponent<PlayerCharacter>();
        ViewCanvasComponent = transform.GetComponent<Canvas>();
        GameControllerObject.GetComponent<GameController>().ControlBarrack.AddListener(ViewOn);
        NonSelectedState.CancelAllControl += ViewOff;
    }
	

	void Update ()
    {
        CountTraining.text = "В очереди\n" + (int)ConcreteBarrack.GetCounter1Volue();
        ShowLvlBarrack.text = "Lvl " + ConcreteBarrack.Lvl;
        ShowPriceUpBarack.text = "" + ConcreteBarrack.ContructionUpPrice[ConcreteBarrack.Lvl];
        ShowPriceUnit1.text = "" + ConcreteBarrack.Minion1Price;

    }


    public void LvlUpConstrustion()
    {
        if (PlayerCharacterComponent.Money >= ConcreteBarrack.ContructionUpPrice[ConcreteBarrack.Lvl])
        {
            ConcreteBarrack.LvlUpContsruction();
        }
        else
        {
            StartCoroutine(ShowNoMoneyCoroutine());
        }
    }
    // Кнопка подняти лвла здания.

    public void TrainingMobButton1()
    {
        if (PlayerCharacterComponent.Money >= ConcreteBarrack.Minion1Price)
        {
            ConcreteBarrack.IncreaceCounter1();
            PlayerCharacterComponent.Money -= ConcreteBarrack.Minion1Price;
        }
        else
        {
            StartCoroutine(ShowNoMoneyCoroutine());
        }

    }
    // Кнопка тернировки первого моба.

    IEnumerator ShowNoMoneyCoroutine()
    {
        SystemMessage.enabled = true;

        SystemMessage.text = "Не достаточно золота";

        yield return new WaitForSeconds(5f);

        SystemMessage.enabled = false;
    }
    // Корутина о недостатке денег.
}
