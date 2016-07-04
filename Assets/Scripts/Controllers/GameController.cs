using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;


public class GameController : MonoBehaviour {

    public static bool SkillUsedNow;
    // Пременная юзаеться ли скилл. Изменяеться в экземплярах скиллов. 

    bool EnemyOnMapNow;
    // Переменная для системы обнаружения наличи \ отсутствия мобов.

    public float CameraSpeed;
    Vector3 Distantion;
    bool FreeCamera;
    Camera CameraMain;
    //Переменная для  режима движения камеры.

    bool WaveOn;
    public int LvlWave;
    public float[] WaveDuration;
    public float PauseBetweenWawes;
    public int MaxWaves;
    // Переменные дял старта \ окончания игры. Для длительности волн противнико и пауз.

    GameObject HitGObject;
    GUISkin Skin;
    Rect Rect;
    bool Draw;
    bool SingleSelected;
    bool GUIConstrustion;
    bool ClickGUI;
    Vector2 StartPosition;
    Vector2 EndPosition;
    // Переменные для выделения рамкой.

    PlayerCharacter PlayerCharacterComponent;
    MinionCharacter MinionCharacterComponent;

    // Выбранный обьект игрок или миньон и ссылка на ге окомпонент.
    Barrack SelectedBarrack;
    // Выбранная казарма.

    public PoolObjects  ObjectPool;
    // Пул обьектов.
    public GameObject Player;
    // Ссылка на игрока.
    public Sofa SofaObject;
    // Ссылка на диван.
    public Fountain FountainObject;

    StateController State;
    OneMinionControlState OneMinionControl;
    NonSelectedState NonSelected;
    PlayerControlState PlayerControl;
    BarrackControlStat BarrackControlState;
    // Переменные для состояний контроллера.
    
    public UnityEvent LeftClickOnTerrain;
    public UnityEvent RightClickOnEnemy;
    // Событие для утсановки точки назначения.

    public UnityEvent ControlPlayer;
    public UnityEvent ControlBarrack;
    // События для состояний. Подисаны вью.

    public UnityEvent NoEnemyOnMap;
    public UnityEvent EnemyOnMap;
    public UnityEvent StartWave;
    public UnityEvent StartPause;
    public UnityEvent VictoryGame;
    public UnityEvent LoseGame;
    // События начала \ остановки игры.


    void Start ()
    {
        PlayerCharacterComponent = Player.GetComponent<PlayerCharacter>();
        // Получение компонентов.
       
        CameraMain = Camera.main;
        Distantion = CameraMain.transform.position - Player.transform.position;
        CameraSpeed = 0.3f;
        // Вводим данные для камеры.

        Time.timeScale = 1;
        // Выставляем течение времени. Если переигрываем после поражения.
        LvlWave = 0;
        // Начальный уровень волны.

        OneMinionControl = new OneMinionControlState();
        NonSelected = new NonSelectedState();
        PlayerControl = new PlayerControlState();
        BarrackControlState = new BarrackControlStat();

        LeftClickOnTerrain = new UnityEvent();
        RightClickOnEnemy = new UnityEvent();

        ControlPlayer = new UnityEvent();
        ControlBarrack = new UnityEvent();
        // События.

        PlayerCharacterComponent.PlayerDead.AddListener(FreeCameraOn);
        StartWave.AddListener(StartWaveOn);
        StartPause.AddListener(PauseWavesOn);
        // Подписи.

        PauseWavesOn();
        // Старт игры.
    }
	
	void Update ()
    {
        InspectEnemyOnMap();
        ClickMouseFunctions();
        InspectionLifeSofa();
    }

    void LateUpdate()
    {
        CameraMove();
    }

    void ClickMouseFunctions()
    {
        if (Input.GetMouseButton(0) && !Draw && !SkillUsedNow)
        {
            RaycastFucntionLeftClick();
        }
        else if (Input.GetMouseButton(1))
        {
            RaycastFunctionRightClick();
        }
        // Обработка одинчоных нажатий мышки.

        if (Input.GetMouseButtonDown(0) && !SkillUsedNow)
        {
            StartPosition = Input.mousePosition;
            Draw = true;
        }
        else if (Input.GetMouseButtonUp(0) && !SkillUsedNow)
        {
            Draw = false;
        }
        // Обработка выделения рамкой.

        if (SkillUsedNow)
        {
            Draw = false;
        }
        // Держим отрисовку рамки выключенной пока идет юзание скилла.
    }
    // Все отслеживания нажатий кнопок мышки.

