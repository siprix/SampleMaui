#pragma warning disable IDE1006

namespace Siprix;

public class CoreService : ICoreService
{
    private Com.Siprix.SiprixCore core_ = null!;//Create in the 'Initialize'
    private Com.Siprix.ISiprixServiceListener? serviceListener_;
    private ModelListenter? modelListener_;    

    /// [Module] /////////////////////////////////////////////////////////////////////////////////

    public bool IsInitialized()
    {
        return (core_ != null) && core_.IsInitialized;
    }

    public async Task<int> Initialize(IEventDelegate eventDelegate, IniData iniData)
    {
        if (core_ != null) return ErrorCode.kAlreadyInitialized;

        var activity = await Platform.WaitForActivityAsync();
        bool gotPermissions = await SampleMaui.MainActivity.RequestPermissions();

        core_ = new Com.Siprix.SiprixCore(activity);//use Activity as Context
        modelListener_ = new ModelListenter(eventDelegate);

        core_.Initialize(getNative(iniData));
        core_.SetServiceListener(serviceListener_);
        core_.SetModelListener(modelListener_);
        return ErrorCode.kNoErr;
    }

    public void SetServiceListener(Com.Siprix.ISiprixServiceListener servListener)
    {
        serviceListener_ = servListener;
        core_?.SetServiceListener(servListener);
    }

    public int UnInitialize()
    {
        return core_.UnInitialize();
    }

    public string HomeFolder()
    {
        return core_.HomeFolder!;
    }

    public string Version()
    {
        return core_.Version!;
    }

    public string ErrorText(int code)
    {
        return core_.GetErrText(code)!;
    }

    /// [Account] /////////////////////////////////////////////////////////////////////////////////
    
    public int Account_Add(AccData accData)
    {
        Com.Siprix.SiprixCore.IdOutArg id = new();
        int err = core_.AccountAdd(getNative(accData), id);

        accData.MyAccId = (uint)id.Value;
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
        return core_.AccountUnregister((int)accId);
    }

    public int Account_Delete(uint accId)
    {
        return core_.AccountDelete((int)accId);
    }


    /// [Calls] /////////////////////////////////////////////////////////////////////////////////

    public int Call_Invite(DestData dest)
    {
        Com.Siprix.SiprixCore.IdOutArg id = new();
        int err = core_.CallInvite(getNative(dest), id);

        dest.MyCallId = (uint)id.Value;
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
        Com.Siprix.SiprixCore.IdOutArg internalState = new();
        int err = core_.CallGetHoldState((int)callId, internalState);
        state = (HoldState)internalState.Value;
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
        Com.Siprix.SiprixCore.IdOutArg id = new();
        int err = core_.CallPlayFile((int)callId, pathToMp3File, loop, id);

        playerId = (uint)id.Value;
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
        return core_.CallRenegotiate((int)callId);
    }

    /// [Mixer] /////////////////////////////////////////////////////////////////////////////////

    public int Mixer_SwitchToCall(uint callId)
    {
        return core_.MixerSwitchToCall((int)callId);
    }

    public int Mixer_MakeConference()
    {
        return core_.MixerMakeConference();
    }

    /// [Message] /////////////////////////////////////////////////////////////////////////////////

    public int Message_Send(MsgData msgData)
    {
        Com.Siprix.SiprixCore.IdOutArg id = new();
        int err = core_.MessageSend(getNative(msgData), id);

        msgData.MyMsgId = (uint)id.Value;
        return err;
    }

    public int Subscription_Add(SubscrData subData)
    {
        Com.Siprix.SiprixCore.IdOutArg id = new();
        int err = core_.SubscrCreate(getNative(subData), id);

        subData.MySubId = (uint)id.Value;
        return err;
    }

    public int Subscription_Delete(uint subId)
    {
        return core_.SubscrDestroy((int)subId);
    }

    private static Com.Siprix.IniData getNative(IniData iniData)
    {
        Com.Siprix.IniData internalIni = new();
        if (iniData.License           != null) internalIni.SetLicense(iniData.License);
        if (iniData.LogLevelFile      != null) internalIni.SetLogLevelFile(getNative(iniData.LogLevelFile.Value));
        if (iniData.LogLevelIde       != null) internalIni.SetLogLevelIde(getNative(iniData.LogLevelIde.Value));
        if (iniData.ShareUdpTransport != null) internalIni.SetShareUdpTransport(iniData.ShareUdpTransport.Value);
        if (iniData.SingleCallMode    != null) internalIni.SetSingleCallMode(iniData.SingleCallMode.Value);
        if (iniData.RtpStartPort      != null) internalIni.SetRtpStartPort(iniData.RtpStartPort.Value);
        if (iniData.HomeFolder        != null) internalIni.HomeFolder = iniData.HomeFolder;
        if (iniData.BrandName         != null) internalIni.BrandName = iniData.BrandName;
        if (iniData.UseDnsSrv         != null) internalIni.SetUseDnsSrv(iniData.UseDnsSrv.Value);
        if (iniData.RecordStereo      != null) internalIni.SetRecordStereo(iniData.RecordStereo.Value);

        if (iniData.DnsServers != null)
        {
            foreach (var dns in iniData.DnsServers)
                internalIni.AddDnsServer(dns);
        }
        
        internalIni.SetUseExternalRinger(true);
        return internalIni;
    }

