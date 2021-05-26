using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Aurora;
using Aurora.Properties;
using Lib;
using HarmonyLib;



namespace DesignerMode
{
    public class DesignerMode : AuroraPatch.Patch
    {
        protected override void Loaded(Harmony harmony)
        {
            LogInfo("Loading patch DesignerMode...");

            // jt.cs is TacticalMap.cs
            // probably supposed to use knowledgebase but i dunno how
            Type tacmap = AuroraAssembly.GetType("jt");

            HarmonyMethod designerModePostfixMethod = new HarmonyMethod(GetType().GetMethod("DesignerModePostfix", AccessTools.all));

            // one of the methods called "c" is the big 
            //  drawing function that creates the UI
            IEnumerable<MethodInfo> allthecs = tacmap.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(c => c.Name == "c");
            MethodInfo methodtopostfix = null;

            // all i know to do is look for the private "c" with zero args
            foreach (MethodInfo thismethod in allthecs)
            {
                int i = thismethod.GetParameters().Length;
                if (i == 0)
                {
                    methodtopostfix = thismethod;
                }
            }

            if (methodtopostfix != null)
            {
                LogInfo("We found it");
                harmony.Patch(methodtopostfix, postfix: designerModePostfixMethod);
            }
            else
            {
                LogInfo("Didn't patch anything as methodtopostfix is null");
            }
        }


        private static void DesignerModePostfix(Form __instance)
        {
            Control savebtn = __instance.Controls.Find("cmdToolbarSave", true)[0];
            savebtn.Name = "cmdToolbarDesignerMode";
        }

    }

}