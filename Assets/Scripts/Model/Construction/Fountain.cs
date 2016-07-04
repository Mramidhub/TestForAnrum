using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fountain : MonoBehaviour
{
    List<Character> HealingUnit;
    // Список тех кого фонтан будет лечить.
    public float Heal;
    // Размер восстанавливаемого фонтаном здоровья в один отрезок времени.
    int HealingObjectCounter;
    // Счетсчик для  перебора коллекции.
    public float TimerHeal;
    // Время, частота восстановления.
    float TempTimerHeal;

    void Start ()
    {
        HealingObjectCounter = 0;
        TempTimerHeal = TimerHeal;
        HealingUnit = new List<Character>();

	}
	

	void Update ()
    {
        Healing();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Character>() != null && other.transform.tag != "Enemys")
        HealingUnit.Add(other.GetComponent<Character>());
    }
    // Добавляем того, кто зашел в коллайдер фонтана в коллекцию для лечения.

    void OnTriggerExit(Collider other)
    {
        HealingUnit.Remove(other.GetComponent<Character>());
    }
    // ДУбираем того, кто вышел из коллайдер фонтана в коллекцию для лечения.

    void Healing()
    {
        if (HealingUnit != null)
        {
            TimerHeal -= Time.deltaTime;
            if (TimerHeal < 0)
            {
                if (HealingObjectCounter < HealingUnit.Count)
                {
                    if ((HealingUnit[HealingObjectCounter].Health + Heal) < HealingUnit[HealingObjectCounter].TempMaxHealth)
                    {
                        HealingUnit[HealingObjectCounter].Health += Heal;
                        HealingObjectCounter++;
                    }
                    else
                    {
                        HealingUnit[HealingObjectCounter].Health = HealingUnit[HealingObjectCounter].TempMaxHealth;
                        HealingObjectCounter++;
                    }
                }
                else
                {
                    TimerHeal = TempTimerHeal;
                    HealingObjectCounter = 0;
                }
            }
        }
    } 
    // Восстановление здоровьях тех , кто зашел в коллайдер фонтана.
}
