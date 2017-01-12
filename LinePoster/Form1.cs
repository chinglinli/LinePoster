using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Exchange.WebServices;
using Microsoft.Exchange.WebServices.Data;

namespace LinePoster
{
    public partial class Form1 : Form
    {
        public FolderId target;

        public string account = System.Configuration.ConfigurationManager.AppSettings["account"];
        public string pw = System.Configuration.ConfigurationManager.AppSettings["pw"];
        public string domain = System.Configuration.ConfigurationManager.AppSettings["domain"];
        public string mailbox = System.Configuration.ConfigurationManager.AppSettings["mailbox"];

        //Line Section
        public string TargetLineGroup = System.Configuration.ConfigurationManager.AppSettings["TargetLineGroup"];

        public Form1()
        {
            InitializeComponent();
            //this.Close();
        }

        //Win32 API
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //主程式
        void PostLine()
        {
            this.Text = "Connectting Exchange Server......";
            StatusBox.Text = "Connectting Exchange Server......";
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);

            ExchangeService exchangeService = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
            exchangeService.Credentials = new WebCredentials("ian", "asdf!23", "giga");
            exchangeService.AutodiscoverUrl("ian.lee@gigamedia.com.tw");
            //exchangeService.Url = new Uri("https://ftfe01.gigamedia.com.tw/EWS/Exchange.asmx");
         
            //FindItemsResults<Item> findJunkMails = exchangeService.FindItems(WellKnownFolderName.JunkEmail, new ItemView(int.MaxValue));
            this.Text = "Finding Alert Mail......";
            StatusBox.Text = "Finding Alert Mail......";
            FindItemsResults<Item> findInBoxMails = exchangeService.FindItems(WellKnownFolderName.Inbox, new ItemView(int.MaxValue));

            //Console.WriteLine("Moving junk mails to Inbox");
            //SetView
            FolderView view = new FolderView(100);
            view.PropertySet = new PropertySet(BasePropertySet.IdOnly);
            view.PropertySet.Add(FolderSchema.DisplayName);
            view.Traversal = FolderTraversal.Deep;
            FindFoldersResults findFolderResults = exchangeService.FindFolders(WellKnownFolderName.Inbox, view);
            //find specific folder
            //piblic FolderId target; 
            foreach (Folder f in findFolderResults)
            {
                //show folderId of the folder "XXX"
                //if (f.DisplayName == "Sent2Facebook")
                    if (f.DisplayName == "Monitor Log")
                        //Console.WriteLine(f.Id);
                        target = f.Id;
            }

            //***********************************************************************
            string wName = TargetLineGroup; //LineGroup
            //string wName = "矢正"; //LineGroup

            Process process = new Process();
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(wName))
                {
                    //pList.MainWindowTitle.Contains()
                    hWnd = pList.MainWindowHandle;
                }
            }
            //hWnd = (IntPtr)001721F6;
            // Activate the Notepad Window
            // BringWindowToTop(hWnd);

            //***********************************************************************
            //this.Text = "Post to Line--" + TargetLineGroup;
            //this.BackgroundImage = "";
            //TargetLineID.Text = TargetLineGroup;

            //FindItemsResults<Item> findInBoxMailsXXX = exchangeService.FindItems(target, new ItemView(int.MaxValue));
            //SetForegroundWindow(hWnd);
            //BringWindowToTop(hWnd);
            //Thread.Sleep(2000);
            this.Text = "Ready to Post to Line Group ......";
            StatusBox.Text = "Ready to Post to Line Group ......";
            IntPtr LineWindow = FindWindow("Qt5QWindowIcon", "Funpachi_Alert");
            SetForegroundWindow(LineWindow);
            Thread.Sleep(2000);

            this.Text = "Posting to Line Group-->" + TargetLineGroup; ;
            StatusBox.Text = "Posting to Line Group-->" + TargetLineGroup; ;
            //SetForegroundWindow(hWnd);
            //foreach (Item item in findInBoxMailsXXX.Items)    //DEV Mail進件 Outlook郵件規則先送JunkMails,呼叫Chrome ,POST FB&回送InBox
            foreach (Item item in findInBoxMails.Items) //PRD
            {
                //HTML format
                //ExtendedPropertyDefinition HtmlBodyProperty = new ExtendedPropertyDefinition(0x1013, MapiPropertyType.Binary);
                //PropertySet propertySet = new PropertySet(BasePropertySet.FirstClassProperties, HtmlBodyProperty);

                //Plain-Text
                PropertySet propertySet = new PropertySet(BasePropertySet.FirstClassProperties);
                propertySet.RequestedBodyType = BodyType.Text;

                // Bind to the message by using the property set.
                EmailMessage message = EmailMessage.Bind(exchangeService, item.Id, propertySet);

                string subject = message.Subject;
                if (subject ==  null)
                {
                    subject = "(空白)";
                }

                string body = message.Body.Text;
                if (body == null)
                {
                    body = "(空白)";
                }

                string LineMessage = "[時間]:" + message.DateTimeCreated.ToString() + "\n\n" + "[事件]:\n" + subject + "\n\n" + "[訊息]:\n" + body + "\n\n";
             
                //AlertMessage.FBAlert(subject, body, "", "");
                Clipboard.SetText(LineMessage, TextDataFormat.Text);

                //Response.Write("5秒...");
                Thread.Sleep(1000);  //間隔5秒 因避免被FB封鎖 若發訊間隔太近

                // Use SendKeys to Paste
                SendKeys.Send("^{v}");
                Thread.Sleep(2000);
                SendKeys.Send("{ENTER}");
                Clipboard.Clear();
                //Clipboard.Clear();
                //item.Move(WellKnownFolderName.Inbox);       //DEV
                item.Move(target); //Post到Line的Mail,移至Sent2Facebook  PRD
            }
            //process.Start();

            // Copy the text in the datafield to Clipboard
            Clipboard.Clear();
            Clipboard.SetText("The End--------------", TextDataFormat.Text);
            SendKeys.Send("^{v}");
            Thread.Sleep(2000);
            SendKeys.Send("{ENTER}");
            this.Close();
            //Clipboard.Clear();

        }

        private void Post2Line_Click(object sender, EventArgs e)
        {
            PostLine();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Thread.Sleep(3000);
            PostLine();
            //this.Close();
            //Post2Line.PerformClick();
            //MessageBox.Show("CC");
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {

        }
    }
    
    }


