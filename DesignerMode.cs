using HarmonyLib;
using Lib;
using System.Windows.Forms;
namespace DesignerMode
{
    public class DesignerMode : AuroraPatch.Patch
    {
        public override string Description => "Hit the save button to enter Designer Mode";
        protected override void Started()
        {
            // thanks to u/01010100 for help
            Control savebtn = TacticalMap.Controls.Find("cmdToolbarSave", true)[0];
            savebtn.Name = "cmdToolbarDesignerMode";
        }
    }
}