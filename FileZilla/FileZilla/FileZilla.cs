using FARRCSharpSDK;
using FileZilla.Lists_and_actions;

namespace FileZilla
{
    public class FileZilla : FARRCSharpPluginBase
    {
        protected override void SetGeneralInfoParameters(ref string displayName, ref string versionString, ref string releaseDateString,
                                                         ref string author, ref string homepageURL, ref string updateURL,
                                                         ref string shortDescription, ref string longDescription, ref string advConfigString,
                                                         ref string readMeString, ref string iconFilename)
        {
            displayName = "FileZilla plugin";
            versionString = GetValueFromDCUpdate(EDCUpdateValues.Version);
            releaseDateString = "August 11, 2010";
            author = "Jon Rogers (C# SDK framework by Vitaly Belman)";
            homepageURL = GetValueFromDCUpdate(EDCUpdateValues.WebPage);
            updateURL = GetValueFromDCUpdate(EDCUpdateValues.UpdateFile);
            shortDescription = "FileZilla Site Manager Plugin";
            longDescription = "Search Filezilla Site Manager entries in FARR";
            iconFilename = "filezilla.ico";
        }

        public FileZilla()
            : base(typeof(FileZillaList))
        {
        }
    }
}
