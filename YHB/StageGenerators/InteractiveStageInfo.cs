using Unity.Cinemachine;
using UnityEngine.Rendering;

namespace Assets._00.Work.YHB.Scripts.StageGenerators
{
	public sealed class InteractiveStageInfo
	{
		public CinemachineCamera previousCamera;
		public CinemachineCamera nextCamera;

		public Volume previousVolume;
		public Volume nextVolume;

		public bool IsDirectionHorizontal;

		public InteractiveStageInfo() { }
		public InteractiveStageInfo(CinemachineCamera previousCamera, CinemachineCamera nextCamera, Volume previousVolume, Volume nextVolume, bool isDirectionHorizontal = true)
		{
			this.previousCamera = previousCamera;
			this.nextCamera = nextCamera;
			this.previousVolume = previousVolume;
			this.nextVolume = nextVolume;
			IsDirectionHorizontal = isDirectionHorizontal;
		}
	}
}
