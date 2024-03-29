﻿using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IniLib
{
    public class IniFile
    {
        string Path;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile(string IniPath)
        {
            Path = new FileInfo(IniPath).FullName.ToString();
        }

        public string Read(string Key, string Section, string Default)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, Default, RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section) => WritePrivateProfileString(Section, Key, Value, Path);

        public void DeleteKey(string Key, string Section) => Write(Key, null, Section);

        public void DeleteSection(string Section) => Write(null, null, Section);

        public bool KeyExists(string Key, string Section) => Read(Key, Section, "").Length > 0;
        public void CreateIfNotExists(string Key, string Section, string defaultValue = "")
        {
            if (!KeyExists(Key, Section)) Write(Key, defaultValue, Section);
        }
    }
}