using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Settings
{
    public class Modules
    {
        private List<string>? _permissions;
        private List<MenuItem>? _menuitems;

        private  IEnumerable<string>? AllPermissions { get; set; }
        private  IEnumerable<MenuItem> AllMenuItems { get; }

        public string ModuleCd { get; private set; }

        public string Name { get; set; }

        public IDictionary<string, string> Settings { get; private set; }

        public IDictionary<string, IDictionary<string, string>> SettingsGroup { get; private set; }

        public IEnumerable<MenuItem> MenuItems { get { return _menuitems; } }

        //public Module(string moduleCd)
        //{
        //    _permissions = new List<string>();
        //    _menuitems = new List<MenuItem>();
        //    ModuleCd = moduleCd;
        //    Settings = new Dictionary<string, string>();
        //    SettingsGroup = new Dictionary<string, IDictionary<string, string>>();
        //}

        public bool HasPermission(string permission)
        {
            return _permissions.Contains(permission);
        }

        internal bool AddPermissions(string userRole, IConfigRepository config)
        {
            //foreach (var p in AllPermissions)
            //{
            //    var roles = config.GlobalSettings[ModuleCd + "_Roles_" + p];
            //    if (!string.IsNullOrEmpty(roles) && roles.Split(',').Contains(userRole))
            //        _permissions.Add(p);
            //}
            //foreach (var menuItem in AllMenuItems)
            //{
            //    if (HasPermission(menuItem.Permission))
            //        _menuitems.Add(menuItem);
            //}
            //return _permissions.Count > 0 && _menuitems.Count > 0;
            return true;
        }
    }

}

