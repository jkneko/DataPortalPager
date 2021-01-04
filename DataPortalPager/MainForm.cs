using System;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace DataPortalPager
{
    public partial class MainForm : Form
    {
        private CefSharp.WinForms.ChromiumWebBrowser webContent;
        private int pageCount = 0;
        private int currentPage = 0;
        private String settingsFileName = "settings.json";
        private Settings settings;

        public MainForm()
        {
            InitializeComponent();
            init();
        }

        private async void init()
        {
            // Read options
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var stream = new FileStream(settingsFileName, FileMode.Open, FileAccess.Read);
            settings = await JsonSerializer.DeserializeAsync<Settings>(stream, options);
            timer1.Interval = settings.initialWait * 1000;
            pageCount = settings.pageCount;

            // Initialize chromium webview
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(appPath);
            CefSettings cefSettings = new CefSettings();
            cefSettings.UserDataPath = appPath;
            cefSettings.CachePath = appPath + @"\caches";
            cefSettings.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66";
            Cef.EnableHighDPISupport();
            Cef.Initialize(cefSettings);

            webContent = new ChromiumWebBrowser(settings.url);
            this.Controls.Add(webContent);

            // Hide cursor
            Cursor.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = settings.interval * 1000;
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

        private void fullscreenTimer_Tick(object sender, EventArgs e)
        {
            fullscreenTimer.Enabled = false;

            // Set window fullscreen
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }
    }

    public class Settings
    {
        public String url { get; set; }
        public int pageCount { get; set; }
        public int initialWait { get; set; }
        public int interval { get; set; }
    }
}
