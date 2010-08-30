using System;
using System.Diagnostics;
using System.Windows.Forms;
using FARRPlugin;

namespace FileZilla.Lists_and_actions
{
    class FileZillaAction : ActionItem
    {
        private readonly FileZillaSite m_site;
        private string m_caption;
        public FileZillaAction(FileZillaSite site)
        {
            m_site = site;
        }


        protected override void GetDataRaw(ref string Group, ref string PathItem)
        {
            Group = m_site.Url;
            Icon = GetFilePluginPath("filezilla.ico");
            m_caption = m_site.Name;
        }

        public override void Execute(ref bool HideOnExecute)
        {
            string args = string.Format("-c \"{0}\"", m_site.Path);
            try
            {
                Process.Start(
                    Environment.ExpandEnvironmentVariables(Properties.Settings.Default.FileZilla_Path), args);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error launching: " + ex.Message);
            }
        }

        public override string CaptionRaw
        {
            get { return m_caption; }
        }
    }
}