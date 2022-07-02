using GmadDownloadAPINamespace;

namespace gmadget2
{
    public partial class gmadui : Form
    {
        public gmadui()
        {
            InitializeComponent();
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
        }
    }
}