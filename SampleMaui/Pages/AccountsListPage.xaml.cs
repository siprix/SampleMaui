using System.ComponentModel;

namespace SampleMaui.Pages;
public partial class AccountsListPage : ContentPage
{
    readonly Siprix.ObjModel objModel_;
    public AccountsListPage(Siprix.ObjModel om)
    {
        InitializeComponent();
        objModel_ = om;
        objModel_.Calls.PropertyChanged += OnCallsList_PropertyChanged;
        this.BindingContext = objModel_.Accounts;        
        tbNetworkLost.BindingContext = objModel_.Networks;

        //Display 'Foreground mode' switch only for Android
        slForegroundMode.IsVisible = (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.Android);

        if (Application.Current != null)
            ToggleThemeButton(Application.Current.RequestedTheme == AppTheme.Light);
    }

    private void OnCallsList_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Siprix.CallsListModel.LastIncomingCallId))
        {
            //Switch to 'Calls' page when incoming call received
            if (App.Current?.MainPage is AppShell appShell)
                appShell.SetCurrentCallsTab();
        }
    }

    async private void AccountAdd_Click(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AccountAddPage(objModel_));
    }

    async private void AccountEdit_Click(object sender, EventArgs e)
    {
        Button? editBtn = sender as Button;
        if (editBtn?.BindingContext is Siprix.AccountModel acc)
            await Navigation.PushModalAsync(new AccountAddPage(objModel_, acc.AccData));
    }

    async private void AccountDelete_Click(object sender, EventArgs e)
    {
        //Get selected
        Button? editBtn = sender as Button;
        if (editBtn?.BindingContext is not Siprix.AccountModel acc) return;

        //Confirm deleting
        bool answerYes = await DisplayAlert("Delete account", "Confirm deleting account?", "Yes", "No");
        if (!answerYes) return;

        //Delete
        int err = objModel_.Accounts.Delete(acc);
        if (err != Siprix.ErrorCode.kNoErr)
        {
            await DisplayAlert("Alert", objModel_.ErrorText(err), "OK");
        }
    }

    private void ButtonMenu_Click(object sender, EventArgs e)
    {
        Button? menuBtn = sender as Button;
        if (menuBtn?.Parent is not HorizontalStackLayout parentLayout) return;
        foreach (var ctrl in parentLayout.Children)
        {
            if (ctrl is HorizontalStackLayout menuLayout)
                menuLayout.IsVisible = !menuLayout.IsVisible; 
        }
    }

    async void OnTapGestureRecognizerTapped(object sender, TappedEventArgs e)
    {
        if((e.Parameter is string url)&&(!string.IsNullOrEmpty(url)))
                await Launcher.OpenAsync(url);
    }

    void StartForegroundMode_Click(object sender, EventArgs e)
    {
        var service = IPlatformApplication.Current?.Services.GetService<Siprix.ICallNotifService>();
        service?.ToggleForegroundMode();
    }

    void SwitchTheme_Click(object sender, EventArgs e)
    {
        if (Application.Current != null)
        {
            AppTheme curTheme = Application.Current.UserAppTheme;
            if (curTheme == AppTheme.Unspecified) 
                curTheme = Application.Current.RequestedTheme;
            
            bool themeIsLight = (curTheme == AppTheme.Light);
            Application.Current.UserAppTheme = themeIsLight ? AppTheme.Dark : AppTheme.Light;
            ToggleThemeButton(!themeIsLight);
        }
    }

    void ToggleThemeButton(bool themeIsLight)
    {
        lblSwitchTheme.Text = themeIsLight ? Icons.dark_mode : Icons.light_mode;
        lblSwitchTheme.TextColor = themeIsLight ? Colors.White : Colors.Black;
        borderSwitchTheme.BackgroundColor = themeIsLight ? Colors.Black : Colors.White;
    }
}