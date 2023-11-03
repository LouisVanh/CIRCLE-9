using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIAnimationHelper : MonoBehaviour
{
    public bool DeathAnimationEnd = false;
    private EnemyAI _enemyAI;
    private void Start()
    {
        _enemyAI = GetComponentInParent<EnemyAI>();
    }
    private void DeathAnimationStopped()
    {
        //called in animation
        DeathAnimationEnd = true;
    }
    private void PlayDeathSound()
    {
        //called in animation
        _enemyAI.PlayDeathSound();
    }
    private void PlayRandomGrowlSound()
    {
        //called in animation
        _enemyAI.PlayRandomAtackSound();
    }
}
