using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;


namespace BetterGovernors
{
    public class BetterGovernors : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Better Governors Test",
                new TextObject("Better Governors Test", null),
                9990,
                () => { InformationManager.DisplayMessage(new InformationMessage("Better Governors Test Successful!")); },
                () => { return (false, null); }));
        }
    }
}
