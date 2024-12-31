using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectApplier
{
    public static void ApplyEffect(CharacterType characterType, Dictionary<BallColor, int> ballDestroyed)
    {
        Dictionary<EffectType, int> userChangedEffects = new Dictionary<EffectType, int>();
        Dictionary<EffectType, int> enemyChangedEffects = new Dictionary<EffectType, int>();

        foreach (KeyValuePair<BallColor, int> kvp in ballDestroyed)
        {
            int stack = kvp.Value / DefaultStats.EFFECT_BallNumForAStack;
            if (characterType == CharacterType.Player)
            {
                BossEvent.OnPlayerRegenMana.Invoke(kvp.Value);
            }
            else BossEvent.OnBossRegenMana.Invoke(kvp.Value);

            switch (kvp.Key)
            {
                //RED: Attack Buff
                case BallColor.Red:
                    userChangedEffects.Add(EffectType.AttackBuff, Mathf.Min(stack, DefaultStats.EFFECT_MaxStatStack));
                    Debug.Log("-Attack buff + " + stack);
                    break;
                //PINK: Attack Debuff enemy
                case BallColor.Pink:
                    enemyChangedEffects.Add(EffectType.AttackDebuff, Mathf.Min(stack, DefaultStats.EFFECT_MaxStatStack));
                    Debug.Log("-Attack debuff + " + stack);
                    break;
                //ORANGE: Defense Buff
                case BallColor.Orange:
                    userChangedEffects.Add(EffectType.DefenseBuff, stack);
                    Debug.Log("-Defense buff + " + stack);
                    break;
                //YELLOW: Defense Debuff Enemy
                case BallColor.Yellow:
                    enemyChangedEffects.Add(EffectType.DefenseDebuff, Mathf.Min(stack, DefaultStats.EFFECT_MaxStatStack));
                    Debug.Log("-Defense debuff + " + stack);
                    break;
                //GREEN: Heal
                case BallColor.Green:
                    if (characterType == CharacterType.Player)
                    {
                        BossEvent.OnPlayerHeal.Invoke(Mathf.Min(stack, DefaultStats.EFFECT_MaxStatStack));
                    }
                    else BossEvent.OnBossHeal.Invoke(Mathf.Min(stack, DefaultStats.EFFECT_MaxStatStack));
                    Debug.Log("-Heal + " + stack);
                    break;
                //CYAN: Crit
                case BallColor.Blue:
                    userChangedEffects.Add(EffectType.Crit, 1);
                    Debug.Log("-Crit + " + 1);
                    break;
                //BLUE: Shield
                case BallColor.Cyan:
                    userChangedEffects.Add(EffectType.Shield, stack);
                    Debug.Log("-Shield + " + stack);
                    break;
                //Violet: Dot on enemy
                case BallColor.Violet:
                    enemyChangedEffects.Add(EffectType.DOT, DefaultStats.EFFECT_MaxDotStack);
                    Debug.Log("-DOT");
                    break;
            }
        }
        //Update Effects on Player & Boss
        BossEvent.OnPlayerReceiveEffect.Invoke(characterType == CharacterType.Player ? userChangedEffects : enemyChangedEffects);
        BossEvent.OnBossReceiveEffect.Invoke(characterType == CharacterType.Boss ? userChangedEffects : enemyChangedEffects);
    }
}
