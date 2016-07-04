using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerCharacter : Character {

    public float ReburnTime;
    // Время возрождения.

    public int[] LevelUpExpirience;
    public float[] LvlUpAddArmor;
    public float[] LvlUpAddPAtack;
    public float[] LvlUpAddPAtackSpeed;
    public float[] LvlUpAddSpeed;
    public float[] LvlUpAddHealth;
    // Добавляемые значения к статам в зависимости при лвлапе.

    public SkillBase Skill1;
    public SkillBase Skill2;
    // Скиллы.

    bool Life;
    public SkinnedMeshRenderer Skin;
    CapsuleCollider ColliderPlayer;
    // Переменные для корутины DeadTime.

    public UnityEvent PlayerDead;
    public UnityEvent PlayerLife;
    public UnityEvent LvlUp;

    Vector3 RespawnPosition;
    // Позиция возрождения игрока.

    void Start ()
    {

        NonSelectedState.CancelAllControl += CancelPlayerCotrolThis;
        NonSelectedState.CancelAllControl += ControlMarkDisable;
        // Событие снятия контроля игроком со всех.

        Lvl = 1;
        Money = 500;
        TempHealth = Health;
        TempMaxHealth = Health;
        gameObject.SetActive(true);
        EndPosition = transform.position;
        AnimationChar = GetComponent<Animator>();
        ColliderPlayer = transform.GetComponent<CapsuleCollider>();
        ControlMarkRendereComponent = ControlMark.GetComponent<Renderer>();
        RespawnPosition = new Vector3(GameObject.Find("Fontain").transform.position.x + 3f, GameObject.Find("Fontain").transform.position.y, GameObject.Find("Fontain").transform.position.z + 3f);
    }


    void Update ()
    {
        Life = true;
        if (AttakingOn == false)
        {
            if (!InPlayerControl)
            {
                TargetSearchCounter(TargetLayerMask);
            }
            MoveToTarget(EndPosition);
        }
        else
        {
            Attacking();
        }
        DeadPlayer();
        InspectLvlUp();
    }



    void DeadPlayer()
    {
        if (Health < 0 && Life == true)
        {
            StartCoroutine(DeadTime());
        }
    }
    // Смерть персонажа.

    IEnumerator DeadTime()
    {
        Life = false;
        PlayerDead.Invoke();
        transform.position = RespawnPosition;
        EndPosition = transform.position;
        Tartget = null;
        Skin.enabled = false;
        ColliderPlayer.enabled = false;
        ControlMarkDisable();
        yield return new WaitForSeconds(ReburnTime);
        ColliderPlayer.enabled = true;
        Skin.enabled = true;
        Life = true;
        Health = TempMaxHealth;
        PlayerLife.Invoke();
    }


    void InspectLvlUp()
    {
        if (Lvl < LevelUpExpirience.Length)
        {
            if (Expirience >= LevelUpExpirience[Lvl - 1] && (Lvl) <= LevelUpExpirience.Length)
            {
                int TempLvl = Lvl - 1;
                Armor += LvlUpAddArmor[TempLvl];
                TempMaxHealth += LvlUpAddHealth[TempLvl];
                PAtack += LvlUpAddPAtack[TempLvl];
                PAtackSpeed += LvlUpAddPAtackSpeed[TempLvl];
                Speed += LvlUpAddSpeed[TempLvl];
                Lvl++;
                LvlUp.Invoke();
            }
        }
    }
    // Проверка опыта и поднятие лвла если накоплено достаточное количество опыта.

    void CacnelUsedSkillOnRightCklick()
    {
        Skill1.DisableSkill();
    }

    // Геттеры - сеттеры.
    public bool GetLifePayer()
    {
        return Life;
    }

 }

