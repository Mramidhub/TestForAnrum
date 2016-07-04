using UnityEngine;
using System.Collections;

public class EnemyCharacter : Character
{
    public Vector3 EnemyEndPosition;

    void Start()
    {

        NonSelectedState.CancelAllControl += ControlMarkDisable;
        ControlMarkRendereComponent = ControlMark.GetComponent<Renderer>();
        TempHealth = Health;
        TempMaxHealth = Health;
        EndPosition = EnemyEndPosition;
        TargetSearch = false;
        AttakingOn = false;
        AnimationChar = GetComponent<Animator>();
    }


    void Update()
    {
        
        if (AttakingOn == false)
        {
            TargetSearchCounter(TargetLayerMask);
            EndPosition = EnemyEndPosition;
            MoveToTarget(EndPosition);
        }
        else
        {
            Attacking();
        }
        Dead();
    }

    //Счетсчик для  задействования функции NextTarget

}