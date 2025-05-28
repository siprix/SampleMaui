using ObjCRuntime;

namespace BindingIOS;

[Native]
public enum SipTransport : long
{
	Udp = 0,
	Tcp,
	Tls
}

[Native]
public enum RegState : long
{
	Success = 0,
	Failed,
	Removed,
	InProgress
}

[Native]
public enum SubscrState : long
{
	Created = 0,
	Updated,
	Destroyed
}

[Native]
public enum NetworkState : long
{
	Lost = 0,
	Restored,
	Switched
}

[Native]
public enum PlayerState : long
{
	Started = 0,
	Stopped,
	Failed
}

[Native]
public enum HoldState : long
{
	None = 0,
	Local = 1,
	Remote = 2,
	LocalAndRemote = 3
}

[Native]
public enum CallState : long
{
	Dialing = 0,
	Proceeding,
	Ringing,
	Rejecting,
	Accepting,
	Connected,
	Disconnecting,
	Holding,
	Held,
	Transferring
}

[Native]
public enum LogLevel : long
{
	Stack = 0,
	Debug = 1,
	Info = 2,
	Warning = 3,
	Error = 4,
	NoLog = 5
}

[Native]
public enum DtmfMethod : long
{
	Rtp = 0,
	Info = 1
}

[Native]
public enum SecureMedia : long
{
	Disabled = 0,
	SdesSrtp,
	DtlsSrtp
}

[Native]
public enum AudioCodecs : long
{
	Opus = 65,
	Isac16 = 66,
	Isac32 = 67,
	G722 = 68,
	Ilbc = 69,
	Pcmu = 70,
	Pcma = 71,
	Dtmf = 72,
	Cn = 73
}

[Native]
public enum VideoCodecs : long
{
	H264 = 80,
	Vp8 = 81,
	Vp9 = 82,
	Av1 = 83
}

[Native]
public enum VideoFrameRotation : long
{
	VideoFrameRotationRotation_0 = 0,
	VideoFrameRotationRotation_90 = 90,
	VideoFrameRotationRotation_180 = 180,
	VideoFrameRotationRotation_270 = 270
}

[Native]
public enum VideoFrameRGBType : long
{
	Argb,
	Bgra,
	Abgr,
	Rgba
}
