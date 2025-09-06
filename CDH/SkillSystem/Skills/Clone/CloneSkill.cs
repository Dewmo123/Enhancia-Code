using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace _00.Work.CDH.Code.SkillSystem.Skills.Clone
{
    public class CloneSkill : Skill
    {
        [Header("Clone Info")]
        [SerializeField] private ObjectFinderSO poolMonoFinder;
        [SerializeField] private PoolItemSO clonePrefabSO;
        [SerializeField] private float cloneCreateDuration;
        [SerializeField] private float cloneOffset = 2f;

        private PoolManagerMono _poolManager;
        private EntityRenderer _renderer;
        private float _timer;
        private int _maxComboCount;
        private int _currentCombo;
        private bool _canCreateClone;
        private Player _player;


        public override void InitializeSkill(Entity entity)
        {
            base.InitializeSkill(entity);
            _renderer = entity.GetCompo<EntityRenderer>();
            _player = entity as Player;

            bool settingPoolSuccess = poolMonoFinder.GetObject(out _poolManager);
            Debug.Assert(settingPoolSuccess, "can't set poolMono");
        }

        public override void UpdateSkill()
        {
            base.UpdateSkill();

            _timer += Time.deltaTime;
            if (_canCreateClone)
            {
                if (_timer >= cloneCreateDuration && _currentCombo < _maxComboCount)
                {
                    _timer = 0f;
                    ++_currentCombo;
                    CreateCloneSkill();
                }

                if (_currentCombo >= _maxComboCount)
                {
                    _canCreateClone = false;
                }
            }
        }

        public void CreateClone(Transform originTrm, Vector3 offset)
        {
            Clone newClone = _poolManager.Pop<Clone>(clonePrefabSO);
            newClone.transform.position = originTrm.position + offset;
            newClone.SetUp(
                entityHealth => CalculateDamage(_damage, entityHealth),
                SkillData,
                _entity,
                _poolManager,
                SkillData.AttackCount,
                SkillData.attackTargetCount
                );
        }

        private void CreateCloneSkill()
        {
            float cloneXPosition = cloneOffset * _renderer.FacingDirection;
            CreateClone(_entity.transform, new Vector3(cloneXPosition, 0));
        }

        protected override void UseSkill()
        {
            _player.ChangeState("CLONE");
            base.UseSkill();
            SkillData.attackTargetCount = (int)_player.GetCompo<EntityStat>().GetStat("attack_target_count").Value;
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            _timer = cloneCreateDuration;
            _currentCombo = 0;
            _maxComboCount = SkillData.AttackCount;
            _canCreateClone = true;
        }
    }
}