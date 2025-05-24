namespace Siprix;

public interface IEventDelegate
{
    void OnTrialModeNotified();
    void OnDevicesAudioChanged();
    
    void OnAccountRegState(uint accId, RegState state, string response);
    void OnSubscriptionState(uint subId, SubscriptionState state, string response);
    void OnNetworkState(string name, NetworkState state);
    void OnPlayerState(uint playerId, PlayerState state);
    void OnRingerState(bool start);
    
    void OnCallIncoming(uint callId, uint accId, bool withVideo, string hdrFrom, string hdrTo);
    void OnCallConnected(uint callId, string hdrFrom, string hdrTo, bool withVideo);
    void OnCallTerminated(uint callId, uint statusCode);
    void OnCallProceeding(uint callId, string response);
    void OnCallTransferred(uint callId, uint statusCode);
    void OnCallRedirected(uint origCallId, uint relatedCallId, string referTo);
    void OnCallDtmfReceived(uint callId, ushort tone);
    void OnCallHeld(uint callId, HoldState state);
    void OnCallSwitched(uint callId);

    void OnMessageSentState(uint messageId, bool success, string response);
    void OnMessageIncoming(uint accId, string hdrFrom, string body);
}
