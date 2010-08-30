using System.Collections.Generic;
using FARRCSharpSDK;
using FARRPlugin;

namespace FileZilla.Lists_and_actions
{
    class FileZillaList : ActionsList
    {
        public FileZillaList(FARRCSharpPluginBase pluginBase)
            : base(pluginBase, "^ftp")
        {
            List<FileZillaSite> sites = FileZillaSite.GetSites();
            foreach(FileZillaSite site in sites)
            {
                SourceList.Add(new FileZillaAction(site));
            }
        }
    }



}
