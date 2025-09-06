using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Core
{
    [DefaultExecutionOrder(-10)]
    public class FinderManager : MonoBehaviour
    {
        [SerializeField] private List<ObjectFinderSO> objectFinderSOList;

        private void Awake()
        {
            foreach (ObjectFinderSO finders in objectFinderSOList)
            {
                GameObject targetObject = finders.FindObject();
                Debug.Assert(targetObject != null, $"Can't find object. target tag is {finders.TargetTag}");
                finders.SetObject(targetObject);
            }
        }
    }
}
