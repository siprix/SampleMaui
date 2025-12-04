//using BindingIOS;

using Foundation;

namespace Siprix;

public class CoreService : ICoreService
{
    private BindingIOS.SiprixModule core_ = null!;//Create in the 'Initialize'
    private ModelListenter? modelListener_;

    /// [Module] /////////////////////////////////////////////////////////////////////////////////

    public bool IsInitialized()
    {
        return (core_ != null);
    }

    public Task<int> Initialize(IEventDelegate eventDelegate, IniData iniData)
    {
        if (core_ != null) return Task.FromResult(ErrorCode.kAlreadyInitialized);

        core_ = new BindingIOS.SiprixModule();//use Activity as Context
        modelListener_ = new ModelListenter(eventDelegate);

        core_.Initialize(modelListener_, getNative(iniData));
        return Task.FromResult(ErrorCode.kNoErr);
    }

    public int UnInitialize()
    {
        return core_.UnInitialize();
    }

    public string HomeFolder()
    {
        return core_.HomeFolder();
    }

    public string Version()
    {
        return core_.Version();
    }

    public string ErrorText(int code)
    {
        return core_.GetErrorText(code);
    }

    /// [Account] /////////////////////////////////////////////////////////////////////////////////
    public int Account_Add(AccData accData)
    {
        BindingIOS.SiprixAccData nativeAcc = getNative(accData);
        int err = core_.AccountAdd(nativeAcc);

        accData.MyAccId = (uint)nativeAcc.MyAccId;
        return err;
    }

    public int Account_Update(AccData accData, uint accId)
    {
        return core_.AccountUpdate(getNative(accData), (int)accId);
    }

    public int Account_GetRegState(uint accId, ref RegState state)
    {
        return Siprix.ErrorCode.kNotImplemented;
    }

    public int Account_Register(uint accId, uint expireTime)
    {
        return core_.AccountRegister((int)accId, (int)expireTime);
    }

    public int Account_Unregister(uint accId)
    {
        return core_.AccountUnRegister((int)accId);
    }

    public int Account_Delete(uint accId)
    {
        return core_.AccountDelete((int)accId);
    }

    /// [Calls] /////////////////////////////////////////////////////////////////////////////////   

    public int Call_Invite(DestData dest)
    {
        BindingIOS.SiprixDestData nativeDest = getNative(dest);
        int err = core_.CallInvite(nativeDest);

        dest.MyCallId = (uint)nativeDest.MyCallId;
        return err;
    }

    public int Call_Reject(uint callId, uint statusCode = 486)
    {
        return core_.CallReject((int)callId, (int)statusCode);
    }

    public int Call_Accept(uint callId, bool withVideo)
    {
        return core_.CallAccept((int)callId, withVideo);
    }

    public string Call_GetSipHeader(uint callId, string hdrName)
    {
        return core_.CallGetSipHeader((int)callId, hdrName)!;
    }

    public int Call_GetHoldState(uint callId, ref HoldState state)
    {
        BindingIOS.SiprixHoldData nativeData = new();
        int err = core_.CallGetHoldState((int)callId, nativeData);
        state = (HoldState)nativeData.HoldState;
        return err;
    }

    public int Call_Hold(uint callId)
    {
        return core_.CallHold((int)callId);
    }

    public int Call_MuteMic(uint callId, bool mute)
    {
        return core_.CallMuteMic((int)callId, mute);
    }

    public int Call_MuteCam(uint callId, bool mute)
    {
        return core_.CallMuteCam((int)callId, mute);
    }

    public int Call_SendDtmf(uint callId, string dtmfs, short durationMs, short intertoneGapMs, DtmfMethod method)
    {
        return core_.CallSendDtmf((int)callId, dtmfs, durationMs, intertoneGapMs, getNative(method));
    }

    public int Call_PlayFile(uint callId, string pathToMp3File, bool loop, ref uint playerId)
    {
        BindingIOS.SiprixPlayerData nativeData = new();
        int err = core_.CallPlayFile((int)callId, pathToMp3File, loop, nativeData);

        playerId = (uint)nativeData.PlayerId;
        return err;
    }

