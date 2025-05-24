#pragma warning disable CA1416
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace SampleMaui;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        HandleIntent(this.Intent);
    }

    protected override void OnNewIntent(Intent? intent)
    {
        base.OnNewIntent(intent);
        HandleIntent(intent);
    }

    private void HandleIntent(Intent? intent)
    {
        if (intent == null) return;
        
        var service = IPlatformApplication.Current?.Services.GetService<Siprix.ICallNotifService>();
        if (service is Platforms.Android.CallNotifService callNotifService)
            callNotifService?.HandleIncomingCallIntent(intent);    
    }

    public static async Task<bool> RequestPermissions()
    {
        try
        {
            //Microphone
            if (await Permissions.CheckStatusAsync<Permissions.Microphone>() != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.Microphone>();
            }

            //Notifications
            if (OperatingSystem.IsAndroidVersionAtLeast(33))
            {
                if (await Permissions.CheckStatusAsync<Permissions.PostNotifications>() != PermissionStatus.Granted)
                {
                    await Permissions.RequestAsync<Permissions.PostNotifications>();
                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}//MainActivity
