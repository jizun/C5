using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Ziri.DAL;
using Ziri.MDL;
using Microsoft.Win32;

namespace Ziri.BLL
{
    public class Utils  // Utility实用 Utils通用 Tools专用
    {
        public static void CreateDirectory(string infoPath)
        {
            DirectoryInfo directoryInfo = Directory.GetParent(infoPath);
            if (!directoryInfo.Exists) { directoryInfo.Create(); }
        }

        public static string MessageToString(List<string> Message)
        {
            string MessageString = "";
            foreach (var item in Message) { MessageString += "<li>" + item + "</li>"; }
            return "<ul>" + MessageString + "</ul>";
        }

        //枚举绑定到选择列表
        public static void EnumToSelectList(object SelectObject, Type EnumType)
        {
            string[] EnumText = Enum.GetNames(EnumType);
            int[] EnumValue = (int[])Enum.GetValues(EnumType);

            if (SelectObject.GetType().Name == typeof(CheckBoxList).Name)
            {
                for (int i = 0; i < EnumText.Length; i++)
                {
                    ((CheckBoxList)SelectObject).Items.Add(new ListItem(EnumText[i], EnumValue[i].ToString()));
                }
            }
            else if (SelectObject.GetType().Name == typeof(RadioButtonList).Name)
            {
                for (int i = 0; i < EnumText.Length; i++)
                {
                    ((RadioButtonList)SelectObject).Items.Add(new ListItem(EnumText[i], EnumValue[i].ToString()));
                }
            }
            else if (SelectObject.GetType().Name == typeof(DropDownList).Name)
            {
                for (int i = 0; i < EnumText.Length; i++)
                {
                    ((DropDownList)SelectObject).Items.Add(new ListItem(EnumText[i], EnumValue[i].ToString()));
                }
            }
        }

        //GET MIME，.Net 4.5以上可用 MimeMapping.GetMimeMapping(httpFile.FileName);
        public static string GetMime(string FileExtName)
        {
            string mimeType = "application/unknown";
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(FileExtName);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }
    }
}
