using System;
using System.Text;
using System.Xml;
using SagaLib;

namespace SagaValidation {
    public class Configuration : Singleton<Configuration> {
        string dbhost, dbuser, dbpass, dbname;
        int dbport, port, loglevel, dbType;
        string encoding;
        string password = "saga";
        string servername, serverip;
        string clientgameversion = "All";
        bool serverclose = false;

        SagaLib.Version version;

        public string DBHost {
            get { return dbhost; }
            set { dbhost = value; }
        }

        public string DBUser {
            get { return dbuser; }
            set { dbuser = value; }
        }

        public string DBPass {
            get { return dbpass; }
            set { dbpass = value; }
        }

        public string DBName {
            get { return dbname; }
            set { dbname = value; }
        }

        public string Password {
            get { return password; }
            set { password = value; }
        }

        public int DBPort {
            get { return dbport; }
            set { dbport = value; }
        }

        public int Port {
            get { return port; }
            set { port = value; }
        }

        public int DBType {
            get { return dbType; }
            set { dbType = value; }
        }

        public string ServerName {
            get { return servername; }
            set { servername = value; }
        }

        public string ServerIP {
            get { return serverip; }
            set { serverip = value; }
        }

        public SagaLib.Version Version {
            get { return version; }
            set { version = value; }
        }

        public string ClientGameVersion {
            get { return clientgameversion; }
            set { clientgameversion = value; }
        }

        public bool ServerClose {
            get { return serverclose; }
            set { serverclose = value; }
        }

        public string DBEncoding {
            get {
                if (encoding == null) {
                    encoding = Encoding.UTF8.EncodingName;
                    Logger.ShowDebug("DB Encoding not set, set to default value: " + encoding, null);
                }

                return encoding;
            }
            set { encoding = value; }
        }

        public int LogLevel {
            get { return loglevel; }
            set { loglevel = value; }
        }


        public void Initialization(string path) {
            // XmlDocument xml = new XmlDocument();
            try {
                XmlElement root;
                XmlNodeList list;
                bool getVersion = false;
                bool nullClientGameVersion = false;
                // xml.Load(path);
                root = ConfigLoader.LoadConfig("SagaValidation");
                list = root.ChildNodes;
                foreach (object j in list) {
                    XmlElement i;
                    if (j.GetType() != typeof(XmlElement)) continue;
                    i = (XmlElement)j;
                    switch (i.Name.ToLower()) {
                        case "dbtype":
                            dbType = int.Parse(i.InnerText);
                            break;
                        case "port":
                            port = int.Parse(i.InnerText);
                            break;
                        case "dbhost":
                            dbhost = i.InnerText;
                            break;
                        case "dbport":
                            dbport = int.Parse(i.InnerText);
                            break;
                        case "dbuser":
                            dbuser = i.InnerText;
                            break;
                        case "dbpass":
                            dbpass = i.InnerText;
                            break;
                        case "dbname":
                            dbname = i.InnerText;
                            break;
                        case "dbencoding":
                            encoding = i.InnerText;
                            break;
                        case "password":
                            password = i.InnerText;
                            break;
                        case "loglevel":
                            loglevel = int.Parse(i.InnerText);
                            break;
                        case "version":
                            try {
                                version = (SagaLib.Version)Enum.Parse(typeof(SagaLib.Version), i.InnerText);
                                getVersion = true;
                            }
                            catch {
                                Logger.GetLogger().Warning(string.Format(
                                    "Cannot find Version:[{0}], using default version:[{1}]", i.InnerText, version));
                            }

                            break;
                        case "clientgameversion":
                            clientgameversion = i.InnerText;
                            if (clientgameversion == "All" || clientgameversion == "") {
                                clientgameversion = "All";
                                nullClientGameVersion = true;
                            }

                            break;
                        case "servername":
                            servername = i.InnerText;
                            break;
                        case "serverip":
                            serverip = i.InnerText;
                            break;
                        case "serverclose":
                            serverclose = bool.Parse(i.InnerText);
                            break;
                    }
                }

                if (nullClientGameVersion)
                    Logger.GetLogger().Warning("ClientGameVersion is undefined, accepting all version.");
                Logger.GetLogger().Information("Done reading configuration...");
            }
            catch (Exception ex) {
                Logger.GetLogger().Error(ex, ex.Message);
            }
        }
    }
}