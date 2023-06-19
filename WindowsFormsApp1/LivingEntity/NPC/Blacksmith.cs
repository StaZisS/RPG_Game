using System.Windows.Forms;
using RPGProject.LivingEntity.NPC.DialogWindow;
using WindowsFormsApp1.GameEngine;

namespace RPGProject.LivingEntity
{
    public class Blacksmith : Npc, IHasDialogWindow
    {
        public Blacksmith()
        {
            InitializeNpc(NameNpc.Blacksmith);
        }

        protected override void StartDialog(Npc npc)
        {
            DialogWindow = new DialogWindowForBlacksmith("Blacksmith");
            DialogWindow.Npc = npc;
            DialogWindow.ShowDialog();
        }

        public DialogWindow DialogWindow { get; set; }
    }
}