using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using WindowsDisplayAPI;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GUI
{
    public partial class MainWindow
    {
        private string fileLoc = "screen.cfg";

        public MainWindow()
        {
            InitializeComponent();
            int screenNumber = 0;
            foreach (var display in Display.GetDisplays())
            {
                screenNumber++;
                if (screenNumber == 1)
                {
                    Resolution1.Text = display.CurrentSetting.Resolution.Width + "x" + display.CurrentSetting.Resolution.Height;
                    Offset1.Text = display.CurrentSetting.Position.X + "x" + display.CurrentSetting.Position.Y;
                    Hz1.Text = display.CurrentSetting.Frequency.ToString();
                }

                if (screenNumber == 2)
                {
                    Resolution2.Text = display.CurrentSetting.Resolution.Width + "x" + display.CurrentSetting.Resolution.Height;
                    Offset2.Text = display.CurrentSetting.Position.X + "x" + display.CurrentSetting.Position.Y;
                    Hz2.Text = display.CurrentSetting.Frequency.ToString();
                }

                if (screenNumber == 3)
                {
                    Resolution3.Text = display.CurrentSetting.Resolution.Width + "x" + display.CurrentSetting.Resolution.Height;
                    Offset3.Text = display.CurrentSetting.Position.X + "x" + display.CurrentSetting.Position.Y;
                    Hz3.Text = display.CurrentSetting.Frequency.ToString();
                }
            }

            LoadSettings();

            if (screenNumber == 1)
            {
                TabScreen2.IsEnabled = false;
                TabScreen3.IsEnabled = false;
            }

            if (screenNumber == 2)
            {
                TabScreen3.IsEnabled = false;
            }

            // MessageBox.Show(
            // "The filled in values are the current settings.\nSplit width and height with a small x!\nWidth comes before the x and after the x comes the height!\nIf the program crashes, you did something wrong!\nTo disable GUI-less mode, delete the screen.cfg file in this directory!",
            // "How to use?", MessageBoxButton.OK, MessageBoxImage.Information);

            CoreAudioController audioController = new CoreAudioController();
            var devices = audioController.GetCaptureDevices(DeviceState.Active);

            foreach (CoreAudioDevice device in devices)
            {
                if (device.IsDefaultDevice)
                {
                    labelMic.Content = device.Name;
                    device.SetVolumeAsync(82);
                }
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (SaveCbx.IsChecked.Value)
                SaveSettingsClick(null, null);
            foreach (var display in Display.GetDisplays())
            {
                if (display.DisplayName.Contains("3"))
                {
                    string[] r = Resolution3.Text.Split("x");
                    string[] o = Offset3.Text.Split("x");
                    var s = new DisplaySetting(new Size(Convert.ToInt32(r[0]), Convert.ToInt32(r[1])), new Point(Convert.ToInt32(o[0]), Convert.ToInt32(o[1])), Convert.ToInt32(Hz3.Text));
                    display.SetSettings(s, true);
                }
                else if (display.DisplayName.Contains("2"))
                {
                    string[] r = Resolution2.Text.Split("x");
                    string[] o = Offset2.Text.Split("x");
                    var s = new DisplaySetting(new Size(Convert.ToInt32(r[0]), Convert.ToInt32(r[1])), new Point(Convert.ToInt32(o[0]), Convert.ToInt32(o[1])), Convert.ToInt32(Hz2.Text));
                    display.SetSettings(s, true);
                }
                else if (display.DisplayName.Contains("1"))
                {
                    string[] r = Resolution1.Text.Split("x");
                    string[] o = Offset1.Text.Split("x");
                    var s = new DisplaySetting(new Size(Convert.ToInt32(r[0]), Convert.ToInt32(r[1])), new Point(Convert.ToInt32(o[0]), Convert.ToInt32(o[1])), Convert.ToInt32(Hz1.Text));
                    display.SetSettings(s, true);
                }
            }
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            if (c.IsChecked.Value)
            {
                Guiless.IsEnabled = true;
            }
            else
            {
                Guiless.IsChecked = false;
                Guiless.IsEnabled = false;
            }
        }

        private void SaveSettingsClick(object sender, RoutedEventArgs e)
        {
            string[] lines =
            {
                SaveCbx.IsChecked.Value.ToString(), Guiless.IsChecked.Value.ToString(),
                Resolution1.Text, Offset1.Text, Hz1.Text,
                Resolution2.Text, Offset2.Text, Hz2.Text,
                Resolution3.Text, Offset3.Text, Hz3.Text
            };

            File.WriteAllLines(fileLoc, lines);
        }

        void LoadSettings()
        {
            if (File.Exists(fileLoc))
            {
                var lines = File.ReadAllLines(fileLoc);
                if (!Convert.ToBoolean(lines[0]))
                    return;
                Resolution1.Text = lines[2];
                Offset1.Text = lines[3];
                Hz1.Text = lines[4];
                Resolution2.Text = lines[5];
                Offset2.Text = lines[6];
                Hz2.Text = lines[7];
                Resolution3.Text = lines[8];
                Offset3.Text = lines[9];
                Hz3.Text = lines[10];
                if (Convert.ToBoolean(lines[1]))
                {
                    Guiless.IsChecked = true;
                    ButtonBase_OnClick(null, null);
                    Environment.Exit(0);
                }
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.youtube.com/channel/UCg3F3ebXqoW27WVZdKSGdCw",
                UseShellExecute = true
            });
            Tabs.SelectedIndex = 0;
        }

        private void CrashHelper_Click(object sender, RoutedEventArgs e)
        {
            Popup p = new Popup();
            p.ShowDialog();
            return;
        }
    }
}