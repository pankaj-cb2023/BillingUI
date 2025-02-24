//using BillingUI.Settings.Globals;
//using BillingUI.Settings.Models;
//using Microsoft.Extensions.Caching.Memory;
//using System.Collections.Specialized;
//using System.Configuration;
//using System.Data;
//using System.Text.Json;
//using Constants = BillingUI.Settings.Globals.Constants;

//namespace BillingUI.Settings
//{
//    public class AppConfig
//    {
//        public delegate IList<Setting> OnGetSettingsByGroupEvent(string groupCode);
//        public OnGetSettingsByGroupEvent OnGetSettingsByGroup = null;
//        public static readonly string ARES_COMMON_SERRVICE_URL_CODE = "http://ld-billing01:8099/AppConfigPortal/Setting/SelectSettingGroupCd";

//        private const string INTERNAL_GROUP_CODE_LIST = "_X_INTERNAL_GROUP_CODE_LIST";
//        private static string ClassCode = "HelperLibrary.Config.AppConfig";

//        private AppConfigSourceTypeEnum AppConfigSourceType = AppConfigSourceTypeEnum.AresCommonService;
//        private string SubGroupCode = null;
//        private string ServiceUrl = null;
//        private readonly IMemoryCache _cache;

//        public AppConfig(AppConfigSourceTypeEnum appConfigSourceType)
//        {
//            this.AppConfigSourceType = appConfigSourceType;

//        }

//        public AppConfig(AppConfigSourceTypeEnum appConfigSourceType, string subGroupCode)
//            : this(appConfigSourceType)
//        {
//            this.SubGroupCode = subGroupCode;
//        }

//        public AppConfig(AppConfigSourceTypeEnum appConfigSourceType, string subGroupCode, string serviceUrl)
//            : this(appConfigSourceType, subGroupCode)
//        {
//            this.ServiceUrl = serviceUrl;
//        }

//        /// <summary>
//        /// </summary>
//        ~AppConfig()
//        {

//        }

//        public async Task<Setting> GetObject(string groupCode, string settingCode, bool loadFromCache = true)
//        {
//            IDictionary<string, Setting> settingList = await GetSettingsList(groupCode, loadFromCache);

//            if (settingList != null && settingList.ContainsKey(settingCode))
//                return settingList[settingCode];

//            return null;
//        }

//        public async Task<IDictionary<string, Setting>> GetObjects(string groupCode, bool loadFromCache = true)
//        {
//            return await GetSettingsList(groupCode, loadFromCache);
//        }

//        private async Task<IDictionary<string, Setting>> GetSettingsList(string groupCode, bool loadFromCache = true)
//        {
//            IDictionary<string, Setting>? settingList = loadFromCache ? _cache.Get<IDictionary<string, Setting>>(groupCode) : null;

//            if (settingList == null || settingList.Count == 0)
//            {
//                if (this.AppConfigSourceType == AppConfigSourceTypeEnum.AresCommonService)
//                {
//                    string? url = string.IsNullOrEmpty(this.ServiceUrl) ? ConfigurationManager.AppSettings[ARES_COMMON_SERRVICE_URL_CODE] : this.ServiceUrl;

//                    var headers = new Dictionary<string, string>
//                    {
//                        { Constants.ARES_X_HEADER_SOURCE_APP, ClassCode },
//                        { Constants.ARES_X_HEADER_OPTIONS, Constants.ARES_NO_HTTP_STATUS_CODE_INJECTION }
//                     };

//                    string queryString = $"groupCode={Uri.EscapeDataString(groupCode + (string.IsNullOrWhiteSpace(this.SubGroupCode) ? string.Empty : "/" + this.SubGroupCode))}";

//                    using var httpClient = new HttpClient { BaseAddress = new Uri(url) };
//                    foreach (var header in headers)
//                    {
//                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
//                    }

//                    HttpResponseMessage response = await httpClient.GetAsync($"/appconfigs/getSettingsByGroup?{queryString}");

//                    if (response.IsSuccessStatusCode)
//                    {
//                        string responseBody = await response.Content.ReadAsStringAsync();
//                        var settingsResponse = JsonSerializer.Deserialize<List<Setting>>(responseBody);

//                        if (settingsResponse?.Count > 0)
//                        {
//                            settingList = new Dictionary<string, Setting>();
//                            foreach (var setting in settingsResponse.ToList())
//                            {
//                                settingList[setting.Value] = setting;
//                            }

//                            // Save to cache
//                            _cache.Set(groupCode, settingList);
//                            settingList = PopulateSettingList(settingsResponse);
//                        }
//                    }
//                    else
//                    {
//                        throw new Exception($"Error fetching settings: {response.StatusCode} - {response.ReasonPhrase}");
//                    }
//                }
//                else //AppConfigSourceTypeEnum.Custom
//                {

