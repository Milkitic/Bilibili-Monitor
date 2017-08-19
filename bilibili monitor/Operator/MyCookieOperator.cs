using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

using Monitor.Object;
using Monitor.Operator;

namespace Monitor.Operator
{
    static class MyCookieOperator
    {
        public static OBJ_Sess GetMyCookie()
        {
            OBJ_Sess myCookie;
            RegistryKey key = Registry.CurrentUser;
            RegistryKey software = key.CreateSubKey("software\\BiliSess\\MyAccount");

            string[] subkeyNames;
            subkeyNames = software.GetValueNames();

            if (subkeyNames.Length == 1)
            {
                string subkeyVals = software.GetValue(subkeyNames[0]).ToString();
                myCookie = new OBJ_Sess(subkeyNames[0], subkeyVals);
                return myCookie;
            }

            return null;
        }

        public static void UpdateMe()
        {
            Poster.MyAccount = GetMyCookie();
        }

        public static void SetMyCookie(string id, string cook)
        {
            RegistryKey key = Registry.CurrentUser;
            RegistryKey software = key.CreateSubKey("software\\BiliSess\\MyAccount");

            string[] subkeyNames;
            subkeyNames = software.GetValueNames();

            if (subkeyNames.Length > 0)
                for (int i = 0; i < subkeyNames.Length; i++)
                    Delete(subkeyNames[i]);

            if (id != null)
                Save(id, cook);
            else
                Save(subkeyNames.Length.ToString(), cook);

            key.Close();
        }

        private static void Delete(string name)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("software\\BiliSess\\MyAccount", true);
            key.DeleteValue(name);
            key.Close();
        }
        private static void Save(string id, string cook)
        {
            RegistryKey key = Registry.CurrentUser;
            RegistryKey software = key.OpenSubKey("software\\BiliSess\\MyAccount", true);
            software.SetValue(id, cook);
        }
    }
}
