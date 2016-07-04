using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PoolObjects : MonoBehaviour {

    string NamePrefab;
    GameObject Temp;
    // Для кэша.

    Vector3 NoActivePosition;
    //Позиционирование обьектов пула.

    GameObject ParentMinion;
    GameObject ParentEnemy;
    // Обьекты родители для пула для упорядочивания.

    public GameObject Enemy1Prefab;
    public GameObject Enemy2Prefab;
    public GameObject Enemy3Prefab;
    public GameObject Minion1Prefab;
    public GameObject Minion2Prefab;
    public GameObject Minion3Prefab;
    // Префабы обьектов для пула.

    public GameObject Player;
    // Префаб игрока.


    public int PoolCountMinion1;
    public int PoolCountMinion2;
    public int PoolCountEnemy1;
    public int PoolCountEnemy2;
    public int PoolCountEnemy3;
    // Конкретное коллличество создаваемых в пул конкретных юнитов.

    public List<GameObject> Enemys;
    public List<GameObject> Minions;
    // Пул для Енеми и Миньонов.

    public List<MinionCharacter> ComponentMinionCharPool;
    public List<EnemyCharacter> ComponentEnemyCharPool;
    // Пул Компонентов Миньонов.


    private PoolObjects() { }
    // Закрываем коснтруктор по умолчанию во избежание.

    void Start ()
    {
        ParentMinion = new GameObject();
        ParentEnemy = new GameObject();
        ParentMinion.name = "Minions";
        ParentEnemy.name = "Enemys";
        // Создаем родительские обьекты для пулов и обзываем их соответствующе.

        StartPool(Enemys, PoolCountEnemy1, Enemy1Prefab);
        StartPool(Minions, PoolCountMinion1, Minion1Prefab);
        StartPool(Enemys, PoolCountEnemy2, Enemy2Prefab);
        StartPool(Minions, PoolCountMinion2, Minion2Prefab);
        StartPool(Enemys, PoolCountEnemy3, Enemy3Prefab);
        PoolComponentMinion();
        // Заполняем нужные пулы.

        NoActivePosition = new Vector3(0, 0, 0);
        Temp = new GameObject();
    }


	void Update ()
    {
	
	}


    void  StartPool(List<GameObject> Pool, int PoolCount, GameObject PrefabObjectForPooling)
    {
        NamePrefab = PrefabObjectForPooling.tag;

        for (int i = 0; i < PoolCount; i++)
        {
            GameObject obj = (GameObject)Instantiate(PrefabObjectForPooling);
            obj.name = PrefabObjectForPooling.name;
            // Выставляем нормальное имя юниту.
            obj.SetActive(false);
            Pool.Add(obj);
            obj.transform.position = NoActivePosition;
            if (NamePrefab == "Enemy")
            {
                obj.transform.parent = ParentEnemy.transform;
            }
            else if (NamePrefab == "Minion")
            {
                obj.transform.parent = ParentMinion.transform;

            }
        }

    }
    // Заполнение пула.

    public GameObject GetObjectsFromPool(string NameObject, List<GameObject>PoolObject)
    {
        for (int i = 0; i < PoolObject.Count; i++)
        {
            if (PoolObject[i].activeInHierarchy == false && PoolObject[i].transform.name == NameObject)
            {
                PoolObject[i].gameObject.SetActive(true);
                Temp = PoolObject[i].gameObject;
                return Temp;
            }
        }
        return Temp;
    }
    // Взятие обьекта из пула.

    void PoolComponentMinion()
    {
        for (int i = 0; i < Minions.Count; i++)
        {
            ComponentMinionCharPool.Add(Minions[i].GetComponent<MinionCharacter>()); 
        }
    }
    // Заполнение пула ссылками на компонент миньонов. Для исопльзования групового выделения.

    void PoolComponentEnemy()
    {
        for (int i = 0; i < Minions.Count; i++)
        {
            ComponentMinionCharPool.Add(Minions[i].GetComponent<MinionCharacter>());
        }
    }
    // Заполнение пула ссылками на компонент миньонов. Для исопльзования групового выделения.

}
