using _00.Work.CDH.Code.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Core
{
    [CreateAssetMenu(fileName = "ObjectFinder", menuName = "SO/ObjectFinder", order = 0)]
    public class ObjectFinderSO : ScriptableObject
    {
        [field: SerializeField] public string TargetTag { get; private set; } = "Player";

        public GameObject Object { get; private set; } = null;

        public GameObject FindObject()
        {
            if (string.IsNullOrEmpty(TargetTag))
                return null;

            GameObject targetObject = GameObject.FindGameObjectWithTag(TargetTag);

            if (targetObject == null)
                Debug.LogWarning("Cant Find Object. tag name : " + TargetTag);

            return targetObject;
        }

        public void SetObject(GameObject obj)
            => Object = obj;

        public T GetObject<T>() where T : Component
            => Object.TryGetComponent<T>(out T component) ? component : null;

        public bool GetObject<T>(out T @object) where T : Component
        {
            bool success = Object.TryGetComponent<T>(out T component);

            if (success)
                @object = component;
            else
                @object = null;

            return success;
        }
    }
}
