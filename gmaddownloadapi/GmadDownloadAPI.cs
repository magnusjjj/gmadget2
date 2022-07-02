using SteamCMD.ConPTY;
using SteamCMD.ConPTY.Interop.Definitions;
using System.Text.RegularExpressions;

namespace GmadDownloadAPINamespace
{
    public class GmadDownloadAPI
    {
        public static event EventHandler<string> TitleReceived;
        public static event EventHandler<string> OutputDataReceived;

        public static void DownloadMod(string path)
        {
            Regex regex = new Regex(@".*id=(?<id>\d*)$");
            Match match = regex.Match(path);

            string id = match.Groups["id"].Value;


            var steamCMDConPTY = new SteamCMDConPTY
            {
                Arguments = "+login anonymous +workshop_download_item 4000 "+id+" +quit",
            };

            steamCMDConPTY.TitleReceived += (sender, data) => {
                TitleReceived?.Invoke(sender, data);
            };

            steamCMDConPTY.OutputDataReceived += (sender, data) => { 
                OutputDataReceived?.Invoke(sender, data);
            };

            steamCMDConPTY.Exited += (sender, exitCode) => { 
            
            };

            ProcessInfo processInfo = steamCMDConPTY.Start();
        }
    }
}