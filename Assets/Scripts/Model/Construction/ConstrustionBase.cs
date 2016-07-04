using UnityEngine;
using System.Collections;

public abstract class ConstrustionBase : MonoBehaviour
{
    public int Lvl;
    // Уровнеь здания.

    protected float speedTrainig;

    protected int Counter1;
    // Счетчик для очреди;

    protected float TrainingTime;
    // Время тренировки юнита.

    public GameObject ObjectPool;
    // Пул обьектов.

    public string NameMinion1;
    // Имена миньонов, которые будут таскаться из пула и улучшаться при лвлапе здания.

    protected GameObject TempCharObject;
    // Временный ГО.



    // Геттеры - сеттеры.
    public void IncreaceCounter1()
    {
        Counter1++; ;
    }
    // Увеличение значения счетсчика.

    public int GetCounter1Volue()
    {
        return Counter1;
    }
    // Возвращает значние сетсчика.


}