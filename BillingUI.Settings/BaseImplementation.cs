using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Configuration;
namespace BillingUI.Settings
{
    public class BaseImplementation
    {
        protected readonly IRepository _repository;
        protected readonly HttpContext _httpContext;
        private readonly IHostEnvironment _webHostEnvironment;
        //private IRepository Repository { get; set; }
        private Modules? _modules;

        public BaseImplementation(IRepository repository, IHttpContextAccessor contextAccessor,IHostEnvironment hostEnvironment)
        {
            _repository = repository;          
            _httpContext = contextAccessor.HttpContext;
            _webHostEnvironment = hostEnvironment;
        }


        public async Task<Modules> GetModules()
        {

            if (_modules == null)
            {
                //_modules = HttpContext.Session["BillingUI.Core.Modules"] as Modules;

                if (_modules == null)
                {
                    _modules = new Modules();
                    var assemblies = new List<string>();
                    var gb = _repository.Config.GlobalSettings;
                    foreach (var m in _repository.Config.GlobalSettings["Modules"].Split(','))
                    {
                        // Check if user is authorized for this module                     
                        var user = _httpContext.User.Identity?.Name?.ToLower();
                        var userRole = _repository.Config.GetValue("BillingUI_" + m + "_USERS", "a0016645");
                        if (string.IsNullOrEmpty(userRole))
                            continue;

                        //Load assembly if not already loaded
                        var assemblyCd = _repository.Config.GlobalSettings[m + "_Assembly"];
                        var assemblyPath = Path.Combine(_webHostEnvironment.ContentRootPath, "bin", $"BillingUI.{assemblyCd}.dll");

                        if (!File.Exists(assemblyPath))
                            continue;
                        var assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);

                        //Create module
                        var type = assembly.GetType("BillingUI." + assemblyCd + "." + m + "Module", false, true);
                        if (type == null || !type.IsSubclassOf(typeof(Modules)))
                            continue;

                        var module = (Modules)assembly.CreateInstance(type.FullName);
                        if (module == null)
                            continue;

                        //Load config file
                        var filename = Path.Combine(_webHostEnvironment.ContentRootPath, "~/bin/BillingUI." + assemblyCd + ".dll.config");
                        if (File.Exists(filename))
                        {
                            var fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = filename };
                            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                            foreach (var setting in configuration.AppSettings.Settings.AllKeys)
                                module.Settings.Add(setting, configuration.AppSettings.Settings[setting].Value);
                            foreach (AppSettingsSection section in configuration.Sections.OfType<AppSettingsSection>())
                            {
                                var settings = new Dictionary<string, string>();
                                foreach (var setting in section.Settings.AllKeys)
                                    settings.Add(setting, section.Settings[setting].Value);
                                module.SettingsGroup.Add(section.SectionInformation.Name, settings);
                            }
                        }

                        //Load permissions based on the user's role
                        //if (!_modules.AddPermissions(userRole, _repository.Config))
                        //    continue;

                        //Load settings from AppConfig database that weren't in the assembly's web.config file
                        //var appSettings = _repository.Config.GetSettings("BillingUI_" + m);
                        //if (appSettings != null)
                        //    foreach (var setting in appSettings.Keys)
                        //        if (!_modules.Settings.ContainsKey(setting))
                        //            _modules.Settings.Add(setting, appSettings[setting]);

                        //Load assembly wide settings from AppConfig database that weren't in the assembly's web.config file
                        //var assemblySettings = _repository.Config.GetSettings("BillingUI_" + assemblyCd);
                        //if (assemblySettings != null)
                        //    foreach (var setting in assemblySettings.Keys)
                        //        if (!module.Settings.ContainsKey(setting))
                        //            module.Settings.Add(setting, assemblySettings[setting]);

                        //Add module to list of available modules
                        _modules.Name = _repository.Config.GlobalSettings[m + "_Name"];
                        //_modules.Add(m, module);
                    }

                    //HttpContext.Session["BillingUI.Core.Modules"] = _modules;
                }
            }
            return _modules;
        }

    }
}
