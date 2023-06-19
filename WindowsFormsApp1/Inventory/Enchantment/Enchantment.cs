using System;
using System.Linq;
using RPGProject.Inventory.Items.EquippableItem;
using RPGProject.SubscriptionService;

namespace RPGProject.Inventory.Enchantment
{
    public class Enchantment
    {
        private Guid _id;
        private NameEnchantment _nameEnchantment;
    
        public Enchantment(NameEnchantment nameEnchantment)
        {
            _nameEnchantment = nameEnchantment;
        }
        
        public void Apply(LivingEntity.LivingEntity target, EquippableItem item)
        {
            _id = target.AddSubscription(TypeSubscription.Enchantment, () =>
            {
                Use(target, item);
            });
        }
        
        public void Remove(LivingEntity.LivingEntity target)
        {
            target.RemoveSubscription(TypeSubscription.Enchantment, _id);
        }
        
        private void Use(LivingEntity.LivingEntity target, EquippableItem item)
        {
            switch (_nameEnchantment)
            {
                case NameEnchantment.Sharpness:
                    target.SetDamageRatio(target.GetDamageRatio() + 0.2);
                    break;
                case NameEnchantment.Protection:
                    target.SetRegenerationRatio(target.GetRegenerationRatio() + 0.2);
                    break;
                case NameEnchantment.InfiniteDurability:
                    item.Repair();
                    break;
            }
        }
        
        public NameEnchantment GetNameEnchantment()
        {
            return _nameEnchantment;
        }

        public static NameEnchantment GetAppropriateNameForItem(TypeEquippableItem typeEquippableItem)
        {
            var nameEnchantment = Enum.GetValues(typeof(NameEnchantment))
                .Cast<NameEnchantment>()
                .Where(enchantment =>
                {
                    var enchantmentTypeAttribute = enchantment.GetType()
                        .GetField(enchantment.ToString())
                        .GetCustomAttributes(typeof(EnchantmentTypeAttribute), false)
                        .FirstOrDefault() as EnchantmentTypeAttribute;

                    return enchantmentTypeAttribute != null && enchantmentTypeAttribute.EquipmentTypes.Contains(typeEquippableItem);
                })
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefault();
            return nameEnchantment;
        }

        public string GetDescription()
        {
            switch (_nameEnchantment)
            {
                case NameEnchantment.Sharpness:
                    return "Sharpness: +20% damage";
                case NameEnchantment.Protection:
                    return "Protection: +20% regeneration";
                case NameEnchantment.InfiniteDurability:
                    return "Infinite durability";
                default:
                    return "";
            }
        }
    }
    
    
}