using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using System;
using System.Collections;
using UnityEngine;

namespace _00.Work.CDH.Code.Feedbacks
{
    public class DeadFeedback : Feedback, IEntityComponent
    {
        private Player _player;

        public override void CreateFeedback()
        {
            _player.ChangeState("DEAD");
        }

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
        }
    }
}