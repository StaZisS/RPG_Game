using System;
using System.IO;
using Newtonsoft.Json;
using RPGProject.Inventory.Items;
using RPGProject.Inventory.Items.EquippableItem;
using RPGProject.Inventory.Items.NullItem;
using RPGProject.Inventory.Items.UsableItem;
using WindowsFormsApp1.GameEngine;

namespace RPGProject.Inventory
{
    public class Inventory
    {
        private const int SizeArmor = 4;
        private struct InventoryProperty
        {
            public int Capacity { get; set; }
            public int Money { get; set; }
        }
        private readonly InventoryProperty _inventoryProperty;
        
        private LivingEntity.LivingEntity _owner;
        private ItemStack[] _items;
        private ItemStack[] _armor;
        private readonly string _inventoryPropertyConfigPath =
            $"{Environment.CurrentDirectory}\\PropertyJson\\InventoryProperties.json";

        public Inventory(LivingEntity.LivingEntity owner)
        {
            try
            {
                var data = File.ReadAllText(_inventoryPropertyConfigPath);
                using (var stringReader = new StringReader(data))
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    var serializer = new JsonSerializer();
                    _inventoryProperty = serializer.Deserialize<InventoryProperty>(jsonReader);
                }
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            _owner = owner;
            _items = new ItemStack[_inventoryProperty.Capacity];
            for(int i = 0; i < _items.Length; i++)
                _items[i] = new NullItem();
            _armor = new ItemStack[SizeArmor];
            for(int i = 0; i < _armor.Length; i++)
                _armor[i] = new NullItem();
        }
        
        public bool SwapItemStacks(int firstIndex, int secondIndex, TypeCell firstType, TypeCell secondType)
        {
            if (firstType == TypeCell.Bag && secondType == TypeCell.Bag)
            {
                (_items[firstIndex], _items[secondIndex]) = (_items[secondIndex], _items[firstIndex]);
                return true;
            }
            
            if (firstType == TypeCell.Armor && secondType == TypeCell.Armor)
            {
                (_armor[firstIndex], _armor[secondIndex]) = (_armor[secondIndex], _armor[firstIndex]);
                return true;
            }

            if (firstType == TypeCell.Armor && secondType == TypeCell.Bag)
            {
                //с брони в инвентарь
                if (_armor[firstIndex] is NullItem && _items[secondIndex] is NullItem)
                {
                    return true;
                }
                if (_items[secondIndex] is UsableItem ||
                    _items[secondIndex] is EquippableItem equippableItem &&
                    equippableItem.GetNameEquippableItem() == NameEquippableItem.Sword)
                {
                    return false;
                }
                SwapEquippableItemInArmor(secondIndex, firstIndex);
                return true;
            }
                
            if (firstType == TypeCell.Bag && secondType == TypeCell.Armor)
            {
                //с инвентаря в броню
                if (_items[firstIndex] is NullItem && _armor[secondIndex] is NullItem)
                {
                    return true;
                }
                if (_items[firstIndex] is UsableItem ||
                    _items[firstIndex] is EquippableItem equippableItem &&
                    equippableItem.GetNameEquippableItem() == NameEquippableItem.Sword)
                {
                    return false;
                }
                SwapEquippableItemInArmor(firstIndex, secondIndex);
                return true;
            }
            return false;
        }
        
        private void SwapEquippableItemInArmor(int firstIndex, int secondIndex)
        {
            if (_items[firstIndex] is NullItem)
            {
                (_armor[secondIndex] as EquippableItem).Unequip(_owner);
                (_armor[secondIndex], _items[firstIndex]) = (_items[firstIndex], _armor[secondIndex]);
                return;
            }

            if (_armor[secondIndex] is NullItem)
            {
                (_items[firstIndex] as EquippableItem).UseItem(_owner);
                (_armor[secondIndex], _items[firstIndex]) = (_items[firstIndex], _armor[secondIndex]);
            }
            
            for (int i = 0; i < _armor.Length; i++)
            {
                if(_armor[i] is NullItem || i == secondIndex)
                {
                    continue;
                }
                if ((_armor[i] as EquippableItem).GetNameEquippableItem() ==
                    (_armor[secondIndex] as EquippableItem).GetNameEquippableItem())
                {
                    (_armor[i] as EquippableItem).Unequip(_owner);
                    (_items[firstIndex], _armor[i]) = (_armor[i], _items[firstIndex]);
                    break; 
                }
            }
            
        }
        
        public ItemStack[] GetItems() => _items;

        public ItemStack[] GetArmor() => _armor;

        public void AddItem(ItemStack itemStack)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if(_items[i].GetName() == itemStack.GetName())
                { 
                    if (_items[i].GetMaxCount() < _items[i].GetCount() + itemStack.GetCount())
                    {
                        itemStack.SetCount(_items[i].GetCount() + itemStack.GetCount() - _items[i].GetMaxCount());
                        _items[i].SetCount(_items[i].GetMaxCount());
                        continue;
                    }
                    _items[i].SetCount(_items[i].GetCount() + itemStack.GetCount());
                    break;
                }
                if (!(_items[i] is NullItem)) continue;
                _items[i] = itemStack;
                break;
            }
        }

        public void DeleteRandomNonNullItem()
        {
            var index = GameEngine.Random.Next(0, _items.Length);
            var counter = 0;
            while (_items[index] is NullItem && counter < 10)
            {
                index = GameEngine.Random.Next(0, _items.Length);
                counter++;
            }
            _items[index] = new NullItem();
        }
        
        public void DecreaseArmorDurability()
        {
            foreach (var armor in _armor)
            {
                if (armor is EquippableItem item)
                {
                    item.DecriesDurability(_owner);
                }
            }
        }
        
        public void CreateRandomItemsAndPutThemInInventory(int countItems)
        {
            for (var i = 0; i < countItems; i++)
            {
                AddItem(ItemStack.CreateRandomItem());
            }
        }
    }
}