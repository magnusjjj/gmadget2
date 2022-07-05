using Indieteur.SAMAPI;
using SteamCMD.ConPTY;
using SteamCMD.ConPTY.Interop.Definitions;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace GmadDownloadAPINamespace
{
    public class GmadDownloadAPI
    {
        public static event EventHandler<string> TitleReceived;
        public static event EventHandler<string> OutputDataReceived;

        public static string gmadlocation = "";

        public static string LocateGMADexe()
        {
            SteamAppsManager sam = new SteamAppsManager();
            gmadlocation = sam.SteamApps.FindAppByID(4000).InstallDir + "\\bin\\gmad.exe";
            return gmadlocation;
        }

        static void Log(string s)
        {
            OutputDataReceived?.Invoke(null, s);
        }


        public static async void DownloadMod(string path)
        {
            Regex regex = new Regex(@".*id=(?<id>\d*)$");
            Match match = regex.Match(path);

            string id = match.Groups["id"].Value;

            var webInterfaceFactory = new SteamWebInterfaceFactory("ABC123");
            var steaminterface = webInterfaceFactory.CreateSteamWebInterface<SteamRemoteStorage>(new HttpClient());
            var filedetails = await steaminterface.GetPublishedFileDetailsAsync(new List<ulong> { ulong.Parse(id) });

            string foldername = "output/" + string.Join("_", filedetails.Data.ElementAt(0).Title.Split(Path.GetInvalidFileNameChars()));

            DirectoryInfo di = Directory.CreateDirectory(foldername);

            var steamCMDConPTY = new SteamCMDConPTY
            {
                Arguments = "+login anonymous +workshop_download_item 4000 "+id+" +quit",
            };

            steamCMDConPTY.TitleReceived += (sender, data) => {
                TitleReceived?.Invoke(sender, data);
            };

            steamCMDConPTY.OutputDataReceived += (sender, data) => { 
                Log(data);
                Regex regex = new Regex(Regex.Escape("Success. Downloaded item ") + @"\d* to ""(?<newpath>[^""]*)""");
                Match match = regex.Match(data);

                if (match.Success)
                {
                    string newpath = match.Groups["newpath"].ToString().Replace("\n", "").Replace("\r", "");
                    Log("Found path!" + newpath);
                    var files = Directory.EnumerateFiles(newpath, "*.gma");
                    Log("Found gma! " + files.ElementAt(0));
                    Process.Start(gmadlocation, new string[] { "extract", "-file", files.ElementAt(0), "-out", foldername});

                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = di.FullName,
                        UseShellExecute = true,
                        Verb = "open"
                    });

                }
            };

            steamCMDConPTY.Exited += (sender, exitCode) => { 
            
            };

            ProcessInfo processInfo = steamCMDConPTY.Start();
        }
    }
}