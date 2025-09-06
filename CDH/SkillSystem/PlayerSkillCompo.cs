using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using Assets._00.Work.CDH.Code.SkillSystem.Skills;
using Assets._00.Work.YHB.Scripts.Core;
using Core.EventSystem;
using DewmoLib.ObjectPool.RunTime;
using Scripts.Core.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _00.Work.CDH.Code.SkillSystem
{
    public class PlayerSkillCompo : MonoBehaviour, IEntityComponent
    {
        public const int MAX_ACTIVE_SKILL_COUNT = 3;

        [SerializeField] private EventChannelSO inStageEventChannel;
        [SerializeField] private EventChannelSO cameraEventChannel;

        public delegate void OnChangedActiveSkill(int index, Skill skill);
        /// <summary>
        /// 만약 변경된 위치의 스킬이 없다면 Null을 반환함
        /// </summary>
        public event OnChangedActiveSkill OnChangeActiveSkill;

        [SerializeField] private int maxCheckEnemy;
        public ContactFilter2D whatIsEnemy;

        [Header("Sounds")]
        [SerializeField] private PoolItemSO soundPlayerItem;
        [SerializeField] private ObjectFinderSO poolMonoFinder;
        private PoolManagerMono _poolMono;

        private Collider2D[] _colliders;

        private Dictionary<Type, Skill> _skillDictionary;
        // 현재 저장된 스킬들을 배열에 저장해 사용할 것임
        public Skill[] CurrentActiveSkills { get; private set; }

        private Player _player;
        private EntityRenderer _renderComp;
        private EntityMover _moverComp;

        public void Initialize(Entity entity)
        {
            _poolMono = poolMonoFinder.GetObject<PoolManagerMono>();

            _player = entity as Player;
            _renderComp = entity.GetCompo<EntityRenderer>();
            _moverComp = entity.GetCompo<EntityMover>();

            _colliders = new Collider2D[maxCheckEnemy];
            _skillDictionary = new Dictionary<Type, Skill>();
            CurrentActiveSkills = new Skill[MAX_ACTIVE_SKILL_COUNT];

            GetComponentsInChildren<Skill>().ToList().ForEach(skill =>
            {
                _skillDictionary.Add(skill.GetType(), skill);
            });
            _skillDictionary.Values.ToList().ForEach(skill => skill.InitializeSkill(_player));

            _player.InputSO.OnSkillKeyPressed += HandleSkillKeyPressed;

            inStageEventChannel.AddListener<RequestSkillChangeEvent>(HandleRequestSkillChangeEvent);
        }

        private void OnDestroy()
        {
            _player.InputSO.OnSkillKeyPressed -= HandleSkillKeyPressed;
            inStageEventChannel.RemoveListener<RequestSkillChangeEvent>(HandleRequestSkillChangeEvent);
        }

        private void Update()
        {
            for (int i = 0; i < MAX_ACTIVE_SKILL_COUNT; ++i)
            {
                CurrentActiveSkills[i]?.UpdateSkill();
            }
        }

        private void HandleSkillKeyPressed(int skillId)
        {
            UseActiveSkill(skillId);
        }

        public T GetSkill<T>() where T : Skill
        {
            Type type = typeof(T);
            return _skillDictionary.GetValueOrDefault(type) as T;
        }

        public Skill GetSkill(Type skillType)
            => _skillDictionary.GetValueOrDefault(skillType);

        public void SetActiveSkill(int settingIndex, SkillItemSO targetSkill)
        {
            SetActiveSkill(settingIndex, targetSkill != null ? targetSkill.SkillType : null);
            inStageEventChannel.InvokeEvent(InStageEvent.SkillChangeEvent.Initialize(settingIndex, targetSkill));
        }

        // 동적으로 스킬이 바뀌어야하니까 제네릭이 아닌 Type기반으로 가져옴
        public void SetActiveSkill(int settingIndex, Type targetSkillType)
        {
            Debug.Assert(settingIndex >= 0 && settingIndex < MAX_ACTIVE_SKILL_COUNT, $"target settingIndex is Out of range, Max : {MAX_ACTIVE_SKILL_COUNT}, Current : {settingIndex}");

            if (targetSkillType == null)
            {
                CurrentActiveSkills[settingIndex] = null;
                OnChangeActiveSkill?.Invoke(settingIndex, null);
                return;
            }

            Skill targetSkill = GetSkill(targetSkillType);

            bool swap = false;
            // _currentActiveSkills.ToList().Contains(targetSkill); <-- 이게 효율적이고 가독성이 더 좋은 거 같긴 함. 근데 타입으로 비교하는게 더 안전 할 듯.
            // _currentActiveSkills.ToList().Select(skill => skill.GetType()).ToList().Contains(typeof(T)) // <- 바꾸려는 타겟스킬정보를 못가져옴
            // 결국 있는지 확인해도 교환하려면 인덱스를 알아햐하니 이게 더 나을듯
            for (int i = 0; i < MAX_ACTIVE_SKILL_COUNT; i++)
            {
                // 만일 현재 셋팅에 타겟 셋팅이 포함 되어있으면 위치를 바꾸게 함
                if (CurrentActiveSkills[i] != null && EqualityComparer<Type>.Default.Equals(CurrentActiveSkills[i].GetType(), targetSkill.GetType()))
                {
                    // Active 호출할 필요가 없음.
                    // 현재 쿨타임, 차지상태를 유지할것이기 때문
                    Skill temp = CurrentActiveSkills[i];
                    CurrentActiveSkills[i] = CurrentActiveSkills[settingIndex];
                    CurrentActiveSkills[settingIndex] = temp;

                    swap = true;

                    OnChangeActiveSkill?.Invoke(i, temp);
                    OnChangeActiveSkill?.Invoke(settingIndex, CurrentActiveSkills[i]);

                    break;
                }
            }

            if (!swap)
            {
                CurrentActiveSkills[settingIndex]?.UnActivedSkill();
                targetSkill.ActivedSkill();
                CurrentActiveSkills[settingIndex] = targetSkill;

                OnChangeActiveSkill?.Invoke(settingIndex, targetSkill);
            }
        }

        public void RemoveActiveSkill(int settingIndex)
        {
            Debug.Assert(settingIndex >= 0 && settingIndex < MAX_ACTIVE_SKILL_COUNT, $"target settingIndex is Out of range, Max : {MAX_ACTIVE_SKILL_COUNT}, Current : {settingIndex}");

            CurrentActiveSkills[settingIndex]?.UnActivedSkill();
            CurrentActiveSkills[settingIndex] = null;

            OnChangeActiveSkill?.Invoke(settingIndex, null);
        }

        /// <summary>
        /// 현재 활성화 된 스킬을 가져옵니다.
        /// </summary>
        /// /// <returns>현재 할당된 키에 해당하는 스킬을 가져옵니다. 유효하지 않은 인덱스면 null을 반환합니다.</returns>
        public Skill GetActiveSkill(int index)
        {
            if (index < 0 || index >= MAX_ACTIVE_SKILL_COUNT)
            {
                Debug.Log($"target settingIndex is Out of range, Max : {MAX_ACTIVE_SKILL_COUNT}, Current : {index}");
                return null;
            }

            return CurrentActiveSkills[index];
        }

        public bool IsActiveSkill(Type targetSkill)
            => CurrentActiveSkills.FirstOrDefault(stat => EqualityComparer<Type>.Default.Equals(stat?.GetType(), targetSkill)) != default;

        public bool IsActiveSkill<T>() // FirstOrDefault가 IEnumerable이였노..
            => CurrentActiveSkills.FirstOrDefault(stat => EqualityComparer<Type>.Default.Equals(stat?.GetType(), typeof(T))) != default;
        //{
        //    // _currentSettingSkills.ToList().Contains(targetSkill); <-- 이게 효율적이고 가독성이 더 좋은 거 같긴 함. 근데 타입으로 비교하는게 더 안전 할 듯.
        //    _currentActiveSkills.FirstOrDefault(stat => EqualityComparer<Type>.Default.Equals(stat.GetType(), typeof(T)));

        //    return false;
        //}

        /// <summary>
        /// 지정된 인덱스의 스킬을 사용합니다.
        /// </summary>
        /// <returns>스킬을 사용할 수 있었는지 반환합니다.</returns>
        public bool UseActiveSkill(int settingIndex)
        {
            Debug.Assert(settingIndex >= 0 && settingIndex < MAX_ACTIVE_SKILL_COUNT, $"target settingIndex is Out of range, Max : {MAX_ACTIVE_SKILL_COUNT}, Current : {settingIndex}");

            bool useSuccess = CurrentActiveSkills[settingIndex]?.AttemptUseSkill() ?? false;

            if (useSuccess)
            {
                cameraEventChannel.InvokeEvent(CameraEvent.CameraShakeEvent.Initialize(
                    CurrentActiveSkills[settingIndex].SkillData.cameraShakeValue,
                    CurrentActiveSkills[settingIndex].SkillData.cameraShakeDuration));

                SoundPlayer soundPlayer = _poolMono.Pop<SoundPlayer>(soundPlayerItem);
                soundPlayer.PlaySound(CurrentActiveSkills[settingIndex].SkillData.soundSO, _player.transform.position);

                Vector2 movement = CurrentActiveSkills[settingIndex].SkillData.movementValue;
                movement.x *= _renderComp.FacingDirection;
                _moverComp.AddForceToEntity(movement);
            }

            return useSuccess;
        }

        public virtual int GetEnemiesInRange(Vector3 checkPosition, float range)
            => Physics2D.OverlapCircle(checkPosition, range, whatIsEnemy, _colliders);

        public virtual Transform FindClosestEnemy(Vector3 checkPosition, float range)
        {
            Transform closestOne = null;
            int cnt = Physics2D.OverlapCircle(checkPosition, range, whatIsEnemy, _colliders);

            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < cnt; i++)
            {
                if (_colliders[i].TryGetComponent(out Entity enemy))
                {
                    //if (enemy.IsDead) continue; 
                    Debug.Log("Entity에 IsDead 만들어야 해요");
                    float distanceToEnemy = Vector2.Distance(checkPosition, _colliders[i].transform.position);

                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestOne = _colliders[i].transform;
                    }
                }
            }

            return closestOne; // FindClosestEnemy < Find (Closest) Enemy -> Find 'Enemy' 근데, 반환 값은 왜 Transform?
        }

        private void HandleRequestSkillChangeEvent(RequestSkillChangeEvent @event)
        {
            if (@event.skillIndex != -1)
            {
                SetActiveSkill(@event.skillIndex, @event.skillItemSO);
            }
        }

        /* 이미 스킬컴포넌트가 있다고 가정한 상태에서 초기화를 함. 즉, 아래 코드는 필요없을 것으로 생각됨
         
        public void SkillAddToDictionary(Skill newSkill) // 스킬을 매개변수로 넣어주면 스킬의 IsHaveSkill true로 해주고 딕셔너리에 넣어줌
        {
            newSkill.IsHaveSkill = true;
            UpdateSkill();
        }

        private void UpdateSkill() // 자식으로 있는 스킬중, IsHaveSkill 켜져있는 스킬만 딕셔너리에 넣어주고 Initialize해줌
        {
            GetComponentsInChildren<Skill>().ToList().ForEach(skill =>
            {
                if (skill.IsHaveSkill)
                {
                    _skillDictionary.Add(skill.GetType(), skill);
                    skill.InitializeSkill(_entity);
                }
            });
        }

        public void RemoveSkill<T>(T skill) where T : Skill // 스킬을 매개변수로 넣어주면 스킬의 IsHaveSkill false로 해주고 딕셔너리에서 빼줌
        {
            skill.IsHaveSkill = false;
            _skillDictionary.Remove(skill.GetType());
            UpdateSkill();
        }*/

#if UNITY_EDITOR
        [Header("Only Test")]
        [SerializeField] private int index;
        [SerializeField] private SkillItemSO skillItem;

        [ContextMenu("Set Skill")]
        private void SetSkill()
        {
            SetActiveSkill(index, skillItem);
        }
#endif
    }
}
