using UnityEngine;
using System.Collections;

public class MinionCharacter : Character {


    void Start ()
    {
        ControlMarkRendereComponent = ControlMark.GetComponent<Renderer>();
        NonSelectedState.CancelAllControl += CancelPlayerCotrolThis;
        NonSelectedState.CancelAllControl += ControlMarkDisable;
        // Событие снятия контроля игроком со всех.
        GameObject.Find("GameController").GetComponent<GameController>().LeftClickOnTerrain.AddListener(PlayerControlThis);
        GameObject.Find("GameController").GetComponent<GameController>().NoEnemyOnMap.AddListener(RefreshPosition);
        GameObject.Find("GameController").GetComponent<GameController>().EnemyOnMap.AddListener(RefreshPosition);
        // Подпись на нужные миньонам события.
        TempHealth = Health;
        TempMaxHealth = Health;
        TargetSearch = false;
        AnimationChar = GetComponent<Animator>();
    }


    void Update ()
    {
        if (AttakingOn == false) 
        {
            if (!InPlayerControl)
            {
                TargetSearchCounter(TargetLayerMask);
            }
            MoveToTarget(EndPosition);
        }
        else
        {
            Attacking();
        }
        Dead();
    }

    void RefreshPosition()
    {
        EndPosition = StaticData.DestionationPoint;
    }
    // Обновление  точки назначения. Подписана на наличие \ отсутствие мобов.
}
