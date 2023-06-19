using System;
using RPGProject.Inventory.Items.EquippableItem;
using RPGProject.Inventory.Items.UsableItem;
using WindowsFormsApp1.GameEngine;

namespace RPGProject.Inventory.Items
{
    public abstract class ItemStack
    {
        public struct ItemStackProperty
        {
            public int Price { get; set; }
            public string Name { get; set; }
            public int StackSize { get; set; }
            public int MaxStackSize { get; set; }
            public string Description { get; set; }
            public TypeItemStack TypeItemStack { get; set; }
        }
        public ItemStackProperty _itemStackProperty;

        public void UseItem(LivingEntity.LivingEntity target)
        {
            switch (_itemStackProperty.TypeItemStack)
            {
                case TypeItemStack.Null:
                    return;
                case TypeItemStack.EquipmentItem:
                    Use(target);
                    break;
                case TypeItemStack.QuestItem:
                    Use(target);
                    break;
                case TypeItemStack.UsableItem:
                    Use(target);
                    if (StackPop() <= 0)
                    {
                        SetToNull();
                    }
                    break;
            }
        }

        public void SetToNull()
        {
            _itemStackProperty = new ItemStackProperty
            {
                Price = 0,
                Name = "",
                StackSize = 0,
                MaxStackSize = 0,
                Description = "",
                TypeItemStack = TypeItemStack.Null
            };
        }
        
        protected abstract void Use(LivingEntity.LivingEntity target);

        public int StackPop()
        {
            _itemStackProperty.StackSize--;
            return _itemStackProperty.StackSize;
        }
        
        public string GetName()
        {
            return _itemStackProperty.Name;
        }
        
        public int GetPrice()
        {
            return _itemStackProperty.Price;
        }
        
        public int GetCount()
        {
            return _itemStackProperty.StackSize;
        }
        
        public int GetMaxCount()
        {
            return _itemStackProperty.MaxStackSize;
        }
        
        public void SetCount(int count)
        {
            _itemStackProperty.StackSize = count;
        }
        
        public string GetDescription()
        {
            if (this is EquippableItem.EquippableItem equippableItem)
            {
                _itemStackProperty.Description = equippableItem.GenerateDescription();
            }
            return _itemStackProperty.Description;
        }
        
        public TypeItemStack GetTypeItemStack()
        {
            return _itemStackProperty.TypeItemStack;
        }
        
        public static ItemStack CreateRandomItem()
        {
            if (GameEngine.Random.NextDouble() > 0.5)
            {
                NameUsableItem[] names = (NameUsableItem[])Enum.GetValues(typeof(NameUsableItem));
                NameUsableItem randomName = names[GameEngine.Random.Next(names.Length)];
                return new UsableItem.UsableItem(randomName, 1);
            }
            else
            {
                NameEquippableItem[] names = (NameEquippableItem[])Enum.GetValues(typeof(NameEquippableItem));
                NameEquippableItem randomName = names[GameEngine.Random.Next(names.Length)];
                return new EquippableItem.EquippableItem(randomName);
            }
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}