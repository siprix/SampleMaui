namespace SampleMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        public void SetCurrentCallsTab()
        {
            ShellTabBar.CurrentItem = CallsPage;
        }
    }
}
