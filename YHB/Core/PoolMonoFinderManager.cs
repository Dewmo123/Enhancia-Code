using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Core
{
    [DefaultExecutionOrder(-1)]
    public class PoolMonoFinderManager : MonoBehaviour
    {
        [SerializeField] private ObjectFinderSO poolMonoFinder;

        [Inject] private PoolManagerMono _poolMono;

        private void Awake()
        {
            poolMonoFinder.SetObject(_poolMono.gameObject);
        }
    }
}
