using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Configuration;

namespace BillingUI.Settings
{

    public interface IConfigRepository
    {
        Dictionary<string, string> GlobalSettings { get;}
        IDictionary<string, string> GetSettings(string groupCode);
        string GetValue(string groupCode, string settingCode);
    }

    public class ConfigRepository : IConfigRepository
    {
        private readonly AppConfig _config;

        public ConfigRepository()
        {
            _config = new AppConfig(AppConfigSourceTypeEnum.AresCommonService);
        }


        public Dictionary<string, string> GlobalSettings
        {
            get
            {
                return (Dictionary<string, string>)GetSettings("BILLINGUI");
            }
        }

        

        public IDictionary<string, string> GetSettings(string groupCode)
        {
            var settings =  _config.GetObjects(groupCode, false).Result?.ToDictionary(x => x.Key, y => y.Value.Value) ?? new Dictionary<string, string>();

            foreach (var setting in settings.ToArray())
            {
                var overrideValue = ConfigurationManager.AppSettings[setting.Key];
                if (overrideValue != null)
                {
                    settings[setting.Key] = overrideValue;
                }
            }

            return settings;
        }

        public  string GetValue(string groupCode, string settingCode)
        {
            return  _config.GetObject(groupCode, settingCode, false).Result.Value;
        }
    }

    public class AppConfig
    {
        public delegate IList<Setting> OnGetSettingsByGroupEvent(string groupCode);
        public OnGetSettingsByGroupEvent OnGetSettingsByGroup;

        private readonly IMemoryCache _cache;
        private readonly string _serviceUrl;
        private readonly AppConfigSourceTypeEnum _sourceType;
        private readonly string _subGroupCode;


        public AppConfig(AppConfigSourceTypeEnum sourceType, string subGroupCode = null, string serviceUrl = null, IMemoryCache cache = null)
        {
            _sourceType = sourceType;
            _subGroupCode = subGroupCode;
            _serviceUrl = "http://ld-billing01:8084/DEV/CommonService/v1";
            _cache = cache ?? new MemoryCache(new MemoryCacheOptions());
        }

        public async Task<Setting> GetObject(string groupCode, string settingCode, bool loadFromCache = true)
        {
            var settings = await GetSettingsList(groupCode, loadFromCache);
            return settings != null && settings.ContainsKey(settingCode) ? settings[settingCode] : null;
        }

        public async Task<IDictionary<string, Setting>> GetObjects(string groupCode, bool loadFromCache = true)
        {
            return await GetSettingsList(groupCode, loadFromCache);
        }

        private async Task<IDictionary<string, Setting>> GetSettingsList(string groupCode, bool loadFromCache = true)
        {
            if (loadFromCache && _cache.TryGetValue(groupCode, out IDictionary<string, Setting> cachedSettings))
            {
                return cachedSettings;
            }

            IDictionary<string, Setting> settings = null;

            if (_sourceType == AppConfigSourceTypeEnum.AresCommonService)
            {
                var headers = new Dictionary<string, string>
                {
                    { "X-Source-App", "BillingUI.Settings.ConfigRepository" },
                    { "X-Options", "NoHttpStatusCodeInjection" }
                };

                Dictionary<string, string> queryStringList = new Dictionary<string, string>();

                queryStringList.Add("groupCode", groupCode + (string.IsNullOrWhiteSpace(_subGroupCode)
                    ? string.Empty : "%2F" + _subGroupCode));

                List<KeyValuePair<string, string>> dd = new List<KeyValuePair<string, string>>();


                var service = new CustomClient(_serviceUrl, headers);
                var request = new CustomClientRequest
                {
                    QueryStringList = queryStringList,
                    ReturnType = typeof(List<Setting>)
                };


                using var httpClient = new HttpClient { BaseAddress = new Uri(_serviceUrl) };
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                WebServiceResponse wsResponse = await service.DownloadDataAsync("/appconfigs/getSettingsByGroup", request);

                var wsResponseData = JsonConvert.DeserializeObject<SettingResponse>(wsResponse.Data.ToString()!);

                if (wsResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //List<SettingResponse> response = (List<SettingResponse>)wsResponse.Data;

                    if (wsResponseData.Value.Count > 0)
                    {
                        settings = PopulateSettingList(wsResponseData.Value);
                    }
                }
                else
                {
                    throw new Exception(string.Format("Remote Service Error: {0} - {1}", wsResponse.StatusCode, wsResponse.ErrorMessage));
                }
            }
            else if (_sourceType == AppConfigSourceTypeEnum.Custom)
            {
                if (OnGetSettingsByGroup == null)
                {
                    throw new InvalidOperationException("OnGetSettingsByGroup event handler is not set.");
                }

                var settingsList = OnGetSettingsByGroup(groupCode);
                settings = settingsList?.ToDictionary(s => s.Code, s => s);
            }

            return settings ?? new Dictionary<string, Setting>();
        }      

       
        private IDictionary<string, Setting> PopulateSettingList(IList<Setting> sourceList)
        {
            IDictionary<string, Setting> settingList = new Dictionary<string, Setting>();

            foreach (Setting setting in sourceList)
            {
                settingList.Add(setting.Code, setting);
            }
            return settingList;
        }
    }

    public class WebServiceResponse
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class CustomClientRequest
    {

        //public List<KeyValuePair<string, string>> QueryStringList { get; set; }
        public Type ReturnType { get; set; }
        public Dictionary<string, string>? QueryStringList { get; set; }
    }


    public class SettingResponse
    {
        public SettingResponse()
        {
            List<Setting> setting = new List<Setting>();
        }
        public List<Setting> Value { get; set; }

    }

    public class Setting
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string? TypeCode { get; set; }
        public string CreatedBy { get; set; }        
        public string ModifiedBy { get; set; }


    }

    public enum AppConfigSourceTypeEnum
    {
        AresCommonService,
        Custom
    }

    public class ListResponse<T> : BaseResponse
    {
        public List<T> Value;

        /// <summary>
        /// Initializes a new instance of the ListResponse class.
        /// </summary>
        public ListResponse()
            : base()
        {
            this.Value = new List<T>();
        }

        public ListResponse(BaseResponse repsonse)
            : base()
        {
            base.ErrorCode = repsonse.ErrorCode;
            base.Message = repsonse.Message;
            this.Value = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the success ListResponse class with initialized value.
        /// </summary>
        public ListResponse(List<T> value)
            : base()
        {
            base.ErrorCode = 0;
            base.Message = "Success";
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the ListResponse class with initialized values.
        /// </summary>
        public ListResponse(int errorCode, string message, List<T> value)
            : base()
        {
            base.ErrorCode = errorCode;
            base.Message = message;
            this.Value = value;
        }

    }

    public class BaseResponse
    {
        public int ErrorCode = 0;
        public string Message = null;

        public BaseResponse() { }

        public BaseResponse(int errorCode, string message)
        {
            this.ErrorCode = errorCode;
            this.Message = message;
        }
    }
}
