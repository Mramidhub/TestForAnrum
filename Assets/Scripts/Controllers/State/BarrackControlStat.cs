using UnityEngine;
using System.Collections;
using System;

public class BarrackControlStat : StateController {


    void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public override void ControlState(GameController GameController1)
    {
        GameController1.ControlBarrack.Invoke();
        GameObject.Find("BarracksView").GetComponent<BarracksView>().ConcreteBarrack = GameController1.GetSelectedBarrack();
    }

}
