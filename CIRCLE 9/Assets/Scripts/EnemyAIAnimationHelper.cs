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
       DeathAnimationEnd = true;
    }
    private void PlayDeathSound()
    {
        _enemyAI.PlayDeathSound();
    }
    private void PlayRandomGrowlSound()
    {
        _enemyAI.PlayRandomAtackSound();
    }
}
