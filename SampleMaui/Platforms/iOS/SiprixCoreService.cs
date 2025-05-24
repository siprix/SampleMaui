
namespace Siprix;

public class CoreService : ICoreService
{
    public Task<int> Initialize(IEventDelegate eventDelegate, IniData iniData)
    {
        throw new NotImplementedException();
    }

    public bool IsInitialized()
    {
        throw new NotImplementedException();
    }

    public string ErrorText(int code)
    {
        throw new NotImplementedException();
    }

    public string HomeFolder()
    {
        throw new NotImplementedException();
    }

    public int Account_Add(AccData accData)
    {
        throw new NotImplementedException();
    }

    public int Account_Delete(uint accId)
    {
        throw new NotImplementedException();
    }

    public int Account_GetRegState(uint accId, ref RegState state)
    {
        throw new NotImplementedException();
    }

    public int Account_Register(uint accId, uint expireTime)
    {
        throw new NotImplementedException();
    }

    public int Account_Unregister(uint accId)
    {
        throw new NotImplementedException();
    }

    public int Account_Update(AccData accData, uint accId)
    {
        throw new NotImplementedException();
    }

    public int Call_Accept(uint callId, bool withVideo)
    {
        throw new NotImplementedException();
    }

    public int Call_Bye(uint callId)
    {
        throw new NotImplementedException();
    }

    public int Call_GetHoldState(uint callId, ref HoldState state)
    {
        throw new NotImplementedException();
    }

    public string Call_GetSipHeader(uint callId, string hdrName)
    {
        throw new NotImplementedException();
    }

    public int Call_Hold(uint callId)
    {
        throw new NotImplementedException();
    }

    public int Call_Invite(DestData dest)
    {
        throw new NotImplementedException();
    }

    public int Call_MuteCam(uint callId, bool mute)
    {
        throw new NotImplementedException();
    }

    public int Call_MuteMic(uint callId, bool mute)
    {
        throw new NotImplementedException();
    }

    public int Call_PlayFile(uint callId, string pathToMp3File, bool loop, ref uint playerId)
    {
        throw new NotImplementedException();
    }

    public int Call_RecordFile(uint callId, string pathToMp3File)
    {
        throw new NotImplementedException();
    }

    public int Call_Reject(uint callId, uint statusCode = 486)
    {
        throw new NotImplementedException();
    }

    public int Call_Renegotiate(uint callId)
    {
        throw new NotImplementedException();
    }

    public int Call_SendDtmf(uint callId, string dtmfs, short durationMs, short intertoneGapMs, DtmfMethod method)
    {
        throw new NotImplementedException();
    }

    public int Call_SetVideoWindow(uint callId, nint hwnd)
    {
        throw new NotImplementedException();
    }

    public int Call_StopFile(uint playerId)
    {
        throw new NotImplementedException();
    }

    public int Call_StopRecordFile(uint callId)
    {
        throw new NotImplementedException();
    }

    public int Call_TransferAttended(uint fromCallId, uint toCallId)
    {
        throw new NotImplementedException();
    }

    public int Call_TransferBlind(uint callId, string toExt)
    {
        throw new NotImplementedException();
    }


    public int Message_Send(MsgData msgData)
    {
        throw new NotImplementedException();
    }

    public int Mixer_MakeConference()
    {
        throw new NotImplementedException();
    }

    public int Mixer_SwitchToCall(uint callId)
    {
        throw new NotImplementedException();
    }

    public int Subscription_Add(SubscrData subData)
    {
        throw new NotImplementedException();
    }

    public int Subscription_Delete(uint subId)
    {
        throw new NotImplementedException();
    }

    public int UnInitialize()
    {
        throw new NotImplementedException();
    }

    public string Version()
    {
        throw new NotImplementedException();
    }
}