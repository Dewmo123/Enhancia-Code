using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine.Rendering;

namespace Core.EventSystem
{
    public class VolumeEvent
    {
        public static readonly VolumeChangeEvent volumeChangeChangeEvent = new VolumeChangeEvent();
    }

    public class VolumeChangeEvent : GameEvent
    {
        public Volume previousVolume;
        public Volume nextVolume;
    }
}