    void RaycastFucntionLeftClick()
    {
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

            if (hit.transform.tag == "Minion")
            {
                this.State = NonSelected;
                State.ControlState(this);
                MinionCharacterComponent = hit.transform.GetComponent<MinionCharacter>();
                this.State = OneMinionControl;
                State.ControlState(this);
            }
            // Если попали в миньона, то переводим состояние котнтроллера в состояние КонтроляОдногоМиньона.
            // Предварительно вызываем состояние NonSelected, что бы сбросить предыдущие контроли.
            else if (hit.transform.tag == "Player")
            {
                this.State = NonSelected;
                State.ControlState(this);
                this.State = PlayerControl;
                State.ControlState(this);
            }
            // Если попали по игроку.
            else if (hit.transform.tag == "Terrain")
            {
                this.State = NonSelected;
                State.ControlState(this);
            }
            // Если попали по земле сбрасываем все контроли и выключаем все вью кроме левел-вью через состояние NonSelected.
            else if (hit.transform.tag == "Barrack")
            {
                this.State = NonSelected;
                State.ControlState(this);
                SelectedBarrack = hit.transform.gameObject.GetComponent<Barrack>();
                this.State = BarrackControlState;
                State.ControlState(this);
            }
        }
    }
    // Нажатие левой кнопки мыши. Выбор состояния контроллера.

    void RaycastFunctionRightClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Terrain")
            {
                StaticData.DestionationPoint = hit.point;
                LeftClickOnTerrain.Invoke();
                PlayerCharacterComponent.Skill1.DeactivateSkill.Invoke();
                PlayerCharacterComponent.Skill2.DeactivateSkill.Invoke();
                // Деактивируем все скиллы. При нажатии правой кнопкой по земле идет отмена скиллов.
            }
            // Если попали по земле то выставляем статическую переменную точка назначения.

            else if (hit.transform.tag == "Enemy")
            {
                StaticData.Target = hit.transform.gameObject;
                StaticData.TargetComponent = hit.transform.GetComponent<Character>();
                RightClickOnEnemy.Invoke();
            }
         }
        // Если попали по врагу то засовываем его в статическую переменную таргета.
    }
    // Нажатие правой кнопки мыши.  Запись глобальной конечной точки или цели.

     void OnGUI()
    {
        if (!SkillUsedNow)
        {
            RaycastHit hit1;
            Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray1, out hit1, Mathf.Infinity))
            {
                GUI.skin = Skin;
                GUI.depth = 99;

                if (Draw)
                {
                    EndPosition = Input.mousePosition;
                    if (StartPosition == EndPosition)
                    {
                        return;
                    }

                    Rect = new Rect(Mathf.Min(EndPosition.x, StartPosition.x),
                        Screen.height - Mathf.Max(EndPosition.y, StartPosition.y),
                        Mathf.Max(EndPosition.x, StartPosition.x) - Mathf.Min(EndPosition.x, StartPosition.x),
                        Mathf.Max(EndPosition.y, StartPosition.y) - Mathf.Min(EndPosition.y, StartPosition.y)
                        );

                    GUI.Box(Rect, "");
                    for (int j = 0; j < ObjectPool.Minions.Count; j++)
                    {
                        Vector2 Tmp = new Vector2(Camera.main.WorldToScreenPoint(ObjectPool.Minions[j].transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(ObjectPool.Minions[j].transform.position).y);
                        // Трансформируем позицию объекта из мирового пространства, в пространство экрана.
                        Vector2 Tmp1 = new Vector2(Camera.main.WorldToScreenPoint(Player.transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(Player.transform.position).y);
                        if (Rect.Contains(Tmp))
                        {
                            ObjectPool.ComponentMinionCharPool[j].SetControlPlayer(true);
                            ObjectPool.ComponentMinionCharPool[j].ControlMarkEnable();
                            ObjectPool.ComponentMinionCharPool[j].EnableControlMarkOnTarget();
                            RightClickOnEnemy.AddListener(ObjectPool.ComponentMinionCharPool[j].AttackTarget);
                        }
                        // Выставляем попавшим в рамку миньонам буллевую , отвечающу за контроль игроком.
                        // Подвписываем всех выделенных на событие нажатие правой кнопки мыши.
                        if (Rect.Contains(Tmp1) && Input.GetMouseButtonUp(0))
                        {
                            PlayerCharacterComponent.SetControlPlayer(true);
                            PlayerCharacterComponent.EnableControlMarkOnTarget();
                            RightClickOnEnemy.AddListener(PlayerCharacterComponent.AttackTarget);
                            this.State = PlayerControl;
                            State.ControlState(this);
                        }
                        // Если игрок в рамке то контролируем и его. Включаем вью игрока.
                    }
                }
            }
        }
    }
    // Групповое выделение рамкой.
    


    void PauseWavesOn()
    {
        StartCoroutine(PauseWaves());
    }
    // Метода запускающий корутину Паузы
    
    IEnumerator PauseWaves()
    {
        WaveOn = false;
        yield return new WaitForSeconds(PauseBetweenWawes);
        StartWave.Invoke();
    }
    // Пауза между волнами.

    void StartWaveOn()
    {
        StartCoroutine(Wave());
    }
    // Метода запускающий корутину Волны.

    IEnumerator Wave()
    {
        WaveOn = true;
        yield return new WaitForSeconds(WaveDuration[LvlWave]);
        if ((LvlWave + 1) < MaxWaves)
        {
            LvlWave++;
            StartPause.Invoke();
        }
        else
        {
            Time.timeScale = 0;
            WaveOn = false;
            VictoryGame.Invoke();
        }
    }
    // Волна.
    


    void InspectionLifeSofa()
    {
        if (SofaObject.Health < 0)
        {
            Time.timeScale = 0;
            WaveOn = false;
            LoseGame.Invoke();
        }
    }
    // Проверка жив ли диван. И если мертв то  событие прогигрыша.

    void InspectEnemyOnMap()
    {
        if (EnemyOnMapNow == true)
        {
            if (GameObject.FindObjectOfType<EnemyCharacter>() == null)
            {
                Debug.Log("Нет мобов!");
                StaticData.DestionationPoint = FountainObject.transform.position;
                NoEnemyOnMap.Invoke();
                EnemyOnMapNow = false;
            }
        }
        if (EnemyOnMapNow == false)
        {
            if (GameObject.FindObjectOfType<EnemyCharacter>() != null)
            {
                Debug.Log("Есть мобы");
                StaticData.DestionationPoint = SofaObject.transform.position;
                EnemyOnMap.Invoke();
                EnemyOnMapNow = true;
            }
        }
    }
    // Проверка наличия \ отстствия мобов. Если нету, то миньоны отправляются к фонтану. Если есть к дивнану. 

    void NoEnemyInstallPoinDestination()
    {
        StaticData.DestionationPoint = SofaObject.transform.position;
    }



    void CameraMove()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (FreeCamera == false)
            {
                FreeCamera = true;
            }
            else if (FreeCamera == true)
            {
                FreeCamera = false;
            }
        }
        // Кнопкой F переключаем способ поведения камеры.

        if (FreeCamera == false)
        {
            CameraMain.transform.position = Player.transform.position + Distantion;
            // В конце каждого кадра назначаем новую позицию камеры.
        }
        // Камера следует за игроком.
        else if (FreeCamera == true)
        {
            if (Input.mousePosition.y <= 5)
            {
                CameraMain.transform.position = new Vector3(CameraMain.transform.position.x, CameraMain.transform.position.y, CameraMain.transform.position.z - CameraSpeed);
            }
            else if (Input.mousePosition.y >= (Screen.height - 5))
            {
                CameraMain.transform.position = new Vector3(CameraMain.transform.position.x, CameraMain.transform.position.y, CameraMain.transform.position.z + CameraSpeed);

            }
            else if (Input.mousePosition.x <= 5)
            {
                CameraMain.transform.position = new Vector3(CameraMain.transform.position.x - CameraSpeed, CameraMain.transform.position.y, CameraMain.transform.position.z);

            }
            else if (Input.mousePosition.x >= (Screen.width - 5))
            {
                CameraMain.transform.position = new Vector3(CameraMain.transform.position.x + CameraSpeed, CameraMain.transform.position.y, CameraMain.transform.position.z);

            }

        }
        // Управление камерой с посощью позиции мышки на ее границе.

    }
    // Движение камеры либо за игроком. Либо управляеться наведением мыши на края. Переключаеться клавишей "V".

    void FreeCameraOn()
    {
        FreeCamera = true;
    }
    // Включение  управления камерой с помощью мыши. Подписана на событие смерти персонажа.



    // Геттеры - сеттеры.

    public bool GetWaveBool()
    {
        return WaveOn;
    }
    // Возвращет знаяение включена ли волна.

    public Barrack GetSelectedBarrack()
    {
        return SelectedBarrack;
    }
    // Возвращает обьект выделенной казамрмы.

    public PlayerCharacter GetPlayerComponent()
    {
        return PlayerCharacterComponent;
    }
    // Возвращает компонент игрока.
    public MinionCharacter GetMinionComponent()
    {
        return MinionCharacterComponent;
    }
    // Возвращает компонент миньона.
}