//****************************************************
//20160222 PRD Working Vesion 
//
//****************************************************
//namespace LinePoster
//{
//    public partial class Form1 : Form
//    {
//        public FolderId target;

//        public string account = System.Configuration.ConfigurationManager.AppSettings["account"];
//        public string pw = System.Configuration.ConfigurationManager.AppSettings["pw"];
//        public string domain = System.Configuration.ConfigurationManager.AppSettings["domain"];
//        public string mailbox = System.Configuration.ConfigurationManager.AppSettings["mailbox"];

//        //Line Section
//        public string TargetLineGroup = System.Configuration.ConfigurationManager.AppSettings["TargetLineGroup"];

//        public Form1()
//        {
//            InitializeComponent();
//            //this.Close();
//        }

//        //Win32 API
//        [DllImport("user32.dll", SetLastError = true)]
//        private static extern bool BringWindowToTop(IntPtr hWnd);

//        [DllImport("User32.dll")]
//        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

//        //主程式
//        void PostLine()
//        {
//            this.Text = "Search MailBox...";
//            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);

//            ExchangeService exchangeService = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
//            exchangeService.Credentials = new WebCredentials("ian", "qwer!23", "giga");
//            exchangeService.AutodiscoverUrl("ian.lee@gigamedia.com.tw");

//            //FindItemsResults<Item> findJunkMails = exchangeService.FindItems(WellKnownFolderName.JunkEmail, new ItemView(int.MaxValue));

//            //FindItemsResults<Item> findInBoxMails = exchangeService.FindItems(WellKnownFolderName.Inbox, new ItemView(int.MaxValue));

//            //Console.WriteLine("Moving junk mails to Inbox");
//            //SetView
//            FolderView view = new FolderView(100);
//            view.PropertySet = new PropertySet(BasePropertySet.IdOnly);
//            view.PropertySet.Add(FolderSchema.DisplayName);
//            view.Traversal = FolderTraversal.Deep;
//            FindFoldersResults findFolderResults = exchangeService.FindFolders(WellKnownFolderName.Inbox, view);
//            //find specific folder
//            //piblic FolderId target; 
//            foreach (Folder f in findFolderResults)
//            {
//                //show folderId of the folder "XXX"
//                if (f.DisplayName == "XXX")
//                    //Console.WriteLine(f.Id);
//                    target = f.Id;
//            }

//            //***********************************************************************
//            string wName = TargetLineGroup; //LineGroup
//            //string wName = "矢正"; //LineGroup

