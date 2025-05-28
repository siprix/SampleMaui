using System;
//using AVFAudio;
using Foundation;
using ObjCRuntime;
using UIKit;
//using siprix;

namespace BindingIOS;

// @interface SiprixIniData : NSObject
[BaseType (typeof(NSObject))]
interface SiprixIniData
{
	// @property (retain, nonatomic) NSString * _Nullable license;
	[NullAllowed, Export ("license", ArgumentSemantic.Retain)]
	string License { get; set; }

	// @property (retain, nonatomic) NSString * _Nullable homeFolder;
	[NullAllowed, Export ("homeFolder", ArgumentSemantic.Retain)]
	string HomeFolder { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable logLevelFile;
	[NullAllowed, Export ("logLevelFile", ArgumentSemantic.Retain)]
	NSNumber LogLevelFile { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable logLevelIde;
	[NullAllowed, Export ("logLevelIde", ArgumentSemantic.Retain)]
	NSNumber LogLevelIde { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable rtpStartPort;
	[NullAllowed, Export ("rtpStartPort", ArgumentSemantic.Retain)]
	NSNumber RtpStartPort { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable tlsVerifyServer;
	[NullAllowed, Export ("tlsVerifyServer", ArgumentSemantic.Retain)]
	NSNumber TlsVerifyServer { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable singleCallMode;
	[NullAllowed, Export ("singleCallMode", ArgumentSemantic.Retain)]
	NSNumber SingleCallMode { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable shareUdpTransport;
	[NullAllowed, Export ("shareUdpTransport", ArgumentSemantic.Retain)]
	NSNumber ShareUdpTransport { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable unregOnDestroy;
	[NullAllowed, Export ("unregOnDestroy", ArgumentSemantic.Retain)]
	NSNumber UnregOnDestroy { get; set; }

	// @property (retain, nonatomic) NSArray * _Nullable dnsServers;
	[NullAllowed, Export ("dnsServers", ArgumentSemantic.Retain)]
	//[Verify (StronglyTypedNSArray)]
	NSObject[] DnsServers { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable useDnsSrv;
	[NullAllowed, Export ("useDnsSrv", ArgumentSemantic.Retain)]
	NSNumber UseDnsSrv { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable recordStereo;
	[NullAllowed, Export ("recordStereo", ArgumentSemantic.Retain)]
	NSNumber RecordStereo { get; set; }

	// @property (retain, nonatomic) NSString * _Nullable brandName;
	[NullAllowed, Export ("brandName", ArgumentSemantic.Retain)]
	string BrandName { get; set; }
}

// @interface SiprixAccData : NSObject
[BaseType (typeof(NSObject))]
interface SiprixAccData
{
	// @property (assign, nonatomic) int myAccId;
	[Export ("myAccId")]
	int MyAccId { get; set; }

	// @property (retain, nonatomic) NSString * _Nonnull sipServer;
	[Export ("sipServer", ArgumentSemantic.Retain)]
	string SipServer { get; set; }

	// @property (retain, nonatomic) NSString * _Nonnull sipExtension;
	[Export ("sipExtension", ArgumentSemantic.Retain)]
	string SipExtension { get; set; }

	// @property (retain, nonatomic) NSString * _Nonnull sipPassword;
	[Export ("sipPassword", ArgumentSemantic.Retain)]
	string SipPassword { get; set; }

	// @property (retain, nonatomic) NSString * _Nullable sipAuthId;
	[NullAllowed, Export ("sipAuthId", ArgumentSemantic.Retain)]
	string SipAuthId { get; set; }

	// @property (retain, nonatomic) NSString * _Nullable sipProxy;
	[NullAllowed, Export ("sipProxy", ArgumentSemantic.Retain)]
	string SipProxy { get; set; }

	// @property (retain, nonatomic) NSString * _Nullable displName;
	[NullAllowed, Export ("displName", ArgumentSemantic.Retain)]
	string DisplName { get; set; }

	// @property (retain, nonatomic) NSString * _Nullable userAgent;
	[NullAllowed, Export ("userAgent", ArgumentSemantic.Retain)]
	string UserAgent { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable expireTime;
	[NullAllowed, Export ("expireTime", ArgumentSemantic.Retain)]
	NSNumber ExpireTime { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable port;
	[NullAllowed, Export ("port", ArgumentSemantic.Retain)]
	NSNumber Port { get; set; }

	// @property (assign, nonatomic) SipTransport transport;
	[Export ("transport", ArgumentSemantic.Assign)]
	SipTransport Transport { get; set; }

	// @property (retain, nonatomic) NSString * _Nullable tlsCaCertPath;
	[NullAllowed, Export ("tlsCaCertPath", ArgumentSemantic.Retain)]
	string TlsCaCertPath { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable tlsUseSipScheme;
	[NullAllowed, Export ("tlsUseSipScheme", ArgumentSemantic.Retain)]
	NSNumber TlsUseSipScheme { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable rtcpMuxEnabled;
	[NullAllowed, Export ("rtcpMuxEnabled", ArgumentSemantic.Retain)]
	NSNumber RtcpMuxEnabled { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable iceEnabled;
	[NullAllowed, Export ("iceEnabled", ArgumentSemantic.Retain)]
	NSNumber IceEnabled { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable keepAliveTime;
	[NullAllowed, Export ("keepAliveTime", ArgumentSemantic.Retain)]
	NSNumber KeepAliveTime { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable rewriteContactIp;
	[NullAllowed, Export ("rewriteContactIp", ArgumentSemantic.Retain)]
	NSNumber RewriteContactIp { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable verifyIncomingCall;
	[NullAllowed, Export ("verifyIncomingCall", ArgumentSemantic.Retain)]
	NSNumber VerifyIncomingCall { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable forceSipProxy;
	[NullAllowed, Export ("forceSipProxy", ArgumentSemantic.Retain)]
	NSNumber ForceSipProxy { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable secureMedia;
	[NullAllowed, Export ("secureMedia", ArgumentSemantic.Retain)]
	NSNumber SecureMedia { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable transpPreferIPv6;
	[NullAllowed, Export ("transpPreferIPv6", ArgumentSemantic.Retain)]
	NSNumber TranspPreferIPv6 { get; set; }

	// @property (retain, nonatomic) NSString * _Nullable instanceId;
	[NullAllowed, Export ("instanceId", ArgumentSemantic.Retain)]
	string InstanceId { get; set; }

	// @property (retain, nonatomic) NSString * _Nullable ringTonePath;
	[NullAllowed, Export ("ringTonePath", ArgumentSemantic.Retain)]
	string RingTonePath { get; set; }

	// @property (retain, nonatomic) NSDictionary * _Nullable xheaders;
	[NullAllowed, Export ("xheaders", ArgumentSemantic.Retain)]
	NSDictionary Xheaders { get; set; }

	// @property (retain, nonatomic) NSDictionary * _Nullable xContactUriParams;
	[NullAllowed, Export ("xContactUriParams", ArgumentSemantic.Retain)]
	NSDictionary XContactUriParams { get; set; }

	// @property (retain, nonatomic) NSArray * _Nullable aCodecs;
	[NullAllowed, Export ("aCodecs", ArgumentSemantic.Retain)]
	//[Verify (StronglyTypedNSArray)]
	NSObject[] ACodecs { get; set; }

	// @property (retain, nonatomic) NSArray * _Nullable vCodecs;
	[NullAllowed, Export ("vCodecs", ArgumentSemantic.Retain)]
	//[Verify (StronglyTypedNSArray)]
	NSObject[] VCodecs { get; set; }

	// -(NSDictionary * _Nonnull)toDictionary;
	[Export("toDictionary")]
	//[Verify (MethodToProperty)]
	NSDictionary ToDictionary();// { get; }

	// -(void)fromDictionary:(NSDictionary * _Nonnull)dictionary;
	[Export ("fromDictionary:")]
	void FromDictionary (NSDictionary dictionary);
}

// @interface SiprixDestData : NSObject
[BaseType (typeof(NSObject))]
interface SiprixDestData
{
	// @property (assign, nonatomic) int myCallId;
	[Export ("myCallId")]
	int MyCallId { get; set; }

	// @property (assign, nonatomic) int fromAccId;
	[Export ("fromAccId")]
	int FromAccId { get; set; }

	// @property (retain, nonatomic) NSString * _Nonnull toExt;
	[Export ("toExt", ArgumentSemantic.Retain)]
	string ToExt { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable withVideo;
	[NullAllowed, Export ("withVideo", ArgumentSemantic.Retain)]
	NSNumber WithVideo { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable inviteTimeoutSec;
	[NullAllowed, Export ("inviteTimeoutSec", ArgumentSemantic.Retain)]
	NSNumber InviteTimeoutSec { get; set; }

	// @property (retain, nonatomic) NSDictionary * _Nullable xheaders;
	[NullAllowed, Export ("xheaders", ArgumentSemantic.Retain)]
	NSDictionary Xheaders { get; set; }

	// @property (retain, nonatomic) NSString * _Nullable displName;
	[NullAllowed, Export ("displName", ArgumentSemantic.Retain)]
	string DisplName { get; set; }
}

// @interface SiprixSubscrData : NSObject
[BaseType (typeof(NSObject))]
interface SiprixSubscrData
{
	// @property (assign, nonatomic) int mySubscrId;
	[Export ("mySubscrId")]
	int MySubscrId { get; set; }

	// @property (assign, nonatomic) int fromAccId;
	[Export ("fromAccId")]
	int FromAccId { get; set; }

	// @property (retain, nonatomic) NSString * _Nonnull toExt;
	[Export ("toExt", ArgumentSemantic.Retain)]
	string ToExt { get; set; }

	// @property (retain, nonatomic) NSString * _Nonnull mimeSubtype;
	[Export ("mimeSubtype", ArgumentSemantic.Retain)]
	string MimeSubtype { get; set; }

	// @property (retain, nonatomic) NSString * _Nonnull eventType;
	[Export ("eventType", ArgumentSemantic.Retain)]
	string EventType { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable expireTime;
	[NullAllowed, Export ("expireTime", ArgumentSemantic.Retain)]
	NSNumber ExpireTime { get; set; }
}

// @interface SiprixMsgData : NSObject
[BaseType (typeof(NSObject))]
interface SiprixMsgData
{
	// @property (assign, nonatomic) int myMessageId;
	[Export ("myMessageId")]
	int MyMessageId { get; set; }

	// @property (assign, nonatomic) int fromAccId;
	[Export ("fromAccId")]
	int FromAccId { get; set; }

	// @property (retain, nonatomic) NSString * _Nonnull toExt;
	[Export ("toExt", ArgumentSemantic.Retain)]
	string ToExt { get; set; }

	// @property (retain, nonatomic) NSString * _Nonnull body;
	[Export ("body", ArgumentSemantic.Retain)]
	string Body { get; set; }
}

// @interface SiprixHoldData : NSObject
[BaseType (typeof(NSObject))]
interface SiprixHoldData
{
	// @property (assign, nonatomic) HoldState holdState;
	[Export ("holdState", ArgumentSemantic.Assign)]
	HoldState HoldState { get; set; }
}

// @interface SiprixVideoStateData : NSObject
[BaseType (typeof(NSObject))]
interface SiprixVideoStateData
{
	// @property (assign, nonatomic) BOOL hasVideo;
	[Export ("hasVideo")]
	bool HasVideo { get; set; }
}

// @interface SiprixVideoData : NSObject
[BaseType (typeof(NSObject))]
interface SiprixVideoData
{
	// @property (retain, nonatomic) NSString * _Nullable noCameraImgPath;
	[NullAllowed, Export ("noCameraImgPath", ArgumentSemantic.Retain)]
	string NoCameraImgPath { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable framerateFps;
	[NullAllowed, Export ("framerateFps", ArgumentSemantic.Retain)]
	NSNumber FramerateFps { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable bitrateKbps;
	[NullAllowed, Export ("bitrateKbps", ArgumentSemantic.Retain)]
	NSNumber BitrateKbps { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable height;
	[NullAllowed, Export ("height", ArgumentSemantic.Retain)]
	NSNumber Height { get; set; }

	// @property (retain, nonatomic) NSNumber * _Nullable width;
	[NullAllowed, Export ("width", ArgumentSemantic.Retain)]
	NSNumber Width { get; set; }
}

// @interface SiprixPlayerData : NSObject
[BaseType (typeof(NSObject))]
interface SiprixPlayerData
{
	// @property (assign, nonatomic) int playerId;
	[Export ("playerId")]
	int PlayerId { get; set; }
}

// @interface SiprixDevicesNumbData : NSObject
[BaseType (typeof(NSObject))]
interface SiprixDevicesNumbData
{
	// @property (assign, nonatomic) int number;
	[Export ("number")]
	int Number { get; set; }
}

// @interface SiprixDeviceData : NSObject
[BaseType (typeof(NSObject))]
interface SiprixDeviceData
{
	// @property (retain, nonatomic) NSString * _Nonnull name;
	[Export ("name", ArgumentSemantic.Retain)]
	string Name { get; set; }

	// @property (retain, nonatomic) NSString * _Nonnull guid;
	[Export ("guid", ArgumentSemantic.Retain)]
	string Guid { get; set; }
}

// @protocol SiprixVideoFrame <NSObject>
/*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/[Protocol]
[BaseType (typeof(NSObject))]
interface SiprixVideoFrame
{
	// @required -(int)width;
	[Abstract]
	[Export ("width")]
	//[Verify (MethodToProperty)]
	int Width { get; }

	// @required -(int)height;
	[Abstract]
	[Export ("height")]
	//[Verify (MethodToProperty)]
	int Height { get; }

	// @required -(VideoFrameRotation)rotation;
	[Abstract]
	[Export ("rotation")]
	//[Verify (MethodToProperty)]
	VideoFrameRotation Rotation { get; }

	// @required -(void)convertToARGB:(VideoFrameRGBType)type dstBuffer:(uint8_t * _Nonnull)dstBuffer dstWidth:(int)dstWidth dstHeight:(int)dstHeight;
	//[Abstract] //TODO Video fix
	//[Export ("convertToARGB:dstBuffer:dstWidth:dstHeight:")]
	//unsafe void DstBuffer (VideoFrameRGBType type, byte* dstBuffer, int dstWidth, int dstHeight);
}

// @protocol SiprixVideoRendererDelegate <NSObject>
[Protocol, Model]
[BaseType (typeof(NSObject))]
interface SiprixVideoRendererDelegate
{
	// @required -(void)onFrame:(id<SiprixVideoFrame> _Nonnull)videoFrame;
	[Abstract]
	[Export ("onFrame:")]
	void OnFrame (SiprixVideoFrame videoFrame);
}

// @protocol SiprixEventDelegate <NSObject>
[Protocol, Model]
[BaseType (typeof(NSObject))]
interface SiprixEventDelegate
{
	// @required -(void)onTrialModeNotified;
	[Abstract]
	[Export ("onTrialModeNotified")]
	void OnTrialModeNotified ();

	// @required -(void)onDevicesAudioChanged;
	[Abstract]
	[Export ("onDevicesAudioChanged")]
	void OnDevicesAudioChanged ();

	// @required -(void)onAccountRegState:(NSInteger)accId regState:(RegState)regState response:(NSString * _Nonnull)response;
	[Abstract]
	[Export ("onAccountRegState:regState:response:")]
	void OnAccountRegState (nint accId, RegState regState, string response);

	// @required -(void)onSubscriptionState:(NSInteger)subscrId subscrState:(SubscrState)subscrState response:(NSString * _Nonnull)response;
	[Abstract]
	[Export ("onSubscriptionState:subscrState:response:")]
	void OnSubscriptionState (nint subscrId, SubscrState subscrState, string response);

	// @required -(void)onNetworkState:(NSString * _Nonnull)name netState:(NetworkState)netState;
	[Abstract]
	[Export ("onNetworkState:netState:")]
	void OnNetworkState (string name, NetworkState netState);

	// @required -(void)onPlayerState:(NSInteger)playerId playerState:(PlayerState)playerState;
	[Abstract]
	[Export ("onPlayerState:playerState:")]
	void OnPlayerState (nint playerId, PlayerState playerState);

	// @required -(void)onRingerState:(BOOL)started;
	[Abstract]
	[Export ("onRingerState:")]
	void OnRingerState (bool started);

	// @required -(void)onCallProceeding:(NSInteger)callId response:(NSString * _Nonnull)response;
	[Abstract]
	[Export ("onCallProceeding:response:")]
	void OnCallProceeding (nint callId, string response);

	// @required -(void)onCallTerminated:(NSInteger)callId statusCode:(NSInteger)statusCode;
	[Abstract]
	[Export ("onCallTerminated:statusCode:")]
	void OnCallTerminated (nint callId, nint statusCode);

	// @required -(void)onCallConnected:(NSInteger)callId hdrFrom:(NSString * _Nonnull)hdrFrom hdrTo:(NSString * _Nonnull)hdrTo withVideo:(BOOL)withVideo;
	[Abstract]
	[Export ("onCallConnected:hdrFrom:hdrTo:withVideo:")]
	void OnCallConnected (nint callId, string hdrFrom, string hdrTo, bool withVideo);

	// @required -(void)onCallIncoming:(NSInteger)callId accId:(NSInteger)accId withVideo:(BOOL)withVideo hdrFrom:(NSString * _Nonnull)from hdrTo:(NSString * _Nonnull)to;
	[Abstract]
	[Export ("onCallIncoming:accId:withVideo:hdrFrom:hdrTo:")]
	void OnCallIncoming (nint callId, nint accId, bool withVideo, string from, string to);

	// @required -(void)onCallDtmfReceived:(NSInteger)callId tone:(NSInteger)tone;
	[Abstract]
	[Export ("onCallDtmfReceived:tone:")]
	void OnCallDtmfReceived (nint callId, nint tone);

	// @required -(void)onCallSwitched:(NSInteger)callId;
	[Abstract]
	[Export ("onCallSwitched:")]
	void OnCallSwitched (nint callId);

	// @required -(void)onCallTransferred:(NSInteger)callId statusCode:(NSInteger)statusCode;
	[Abstract]
	[Export ("onCallTransferred:statusCode:")]
	void OnCallTransferred (nint callId, nint statusCode);

	// @required -(void)onCallRedirected:(NSInteger)origCallId relatedCallId:(NSInteger)relatedCallId referTo:(NSString * _Nonnull)referTo;
	[Abstract]
	[Export ("onCallRedirected:relatedCallId:referTo:")]
	void OnCallRedirected (nint origCallId, nint relatedCallId, string referTo);

	// @required -(void)onCallHeld:(NSInteger)callId holdState:(HoldState)holdState;
	[Abstract]
	[Export ("onCallHeld:holdState:")]
	void OnCallHeld (nint callId, HoldState holdState);

	// @required -(void)onMessageSentState:(NSInteger)messageId success:(BOOL)success response:(NSString * _Nonnull)response;
	[Abstract]
	[Export ("onMessageSentState:success:response:")]
	void OnMessageSentState (nint messageId, bool success, string response);

	// @required -(void)onMessageIncoming:(NSInteger)accId hdrFrom:(NSString * _Nonnull)hdrFrom body:(NSString * _Nonnull)body;
	[Abstract]
	[Export ("onMessageIncoming:hdrFrom:body:")]
	void OnMessageIncoming (nint accId, string hdrFrom, string body);
}

// @interface SiprixModule : NSObject
[BaseType (typeof(NSObject))]
interface SiprixModule
{
	// -(int)initialize:(id<SiprixEventDelegate> _Nonnull)delegate iniData:(SiprixIniData * _Nonnull)iniData;
	[Export ("initialize:iniData:")]
	int Initialize (SiprixEventDelegate @delegate, SiprixIniData iniData);

	// -(int)unInitialize;
	[Export("unInitialize")]
	//[Verify (MethodToProperty)]
	int UnInitialize();// { get; }

	// -(NSString * _Nonnull)version;
	[Export("version")]
	//[Verify (MethodToProperty)]
	string Version();// { get; }

	// -(NSString * _Nonnull)homeFolder;
	[Export("homeFolder")]
	//[Verify (MethodToProperty)]
	string HomeFolder();// { get; }

	// -(int)versionCode;
	[Export("versionCode")]
	//[Verify (MethodToProperty)]
	int VersionCode();// { get; }

	// -(void)enableCallKit:(BOOL)enable;
	[Export ("enableCallKit:")]
	void EnableCallKit (bool enable);

        // -(void)activateSession:(AVAudioSession * _Nonnull)session;
        //[Export ("activateSession:")]	 //TODO CallKit
        //void ActivateSession (AVAudioSession session);

        // -(void)deactivateSession:(AVAudioSession * _Nonnull)session;
        //[Export ("deactivateSession:")]	//TODO CallKit
        //void DeactivateSession (AVAudioSession session);

        // -(BOOL)overrideAudioOutputToSpeaker:(BOOL)on;
        [Export ("overrideAudioOutputToSpeaker:")]
	bool OverrideAudioOutputToSpeaker (bool on);

	// -(BOOL)routeAudioToBluetoth;
	[Export("routeAudioToBluetoth")]
	//[Verify (MethodToProperty)]
	bool RouteAudioToBluetoth();// { get; }

	// -(BOOL)routeAudioToBuiltIn;
	[Export("routeAudioToBuiltIn")]
	//[Verify (MethodToProperty)]
	bool RouteAudioToBuiltIn();// { get; }

	// -(int)accountAdd:(SiprixAccData * _Nonnull)accData;
	[Export ("accountAdd:")]
	int AccountAdd (SiprixAccData accData);

	// -(int)accountUpdate:(SiprixAccData * _Nonnull)accData accId:(int)accId;
	[Export ("accountUpdate:accId:")]
	int AccountUpdate (SiprixAccData accData, int accId);

	// -(int)accountRegister:(int)accId expireTime:(int)expireTime;
	[Export ("accountRegister:expireTime:")]
	int AccountRegister (int accId, int expireTime);

	// -(int)accountUnRegister:(int)accId;
	[Export ("accountUnRegister:")]
	int AccountUnRegister (int accId);

	// -(int)accountDelete:(int)accId;
	[Export ("accountDelete:")]
	int AccountDelete (int accId);

	// -(NSString * _Nonnull)accountGenInstId;
	[Export("accountGenInstId")]
	//[Verify (MethodToProperty)]
	string AccountGenInstId();// { get; }

	// -(int)callInvite:(SiprixDestData * _Nonnull)destData;
	[Export ("callInvite:")]
	int CallInvite (SiprixDestData destData);

	// -(int)callReject:(int)callId statusCode:(int)statusCode;
	[Export ("callReject:statusCode:")]
	int CallReject (int callId, int statusCode);

	// -(int)callAccept:(int)callId withVideo:(BOOL)withVideo;
	[Export ("callAccept:withVideo:")]
	int CallAccept (int callId, bool withVideo);

	// -(int)callHold:(int)callId;
	[Export ("callHold:")]
	int CallHold (int callId);

	// -(int)callGetHoldState:(int)callId holdState:(SiprixHoldData * _Nonnull)data;
	[Export ("callGetHoldState:holdState:")]
	int CallGetHoldState (int callId, SiprixHoldData data);

	// -(int)callGetVideoState:(int)callId hasVideo:(SiprixVideoStateData * _Nonnull)data;
	[Export ("callGetVideoState:hasVideo:")]
	int CallGetVideoState (int callId, SiprixVideoStateData data);

	// -(int)callMuteMic:(int)callId mute:(BOOL)mute;
	[Export ("callMuteMic:mute:")]
	int CallMuteMic (int callId, bool mute);

	// -(int)callMuteCam:(int)callId mute:(BOOL)mute;
	[Export ("callMuteCam:mute:")]
	int CallMuteCam (int callId, bool mute);

	// -(int)callSendDtmf:(int)callId dtmfs:(NSString * _Nonnull)dtmfs durationMs:(int)durationMs intertoneGapMs:(int)intertoneGapMs method:(DtmfMethod)method;
	[Export ("callSendDtmf:dtmfs:durationMs:intertoneGapMs:method:")]
	int CallSendDtmf (int callId, string dtmfs, int durationMs, int intertoneGapMs, DtmfMethod method);

	// -(int)callSendDtmf:(int)callId dtmfs:(NSString * _Nonnull)dtmfs;
	[Export ("callSendDtmf:dtmfs:")]
	int CallSendDtmf (int callId, string dtmfs);

	// -(int)callPlayFile:(int)callId pathToMp3File:(NSString * _Nonnull)pathToMp3File loop:(BOOL)loop playerData:(SiprixPlayerData * _Nonnull)data;
	[Export ("callPlayFile:pathToMp3File:loop:playerData:")]
	int CallPlayFile (int callId, string pathToMp3File, bool loop, SiprixPlayerData data);

	// -(int)callStopPlayFile:(int)playerId;
	[Export ("callStopPlayFile:")]
	int CallStopPlayFile (int playerId);

	// -(int)callRecordFile:(int)callId pathToMp3File:(NSString * _Nonnull)pathToMp3File;
	[Export ("callRecordFile:pathToMp3File:")]
	int CallRecordFile (int callId, string pathToMp3File);

	// -(int)callStopRecordFile:(int)callId;
	[Export ("callStopRecordFile:")]
	int CallStopRecordFile (int callId);

	// -(int)callTransferBlind:(int)callId toExt:(NSString * _Nonnull)toExt;
	[Export ("callTransferBlind:toExt:")]
	int CallTransferBlind (int callId, string toExt);

	// -(int)callTransferAttended:(int)fromCallId toCallId:(int)toCallId;
	[Export ("callTransferAttended:toCallId:")]
	int CallTransferAttended (int fromCallId, int toCallId);

	// -(int)callBye:(int)callId;
	[Export ("callBye:")]
	int CallBye (int callId);

	// -(int)callSetVideoRenderer:(int)callId renderer:(id<SiprixVideoRendererDelegate> _Nullable)renderer;
	[Export ("callSetVideoRenderer:renderer:")]
	int CallSetVideoRenderer (int callId, [NullAllowed] SiprixVideoRendererDelegate renderer);

	// -(NSString * _Nonnull)callGetSipHeader:(int)callId hdrName:(NSString * _Nonnull)hdrName;
	[Export ("callGetSipHeader:hdrName:")]
	string CallGetSipHeader (int callId, string hdrName);

	// -(int)switchCamera;
	[Export("switchCamera")]
	//[Verify (MethodToProperty)]
	int SwitchCamera();// { get; }

	// -(int)callSetVideoWindow:(int)callId view:(UIView * _Nullable)view;
	[Export ("callSetVideoWindow:view:")]
	int CallSetVideoWindow (int callId, [NullAllowed] UIView view);

	// -(UIView * _Nonnull)createVideoWindow;
	[Export("createVideoWindow")]
	//[Verify (MethodToProperty)]
	UIView CreateVideoWindow();// { get; }

	// -(int)mixerSwitchCall:(int)callId;
	[Export ("mixerSwitchCall:")]
	int MixerSwitchCall (int callId);

	// -(int)mixerMakeConference;
	[Export("mixerMakeConference")]
	//[Verify (MethodToProperty)]
	int MixerMakeConference();// { get; }

	// -(int)dvcSetVideoParams:(SiprixVideoData * _Nonnull)vdoData;
	[Export ("dvcSetVideoParams:")]
	int DvcSetVideoParams (SiprixVideoData vdoData);

	// -(int)subscrCreate:(SiprixSubscrData * _Nonnull)subscrData;
	[Export ("subscrCreate:")]
	int SubscrCreate (SiprixSubscrData subscrData);

	// -(int)subscrDestroy:(int)subscrId;
	[Export ("subscrDestroy:")]
	int SubscrDestroy (int subscrId);

	// -(int)messageSend:(SiprixMsgData * _Nonnull)msgData;
	[Export ("messageSend:")]
	int MessageSend (SiprixMsgData msgData);

	// -(NSString * _Nonnull)getErrorText:(int)errCode;
	[Export ("getErrorText:")]
	string GetErrorText (int errCode);

	// -(void)dealloc;
	[Export ("dealloc")]
	void Dealloc ();
}
