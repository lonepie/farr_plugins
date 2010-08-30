using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace FileZilla.Lists_and_actions
{
    class FileZillaSite
    {
        private string m_name;
        private string m_path;
        private string m_url;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Path
        {
            get { return m_path; }
            set { m_path = value; }
        }

        public string Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        public FileZillaSite(string name, string url)
        {
            m_name = name;
            m_url = url;
            m_path = string.Format("0/{0}", name);
        }

        public FileZillaSite(DataRow row)
        {
            if (row != null && row.ItemArray.Length > 0)
            {
                m_name = row["Name"].ToString();
                m_url = GetUrl(row);
                m_path = string.Format("0/{0}", m_name);
            }
        }

        private static string GetUrl(DataRow row)
        {
            const string urlFormat = "{0}://{1}{2}:{3}/{4}";

            string protocol;
            switch(row["Protocol"].ToString())
            {
                case "3":
                    protocol = "ftpes";
                    break;
                case "2":
                    protocol = "ftps";
                    break;
                case "1":
                    protocol = "sftp";
                    break;
                default:
                    protocol = "ftp";
                    break;
            }
            string remoteDir = "";
            if (!string.IsNullOrEmpty(row["RemoteDir"].ToString()))
            {
                Regex rx = new Regex("(\\s*\\d+\\s)+");
                remoteDir = rx.Replace(row["RemoteDir"].ToString(), "/");
            }

            string credentials = "";
            if (!string.IsNullOrEmpty(row["User"].ToString()) && !string.IsNullOrEmpty(row["Pass"].ToString()))
                credentials = string.Format("{0}:{1}@", row["User"], row["Pass"]);


            return string.Format(urlFormat, protocol, credentials, row["Host"], row["Port"], remoteDir);
        }

        public static List<FileZillaSite> GetSites()
        {
            //ArrayList sites = new ArrayList();
            List<FileZillaSite> sites = new List<FileZillaSite>();
            string xmlPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.SiteMgr_Path);

            DataSet dsSites = new DataSet();
            dsSites.ReadXml(xmlPath);
            if (dsSites.Tables["Server"] != null)
            {
                foreach (DataRow row in dsSites.Tables["Server"].Rows)
                {
                    sites.Add(new FileZillaSite(row));
                }
            }

            return sites;
        }

        public static List<FileZillaSite> FilterSites(List<FileZillaSite> sites, string searchText)
        {
            if(sites.Count > 0)
            {
               return sites.FindAll(delegate(FileZillaSite site)
                                  {
                                      return Regex.IsMatch(site.Name, "\\A.*" + searchText + ".*\\z",
                                                           RegexOptions.IgnoreCase);
                                  });
            }

            return new List<FileZillaSite>();
        }

        public static List<FileZillaSite> GetFilteredSites(string searchText)
        {
            List<FileZillaSite> sites = GetSites();
            return FilterSites(sites, searchText);
        }
    }
}
