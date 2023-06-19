

using System;
using System.Drawing;
using System.Linq;
using RPGProject.Inventory;
using RPGProject.Inventory.Effect;
using RPGProject.Inventory.Items.EquippableItem;
using RPGProject.Inventory.Items.NullItem;
using RPGProject.Inventory.Items.UsableItem;
using RPGProject.LivingEntity;
using RPGProject.SubscriptionService;
using WindowsFormsApp1.GameEngine;
using WindowsFormsApp1.Map;

public class Player : LivingEntity, IGameObject
{
    private static readonly Lazy<Player> InstanceHolder = new Lazy<Player>(() => new Player());

    private Player()
    {
        var livingEntityProperties = new LivingEntityPropertiesStruct
            {
                Health = 100,
                Food = 100,
                Speed = 40,
                Power = 100,
                MaxHealth = 100,
                MaxFood = 100,
                MaxSpeed = 60,
                MaxPower = 100,
                DamageRatio = 0,
                AttackDistance = 200,
                RegenerationRatio = 0,
                EyeDirection = -1,
                PositionX = 100.0,
                PositionY = 100.0,
                Money = 2000,
                ItemStackInHand = new NullItem()
            };
        SetLivingEntityProperties(livingEntityProperties);
        SetInventory(new Inventory(this));
        
        GetInventory().AddItem(new UsableItem(NameUsableItem.Meat, 64));
        GetInventory().AddItem(new EquippableItem(NameEquippableItem.Helmet));
        GetInventory().AddItem(new EquippableItem(NameEquippableItem.Boots));
        GetInventory().AddItem(new EquippableItem(NameEquippableItem.BodyArmor));
        GetInventory().AddItem(new EquippableItem(NameEquippableItem.Leggings));
        GetInventory().AddItem(new UsableItem(NameUsableItem.Mushroom, 23));
        GetInventory().AddItem(new UsableItem(NameUsableItem.Soup, 15));
        GetInventory().AddItem(new UsableItem(NameUsableItem.TreatmentPotion, 5));
        GetInventory().AddItem(new UsableItem(NameUsableItem.AgilityPotion, 5));
        GetInventory().AddItem(new UsableItem(NameUsableItem.PowerPotion, 5));
        GetInventory().AddItem(new EquippableItem(NameEquippableItem.Sword));
    }
    
    private const double EyeDirectionStops = -1;
    
    public static Player Instance => InstanceHolder.Value;
    
    public override void Action()
    {
        SetFood(GetFood() - 0.01);
        Effect.UpdateEffects(this);
        UpdateQuest();
        MakeStep();
    }

    protected override void OnDie()
    {
        GameEngine.GameOver();
    }

    public void TryAttack()
    {
        var enemyToAttack = Map.Instance.GetEnemies().ToList();
        foreach (var enemy in enemyToAttack)
        {
            if (GetDistanceToEntity(enemy) <= LivingEntityProperties.AttackDistance)
            {
                Attack(enemy);
                return;
            }
        }
    }

    private void UpdateQuest()
    {
        var quest = Player.Instance.GetSubscriptions(TypeSubscription.QuestAction).ToList();
        foreach (var questAction in quest)
        {
            questAction.Invoke();
        }
    }

    public static void MoveUp() => Instance.SetEyeDirection(90);

    public static void MoveDown() => Instance.SetEyeDirection(270);

    public static void MoveLeft() => Instance.SetEyeDirection(180);

    public static void MoveRight() => Instance.SetEyeDirection(0);

    public static void MoveUpLeft() => Instance.SetEyeDirection(135);

    public static void MoveUpRight() => Instance.SetEyeDirection(45);

    public static void MoveDownLeft() => Instance.SetEyeDirection(225);

    public static void MoveDownRight() => Instance.SetEyeDirection(315);

    public static void Stop() => Instance.SetEyeDirection(EyeDirectionStops);
    
    private void MakeStep()
    {
        if (LivingEntityProperties.EyeDirection == EyeDirectionStops) return;

        var angle = LivingEntityProperties.EyeDirection * -Math.PI / 180.0;
        var stepSize = LivingEntityProperties.Speed;

        var dx = Math.Cos(angle) * stepSize;
        var dy = Math.Sin(angle) * stepSize;

        if (SwitchMap(dx, dy)) return;
        
        Move((dx, dy));
    }

    private bool SwitchMap(double dx, double dy)
    {
        double newX = LivingEntityProperties.PositionX + dx;
        double newY = LivingEntityProperties.PositionY + dy;

        if (!InMap(newX, newY))
        {
            DirectionOfMove direction;
            if (newX < 0)
            {
                direction = DirectionOfMove.Left;
                newX = Render.Resolution.X - Math.Abs(newX);
            }
            else if (newX >= Render.Resolution.X)
            {
                direction = DirectionOfMove.Right;
                newX -= Render.Resolution.X;
            }
            else if (newY < 0)
            {
                direction = DirectionOfMove.Up;
                newY = Render.Resolution.Y - Math.Abs(newY);
            }
            else
            {
                direction = DirectionOfMove.Down;
                newY -= Render.Resolution.Y;
            }

            GameEngine.Instance.SwitchIsRunning();
            
            Map.Instance.SwitchCell(direction);
            
            GameEngine.Instance.SwitchIsRunning();
            
            SetPosition(newX, newY);
            return true;
        }

        return false;
    }

    public void Draw(Graphics g)
    {
        g.DrawImage(
            Resources.GetFrame("Player"),
            (int)Math.Round(GetPosition().Item1),
            (int)Math.Round(GetPosition().Item2),
            100,
            100
        );
    }

    public void TryInteractWithNpc()
    {
        var nearestNpc = Map.Instance.GetNearestNpc();
        if(nearestNpc == null) return;
        if (GetDistanceToEntity(nearestNpc) <= LivingEntityProperties.AttackDistance)
        {
            nearestNpc.InteractWithPlayer();
        }
    }
}