using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace TechnoService.Styles;

public static class BorderBrushes
{
    public static readonly LinearGradientBrush TextBoxDefaultBorderBrush = (LinearGradientBrush)new TextBox().BorderBrush;
    public static readonly SolidColorBrush TextBoxUncorrectBorderBrush = new() { Color = Colors.Red };
}
