using System;
using RPGProject.Inventory.Items.EquippableItem;

namespace RPGProject.Inventory.Items.QuestItem
{
    public class KillTenEnemy : Quest
    {
        public Guid Id;
        private int _countKillEnemy;
        
        public override void ActivateQuest(LivingEntity.LivingEntity target)
        {
            _countKillEnemy = Player.Instance.GetCountOfKilledEnemies();
            Id = AddSubscription(target, Action);
        }

        private void Action()
        {
            if(_countKillEnemy + 10 > Player.Instance.GetCountOfKilledEnemies()) return;
            Player.Instance.GetInventory().AddItem(new EquippableItem.EquippableItem(NameEquippableItem.Sword));
            RemoveSubscription(Player.Instance, Id);
        }
    }
}