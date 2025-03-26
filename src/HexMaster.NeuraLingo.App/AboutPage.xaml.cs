using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexMaster.NeuraLingo.App;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private void ImageButton_OnClicked(object? sender, EventArgs e)
    {
        // Open the browser to the NeuraLingo GitHub repository at https://github.com/nikneem/NeuraLingo
        var url = "https://github.com/nikneem/NeuraLingo";
        Launcher.OpenAsync(new Uri(url)).ConfigureAwait(false);
    }

}