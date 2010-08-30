using FARRCSharpSDK;
using PuTTY.Lists_and_actions;

namespace PuTTY
{
    public class PuTTY : FARRCSharpPluginBase
    {
        protected override void SetGeneralInfoParameters(ref string displayName, ref string versionString, ref string releaseDateString,
                                                         ref string author, ref string homepageURL, ref string updateURL,
                                                         ref string shortDescription, ref string longDescription, ref string advConfigString,
                                                         ref string readMeString, ref string iconFilename)
        {
            displayName = "PuTTY plugin";
            versionString = GetValueFromDCUpdate(EDCUpdateValues.Version);
            releaseDateString = "August 16, 2010";
            author = "Jonathon Rogers (C# SDK framework by Vitaly Belman)";
            homepageURL = GetValueFromDCUpdate(EDCUpdateValues.WebPage);
            updateURL = GetValueFromDCUpdate(EDCUpdateValues.UpdateFile);
            shortDescription = "PuTTY Sessions";
            longDescription = "Launch PuTTY sessions via FARR";
            iconFilename = "kitty.ico";
        }

        public PuTTY()
            : base(typeof(PuTTYList))
        {
        }
    }
}
