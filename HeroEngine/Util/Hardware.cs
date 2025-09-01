using System.Management;

namespace HeroEngine.Util
{
    public class Hardware
    {
        public static new string ToString()
        {
            return $"{GetProcessor()},{GetDrive()},{GetNicHwid()}";
        }

        private static string GetProcessor()
        {
            var query = "SELECT ProcessorId FROM Win32_Processor";
            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (var obj in searcher.Get())
                {
                    return obj["ProcessorId"]?.ToString();
                }
            }
            return "unknown";
        }

        private static string GetDrive()
        {
            var query = "SELECT SerialNumber FROM Win32_DiskDrive";
            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (var obj in searcher.Get())
                {
                    return obj["SerialNumber"]?.ToString();
                }
            }
            return "unknown";
        }

        private static string GetNicHwid()
        {
            var query = "SELECT MACAddress, AdapterType, NetConnectionID FROM Win32_NetworkAdapter WHERE MACAddress IS NOT NULL AND AdapterType != 'Microsoft Virtual WiFi Miniport Adapter'";
            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (var obj in searcher.Get())
                {
                    string macAddress = obj["MACAddress"]?.ToString();
                    if (!string.IsNullOrEmpty(macAddress))
                    {
                        return macAddress;
                    }
                }
            }
            return "unknown";
        }
    }
}
