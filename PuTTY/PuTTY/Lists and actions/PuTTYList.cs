using System.Collections.Generic;
using FARRCSharpSDK;
using FARRPlugin;

namespace PuTTY.Lists_and_actions
{
    class PuTTYList : ActionsList
    {
        public PuTTYList(FARRCSharpPluginBase pluginBase) : base(pluginBase, "^ssh")
        {
            PuTTYSessionHelper helper = new PuTTYSessionHelper("Registry;HKEY_CURRENT_USER\\Software\\9bis.com\\KiTTY\\Sessions");
            List<PuTTYSession> sessions = helper.GetSessions();
            foreach(PuTTYSession session in sessions)
            {
                SourceList.Add(new PuTTYAction(session));
            }
        }
    }
}
