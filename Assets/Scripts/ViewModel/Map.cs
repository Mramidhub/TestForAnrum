using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public PoolObjects ObjectPool;
    public PlayerCharacter Player;
    public GameObject PointMarkMinion;
    public GameObject PointMarkEnemy;
    public GameObject PointMarkPlayer;

    bool StartShowPositions;
    bool StartPooling;

    public int PoolCountMinion;
    public int PoolCountEnemy;
    // Размеры пулов.

    public List<GameObject> PoolPointMinion;
    public List<GameObject> PoolPointEnemy;


    void Start ()
    {
        PoolCountMinion = 34;
        PoolCountEnemy = 34;
        StartPoolMark(PoolCountEnemy, PoolPointEnemy, PointMarkEnemy);
        StartPoolMark(PoolCountMinion, PoolPointMinion, PointMarkMinion);
    }
	

	void Update ()
    {
        PointPositionShow(PoolPointEnemy, ObjectPool.Enemys);
        PointPositionShow(PoolPointMinion, ObjectPool.Minions);
        PlayerPointShow();
    }


    void StartPoolMark(int Count, List<GameObject> Pool, GameObject MarkPoint)
    {
        for (int i = 0; i < Count; i++)
        {
            GameObject obj = Instantiate(MarkPoint, transform.position, Quaternion.identity) as GameObject;
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);
            obj.transform.localScale = new Vector3(1f, 1f, 1f);
            Pool.Add(obj);

            if ((i - 1) < PoolCountMinion)
            {
                StartShowPositions = true;
            }

        }
    }
    // Заполняем пул точек поизций.

    void PointPositionShow(List<GameObject> Pool, List<GameObject> CharacterPool)
    {
        if (StartShowPositions)
        {
            for (int i = 0; i < CharacterPool.Count; i++)
            {
                if (CharacterPool[i].activeInHierarchy)
                {
                    Pool[i].SetActive(true);
                    Pool[i].transform.localPosition = new Vector3(CharacterPool[i].transform.position.x - 64f, CharacterPool[i].transform.position.z - 64f, 0);
                }
                else
                {
                    Pool[i].SetActive(false);
                }
            }
        }

    }
    // Отрисовка точек позиций на карте.

    void PlayerPointShow()
    {
        if (StartShowPositions)
        {
            if (Player.GetLifePayer())
            {
                PointMarkPlayer.SetActive(true);
                PointMarkPlayer.transform.localPosition = new Vector3(Player.transform.position.x - 64f, Player.transform.position.z - 64f, 0);
            }
            else
            {
                PointMarkPlayer.SetActive(false);
            }
        }
    }
    // Отрисовка точки персонажа.

}