    private static Com.Siprix.AccData getNative(AccData accData)
    {
        Com.Siprix.AccData internalAcc = new();
        internalAcc.SetSipServer(accData.SipServer);
        internalAcc.SetSipExtension(accData.SipExtension);
        internalAcc.SetSipPassword(accData.SipPassword);
        internalAcc.SetExpireTime((int)accData.ExpireTime);

        if (accData.SipAuthId          != null) internalAcc.SetSipAuthId(accData.SipAuthId);
        if (accData.SipProxyServer     != null) internalAcc.SetSipProxyServer(accData.SipProxyServer);

        if (accData.UserAgent          != null) internalAcc.SetUserAgent(accData.UserAgent);
        if (accData.DisplayName        != null) internalAcc.SetDisplayName(accData.DisplayName);
        if (accData.InstanceId         != null) internalAcc.SetInstanceId(accData.InstanceId);
        if (accData.RingToneFile       != null) internalAcc.SetRingToneFile(accData.RingToneFile);

        if (accData.SecureMediaMode    != null) internalAcc.SetSecureMediaMode(getNative(accData.SecureMediaMode.Value));
        if (accData.UseSipSchemeForTls != null) internalAcc.SetUseSipSchemeForTls(accData.UseSipSchemeForTls.Value);
        if (accData.RtcpMuxEnabled     != null) internalAcc.SetRtcpMuxEnabled(accData.RtcpMuxEnabled.Value);
        if (accData.KeepAliveTime      != null) internalAcc.SetKeepAliveTime((int)accData.KeepAliveTime.Value);

        if (accData.TranspProtocol     != null) internalAcc.SetTranspProtocol(getNative(accData.TranspProtocol.Value));
        if (accData.TranspPort         != null) internalAcc.SetTranspPort(accData.TranspPort.Value);
        if (accData.TranspTlsCaCert    != null) internalAcc.SetTranspTlsCaCert(accData.TranspTlsCaCert);
        if (accData.TranspBindAddr     != null) internalAcc.SetTranspBindAddr(accData.TranspBindAddr);
        if (accData.TranspPreferIPv6   != null) internalAcc.SetTranspPreferIPv6(accData.TranspPreferIPv6.Value);
        if (accData.RewriteContactIp   != null) internalAcc.SetRewriteContactIp(accData.RewriteContactIp.Value);
        if (accData.VerifyIncomingCall != null) internalAcc.SetVerifyIncomingCall(accData.VerifyIncomingCall.Value);

        if (accData.AudioCodecs != null)
        {
            internalAcc.ResetAudioCodecs();
            foreach (var ac in accData.AudioCodecs)
                internalAcc.AddAudioCodec(getNative(ac));
        }

        if (accData.VideoCodecs != null)
        {
            internalAcc.ResetVideoCodecs();
            foreach (var vc in accData.VideoCodecs)
                internalAcc.AddVideoCodec(getNative(vc));
        }

        if (accData.Xheaders != null)
        {
            foreach (var hdr in accData.Xheaders)
                internalAcc.AddXHeader(hdr.Key, hdr.Value);
        }
        return internalAcc;
    }

    private static Com.Siprix.DestData getNative(DestData destData)
    {
        Com.Siprix.DestData internalDest = new();
        internalDest.SetExtension(destData.ToExt);
        internalDest.SetAccountId((int)destData.FromAccId);
        internalDest.SetVideoCall(destData.WithVideo);

        if (destData.DisplayName != null)   internalDest.SetDisplayName(destData.DisplayName);
        if (destData.InviteTimeout != null) internalDest.SetInviteTimeout(destData.InviteTimeout.Value);

        if (destData.Xheaders != null)
        {
            foreach (var hdr in destData.Xheaders)
                internalDest.AddXHeader(hdr.Key, hdr.Value);
        }
        return internalDest;
    }

    private static Com.Siprix.MsgData getNative(MsgData msgData)
    {
        Com.Siprix.MsgData internalMsg = new();
        internalMsg.SetExtension(msgData.ToExt);
        internalMsg.SetAccountId((int)msgData.FromAccId);
        internalMsg.SetBody(msgData.Body);
        return internalMsg;
    }

