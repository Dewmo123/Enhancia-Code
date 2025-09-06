using DewmoLib.ObjectPool.RunTime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Stages
{
    [Serializable]
    public struct EnemySpawnInfomation
    {
        public PoolItemSO enemyPoolItem;
        public sbyte spawnFrequency;
    }
}