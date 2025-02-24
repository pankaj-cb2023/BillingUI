
using System.Reflection;

namespace BillingUICore.Server.Infrastructure
{
    //public class BaseImplementation
    //{
    //    protected IRepository Repository { get; private set; }

    //    private Modules _modules;
    //    public async Task<Modules> GetModules()
    //    {
    //        if (_modules == null)
    //        { 
    //            if (_modules == null)
    //            {
    //                _modules = new Modules();
    //                var assemblies = new List<string>();

    //                foreach (var m in Repository.Config.GlobalSettings["Modules"].Split(','))
    //                {
    //                    // Check if user is authorized for this module
    //                    var user = HttpContext.CurrentUser().ToLower();
    //                    var userRole = Repository.Config.GetValue("BillingUI_" + m + "_USERS", user);
    //                    if (string.IsNullOrEmpty(userRole))
    //                        continue;

    //                    // Load assembly if not already loaded
    //                    var assemblyCd = Repository.Config.GlobalSettings[m + "_Assembly"];
    //                    var assemblyPath = HttpContext.Server.MapPath("~/bin/BillingUI." + assemblyCd + ".dll");
    //                    if (!File.Exists(assemblyPath))
    //                        continue;
    //                    var assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);

    //                    // Create module 
    //                    var type = assembly.GetType("BillingUI." + assemblyCd + "." + m + "Module", false, true);
    //                    if (type == null || !type.IsSubclassOf(typeof(Module)))
    //                        continue;

    //                    var module = (Module)assembly.CreateInstance(type.FullName);
    //                    if (module == null)
    //                        continue;

    //                    // Load config file
    //                    var filename = HttpContext.Server.MapPath("~/bin/BillingUI." + assemblyCd + ".dll.config");
    //                    if (File.Exists(filename))
    //                    {
    //                        var fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = filename };
    //                        var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
    //                        foreach (var setting in configuration.AppSettings.Settings.AllKeys)
    //                            module.Settings.Add(setting, configuration.AppSettings.Settings[setting].Value);
    //                        foreach (AppSettingsSection section in configuration.Sections.OfType<AppSettingsSection>())
    //                        {
    //                            var settings = new Dictionary<string, string>();
    //                            foreach (var setting in section.Settings.AllKeys)
    //                                settings.Add(setting, section.Settings[setting].Value);
    //                            module.SettingsGroup.Add(section.SectionInformation.Name, settings);
    //                        }
    //                    }

    //                    // Load permissions based on the user's role
    //                    if (!module.AddPermissions(userRole, Repository.Config))
    //                        continue;

    //                    // Load settings from AppConfig database that weren't in the assembly's web.config file
    //                    var appSettings = Repository.Config.GetSettings("BillingUI_" + m);
    //                    if (appSettings != null)
    //                        foreach (var setting in appSettings.Keys)
    //                            if (!module.Settings.ContainsKey(setting))
    //                                module.Settings.Add(setting, appSettings[setting]);

    //                    // Load assembly wide settings from AppConfig database that weren't in the assembly's web.config file
    //                    var assemblySettings = Repository.Config.GetSettings("BillingUI_" + assemblyCd);
    //                    if (assemblySettings != null)
    //                        foreach (var setting in assemblySettings.Keys)
    //                            if (!module.Settings.ContainsKey(setting))
    //                                module.Settings.Add(setting, assemblySettings[setting]);

    //                    // Add module to list of available modules
    //                    module.Name = Repository.Config.GlobalSettings[m + "_Name"];
    //                    _modules.Add(m, module);
    //                }

    //                HttpContext.Session["BillingUI.Core.Modules"] = _modules;
    //            }
    //        }
    //        return _modules;
    //    }
    //}
}
