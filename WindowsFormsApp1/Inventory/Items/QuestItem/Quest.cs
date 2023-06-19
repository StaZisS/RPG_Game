using System;
using System.Collections.Generic;
using RPGProject.SubscriptionService;

namespace RPGProject.Inventory.Items.QuestItem
{
    public abstract class Quest
    {
        public abstract void ActivateQuest(LivingEntity.LivingEntity target);
        
        protected Guid AddSubscription(LivingEntity.LivingEntity target, Action action)
        {
            return target.AddSubscription(TypeSubscription.QuestAction, action);
        }
        
        protected void RemoveSubscription(LivingEntity.LivingEntity target, Guid id)
        {
            target.RemoveSubscription(TypeSubscription.QuestAction, id);
        }
    }
}