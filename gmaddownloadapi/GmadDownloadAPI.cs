using Indieteur.SAMAPI;
using Newtonsoft.Json;
using Steam.Models;
using SteamCMD.ConPTY;
using SteamCMD.ConPTY.Interop.Definitions;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using SevenZip;

// TODO: Fault handling for if gmad isn't located



namespace GmadDownloadAPINamespace
{
    public class GmadDownloadAPI
    {
        class GetCollectionDetailsResponse
        {
            public class Child
            {
                public string publishedfileid { get; set; }
                public int sortorder { get; set; }
                public int filetype { get; set; }
            }
            public class CollectionDetails
            {
                public string publishedfileid { get; set; }
                public int result { get; set; }

                public List<Child> children { get; set; }
            }
            public class InnerResponse
            {
                public int result { get; set; }
                public int resultcount { get; set; }
                public List<CollectionDetails> collectiondetails { get; set; }
            }
            public InnerResponse response { get; set; }
        }

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


        public static async Task CreateAndRunScript(string path)
        {
            LocateGMADexe();
            if(gmadlocation == null || gmadlocation == "")
            {
                Log("Could not find gmad.exe! Garrys mod needs to be downloaded through steam");
                return;
            } else
            {
                Log("Found gmad.exe at " + gmadlocation + "\r\n");
            }

            List<PublishedFileDetailsModel> mod_list = new List<PublishedFileDetailsModel>(); // Here is where we are going to store a list of workshop id's to download..

            Log("Grabbing information about the link...\r\n");

            // Grab the ID from the link..
            Regex regex = new Regex(@".*id=(?<id>\d*)$");
            Match match = regex.Match(path);
            string id = match.Groups["id"].Value;

            // Next, it's time to grab information about the mod..
            var webInterfaceFactory = new SteamWebInterfaceFactory("ABC123");
            var steaminterface = webInterfaceFactory.CreateSteamWebInterface<SteamRemoteStorage>(new HttpClient());
            Log("Downloading information about the mod " + id + "\r\n");
            var filedetails_complete = await steaminterface.GetPublishedFileDetailsAsync(new List<ulong> { ulong.Parse(id) });
            var filedetails = filedetails_complete.Data.ElementAt(0);

            // At this point, it's time to figure out if this is a workshop collection, or if its a normal mod.
            if (filedetails.CreatorAppId == 766) // This is our best guess at the difference. 766 if it's a workshop addon, or 4000 if its a garrys mod addon.
            {
                Log("Turns out, it's not a mod, but a collection. Downloading information about the collection..\r\n");
                HttpClient hc = new HttpClient();
                var values = new Dictionary<string, string>
                  {
                      { "collectioncount", "1" },
                      { "publishedfileids[0]", id }
                  };


                var response = await hc.PostAsync("https://api.steampowered.com/ISteamRemoteStorage/GetCollectionDetails/v0001/", new FormUrlEncodedContent(values));
                string responsestring = await response.Content.ReadAsStringAsync();
                GetCollectionDetailsResponse gcdr = JsonConvert.DeserializeObject<GetCollectionDetailsResponse>(responsestring);

                // After downloading the list of mods in the collection, it's time to download information *about* the mods.
                // We first generate the list of mod id's to download... (If someone could teach me the neat lamdba thing to replace this, this would be nice)
                List<ulong> modinfo_to_get = new List<ulong>();

                foreach (var child in gcdr.response.collectiondetails[0].children)
                {
                    modinfo_to_get.Add(ulong.Parse(child.publishedfileid));    
                }

                // We then grab all of the information about the mods.
                var modinfo_list = await steaminterface.GetPublishedFileDetailsAsync(modinfo_to_get);

                foreach(var modinfo in modinfo_list.Data)
                {
                    mod_list.Add(modinfo);
                }
            }
            else if (filedetails.CreatorAppId != 4000) // If it's not from garrys mod, straight up just give up. Patches are welcome.
            {
                Log("This is not a garrys mod addon!\r\n");
                return;
            } else
            {
                mod_list.Add(filedetails); // Normal mod, woo.
            }

            // Time to actually generate the file, and download any old-style mods.
            string scriptcontent = "";
            scriptcontent += "login anonymous\n";

            foreach(var mod in mod_list) {
                if(mod.FileUrl != null)
                {
                    // It's an old style mod! Download that sucker!
                    Log(mod.Title + "(" + mod.PublishedFileId.ToString() +  ") is a Old Style mod. Downloading directly without steamcmd.\r\n");

                    if (mod.CreatorAppId == 766)
                    {
                        Log("UH OH! Turns out, this 'mod' is actually a linked workshop collection. You are going to have to download that collection via its own link.\r\n");
                        continue;
                    }

                    Directory.CreateDirectory("temp");
                    string tempname = "temp/" + mod.PublishedFileId + Path.GetExtension(mod.FileName);

                    if (!File.Exists(tempname)) { 
                        var outstream = File.Create(tempname);
                        HttpClient hc = new HttpClient();
                        var instream = await hc.GetStreamAsync(mod.FileUrl);
                        await instream.CopyToAsync(outstream);
                        instream.Close();
                        outstream.Close();
                    }

                    Log("Downloaded the file. Extracting.\r\n");
                    // ^--- OK, thats the heck of a blob just to download one file. RIP WebClient.
                    // Next, extract the gmad! Woo!
                    string outname = "output/" + string.Join("_", mod.Title.Split(Path.GetInvalidFileNameChars()));

                    try {
                        FileStream fi = File.Open(tempname, FileMode.Open);
                        FileStream fo = File.Open(tempname + ".out", FileMode.Create);

                        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();

                        // Read the decoder properties
                        byte[] properties = new byte[5];
                        fi.Read(properties, 0, 5);

                        // Read in the decompress file size.
                        byte[] fileLengthBytes = new byte[8];
                        fi.Read(fileLengthBytes, 0, 8);
                        long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                        coder.SetDecoderProperties(properties);
                        coder.Code(fi, fo, fi.Length, fileLength, null);
                        fo.Flush();
                        fo.Close();
                        fi.Close();
                    } catch (Exception e)
                    {
                        Log("Could not extract file, error given: " + e.ToString());
                        return;
                    }

                    Log("LZMA extraction done! Now for gmad...\r\n");

                    Directory.CreateDirectory(outname);
                    await RunGMAD(tempname + ".out", outname);
                } else { 
                    scriptcontent += "workshop_download_item 4000 " + mod.PublishedFileId.ToString() + "\n";
                }
            }

            scriptcontent += "quit";
            File.WriteAllText("steamscript.tmp", scriptcontent);

            var steamCMDConPTY = new SteamCMDConPTY
            {
                Arguments = "+runscript steamscript.tmp",
                FilterControlSequences = true,
            };

            steamCMDConPTY.TitleReceived += (sender, data) => {
                TitleReceived?.Invoke(sender, data);
            };

            steamCMDConPTY.OutputDataReceived += async (sender, data) => {
                Log(data);
                Regex regex = new Regex(Regex.Escape("Success. Downloaded item ") + @"(?<modid>\d*) to ""(?<newpath>[^""]*)""");
                Match match = regex.Match(data);

                if (match.Success)
                {
                    var currentmod = mod_list.Find(x => x.PublishedFileId == ulong.Parse(match.Groups["modid"].ToString()));
                    Log("Downloading " + currentmod.Title + "\r\n");

                    string foldername = "output/" + string.Join("_", currentmod.Title.Split(Path.GetInvalidFileNameChars()));

                    DirectoryInfo di = Directory.CreateDirectory(foldername);

                    string newpath = match.Groups["newpath"].ToString().Replace("\n", "").Replace("\r", "");
                    Log("Found path!" + newpath + "\r\n");
                    var files = Directory.EnumerateFiles(newpath, "*.gma");
                    Log("Found gma! " + files.ElementAt(0) + "\r\n");

                    await RunGMAD(files.ElementAt(0), foldername);
                }
            };

            steamCMDConPTY.Exited += (sender, exitCode) => {
                Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = Path.GetFullPath("output/"),
                    UseShellExecute = true,
                    Verb = "open"
                });
            };

            ProcessInfo processInfo = steamCMDConPTY.Start();
            Process.GetProcessById(processInfo.dwProcessId).WaitForExit();
            Log("Done!\r\n");
        }

        public static async Task RunGMAD(string inpath, string outpath)
        {
            try { 
                Process p = new Process();
                p.OutputDataReceived += GmadOutputReceived;
                p.ErrorDataReceived += GmadOutputReceived;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.EnableRaisingEvents = true;
                p.StartInfo.FileName = gmadlocation;
                p.StartInfo.ArgumentList.Add("extract");
                p.StartInfo.ArgumentList.Add("-file");
                p.StartInfo.ArgumentList.Add(inpath);
                p.StartInfo.ArgumentList.Add("-out");
                p.StartInfo.ArgumentList.Add(outpath);
                p.Start();
                p.BeginOutputReadLine();
                await p.WaitForExitAsync();

                Log("Done extracting!\r\n");
            } catch(Exception e)
            {
                Log("Exception running gmad: " + e.Message);
            }
        }

        private static void GmadOutputReceived(object sender, DataReceivedEventArgs e)
        {
            if(e.Data != null)
                Log(e.Data.ToString() + "\r\n");
        }
    }
}