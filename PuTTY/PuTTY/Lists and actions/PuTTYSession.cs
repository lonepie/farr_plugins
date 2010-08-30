using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PuTTY.Lists_and_actions
{
    class PuTTYSession
    {
        private string m_name;
        private string m_hostname;
        private string m_protocol;

        public string Name
        {
            get { return m_name; }
        }
        public string Hostname
        {
            get { return m_hostname; }
        }
        public string Protocol
        {
            get { return m_protocol; }
        }

        public PuTTYSession(string name, string hostname, string protocol)
        {
            m_name = name;
            m_hostname = hostname;
            if(string.IsNullOrEmpty(hostname))
                m_hostname = name;
            m_protocol = protocol;
        }
    }

    class PuTTYSessionHelper
    {
        private PuTTYSessionStoreType m_storeType;
        private object m_sessionStore;

        public PuTTYSessionHelper()
        {
            SetupSessionStore(Properties.Settings.Default.PuTTY_Session_Store);
        }

        public PuTTYSessionHelper(string sessionStoreInfo)
        {
            SetupSessionStore(sessionStoreInfo);
        }

        private void SetupSessionStore(string sessionStoreInfo)
        {
            //try
            //{
                string[] info = sessionStoreInfo.Split(';');
                m_storeType = (PuTTYSessionStoreType) Enum.Parse(typeof (PuTTYSessionStoreType), info[0]);

                if(m_storeType == PuTTYSessionStoreType.Registry)
                {
                    if (info[1].StartsWith("HKEY_LOCAL_MACHINE"))
                        m_sessionStore = Registry.LocalMachine.OpenSubKey(info[1].Replace("HKEY_LOCAL_MACHINE\\", ""));
                    else if (info[1].StartsWith("HKEY_CURRENT_USER"))
                        m_sessionStore = Registry.CurrentUser.OpenSubKey(info[1].Replace("HKEY_CURRENT_USER\\", ""));
                    else
                        throw new Exception("Registry path not found - case sensitive!");
                }
                else
                {
                    if (Directory.Exists(info[1]))
                        m_sessionStore = new DirectoryInfo(info[1]);
                    else
                        throw new Exception("File path not found");
                }
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message);
            //}
        }

        public List<PuTTYSession> GetSessions()
        {
            List<PuTTYSession> sessions = new List<PuTTYSession>();

            if (m_sessionStore != null)
            {
                if (m_storeType == PuTTYSessionStoreType.Registry)
                {
                    RegistryKey key = (RegistryKey) m_sessionStore;

                    foreach(string subkeyName in key.GetSubKeyNames())
                    {
                        //try
                        //{
                            RegistryKey subkey = key.OpenSubKey(subkeyName);
                            string sessionName = subkeyName.Replace("%20", " ");
                            sessions.Add(new PuTTYSession(sessionName, subkey.GetValue("HostName").ToString(),
                                                          subkey.GetValue("Protocol").ToString()));
                        //}
                        //catch(Exception ex)
                        //{
                        //    MessageBox.Show("Registry Subkey error on " + subkeyName + ": " + ex.Message);
                        //}
                    }
                }
                else if (m_storeType == PuTTYSessionStoreType.Directory)
                {
                    DirectoryInfo di = (DirectoryInfo) m_sessionStore;
                    foreach(FileInfo fi in di.GetFiles())
                    {
                        //try
                        //{
                            StreamReader sr = fi.OpenText();
                            string fileContent = sr.ReadToEnd();
                            sr.Close();

                            Regex protoRx = new Regex("Protocol\\(\\w+)\\");
                            Regex hostnameRx = new Regex("HostName\\(\\w+)\\");
                            Match protoMatch = protoRx.Match(fileContent);
                            Match hostnameMatch = hostnameRx.Match(fileContent);

                            sessions.Add(new PuTTYSession(fi.Name,
                                                          hostnameMatch.Captures[hostnameMatch.Captures.Count - 1].Value,
                                                          protoMatch.Captures[protoMatch.Captures.Count - 1].Value));
                        //}
                        //catch(Exception ex)
                        //{
                        //    MessageBox.Show("File error:" + ex.Message);
                        //}
                    }
                }
            }

            return sessions;
        }
    }

    enum PuTTYSessionStoreType
    { Registry, Directory }
}
