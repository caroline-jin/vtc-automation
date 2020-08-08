using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using Windows.Foundation;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Windows.Media.AppRecording;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.IO.Ports;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
//using Accord.Video.FFMPEG;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium;
using WindowsInput.Native;
using WindowsInput;
using System.Net.Sockets;
using System.Net;
using System.Net.Configuration;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;
using Windows.System;

namespace ConsoleApp1
{
    class Program
    {
        InputSimulator sim = new InputSimulator();

        /*const UInt32 WM_KEYDOWN = 0x0100;
        const int VK_F4 = 0x73;
        */
        List<string> findWindowsName(string logfile)
        {
            List<string> namelist = new List<string>();
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (process.ProcessName.Contains("Teams"))
                {
                    Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                    if (!String.IsNullOrEmpty(process.MainWindowTitle))
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                        {
                            file.WriteLine("Found Teams Window: "+ process.MainWindowTitle);
                        }
                        namelist.Add(process.MainWindowTitle);
                    }
                }
            }
            Console.WriteLine(" ");
            //Console.WriteLine(namelist.Count);
            return namelist;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        /*[System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);*/
        [System.Runtime.InteropServices.DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        void GetTeamsWindow(string logfile)
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path = home + @"\AppData\Local\Microsoft\Teams\current\Teams.exe";
            Process teams = Process.Start(path);
            for (int x = 0; x < 3; x++)
            {
                try
                {
                    List<string> Teams_window = findWindowsName(logfile);
                    string window_name = Teams_window[0]; //default
                    IntPtr teams_handle = FindWindow("Chrome_WidgetWin_1", window_name);
                    SetForegroundWindow(teams_handle);
                    try
                    {
                        teams.WaitForInputIdle();
                    }
                    catch (InvalidOperationException e1)
                    {
                        Console.WriteLine("Exception caught: {0}", e1);
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                        {
                            file.WriteLine("Exception caught: {0}", e1);
                        }
                    }
                    break;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("Exception raised: " + e.Message);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                    {
                        file.WriteLine("Exception raised: " + e.Message);
                    }
                    // teams.CloseMainWindow();
                    //teams.Close();
                    teams.Kill();
                    Thread.Sleep(2000);
                    teams.Start();
                }
            }
                                                  // bool call = CheckCall(Teams_window);
            /* if (Teams_window.Count>1)
             {
                 foreach (string name in Teams_window)
                 {
                     if (!name.Contains("Call in progress"))
                     {
                         window_name = name;
                     }
                 }
             }*/

        }

        void AddPerson(string name)
        {
            Thread.Sleep(1000);
            for (int x = 0; x < 3; x++)
            {
                try
                {
                    call_session.FindElementByAccessibilityId("roster-button").Click();
                    Thread.Sleep(1000);
                    for (int y = 0; y < 3; y++)
                    {
                        try
                        {
                            call_session.FindElementByAccessibilityId("people-picker-input").Click();
                            break;
                        }
                        catch
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    break;
                }
                catch
                { 
                    Thread.Sleep(1000);
                }
            }
           // call_session.FindElementByAccessibilityId("people-picker-input").Click();
            Thread.Sleep(1000);
            SendKeys.SendWait(name);
            Thread.Sleep(2000);
            SendKeys.SendWait("{DOWN}");
            Thread.Sleep(2000);
            SendKeys.SendWait("{ENTER}");
            Console.WriteLine("Added {0} to the call", name);
        }

        void ShareScreen()
        {
            Console.WriteLine("Sharing screen...");
          //  call_session.FindElementByAccessibilityId("share-button").Click();
            InputSimulator sim = new InputSimulator();
            sim.Keyboard.Sleep(2000);
            sim.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
            sim.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
            sim.Keyboard.KeyPress(VirtualKeyCode.VK_E);
           // sim.Keyboard.Sleep(3000);
           //sim.Keyboard.KeyPress(VirtualKeyCode.SPACE);
            sim.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
            sim.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
            sim.Keyboard.Sleep(2000);
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
           
          //
          //call_session.FindElementByName("Desktop Screen #1").Click();
            Console.WriteLine("Shared desktop");
        }

        void ToggleMute()
        {
            SendKeys.SendWait("^+m");
            //    Console.ReadLine();
            Console.WriteLine("Mic on/off");

        }

        void ToggleVideo()
        {
            SendKeys.SendWait("^+o");
            // Console.ReadLine();
            Console.WriteLine("Video on/off");

        }

        void HangUp()
        {
            try
            {
                call_session.FindElementByName("Leave (Ctrl+Shift+B)").Click();
            }
            catch
            {
                SendKeys.SendWait("^+B");
            }

            // SendKeys.SendWait("^+b");
            // Console.ReadLine();
            Console.WriteLine("Call ended by tool");

        }
        void PickUpCall()
        {
            /*   while (CheckPendingCall() != true)
               {
                   Thread.Sleep(1000);
               }*/
            SendKeys.SendWait("^+s");
            //Console.ReadLine();
            Console.WriteLine("Picked up call!");

        }

        void SendCall(string name)
        {
            InputSimulator sim = new InputSimulator();

            // CTRL + C (effectively a copy command in many situations)
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_E);
            sim.Keyboard.TextEntry(name);
            sim.Keyboard.Sleep(5000);
            sim.Keyboard.KeyPress(VirtualKeyCode.DOWN);
            sim.Keyboard.Sleep(1000);
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            /*SendKeys.SendWait("^(e)");
            SendKeys.SendWait(name);
            Thread.Sleep(1000);
            SendKeys.SendWait("{DOWN}");
            SendKeys.SendWait("{ENTER}");*/
            Thread.Sleep(2000);
           SendKeys.SendWait("^+C");
            //Ctrl+Shift+B to leave call
            Console.WriteLine("Called {0}", name);


        }


