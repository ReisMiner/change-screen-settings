using System;
using System.Windows;
using WindowsDisplayAPI;

namespace GUI;

public partial class Popup
{
    public Popup()
    {
        string x = "";
        InitializeComponent();
        int c = 1;
        foreach (var display in Display.GetDisplays())
        {
            x += "====================================\nSCREEN " + c + "\n====================================\n";
            foreach (DisplayPossibleSetting displayPossibleSetting in display.GetPossibleSettings())
            {
                x += displayPossibleSetting;
                x += "\n";
            }

            c++;
        }

        Block.Text = x;
    }
}