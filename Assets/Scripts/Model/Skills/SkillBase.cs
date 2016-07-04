using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public abstract class SkillBase : MonoBehaviour {

    public UnityEvent ActivateSkill;
    public UnityEvent UsedSkill;
    public UnityEvent DeactivateSkill;

    public PlayerCharacter SkillMaster;
    // Ссылка на обладателя скилла.

    public int LvlSkill;
    public int MaxLvlSkill;
    public float ReusedTime;
    protected float TempReusedTime;
    // Временная для счетсчика кулдауна скиллов.

    public float[] DamageLvl;
    public float[] RangeLvl;

    protected bool ActivatedSkill;
    // Скилл активирован.
    protected bool SkillRefresh;
    // Откатился ли скилл.

    protected GameObject ParentObject;
    // Переменная для хранения ссылки на родительский обьект.
    protected CapsuleCollider ColliderSkillObject;
    protected Renderer RendererSkillObject;
    // Переменные для включения выключения активности и ивзуализации компонетов.

    protected Vector3 DestinationPointSkill;
    // Конечная точка.
    protected Vector3 DirectionVectorSkill;
    // Направление.
    protected Vector3 StartPosition;
    // Стартовая позиция.

    protected bool SkillReady;
    protected bool SkillUsed;
    // Юзается ли скилл.



    public void ActivateadSkillOn()
    {
        if (!SkillRefresh)
        {
            ActivatedSkill = true;
        }
    }

    public abstract void SkillWaitForUsed();
    // Активирование скилла и ожидание клика мышкой или отмены юзания скилла.

    public void RefreshSkill()
    {
        if (SkillRefresh)
        {
            ReusedTime -= Time.deltaTime;
            if (ReusedTime < 1)
            {
                SkillRefresh = false;
                ReusedTime = TempReusedTime;
            }
        }
    }
    // Откат скила.

    public void EnableVisibilitySkillObject()
    {
        if (ColliderSkillObject)
        {
            ColliderSkillObject.enabled = true;
        }
        RendererSkillObject.enabled = true;

    }
    // Включение визуализации демейдж - обьекта и его коллайдера.

    public void DisableSkill()
    {
        if (!SkillUsed)
        {
            if (ColliderSkillObject)
            {
                ColliderSkillObject.enabled = false;
            }
            RendererSkillObject.enabled = false;
            transform.parent = ParentObject.transform;
            transform.localPosition = StartPosition;
        }
        SkillReady = true;
        ActivatedSkill = false;
        GameController.SkillUsedNow = false;
    }
    // Выключение визуализации демейдж - обьекта и его коллайдера.

    public void SkillLvlUp()
    {
        if ((LvlSkill+1) <= MaxLvlSkill)
        {
            LvlSkill++;
        }
    }
    // Поднятие уровня скилла.

    public abstract void UsedSkillNow();


    // Геттеры - сеттеры.

    public bool GetRefreshSkillBool()
    {
        return SkillRefresh;
    }
    // Возвращает в открате скилл или нет.
}
