using MaxMind.Db;
using MaxMind.GeoIP2;
using Sitecore.Analytics.Lookups;
using Sitecore.Analytics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace SF.Feature.GeoIP
{
    public class MaxMindDBLookupProvider : LookupProviderBase, IDisposable
    {
        private static DatabaseReader Reader { get; set; }
        
        static MaxMindDBLookupProvider()
        {
            var path = GetDatabasePath();
            Sitecore.Diagnostics.Log.Info("Loading Maxmind DB at: " + path, "MaxMindDBLookupProvider");
            Reader = new DatabaseReader(path);
        }

        public override WhoIsInformation GetInformationByIp(string ip)
        {
            
            var whois = new WhoIsInformation();
            try
            {
                var city = Reader.City(ip);

                if (city != null)
                {

                    whois.Country = city.Country.IsoCode;
                    whois.City = city.City.Name;
                    whois.PostalCode = city.Postal.Code;
                    whois.Region = city.MostSpecificSubdivision.IsoCode;
                    whois.Latitude = city.Location.Latitude;
                    whois.Longitude = city.Location.Longitude;

                }
                else
                {
                    Sitecore.Diagnostics.Log.Warn("Ip Address Not Found", this);
                }
            }
            catch(Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("GeoIP Lookup Failed", ex, this);
            }
            return whois;
        }

        public void Dispose()
        {
            if (Reader != null)
            {
                Reader.Dispose();
            }
        }

        private static string GetDatabasePath()
        {
            var setting = Sitecore.Configuration.Settings.GetSetting("SF.MaxMindCityDBPath");
            var path = HostingEnvironment.MapPath(setting);
            return path;
        }
    }
}
