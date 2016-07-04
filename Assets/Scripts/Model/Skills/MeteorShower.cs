using UnityEngine;
using System.Collections;
using System;

public class MeteorShower : SkillBase{

    LayerMask IgnoreMask1;

    public float[] RadiusDamage;

    public ParticleSystem Meteors;
    // Мтеоритный дождь.

    Vector3 MeteorParticleSystemPosition;
    GameObject ParentObjectForParticleSystem;

    void Start ()
    {
        IgnoreMask1 = 223;
        // Игнорирование слоев конструкций и UI.

        SkillReady = true;
        SkillRefresh = false;
        TempReusedTime = ReusedTime;
        ParentObject = SkillMaster.gameObject;
        Meteors.enableEmission = false;
        MeteorParticleSystemPosition = Meteors.transform.localPosition;
        ParentObjectForParticleSystem = transform.gameObject;

        ActivateSkill.AddListener(ActivateadSkillOn);
        UsedSkill.AddListener(EnableVisibilitySkillObject);
        DeactivateSkill.AddListener(DisableSkill);

        StartPosition = new Vector3(0, 2f, 0);
        transform.localPosition = StartPosition;

        RendererSkillObject = transform.GetComponent<Renderer>();
        RendererSkillObject.enabled = false;

        transform.localScale = new Vector3(RadiusDamage[LvlSkill] * 0.1f, RadiusDamage[LvlSkill] * 0.1f, RadiusDamage[LvlSkill] * 0.1f);
    }


    void Update ()
    {
        SkillWaitForUsed();
        UsedSkillNow();
        RefreshSkill();
    }

    public override void SkillWaitForUsed()
    {
        if (ActivatedSkill && !SkillRefresh)
        {
            GameController.SkillUsedNow = true;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, IgnoreMask1))
            {
                transform.position = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
                transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                RendererSkillObject.enabled = true;
                // Рисуем область поражения вслед за движением мышки.
                if (Input.GetMouseButtonDown(0))
                {
                    DestinationPointSkill = hit.point;
                    ActivatedSkill = false;
                    SkillReady = false;
                    transform.parent = null;
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
                Meteors.transform.parent = null;
                SkillMaster.SetEndPosition(SkillMaster.transform.position);
                SkillReady = true;
                SkillUsed = true;
                UsedSkill.Invoke();
                SkillRefresh = true;
            }
        }

        if (SkillUsed)
        {
            StartCoroutine(MeteorShowerAnimation());
            RaycastHit Hit;
            Ray Ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            SkillUsed = false;
            if (Physics.Raycast(Ray1, out Hit, Mathf.Infinity))
            {
                Collider[] HitCollidaers = Physics.OverlapSphere(Hit.transform.position, RadiusDamage[LvlSkill]);
                int i = 0;
                while (i < HitCollidaers.Length)
                {
                    if (HitCollidaers[i].transform.tag == "Enemy")
                    {
                        HitCollidaers[i].GetComponent<Character>().Health -= DamageLvl[LvlSkill];
                    }
                    i++;
                }

                if (i == HitCollidaers.Length)
                {
                    SkillUsed = false;
                }
            }
        }
    }

    IEnumerator MeteorShowerAnimation()
    {
        Meteors.enableEmission = true;
        Meteors.loop = true;
        Meteors.Play();
        yield return new WaitForSeconds(2f);
        DeactivateSkill.Invoke();
        transform.parent = ParentObject.transform;
        Meteors.Stop();
        Meteors.loop = false;
        Meteors.enableEmission = false;
        yield return new WaitForSeconds(1f);
        Meteors.transform.parent = ParentObjectForParticleSystem.transform;
        Meteors.transform.localPosition = MeteorParticleSystemPosition;
    }
}
