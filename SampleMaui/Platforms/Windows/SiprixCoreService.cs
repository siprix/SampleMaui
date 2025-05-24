#pragma warning disable 1998, CA1806, SYSLIB1054, CA2101, IDE1006
using System.Runtime.InteropServices;
using System.Text;

namespace Siprix;

public class CoreService : ICoreService, IDisposable
{
    IntPtr modulePtr_;
    IEventDelegate? eventDelegate_;
    const string DllName = "siprix.dll";

    private readonly OnTrialModeNotified   onTrialModeNotified_;
    private readonly OnDevicesAudioChanged onDevicesAudioChanged_;

    private readonly OnAccountRegState     onAccountRegState_;
    private readonly OnSubscriptionState   onSubscriptionState_;
    private readonly OnNetworkState        onNetworkState_;
    private readonly OnPlayerState         onPlayerState_;
    private readonly OnRingerState         onRingerState_;

    private readonly OnCallIncoming        onCallIncoming_;
    private readonly OnCallConnected       onCallConnected_;
    private readonly OnCallTerminated      onCallTerminated_;
    private readonly OnCallProceeding      onCallProceeding_;
    private readonly OnCallTransferred     onCallTransferred_;
    private readonly OnCallRedirected      onCallRedirected_;
    private readonly OnCallDtmfReceived    onCallDtmfReceived_;
    private readonly OnCallHeld            onCallHeld_;
    private readonly OnCallSwitched        onCallSwitched_;

    private readonly OnMessageSentState    onMessageSentState_;
    private readonly OnMessageIncoming     onMessageIncoming_;

    public CoreService()
    {
        onTrialModeNotified_   = new OnTrialModeNotified     (OnTrialModeNotifiedCallback);
        onDevicesAudioChanged_ = new OnDevicesAudioChanged   (OnDevicesAudioChangedCallback);

        onAccountRegState_     = new OnAccountRegState       (OnAccountRegStateCallback);
        onSubscriptionState_   = new OnSubscriptionState     (OnSubscriptionStateCallback);
        onNetworkState_        = new OnNetworkState          (OnNetworkStateCallback);
        onPlayerState_         = new OnPlayerState           (OnPlayerStateCallback);
        onRingerState_         = new OnRingerState           (OnRingerStateCallback);
                                
        onCallIncoming_        = new OnCallIncoming          (OnCallIncomingCallback);
        onCallConnected_       = new OnCallConnected         (OnCallConnectedCallback);
        onCallTerminated_      = new OnCallTerminated        (OnCallTerminatedCallback);
        onCallProceeding_      = new OnCallProceeding        (OnCallProceedingCallback);
        onCallTransferred_     = new OnCallTransferred       (OnCallTransferredCallback);
        onCallRedirected_      = new OnCallRedirected        (OnCallRedirectedCallback);
        onCallDtmfReceived_    = new OnCallDtmfReceived      (OnCallDtmfReceivedCallback);
        onCallHeld_            = new OnCallHeld              (OnCallHeldCallback);
        onCallSwitched_        = new OnCallSwitched          (OnCallSwitchedCallback);

        onMessageSentState_   = new OnMessageSentState       (OnMessageSentStateCallback);
        onMessageIncoming_    = new OnMessageIncoming        (OnMessageIncomingCallback);
    }

    ~CoreService()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {            
        }
                    
