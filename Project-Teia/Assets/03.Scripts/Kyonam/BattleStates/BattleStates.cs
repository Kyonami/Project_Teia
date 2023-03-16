using UnityEngine;
using System.Text;

public abstract class BattleState
{
    public abstract void Enter(BattleStateMachine bsm);
    public abstract void Handle(BattleStateMachine bsm);
    public abstract void Exit(BattleStateMachine bsm);
}

public class WaitingState : BattleState
{
    public override void Enter(BattleStateMachine bsm)
    {
        if (bsm.GetCameraAction().GetCameraActionState() != CameraState.ZOOMOUT)
            bsm.GetCameraAction().PlayUnZoom();
    } 
    public override void Handle(BattleStateMachine bsm)
    {
        bsm.ChangeState(new PlayerTurnState());
    } 
    public override void Exit(BattleStateMachine bsm)
    {
    }
}
public class PlayerTurnState : BattleState
{
    private Fighter targetFighter;
    private float timeCount;

    public override void Enter(BattleStateMachine bsm)
    {   
        if (bsm.GetCameraAction().GetCameraActionState() != CameraState.ZOOMINTOPLAYER)
            bsm.GetCameraAction().PlayZoomToLife(true);

        timeCount = 0.0f;
        targetFighter = bsm.GetFighterInfo(C.PLAYER);
        targetFighter.ResetChoice();
        bsm.UpdateSkillImage();

        if (targetFighter.myInfo.isAuto)
            bsm.ChooseRandomly();
        else
            bsm.ShowChoicePanel();
    }
    public override void Handle(BattleStateMachine bsm)
    {
        if (targetFighter.MyChoice == Choices.NONE)
            return;

        if (timeCount == 0.0f)
        {
            bsm.ShowResultElements(targetFighter.GetMySentence());
            bsm.ShowResultPanel();
        }

        timeCount += Time.deltaTime;

        if(timeCount >= 2.0f)
            bsm.ChangeState(new EnemyTurnState());
    } 
    public override void Exit(BattleStateMachine bsm)
    {
        bsm.IncreaseTurnCount();
        bsm.GetCameraAction().CancelCameraAnimation();
    }
}
public class EnemyTurnState : BattleState
{
    private Fighter targetFighter;
    private float timeCount;

    public override void Enter(BattleStateMachine bsm)
    {
        if (bsm.GetCameraAction().GetCameraActionState() != CameraState.ZOOMINTOENEMY)
            bsm.GetCameraAction().PlayZoomToLife(false);

        timeCount = 0.0f;
        targetFighter = bsm.GetFighterInfo(C.ENEMY);
        targetFighter.ResetChoice();
        bsm.UpdateSkillImage();

        if (!targetFighter.myInfo.isAuto)
            bsm.ShowChoicePanel();

        else if (targetFighter.myInfo.isAuto)
            bsm.ChooseRandomly();
    }
    public override void Handle(BattleStateMachine bsm)
    {
        if (targetFighter.MyChoice == Choices.NONE)
            return;

        if (timeCount == 0.0f)
            bsm.ShowResultElements(targetFighter.GetMySentence());
         
        timeCount += Time.deltaTime;

        if (timeCount >= 0.5f)
            bsm.ChangeState(new CalculatingState());
    }
    public override void Exit(BattleStateMachine bsm)
    {
        bsm.InitializeTurnCount();
        bsm.GetCameraAction().CancelCameraAnimation();
        Mathf.Clamp(bsm.TurnCount, 0, C.NUMBER_OF_LIFE - 1);
    }
}
public class CalculatingState : BattleState
{
    float timeCount = 0.0f;
    bool flag, flag0;
    StringBuilder stringBuilder = new StringBuilder();

    public override void Enter(BattleStateMachine bsm)
    {
        if (bsm.GetCameraAction().GetCameraActionState() != CameraState.ZOOMOUT)
        {   // 카메라 줌 아웃
            bsm.GetCameraAction().CancelCameraAnimation();
            bsm.GetCameraAction().PlayUnZoom();
        }

        flag = true;

        bsm.ShowResultPanel();

        stringBuilder.Append(bsm.CompareLifeChoice());
    }
    public override void Handle(BattleStateMachine bsm)
    {
        timeCount += Time.deltaTime;

        if (timeCount >= 1.2f && flag)
        {
            bsm.ShowResultElements(stringBuilder.ToString());
            bsm.ApplyDamage();
            flag = false;
        }
        else if (timeCount >= 4.2f) {
            if (bsm.IsDead())
                bsm.ChangeState(new ResultingState());
            else
                bsm.ChangeState(new WaitingState());
        }
    }
    public override void Exit(BattleStateMachine bsm)
    {
        bsm.InitializeLifeAnim();
        stringBuilder.Remove(0, stringBuilder.Length);
        bsm.InitializeResultElements();
        bsm.GetCameraAction().CancelCameraAnimation();
    }
}
public class ResultingState : BattleState
{
    public override void Enter(BattleStateMachine bsm)
    {
        bsm.ShowEndPanel();
        bsm.Notify(EVENT.ENDBATTLE);
    }
    public override void Handle(BattleStateMachine bsm)
    {
        
    } 
    public override void Exit(BattleStateMachine bsm)
    {
    }
}           
