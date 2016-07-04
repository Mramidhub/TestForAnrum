using UnityEngine;
using System.Collections;
using System;

public class Portal : ConstrustionBase
{
    public float[] SpawnEnemy1Time;
    public float[] SpawnEnemy2Time;
    public float[] SpawnEnemy3Time;
    float TempSpawnEnemy1Time;
    float TempSpawnEnemy2Time;
    float TempSpawnEnemy3Time;

    public String NameEnemy1;
    public String NameEnemy2;
    public String NameEnemy3;

    bool SpawnMobsOn;

    GameController GameControllerComponent;

    void Start ()
    {
        SpawnMobsOn = false;
        GameControllerComponent = GameObject.Find("GameController").GetComponent<GameController>();
        GameControllerComponent.StartWave.AddListener(SpawnStart);
        GameControllerComponent.StartPause.AddListener(SpawnStop);
    }
	

	void Update ()
    {
        SpawnUnit();
    }

    void SpawnUnit()
    {
        if (SpawnMobsOn)
        {
            if (TempSpawnEnemy1Time > 0)
            {
                TempSpawnEnemy1Time -= Time.deltaTime;
            }
            else
            {
                TrainingMinion1(NameEnemy1);
                TempSpawnEnemy1Time = SpawnEnemy1Time[GameControllerComponent.LvlWave];
            }

            if (TempSpawnEnemy2Time > 0)
            {
                TempSpawnEnemy2Time -= Time.deltaTime;
            }
            else
            {
                TrainingMinion1(NameEnemy2);
                TempSpawnEnemy2Time = SpawnEnemy2Time[GameControllerComponent.LvlWave];
            }

            if (TempSpawnEnemy3Time > 0)
            {
                TempSpawnEnemy3Time -= Time.deltaTime;
            }
            else
            {
                TrainingMinion1(NameEnemy3);
                TempSpawnEnemy3Time = SpawnEnemy3Time[GameControllerComponent.LvlWave];
            }
        }
    }
    // Функция респавна юнитов.

    public  void TrainingMinion1(string NameMob)
    {
        TempCharObject = ObjectPool.GetComponent<PoolObjects>().GetObjectsFromPool(NameMob, ObjectPool.GetComponent<PoolObjects>().Enemys);
        TempCharObject.transform.position = new Vector3(transform.position.x - 5f, 0, transform.position.z - 5f);
        TempCharObject.GetComponent<Character>().SetEndPosition(TempCharObject.transform.position);
    }
    // Взятие юнита их пула.

    void SpawnStart()
    {
        SpawnMobsOn = true;
    }
    // Подписана на событие в GameController.
    void SpawnStop()
    {
        SpawnMobsOn = false;
    }
    // Подписана на событие в GameController.
    
}
