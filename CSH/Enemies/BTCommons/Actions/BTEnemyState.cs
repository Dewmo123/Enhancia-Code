using Unity.Behavior;
using UnityEngine;

namespace _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions
{
    [BlackboardEnum]
    public enum BTEnemyState
    {
        PATROL, REACT, CHASE, ATTACK, STUN, HIT, DEATH
    }
}
