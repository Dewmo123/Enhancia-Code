using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using _00.Work.CDH.Code.SkillSystem;
using DG.Tweening;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.Rush
{
    public class RushSkill : Skill
    {
        [SerializeField] private ContactFilter2D whatIsGroundAndObstruct;
        [SerializeField] private float duration;
        [SerializeField] private float xDuration;

        private Player _player;
        private Rigidbody2D _rigid;
        private RaycastHit2D[] _results;

        public override void InitializeSkill(Entity entity)
        {
            base.InitializeSkill(entity);
            _player = entity as Player;
            _rigid = _player.GetComponent<Rigidbody2D>();

            _results = new RaycastHit2D[5];
        }

        protected override void UseSkill()
        {
            _player.ChangeState("RUSH");
            base.UseSkill();
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            Vector3 force = _player.transform.right * xDuration;
            Rush(force);
        }

        private void Rush(Vector3 force)
        {
            Vector3 destination = GetRushEndPoint(force);
            Vector2 delta = destination - _entity.transform.position;
            float kbDuration = delta.magnitude * duration / force.magnitude;

            _entity.transform.DOMove(destination, kbDuration).OnComplete(() =>
            {
                _rigid.linearVelocity = Vector3.zero;
            });
        }

        private Vector2 GetRushEndPoint(Vector3 force)
        {
            Vector2 startPosition = _entity.transform.position + new Vector3(0, 0.5f);
            if (Physics2D.Raycast(startPosition, force.normalized, whatIsGroundAndObstruct, _results, force.magnitude) != 0)
            {
                Vector2 hitPoint = _results[0].point;
                foreach (var hit in _results)
                {
                    Debug.Log(hit.transform.name);
                }
                hitPoint.y = _entity.transform.position.y;
                return hitPoint;
            }

            return _entity.transform.position + force;
        }
    }
}
