using _00.Work.CDH.Code.Animators;

namespace _00.Work.CDH.Code.Entities.FSM
{
    public abstract class EntityState
    {
        protected Entity _entity;

        protected AnimParamSO _animParam;
        protected bool _isTriggerCall;

        private EntityAnimationTrigger _animationTriggerCompo;

        protected EntityRenderer _renderer;
        protected bool _isMove = false;
        protected bool _canMove = false;

        public EntityState(Entity entity, AnimParamSO animParam)
        {
            _entity = entity;
            _animParam = animParam;
            _renderer = _entity.GetCompo<EntityRenderer>(true);
            _animationTriggerCompo = _entity.GetCompo<EntityAnimationTrigger>();
        }

        public virtual void Enter()
        {
            _renderer.SetParam(_animParam, true);
            _isTriggerCall = false;
            _animationTriggerCompo.OnAnimationEnd += HandleAnimationEndTrigger;
        }
        public virtual void Update()
        {

        }
        public virtual void Exit()
        {
            _renderer.SetParam(_animParam, false);
            _animationTriggerCompo.OnAnimationEnd -= HandleAnimationEndTrigger;
        }

        protected virtual void HandleAnimationEndTrigger()
            => _isTriggerCall = true;
    }
}