        Module_UnInitialize(modulePtr_);
        modulePtr_ = IntPtr.Zero;
    }


    //---------------------------------------------
    //Module

    public async Task<int> Initialize(IEventDelegate eventDelegate, IniData iniData)
    {
        if(modulePtr_ == IntPtr.Zero)
            modulePtr_ = Module_Create();

        int err = Module_Initialize(modulePtr_, getNative(iniData));
        if(err != ErrorCode.kNoErr) return err;
        
        eventDelegate_ = eventDelegate;

        Callback_SetTrialModeNotified(modulePtr_,    onTrialModeNotified_);
        Callback_SetDevicesAudioChanged(modulePtr_,  onDevicesAudioChanged_);

        Callback_SetAccountRegState(modulePtr_,      onAccountRegState_);
        Callback_SetSubscriptionState(modulePtr_,    onSubscriptionState_);
        Callback_SetNetworkState(modulePtr_,         onNetworkState_);
        Callback_SetPlayerState(modulePtr_,          onPlayerState_);
        Callback_SetRingerState(modulePtr_,          onRingerState_);

        Callback_SetCallProceeding(modulePtr_,       onCallProceeding_);
        Callback_SetCallTerminated(modulePtr_,       onCallTerminated_);
        Callback_SetCallConnected(modulePtr_,        onCallConnected_);
        Callback_SetCallIncoming(modulePtr_,         onCallIncoming_);
        Callback_SetCallDtmfReceived(modulePtr_,     onCallDtmfReceived_);
        Callback_SetCallTransferred(modulePtr_,      onCallTransferred_);
        Callback_SetCallRedirected(modulePtr_,       onCallRedirected_);
        Callback_SetCallSwitched(modulePtr_,         onCallSwitched_);
        Callback_SetCallHeld(modulePtr_,             onCallHeld_);

        Callback_SetMessageSentState(modulePtr_,     onMessageSentState_);
        Callback_SetMessageIncoming(modulePtr_,      onMessageIncoming_);
        return err;
    }

    public int UnInitialize()
    {
        return Module_UnInitialize(modulePtr_);
    }

    public bool IsInitialized()
    {
        return (modulePtr_ != IntPtr.Zero) && Module_IsInitialized(modulePtr_);
    }

    public string HomeFolder()
    {
        IntPtr strPtr = (modulePtr_ != IntPtr.Zero) ? Module_HomeFolder(modulePtr_) : 0;
        string? path = Marshal.PtrToStringUTF8(strPtr);
        return path ?? "";
    }

    public string Version()
    {
        IntPtr strPtr = (modulePtr_ != IntPtr.Zero) ? Module_Version(modulePtr_) : 0;
        string? ver = Marshal.PtrToStringUTF8(strPtr);
        return ver ?? "";
    }

    public uint VersionCode()
    {
        return (modulePtr_ != IntPtr.Zero) ? Module_VersionCode(modulePtr_) : 0;
    }

    public string ErrorText(int code)
    {
        IntPtr strPtr = GetErrorText(code);
        string? ver = Marshal.PtrToStringUTF8(strPtr);
        return ver ?? "";
    }

    /// [Account] ///////////////////////////////////////////////////////////////////////////////////////////////
    public int Account_Add(AccData accData)
    {
        return Account_Add(modulePtr_, getNative(accData), ref accData.MyAccId);
    }

    public int Account_Update(AccData accData, uint accId)
    {
        return Account_Update(modulePtr_, getNative(accData), accId);
    }

    public int Account_GetRegState(uint accId, ref RegState state)
    {
        return Account_GetRegState(modulePtr_, accId, ref state);
    }

    public int Account_Register(uint accId, uint expireTime)
    {
        return Account_Register(modulePtr_, accId, expireTime);
    }

    public int Account_Unregister(uint accId)
    {
        return Account_Unregister(modulePtr_, accId);
    }

    public int Account_Delete(uint accId)
    {
        return Account_Delete(modulePtr_, accId);
    }


    /// [Calls] ///////////////////////////////////////////////////////////////////////////////////////////////

    public int Call_Invite(DestData dest)
    {
        return Call_Invite(modulePtr_, getNative(dest), ref dest.MyCallId);
    }

    public int Call_Reject(uint callId, uint statusCode=486)
    {
        return Call_Reject(modulePtr_, callId, statusCode);
    }

    public int Call_Accept(uint callId, bool withVideo)
    {
        return Call_Accept(modulePtr_, callId, withVideo);
    }

    public int Call_Hold(uint callId)
    {
        return Call_Hold(modulePtr_, callId);
    }

    public int Call_GetHoldState(uint callId, ref HoldState state)
    {
        return Call_GetHoldState(modulePtr_, callId, ref state);
    }

    public string Call_GetSipHeader(uint callId, string hdrName)
    {
        uint hdrValLen = 0;
        Call_GetSipHeader(modulePtr_, callId, hdrName, null, ref hdrValLen);
        if (hdrValLen > 0)
        {
            var sb = new StringBuilder((int)(hdrValLen+1));
            Call_GetSipHeader(modulePtr_, callId, hdrName, sb, ref hdrValLen);
            return sb.ToString();
        }
        else return string.Empty;
    }

    public string Call_GetNonce(uint callId)
    {
        uint nonceValLen = 0;
        Call_GetNonce(modulePtr_, callId, null, ref nonceValLen);
        if (nonceValLen > 0)
        {
            var sb = new StringBuilder((int)(nonceValLen+1));
            Call_GetNonce(modulePtr_, callId, sb, ref nonceValLen);
            return sb.ToString();
        }
        else return string.Empty;
    }


    public int Call_MuteMic(uint callId, bool mute)
    {
        return Call_MuteMic(modulePtr_, callId, mute);
    }

    public int Call_MuteCam(uint callId, bool mute)
    {
        return Call_MuteCam(modulePtr_, callId, mute);
    }        

    public int Call_SendDtmf(uint callId, string dtmfs, 
        Int16 durationMs, Int16 intertoneGapMs, DtmfMethod method)
    {
        return Call_SendDtmf(modulePtr_, callId, dtmfs, durationMs, intertoneGapMs, method);
    }

    public int Call_PlayFile(uint callId, string pathToMp3File, bool loop, ref uint playerId)
    {
        return Call_PlayFile(modulePtr_, callId, pathToMp3File, loop, ref playerId);
    }

    public int Call_StopFile(uint playerId)
    {
        return Call_StopFile(modulePtr_, playerId);
    }

    public int Call_RecordFile(uint callId, string pathToMp3File)
    {
        return Call_RecordFile(modulePtr_, callId, pathToMp3File);
    }

    public int Call_StopRecordFile(uint callId)
    {
        return Call_StopRecordFile(modulePtr_, callId);
    }

    public int Call_TransferBlind(uint callId, string toExt)
    {
        return Call_TransferBlind(modulePtr_, callId, toExt);
    }

    public int Call_TransferAttended(uint fromCallId, uint toCallId)
    {
        return Call_TransferAttended(modulePtr_, fromCallId, toCallId);
    }

    public int Call_SetVideoWindow(uint callId, IntPtr hwnd)
    {
        return Call_SetVideoWindow(modulePtr_, callId, hwnd);
    }

    public int Call_Bye(uint callId)
    {
        return Call_Bye(modulePtr_, callId);
    }

    public int Call_Renegotiate(uint callId)
    {
        return Call_Renegotiate(modulePtr_, callId);
    }

    /// [Mixer] ///////////////////////////////////////////////////////////////////////////////////////////////
    public int Mixer_SwitchToCall(uint callId)
    {
        return Mixer_SwitchToCall(modulePtr_, callId);
    }

    public int Mixer_MakeConference()
    {
        return Mixer_MakeConference(modulePtr_);
    }

    /// [Messages] ///////////////////////////////////////////////////////////////////////////////////////////////
    public int Message_Send(MsgData msgData)
    {
        return Message_Send(modulePtr_, getNative(msgData), ref msgData.MyMsgId);
    }

    /// [Subscriptions] ///////////////////////////////////////////////////////////////////////////////////////////////
    public int Subscription_Add(SubscrData subData)
    {
        return Subscription_Create(modulePtr_, getNative(subData), ref subData.MySubId);
    }

    public int Subscription_Delete(uint subId)
    {
        return Subscription_Destroy(modulePtr_, subId);
    }


    /// [Module] ///////////////////////////////////////////////////////////////////////////////////////////////
    [DllImport(DllName)]
    private static extern IntPtr Module_Create();
    [DllImport(DllName)]
    private static extern int Module_Initialize(IntPtr modulePtr, IntPtr iniDataPtr);
    [DllImport(DllName)]
    private static extern int Module_UnInitialize(IntPtr modulePtr);
    [DllImport(DllName)]
    private static extern bool Module_IsInitialized(IntPtr modulePtr);
    [DllImport(DllName)]        
    private static extern IntPtr Module_HomeFolder(IntPtr modulePtr);
    [DllImport(DllName)]
    private static extern IntPtr Module_Version(IntPtr modulePtr);
    [DllImport(DllName)]
    private static extern uint Module_VersionCode(IntPtr modulePtr);
    [DllImport(DllName)]
    private static extern IntPtr GetErrorText(int code);


    /// [Ini] ///////////////////////////////////////////////////////////////////////////////////////////////
    [DllImport(DllName)]
    private static extern IntPtr Ini_GetDefault();
    [DllImport(DllName)]
    private static extern void Ini_SetLicense(IntPtr ini, [MarshalAs(UnmanagedType.LPUTF8Str)] string license);
    [DllImport(DllName)]
    private static extern void Ini_SetLogLevelFile(IntPtr ini, LogLevel logLevel);
    [DllImport(DllName)]
    private static extern void Ini_SetLogLevelIde(IntPtr ini, LogLevel logLevel);
    [DllImport(DllName)]
    private static extern void Ini_SetShareUdpTransport(IntPtr ini, bool shareUdpTransport);
    [DllImport(DllName)]
    private static extern void Ini_SetAllocStrArg(IntPtr ini, bool callbackAllocStringArgs);
    [DllImport(DllName)]
    private static extern void Ini_SetUseExternalRinger(IntPtr ini, bool useExternalRinger);
    [DllImport(DllName)]
    private static extern void Ini_SetDmpOnUnhandledExc(IntPtr ini, bool writeDmpUnhandledExc);
    [DllImport(DllName)]
    private static extern void Ini_SetTlsVerifyServer(IntPtr ini, bool tlsVerifyServer);
    [DllImport(DllName)]
    private static extern void Ini_SetSingleCallMode(IntPtr ini, bool singleCallMode);
    [DllImport(DllName)]
    private static extern void Ini_SetRtpStartPort(IntPtr ini, ushort rtpStartPort);
    [DllImport(DllName)]
    private static extern void Ini_SetHomeFolder(IntPtr ini, [MarshalAs(UnmanagedType.LPUTF8Str)] string homeFolder);
    [DllImport(DllName)]
    private static extern void Ini_SetBrandName(IntPtr ini, [MarshalAs(UnmanagedType.LPUTF8Str)] string brandName);
    [DllImport(DllName)]
    private static extern void Ini_AddDnsServer(IntPtr ini, [MarshalAs(UnmanagedType.LPUTF8Str)]  string dns);
    [DllImport(DllName)]
    private static extern void Ini_SetUseDnsSrv(IntPtr ini, bool enabled);
    [DllImport(DllName)]
    private static extern void Ini_SetRecordStereo(IntPtr ini, bool enabled);

    private static IntPtr getNative(IniData iniData)
    {
        IntPtr ptr = Ini_GetDefault();
        if (iniData.License              != null) Ini_SetLicense(ptr,           iniData.License);
        if (iniData.LogLevelFile         != null) Ini_SetLogLevelFile(ptr,      iniData.LogLevelFile.Value);
        if (iniData.LogLevelIde          != null) Ini_SetLogLevelIde(ptr,       iniData.LogLevelIde.Value);
        if (iniData.ShareUdpTransport    != null) Ini_SetShareUdpTransport(ptr, iniData.ShareUdpTransport.Value);
        if (iniData.WriteDmpUnhandledExc != null) Ini_SetDmpOnUnhandledExc(ptr, iniData.WriteDmpUnhandledExc.Value);
        if (iniData.SingleCallMode       != null) Ini_SetSingleCallMode(ptr,    iniData.SingleCallMode.Value);
        if (iniData.RtpStartPort         != null) Ini_SetRtpStartPort(ptr,      iniData.RtpStartPort.Value);
        if (iniData.HomeFolder           != null) Ini_SetHomeFolder(ptr,        iniData.HomeFolder);
        if (iniData.BrandName            != null) Ini_SetBrandName(ptr,         iniData.BrandName);
        if (iniData.UseDnsSrv            != null) Ini_SetUseDnsSrv(ptr,         iniData.UseDnsSrv.Value);
        if (iniData.RecordStereo         != null) Ini_SetRecordStereo(ptr,      iniData.RecordStereo.Value);

        if (iniData.DnsServers != null)
        {
            foreach (var dns in iniData.DnsServers) 
                Ini_AddDnsServer(ptr, dns);
        }
        return ptr;
    }

    /// [Acc] ///////////////////////////////////////////////////////////////////////////////////////////////
    [DllImport(DllName)]
    private static extern IntPtr Acc_GetDefault();
    [DllImport(DllName)]
    private static extern void Acc_SetSipServer(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string sipServer);
    [DllImport(DllName)]
    private static extern void Acc_SetSipExtension(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string sipExtension);
    [DllImport(DllName)]
    private static extern void Acc_SetSipAuthId(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string sipAuthId);
    [DllImport(DllName)]
    private static extern void Acc_SetSipPassword(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string sipPassword);
    [DllImport(DllName)]
    private static extern void Acc_SetExpireTime(IntPtr acc, uint expireTime);
    [DllImport(DllName)]
    private static extern void Acc_SetSipProxyServer(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string sipProxyServer);
    
    [DllImport(DllName)]        
    private static extern void Acc_SetStunServer(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string stunServer);
    [DllImport(DllName)]
    private static extern void Acc_SetTurnServer(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string turnServer);
    [DllImport(DllName)]
    private static extern void Acc_SetTurnUser(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string turnUser);
    [DllImport(DllName)]
    private static extern void Acc_SetTurnPassword(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string turnPassword);

    [DllImport(DllName)]
    private static extern void Acc_SetUserAgent(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string userAgent);
    [DllImport(DllName)]
    private static extern void Acc_SetDisplayName(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string displayName);
    [DllImport(DllName)]
    private static extern void Acc_SetInstanceId(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string instanceId);
    [DllImport(DllName)]
    private static extern void Acc_SetRingToneFile(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string ringTonePath);        

    [DllImport(DllName)]
    private static extern void Acc_SetSecureMediaMode(IntPtr acc, SecureMedia mode);
    [DllImport(DllName)]
    private static extern void Acc_SetUseSipSchemeForTls(IntPtr acc, bool useSipSchemeForTls);
    [DllImport(DllName)]
    private static extern void Acc_SetRtcpMuxEnabled(IntPtr acc, bool rtcpMuxEnabled);
    [DllImport(DllName)]
    private static extern void Acc_SetIceEnabled(IntPtr acc, bool iceEnabled);

    [DllImport(DllName)]
    private static extern void Acc_SetKeepAliveTime(IntPtr acc, uint keepAliveTimeSec);
    [DllImport(DllName)]
    private static extern void Acc_SetTranspProtocol(IntPtr acc, SipTransport transp);
    [DllImport(DllName)]
    private static extern void Acc_SetTranspPort(IntPtr acc, ushort transpPort);
    [DllImport(DllName)]
    private static extern void Acc_SetTranspTlsCaCert(IntPtr acc, 
                                [MarshalAs(UnmanagedType.LPUTF8Str)] string pathToCaCertPem);
    [DllImport(DllName)]
    private static extern void Acc_SetTranspBindAddr(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string ipAddr);
    [DllImport(DllName)]
    private static extern void Acc_SetTranspPreferIPv6(IntPtr acc, bool prefer);

    [DllImport(DllName)]
    private static extern void Acc_AddXHeader(IntPtr acc, [MarshalAs(UnmanagedType.LPUTF8Str)] string header,
                                                          [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
    [DllImport(DllName)]
    private static extern void Acc_SetRewriteContactIp(IntPtr acc, bool enabled);

    [DllImport(DllName)]
    private static extern void Acc_SetVerifyIncomingCall(IntPtr acc, bool enabled);


    [DllImport(DllName)]
    private static extern void Acc_AddAudioCodec(IntPtr acc, AudioCodec codec);
    [DllImport(DllName)]
    private static extern void Acc_AddVideoCodec(IntPtr acc, VideoCodec codec);
    [DllImport(DllName)]
    private static extern void Acc_ResetAudioCodecs(IntPtr acc);
    [DllImport(DllName)]
    private static extern void Acc_ResetVideoCodecs(IntPtr acc);
    
    private static IntPtr getNative(AccData accData)
    {
        IntPtr ptr = Acc_GetDefault();
        Acc_SetSipServer(ptr,    accData.SipServer);
        Acc_SetSipExtension(ptr, accData.SipExtension);
        Acc_SetSipPassword(ptr,  accData.SipPassword);
        Acc_SetExpireTime(ptr,   accData.ExpireTime);

        if (accData.SipAuthId         != null) Acc_SetSipAuthId(ptr,          accData.SipAuthId);
        if (accData.SipProxyServer    != null) Acc_SetSipProxyServer(ptr,     accData.SipProxyServer);

        if (accData.UserAgent         != null) Acc_SetUserAgent(ptr,          accData.UserAgent);
        if (accData.DisplayName       != null) Acc_SetDisplayName(ptr,        accData.DisplayName);
        if (accData.InstanceId        != null) Acc_SetInstanceId(ptr,         accData.InstanceId);
        if (accData.RingToneFile      != null) Acc_SetRingToneFile(ptr,       accData.RingToneFile);

        if (accData.SecureMediaMode   != null) Acc_SetSecureMediaMode(ptr,    accData.SecureMediaMode.Value);
        if (accData.UseSipSchemeForTls!= null) Acc_SetUseSipSchemeForTls(ptr, accData.UseSipSchemeForTls.Value);
        if (accData.RtcpMuxEnabled    != null) Acc_SetRtcpMuxEnabled(ptr,     accData.RtcpMuxEnabled.Value);
        if (accData.KeepAliveTime     != null) Acc_SetKeepAliveTime(ptr,      accData.KeepAliveTime.Value);

        if (accData.TranspProtocol    != null) Acc_SetTranspProtocol(ptr,     accData.TranspProtocol.Value);
        if (accData.TranspPort        != null) Acc_SetTranspPort(ptr,         accData.TranspPort.Value);
        if (accData.TranspTlsCaCert   != null) Acc_SetTranspTlsCaCert(ptr,    accData.TranspTlsCaCert);
        if (accData.TranspBindAddr    != null) Acc_SetTranspBindAddr(ptr,     accData.TranspBindAddr);
        if (accData.TranspPreferIPv6  != null) Acc_SetTranspPreferIPv6(ptr,   accData.TranspPreferIPv6.Value);
        if (accData.RewriteContactIp  != null) Acc_SetRewriteContactIp(ptr,   accData.RewriteContactIp.Value);
        if (accData.VerifyIncomingCall!= null) Acc_SetVerifyIncomingCall(ptr, accData.VerifyIncomingCall.Value);

        if (accData.AudioCodecs != null)
        {
            Acc_ResetAudioCodecs(ptr);
            foreach (var ac in accData.AudioCodecs)
                Acc_AddAudioCodec(ptr, ac);
        }

        if (accData.VideoCodecs != null)
        {
            Acc_ResetVideoCodecs(ptr);
            foreach (var vc in accData.VideoCodecs)
                Acc_AddVideoCodec(ptr, vc);
        }

        if (accData.Xheaders != null)
        {
            foreach (var hdr in accData.Xheaders) 
                Acc_AddXHeader(ptr, hdr.Key, hdr.Value);
        }

        return ptr;
    }


    /// [Accounts] ///////////////////////////////////////////////////////////////////////////////////////////////
    [DllImport(DllName)]
    private static extern int Account_Add(IntPtr module, IntPtr acc, ref uint accId);
    [DllImport(DllName)]
    private static extern int Account_Update(IntPtr module, IntPtr acc, uint accId);
    [DllImport(DllName)]
    private static extern int Account_GetRegState(IntPtr module, uint accId, ref RegState state);
    [DllImport(DllName)]
    private static extern int Account_Register(IntPtr module, uint accId, uint expireTime);
    [DllImport(DllName)]
    private static extern int Account_Unregister(IntPtr module, uint accId);
    [DllImport(DllName)]
    private static extern int Account_Delete(IntPtr module, uint accId);


    /// [Dest] ///////////////////////////////////////////////////////////////////////////////////////////////
    [DllImport(DllName)]
    private static extern IntPtr Dest_GetDefault();
    [DllImport(DllName)]
    private static extern void Dest_SetExtension(IntPtr dest, [MarshalAs(UnmanagedType.LPUTF8Str)] string extension);
    [DllImport(DllName)]
    private static extern void Dest_SetAccountId(IntPtr dest, uint accId);
    [DllImport(DllName)]
    private static extern void Dest_SetVideoCall(IntPtr dest, bool video);
    [DllImport(DllName)]
    private static extern void Dest_SetInviteTimeout(IntPtr dest, int inviteTimeoutSec);
    [DllImport(DllName)]
    private static extern void Dest_SetDisplayName(IntPtr dest, [MarshalAs(UnmanagedType.LPUTF8Str)] string displayName);
    [DllImport(DllName)]
    private static extern void Dest_AddXHeader(IntPtr dest, [MarshalAs(UnmanagedType.LPUTF8Str)] string header,
                                                            [MarshalAs(UnmanagedType.LPUTF8Str)] string value);
    private static IntPtr getNative(DestData destData)
    {
        IntPtr ptr = Dest_GetDefault();
        Dest_SetExtension(ptr, destData.ToExt);
        Dest_SetAccountId(ptr, destData.FromAccId);
        Dest_SetVideoCall(ptr, destData.WithVideo);
        
        if (destData.InviteTimeout != null) 
            Dest_SetInviteTimeout(ptr, destData.InviteTimeout.Value);

        if (destData.DisplayName != null)
            Dest_SetDisplayName(ptr, destData.DisplayName);

        if (destData.Xheaders != null)
        {
            foreach (var hdr in destData.Xheaders)
                Dest_AddXHeader(ptr, hdr.Key, hdr.Value);
        }

        return ptr;
    }

    /// [Calls] ///////////////////////////////////////////////////////////////////////////////////////////////
    [DllImport(DllName)]
    private static extern int Call_Invite(IntPtr module, IntPtr destination, ref uint callId);
    [DllImport(DllName)]
    private static extern int Call_Reject(IntPtr module, uint callId, uint statusCode);
    [DllImport(DllName)]
    private static extern int Call_Accept(IntPtr module, uint callId, bool withVideo);
    [DllImport(DllName)]
    private static extern int Call_Hold(IntPtr module, uint callId);
    [DllImport(DllName)]
    private static extern int Call_GetHoldState(IntPtr module, uint callId, ref HoldState state);
    [DllImport(DllName)]
    private static extern int Call_GetNonce(IntPtr module, uint callId, 
			                        [MarshalAs(UnmanagedType.LPStr)] StringBuilder? nonceVal, 
                                    ref uint nonceValLen);
    [DllImport(DllName)]
    private static extern int Call_GetSipHeader(IntPtr module, uint callId,
                            [MarshalAs(UnmanagedType.LPUTF8Str)] string hdrName,
                            [MarshalAs(UnmanagedType.LPStr)] StringBuilder? hdrVal, 
                            ref uint hdrValLen);
    [DllImport(DllName)]
    private static extern int Call_MuteMic(IntPtr module, uint callId, bool mute);
    [DllImport(DllName)]
    private static extern int Call_MuteCam(IntPtr module, uint callId, bool mute);
    [DllImport(DllName)]
    private static extern int Call_SendDtmf(IntPtr module, uint callId,
                                    [MarshalAs(UnmanagedType.LPUTF8Str)] string dtmfs, 
                                    Int16 durationMs, Int16 intertoneGapMs, DtmfMethod method);
    [DllImport(DllName)]
    private static extern int Call_PlayFile(IntPtr module, uint callId, 
                                    [MarshalAs(UnmanagedType.LPUTF8Str)] string pathToMp3File, bool loop,
                                    ref uint playerId);
    [DllImport(DllName)]
    private static extern int Call_StopFile(IntPtr module, uint playerId);
    [DllImport(DllName)]
    private static extern int Call_RecordFile(IntPtr module, uint callId,
                                    [MarshalAs(UnmanagedType.LPUTF8Str)] string pathToMp3File);
    [DllImport(DllName)]
    private static extern int Call_StopRecordFile(IntPtr module, uint callId);
    [DllImport(DllName)]
    private static extern int Call_TransferBlind(IntPtr module, uint callId,
                                    [MarshalAs(UnmanagedType.LPUTF8Str)] string toExt);
    [DllImport(DllName)]
    private static extern int Call_TransferAttended(IntPtr module, uint fromCallId, 
                                    uint toCallId);
    [DllImport(DllName)]
    private static extern int Call_SetVideoWindow(IntPtr module, uint callId, IntPtr hwnd);

    [DllImport(DllName)]
    private static extern int Call_Bye(IntPtr module, uint callId);

    [DllImport(DllName)]
    private static extern int Call_Renegotiate(IntPtr module, uint callId);


    /// [Mixer] ///////////////////////////////////////////////////////////////////////////////////////////////
    [DllImport(DllName)]
    private static extern int Mixer_SwitchToCall(IntPtr module, uint callId);
    [DllImport(DllName)]
    private static extern int Mixer_MakeConference(IntPtr module);


    /// [Messages] ///////////////////////////////////////////////////////////////////////////////////////////////
    [DllImport(DllName)]
    private static extern int Message_Send(IntPtr module, IntPtr msg, ref uint msgId);
    

    /// [MsgData] ///////////////////////////////////////////////////////////////////////////////////////////////
    [DllImport(DllName)]
    private static extern IntPtr Msg_GetDefault();
    [DllImport(DllName)]
    private static extern void Msg_SetExtension(IntPtr sub, [MarshalAs(UnmanagedType.LPUTF8Str)] string extension);
    [DllImport(DllName)]
    private static extern void Msg_SetAccountId(IntPtr sub, uint subId);
    [DllImport(DllName)]
    private static extern void Msg_SetBody(IntPtr sub, [MarshalAs(UnmanagedType.LPUTF8Str)] string body);

    private static IntPtr getNative(MsgData msgData)
    {
        IntPtr ptr = Msg_GetDefault();
        Msg_SetExtension(ptr, msgData.ToExt);
        Msg_SetAccountId(ptr, msgData.FromAccId);
        Msg_SetBody(ptr, msgData.Body);
        return ptr;
    }

    /// [Subscriptions] ///////////////////////////////////////////////////////////////////////////////////////////////
    [DllImport(DllName)]
    private static extern int Subscription_Create(IntPtr module, IntPtr sub, ref uint subId);
    [DllImport(DllName)]
    private static extern int Subscription_Destroy(IntPtr module, uint subId);


    /// [SubscrData] ///////////////////////////////////////////////////////////////////////////////////////////////
    [DllImport(DllName)]
    private static extern IntPtr Subscr_GetDefault();
    [DllImport(DllName)]
    private static extern void Subscr_SetExtension(IntPtr sub, [MarshalAs(UnmanagedType.LPUTF8Str)] string extension);
    [DllImport(DllName)]
    private static extern void Subscr_SetAccountId(IntPtr sub, uint subId);
    [DllImport(DllName)]
    private static extern void Subscr_SetMimeSubtype(IntPtr sub, [MarshalAs(UnmanagedType.LPUTF8Str)] string mimeType);
    [DllImport(DllName)]
    private static extern void Subscr_SetEventType(IntPtr sub, [MarshalAs(UnmanagedType.LPUTF8Str)] string eventType);
    [DllImport(DllName)]
    private static extern void Subscr_SetExpireTime(IntPtr dest, uint expireTimeSec);
    
    private static IntPtr getNative(SubscrData subData)
    {
        IntPtr ptr = Subscr_GetDefault();
        Subscr_SetExtension(ptr,   subData.ToExt);
        Subscr_SetAccountId(ptr,   subData.FromAccId);
        Subscr_SetMimeSubtype(ptr, subData.MimeSubType);
        Subscr_SetEventType(ptr,   subData.EventType);

        if (subData.ExpireTime != null)
            Subscr_SetExpireTime(ptr, subData.ExpireTime.Value);

        return ptr;
    }

    /// [Callbacks] ///////////////////////////////////////////////////////////////////////////////////////////////
    private delegate void OnTrialModeNotified();
    private delegate void OnDevicesAudioChanged();        
    private delegate void OnAccountRegState(uint accId, RegState state, 
                                        [MarshalAs(UnmanagedType.LPUTF8Str)] string response);
    private delegate void OnSubscriptionState(uint subId, SubscriptionState state,
                                        [MarshalAs(UnmanagedType.LPUTF8Str)] string response);
    private delegate void OnNetworkState([MarshalAs(UnmanagedType.LPUTF8Str)] string name,
                                        NetworkState state);
    private delegate void OnPlayerState(uint playerId, PlayerState state);
    private delegate void OnRingerState(bool start);

    private delegate void OnCallIncoming(uint callId, uint accId, bool withVideo, 
                                        [MarshalAs(UnmanagedType.LPUTF8Str)] string hdrFrom,
                                        [MarshalAs(UnmanagedType.LPUTF8Str)] string hdrTo);
    private delegate void OnCallConnected(uint callId, 
                                        [MarshalAs(UnmanagedType.LPUTF8Str)] string hdrFrom, 
                                        [MarshalAs(UnmanagedType.LPUTF8Str)] string hdrTo,
                                        bool withVideo);
    private delegate void OnCallTerminated(uint callId, uint statusCode);
    private delegate void OnCallProceeding(uint callId, 
                                        [MarshalAs(UnmanagedType.LPUTF8Str)] string response);
    private delegate void OnCallTransferred(uint callId, uint statusCode);
    private delegate void OnCallRedirected(uint origCallId, uint relatedCallId, 
                                        [MarshalAs(UnmanagedType.LPUTF8Str)] string referTo);
    private delegate void OnCallDtmfReceived(uint callId, ushort tone);
    private delegate void OnCallHeld(uint callId, HoldState state);
    private delegate void OnCallSwitched(uint callId);

    private delegate void OnMessageSentState(uint messageId, bool success,
                                        [MarshalAs(UnmanagedType.LPUTF8Str)] string response);
    private delegate void OnMessageIncoming(uint accId,
                                        [MarshalAs(UnmanagedType.LPUTF8Str)] string hdrFrom,
                                        [MarshalAs(UnmanagedType.LPUTF8Str)] string body);

    [DllImport(DllName)]
    private static extern int Callback_SetTrialModeNotified(IntPtr module, OnTrialModeNotified callback);
    [DllImport(DllName)]
    private static extern int Callback_SetDevicesAudioChanged(IntPtr module, OnDevicesAudioChanged callback);
    [DllImport(DllName)]
    private static extern int Callback_SetAccountRegState(IntPtr module, OnAccountRegState callback);
    [DllImport(DllName)]
    private static extern int Callback_SetSubscriptionState(IntPtr module, OnSubscriptionState callback);
    [DllImport(DllName)]
    private static extern int Callback_SetNetworkState(IntPtr module, OnNetworkState callback);
    [DllImport(DllName)]
    private static extern int Callback_SetPlayerState(IntPtr module, OnPlayerState callback);
    [DllImport(DllName)]
    private static extern int Callback_SetRingerState(IntPtr module, OnRingerState callback);
    [DllImport(DllName)]
    private static extern int Callback_SetCallProceeding(IntPtr module, OnCallProceeding callback);
    [DllImport(DllName)]
    private static extern int Callback_SetCallTerminated(IntPtr module, OnCallTerminated callback);
    [DllImport(DllName)]
    private static extern int Callback_SetCallConnected(IntPtr module, OnCallConnected callback);
    [DllImport(DllName)]
    private static extern int Callback_SetCallIncoming(IntPtr module, OnCallIncoming callback);
    [DllImport(DllName)]
    private static extern int Callback_SetCallDtmfReceived(IntPtr module, OnCallDtmfReceived callback);
    [DllImport(DllName)]
    private static extern int Callback_SetCallTransferred(IntPtr module, OnCallTransferred callback);
    [DllImport(DllName)]
    private static extern int Callback_SetCallRedirected(IntPtr module, OnCallRedirected callback);
    [DllImport(DllName)]
    private static extern int Callback_SetCallSwitched(IntPtr module, OnCallSwitched callback);
    [DllImport(DllName)]
    private static extern int Callback_SetCallHeld(IntPtr module, OnCallHeld callback);
    [DllImport(DllName)]
    private static extern int Callback_SetMessageSentState(IntPtr module, OnMessageSentState callback);
    [DllImport(DllName)]
    private static extern int Callback_SetMessageIncoming(IntPtr module, OnMessageIncoming callback);


    void OnTrialModeNotifiedCallback()
    {
        eventDelegate_?.OnTrialModeNotified();
    }

    void OnDevicesAudioChangedCallback()
    {
        eventDelegate_?.OnDevicesAudioChanged();
    }

    void OnAccountRegStateCallback(uint accId, RegState state,
                                    [MarshalAs(UnmanagedType.LPUTF8Str)] string response)
    {
        eventDelegate_?.OnAccountRegState(accId, state, response);
    }
            
    void OnSubscriptionStateCallback(uint subId, SubscriptionState state,
                                     [MarshalAs(UnmanagedType.LPUTF8Str)] string response)
    {
        eventDelegate_?.OnSubscriptionState(subId, state, response);
    }

    void OnNetworkStateCallback([MarshalAs(UnmanagedType.LPUTF8Str)] string name,
                       NetworkState state)
    {
        eventDelegate_?.OnNetworkState(name, state);
    }

    void OnPlayerStateCallback(uint playerId, PlayerState state)
    {
        eventDelegate_?.OnPlayerState(playerId, state);
    }

    void OnRingerStateCallback(bool start)
    {
        eventDelegate_?.OnRingerState(start);
    }

    void OnCallIncomingCallback(uint callId, uint accId, bool withVideo,
                       [MarshalAs(UnmanagedType.LPUTF8Str)] string hdrFrom,
                       [MarshalAs(UnmanagedType.LPUTF8Str)] string hdrTo)
    {
        eventDelegate_?.OnCallIncoming(callId, accId, withVideo, hdrFrom, hdrFrom);
    }

    void OnCallConnectedCallback(uint callId,
                       [MarshalAs(UnmanagedType.LPUTF8Str)] string hdrFrom,
                       [MarshalAs(UnmanagedType.LPUTF8Str)] string hdrTo,
                       bool withVideo)
    {
        eventDelegate_?.OnCallConnected(callId, hdrFrom, hdrFrom, withVideo);
    }

    void OnCallTerminatedCallback(uint callId, uint statusCode)
    {
        eventDelegate_?.OnCallTerminated(callId, statusCode);
    }

    void OnCallProceedingCallback(uint callId,
                       [MarshalAs(UnmanagedType.LPUTF8Str)] string response)
    {
        eventDelegate_?.OnCallProceeding(callId, response);
    }

    void OnCallTransferredCallback(uint callId, uint statusCode)
    {
        eventDelegate_?.OnCallTransferred(callId, statusCode);
    }

    void OnCallRedirectedCallback(uint origCallId, uint relatedCallId,
                       [MarshalAs(UnmanagedType.LPUTF8Str)] string referTo)
    {
        eventDelegate_?.OnCallRedirected(origCallId, relatedCallId, referTo);
    }

    void OnCallDtmfReceivedCallback(uint callId, ushort tone)
    {
        eventDelegate_?.OnCallDtmfReceived(callId, tone);
    }


    void OnCallHeldCallback(uint callId, HoldState state)
    {
        eventDelegate_?.OnCallHeld(callId, state);
    }

    void OnCallSwitchedCallback(uint callId)
    {
        eventDelegate_?.OnCallSwitched(callId);
    }

    void OnMessageSentStateCallback(uint messageId, bool success, string response)
    {
        eventDelegate_?.OnMessageSentState(messageId, success, response);
    }

    void OnMessageIncomingCallback(uint accId, string hdrFrom, string body)
    {
        eventDelegate_?.OnMessageIncoming(accId, hdrFrom, body);
    }

}//CoreService

