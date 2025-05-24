using static System.Reflection.Metadata.BlobBuilder;

namespace SampleMaui.Pages;

public partial class LogsPage : ContentPage
{
    public LogsPage(Siprix.ObjModel om)
    {
        InitializeComponent();
        BindingContext = om.Logs;
    }
}