#pragma warning disable 1998
namespace Siprix;

public class StubCoreService : ICoreService
{
    public async Task<int> Initialize(IEventDelegate eventDelegate, IniData iniData)
    {
        eventDelegate_ = eventDelegate;
        return Siprix.ErrorCode.kNoErr;
    }
    public int UnInitialize() { return Siprix.ErrorCode.kNoErr; }
    public bool IsInitialized() { return false; }
    public string HomeFolder() { return "Home"; }
    public string Version() { return "1.0";  }
    public string ErrorText(int code) { return "ErrText";  }

    public int Account_Add(AccData accData) 
    {
        accData.MyAccId = AccountId_++;
        if(accData.MyAccId % 3==0)
            eventDelegate_?.OnAccountRegState(accData.MyAccId, 
                accData.MyAccId%2==1 ? RegState.Success : RegState.Failed, 
                "Response");
        return Siprix.ErrorCode.kNoErr; 
    }
    public int Account_Update(AccData accData, uint accId) { return Siprix.ErrorCode.kNoErr; }
    public int Account_GetRegState(uint accId, ref RegState state) { return Siprix.ErrorCode.kNoErr; }
    public int Account_Register(uint accId, uint expireTime) { return Siprix.ErrorCode.kNoErr; }
    public int Account_Unregister(uint accId) { return Siprix.ErrorCode.kNoErr; }
    public int Account_Delete(uint accId) { return Siprix.ErrorCode.kNoErr; }


    public int Call_Invite(DestData dest) 
    {
        dest.MyCallId = CallId_++;
        eventDelegate_?.OnCallSwitched(dest.MyCallId);
        if (dest.MyCallId % 2 == 1) eventDelegate_?.OnCallConnected(dest.MyCallId, "From", "To", false);
        testCalls_[dest.MyCallId] = new TestCallData();
        return Siprix.ErrorCode.kNoErr; 
    }
    public int Call_Reject(uint callId, uint statusCode = 486)
    {
        eventDelegate_?.OnCallTerminated(callId, statusCode);
        return Siprix.ErrorCode.kNoErr; 
    }
    public int Call_Accept(uint callId, bool withVideo)
    {
        eventDelegate_?.OnCallConnected(callId, "From", "To", false);
        return Siprix.ErrorCode.kNoErr; 
    }
    public string Call_GetSipHeader(uint callId, string hdrName) { return "Header"; }
    public int Call_GetHoldState(uint callId, ref HoldState state) { return Siprix.ErrorCode.kNoErr; }
    public int Call_Hold(uint callId) 
    {
        bool found = testCalls_.TryGetValue(callId, out TestCallData? callData);
        if (!found || callData==null) return Siprix.ErrorCode.kCallNotFound;

        callData.held = !callData.held;
        eventDelegate_?.OnCallHeld(callId, callData.held ? HoldState.Local : HoldState.None);
        return Siprix.ErrorCode.kNoErr; 
    }
    public int Call_MuteMic(uint callId, bool mute) { return Siprix.ErrorCode.kNoErr; }
    public int Call_MuteCam(uint callId, bool mute) { return Siprix.ErrorCode.kNoErr; }
    public int Call_SendDtmf(uint callId, string dtmfs,
        Int16 durationMs, Int16 intertoneGapMs, DtmfMethod method) { return Siprix.ErrorCode.kNoErr;  }
    public int Call_PlayFile(uint callId, string pathToMp3File, bool loop, ref uint playerId)
    {
        playerId = CallId_++;
        eventDelegate_?.OnPlayerState(playerId, PlayerState.PlayerStarted);
        return Siprix.ErrorCode.kNoErr; 
    }
    public int Call_StopFile(uint playerId) 
    {
        eventDelegate_?.OnPlayerState(playerId, PlayerState.PlayerStopped);
        return Siprix.ErrorCode.kNoErr; 
    }
    public int Call_RecordFile(uint callId, string pathToMp3File) { return Siprix.ErrorCode.kNoErr; }
    public int Call_StopRecordFile(uint callId) { return Siprix.ErrorCode.kNoErr; }
    public int Call_TransferBlind(uint callId, string toExt) { return Siprix.ErrorCode.kNoErr; }
    public int Call_TransferAttended(uint fromCallId, uint toCallId) { return Siprix.ErrorCode.kNoErr; }
    public int Call_SetVideoWindow(uint callId, IntPtr hwnd) { return Siprix.ErrorCode.kNoErr; }
    public int Call_Bye(uint callId) 
    {
        eventDelegate_?.OnCallTerminated(callId, 0);
        testCalls_.Remove(callId);
        eventDelegate_?.OnCallSwitched(testCalls_.Count==0 ? 0 : testCalls_.Keys.Last());
        return Siprix.ErrorCode.kNoErr; 
    }
    public int Call_Renegotiate(uint callId) { return Siprix.ErrorCode.kNoErr; }

    public int Mixer_SwitchToCall(uint callId)
    {
        eventDelegate_?.OnCallSwitched(callId);
        return Siprix.ErrorCode.kNoErr; 
    }
    public int Mixer_MakeConference() { return Siprix.ErrorCode.kNoErr; }

    public int Message_Send(MsgData msgData) { msgData.MyMsgId = MsgId_++; return Siprix.ErrorCode.kNoErr; }

    public int Subscription_Add(SubscrData subData) { subData.MySubId = SubscrId_++; return Siprix.ErrorCode.kNoErr; }
    public int Subscription_Delete(uint subId) { return Siprix.ErrorCode.kNoErr; }

    class TestCallData 
    {
        public bool held=false;
    };

    private uint AccountId_ = 1;
    private uint CallId_ = 201;
    private uint SubscrId_ = 101;
    private uint MsgId_ = 701;
    private IEventDelegate? eventDelegate_;
    readonly private Dictionary<uint, TestCallData> testCalls_ = [];
}

