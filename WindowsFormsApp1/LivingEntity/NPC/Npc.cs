using System;
using System.Drawing;
using RPGProject.Inventory.Effect;
using WindowsFormsApp1.GameEngine;
using WindowsFormsApp1.Map;

namespace RPGProject.LivingEntity
{
    public abstract class Npc : LivingEntity, IGameObject
    {
        private readonly string _configPath = $"{Environment.CurrentDirectory}\\PropertyJson\\NPCCharacteristics.json";

        protected struct NpcPropertiesStruct
        {
            public NameNpc NameNpc { get; set; }
        }
        
        protected NpcPropertiesStruct NpcProperties { get; set; }
        
        protected override void OnDie()
        {
            Map.Instance.RemoveLivingEntity(this);
        }
        
        public static Npc GenerateRandomNpc()
        {
            return GetNpcByName(GenerateRandomNameNpc());
        }

        private static NameNpc GenerateRandomNameNpc()
        {
            var values = Enum.GetValues(typeof(NameNpc));
            return (NameNpc) values.GetValue(GameEngine.Random.Next(values.Length));
        }
        
        private static Npc GetNpcByName(NameNpc nameNpc)
        {
            switch (nameNpc)
            {
                case NameNpc.Blacksmith:
                    return new Blacksmith();
                case NameNpc.Dealer:
                    return new Dealer();
                case NameNpc.Inhabitant:
                    return new Inhabitant();
                default:
                    return new Blacksmith();
            }
        }
        
        public override void Action()
        {
            Effect.UpdateEffects(this);
            Idle();
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(
                Resources.GetFrame(NpcProperties.NameNpc.ToString()),
                (int)Math.Round(GetPosition().Item1),
                (int)Math.Round(GetPosition().Item2),
                100,
                100
            );
        }

        protected void InitializeNpc(NameNpc nameNpc)
        {
            var npcProperties = new NpcPropertiesStruct
            {
                NameNpc = nameNpc
            };
            NpcProperties = npcProperties;
            LivingEntityProperties = LoadLivingEntityProperties(
                _configPath,
                nameNpc.ToString()
            );
            LivingEntityProperties.EyeDirection = GameEngine.Random.NextDouble() * 360;
            LivingEntityProperties.Inventory = new Inventory.Inventory(this);
            LivingEntityProperties.Inventory
                .CreateRandomItemsAndPutThemInInventory(20);
            GenerateRandomPosition();
        }

        protected abstract void StartDialog(Npc npc);

        public void InteractWithPlayer()
        {
            GameEngine.Instance.SwitchIsRunning();
            StartDialog(this);
            GameEngine.Instance.SwitchIsRunning();
        }
    }
}