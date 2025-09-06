using _00.Work.CDH.Code.Entities;
using UnityEngine;

namespace _00.Work.CSH._01.Scripts.Enemies
{
    [CreateAssetMenu(fileName = "EntityFinder", menuName = "SO/EntityFinderSO")]
    public class EntityFinderSO : ScriptableObject
    {
        [SerializeField] private string targetTag;
        public Entity target;

        private void OnEnable()
        {
            Debug.Log("¿¿æ÷ πŸ≤„ ¿¿æ÷ ª—ø°ø®");
            if (string.IsNullOrEmpty(targetTag)) return;
            GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
            Debug.Log(targetObject);
            if (targetObject != null)
            {
                target = targetObject.GetComponent<Entity>();

            }
        }

        public void SetEntity(Entity entity)
        {
            target = entity;
        }
    }
}
