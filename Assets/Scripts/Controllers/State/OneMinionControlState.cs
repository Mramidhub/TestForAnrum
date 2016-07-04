using UnityEngine;
using System.Collections;
using System;

public class OneMinionControlState : StateController {

    MinionCharacter MinionComponent;

    public override void ControlState(GameController GameController1)
    {
        MinionComponent = GameController1.GetMinionComponent();
        // Получаем компонент миньона.

        GameController1.RightClickOnEnemy.RemoveAllListeners();
        MinionComponent.SetControlPlayer(true);
        MinionComponent.ControlMarkEnable();
        MinionComponent.EnableControlMarkOnTarget();

        GameController1.RightClickOnEnemy.AddListener(MinionComponent.AttackTarget);
        GameController1.RightClickOnEnemy.AddListener(MinionComponent.EnableControlMarkOnTarget);
        GameController1.LeftClickOnTerrain.AddListener(MinionComponent.PlayerControlThis);
        // Выставляем булевую контроля обьекта игроком и пописываем на событие нажатия правой кнопкой на враге.
    }
}

