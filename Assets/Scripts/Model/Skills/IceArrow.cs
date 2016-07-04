using UnityEngine;
using System.Collections;

public class IceArrow : SkillBase {

    public float[] DecreaseSpeed;
    public float[] DecreaseAttackSpeed;
    // Понижение характеристики атакуемого.

    public string DamageObjectsTag;
    // Тег обьектов, которым скилл наносит урон.
    public float SpeedSkillObject;
    // Скорость движения обьекта, который по логике скилла должен двигаться. В данном случае стрелы.

    Character DamageTargetCharacterComponent;
    // Комопнент цели, в которую попал демейдж-обьект.

	void Start ()
    {
        SkillReady = true;
        SkillRefresh = false;
        TempReusedTime = ReusedTime;
        ParentObject = transform.parent.gameObject;

        ActivateSkill.AddListener(ActivateadSkillOn);
        UsedSkill.AddListener(EnableVisibilitySkillObject);
        DeactivateSkill.AddListener(DisableSkill);

        StartPosition = new Vector3(0, 2f, 0);
        transform.localPosition = StartPosition; 

        ColliderSkillObject = transform.GetComponent<CapsuleCollider>();
        ColliderSkillObject.enabled = false;

        RendererSkillObject = transform.GetComponent<Renderer>();
        RendererSkillObject.enabled = false;
    }
	

	void Update ()
    {
        SkillWaitForUsed();
        UsedSkillNow();
        RefreshSkill();
    }

    void OnTriggerEnter(Collider other)
    {
        if (SkillUsed)
        {
            if (other.transform.tag == DamageObjectsTag)
            {
                DamageTargetCharacterComponent = GetCharacterComponent(other.gameObject);

                DamageTargetCharacterComponent.Health -= DamageLvl[LvlSkill];
                DamageTargetCharacterComponent.Speed -= DecreaseSpeed[LvlSkill];
                DamageTargetCharacterComponent.PAtackSpeed -= DecreaseAttackSpeed[LvlSkill];
                SkillUsed = false;
                DeactivateSkill.Invoke();
                transform.parent = ParentObject.transform;
                transform.localPosition = StartPosition;
                GameController.SkillUsedNow = false;

            }
        }
    }
    // Если скилл юзается проверят коллайдеры попавшиеся на пути и если встречаеться нужный, то производит
    // нужный эффект. За тем отключается скилл.

    public override void SkillWaitForUsed()
    {
        if (ActivatedSkill && !SkillRefresh)
        {
            GameController.SkillUsedNow = true;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    DestinationPointSkill = hit.point;
                    ActivatedSkill = false;
                    DirectionVectorSkill = DestinationPointSkill - transform.position;
                    SkillReady = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ActivatedSkill = false;
                DeactivateSkill.Invoke();
                transform.parent = ParentObject.transform;
                transform.localPosition = StartPosition;
                GameController.SkillUsedNow = false;
            }
        }
    }
    // Активирование скилла и ожидание клика мышкой или отмены юзания скилла.

    public override void UsedSkillNow()
    {
        if (!SkillReady)
        {
            if (Vector3.Distance(SkillMaster.transform.position, DestinationPointSkill) > RangeLvl[LvlSkill])
            {
                SkillMaster.SetEndPosition(DestinationPointSkill);
            }
            else
            {
                SkillMaster.SetEndPosition(SkillMaster.transform.position);
                SkillReady = true;
                SkillUsed = true;
                transform.parent = null;
                UsedSkill.Invoke();
                DirectionVectorSkill = DestinationPointSkill - transform.position;
                DirectionVectorSkill.Normalize();
                transform.LookAt(DestinationPointSkill);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(DirectionVectorSkill), SpeedSkillObject * Time.deltaTime);
                transform.rotation = Quaternion.Euler(90f, -90 + transform.rotation.eulerAngles.y, 0f);
                SkillRefresh = true;
            }
        }

        if (SkillUsed)
        {
            if (Vector3.Distance(transform.position, DestinationPointSkill) > 1)
            {
                transform.Translate(DirectionVectorSkill * SpeedSkillObject * Time.deltaTime, Space.World);
            }
            else
            {
                SkillUsed = false;
                DeactivateSkill.Invoke();
                transform.parent = ParentObject.transform;
                transform.localPosition = StartPosition;
                GameController.SkillUsedNow = false;
            }
        }
    }
    // Если скилл запушен, то двигает демейдж-обьект в нужном направлении. Если долетел до места и ни 
    // в кого не попал, то деактивация скила.

    Character GetCharacterComponent(GameObject obj)
    {
        return obj.GetComponent<Character>();
    }
    // Возвращает Character.

}
