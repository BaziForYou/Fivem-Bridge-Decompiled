using ePOSOne.btnProduct;
using FiveM_Bridge.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace WindowsFormsApplication1
{
  public class Form1 : Form
  {
    private static readonly HttpClient client = new HttpClient();
    private IContainer components;
    private Panel panel1;
    private HelpProvider helpProvider1;
    private Panel panel2;
    private Button_WOC button_WOC2;
    private Label label1;
    private Label label2;

    public Form1() => this.InitializeComponent();

    private void button1_Click(object sender, EventArgs e)
    {
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void panel1_Paint(object sender, PaintEventArgs e)
    {
    }

    private async void button_WOC1_Click(object sender, EventArgs e)
    {
      try
      {
        string[] strArray1 = Form1.RemoveHtmlTags(await Form1.RequestData("https://api.fivembridge.ir/requestAppData")).Split(new string[1]
        {
          "---"
        }, StringSplitOptions.None);
        string str1 = Form1.RemoveSpaces(strArray1[2].Trim());
        string str2 = Form1.RemoveSpaces(strArray1[4].Trim());
        string str3 = Form1.RemoveSpaces(strArray1[10].Trim());
        string[] strArray2 = str2.Split('\n');
        string dns_1 = strArray2[0];
        string dns_2 = strArray2[1];
        if ("1.0" != str3)
        {
          int num = (int) MessageBox.Show("ورژن برنامه قدیمی است! لطفا برنامه را از کنترل پنل حذف کنید و ورژن جدید را دانلود و نصب کنید");
          Process.Start(Form1.RemoveSpaces("https://www.fivembridge.ir"));
          Environment.Exit(0);
        }
        else
        {
          try
          {
            X509Certificate2 certificate = new X509Certificate2(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources//server.crt"));
            X509Store x509Store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            x509Store.Open(OpenFlags.ReadWrite);
            if (!x509Store.Certificates.Contains(certificate))
              x509Store.Add(certificate);
            x509Store.Close();
          }
          catch (Exception ex)
          {
          }
          Process process = new Process();
          ProcessStartInfo processStartInfo = new ProcessStartInfo()
          {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "cmd.exe",
            Verb = "runas",
            CreateNoWindow = true
          };
          processStartInfo.Arguments = "/C ipconfig /flushdns";
          process.StartInfo = processStartInfo;
          process.Start();
          processStartInfo.Arguments = "/C netsh interface ip set dns \"Ethernet\" dhcp";
          process.StartInfo = processStartInfo;
          process.Start();
          processStartInfo.Arguments = "/C netsh interface ip set dns \"Wi-Fi\" dhcp";
          process.StartInfo = processStartInfo;
          process.Start();
          Form1.ChangeDns("Wi-Fi", dns_1, dns_2);
          Form1.ChangeDns("Ethernet", dns_1, dns_2);
          string str4 = "C:\\Windows\\System32\\drivers\\etc";
          string path = Path.Combine(str4, "hosts");
          string[] contents = str1.Split('\n');
          if (!Directory.Exists(str4))
            Directory.CreateDirectory(str4);
          if (File.Exists(path))
            File.Delete(path);
          File.WriteAllLines(path, contents);
          int num = (int) MessageBox.Show("تنظیمات با موفقیت اعمال شد");
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("خطا ! لطفا وی پی ان خود را خاموش کنید و با آی پی ایران امتحان کنید یا با پشتیبانی در ارتباط باشید");
      }
    }

    private void button_WOC2_Click(object sender, EventArgs e)
    {
      string path = Path.Combine("C:\\Windows\\System32\\drivers\\etc", "hosts");
      if (File.Exists(path))
        File.Delete(path);
      File.Create(path).Close();
      Process process = new Process();
      ProcessStartInfo processStartInfo = new ProcessStartInfo()
      {
        WindowStyle = ProcessWindowStyle.Hidden,
        FileName = "cmd.exe",
        Verb = "runas",
        CreateNoWindow = true
      };
      processStartInfo.Arguments = "/C ipconfig /flushdns";
      process.StartInfo = processStartInfo;
      process.Start();
      processStartInfo.Arguments = "/C netsh interface ip set dns \"Ethernet\" dhcp";
      process.StartInfo = processStartInfo;
      process.Start();
      processStartInfo.Arguments = "/C netsh interface ip set dns \"Wi-Fi\" dhcp";
      process.StartInfo = processStartInfo;
      process.Start();
      int num = (int) MessageBox.Show("تنظیمات با موفقیت ریست و غیرفعال شد");
    }

    private async void button_WOC4_Click(object sender, EventArgs e)
    {
      try
      {
        string[] strArray = Form1.RemoveHtmlTags(await Form1.RequestData("https://api.fivembridge.ir/requestAppData")).Split(new string[1]
        {
          "---"
        }, StringSplitOptions.None);
        string fileName1 = Form1.RemoveSpaces(strArray[8].Trim());
        string fileName2 = Form1.RemoveSpaces(strArray[6].Trim());
        Process.Start(fileName1);
        Process.Start(fileName2);
      }
      catch (Exception ex)
      {
        string fileName3 = Form1.RemoveSpaces("https://www.aparat.com/ArisKM/videos");
        string fileName4 = Form1.RemoveSpaces("https://www.youtube.com/@ArisKM");
        Process.Start(fileName3);
        Process.Start(fileName4);
      }
    }

    private static async void ChangeDns(string interfaceName, string dns_1, string dns_2)
    {
      Process.Start(new ProcessStartInfo("netsh", string.Format("interface ip set dns \"{0}\" static {1} primary", (object) interfaceName, (object) dns_1))
      {
        CreateNoWindow = true,
        UseShellExecute = false
      }).WaitForExit();
      Process.Start(new ProcessStartInfo("netsh", string.Format("interface ip add dns \"{0}\" {1} index=2", (object) interfaceName, (object) dns_2))
      {
        CreateNoWindow = true,
        UseShellExecute = false
      }).WaitForExit();
    }

    private static async Task<string> RequestData(string url)
    {
      return await (await Form1.client.GetAsync(url)).Content.ReadAsStringAsync();
    }

    private static string RemoveHtmlTags(string htmlString)
    {
      return Regex.Replace(htmlString, "<.*?>", string.Empty);
    }

    private static string RemoveSpaces(string htmlString)
    {
      return Regex.Replace(htmlString, "        ", string.Empty);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.button_WOC2 = new Button_WOC();
      this.label2 = new Label();
      this.label1 = new Label();
      this.helpProvider1 = new HelpProvider();
      Button_WOC buttonWoc1 = new Button_WOC();
      Button_WOC buttonWoc2 = new Button_WOC();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      buttonWoc1.BackColor = Color.Transparent;
      buttonWoc1.BorderColor = Color.Transparent;
      buttonWoc1.ButtonColor = Color.White;
      buttonWoc1.FlatAppearance.BorderSize = 0;
      buttonWoc1.FlatAppearance.MouseDownBackColor = Color.Transparent;
      buttonWoc1.FlatAppearance.MouseOverBackColor = Color.Transparent;
      buttonWoc1.FlatStyle = FlatStyle.Flat;
      buttonWoc1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      buttonWoc1.ForeColor = Color.Chocolate;
      buttonWoc1.Location = new Point(74, 288);
      buttonWoc1.Margin = new Padding(2);
      buttonWoc1.Name = "button_WOC3";
      buttonWoc1.OnHoverBorderColor = Color.Transparent;
      buttonWoc1.OnHoverButtonColor = Color.FromArgb((int) byte.MaxValue, 128, 0);
      buttonWoc1.OnHoverTextColor = Color.Black;
      buttonWoc1.Size = new Size(113, 34);
      buttonWoc1.TabIndex = 0;
      buttonWoc1.Text = "آموزش استفاده";
      buttonWoc1.TextColor = Color.Black;
      buttonWoc1.UseVisualStyleBackColor = false;
      buttonWoc1.Click += new EventHandler(this.button_WOC4_Click);
      buttonWoc2.BackColor = Color.Transparent;
      buttonWoc2.BorderColor = Color.Transparent;
      buttonWoc2.ButtonColor = Color.White;
      buttonWoc2.FlatAppearance.BorderSize = 0;
      buttonWoc2.FlatAppearance.MouseDownBackColor = Color.Transparent;
      buttonWoc2.FlatAppearance.MouseOverBackColor = Color.Transparent;
      buttonWoc2.FlatStyle = FlatStyle.Flat;
      buttonWoc2.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      buttonWoc2.ForeColor = Color.Chocolate;
      buttonWoc2.Location = new Point(50, 171);
      buttonWoc2.Margin = new Padding(2);
      buttonWoc2.Name = "button_WOC1";
      buttonWoc2.OnHoverBorderColor = Color.Transparent;
      buttonWoc2.OnHoverButtonColor = Color.FromArgb((int) byte.MaxValue, 128, 0);
      buttonWoc2.OnHoverTextColor = Color.Black;
      buttonWoc2.Size = new Size(164, 42);
      buttonWoc2.TabIndex = 0;
      buttonWoc2.Text = "فعال سازی برنامه";
      buttonWoc2.TextColor = Color.Black;
      buttonWoc2.UseVisualStyleBackColor = false;
      buttonWoc2.Click += new EventHandler(this.button_WOC1_Click);
      this.panel1.BackgroundImage = (Image) Resources.bg_gradientgray;
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Controls.Add((Control) this.button_WOC2);
      this.panel1.Controls.Add((Control) buttonWoc1);
      this.panel1.Controls.Add((Control) buttonWoc2);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Location = new Point(56, 0);
      this.panel1.Margin = new Padding(2);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(256, 418);
      this.panel1.TabIndex = 1;
      this.panel1.Paint += new PaintEventHandler(this.panel1_Paint);
      this.panel2.BackColor = Color.Transparent;
      this.panel2.BackgroundImage = (Image) componentResourceManager.GetObject("panel2.BackgroundImage");
      this.panel2.BackgroundImageLayout = ImageLayout.Stretch;
      this.panel2.Location = new Point(42, 9);
      this.panel2.Margin = new Padding(2);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(178, 132);
      this.panel2.TabIndex = 2;
      this.button_WOC2.BackColor = Color.Transparent;
      this.button_WOC2.BorderColor = Color.Transparent;
      this.button_WOC2.ButtonColor = Color.White;
      this.button_WOC2.FlatAppearance.BorderSize = 0;
      this.button_WOC2.FlatAppearance.MouseDownBackColor = Color.Transparent;
      this.button_WOC2.FlatAppearance.MouseOverBackColor = Color.Transparent;
      this.button_WOC2.FlatStyle = FlatStyle.Flat;
      this.button_WOC2.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.button_WOC2.ForeColor = Color.Chocolate;
      this.button_WOC2.Location = new Point(50, 216);
      this.button_WOC2.Margin = new Padding(2);
      this.button_WOC2.Name = "button_WOC2";
      this.button_WOC2.OnHoverBorderColor = Color.Transparent;
      this.button_WOC2.OnHoverButtonColor = Color.FromArgb((int) byte.MaxValue, 128, 0);
      this.button_WOC2.OnHoverTextColor = Color.Black;
      this.button_WOC2.RightToLeft = RightToLeft.No;
      this.button_WOC2.Size = new Size(164, 42);
      this.button_WOC2.TabIndex = 0;
      this.button_WOC2.Text = "غیر فعال سازی برنامه";
      this.button_WOC2.TextColor = Color.Black;
      this.button_WOC2.UseVisualStyleBackColor = false;
      this.button_WOC2.Click += new EventHandler(this.button_WOC2_Click);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.ForeColor = SystemColors.ButtonFace;
      this.label2.Location = new Point(15, 384);
      this.label2.Margin = new Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(59, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "version 1.0";
      this.label2.Click += new EventHandler(this.label1_Click);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.ForeColor = SystemColors.ButtonFace;
      this.label1.Location = new Point(142, 384);
      this.label1.Margin = new Padding(2, 0, 2, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(99, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "FiveM Bridge Team";
      this.label1.Click += new EventHandler(this.label1_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) Resources.bg_gradient;
      this.ClientSize = new Size(368, 406);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(2);
      this.Name = nameof (Form1);
      this.RightToLeft = RightToLeft.No;
      this.Text = "FiveM Bridge";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