    public int Call_StopFile(uint playerId)
    {
        return core_.CallStopPlayFile((int)playerId);
    }

    public int Call_RecordFile(uint callId, string pathToMp3File)
    {
        return core_.CallRecordFile((int)callId, pathToMp3File);
    }

    public int Call_StopRecordFile(uint callId)
    {
        return core_.CallStopRecordFile((int)callId);
    }

    public int Call_TransferBlind(uint callId, string toExt)
    {
        return core_.CallTransferBlind((int)callId, toExt);
    }

    public int Call_TransferAttended(uint fromCallId, uint toCallId)
    {
        return core_.CallTransferAttended((int)fromCallId, (int)toCallId);
    }

    public int Call_SetVideoWindow(uint callId, nint hwnd)
    {
        //return core_.CallSetVideoRenderer(int )
        //TODO impl
        return Siprix.ErrorCode.kNotImplemented;
    }

    public int Call_Bye(uint callId)
    {
        return core_.CallBye((int)callId);
    }

    public int Call_Renegotiate(uint callId)
    {
        return ErrorCode.kNotImplemented;
    }

    /// [Mixer] /////////////////////////////////////////////////////////////////////////////////

    public int Mixer_SwitchToCall(uint callId)
    {
        return core_.MixerSwitchCall((int)callId);
    }

    public int Mixer_MakeConference()
    {
        return core_.MixerMakeConference();
    }

    /// [Message] /////////////////////////////////////////////////////////////////////////////////

    public int Message_Send(MsgData msgData)
    {
        BindingIOS.SiprixMsgData nativeMsg = getNative(msgData);
        int err = core_.MessageSend(nativeMsg);

        msgData.MyMsgId = (uint)nativeMsg.MyMessageId;
        return err;
    }

    public int Subscription_Add(SubscrData subData)
    {
        BindingIOS.SiprixSubscrData nativeSubscr = getNative(subData);
        int err = core_.SubscrCreate(nativeSubscr);

        subData.MySubId = (uint)nativeSubscr.MySubscrId;
        return err;
    }

    public int Subscription_Delete(uint subId)
    {
        return core_.SubscrDestroy((int)subId);
    }


    private static BindingIOS.SiprixIniData getNative(IniData iniData)
    {
        BindingIOS.SiprixIniData internalIni = new();
        if (iniData.License           != null) internalIni.License = iniData.License;
        if (iniData.LogLevelFile      != null) internalIni.LogLevelFile = getNative(iniData.LogLevelFile.Value);
        if (iniData.LogLevelIde       != null) internalIni.LogLevelIde = getNative(iniData.LogLevelIde.Value);
        if (iniData.ShareUdpTransport != null) internalIni.ShareUdpTransport = iniData.ShareUdpTransport.Value;
        if (iniData.SingleCallMode    != null) internalIni.SingleCallMode = iniData.SingleCallMode.Value;
        if (iniData.RtpStartPort      != null) internalIni.RtpStartPort = iniData.RtpStartPort.Value;
        if (iniData.HomeFolder        != null) internalIni.HomeFolder = iniData.HomeFolder;
        if (iniData.BrandName         != null) internalIni.BrandName = iniData.BrandName;
        if (iniData.UseDnsSrv         != null) internalIni.UseDnsSrv = iniData.UseDnsSrv.Value;
        if (iniData.RecordStereo      != null) internalIni.RecordStereo = iniData.RecordStereo.Value;        
        //if (iniData.DnsServers        != null) internalIni.DnsServers = NSObject.FromObject(iniData.DnsServers.ToArray());//TODO Check
        
        return internalIni;
    }