//            Process process = new Process();
//            IntPtr hWnd = IntPtr.Zero;
//            foreach (Process pList in Process.GetProcesses())
//            {
//                if (pList.MainWindowTitle.Contains(wName))
//                {
//                    //pList.MainWindowTitle.Contains()
//                    hWnd = pList.MainWindowHandle;
//                }
//            }
//            //hWnd = (IntPtr)001721F6;
//            // Activate the Notepad Window
//            // BringWindowToTop(hWnd);

//            //***********************************************************************
//            this.Text = "Post to Line--" + TargetLineGroup;
//            //this.BackgroundImage = "";
//            //TargetLineID.Text = TargetLineGroup;

//            FindItemsResults<Item> findInBoxMailsXXX = exchangeService.FindItems(target, new ItemView(int.MaxValue));

//            BringWindowToTop(hWnd);
//            foreach (Item item in findInBoxMailsXXX.Items)    //DEV Mail進件 Outlook郵件規則先送JunkMails,呼叫Chrome ,POST FB&回送InBox
//            //foreach (Item item in findInBoxMails.Items) //PRD
//            {
//                //HTML format
//                //ExtendedPropertyDefinition HtmlBodyProperty = new ExtendedPropertyDefinition(0x1013, MapiPropertyType.Binary);
//                //PropertySet propertySet = new PropertySet(BasePropertySet.FirstClassProperties, HtmlBodyProperty);

//                //Plain-Text
//                PropertySet propertySet = new PropertySet(BasePropertySet.FirstClassProperties);
//                propertySet.RequestedBodyType = BodyType.Text;

//                // Bind to the message by using the property set.
//                EmailMessage message = EmailMessage.Bind(exchangeService, item.Id, propertySet);

//                string subject = message.Subject;
//                string body = message.Body.Text;
//                //AlertMessage.FBAlert(subject, body, "", "");
//                Clipboard.SetText(message.Body.Text, TextDataFormat.Text);

//                //Response.Write("5秒...");
//                Thread.Sleep(1000);  //間隔5秒 因避免被FB封鎖 若發訊間隔太近

//                // Use SendKeys to Paste
//                SendKeys.Send("^{v}");
//                Thread.Sleep(2000);
//                SendKeys.Send("{ENTER}");
//                //Clipboard.Clear();
//                //item.Move(WellKnownFolderName.Inbox);       //DEV
//                //item.Move(target); //發送後的Mail,移至Sent2Facebook  PRD
//            }
//            //process.Start();

//            // Copy the text in the datafield to Clipboard
//            Clipboard.Clear();
//            Clipboard.SetText("-----------The End--------------", TextDataFormat.Text);
//            SendKeys.Send("^{v}");
//            Thread.Sleep(2000);
//            SendKeys.Send("{ENTER}");
//            this.Close();
//            //Clipboard.Clear();

//        }

//        private void Post2Line_Click(object sender, EventArgs e)
//        {
//            PostLine();
//        }

//        private void Form1_Load(object sender, EventArgs e)
//        {

//        }

//        private void Form1_Shown(object sender, EventArgs e)
//        {
//            Thread.Sleep(3000);
//            PostLine();
//            //this.Close();
//            //Post2Line.PerformClick();
//            //MessageBox.Show("CC");
//        }

//        private void Form1_DoubleClick(object sender, EventArgs e)
//        {

//        }
//    }

//}

//****************************************************
//****************************************************


//****************************************************
//****************************************************
//uxData.Text = "123";
//SendMessage(hWnd, 0x000C, 0, message.Body.Text);
//SendMessage(hWnd, 0x0100, 0x03, message.Body.Text);
//uxData.Text = "456";

//namespace LinePoster
//{
//    public partial class Form1 : Form
//    {
//        public Form1()
//        {
//            InitializeComponent();
//        }

//        [DllImport("user32.dll", SetLastError = true)]
//        private static extern bool BringWindowToTop(IntPtr hWnd);

//        private void Post2Line_Click(object sender, EventArgs e)
//        {
//            // Let's start Notepad
//            Process process = new Process();
//            //process.StartInfo.FileName = "C:\\Windows\\Notepad.exe";
//            process.StartInfo.FileName = "C:\\Program Files (x86)\\LINE\\Line.exe";
//            process.Start();

//            // Give the process some time to startup
//            Thread.Sleep(1000);

//            // Copy the text in the datafield to Clipboard
//            Clipboard.SetText(uxData.Text, TextDataFormat.Text);

//            // Get the Notepad Handle
//            IntPtr hWnd = process.Handle;

//            // Activate the Notepad Window
//            BringWindowToTop(hWnd);

//            // Use SendKeys to Paste
//            SendKeys.Send("^V");

//        }

//    }
//}