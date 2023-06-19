using System.Windows.Forms;
using RPGProject.LivingEntity.NPC.DialogWindow;
using WindowsFormsApp1.GameEngine;

namespace RPGProject.LivingEntity
{
    public class Dealer : Npc, IHasDialogWindow
    {
        public Dealer()
        {
            InitializeNpc(NameNpc.Dealer);
        }

        protected override void StartDialog(Npc npc)
        {
            DialogWindow = new DialogWindowForDealer("Dealer");
            DialogWindow.Npc = npc;
            DialogWindow.ShowDialog();
        }

        public DialogWindow DialogWindow { get; set; }
    }
}