    private static BindingIOS.SiprixAccData getNative(AccData accData)
    {
        BindingIOS.SiprixAccData internalAcc = new();
        internalAcc.SipServer    = accData.SipServer;
        internalAcc.SipExtension = accData.SipExtension;
        internalAcc.SipPassword  = accData.SipPassword;
        internalAcc.ExpireTime   = (int)accData.ExpireTime;

        if (accData.SipAuthId          != null) internalAcc.SipAuthId = accData.SipAuthId;
        if (accData.SipProxyServer     != null) internalAcc.SipProxy = accData.SipProxyServer;

        if (accData.UserAgent          != null) internalAcc.UserAgent = accData.UserAgent;
        if (accData.DisplayName        != null) internalAcc.DisplName = accData.DisplayName;
        if (accData.InstanceId         != null) internalAcc.InstanceId = accData.InstanceId;
        if (accData.RingToneFile       != null) internalAcc.RingTonePath = accData.RingToneFile;

        if (accData.SecureMediaMode    != null) internalAcc.SecureMedia = getNative(accData.SecureMediaMode.Value);
        if (accData.UseSipSchemeForTls != null) internalAcc.TlsUseSipScheme = accData.UseSipSchemeForTls.Value;
        if (accData.RtcpMuxEnabled     != null) internalAcc.RtcpMuxEnabled = accData.RtcpMuxEnabled.Value;
        if (accData.KeepAliveTime      != null) internalAcc.KeepAliveTime = (int)accData.KeepAliveTime.Value;

        if (accData.TranspProtocol     != null) internalAcc.Transport = getNative(accData.TranspProtocol.Value);
        if (accData.TranspPort         != null) internalAcc.Port = accData.TranspPort.Value;
        if (accData.TranspTlsCaCert    != null) internalAcc.TlsCaCertPath = accData.TranspTlsCaCert;
        //if (accData.TranspBindAddr     != null) internalAcc.TranspBindAddr = accData.TranspBindAddr;//TODO add
        if (accData.TranspPreferIPv6   != null) internalAcc.TranspPreferIPv6 = accData.TranspPreferIPv6.Value;
        if (accData.RewriteContactIp   != null) internalAcc.RewriteContactIp = accData.RewriteContactIp.Value;
        if (accData.VerifyIncomingCall != null) internalAcc.VerifyIncomingCall = accData.VerifyIncomingCall.Value;

        //if (accData.AudioCodecs        != null) internalAcc.ACodecs = getNative(accData.AudioCodecs);//TODO convert

        if (accData.VideoCodecs != null)
        {
            //internalAcc.ResetVideoCodecs();
            //foreach (var vc in accData.VideoCodecs)
            //    internalAcc.AddVideoCodec(getNative(vc));//TODO convert
        }

        if (accData.Xheaders != null)
        {
            internalAcc.Xheaders = NSDictionary.FromObjectsAndKeys(accData.Xheaders.Values.ToArray(),
                                            accData.Xheaders.Keys.ToArray());
            //foreach (var hdr in accData.Xheaders)
            //internalAcc.AddXHeader(hdr.Key, hdr.Value);
        }
        return internalAcc;
    }

    private static BindingIOS.SiprixDestData getNative(DestData destData)
    {
        BindingIOS.SiprixDestData internalDest = new();
        internalDest.ToExt     = destData.ToExt;
        internalDest.FromAccId = (int)destData.FromAccId;
        internalDest.WithVideo = destData.WithVideo;

        if (destData.DisplayName   != null) internalDest.DisplName = destData.DisplayName;
        if (destData.InviteTimeout != null) internalDest.InviteTimeoutSec = destData.InviteTimeout.Value;

        if (destData.Xheaders != null)
        {
            internalDest.Xheaders = NSDictionary.FromObjectsAndKeys(destData.Xheaders.Values.ToArray()
                                               , destData.Xheaders.Keys.ToArray());
            //foreach (var hdr in destData.Xheaders)
            //    internalDest.AddXHeader(hdr.Key, hdr.Value);
        }
        return internalDest;
    }

    private static BindingIOS.SiprixMsgData getNative(MsgData msgData)
    {
        BindingIOS.SiprixMsgData internalMsg = new();
        internalMsg.ToExt     = msgData.ToExt;
        internalMsg.FromAccId = (int)msgData.FromAccId;
        internalMsg.Body      = msgData.Body;
        return internalMsg;
    }

    private static BindingIOS.SiprixSubscrData getNative(SubscrData subData)
    {
        BindingIOS.SiprixSubscrData internalSub = new();
        internalSub.ToExt       = subData.ToExt;
        internalSub.FromAccId   = (int)subData.FromAccId;
        internalSub.EventType   = subData.EventType;
        internalSub.MimeSubtype = subData.MimeSubType;

        if (subData.ExpireTime != null)
            internalSub.ExpireTime = (int)subData.ExpireTime.Value;

        return internalSub;
    }

