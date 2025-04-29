using System.Diagnostics;
using System.Net;
using System.Web;

namespace ChromeRedirect;

class ChromeRedirect
{
    static void Main(string[] args)
    {
        if (args[0] != "--single-argument")
        {
            Console.WriteLine(args[0]);
            Console.WriteLine("No arguments provided.");
            Console.ReadLine();
            return;
        }
        string arguments = args[1];
        string finalUrl = DecodeUrl(arguments);
        
        if (finalUrl == "")
        {
            Console.WriteLine("Invalid URL.");
            Environment.Exit(1);
        }
        
        RunChrome(finalUrl);
    }

    private static string DecodeUrl(string arguments)
    {
        Uri uri = new Uri(arguments.Replace("microsoft-edge:///?", "http://dummy/?"));
        var queryParams = HttpUtility.ParseQueryString(uri.Query);
        
        string encodedUrl = queryParams["url"];

        if (encodedUrl == null)
        {
            Console.WriteLine("Invalid URL.");
            return "";
        }

        string firstDecode = WebUtility.UrlDecode(encodedUrl);
        string finalUrl = WebUtility.UrlDecode(firstDecode);
        
        if (!Uri.IsWellFormedUriString(finalUrl, UriKind.Absolute) || !(finalUrl.StartsWith("https://") || finalUrl.StartsWith("http://")))
        {
            Console.WriteLine("Invalid or unsafe URL.");
            return "";
        }
        
        return finalUrl;
    }

    private static void RunChrome(string url)
    {
        var psi = new ProcessStartInfo
        {
            Arguments = url,
            FileName = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        Process.Start(psi);
    }
}