    private static Com.Siprix.SubscrData getNative(SubscrData subData)
    {
        Com.Siprix.SubscrData internalSub = new();
        internalSub.SetExtension(subData.ToExt);
        internalSub.SetAccountId((int)subData.FromAccId);
        internalSub.SetEventType(subData.EventType);
        internalSub.SetMimeSubtype(subData.MimeSubType);

        if (subData.ExpireTime != null) 
            internalSub.SetExpireTime((int)subData.ExpireTime.Value);

        return internalSub;
}

    private static Com.Siprix.IniData.LogLevel? getNative(LogLevel l)
    {
        switch (l)
        {
            case LogLevel.Stack:   return Com.Siprix.IniData.LogLevel.Stack;
            case LogLevel.Debug:   return Com.Siprix.IniData.LogLevel.Debug;
            case LogLevel.Warning: return Com.Siprix.IniData.LogLevel.Warning;
            case LogLevel.Error:   return Com.Siprix.IniData.LogLevel.Error;
            case LogLevel.NoLog:   return Com.Siprix.IniData.LogLevel.None;
            default:               return Com.Siprix.IniData.LogLevel.Info;
        }
    }

    private static Com.Siprix.AccData.SecureMediaMode? getNative(SecureMedia m)
    {
        switch (m)
        {
            case SecureMedia.SdesSrtp: return Com.Siprix.AccData.SecureMediaMode.SdesSrtp;
            case SecureMedia.DtlsSrtp: return Com.Siprix.AccData.SecureMediaMode.DtlsSrtp;
            default:                   return Com.Siprix.AccData.SecureMediaMode.Disaled;
        }
    }

    private static Com.Siprix.AccData.SipTransport? getNative(SipTransport t)
    {
        switch (t)
        {
            case SipTransport.TCP: return Com.Siprix.AccData.SipTransport.Tcp;
            case SipTransport.TLS: return Com.Siprix.AccData.SipTransport.Tls;
            default:               return Com.Siprix.AccData.SipTransport.Udp;
        }
    }

    private static Com.Siprix.AccData.AudioCodec? getNative(AudioCodec c)
    {
        switch (c)
        {
            case AudioCodec.Opus:   return Com.Siprix.AccData.AudioCodec.Opus;
            case AudioCodec.ISAC16: return Com.Siprix.AccData.AudioCodec.Isac16;
            case AudioCodec.ISAC32: return Com.Siprix.AccData.AudioCodec.Isac32;
            case AudioCodec.G722:   return Com.Siprix.AccData.AudioCodec.G722;
            case AudioCodec.ILBC:   return Com.Siprix.AccData.AudioCodec.Ilbc;
            case AudioCodec.PCMA:   return Com.Siprix.AccData.AudioCodec.Pcma;
            case AudioCodec.DTMF:   return Com.Siprix.AccData.AudioCodec.Dtmf;
            case AudioCodec.CN:     return Com.Siprix.AccData.AudioCodec.Cn;
            default:                return Com.Siprix.AccData.AudioCodec.Pcmu;
        }
    }

    private static Com.Siprix.AccData.VideoCodec? getNative(VideoCodec c)
    {
        switch (c)
        {
            case VideoCodec.VP8: return Com.Siprix.AccData.VideoCodec.Vp8;
            case VideoCodec.VP9: return Com.Siprix.AccData.VideoCodec.Vp9;
            case VideoCodec.AV1: return Com.Siprix.AccData.VideoCodec.Av1;
            default:             return Com.Siprix.AccData.VideoCodec.H264;
        }
    }

    private static Com.Siprix.SiprixCore.DtmfMethod? getNative(DtmfMethod c)
    {
        return (c == DtmfMethod.DTMF_RTP) ? Com.Siprix.SiprixCore.DtmfMethod.Rtp 
                                          : Com.Siprix.SiprixCore.DtmfMethod.Info;
    }

    private static Siprix.RegState getCommon(Com.Siprix.AccData.RegState s)
    {
        if(s.Equals(Com.Siprix.AccData.RegState.Success)) return RegState.Success;
        else if(s.Equals(Com.Siprix.AccData.RegState.Failed)) return RegState.Failed;
        else if (s.Equals(Com.Siprix.AccData.RegState.Removed)) return RegState.Removed;
        else return RegState.InProgress;
    }

    private static Siprix.SubscriptionState getCommon(Com.Siprix.SubscrData.SubscrState s)
    {
        if (s.Equals(Com.Siprix.SubscrData.SubscrState.Created)) return SubscriptionState.Created;
        else if (s.Equals(Com.Siprix.SubscrData.SubscrState.Updated)) return SubscriptionState.Updated;
        else return SubscriptionState.Destroyed;
    }

