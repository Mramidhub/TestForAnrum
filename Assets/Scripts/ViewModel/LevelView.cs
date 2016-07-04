using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelView : BaseView {


    float TempWaveDuration;
    float TempWaveDuration1Decrese;
    float TempBetweenWaveTime;
    float TempBetweenWaveDecrease;


    float ReburnTime;
    public Text ShowLvlUpExpirience;
    public Text ShowCurrentExpirience;
    public Text ShowReburnTime;
    public Text ShowHpPlayerText;
    public Text ShowMoneyText;
    public Text ShowLvlText;
    public Text ShowSofaHealth;
    public Text GeneralMessage;
    public Text LvlWave;

    PlayerCharacter PlayerCharacterComponent;
    GameController GameControllerComponent;
    Sofa SofaComponent;

    public Canvas EndGameCanvas;
    public Text EndGameText;
    

    void Start()
    {
        GameControllerComponent = GameControllerObject.GetComponent<GameController>();
        PlayerCharacterComponent = PlayerObject.GetComponent<PlayerCharacter>();
        SofaComponent = GameControllerComponent.SofaObject;
        GameControllerComponent.StartPause.AddListener(RefreshWaveInfo);
        GameControllerComponent.VictoryGame.AddListener(EndGameVictoryhow);
        GameControllerComponent.LoseGame.AddListener(EndGameLoseShow);
        PlayerCharacterComponent.PlayerDead.AddListener(ShowReburnTimeTextOn);
        PlayerCharacterComponent.PlayerLife.AddListener(ShowReburnTimeTextOff);
        ReburnTime = PlayerCharacterComponent.ReburnTime;
        SystemMessage.enabled = false;
        RefreshWaveInfo();
    }


    void Update()
    {
        ShowHp();
        ShowLvl();
        ShowMoney();
        ShowWaveInfo();
        ShowHealthSofa();
        ShowReburnCount();
        ShowExpirienceLvlUp();
        ShowExpirienceCurrent();
    }

    void ShowWaveInfo()
    {

        if (GameControllerComponent.GetWaveBool() == true)
        {
            LvlWave.text = "Волна " + (GameControllerComponent.LvlWave +1) + " из 3";

            if (TempWaveDuration1Decrese > 0)
            {
                TempWaveDuration1Decrese -= Time.deltaTime;

                GeneralMessage.text = "До окончания волны:" + (int)TempWaveDuration1Decrese;

            }
        }
        if(GameControllerComponent.GetWaveBool() == false)
        {
            if (TempBetweenWaveDecrease > 0)
            {
                TempBetweenWaveDecrease -= Time.deltaTime;

                GeneralMessage.text = "До начала следующей волны:" + (int)TempBetweenWaveDecrease;

            }
        }
    }
    // Вывод на экран сообщения о длительности волны и о ожидании следующей.


    void ShowReburnCount()
    {
        if (ShowReburnTime.enabled)
        {
            ReburnTime -= Time.deltaTime;
            ShowReburnTime.enabled = true;
            ShowReburnTime.text = "Воскрешение: " + (int)ReburnTime;
        }
        else
        {
            ReburnTime = PlayerCharacterComponent.ReburnTime;
            ShowReburnTime.enabled = false;
        }
    }
    // Вывод времени воскрешения если обьект текста активен.

    void ShowReburnTimeTextOn()
    {
        ShowReburnTime.enabled = true;
    }
    // Включение вывода времени воскрешения. Подписана на события смерти \ жизни игрока.
    void ShowReburnTimeTextOff()
    {
        ShowReburnTime.enabled = false;
    }
    // Выключение вывода времени воскрешения. Подписана на события смерти \ жизни игрока.

    void ShowExpirienceCurrent()
    {
        ShowCurrentExpirience.text = "Опыт:\n" + PlayerCharacterComponent.Expirience;
    }
    // Вывод на экран текущего опыта.
    void ShowExpirienceLvlUp()
    {
        ShowLvlUpExpirience.text = "LvlUp при:\n" + PlayerCharacterComponent.LevelUpExpirience[PlayerCharacterComponent.Lvl - 1];
    }
    // Вывод на экран нужного для поднятия лвла опыта.

    void ShowHp()
    {
        ShowHpPlayerText.text = "HP: \n" + (int)PlayerCharacterComponent.Health;
    }
    // Вывод на экран HP.

    void ShowMoney()
    {
        ShowMoneyText.text = "Золото \n" + PlayerCharacterComponent.Money;
    }
    // Вывод на экран денжищ.

    void ShowLvl()
    {
        ShowLvlText.text = "Lvl\n" + PlayerCharacterComponent.Lvl;
    }
    // Вывод уровня игрока.
    void ShowHealthSofa()
    {
        ShowSofaHealth.text = "" + SofaComponent.Health;
    }
    // Вывод жизней дивана, как бы странно это не звучало:)

    void RefreshWaveInfo()
    {
        TempWaveDuration = GameControllerComponent.WaveDuration[GameControllerComponent.LvlWave];
        TempWaveDuration1Decrese = TempWaveDuration;
        TempBetweenWaveTime = GameControllerComponent.PauseBetweenWawes;
        TempBetweenWaveDecrease = TempBetweenWaveTime;
    }
    // Обновление даных с повышением уровня волны.  Подписана на событие геймконтроллера StartPause.

    void EndGameVictoryhow()
    {
        EndGameCanvas.enabled = true;

        EndGameText.text = "Победа!";
    }
    // Активация баннера победы. Подписана на событие в GameControllere.
    void EndGameLoseShow()
    {
        EndGameCanvas.enabled = true;

        EndGameText.text = "Поражение!";
    }
    // Активация баннера поражения. Подписана на событие в GameControllere.

    public void EndGameOkButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    //Кнокпка в баннере победы \ поражения. Возвращает в главное меню.

   
}
