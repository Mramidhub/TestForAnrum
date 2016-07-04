using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class NonSelectedState : StateController {

    public delegate void  CancelPlayerControl();
    public static event CancelPlayerControl CancelAllControl;

    // Событие отмены контроля игроком всех миньонов и зданий.

    void Start ()
    {
       
	}

    public override void ControlState(GameController GameController)
    {
        CancelAllControl();
    }

    // Снимаем селект со всех обектов.

}
