using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Barrack : ConstrustionBase
{
    public GameObject Minion1Prefab;
    MinionCharacter ComponentMinionCharacter;
    // Префабы производимых юнитов. Для повышения характеристик мобов при подняти лвл здания.

    public int Minion1Price;
    // Цена юнита.

    public int[] ContructionUpPrice;
    // Стоимость улучшения здания.
    public float[] AddHealth;
    public float[] AddPAtack;
    public float[] AddPAtackSpeed;
    public float[] AddSpeed;
    public float[] AddArmor;
    // Характеристики, которые добавяться к характристикам производимых зданием юнитов.


    void Start ()
    {
        ComponentMinionCharacter = Minion1Prefab.GetComponent<MinionCharacter>();
        Counter1 = 0;
        TrainingTime = 4;
    }

	void Update ()
    {
        TrainingMinion1(NameMinion1);
    }

    public  void TrainingMinion1(string NameMob)
    {
        if (Counter1 > 0)
        {
            TrainingTime -= Time.deltaTime;
            if (TrainingTime < 0)
            {
                TempCharObject = ObjectPool.GetComponent<PoolObjects>().GetObjectsFromPool(NameMob, ObjectPool.GetComponent<PoolObjects>().Minions);
                TempCharObject.transform.position = new Vector3(transform.position.x - 5f, 0f, transform.position.z - 5f);
                TempCharObject.GetComponent<Character>().SetEndPosition(TempCharObject.transform.position);
                Counter1--;
                TrainingTime = 4;
            }
        }
    }
    // Взятие из пула нужного моба. Проверка на наличие у игрока денег. Если нет, то статическое событие NoMoney.

    public void LvlUpContsruction()
    {
        PoolObjects TempPoolComponent =  ObjectPool.GetComponent<PoolObjects>();

        for (int j = 0; j < TempPoolComponent.ComponentMinionCharPool.Count; j++)
        {
            if (TempPoolComponent.Minions[j].gameObject.name == NameMinion1)
            {
                TempPoolComponent.ComponentMinionCharPool[j].Health += AddHealth[Lvl];
                TempPoolComponent.ComponentMinionCharPool[j].PAtack += AddPAtack[Lvl];
                TempPoolComponent.ComponentMinionCharPool[j].PAtackSpeed += AddPAtackSpeed[Lvl];
                TempPoolComponent.ComponentMinionCharPool[j].Speed += AddSpeed[Lvl];
                TempPoolComponent.ComponentMinionCharPool[j].Armor += AddArmor[Lvl];
            }
        }
        // Перебираем колекцию компонентов Миньонов из пула и меняем характеристики.

        Lvl++;
        ComponentMinionCharacter.Health += AddHealth[Lvl];
        ComponentMinionCharacter.PAtack += AddPAtack[Lvl];
        ComponentMinionCharacter.PAtackSpeed += AddPAtackSpeed[Lvl];
        ComponentMinionCharacter.Speed += AddSpeed[Lvl];
        ComponentMinionCharacter.Armor += AddArmor[Lvl];
        // Меняем характеристики префаба Миньона.
    }
}
