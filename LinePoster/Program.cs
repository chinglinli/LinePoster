using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinePoster
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
        }
    }
}

//*****************************
//關鍵點 若form未顯示 無法Sendkey 因而必須在Shown之後 delegate event 
//this.Shown += new System.EventHandler(this.Form1_Shown);
