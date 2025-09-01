namespace HeroEngine.Util
{
    public class Internet
    {
        public static bool HasInternetConnection(out string address)
        {
            address = "";
            using (var client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = client.GetAsync("https://api.ipify.org").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        address = response.Content.ReadAsStringAsync().Result;
                        return !string.IsNullOrEmpty(address);
                    }
                } catch
                {

                }

                try
                {
                    response = client.GetAsync("https://ipv4.icanhazip.com/").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        address = response.Content.ReadAsStringAsync().Result;
                        return !string.IsNullOrEmpty(address);
                    }
                } catch
                {

                }
            }

            return false;
        }
    }
}
