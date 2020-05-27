using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Coder.WebPusher
{
    public static class FileUtility
    {
        public static Regex folderRegex = new Regex("[\\\\/:\\*\\?\\\"\\<\\>\\|]");

        public static string GetStandardFileName(string messageType)
        {
            if (messageType == null) throw new ArgumentNullException(nameof(messageType));
            return folderRegex.Replace(messageType, "_");
        }

        public static void AutoCreateDirectory(string folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            var ary = folder.Split('/', '\\');
            var starck = new Stack<string>(ary);

            while (starck.Count != 0)
                if (!Directory.Exists(starck.Peek()))
                {
                    var parentDir = starck.Pop();
                    Directory.CreateDirectory(parentDir);
                }
        }
    }
}
