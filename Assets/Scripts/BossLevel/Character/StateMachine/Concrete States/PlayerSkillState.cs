using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerState
{
    public override void EnterState(PlayerStateMachine stateMachine)
    {
        base.EnterState(stateMachine);

        if (!stateMachine.IsSkillActivated)
        {
            CustomDebug.Log("No Skill Activated", Color.cyan);
            stateMachine.EndTurn();
        }
        else
        {
            stateMachine.SetCurrentSkill();
            PlayerAnimationEvent.OnAnimationEnd.AddListener(FinishSkillAnimation);
            PlayAnimation(stateMachine);
        }
    }

    private void PlayAnimation(PlayerStateMachine stateMachine)
    {
        stateMachine.DisableAllAnimations();
        switch (stateMachine.CurrentSkill.SkillType)
        {
            case SkillType.Heal:
                stateMachine.SetBoolAnimator(AnimationStrings.isHealSkill, true);
                break;
            case SkillType.ComboDamage:
                stateMachine.SetBoolAnimator(AnimationStrings.isComboSkill, true);
                break;
            case SkillType.Freeze:
                stateMachine.SetBoolAnimator(AnimationStrings.isFreezeSkill, true);
                break;
            default:
                Debug.LogError("Unidentified skill type.");
                break;
        }
    }

    public void FinishSkillAnimation()
    {
        PlayerAnimationEvent.OnSkillAnimationEnd.Invoke();
    }

    public override void ExitState(PlayerStateMachine stateMachine)
    {
        base.ExitState(stateMachine);
        PlayerAnimationEvent.OnAnimationEnd.RemoveAllListeners();
    }
}
