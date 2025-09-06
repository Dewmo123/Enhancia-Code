using Core.EventSystem;

namespace Assets._00.Work.AKH.Scripts.Core.EventSystem
{
    public class EnemySpawnEvent
    {
        public static EnemyCountChangeEvent EnemyCountChangeEvent = new EnemyCountChangeEvent();
    }

    public class EnemyCountChangeEvent : GameEvent
    {
        public int count;

        public EnemyCountChangeEvent Initialize(int count)
        {
            this.count = count;
            return this;
        }
    }
}
