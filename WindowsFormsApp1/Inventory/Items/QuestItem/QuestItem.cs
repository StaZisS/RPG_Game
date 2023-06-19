using System;

namespace RPGProject.Inventory.Items.QuestItem
{
    public class QuestItem : ItemStack
    {
        private Quest _quest;

        public QuestItem(NameQuestItem nameQuestItem)
        {
            _itemStackProperty.Name = nameQuestItem.ToString();
            _itemStackProperty.TypeItemStack = TypeItemStack.QuestItem;
            _itemStackProperty.StackSize = 1;
            switch (nameQuestItem)
            {
                case NameQuestItem.KillTenEnemy:
                    _quest = new KillTenEnemy();
                    break;
            }
        }
    
        protected override void Use(LivingEntity.LivingEntity target)
        {
            _quest.ActivateQuest(target);
        }
        
    
        static NameQuestItem GetRandomNameQuestItem()
        {
            return (NameQuestItem) new Random().Next(0, Enum.GetNames(typeof(NameQuestItem)).Length);
        }
    }
}