    private static NSNumber getNative(LogLevel l)
    {
        switch (l)
        {
            case LogLevel.Stack:   return (int)BindingIOS.LogLevel.Stack;
            case LogLevel.Debug:   return (int)BindingIOS.LogLevel.Debug;
            case LogLevel.Warning: return (int)BindingIOS.LogLevel.Warning;
            case LogLevel.Error:   return (int)BindingIOS.LogLevel.Error;
            case LogLevel.NoLog:   return (int)BindingIOS.LogLevel.NoLog;
            default:               return (int)BindingIOS.LogLevel.Info;
        }
    }

    private static NSNumber getNative(SecureMedia m)
    {
        switch (m)
        {
            case SecureMedia.SdesSrtp: return (int)BindingIOS.SecureMedia.SdesSrtp;
            case SecureMedia.DtlsSrtp: return (int)BindingIOS.SecureMedia.DtlsSrtp;
            default:                   return (int)BindingIOS.SecureMedia.Disabled;
        }
    }

    private static BindingIOS.SipTransport getNative(SipTransport t)
    {
        switch (t)
        {
            case SipTransport.TCP: return BindingIOS.SipTransport.Tcp;
            case SipTransport.TLS: return BindingIOS.SipTransport.Tls;
            default:               return BindingIOS.SipTransport.Udp;
        }
    }

    private static BindingIOS.AudioCodecs getNative(AudioCodec c)
    {
        switch (c)
        {
            case AudioCodec.Opus:   return BindingIOS.AudioCodecs.Opus;
            case AudioCodec.ISAC16: return BindingIOS.AudioCodecs.Isac16;
            case AudioCodec.ISAC32: return BindingIOS.AudioCodecs.Isac32;
            case AudioCodec.G722:   return BindingIOS.AudioCodecs.G722;
            case AudioCodec.ILBC:   return BindingIOS.AudioCodecs.Ilbc;
            case AudioCodec.PCMA:   return BindingIOS.AudioCodecs.Pcma;
            case AudioCodec.DTMF:   return BindingIOS.AudioCodecs.Dtmf;
            case AudioCodec.CN:     return BindingIOS.AudioCodecs.Cn;
            default:                return BindingIOS.AudioCodecs.Pcmu;
        }
    }

    private static BindingIOS.VideoCodecs getNative(VideoCodec c)
    {
        switch (c)
        {
            case VideoCodec.VP8: return BindingIOS.VideoCodecs.Vp8;
            case VideoCodec.VP9: return BindingIOS.VideoCodecs.Vp9;
            case VideoCodec.AV1: return BindingIOS.VideoCodecs.Av1;
            default:             return BindingIOS.VideoCodecs.H264;
        }
    }

    private static BindingIOS.DtmfMethod getNative(DtmfMethod c)
    {
        return (c == DtmfMethod.DTMF_RTP) ? BindingIOS.DtmfMethod.Rtp
                                          : BindingIOS.DtmfMethod.Info;
    }

    private static Siprix.RegState getCommon(BindingIOS.RegState s)
    {
        if (s.Equals(BindingIOS.RegState.Success))      return RegState.Success;
        else if (s.Equals(BindingIOS.RegState.Failed))  return RegState.Failed;
        else if (s.Equals(BindingIOS.RegState.Removed)) return RegState.Removed;
        else                                            return RegState.InProgress;
    }

    private static Siprix.SubscriptionState getCommon(BindingIOS.SubscrState s)
    {
        if (s.Equals(BindingIOS.SubscrState.Created))      return SubscriptionState.Created;
        else if (s.Equals(BindingIOS.SubscrState.Updated)) return SubscriptionState.Updated;
        else                                               return SubscriptionState.Destroyed;
    }

    private static Siprix.NetworkState getCommon(BindingIOS.NetworkState s)
    {
        if (s.Equals(BindingIOS.NetworkState.Lost))          return NetworkState.NetworkLost;
        else if (s.Equals(BindingIOS.NetworkState.Restored)) return NetworkState.NetworkRestored;
        else                                                 return NetworkState.NetworkSwitched;
    }

