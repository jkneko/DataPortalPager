using System;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace DataPortalPager
{
    public partial class MainForm : Form
    {
        private CefSharp.WinForms.ChromiumWebBrowser webContent;
        private int pageCount = 32;
        private int currentPage = 0;

        public MainForm()
        {
            InitializeComponent();

            Cursor.Hide();

            // 初回のみ30秒とする
            timer1.Interval = 30000;

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(appPath);
            CefSettings settings = new CefSettings();
            settings.UserDataPath = appPath;
            settings.CachePath = appPath + @"\caches";
            settings.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66";
            Cef.EnableHighDPISupport();
            Cef.Initialize(settings);

            webContent = new ChromiumWebBrowser("https://datastudio.google.com/embed/reporting/1h_UHdcPryyBwV2hLrW8G_eOhAsDA6ZB1/page/KG79");
            this.Controls.Add(webContent);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 10000;
            if (Form.ActiveForm != this)
                return;

            currentPage++;
            if(currentPage >= pageCount)
            {
                SendKeys.Send("{HOME}");
                currentPage = 0;
            }
            else
            {
                SendKeys.Send("{RIGHT}");
            }
        }
    }
}
