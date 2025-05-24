using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;

namespace SampleMaui.Platforms.Android;

[Service]
public class CallNotifService : Service, Siprix.ICallNotifService
{
    private const int kForegroundId = 777;
    private const int kCallBaseNotifId = 555;

    private const string TAG = "CallNotifService";

    private const string kExtraCallId = "kExtraCallId";
    private const string kExtraAccId = "kExtraAccId";
    private const string kExtraWithVideo = "kExtraWithVideo";
    private const string kExtraHdrFrom = "kExtraHdrFrom";
    private const string kExtraHdrTo = "kExtraHdrTo";

    private const string kActionForeground = "kActionForeground";
    private const string kActionIncomingCall = "kActionIncomingCall";
    private const string kActionIncomingCallAccept = "kActionIncomingCallAccept";
    private const string kActionIncomingCallReject = "kActionIncomingCallReject";
    private const string kActionIncomingCallStopRinger = "kActionIncomingCallStopRinger";
    private const string kCallChannelId = "kSiprixMauiCallChannelId";

    private const string kForegroundDescr = "Siprix call notification service";
    private const string kChannelDescr = "Incoming calls notifications channel";
    private const string kContentLabel = "Incoming call";
    private const string kRejectBtnLabel = "Reject call";
    private const string kAcceptBtnLabel = "Accept call";


    ServiceListenter? serviceListenrer_;
    Com.Siprix.SiprixRinger? ringer_;
    bool foregroundModeStarted_ = false;
    int requestCode_ = 1;

    public async void Create(Siprix.ICoreService coreService)
    {
        var activity = await Platform.WaitForActivityAsync();
        ringer_ = new(activity);

        CreateNotifChannel();

        if (coreService is Siprix.CoreService androidCoreService)
        {
            serviceListenrer_ = new(this);
            androidCoreService.SetServiceListener(serviceListenrer_);
        }
    }

