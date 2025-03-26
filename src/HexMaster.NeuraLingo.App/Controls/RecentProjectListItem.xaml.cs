namespace HexMaster.NeuraLingo.App.Controls;

public partial class RecentProjectListItem : ContentView
{
    public RecentProjectListItem()
    {
        InitializeComponent();
    }

    private void PointerGestureRecognizer_OnPointerEntered(object? sender, PointerEventArgs e)
    {
        if (sender is Border border)
        {
            border.Style = (Style)Resources["RecentProjectHover"];
        }
    }

    private void PointerGestureRecognizer_OnPointerExited(object? sender, PointerEventArgs e)
    {
        if (sender is Border border)
        {
            border.Style = (Style)Resources["RecentProject"];
        }
    }
}