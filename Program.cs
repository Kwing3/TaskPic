using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskPic
{
    static class Program
    {

        static NotifyIcon notifyIcon;
        static bool isChanging = false;
        static Thread changeThread;
        static Icon icon1;
        static Icon icon2;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            icon1 = new Icon(@"../../Icon1.ico");
            icon2 = new Icon(@"../../Icon2.ico");

            notifyIcon = new NotifyIcon
            {
                Icon = icon1,
                Visible = true
            };

            notifyIcon.DoubleClick += (sender, e) =>
            {
                Application.Exit(); // ダブルクリックで終了
            };

            notifyIcon.Click += (sender, e) =>
            {
                if (isChanging)
                {
                    StopChanging();
                }
                else
                {
                    StartChanging();
                }
            };

            Application.Run();
        }

        static void StartChanging()
        {
            isChanging = true;
            changeThread = new Thread(() =>
            {
                while (isChanging)
                {
                    // アイコンを交互に切り替える
                    if (notifyIcon.Icon == icon1)
                    {
                        notifyIcon.Icon = icon2;
                    }
                    else
                    {
                        notifyIcon.Icon = icon1;
                    }

                    Thread.Sleep(500); // 速さ（ミリ秒単位）を調整
                }
            });

            changeThread.Start();
        }

        static void StopChanging()
        {
            isChanging = false;
            changeThread.Join();
            notifyIcon.Icon = icon1;
        }

    }
}
