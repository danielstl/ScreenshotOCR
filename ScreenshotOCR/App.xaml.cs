using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace ScreenshotOCR
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var iconsDll = Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase).Replace("\\", "/") + "/ScreenshotOCRIcons.dll";

            var screenshotTask = new JumpTask()
            {
                Title = "Take instant screenshot",
                Description = "Immediately launch screenshot mode",
                Arguments = "/screenshot",
                IconResourcePath = iconsDll,
                IconResourceIndex = 0,
                ApplicationPath = Assembly.GetEntryAssembly().CodeBase
            };

            var s = Assembly.GetEntryAssembly();

            var settingsTask = new JumpTask()
            {
                Title = "Settings",
                Description = "Edit ScreenshotOCR settings",
                Arguments = "/settings",
                IconResourcePath = iconsDll,
                IconResourceIndex = 1,
                ApplicationPath = Assembly.GetEntryAssembly().CodeBase
            };

            var jumpList = new JumpList();
            jumpList.JumpItems.Add(screenshotTask);
            jumpList.JumpItems.Add(settingsTask);
            jumpList.ShowFrequentCategory = false;
            jumpList.ShowRecentCategory = false;

            JumpList.SetJumpList(Current, jumpList);

            if (e.Args.Length > 0)
            {
                if (e.Args[0] == "/screenshot")
                {
                    new ScreenshotTaker().Show();
                    return;
                } else if (e.Args[0] == "/settings")
                {
                    //todo
                    MessageBox.Show("settings coming soon!");
                    //return;
                }
            }

            new MainWindow().Show();
        }
    }
}