//                    if (this.OnGetSettingsByGroup != null)
//                    {
//                        IList<Setting> list = this.OnGetSettingsByGroup(groupCode);

//                        if (list != null && list.Count > 0)
//                        {
//                            settingList = PopulateSettingList(list);
//                        }
//                    }
//                    else
//                        throw new Exception("OnGetSettingsByGroup event is not set.");
//                }
//            }
//            return settingList;
//        }

//        /// <summary>
//        /// Gets settings from the DB.
//        /// This method does not cache the result.
//        /// </summary>
//        /// <param name="groupCode">Group Code of the settings to retrieve</param>
//        /// <returns></returns>
//        //public static IList<Setting> GetSettingsByGroup(string groupCode, string connectionString = "")
//        //{
//        //    IList<Setting> list = new List<Setting>();

//        //    using (SqlCommand cmd = new SqlCommand())
//        //    using (SqlConnection cn = new SqlConnection(string.IsNullOrEmpty(connectionString) ?
//        //        ConfigurationManager.AppSettings[ClassCode + ".AppConfigConnection"] : connectionString))
//        //    {
//        //        cmd.Connection = cn;
//        //        cmd.CommandType = CommandType.StoredProcedure;
//        //        cmd.CommandText = "usp_config_GetSettingsByGroupCd";
//        //        cmd.CommandTimeout = 120;
//        //        cmd.Parameters.AddWithValue("@GroupCd", groupCode);

//        //        cn.Open();

//        //        DataTable resultTable = new DataTable("Settings");

//        //        using (SqlDataAdapter _adapter = new SqlDataAdapter(cmd))
//        //        {
//        //            _adapter.Fill(resultTable);
//        //        }

//        //        cn.Close();

//        //        if (resultTable != null && resultTable.Rows.Count > 0)
//        //        {
//        //            foreach (DataRow row in resultTable.Rows)
//        //            {
//        //                Setting setting = new Setting();
//        //                setting.Id = Convert.ToInt32(row["SettingId"].ToString());
//        //                setting.GroupId = Convert.ToInt32(row["GroupId"].ToString());
//        //                setting.TypeCode = row["SettingTypeCd"].ToString();
//        //                setting.Code = row["SettingCd"].ToString();
//        //                setting.Value = row["SettingValue"].ToString();
//        //                setting.Description = row["SettingDs"].ToString();

//        //                list.Add(setting);
//        //            }
//        //        }
//        //    }

//        //    return list;
//        //}

//        public async Task<string> GetValue(string groupCode, string settingCode, bool allowLocalOverride = true, string localOverrideGroupCode = null)
//        {
//            string value = null;

//            if (allowLocalOverride)
//            {
//                string overrideValue = null;

//                if (string.IsNullOrEmpty(localOverrideGroupCode))
//                {
//                    overrideValue = ConfigurationManager.AppSettings[settingCode];
//                }
//                else
//                {
//                    NameValueCollection section = ConfigurationManager.GetSection("appSettings" + localOverrideGroupCode) as NameValueCollection;

//                    if (section != null)
//                        overrideValue = section[settingCode];
//                }

//                if (overrideValue != null)
//                {
//                    value = overrideValue;
//                }
//            }

//            if (value == null)
//            {
//                Setting setting = await GetObject(groupCode, settingCode);
//                value = (setting == null ? null : setting.Value);
//            }

//            return value;
//        }

//        public async Task<string> GetValue(string groupCode, string settingCode, string defaultValue)
//        {
//            string value = await GetValue(groupCode, settingCode);

//            if (value != null)
//                return value;

//            return defaultValue;
//        }

//        public async Task<long> GetValue(string groupCode, string settingCode, long defaultValue)
//        {
//            string value = await GetValue(groupCode, settingCode);
//            long returnValue = 0;

//            if (value != null && Int64.TryParse(value, out returnValue))
//                return returnValue;

//            return defaultValue;
//        }

//        public async Task<int> GetValue(string groupCode, string settingCode, int defaultValue)
//        {
//            string value = await GetValue(groupCode, settingCode);
//            int returnValue = 0;

//            if (value != null && Int32.TryParse(value, out returnValue))
//                return returnValue;

//            return defaultValue;
//        }

//        public async Task<short> GetValue(string groupCode, string settingCode, short defaultValue)
//        {
//            string value = await GetValue(groupCode, settingCode);
//            short returnValue = 0;

//            if (value != null && Int16.TryParse(value, out returnValue))
//                return returnValue;

//            return defaultValue;
//        }

//        public async Task<double> GetValue(string groupCode, string settingCode, double defaultValue)
//        {
//            string value = await GetValue(groupCode, settingCode);
//            double returnValue = 0;

//            if (value != null && double.TryParse(value, out returnValue))
//                return returnValue;

//            return defaultValue;
//        }

//        public async Task<decimal> GetValue(string groupCode, string settingCode, decimal defaultValue)
//        {
//            string value = await GetValue(groupCode, settingCode);
//            decimal returnValue = 0;

