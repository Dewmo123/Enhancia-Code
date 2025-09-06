using _00.Work.CDH.Code.SkillSystem;
using Assets._00.Work.CDH.Code.SkillSystem.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventSystem
{
	public class InStageEvent
	{
		public static StageEndEvent StageEndEvent = new StageEndEvent();
		public static ProclaimSkillCompEvent ProclaimSkillCompEvent = new ProclaimSkillCompEvent();
		public static SkillChangeEvent SkillChangeEvent = new SkillChangeEvent();
		public static RequestSkillChangeEvent RequestSkillChangeEvent = new RequestSkillChangeEvent();
	}

	public class StageEndEvent : GameEvent
	{

	}

	public class ProclaimSkillCompEvent : GameEvent
	{
		public bool isInited = false;
		public PlayerSkillCompo skillComponent;
		public IEnumerable<SkillItemSO> skillItemList;

		public ProclaimSkillCompEvent Initialize(PlayerSkillCompo skillComponent, IEnumerable<SkillItemSO> skillItemList)
		{
			isInited = true;

            this.skillComponent = skillComponent;
			this.skillItemList = skillItemList;
			return this;
		}
    }

	public class SkillChangeEvent : GameEvent
	{
		public int skillIndex;
		public SkillItemSO skillItemSO;

		public SkillChangeEvent Initialize(int index, SkillItemSO skillIten)
		{
			this.skillIndex = index;
			this.skillItemSO = skillIten;
			return this;
		}
    }

    public class RequestSkillChangeEvent : GameEvent
    {
        public int skillIndex;
        public SkillItemSO skillItemSO;

        public RequestSkillChangeEvent Initialize(int index, SkillItemSO skillIten)
        {
            this.skillIndex = index;
            this.skillItemSO = skillIten;
            return this;
        }
    }
}
