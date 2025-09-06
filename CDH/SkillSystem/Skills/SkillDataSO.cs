using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities.FSM;
using Scripts.Core.Sound;
using UnityEngine;

namespace _00.Work.CDH.Code.SkillSystem.Skills
{
	[CreateAssetMenu(fileName = "Skill", menuName = "SO/SkillSystem/SkillData", order = 0)]
	public class SkillDataSO : ScriptableObject
	{
		[Header("Skill State Info")]
		[Tooltip("스킬 실행시 출력 할 애니메이션")]
        public AnimParamSO skillAnimParam;
		[Tooltip("스테이트 넘어가는게 필요할 때 추가")] 
		public StateSO skillState;
		//public SkillStatValue[] skillStatByEnchance;

		//private Dictionary<int, SkillStatValue> _skillStatDictionary;
		
		// public int enchanceValue;
		[Header("Skill Target Info")]
		public LayerMask canCollisionTargetLayer;
		public int attackTargetCount;

		[Header("Skill Attack Info")]
		public bool isPowerAttack;
		[SerializeField] private int minAttackCount;
		[SerializeField] private int maxAttackCount;
		public int AttackCount
			=> UnityEngine.Random.Range(minAttackCount, maxAttackCount);

		[Header("Skill Damage Info")]
		[Tooltip("스킬의 대미지 배수 수치입니다.")]
		[field : SerializeField] public float MultiplyDamage {  get; private set; }
		[Tooltip("스킬의 추가 대미지 수치입니다.")]
        [field: SerializeField] public float AdditionalDamage { get; private set; }
        [Tooltip("적의 최대 체력에 비례한 추가 대미지 백분율 수치입니다.")]
		[Range(0f, 1f)]
        [field: SerializeField] public float EnemyHpProportionProbability { get; private set; }

        [Header("Skill Charge Info")]
		[Tooltip("스킬의 충전되기까지 걸리는 시간입니다.")]
		public float cooldown;
		[Tooltip("스킬의 최대 충전량입니다.")]
		[field: SerializeField] public int MaxChargeCount { get; private set; } = 1; // 스킬 충전량

		[Header("Skill Movement Info")]
		[Tooltip("스킬 사용시에 움직일 값입니다.")]
		public Vector2 movementValue;
		[Tooltip("적이 공격을 받으면 적용될 넉백 수치입니다.")]
		public Vector2 enemyKnockbackForce;
        [Tooltip("스킬 사용 후 적용될 카메라 움직임 값입니다.")]
        public float cameraShakeValue;
        [Tooltip("스킬 사용 후 적용될 카메라가 움직이는 시간입니다.")]
        public float cameraShakeDuration;

		[Header("Skill Sound Info")]
		public SoundSO soundSO;

        //public bool isPowerAttack;

        /* [Serializable]
		public struct SkillStatValue
		{
			// public int enchanceValue;
			public LayerMask targets;
			public int attackTargetCount;
			[SerializeField] private int minAttackCount;
			[SerializeField] private int maxAttackCount;
			public int AttackCount
				=> UnityEngine.Random.Range(minAttackCount, maxAttackCount);

			[SerializeField] private float damage;
			[SerializeField] private float multiplyDamage;
			[SerializeField] private float additionalDamage;
			[SerializeField] private float enemyHpProportionProbability;

			public float cooldown;
			public int maxChargeCount; // 스킬 충전량
			public bool canMove;
			public bool isMove;
			public bool isPowerAttack;

			/// <summary>
			/// 데미지를 계산해서 가져옵니다.
			/// </summary>
			/// <param name="enemyMaxHealth">적의 최대 체력입니다. 0일 경우 체력비례 데미지를 계산하지 않습니다.</param>
			/// <returns>계산된 데미지</returns>
			public float CalculateDamage(float enemyMaxHealth)
				=> ((damage * multiplyDamage) + additionalDamage) + (enemyHpProportionProbability / 100 * enemyMaxHealth);
		} */

        /*
	        public string ScriptPath { get; private set; } = "Assets/00.Work/CDH/Code/SkillSystem/Skills";
        public string AssetPath { get; private set; } = "Assets/00.Work/CDH/Code/SkillSystem/SkillsSO";
        public string StatePath { get; private set; } = "Assets/00.Work/CDH/08_SO/FSM/Skill";
        public string ParamPath { get; private set; } = "Assets/00.Work/CDH/Code/SkillSystem/SkillsSO/Animator"; 
	 
	        public string SkillClassName
        {
            get => _className;
            set => _className = value";
        }

	        private string _skillScriptPath;
        private string _className = "";

	public SkillStatValue GetSkillStats(int enchance)
	{
		if (_skillStatDictionary == null)
		{
			_skillStatDictionary = new Dictionary<int, SkillStatValue>();
			foreach (var skillStats in skillStatByEnchance)
				_skillStatDictionary[skillStats.enchanceValue] = skillStats;
		}
		return _skillStatDictionary.GetValueOrDefault(enchance);
	}

	public void CreateSkill(string skillName, string paramName)
	{
		CreateFsmState(skillName, paramName);
		CreateSkillScript(skillName);

		AssetDatabase.Refresh();
	}

	private void CreateFsmState(string newName, string paramName)
	{
		StateSO newState = CreateInstance<StateSO>();
		CreateIfNotExists(StatePath);
		AssetDatabase.CreateAsset(newState, $"{StatePath}/{newName}.asset");
		newState.stateName = newName;
		SkillClassName = newName;
		newState.className = SkillClassName;
		newState.animParam = CreateAnimParam(paramName);
		skillState  = newState;
	}

	private AnimParamSO CreateAnimParam(string newName)
	{
		newName += "Param";
		AnimParamSO newParam = CreateInstance<AnimParamSO>();
		CreateIfNotExists(ParamPath);
		AssetDatabase.CreateAsset(newParam, $"{ParamPath}/{newName}.asset");
		newParam.name = newName;
		skillAnimParam = newParam;
		return newParam;
	}

	private void CreateSkillScript(string name)
	{
		CreateIfNotExists(ScriptPath);
		_skillScriptPath = Path.Combine(ScriptPath + "/", SkillClassName);

		string scriptContent = $@"
using _00.Work.CDH.Code.SkillSystem;

public class {SkillClassName} : {"Skill"}
{{
	[field : SerializeField] public SkillSO SkillSo;

	private void Start()
	{{
		SkillSo = GetSkillSo();
	}}

	protected override void UseSkill()
	{{
	}}
}}";
		File.WriteAllText(_skillScriptPath + ".cs", scriptContent);
	}

	public void DeleteSkillScript()
	{
		if (File.Exists(_skillScriptPath + ".cs"))
		{
			Debug.Log("Skill script deleted");
			AssetDatabase.DeleteAsset(_skillScriptPath + ".cs");
		}
		else
		{
			Debug.Log(_skillScriptPath + ".cs");
		}

		AssetDatabase.Refresh();
	}

	public void CreateIfNotExists(string savePath)
	{
		if(!System.IO.Directory.Exists(savePath))
		{
			System.IO.Directory.CreateDirectory(savePath);
		}
	}
}
	*/
    }
}