using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonService
{
    public interface IConfigurationService
    {
        Dictionary<string, string> AppSettings();
        Dictionary<string, string> ConnectionStrings();
        string getAppSetting(string key);
        string getConnectionString(string name);
    }
}