    static private void CreateNotifChannel()
    {
#pragma warning disable CA1416
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            NotificationChannel channel = new(kCallChannelId, AppInfo.Current.Name, NotificationImportance.High);
            channel.Description = kChannelDescr;

            GetNotifMgr()?.CreateNotificationChannel(channel);
        }
    }

    public override IBinder OnBind(Intent? intent) { throw new NotImplementedException(); }

    public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
    {
        if (intent != null)
        {
            if (intent.Action == kActionForeground)
                HandleToggleForegroundMode();
            else
                HandleIncomingCallIntent(intent);
        }

        return StartCommandResult.NotSticky;
    }

    public void HandleIncomingCallIntent(Intent intent)
    {
        int callId = intent.Extras?.GetInt(kExtraCallId, 0) ?? 0;
        if (callId <= 0) return;

        if (kActionIncomingCallAccept == intent.Action)
        {
            GetCoreService()?.Call_Accept((uint)callId, true);
        }
        else if (kActionIncomingCallReject == intent.Action)
        {
            GetCoreService()?.Call_Reject((uint)callId);
        }
        CancelNotification(callId);
    }

    public void ToggleForegroundMode()
    {
        Intent srvIntent = new(Platform.AppContext, typeof(CallNotifService));
        srvIntent.SetAction(kActionForeground);
        Platform.AppContext.StartService(srvIntent);
    }

    private void HandleToggleForegroundMode()
    {
        if (foregroundModeStarted_)
        {
#pragma warning disable CA1422
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
                StopForeground(StopForegroundFlags.Remove);
            else
                StopForeground(true);
        }
        else
        {
            var contentIntent = GetIntentActivity(kActionForeground, new Bundle());
            NotificationCompat.Builder builder = new NotificationCompat.Builder(Platform.AppContext, kCallChannelId)
                .SetSmallIcon(Resource.Drawable.ic_notif_icon)
                .SetContentTitle(AppInfo.Current.Name)
                .SetContentText(kForegroundDescr)
                .SetContentIntent(contentIntent);

            //if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            //StartForeground(kForegroundId, builder.Build(), ForegroundService.TypePhoneCall);
            //else
            StartForeground(kForegroundId, builder.Build());
        }

        foregroundModeStarted_ = !foregroundModeStarted_;
    }

    private bool IsAppInForeground()
    {
        ActivityManager? activityMgr = (ActivityManager?)Platform.AppContext.GetSystemService(ActivityService);
        var appProcs = activityMgr?.RunningAppProcesses;
        if (appProcs == null) return false;

        foreach (var app in appProcs) {
            if (app.Importance == Importance.Foreground) {
                if ((app.PkgList != null) &&
                    app.PkgList.Contains(AppInfo.Current.PackageName)) return true;
            }
        }
        return false;
    }

    private void CancelNotification(int callId)
    {
        GetNotifMgr()?.Cancel(GetNotifId(callId));
    }

    static private int GetNotifId(int callId)
    {
        return kCallBaseNotifId + callId;
    }

    static NotificationManager? GetNotifMgr()
    {
        return Platform.AppContext.GetSystemService(NotificationService) as NotificationManager;
    }

    static Siprix.ICoreService? GetCoreService()
    {
        return IPlatformApplication.Current?.Services.GetService<Siprix.ICoreService>();
    }

    private Bundle BuildBundle(int callId, int accId,
                 bool withVideo, string? hdrFrom, string? hdrTo)
    {
        Bundle bundle = new();
        bundle.PutInt(kExtraCallId, callId);
        bundle.PutInt(kExtraAccId, accId);
        bundle.PutBoolean(kExtraWithVideo, withVideo);
        bundle.PutString(kExtraHdrFrom, hdrFrom);
        bundle.PutString(kExtraHdrTo, hdrTo);
        return bundle;
    }

    private PendingIntent? GetIntentActivity(string action, Bundle bundle)
    {
        Intent activityIntent = new(Platform.AppContext, typeof(MainActivity));
        activityIntent.SetAction(action);
        activityIntent.AddFlags(ActivityFlags.NewTask);
        activityIntent.PutExtras(bundle);
        activityIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);

        return PendingIntent.GetActivity(Platform.AppContext,
            requestCode_++, activityIntent, getPendingIntentFlags());
    }

    private PendingIntent? GetIntentService(string action, Bundle bundle)
    {
        Intent srvIntent = new(Platform.AppContext, typeof(CallNotifService));
        srvIntent.SetAction(action);
        srvIntent.PutExtras(bundle);

        return PendingIntent.GetService(Platform.AppContext,
                 requestCode_++, srvIntent, getPendingIntentFlags());
    }

    PendingIntentFlags getPendingIntentFlags()
    {
        return (Build.VERSION.SdkInt >= BuildVersionCodes.S)
                ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
                : PendingIntentFlags.UpdateCurrent;
    }

    protected string parseDisplayName(string? hdrFrom)
    {
        //uri format: "DisplName" <sip:ext@domain:port>
        if (hdrFrom == null) return "?";
        var startIndex = hdrFrom.IndexOf('"');
        var endIndex = (startIndex == -1) ? -1 : hdrFrom.IndexOf('"', startIndex + 1);
        return (endIndex == -1) ? "?" : hdrFrom.Substring(startIndex + 1, endIndex - startIndex - 1);
    }

    protected string parseExt(string hdrFrom)
    {
        //uri format: "displName" <sip:EXT@domain:port>
        if (hdrFrom == null) return "?";
        var startIndex = hdrFrom.IndexOf(':');
        var endIndex = (startIndex == -1) ? -1 : hdrFrom.IndexOf('@', startIndex + 1);
        return (endIndex == -1) ? "?" : hdrFrom.Substring(startIndex + 1, endIndex - startIndex - 1);
    }

    protected string BuildContentString(string? hdrFrom)
    {
        if (hdrFrom == null) return "???";

        //Return string same as uses Flutter app in 'CallModel.nameAndExt'
        var displName = parseDisplayName(hdrFrom);
        var sipExt = parseExt(hdrFrom);
        return displName.Length==0 ? sipExt : $"{displName} ({sipExt})";
    }

    private void DisplayIncomingCallNotification(int callId, int accId,
                    bool withVideo, string? hdrFrom, string? hdrTo)
    {
        Log.Debug(TAG, $"displayIncomingCallNotification {callId}");
        var bundle = BuildBundle(callId, accId, withVideo, hdrFrom, hdrTo);

        var contentIntent = GetIntentActivity(kActionIncomingCall, bundle);
        var pendingAcceptCall = GetIntentActivity(kActionIncomingCallAccept, bundle);
        var pendingRejectCall = GetIntentService(kActionIncomingCallReject, bundle);
        var contentStr = BuildContentString(hdrFrom);

        NotificationCompat.Builder builder = new NotificationCompat.Builder(Platform.AppContext, kCallChannelId)
            .SetSmallIcon(Resource.Drawable.ic_notif_icon)
            .SetContentTitle(kContentLabel)
            .SetContentText(contentStr)
            .SetAutoCancel(true)
            .SetContentIntent(contentIntent)
            .SetFullScreenIntent(contentIntent, true)
            .SetOngoing(true)
            .AddAction(0, kRejectBtnLabel, pendingRejectCall)
            .AddAction(0, kAcceptBtnLabel, pendingAcceptCall)
            .SetDeleteIntent(GetIntentService(kActionIncomingCallStopRinger, bundle))
            .SetCategory(NotificationCompat.CategoryCall);

        GetNotifMgr()?.Notify(GetNotifId(callId), builder.Build());
    }

    private class ServiceListenter(CallNotifService service_) : Java.Lang.Object, Com.Siprix.ISiprixServiceListener
    {
        public void OnCallIncoming2(int callId, int accId, bool withVideo, string? hdrFrom, string? hdrTo)
        {
            if (!service_.IsAppInForeground())
            {
                service_.DisplayIncomingCallNotification(callId, accId, withVideo, hdrFrom, hdrTo);
            }
        }

        public void OnCallTerminated2(int callId, int statusCode)
        {
            service_.CancelNotification(callId);
        }

        public void OnRingerState(bool start)
        {
            if (start) service_.ringer_!.Start();
            else       service_.ringer_!.Stop();
        }
    }
}
