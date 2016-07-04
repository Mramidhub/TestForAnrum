using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    public int TargetLayerMask;

    public GameObject ControlMark;
    protected Renderer ControlMarkRendereComponent;
    // Ссылка на марку контроля.

    protected bool TargetSearch;
    float CounterInt;
    public GameObject Radar;
    // Переменныедля счетстчика.

    protected bool InPlayerControl;
    // Переменна контроля обьекта игроком.

    protected GameObject Tartget;
    // Обьект цели.
    protected Character TargetCharacterComponent;
    // Компонент цели.

    protected bool AttakingOn;
    // Атакуеться ли цель.
    protected Animator AnimationChar;
    // Анимации.
    protected float TempHealth;
    // Для восставновление ХП обьекта после смерти.

    protected Vector3 EndPosition;
    // Точка назначения для метода MoveToTarget.

    public int Lvl;
    public float Health;
    public float TempMaxHealth;
    public float PAtack;
    public float PAtackSpeed;
    public float Speed;
    public float Armor;
    public float RangeAttack;
    public int Money;
    public int Expirience;
    // Статы.

    void Start ()
    {
        TargetSearch = false;
    }
	
    public void MoveToTarget(Vector3 EndPositionTemp )
    {
        if (Vector3.Distance(transform.position, EndPositionTemp) < 1.7f)
        {
            Idle();
        }
        else
        {
            Vector3 Direction = EndPositionTemp - transform.position;
            Direction = new Vector3(Direction.x, 0, Direction.z);
            Direction.Normalize();
            transform.LookAt(EndPositionTemp);
            if (Direction != new Vector3(0, 0, 0))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), Speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            }
            transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
            AnimationChar.SetBool("Running", true);
            AnimationChar.SetBool("Attacking", false);
        }
    }
    // Бег. Перемещение + проигрываение анимации бега.

    public void Idle()
    {
        AnimationChar.SetBool("Attacking", false);
        AnimationChar.SetBool("Running", false);
    }
    // Ожидание. Анимация ожидания.

    public void Attacking()
    {
        if (Tartget != null)
        {
            if (Tartget.active == true && TargetCharacterComponent.Health > 0)
            {
                if (Vector3.Distance(transform.position, Tartget.transform.position) > RangeAttack)
                {
                    MoveToTarget(Tartget.transform.position);
                }
                else
                {
                    AnimationChar.speed = PAtackSpeed;
                    AnimationChar.SetBool("Attacking", true);
                    AnimationChar.SetBool("Running", false);
                    //if (Tartget.active == true)
                    if (Tartget.activeInHierarchy)
                    {
                        transform.LookAt(Tartget.transform.position);
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Tartget.transform.position), Speed * Time.deltaTime);
                        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
                        float TempPAttack = (PAtack / 100) * (100 - TargetCharacterComponent.Armor);
                        // Высчитывается наносимый урон с учетом юрони цели.
                        TargetCharacterComponent.Health -= TempPAttack * PAtackSpeed * Time.deltaTime;
                    }
                }
            }
            else
            {
                if (transform.tag == "Player")
                {
                    Expirience += TargetCharacterComponent.Expirience;
                    Money += TargetCharacterComponent.Money;
                }
                EndPosition = transform.position;
                Tartget = null;
                AttakingOn = false;
            }
        }
    }
    // Атака цели. Анимация атаки, отнятие ХП у цели.

    public void AutoMove(string TargetTag)
    {
        if (Tartget != null)
        {
            AttakingOn = true;
        }
        else
        {
            TargetSearchCounter(TargetLayerMask);
        }
    }
    // Автоматическиое движение и определение целей.

    public void Dead()
    {
        if (Health < 0)
        {
            AttakingOn = false;
            ControlMarkDisable();
            Health = TempHealth;
            gameObject.SetActive(false);
            gameObject.transform.position = new Vector3(0, 0, 0);
            Tartget = null;
        }

    }
    // Смерть и возвращение в пул.

    public void SetTarget(Vector3 EndPosition1)
    {
        EndPosition = EndPosition1;
    }




    public void TargetSearchCounter(int LayerMask)
    {
        CounterInt += Time.deltaTime;
        if (CounterInt  >= 1.5)
        {
            NextTarget(LayerMask);
            CounterInt = 0;
        }
    }
    //Счетсчик для  задействования функции NextTarget

    public void NextTarget(int LayerMask)
    {
        RaycastHit hit;
        for (int i = 0; i < 70; i ++)
        {
            Radar.transform.Rotate(Radar.transform.rotation.x, Radar.transform.rotation.y + 5, Radar.transform.rotation.z);
            Ray ray = new Ray(Radar.transform.position,  Radar.transform.forward);
            if (Physics.Raycast(ray, out hit, 10f, LayerMask))
            {
                Tartget = hit.transform.gameObject;
                TargetCharacterComponent = hit.transform.GetComponent<Character>();
                if (InPlayerControl)
                {
                    EnableControlMarkOnTarget();
                }
                AttakingOn = true;
                CounterInt = 0;
                return;
            }
        }
    }
    // Автоматический выбор следующей цели.  Задействуется когда характер находится в автоматическом режиме.




    public void PlayerControlThis()
    {
        if (InPlayerControl == true)
        {
            AttakingOn = false;
            EndPosition = StaticData.DestionationPoint;
            Tartget = StaticData.Target;
        }
    }
    // Установка пункта назначения и цели. Прекрашение атаки.

    public void CancelPlayerCotrolThis()
    {
        InPlayerControl = false;
    }
    // Отмена контроля игрком миньиона. Подписана на событие в сотоянии NonSelectedState.

    public void AttackTarget()
    {
        AttakingOn = true;
        Tartget = StaticData.Target;
        TargetCharacterComponent = StaticData.TargetComponent;

    }
    // Метод подписавается на событие RightClickOnEmey в GameControllere.

    public void CancelAttackTarget()
    {
        AttakingOn = false;
    }
    // Отлючение режима атаки.

    public void ControlMarkEnable()
    {
        ControlMarkRendereComponent.enabled = true;
    }
    // Включение марки контроля.

    protected void ControlMarkDisable()
    {
        ControlMarkRendereComponent.enabled = false;
    }
    // Выключение марки контроля.



        // Геттеры - сеттеры.


    public void EnableControlMarkOnTarget()
    {
        if (Tartget != null)
        {
            TargetCharacterComponent.ControlMarkEnable();
        }
    }
    // Включение контроль-марки цели, которая взята при контроле миньона \ игрока.

    public void SetEndPosition(Vector3 Position)
    {
        EndPosition = Position;
    }
    // Назначение конечной точки назначения.

    public void SetControlPlayer(bool control)
    {
        InPlayerControl = control;
    }
    // Установка контроля на обьектом игроком.

    public Vector3 GetEndPosition()
    {
        return EndPosition;
    }

}