    private static Siprix.NetworkState getCommon(Com.Siprix.SiprixCore.NetworkState s)
    {
        if (s.Equals(Com.Siprix.SiprixCore.NetworkState.Lost)) return NetworkState.NetworkLost;
        else if (s.Equals(Com.Siprix.SiprixCore.NetworkState.Restored)) return NetworkState.NetworkRestored;
        else return NetworkState.NetworkSwitched;
    }

    private static Siprix.PlayerState getCommon(Com.Siprix.SiprixCore.PlayerState s)
    {
        if (s.Equals(Com.Siprix.SiprixCore.PlayerState.Started)) return PlayerState.PlayerStarted;
        else if (s.Equals(Com.Siprix.SiprixCore.PlayerState.Stopped)) return PlayerState.PlayerStopped;
        else return PlayerState.PlayerFailed;
    }

    private static Siprix.HoldState getCommon(Com.Siprix.SiprixCore.HoldState s)
    {
        if (s.Equals(Com.Siprix.SiprixCore.HoldState.LocalAndRemote)) return HoldState.LocalAndRemote;
        else if (s.Equals(Com.Siprix.SiprixCore.HoldState.Remote)) return HoldState.Remote;
        else if (s.Equals(Com.Siprix.SiprixCore.HoldState.Local)) return HoldState.Local;
        else return HoldState.None;
    }

    private class ModelListenter(IEventDelegate eventDelegate_) : Java.Lang.Object, Com.Siprix.ISiprixModelListener
    {
        public void OnTrialModeNotified()
        {
            eventDelegate_.OnTrialModeNotified();
        }

        public void OnDevicesAudioChanged()
        {
            eventDelegate_.OnDevicesAudioChanged();
        }

        public void OnAccountRegState(int accId, Com.Siprix.AccData.RegState? state, string? response)
        {
            if ((state != null) && (response != null))
                eventDelegate_.OnAccountRegState((uint)accId, getCommon(state), response);
        }

        public void OnSubscriptionState(int subId, Com.Siprix.SubscrData.SubscrState? state, string? response)
        {
            if ((state != null) && (response != null))
                eventDelegate_.OnSubscriptionState((uint)subId, getCommon(state), response);
        }

        public void OnNetworkState(string? name, Com.Siprix.SiprixCore.NetworkState? state)
        {
            if ((state != null) && (name != null))
                eventDelegate_.OnNetworkState(name, getCommon(state));
        }
        public void OnPlayerState(int playerId, Com.Siprix.SiprixCore.PlayerState? state)
        {
            if ((state != null) && (state != null))
                eventDelegate_.OnPlayerState((uint)playerId, getCommon(state));
        }

        public void OnCallIncoming(int callId, int accId, bool withVideo, string? hdrFrom, string? hdrTo)
        {
            if ((hdrFrom != null) && (hdrTo != null))
                eventDelegate_.OnCallIncoming((uint)callId, (uint)accId, withVideo, hdrFrom, hdrTo);
        }

        public void OnCallConnected(int callId, string? hdrFrom, string? hdrTo, bool withVideo)
        {
            if ((hdrFrom != null) && (hdrTo != null))
                eventDelegate_.OnCallConnected((uint)callId, hdrFrom, hdrTo, withVideo);
        }

        public void OnCallTerminated(int callId, int statusCode)
        {
             eventDelegate_.OnCallTerminated((uint)callId, (uint)statusCode);
        }

        public void OnCallProceeding(int callId, string? response)
        {
            if (response != null)
                eventDelegate_.OnCallProceeding((uint)callId, response);
        }

        public void OnCallTransferred(int callId, int statusCode)
        {
            eventDelegate_.OnCallTransferred((uint)callId, (uint)statusCode);
        }

        public void OnCallRedirected(int origCallId, int relatedCallId, string? referTo)
        {
            if (referTo != null)
                eventDelegate_.OnCallRedirected((uint)origCallId, (uint)relatedCallId, referTo);
        }

        public void OnCallDtmfReceived(int callId, int tone)
        {
            eventDelegate_.OnCallDtmfReceived((uint)callId, (ushort)tone);
        }
        public void OnCallHeld(int callId, Com.Siprix.SiprixCore.HoldState? state)
        {
            if (state != null)
                eventDelegate_.OnCallHeld((uint)callId, getCommon(state));
        }

        public void OnCallSwitched(int callId)
        {
            eventDelegate_.OnCallSwitched((uint)callId);
        }

        public void OnMessageSentState(int messageId, bool success, string? response)
        {
            if (response != null)
                eventDelegate_.OnMessageSentState((uint)messageId, success, response);
        }
        
        public void OnMessageIncoming(int accId, string? hdrFrom, string? body)
        {
            if ((hdrFrom != null)&& (body != null))
                eventDelegate_.OnMessageIncoming((uint)accId, hdrFrom, body);
        }
    };
}