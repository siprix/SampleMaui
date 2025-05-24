namespace Siprix;

public interface ICoreService
{
    Task<int> Initialize(IEventDelegate eventDelegate, IniData iniData);
    int UnInitialize();
    bool IsInitialized();
    string HomeFolder();
    string Version();
    string ErrorText(int code);
    
    int Account_Add(AccData accData);
    int Account_Update(AccData accData, uint accId);
    int Account_GetRegState(uint accId, ref RegState state);
    int Account_Register(uint accId, uint expireTime);
    int Account_Unregister(uint accId);
    int Account_Delete(uint accId);


    int Call_Invite(DestData dest);
    int Call_Reject(uint callId, uint statusCode = 486);
    int Call_Accept(uint callId, bool withVideo);
    string Call_GetSipHeader(uint callId, string hdrName);
    int Call_GetHoldState(uint callId, ref HoldState state);
    int Call_Hold(uint callId);
    int Call_MuteMic(uint callId, bool mute);
    int Call_MuteCam(uint callId, bool mute);
    int Call_SendDtmf(uint callId, string dtmfs,
            Int16 durationMs, Int16 intertoneGapMs, DtmfMethod method);
    int Call_PlayFile(uint callId, string pathToMp3File, bool loop, ref uint playerId);
    int Call_StopFile(uint playerId);
    int Call_RecordFile(uint callId, string pathToMp3File);
    int Call_StopRecordFile(uint callId);
    int Call_TransferBlind(uint callId, string toExt);
    int Call_TransferAttended(uint fromCallId, uint toCallId);
    int Call_SetVideoWindow(uint callId, IntPtr hwnd);
    int Call_Bye(uint callId);
    int Call_Renegotiate(uint callId);

    int Mixer_SwitchToCall(uint callId);
    int Mixer_MakeConference();

    int Message_Send(MsgData msgData);

    int Subscription_Add(SubscrData subData);
    int Subscription_Delete(uint subId);
}
