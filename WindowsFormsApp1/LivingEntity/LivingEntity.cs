using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using RPGProject.Inventory.Items;
using RPGProject.Inventory.Items.EquippableItem;
using RPGProject.SubscriptionService;
using WindowsFormsApp1.GameEngine;

namespace RPGProject.LivingEntity
{
    public abstract class LivingEntity
    {
        private readonly SubscriptionService.SubscriptionService _subscriptionService =
            new SubscriptionService.SubscriptionService();
        
        public readonly Guid Id = Guid.NewGuid();
        
        public Guid AddSubscription(TypeSubscription typeSubscription, Action action) =>
            _subscriptionService.Subscribe(typeSubscription, action);

        public List<Action> GetSubscriptions(TypeSubscription typeSubscription) =>
            _subscriptionService.GetSubscriptions(typeSubscription);

        public void RemoveSubscription(TypeSubscription typeSubscription, Guid id) =>
            _subscriptionService.Unsubscribe(typeSubscription, id);

        protected struct LivingEntityPropertiesStruct
        {
            public double Health { get; set; }
            
            public double Food { get; set; }
            
            public double Speed { get; set; }
            
            public double Power { get; set; }
            
            public int Money { get; set; }
            
            public int CountOfKilledEnemies { get; set; }
            
            public double MaxHealth { get; set; }
            
            public double MaxPower { get; set; }
            
            public double MaxFood { get; set; }
            
            public double MaxSpeed { get; set; }
            
            public double DamageRatio { get; set; }
            
            public double RegenerationRatio { get; set; }
            
            public ItemStack ItemStackInHand { get; set; }
            
            public double PositionX { get; set; }
            
            public double PositionY { get; set; }
            
            public double ViewDistance { get; set;}
            
            public double AttackDistance { get; set;}
            
            public double EyeDirection { get; set; }

            public Inventory.Inventory Inventory { get; set; }
        }
        
        protected LivingEntityPropertiesStruct LivingEntityProperties;
        
        protected void SetLivingEntityProperties(LivingEntityPropertiesStruct livingEntityPropertiesStruct) =>
            LivingEntityProperties = livingEntityPropertiesStruct;

        public void SetHealth(double health) =>
            LivingEntityProperties.Health = GetMin(health, LivingEntityProperties.MaxHealth);
        
        public void SetFood(double food) =>
            LivingEntityProperties.Food = GetMin(food, LivingEntityProperties.MaxFood);

        public void SetSpeed(double speed) =>
            LivingEntityProperties.Speed = GetMin(speed, LivingEntityProperties.MaxSpeed);

        public void SetPower(double power) => 
            LivingEntityProperties.Power = GetMin(power, LivingEntityProperties.MaxPower);
        
        private double GetMin(double available, double bust) => Math.Max(0, Math.Min(available, bust));

        public double GetHealth() => LivingEntityProperties.Health;

        public double GetFood() => LivingEntityProperties.Food;

        public double GetSpeed() => LivingEntityProperties.Speed;

        public double GetPower() => LivingEntityProperties.Power;
        
        public int GetMoney() => LivingEntityProperties.Money;
        
        public void SetMoney(int money) => LivingEntityProperties.Money = money;
        
        private void AddMoney(int money) => LivingEntityProperties.Money += money;
        
        public bool SubtractMoney(int money)
        {
            if (LivingEntityProperties.Money < money)
            {
                return false;
            }

            LivingEntityProperties.Money -= money;
            return true;
        }
        
        public void SetCountOfKilledEnemies(int countOfKilledEnemies) =>
            LivingEntityProperties.CountOfKilledEnemies = countOfKilledEnemies;
        
        private void AddCountOfKilledEnemies() =>
            LivingEntityProperties.CountOfKilledEnemies += 1;
        
        public int GetCountOfKilledEnemies() => LivingEntityProperties.CountOfKilledEnemies;

        public void SetDamageRatio(double damageRatio) =>
            LivingEntityProperties.DamageRatio = damageRatio;

        public void SetRegenerationRatio(double regenerationRatio) =>
            LivingEntityProperties.RegenerationRatio = regenerationRatio;

        public double GetDamageRatio() => LivingEntityProperties.DamageRatio;

        public double GetRegenerationRatio() =>
            LivingEntityProperties.RegenerationRatio;

        public double GetMaxHealth()
        {
            return LivingEntityProperties.MaxHealth;
        }
        
        public double GetMaxFood()
        {
            return LivingEntityProperties.MaxFood;
        }

        public Inventory.Inventory GetInventory() =>
            LivingEntityProperties.Inventory;
        
        public void SetInventory(Inventory.Inventory inventory) =>
            LivingEntityProperties.Inventory = inventory;

        public (double, double) GetPosition() =>
            (LivingEntityProperties.PositionX, LivingEntityProperties.PositionY);


        public double GetEyeDirection()
        {
            return LivingEntityProperties.EyeDirection;
        }

