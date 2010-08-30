using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using FARRPlugin;
using System.Drawing;
using System.Reflection;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;

namespace PuTTY.Lists_and_actions
{
    class PuTTYAction : ActionItem
    {
        private string m_caption;
        private readonly PuTTYSession m_session;

        public PuTTYAction(PuTTYSession session)
        {
            m_session = session;
        }

        protected override void GetDataRaw(ref string Group, ref string PathItem)
        {
            Icon = GetFilePluginPath("kitty.ico");
            Group = m_session.Protocol + ": " + m_session.Hostname;
            m_caption = m_session.Name;
        }

        public override void Execute(ref bool HideOnExecute)
        {
            string args = string.Format("-load \"{0}\"", m_session.Name);
            try
            {
                Process.Start(Environment.ExpandEnvironmentVariables(Properties.Settings.Default.PuTTY_Path), args);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error launching PuTTY: " + ex.Message);
            }
        }

        public override string CaptionRaw
        {
            get { return m_caption; }
        }
    }
}