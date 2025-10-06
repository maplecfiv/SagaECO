using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using SagaLib.VirtualFileSytem;

namespace SagaLib
{
    public abstract class FactoryList<K, T> where K : new() where T : new()
    {
        protected string databaseName = "";
        private Encoding encoding;
        private bool isFolder;
        protected string loadedTab = "";
        protected string loadingTab = "";
        private string path;

        public Dictionary<uint, List<T>> Items { get; set; } = new Dictionary<uint, List<T>>();

        public FactoryType FactoryType { get; set; }


        /// <summary>
        ///     Return an instance of
        /// </summary>
        public static K Instance
        {
            get => SingletonHolder.instance;
            set => SingletonHolder.instance = value;
        }

        protected abstract uint GetKey(T item);

        protected abstract void ParseCSV(T item, string[] paras);

        protected abstract void ParseXML(XmlElement root, XmlElement current, T item);

        public void Reload()
        {
            Items.Clear();
            Init(path, encoding, isFolder);
        }

        public void Init(string[] files, Encoding encoding)
        {
            var count = 0;
            this.encoding = encoding;
            switch (FactoryType)
            {
                case FactoryType.CSV:
                    foreach (var i in files)
                    {

                        count += InitCSV(i, encoding);
                    }
                    break;
                case FactoryType.XML:
                    foreach (var i in files)
                    {

                        count += InitXML(i, encoding);
                    }
                    break;
                default:
                    throw new Exception(string.Format("No FactoryType set for class:{0}", ToString()));
            }

            Logger.ProgressBarHide(count + loadedTab);
        }

        public void Init(string path, Encoding encoding, bool isFolder)
        {
            string[] files = null;
            var count = 0;
            this.path = path;
            this.encoding = encoding;
            this.isFolder = isFolder;
            if (isFolder)
            {
                var pattern = "*.*";
                switch (FactoryType)
                {
                    case FactoryType.CSV:
                        pattern = "*.csv";
                        break;
                    case FactoryType.XML:
                        pattern = "*.xml";
                        break;
                }
                files = VirtualFileSystemManager.Instance.FileSystem.SearchFile(path, pattern);
            }
            else
            {
                files = new string[1];
                files[0] = path;
            }

            switch (FactoryType)
            {
                case FactoryType.CSV:
                    foreach (var i in files)
                    {

                        count += InitCSV(i, encoding);
                    }
                    break;
                case FactoryType.XML:
                    foreach (var i in files)
                    {

                        count += InitXML(i, encoding);
                    }
                    break;
                default:
                    throw new Exception(string.Format("No FactoryType set for class:{0}", ToString()));
            }

            Logger.ProgressBarHide(count + loadedTab);
        }

        public void Init(string path, Encoding encoding)
        {
            Init(path, encoding, false);
        }

        private XmlElement FindRoot(XmlDocument doc)
        {
            foreach (var i in doc.ChildNodes)
            {

                if (i.GetType() != typeof(XmlElement))
                {
                    continue;
                }

                return (XmlElement)i;
            }
            return null;
        }

        private void ParseNode(XmlElement ele, T item)
        {
            XmlNodeList list;
            list = ele.ChildNodes;
            foreach (var j in list)
            {
                XmlElement i;
                if (j.GetType() != typeof(XmlElement))
                {
                    continue;
                }
                i = (XmlElement)j;
                ParseXML(ele, i, item);
                if (i.ChildNodes.Count != 0)
                {

                    ParseNode(i, item);
                }
            }
        }

        private int InitXML(string path, Encoding encoding)
        {
            var xml = new XmlDocument();
            var count = 0;
            try
            {
                XmlElement root;
                XmlNodeList list;
                xml.Load(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path));
                root = FindRoot(xml);
                list = root.ChildNodes;
                var time = DateTime.Now;
                var label = loadingTab;
                if (list.Count > 100)
                {

                    Logger.ProgressBarShow(0, (uint)list.Count, label);

                }
                foreach (var j in list)
                {
                    var item = new T();
                    XmlElement i;
                    if (j.GetType() != typeof(XmlElement))
                    {
                        continue;
                    }
                    i = (XmlElement)j;
                    ParseXML(root, i, item);
                    if (i.ChildNodes.Count != 0)
                    {

                        ParseNode(i, item);
                    }

                    var key = GetKey(item);
                    if (!Items.ContainsKey(key)) Items.Add(key, new List<T>());
                    Items[key].Add(item);

                    if ((DateTime.Now - time).TotalMilliseconds > 10)
                    {
                        time = DateTime.Now;
                        if (list.Count > 100)
                        {

                            Logger.ProgressBarShow((uint)count, (uint)list.Count, label);
                        }
                    }

                    count++;
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex.Message);
            }

            return count;
        }

        private int InitCSV(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            var count = 0;
            var lines = 0;
            var label = loadingTab;
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
            var time = DateTime.Now;
            while (!sr.EndOfStream)
            {
                string line;
                lines++;
                line = sr.ReadLine();
                string[] paras;
                try
                {
                    var item = new T();
                    if (line.IndexOf('#') != -1)
                    {

                        line = line.Substring(0, line.IndexOf('#'));
                    }

                    if (line == "")
                    {
                        continue;
                    }
                    paras = line.Split(',');
                    if (paras.Length < 2)
                    {

                        continue;
                    }

                    for (var i = 0; i < paras.Length; i++)
                    {

                        if (paras[i] == "")
                        {

                            paras[i] = "0";
                        }
                    }
                    ParseCSV(item, paras);

                    var key = GetKey(item);
                    if (!Items.ContainsKey(key)) Items.Add(key, new List<T>());
                    Items[key].Add(item);

                    if ((DateTime.Now - time).TotalMilliseconds > 10)
                    {
                        time = DateTime.Now;
                        Logger.ProgressBarShow((uint)sr.BaseStream.Position, (uint)sr.BaseStream.Length, label);
                    }

                    count++;
                }
                catch (Exception)
                {
                    Logger.ShowError("Error on parsing " + databaseName + " db!\r\n       File:" + path + ":" + lines +
                                     "\r\n       Content:" + line);
                }
            }

            sr.Close();
            return count;
        }

        /// <summary>
        ///     Sealed class to avoid any heritage from this helper class
        /// </summary>
        private sealed class SingletonHolder
        {
            internal static K instance = new K();

            /// <summary>
            ///     Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
            /// </summary>
            static SingletonHolder()
            {
            }
        }
    }
}