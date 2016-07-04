using UnityEngine;
using System.Collections;
using System;

public class PlayerControlState : StateController {

    PlayerCharacter PlayerCharacterComponent;

    public override void ControlState(GameController GameController1)
    {
        PlayerCharacterComponent = GameController1.GetPlayerComponent();
        // Получаем комонент игрока.

        PlayerCharacterComponent.SetControlPlayer(true);
        PlayerCharacterComponent.ControlMarkEnable();
        PlayerCharacterComponent.EnableControlMarkOnTarget();
        GameController1.RightClickOnEnemy.RemoveAllListeners();
        GameController1.RightClickOnEnemy.AddListener(PlayerCharacterComponent.EnableControlMarkOnTarget);
        GameController1.RightClickOnEnemy.AddListener(PlayerCharacterComponent.AttackTarget);
        GameController1.LeftClickOnTerrain.AddListener(PlayerCharacterComponent.PlayerControlThis);
        GameController1.LeftClickOnTerrain.AddListener(PlayerCharacterComponent.Skill1.DisableSkill);
        GameController1.LeftClickOnTerrain.AddListener(PlayerCharacterComponent.Skill2.DisableSkill);
        GameController1.ControlPlayer.Invoke();
        // Выставляем булевую контроля обьекта игроком и пописываем на событие нажатия правой кнопкой на враге.
    }
}
