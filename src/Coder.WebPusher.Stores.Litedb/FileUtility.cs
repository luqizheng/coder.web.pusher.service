﻿using System;
using System.Collections.Generic;
using System.IO;
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
            var sta = new Stack<string>(ary);

            while (sta.Count != 0)
            {
                var parentDir = sta.Pop();
                if (!Directory.Exists(parentDir)) Directory.CreateDirectory(parentDir);
            }
        }
    }
}
