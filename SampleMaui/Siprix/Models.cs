namespace Siprix;
public enum LogLevel : byte
{
    Stack = 0,
    Debug = 1,
    Info = 2,
    Warning = 3,
    Error = 4,
    NoLog = 5
}

public enum RegState : byte
{
    Success = 0, //Registeration success
    Failed,      //Registration failed
    Removed,     //Registration removed
    InProgress
};

public enum SubscriptionState : byte
{
    Created = 0, //Subscription just created and waiting response
    Updated,     //(received NOTIFY)
    Destroyed    //Received error (timeout) on initial SUBSCRIBE or app unsubscribed it
};

public enum SecureMedia : byte
{
    Disabled = 0,        
    SdesSrtp,
    DtlsSrtp,
};

public enum SipTransport : byte
{
    UDP = 0,
    TCP,
    TLS,
};

public enum DtmfMethod : byte
{
    DTMF_RTP = 0,
    DTMF_INFO
};

public enum AudioCodec : byte
{
    Opus = 65,
    ISAC16 = 66,
    ISAC32 = 67,
    G722 = 68,
    ILBC = 69,
    PCMU = 70,
    PCMA = 71,
    DTMF = 72,
    CN = 73
};

public enum VideoCodec : byte
{
    H264 = 80,
    VP8 = 81,
    VP9 = 82,
    AV1 = 83
};

public enum HoldState : byte
{
    None = 0,
    Local = 1,
    Remote = 2,
    LocalAndRemote = 3
};

public enum PlayerState : byte
{
    PlayerStarted = 0,
    PlayerStopped = 1,
    PlayerFailed = 2,
};

public enum NetworkState : byte
{
    NetworkLost = 0,
    NetworkRestored = 1,
    NetworkSwitched = 2
};

public enum CallState
{
    Dialing,      //Outgoing call just initiated
    Proceeding,   //Outgoing call in progress, received 100Trying or 180Ringing

    Ringing,      //Incoming call just received
    Rejecting,    //Incoming call rejecting after invoke 'call.reject'
    Accepting,    //Incoming call aceepting after invoke 'call.accept'

    Connected,    //Call successfully established, RTP is flowing

    Disconnecting,//Call disconnecting after invoke 'call.bye'

    Holding,      //Call holding (renegotiating RTP stream states)
    Held,         //Call held, RTP is NOT flowing

    Transferring, //Call transferring
}


public class IniData
{
    public string?   License;
    public LogLevel? LogLevelFile;
    public LogLevel? LogLevelIde;
    public bool?     ShareUdpTransport;
    public bool?     WriteDmpUnhandledExc;
    public bool?     TlsVerifyServer;
    public bool?     SingleCallMode;
    public ushort?   RtpStartPort;
    public string?   HomeFolder;
    public string?   BrandName;
    public bool?     UseDnsSrv;
    public bool?     RecordStereo;
    public List<string>? DnsServers;

}//IniData


public class AccData
{
    public uint    MyAccId = 0;//Assigned by module in 'Account_Add'
    public string  SipServer ="";
    public string  SipExtension = "";        
    public string  SipPassword = "";
     
    public uint    ExpireTime = 300;
    public string? SipAuthId;
    public string? SipProxyServer;

    public string? UserAgent;
    public string? DisplayName;
    public string? InstanceId;
    public string? RingToneFile;

    public SecureMedia?   SecureMediaMode;
    public bool?          UseSipSchemeForTls;
    public bool?          RtcpMuxEnabled;
    public uint?          KeepAliveTime;

    public SipTransport?  TranspProtocol;
    public ushort?        TranspPort;
    public string?        TranspTlsCaCert;
    public string?        TranspBindAddr;
    public bool?          TranspPreferIPv6;
    public bool?          RewriteContactIp;
    public bool?          VerifyIncomingCall;
    
    public List<AudioCodec>? AudioCodecs;
    public List<VideoCodec>? VideoCodecs;
    public Dictionary<string, string>? Xheaders;

}//AccData

public class DestData
{
    public uint    MyCallId = 0;     //Assigned by module in 'Call_Invite'
    public string  ToExt = "";
    public uint    FromAccId = 0;
    public bool    WithVideo = false;
    public string? DisplayName;
    public int?    InviteTimeout;
    public Dictionary<string, string>? Xheaders;
}

public class SubscrData
{
    public uint    MySubId = 0;     //Assigned by module in 'Subscription_Add'
    public string  ToExt = "";
    public uint    FromAccId = 0;
    public string  MimeSubType="";
    public string  EventType="";
    public uint?   ExpireTime;
}

public class MsgData
{
    public uint   MyMsgId = 0;     //Assigned by module in 'Message_Send'
    public string ToExt = "";
    public uint   FromAccId = 0;
    public string Body = "";
}

public static class ErrorCode
{
    public const uint kInvalidId = 0;

    public const int kNoErr               = 0;
    public const int kAlreadyInitialized  = -1000;
    public const int kNotInitialized      = -1001;
    public const int kInitializeFailure   = -1002;
    public const int kObjectNull          = -1003;
    public const int kArgumentNull        = -1004;
    public const int kNotImplemented      = -1005;

    public const int kBadSipServer        = -1010;
    public const int kBadSipExtension     = -1011;
    public const int kBadSecureMediaMode  = -1012;
    public const int kBadTranspProtocol   = -1013;
    public const int kBadTranspPort       = -1014;

    public const int kDuplicateAccount    = -1021;
    public const int kAccountNotFound     = -1022;
    public const int kAccountHasCalls     = -1023;
    public const int kAccountDoenstMatch  = -1024;
    public const int kSingleAccountMode   = -1025;
    public const int kAccountHasSubscr    = -1026;

    public const int kDestNumberEmpty     = -1030;
    public const int kDestNumberSpaces    = -1031;
    public const int kDestNumberScheme    = -1032;
    public const int kDestBadFormat       = -1033;
    public const int kDestSchemeMismatch  = -1034;
    public const int kOnlyOneCallAllowed  = -1035;    

    public const int kCallNotFound        = -1040;
    public const int kCallNotIncoming     = -1041;
    public const int kCallAlreadyAnswered = -1042;
    public const int kCallNotConnected    = -1043;
    public const int kBadDtmfStr          = -1044;
    public const int kFileDoesntExists    = -1045;
    public const int kFileExtMp3Expected  = -1046;
    public const int kCallAlreadySwitched = -1047;
    public const int kCallAlredyMuted     = -1048;
    public const int kCallRecAlredyStarted= -1049;
    public const int kCallRecNotStarted   = -1050;
    public const int kCallCantReferBlind  = -1051;
    public const int kCallReferInProgress = -1052;
    public const int kCallCantReferAtt    = -1053;
    public const int kCallReferAttSameId  = -1054;
    public const int kConfRequires2Calls  = -1055;
    public const int kCallIsHolding       = -1056;
    public const int kRndrAlreadyAssigned = -1057;
    public const int kSipHeaderNotFound   = -1058;
     
    public const int kBadDeviceIndex      = -1070;

    public const int kEventTypeCantBeEmpty= -1080;
    public const int kSubTypeCantBeEmpty  = -1081;
    public const int kSubscrDoesntExist   = -1082;
    public const int kSubscrAlreadyExist  = -1083;

    public const int kMsgBodyCantBeEmpty  = -1085;

    public const int kMicPermRequired     = -1111;
}