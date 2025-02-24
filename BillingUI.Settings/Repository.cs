using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingUI.Settings
{
    public interface IRepository
    {
        BaseImplementation Implementation { get; set; }
        IConfigRepository Config { get; }
    }

    public class Repository : IRepository
    {
        public BaseImplementation? Implementation { get; set; }

        public IConfigRepository? _config;
        public IConfigRepository Config
        {
            get { if (_config == null) _config = new ConfigRepository(); return _config; }
        }
    }
}
