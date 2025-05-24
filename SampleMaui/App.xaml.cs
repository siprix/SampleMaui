namespace SampleMaui
{
    public partial class App : Application
    {
        public App(Siprix.ObjModel objModel, Siprix.ICallNotifService callNotifService)
        {
            InitializeComponent();

            MainPage = new AppShell();

            objModel.Initialize(MainPage.Dispatcher);
            callNotifService.Create(objModel.Core);
        }
    }
}
