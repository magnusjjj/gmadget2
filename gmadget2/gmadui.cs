using GmadDownloadAPINamespace;
using System.Text.RegularExpressions;

namespace gmadget2
{
    public partial class gmadui : Form
    {
        public gmadui()
        {
            InitializeComponent();
            Log(GmadDownloadAPI.LocateGMADexe());
            /*            try
                        {
                            Log(GmadDownloadAPI.LocateGMADexe());
                        }
                        catch (Exception e)
                        {*/
            //Log("Could not find garrys mod!");
            //}
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            GmadDownloadAPI.TitleReceived += GmadDownloadAPI_TitleReceived;
            GmadDownloadAPI.OutputDataReceived += GmadDownloadAPI_OutputDataReceived;
            GmadDownloadAPI.DownloadMod(txtURL.Text);
        }

        private void Log(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(Log), new object[] { text });
                return;
            }
            txtLog.Text += text;
        }

        private void GmadDownloadAPI_TitleReceived(object? sender, string e)
        {
            return;
//            throw new NotImplementedException();
        }

        private void GmadDownloadAPI_OutputDataReceived(object? sender, string e)
        {
            Log(e);


            //Success.Downloaded item 2811906809 to "C:\Users\Tuxie\source\repos\gmadget2\gmadget2\bin\Debug\net6.0-windows\steamapps\workshop\content\4000\2811906809"
        }
    }
}