        public double GetDistanceToEntity(LivingEntity entity)
        {
            var xa = LivingEntityProperties.PositionX;
            var ya = LivingEntityProperties.PositionY;
            var xb = entity.LivingEntityProperties.PositionX;
            var yb = entity.LivingEntityProperties.PositionY;
            return Math.Sqrt(Math.Pow(xa - xb, 2) + Math.Pow(ya - yb, 2));
        }

        public double GetViewDistance() => LivingEntityProperties.ViewDistance;

        public double GetAttackDistance() => LivingEntityProperties.AttackDistance;

        public void SetEyeDirection(double eyeDirection) => 
            LivingEntityProperties.EyeDirection = eyeDirection;

        protected void Move((double, double) vector)
        {
            var position = GetPosition();
            var newX = position.Item1 + vector.Item1;
            var newY = position.Item2 + vector.Item2;

            if (!InMap(newX, newY))
            {
                SetEyeDirection(LivingEntityProperties.EyeDirection + 180);
                vector.Item1 *= -1;
                vector.Item2 *= -1;
                newX = position.Item1 + vector.Item1;
                newY = position.Item2 + vector.Item2;
            }

            SetPosition(newX, newY);
        }

        protected bool InMap(double x, double y) =>
            x >= 0 && x <= Render.Resolution.X && y >= 0 && y <= Render.Resolution.Y;


        protected void Idle()
        {
            var angle = LivingEntityProperties.EyeDirection * Math.PI / 180.0;
            var speed = LivingEntityProperties.Speed;
    
            const double maxDeviation = Math.PI / 4;
            var randomDeviation = (GameEngine.Random.NextDouble() - 0.5) * 2 * maxDeviation;
            var finalAngle = angle + randomDeviation;

            var dx = Math.Cos(finalAngle) * speed;
            var dy = Math.Sin(finalAngle) * speed;

            Move((dx, dy));
        }

        protected void ChasePlayer()
        {
            var (px, py) = Player.Instance.GetPosition();
            var ( ex,  ey) = GetPosition();
            var dx = px - ex;
            var dy = py - ey;

            var angle = Math.Atan2(dy, dx);
            var speed = LivingEntityProperties.Speed;

            dx = Math.Cos(angle) * speed;
            dy = Math.Sin(angle) * speed;

            Move((dx, dy));
        }
        
        protected void Attack(LivingEntity entity)
        {
            if (LivingEntityProperties.ItemStackInHand is EquippableItem currentItemStack)
            {
                currentItemStack.DecriesDurability(this);
            }
            entity.TakeDamage(LivingEntityProperties.Power, LivingEntityProperties.DamageRatio);
        }
        
        public void TakeDamage(double power, double damageRatio)
        {
            GetInventory().DecreaseArmorDurability();
            SetHealth(
                LivingEntityProperties.Health - 
                (power + damageRatio - LivingEntityProperties.RegenerationRatio)
                );
            if (LivingEntityProperties.Health <= 0)
            {
                if (!(this is Player))
                {
                    Player.Instance.AddMoney(100);
                    Player.Instance.AddCountOfKilledEnemies();
                }
                OnDie();
            }
        }

        public ItemStack GetItemStackInHand()
        {
            return LivingEntityProperties.ItemStackInHand;
        }
        
        public void SetItemStackInHand(ItemStack itemStack)
        {
            if (LivingEntityProperties.ItemStackInHand is EquippableItem currentItemStack)
            {
                if (currentItemStack.GetNameEquippableItem() == NameEquippableItem.Sword) 
                    currentItemStack.Unequip(this);
            }

            if (itemStack is EquippableItem newItemStack)
            {
                if(newItemStack.GetNameEquippableItem() == NameEquippableItem.Sword)
                    newItemStack.UseItem(this);
            }

            LivingEntityProperties.ItemStackInHand = itemStack;
        }
        
        public void SetPosition(double x, double y)
        {
            LivingEntityProperties.PositionX = x;
            LivingEntityProperties.PositionY = y;
        }

        public void SetViewDistance(double distance)
        {
            LivingEntityProperties.ViewDistance = distance;
        }

        public void SetAttackDistance(double distance)
        {
            LivingEntityProperties.AttackDistance = distance;
        }
        
        protected LivingEntityPropertiesStruct LoadLivingEntityProperties(string configPath, string nameEntity)
        {
            try
            {
                var data = File.ReadAllText(configPath);
                return JsonConvert.DeserializeObject<
                    Dictionary
                    <string, LivingEntityPropertiesStruct>
                >(data)[nameEntity];
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
                throw new Exception("Ошибка загрузки LivingEntityProperties");
            }
        }
        
        protected void GenerateRandomPosition()
        {
            double x = GameEngine.Random.NextDouble() * Render.Resolution.X;
            double y = GameEngine.Random.NextDouble() * Render.Resolution.Y;
            SetPosition(x, y);
        }
        
        protected (double, double) GetRandomPosition()
        {
            double x = GameEngine.Random.NextDouble() * Render.Resolution.X;
            double y = GameEngine.Random.NextDouble() * Render.Resolution.Y;
            return (x, y);
        }

        public abstract void Action();
        
        protected abstract void OnDie();
        
        
    }
}