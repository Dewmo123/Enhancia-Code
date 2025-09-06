using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Feedbacks;
using Core.EventSystem;
using Scripts.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Feedbacks
{
    public class ItemDropFeedback : Feedback, IEntityComponent
    {
        [SerializeField] private EventChannelSO invenChannel;
        [SerializeField] private DropTableSO dropTable;

        private Entity _owner;

        public void Initialize(Entity entity)
        {
            _owner = entity;
            _owner.OnDead.AddListener(CreateFeedback);
        }

        private void OnDestroy()
        {
            _owner.OnDead.RemoveListener(CreateFeedback);
        }

        public override void CreateFeedback()
        {
            var evt = InvenEvents.RequestAddItemsEvent;
            evt.dropInfos = dropTable.PullUpItem();
            invenChannel.InvokeEvent(evt);
        }
    }
}
