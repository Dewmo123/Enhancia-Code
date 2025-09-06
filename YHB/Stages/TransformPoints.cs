using System;
using UnityEngine;
using DirectionInformation = Assets._00.Work.YHB.Scripts.Core.CommonEnum.DirectionInformation;

namespace Assets._00.Work.YHB.Scripts.Stages
{
    [Serializable]
    public sealed class TransformPoints // 편의성 및 깔끔하게 보고, 관리하기 위해서 이리 만듬
    {
        [SerializeField] private Transform upPoint;
        [SerializeField] private Transform downPoint;
        [SerializeField] private Transform leftPoint;
        [SerializeField] private Transform rightPoint;

        public Transform this[DirectionInformation direction] => direction switch
        {
            DirectionInformation.Up => upPoint,
            DirectionInformation.Down => downPoint,
            DirectionInformation.Left => leftPoint,
            DirectionInformation.Right => rightPoint,
            _ => throw new NotImplementedException()
        };

#if UNITY_EDITOR
        public void SetTransform(DirectionInformation direction, Transform trm)
        {
            switch (direction)
            {
                case DirectionInformation.Up:
                    upPoint = trm;
                    break;
                case DirectionInformation.Down:
                    downPoint = trm;
                    break;
                case DirectionInformation.Left:
                    leftPoint = trm;
                    break;
                case DirectionInformation.Right:
                    rightPoint = trm;
                    break;
                default:
                    break;
            };
        }
#endif
    }
}