        static bool CheckPendingCall()
        {
            string app = "com.squirrel.Teams.Teams";

            IReadOnlyList<ToastNotification> notifs = ToastNotificationManager.History.GetHistory(app);
            ToastNotification first = notifs[0];
            string info = first.Content.InnerText;
            if (info.Contains("call") == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        void ScreenRecord(string path, int width, int height)
        {
            List<string> imagesequence = new List<string>();
            int FileCount = 1;
            // string filepath = CreateTempFolder("Screenshots");
            string directory = path + @"\recordings";
            for (int i = 0; i < 100; i++)
            {
                FileCount = Screenshot(directory, FileCount, width, height);
                Thread.Sleep(100);
            }
        }

        void DeleteScreenshots(string path)
        {
            string directory = path + @"\recordings";
            string[] files = Directory.GetFiles(directory);

            foreach (string file in files) {
                if (file != "ffmpeg" || file != "finalvideo.mp4")
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

            }
        }

        int Screenshot(string path, int count, int width, int height)
        {
            //Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap bm = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            Console.WriteLine(bm.Size);
            string filename = path + "//screenshot-" + count + ".png";
            bm.Save(filename, ImageFormat.Png);
            //ImageSequence.Add(filename);
           // count++;
            bm.Dispose();
            return count;
        }

        string CreateTempFolder(string name)
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path = home + @"\Desktop\" + name;
            Directory.CreateDirectory(path);
            return path;
        }

        void SendFile(string filename)
        {
            //sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_O);
            Console.WriteLine("Sending file...");
            sim.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
            sim.Keyboard.KeyPress(VirtualKeyCode.VK_O);
            sim.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
            Console.WriteLine("control+O");
            sim.Keyboard.Sleep(1000);
            sim.Keyboard.KeyPress(VirtualKeyCode.DOWN);
             sim.Keyboard.Sleep(1000);
             sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
           
           // SendKeys.SendWait("^o");
            // SendKeys.SendWait("{DOWN}");
             // SendKeys.SendWait("{ENTER}");
              SendKeys.SendWait(filename);
              SendKeys.SendWait("{ENTER}");
            Console.WriteLine("Here!");
               Console.ReadLine();
        }
        protected static WindowsDriver<WindowsElement> session;
        protected static WindowsDriver<WindowsElement> desktop_session;
        private const string AppId = "com.squirrel.Teams.Teams";
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        void startSession(string logfile)
        {
           
                AppiumOptions options = new AppiumOptions();
                Uri url = new Uri(WindowsApplicationDriverUrl);

                string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string path = home + @"\AppData\Local\Microsoft\Teams\current\Teams.exe";
                Process teams = Process.Start(path);
               // options.AddAdditionalCapability("app", path);
                                
                options .AddAdditionalCapability("app", "Root");
                desktop_session = new WindowsDriver<WindowsElement>(url, options);
            for (int x = 0; x < 3; x++)
            {
                try
                {
                    List<string> namelist = findWindowsName(logfile);
                    string windows_name = namelist[0]; //default
                    if (windows_name.Contains("Call in progress"))
                    {
                        windows_name = windows_name.Replace("Microsoft Teams Call in progress ", "");
                        windows_name = windows_name + " | Microsoft Teams";
                    }
                    windows_name = windows_name + ", Main Window";
                    var mainwindow = desktop_session.FindElementByName(windows_name);
                    var mainWindowHandle = mainwindow.GetAttribute("NativeWindowHandle");
                    mainWindowHandle = (int.Parse(mainWindowHandle)).ToString("x"); // Convert to Hex
                    options = new AppiumOptions();
                    options.AddAdditionalCapability("deviceName", "WindowsPC");
                    options.AddAdditionalCapability("appTopLevelWindow", mainWindowHandle);
                    session = new WindowsDriver<WindowsElement>(url, options);
                    // Assert.IsNotNull(session);
                    //  Console.WriteLine("Found the Teams Window");
                    // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                    session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
                    break;

                    // session.FindElementByName("Audio call").Click();

                }
                catch 
                {
                    if (x != 2)
                    {
                        Console.WriteLine("Exception raised finding Teams window... trying again");
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                        {
                            file.WriteLine("Exception raised finding Teams window... trying again");
                        }
                        // teams.CloseMainWindow();
                        //teams.Close();
                        teams.Kill();
                        Thread.Sleep(2000);
                        teams.Start();
                    }
                    else
                    {
                        Console.WriteLine("Exception not handled... returning to main menu");
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                        {
                            file.WriteLine("Exception not handled... returning to main menu");
                        }
                    }
                }
            }
           /* if(call_status)
            {
                var mainwindow = desktop_session.FindElementByName(windows_name);
                var mainWindowHandle = mainwindow.GetAttribute("NativeWindowHandle");
                mainWindowHandle = (int.Parse(mainWindowHandle)).ToString("x"); // Convert to Hex
                options = new AppiumOptions();
                options.AddAdditionalCapability("deviceName", "WindowsPC");
                options.AddAdditionalCapability("appTopLevelWindow", mainWindowHandle);
                session = new WindowsDriver<WindowsElement>(url, options);
                // Assert.IsNotNull(session);
                //  Console.WriteLine("Found the Teams Window");
                // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
            }*/
           
                
               
        }

        protected static WindowsDriver<WindowsElement> call_session;
        void findCallSession(string logfile)
        {
            /* AppiumOptions options = new AppiumOptions();
             Uri url = new Uri(WindowsApplicationDriverUrl);

             List<string> namelist = findWindowsName();
             foreach (string name in namelist)
             {
                 var mainwindow = desktop_session.FindElementByName(name);
                 var mainWindowHandle = mainwindow.GetAttribute("NativeWindowHandle");
                 mainWindowHandle = (int.Parse(mainWindowHandle)).ToString("x"); // Convert to Hex
                 options = new AppiumOptions();
                 options.AddAdditionalCapability("deviceName", "WindowsPC");
                 options.AddAdditionalCapability("appTopLevelWindow", mainWindowHandle);
                 call_session = new WindowsDriver<WindowsElement>(url, options);
                     try
                     {
                         var call_button = call_session.FindElementByName("Leave Call Ctrl+Shift+B"); //find proper name
                         IntPtr nativewindowhandle= new IntPtr(long.Parse(mainWindowHandle));
                         SetForegroundWindow(nativewindowhandle);
                         break;
                     }
                     catch
                     {

                     }
             }*/
            /*var windows = session.WindowHandles;
            foreach (var win in windows)
            {
                Console.WriteLine("Window is " + win);
            }
            foreach (var new_window in windows)
            {
                session.SwitchTo().Window(new_window);
                try
                {
                    var call_button = session.FindElementByName("Leave Call Ctrl+Shift+B");
                    break;
                }
                catch {
                    Console.WriteLine("NO call window!");
                }
            }*/

            for (int x = 0; x < 3; x++)
            {
                try
                {
                    List<string> namelist = findWindowsName(logfile);
                    string windows_name = namelist[0]; //default

                    var call_window = desktop_session.FindElementByName(windows_name);
                    var mainWindowHandle = call_window.GetAttribute("NativeWindowHandle");
                    mainWindowHandle = (int.Parse(mainWindowHandle)).ToString("x"); // Convert to Hex

                    AppiumOptions options = new AppiumOptions();
                    Uri url = new Uri(WindowsApplicationDriverUrl);

                    options.AddAdditionalCapability("deviceName", "WindowsPC");
                    options.AddAdditionalCapability("appTopLevelWindow", mainWindowHandle);
                    call_session = new WindowsDriver<WindowsElement>(url, options);
                    break;
                }
                catch
                {
                    if (x != 3)
                    {
                        Console.WriteLine("Exception caught finding call window! Trying again...");
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                        {
                            file.WriteLine("Exception caught finding call window! Trying again...");
                        }
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.WriteLine("Exception not handled. Returning to main menu");
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                        {
                            file.WriteLine("Call window exception not handled. Returning to main menu");
                        }
                    }
                }
            }
            }

        void SendScreenshot(string filename, string path, string logfile)
        {
            bool error = false;
            bool sent = false;
            for (int x = 0; x < 3; x++)
            {
                try
                {
                    session.FindElementByName("Attach. Use left and right arrow keys to navigate.").Click();
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    SendKeys.SendWait("{DOWN}");
                    Thread.Sleep(1000);
                    SendKeys.SendWait("{ENTER}");
                    Thread.Sleep(1000);
                    error = false;
                    for (int y=0; y<3; y++)
                    {
                        try
                        {
                            var fileExplorer = session.FindElementByClassName("#32770");
                            error = false;
                            for (int z=0; z<3; z++)
                            {
                                try
                                {
                                    var editbox = fileExplorer.FindElementByClassName("ToolbarWindow32");
                                    editbox.Click();
                                    error = false;
                                    SendKeys.SendWait(path);
                                    //fileExplorer.FindElementByName("Downloads").Click();
                                    //fileExplorer.FindElementByName("app").Click();
                                    SendKeys.SendWait("{ENTER}");
                                    for (int a = 0; a < 3; a++)
                                    {
                                        try
                                        {
                                            var textbox = fileExplorer.FindElementByAccessibilityId("1148");
                                            textbox.Click();
                                            error = false;
                                            Thread.Sleep(TimeSpan.FromSeconds(2));
                                            SendKeys.SendWait(filename);
                                            Thread.Sleep(TimeSpan.FromSeconds(2));
                                            SendKeys.SendWait("{ENTER}");
                                            Thread.Sleep(TimeSpan.FromSeconds(2));
                                            try
                                            {
                                                var popup = session.FindElementByName("This file already exists");
                                                popup.FindElementByName("Replace").Click();
                                                while (!sent)
                                                {
                                                    try
                                                    {
                                                        session.FindElementByName("Send").Click();
                                                        var update = session.FindElementByName("Uploading... Please wait.");
                                                        Console.WriteLine("Uploading...");
                                                        Thread.Sleep(TimeSpan.FromSeconds(2));
                                                    }
                                                    catch
                                                    {
                                                        //                        session.FindElementByName("Send").Click();
                                                        sent = true;
                                                        Console.WriteLine("Sent!");
                                                    }
                                                }

                                            }
                                            catch
                                            {

                                                while (!sent)
                                                {
                                                    try
                                                    {
                                                        session.FindElementByName("Send").Click();
                                                        var update = session.FindElementByName("Uploading... Please wait.");
                                                        Console.WriteLine("Uploading...");
                                                        Thread.Sleep(TimeSpan.FromSeconds(2));
                                                    }
                                                    catch
                                                    {
                                                        //                       session.FindElementByName("Send").Click();
                                                        sent = true;
                                                        Console.WriteLine("Sent!");
                                                    }
                                                }

                                            }
                                            //fileExplorer.FindElementByName("Open").Click();
                                            Console.WriteLine("File sent!");
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                                            {
                                                file.WriteLine("File sent!");
                                            }
                                            break;

                                        }
                                        catch
                                        {
                                            Console.WriteLine("Exception caught! Textbox not found");
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                                            {
                                                file.WriteLine("Exception caught! Textbox not found");
                                            }
                                            error = true;
                                            Thread.Sleep(1000);
                                        }
                                    }
                                    if (!error)
                                    {
                                        break;
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Exception caught! Textbox not found");
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                                    {
                                        file.WriteLine("Exception caught! Textbox not found");
                                    }
                                    error = true;
                                    Thread.Sleep(1000);
                                }
                            }
                            if (!error)
                            {
                                break;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Exception caught! File explorer not found");
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Exception caught! File explorer not found");
                            }
                            error = true;
                            Thread.Sleep(1000);
                        }
                    }
                    if (!error)
                    {
                        break;
                    }
                }
                catch
                {
                    Console.WriteLine("Exception caught! Attach button not found");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                    {
                        file.WriteLine("Exception caught! Attach button not found");
                    }
                    error = true;
                    Thread.Sleep(1000);
                }
            }

            //  var filesPane = session.FindElementByName("Files Pane");
            //filesPane.FindElementByName("Upload from my computer").Click();

            
            
            
            
           
        }

        void SendFileInCall(string filename, string path, string logfile)
        {
            bool sent = false;
            for (int x = 0; x < 3; x++)
            {
                try
                {
                    call_session.FindElementByAccessibilityId("chat-button").Click();
                    Thread.Sleep(3000);
                    for (int y = 0; y < 3; y++)
                    {
                        try
                        {
                            try
                            {
                                call_session.FindElementByName("Attach Files").Click();
                            }
                            catch
                            {
                                call_session.FindElementByAccessibilityId("chat-button").Click();
                                Thread.Sleep(3000);
                                call_session.FindElementByName("Attach Files").Click();
                            }

                            Thread.Sleep(1000);
                            SendKeys.SendWait("{DOWN}");
                            Thread.Sleep(1000);
                            SendKeys.SendWait("{ENTER}");
                            Thread.Sleep(1000);
                            for (int z = 0; z < 3; z++)
                            {
                                try
                                {
                                    var fileExplorer = call_session.FindElementByClassName("#32770");
                                    for (int a = 0; a < 3; a++)
                                    {
                                        try
                                        {
                                            var editbox = fileExplorer.FindElementByClassName("ToolbarWindow32");
                                            editbox.Click();
                                            SendKeys.SendWait(path);
                                            //fileExplorer.FindElementByName("Downloads").Click();
                                            //fileExplorer.FindElementByName("app").Click();
                                            SendKeys.SendWait("{ENTER}");
                                            //   var textbox = fileExplorer.FindElementByAccessibilityId("1148");
                                            //  textbox.Click();
                                            Thread.Sleep(TimeSpan.FromSeconds(2));
                                            SendKeys.SendWait(filename);
                                            Thread.Sleep(TimeSpan.FromSeconds(2));
                                            SendKeys.SendWait("{ENTER}");
                                            Thread.Sleep(TimeSpan.FromSeconds(2));

                                            try
                                            {

                                                var popup = call_session.FindElementByName("This file already exists");
                                                Console.WriteLine("Replacing file...");
                                                popup.FindElementByName("Replace ").Click();
                                                while (!sent)
                                                {
                                                    try
                                                    {
                                                        call_session.FindElementByName("Send").Click();
                                                        var update = call_session.FindElementByName("Uploading... Please wait.");
                                                        Console.WriteLine("Uploading...");
                                                        Thread.Sleep(TimeSpan.FromSeconds(2));
                                                    }
                                                    catch
                                                    {
                                                        //                        session.FindElementByName("Send").Click();
                                                        sent = true;
                                                        Console.WriteLine("Sent!");
                                                    }
                                                }

                                            }
                                            catch
                                            {

                                                while (!sent)
                                                {
                                                    try
                                                    {
                                                        call_session.FindElementByName("Send").Click();
                                                        var update = call_session.FindElementByName("Uploading... Please wait.");
                                                        Console.WriteLine("Uploading");
                                                        Thread.Sleep(TimeSpan.FromSeconds(2));
                                                    }
                                                    catch
                                                    {
                                                        //                       session.FindElementByName("Send").Click();
                                                        sent = true;
                                                        Console.WriteLine("Sent!");
                                                    }
                                                }

                                            }
                                            //fileExplorer.FindElementByName("Open").Click();
                                            Console.WriteLine("File sent!");
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                                            {
                                                file.WriteLine("File sent!");
                                            }
                                            break;
                                        }
                                        catch
                                        {
                                            Console.WriteLine("Exception caught! Textbox not found");
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                                            {
                                                file.WriteLine("Exception caught! Textbox not found");
                                            }
                                            Thread.Sleep(1000);
                                        }
                                    }
                                break;
                                }
                                catch
                                {
                                    Console.WriteLine("Exception caught! File explorer not found");
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                                    {
                                        file.WriteLine("Exception caught! File explorer not found");
                                    }
                                    Thread.Sleep(1000);
                                }
                            }
                        break;
                        }
                        catch
                        {
                            Console.WriteLine("Exception caught! Attach files not found");
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Exception caught! Attach files not found");
                            }
                            Thread.Sleep(1000);
                        }
                    }
                break;
                }
                catch
                {
                    Console.WriteLine("Exception caught! Chat button not found");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                    {
                        file.WriteLine("Exception caught! Chat button not found");
                    }
                    Thread.Sleep(1000);
                }
            }

        }

        bool CheckCall(List<string> names)
        {
            if (names.Count>1)
                {
                    return true;
                }
            else
            {
                return false;
            }
        }
        
        Process startWinAppDriver()
        {
            //  string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
            string remove = @"\ConsoleApp1\ConsoleApp1\bin\Debug";
            string path = @"\winappdriver\WinAppDriver.exe";
            if (strWorkPath.EndsWith(remove))
            {
                strWorkPath = strWorkPath.Substring(0, strWorkPath.LastIndexOf(remove));

            }
            path = strWorkPath + path;
            Process winappdriver = Process.Start(path);
            return winappdriver;

        }

        List<UserInput> downloadJSONdata(string url)
        {
            using (var w = new WebClient())
            {

                var json_data = string.Empty;
                // attempt to download JSON data as a string
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch
                {
                  //  Console.WriteLine("whoops!");
                }
                var user_input = JsonConvert.DeserializeObject<List<UserInput>>(json_data);
                //return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<UserList>(json_data) : new UserList();
                return user_input;
            }
            /*string html = url;

            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);

            var element = htmlDoc.GetElementbyId("results");
            string data = "";
            if (element != null)
            {
                data = element.InnerText;
            }
            var user_input = JsonConvert.DeserializeObject<List<UserInput>>(data);

            return user_input;*/

            /*using (var client = new WebClient())
            {
                var s = client.DownloadString(url);
                var htmldoc2 = (IHTMLDocument2)new HTMLDocument();
                htmldoc2.write(s);
                var plainText = htmldoc2.body.outerText;
                var user_input = JsonConvert.DeserializeObject<List<UserInput>>(plainText);
                return user_input;
            }*/
            

        }


        static SerialPort serialPort;

        static void Main(string[] args) {

            var myclass = new Program();
            int count = 0;
            int width, height;
            string filename;


            /*List<UserInput> item;
            item = myclass.downloadJSONdata("https://localhost:44316/api/Items");


            UserInput item_object = item[0];
            Console.Write(item_object.name);
            Console.ReadLine();*/

            List<UserInput> item;
            UserInput item_option = new UserInput();
            Process winappdriver = myclass.startWinAppDriver();

            bool exit = true;
            bool sharescreen = false;
            // List<string> windows_names;
            bool in_call = false;
            bool command;
            string choice;
            int counter = 0;
            string identifier;
            string port; 

            Console.WriteLine("What are the dimensions of your screen?");
            Console.Write("Width: ");
            width = Convert.ToInt32(Console.ReadLine());
            Console.Write("Height: ");
            height = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("What is the unique ID?");
            identifier = Console.ReadLine();

            Console.WriteLine("What is the Bluetooth port?");
            port = Console.ReadLine();


            serialPort = new SerialPort();
            try
            {
                serialPort.PortName = port; 
                serialPort.BaudRate = 9600;
            }
            catch
            {
                Console.WriteLine("Check your bluetooth.");
            }



            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
            string remove = @"\ConsoleApp1\ConsoleApp1\bin\Debug";
            if (strWorkPath.EndsWith(remove))
            {
                strWorkPath = strWorkPath.Substring(0, strWorkPath.LastIndexOf(remove));

            }

            string date = DateTime.Now.ToString("MM-dd-yyyy");
            string logfile = strWorkPath + @"\" + date + ".txt";

            string text = "\n:" + DateTime.Now.ToString("h:mm:ss tt") + "The automation tool has started!";
            if (!File.Exists(logfile))
            {
                System.IO.File.WriteAllText(logfile, text);
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                {
                    file.WriteLine(text);
                }
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = strWorkPath + @"\taskscheduler.ps1";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = false;
            Process process = new Process(); 
            process.StartInfo = startInfo;
            process.Start();

            //open teams
            while (exit)
            {
                //check is user is in call or not
                if (!in_call)
                {
                    if (!sharescreen)
                    {
                        myclass.startSession(logfile);
                    }
                    Console.WriteLine("You are not in a call. What would you like to do?");
                    Console.WriteLine("1)Place an audio call 2)Pick up an incoming call 3)Screenshot 4)Send file 5)Share screen 6)Exit");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                    {
                        file.WriteLine("You are not in a call. What would you like to do?");
                        file.WriteLine("1)Place an audio call 2)Pick up an incoming call 3)Screenshot 4)Send file 5)Share screen 6)Exit");
                    }
                    item = myclass.downloadJSONdata("https://automationtest2.azurewebsites.net/api/Items");
                    command = false;
                    while (!command)
                    {
                        if (item != null)
                        {
                            foreach (UserInput option in item)
                            {
                                if (option.unique_id == identifier)
                                {
                                    if (option.count > counter || option.count == 0)
                                    {
                                        item_option = option;
                                        counter = option.count;
                                        command = true;
                                        break;
                                    }
                                }
                            }
                        }
                        Thread.Sleep(1000);
                        Console.WriteLine("Checking for new data...");
                        item = myclass.downloadJSONdata("https://automationtest2.azurewebsites.net/api/Items");

                    }
                 /*   while (item[(item.Count - 1)].count <= counter)
                    {
                        Thread.Sleep(1000);
                        item = myclass.downloadJSONdata("https://automationtest2.azurewebsites.net/api/Items");
                        Console.WriteLine("Checking for new data...");
                       // Console.WriteLine("Count is:" + item.Count + "Counter is: " + counter);
                    }*/
                    //counter++;
                   // Console.WriteLine("Counter ++"); 
                   /* if (item[(item.Count - 1)].count == counter)
                    {
                        item_option = item[(item.Count - 1)];
                    }
                    else
                    {
                        item_option = item[(item.Count - 1)];
                        counter = item[(item.Count - 1)].count;
                       // Console.WriteLine("Counter changed to " + counter);
                    }*/
                    //choice = Console.ReadLine();
                    switch (item_option.name)
                    {
                        case "1":
                            Console.WriteLine("Who do you want to call?");
                             using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                             {
                                 file.WriteLine("Option 1 received");
                             }
                             //string person = Console.ReadLine();
                             myclass.GetTeamsWindow(logfile);
                             myclass.SendCall(item_option.extra);
                             using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                             {
                                 file.WriteLine("Called {0}", item_option.extra);
                             }
                             in_call = true;


                            break;
                        case "2":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 2 received");
                            }
                            myclass.GetTeamsWindow(logfile);
                            myclass.PickUpCall();
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Picked up call!");
                            }
                            in_call = true;
                            break;
                        case "3":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 3 received");
                            }
                            //string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                            //string path = home + @"\Downloads\app";
                            myclass.GetTeamsWindow(logfile);
                            myclass.Screenshot(strWorkPath, count, width, height);
                            filename = "screenshot-" + count;
                           // myclass.startSession();
                            myclass.SendScreenshot(filename, strWorkPath, logfile);
                            count++;
                            break;
                     /*   case "4":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 4 received");
                            }
                            Console.WriteLine("What file do you want to send?");
                            // filename = Console.ReadLine();
                            //   myclass.startSession();
                            myclass.GetTeamsWindow(logfile);
                            myclass.SendScreenshot(item_option.extra, strWorkPath, logfile);
                            break;
                        case "5":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 5 received");
                            }
                            myclass.ShareScreen();
                            sharescreen = true;
                            in_call = true;
                            break;*/
                        case "4":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 6 received");
                            }
                            if (!winappdriver.WaitForExit(5000))
                            {
                                if (!winappdriver.HasExited)
                                {
                                    winappdriver.Kill();
                                }
                            }
                            exit = false;
                            break;
                        default:
                            Console.WriteLine("invalid input");
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("invalid input");
                            }
                            break;
                    }
                }
                else
                {
                    if (!sharescreen)
                    {
                        myclass.findCallSession(logfile);
                    }
                    Console.WriteLine("You are in a call. What would you like to do?");
                    Console.WriteLine("1) Play recording 2)Video on/off 3) Mic on/off 4)Stop share screen 5)Add person to call 6)Screenshot 7)Send file 8)Hang up 9)Exit call options");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                    {
                        file.WriteLine("You are in a call. What would you like to do?");
                        file.WriteLine("1) Play recording 2)Video on/off 3) Mic on/off 4)Stop share screen 5)Add person to call 6)Screenshot 7)Send file 8)Hang up 9)Exit call options");
                    }
                    item = myclass.downloadJSONdata("https://automationtest2.azurewebsites.net/api/Items");
                    command = false;
                    while (!command)
                    {
                        if (item != null)
                        {
                            foreach (UserInput option in item)
                            {
                                if (option.unique_id == identifier)
                                {
                                    if (option.count > counter || option.count == 0)
                                    {
                                        item_option = option;
                                        counter = option.count;
                                        command = true;
                                        break;
                                    }
                                }
                            }
                        }
                        Thread.Sleep(1000);
                       // Console.WriteLine("Checking for new data...");
                        item = myclass.downloadJSONdata("https://automationtest2.azurewebsites.net/api/Items");

                    }
                    switch (item_option.name)
                    {
                        case "1":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 1 received");
                            }

                            Console.WriteLine("Which option?");
                            serialPort.Open();
                            //song = Console.ReadLine();
                            if (item_option.extra == "1")
                            {
                                serialPort.Write("p");
                            }
                            if (item_option.extra == "2")
                            {
                                serialPort.Write("P");
                            }
                            if (item_option.extra == "3")
                            {
                                serialPort.Write(">");
                            }
                            if (item_option.extra == "4")
                            {
                                serialPort.Write("<");
                            }
                            if (item_option.extra == "5")
                            {
                                for (int x = 0; x < 3; x++)
                                {
                                    serialPort.Write("+");
                                }
                            }
                            if (item_option.extra == "6")
                            {
                                for (int x = 0; x < 3; x++)
                                {
                                    serialPort.Write("-");
                                }
                            }
                            serialPort.Close();
                            break;
                        case "2":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 2 received");
                            }
                            if (!sharescreen)
                            {
                                myclass.GetTeamsWindow(logfile);
                            }
                            /*else
                            {
                                SendKeys.SendWait("^+E");
                            }*/
                            myclass.ToggleVideo();
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Video on/off");
                            }

                            break;
                        case "3":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 3 received");
                            }
                            if (!sharescreen)
                            {
                                myclass.GetTeamsWindow(logfile);
                            }
                            /*else
                            {
                                SendKeys.SendWait("^+E");
                            }*/
                            myclass.ToggleMute();
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Mic on/off");
                            }
                            break;
                        case "4":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 4 received");
                            }
                            SendKeys.SendWait("^+E");
                            Thread.Sleep(1000);
                            SendKeys.SendWait("^+E");
                            Thread.Sleep(1000);
                            sharescreen = false;
                            break;
                        case "5":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 5 received");
                            }
                            // myclass.findCallSession();
                            if (!sharescreen)
                            {
                                myclass.AddPerson(item_option.extra);
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                                {
                                    file.WriteLine("Added {0} to the call", item_option.extra);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Cannot add person while screen sharing... Consider creating a group chat");
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                                {
                                    file.WriteLine("Option 5: Cannot add person while screen sharing... Consider creating a group chat");
                                }
                            }
                            break;
                        case "6":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 6 received");
                            }
                            //   myclass.startSession(in_call);
                            myclass.Screenshot(strWorkPath, count, width, height);
                            filename = "screenshot-" + count;
                            // myclass.startSession();
                            if (call_session != null)
                            {
                                myclass.SendFileInCall(filename, strWorkPath, logfile);
                            }
                            else
                            {
                                Console.WriteLine("Screenshot cannot be sent right now... Try restarting the call");
                            }
                            count++;
                            break;
                        case "7":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 7 received");
                            }
                            //  myclass.startSession(in_call);
                            Console.WriteLine("What file do you want to send?");
                            // filename = Console.ReadLine();
                            //   myclass.startSession();
                            if (call_session != null)
                            {
                                myclass.SendFileInCall(item_option.extra, strWorkPath, logfile);
                            }
                            else
                            {
                                Console.WriteLine("File cannot be sent right now... Try restarting the call");
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                                {
                                    file.WriteLine("Option 7: File cannot be sent right now... Try restarting the call");
                                }

                            }
                            break;
                        case "8":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 8 received");
                            }
                            if (!sharescreen)
                            {
                              //  myclass.GetTeamsWindow();
                                myclass.HangUp();
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                                {
                                    file.WriteLine("Call ended by tool");
                                }
                                in_call = false;
                                sharescreen = false;
                            }
                            else
                            {
                                SendKeys.SendWait("^+B");
                            }
                            break;
                        case "9":
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 9 received");
                            }
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
                            {
                                file.WriteLine("Option 9: User is not longer in a call.");
                            }
                            in_call = false;
                            break;
                    }

                }
            }

           serialPort.Close();

            Console.WriteLine("Exiting...");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logfile, true))
            {
                file.WriteLine(DateTime.Now.ToString("h:mm:ss tt")+"Exiting the program success\n");
            }

            ProcessStartInfo startInfo2 = new ProcessStartInfo();
            startInfo2.FileName = @"powershell.exe";
            startInfo2.Arguments = strWorkPath + @"\unschedule.ps1";
            startInfo2.RedirectStandardOutput = true;
            startInfo2.RedirectStandardError = true;
            startInfo2.UseShellExecute = false;
            startInfo2.CreateNoWindow = false;
            Process process2 = new Process();
            process2.StartInfo = startInfo;
            process2.Start();

        }

        public class UserInput
        {
            public long id { get; set; }
            public string name { get; set; }
            public string extra { get; set; }
            public int count { get; set; }
            public string unique_id { get; set; }
            public bool isComplete { get; set; }
        }


    }
}