    private static Siprix.PlayerState getCommon(BindingIOS.PlayerState s)
    {
        if (s.Equals(BindingIOS.PlayerState.Started))      return PlayerState.PlayerStarted;
        else if (s.Equals(BindingIOS.PlayerState.Stopped)) return PlayerState.PlayerStopped;
        else                                               return PlayerState.PlayerFailed;
    }

    private static Siprix.HoldState getCommon(BindingIOS.HoldState s)
    {
        if (s.Equals(BindingIOS.HoldState.LocalAndRemote)) return HoldState.LocalAndRemote;
        else if (s.Equals(BindingIOS.HoldState.Remote))    return HoldState.Remote;
        else if (s.Equals(BindingIOS.HoldState.Local))     return HoldState.Local;
        else                                               return HoldState.None;
    }

    private class ModelListenter(IEventDelegate eventDelegate_) : BindingIOS.SiprixEventDelegate
    {
        public override void OnTrialModeNotified()
        {
            eventDelegate_.OnTrialModeNotified();
        }

        public override void OnDevicesAudioChanged()
        {
            eventDelegate_.OnDevicesAudioChanged();
        }

        public override void OnAccountRegState(nint accId, BindingIOS.RegState state, string response)
        {
            eventDelegate_.OnAccountRegState((uint)accId, getCommon(state), response);
        }

        public override void OnSubscriptionState(nint subId, BindingIOS.SubscrState state, string response)
        {
            eventDelegate_.OnSubscriptionState((uint)subId, getCommon(state), response);
        }

        public override void OnNetworkState(string name, BindingIOS.NetworkState state)
        {
            eventDelegate_.OnNetworkState(name, getCommon(state));
        }
        public override void OnPlayerState(nint playerId, BindingIOS.PlayerState state)
        {
            eventDelegate_.OnPlayerState((uint)playerId, getCommon(state));
        }

        public override void OnRingerState(bool started)
        {
            eventDelegate_.OnRingerState(started);
        }

        public override void OnCallIncoming(nint callId, nint accId, bool withVideo, string hdrFrom, string hdrTo)
        {
            eventDelegate_.OnCallIncoming((uint)callId, (uint)accId, withVideo, hdrFrom, hdrTo);
        }

        public override void OnCallConnected(nint callId, string? hdrFrom, string hdrTo, bool withVideo)
        {
            if ((hdrFrom != null) && (hdrTo != null))
                eventDelegate_.OnCallConnected((uint)callId, hdrFrom, hdrTo, withVideo);
        }

        public override void OnCallTerminated(nint callId, nint statusCode)
        {
            eventDelegate_.OnCallTerminated((uint)callId, (uint)statusCode);
        }

        public override void OnCallProceeding(nint callId, string response)
        {
            if (response != null)
                eventDelegate_.OnCallProceeding((uint)callId, response);
        }

        public override void OnCallTransferred(nint callId, nint statusCode)
        {
            eventDelegate_.OnCallTransferred((uint)callId, (uint)statusCode);
        }

        public override void OnCallRedirected(nint origCallId, nint relatedCallId, string referTo)
        {
            if (referTo != null)
                eventDelegate_.OnCallRedirected((uint)origCallId, (uint)relatedCallId, referTo);
        }

        public override void OnCallDtmfReceived(nint callId, nint tone)
        {
            eventDelegate_.OnCallDtmfReceived((uint)callId, (ushort)tone);
        }
        public override void OnCallHeld(nint callId, BindingIOS.HoldState state)
        {
            eventDelegate_.OnCallHeld((uint)callId, getCommon(state));
        }

        public override void OnCallSwitched(nint callId)
        {
            eventDelegate_.OnCallSwitched((uint)callId);
        }

        public override void OnMessageSentState(nint messageId, bool success, string response)
        {
            eventDelegate_.OnMessageSentState((uint)messageId, success, response);
        }

        public override void OnMessageIncoming(nint accId, string hdrFrom, string body)
        {
            eventDelegate_.OnMessageIncoming((uint)accId, hdrFrom, body);
        }
    };
}