//            if (value != null && decimal.TryParse(value, out returnValue))
//                return returnValue;

//            return defaultValue;
//        }

//        public async Task<bool> GetValue(string groupCode, string settingCode, bool defaultValue)
//        {
//            string value = await GetValue(groupCode, settingCode);
//            bool returnValue = false;

//            if (value != null && bool.TryParse(value, out returnValue))
//                return returnValue;

//            return defaultValue;
//        }

//        public async Task<DateTime> GetValue(string groupCode, string settingCode, DateTime defaultValue)
//        {
//            string value = await GetValue(groupCode, settingCode);
//            DateTime returnValue = DateTime.MinValue;

//            if (value != null && DateTime.TryParse(value, out returnValue))
//                return returnValue;

//            return defaultValue;
//        }

//        //public void InsertObject(Setting setting, string connectionString = "")
//        //{
//        //    using (var cmd = new SqlCommand())
//        //    using (var cn = new SqlConnection(string.IsNullOrEmpty(connectionString) ?
//        //        ConfigurationManager.AppSettings[ClassCode + ".AppConfigConnection"] : connectionString))
//        //    {
//        //        cmd.Connection = cn;
//        //        cmd.CommandType = CommandType.StoredProcedure;
//        //        cmd.CommandText = "usp_config_InsertSetting";
//        //        cmd.CommandTimeout = 120;

//        //        cmd.Parameters.AddWithValue("@GroupId", (short)setting.GroupId);
//        //        cmd.Parameters.AddWithValue("@SettingTypeCd", setting.TypeCode);
//        //        cmd.Parameters.AddWithValue("@SettingCd", setting.Code);
//        //        cmd.Parameters.AddWithValue("@SettingValue", setting.Value);
//        //        cmd.Parameters.AddWithValue("@SettingDs", setting.Description);
//        //        cmd.Parameters.AddWithValue("@CreatedBy", setting.CreatedBy);

//        //        cn.Open();
//        //        cmd.ExecuteNonQuery();
//        //        cn.Close();
//        //    }
//        //}

//        //public void UpdateObject(Setting setting, string connectionString = "")
//        //{
//        //    using (var cmd = new SqlCommand())
//        //    using (var cn = new SqlConnection(string.IsNullOrEmpty(connectionString) ?
//        //        ConfigurationManager.AppSettings[ClassCode + ".AppConfigConnection"] : connectionString))
//        //    {
//        //        cmd.Connection = cn;
//        //        cmd.CommandType = CommandType.StoredProcedure;
//        //        cmd.CommandText = "usp_config_UpdateSetting";
//        //        cmd.CommandTimeout = 120;

//        //        cmd.Parameters.AddWithValue("@SettingId", setting.Id);
//        //        cmd.Parameters.AddWithValue("@GroupId", (short)setting.GroupId);
//        //        cmd.Parameters.AddWithValue("@SettingTypeCd", setting.TypeCode);
//        //        cmd.Parameters.AddWithValue("@SettingCd", setting.Code);
//        //        cmd.Parameters.AddWithValue("@SettingValue", setting.Value);
//        //        cmd.Parameters.AddWithValue("@SettingDs", setting.Description);
//        //        cmd.Parameters.AddWithValue("@ModifiedBy", setting.ModifiedBy);

//        //        cn.Open();
//        //        cmd.ExecuteNonQuery();
//        //        cn.Close();
//        //    }
//        //}

//        public async Task ClearSettingsAsync(string groupCode)
//        {
//            if (groupCode == "*") // clear all
//            {
//                string groupCodeListSync = string.Empty;
//                var groupCodeList = _cache.Get<IList<string>>(INTERNAL_GROUP_CODE_LIST);

//                if (groupCodeList != null && groupCodeList.Count > 0)
//                {
//                    foreach (string gc in groupCodeList)
//                    {
//                        groupCodeListSync += gc + "~";
//                        _cache.Remove(gc);
//                    }

//                    _cache.Remove(INTERNAL_GROUP_CODE_LIST);
//                }
//            }
//            else if (!string.IsNullOrEmpty(groupCode)) // clear specific groupCode
//            {
//                _cache.Remove(groupCode);
//            }
//        }


//        private IDictionary<string, Setting> PopulateSettingList(IList<Setting> sourceList)
//        {
//            IDictionary<string, Setting> settingList = new Dictionary<string, Setting>();

//            foreach (Setting setting in sourceList)
//            {
//                settingList.Add(setting.Code, setting);
//            }

//            return settingList;
//        }

//        //public bool IsSettingExist(string GroupCode, string SettingCode)
//        //{
//        //    bool check = false;
//        //    Setting obj = GetObject(GroupCode, SettingCode);

//        //    if (obj != null)
//        //        check = true;

//        //    return check;
//        //}




//    }
//}
