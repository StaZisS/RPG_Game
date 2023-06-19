using System.Windows.Forms;
using RPGProject.LivingEntity.NPC.DialogWindow;
using WindowsFormsApp1.GameEngine;

namespace RPGProject.LivingEntity
{
    public class Inhabitant : Npc, IHasDialogWindow
    {
        public Inhabitant()
        {
            InitializeNpc(NameNpc.Inhabitant);
        }

        protected override void StartDialog(Npc npc)
        {
            DialogWindow = new DialogWindowForInhabitant("Inhabitant");
            DialogWindow.Npc = npc;
            DialogWindow.ShowDialog();
        }

        public DialogWindow DialogWindow { get; set; }
    }
}