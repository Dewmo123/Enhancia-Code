using Core.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine.Rendering;

namespace Assets._00.Work.AKH.Scripts.Core.EventSystem
{
	public class GenerateEvent
	{
		public static GenerateCompleteEvent GenerateCompleteEvent = new GenerateCompleteEvent();
		public static GenerateStartEvent GenerateStartEvent = new GenerateStartEvent();
	}

	public class GenerateStartEvent : GameEvent
	{
	}

	public class GenerateCompleteEvent : GameEvent
	{
		public bool success;
		public int currentFloor;
		public IEnumerable<CinemachineCamera> stageCameraList;
		public IEnumerable<Volume> volumes;

		public GenerateCompleteEvent Initialize(bool success, IEnumerable<CinemachineCamera> stageCameraList, IEnumerable<Volume> volumes, int currentFloor)
		{
			this.success = success;
			this.stageCameraList = stageCameraList;
			this.volumes = volumes;
			this.currentFloor = currentFloor;

            return this;
		}
	}
}
