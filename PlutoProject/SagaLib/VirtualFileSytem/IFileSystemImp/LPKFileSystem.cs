using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using SagaLib.VirtualFileSystem.Lpk;

namespace SagaLib.VirtualFileSystem
{
    public class LPKFileSystem : IFileSystem
    {
        private LpkFile lpk;

        #region IFileSystem Members

        public bool Init(string path)
        {
            try
            {
                lpk = new LpkFile(new FileStream(path, FileMode.Open, FileAccess.Read));
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
                return false;
            }

            return true;
        }

        public Stream OpenFile(string path)
        {
            path = path.Replace("/", "\\");
            if (lpk.Exists(path)) return lpk.OpenFile(path);

            throw new IOException("Cannot find file:" + path);
        }

        public string[] SearchFile(string path, string pattern)
        {
            return SearchFile(path, pattern, SearchOption.AllDirectories);
        }

        public string[] SearchFile(string path, string pattern, SearchOption option)
        {
            var files = lpk.GetFileNames;
            var result = new List<string>();
            if (path.Substring(path.Length - 1) != "/" && path.Substring(path.Length - 1) != "\\")
                path = path + "\\";
            path = path.Replace("/", "\\");
            pattern = pattern.Replace("*", "\\w*");
            foreach (var i in files)
                if (i.Name.StartsWith(path))
                {
                    var s = i.Name.Replace(path, "");
                    var token = s.Split('\\');
                    if (option == SearchOption.TopDirectoryOnly && token.Length > 1)
                        continue;
                    var filename = token[token.Length - 1];
                    if (Regex.IsMatch(filename, pattern, RegexOptions.IgnoreCase))
                        result.Add(i.Name);
                }

            return result.ToArray();
        }

        public void Close()
        {
            lpk.Close();
        }

        #endregion
    }
}