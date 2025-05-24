# SampleMaui
Project contains ready to use SIP VoIP Client application for Windows/Android/iOS, written on C# MAUI.
As SIP engine it uses Siprix SDK, included in binary form.

Application (Siprix) has ability to:

- Add multiple SIP accounts
- Send/receive multiple calls (Audio and Video)
- Manage calls with:
   - Hold
   - Mute microphone/camera
   - Play sound to call from mp3 file
   - Record received sound to file
   - Send/receive DTMF
   - Transfer
   - ...

Application's UI may not contain all the features, avialable in the SDK, they will be added later.

## Limitations

Siprix doesn't have any limitations and can work with all existing servers (PBX) supported SIP.
For testing app you need an account(s) credentials from a SIP service provider(s).
Some features may be not supported by all SIP providers.

Included Siprix SDK works in trial mode and has limited call duration - it drops call after 60sec.
Upgrading to a paid license removes this restriction, enabling calls of any length.

Please contact [sales@siprix-voip.com](mailto:sales@siprix-voip.com) for more details.

## More resources

Product web site: https://siprix-voip.com

Manual: https://docs.siprix-voip.com


## Screenshots

<a href="https://docs.siprix-voip.com/screenshots/Maui_Android_Accounts.png"  title="Accounts list Android">
<img src="https://docs.siprix-voip.com/screenshots/Maui_Android_AccountsMini.png" width="50"></a>
<a href="https://docs.siprix-voip.com/screenshots/Maui_Android_Calls.png"  title="Call in progress Android">
<img src="https://docs.siprix-voip.com/screenshots/Maui_Android_CallsMini.png" width="50"></a>
<a href="https://docs.siprix-voip.com/screenshots/Maui_Android_CallsRecent.png"  title="Recent calls Android">
<img src="https://docs.siprix-voip.com/screenshots/Maui_Android_CallsRecentMini.png" width="50"></a>
<a href="https://docs.siprix-voip.com/screenshots/Maui_Android_Messages.png"  title="Messages Android">
<img src="https://docs.siprix-voip.com/screenshots/Maui_Android_MessagesMini.png" width="50"></a>
<a href="https://docs.siprix-voip.com/screenshots/Maui_Android_Logs.png"  title="Logs Android">
<img src="https://docs.siprix-voip.com/screenshots/Maui_Android_LogsMini.png" width="50"></a>

<a href="https://docs.siprix-voip.com/screenshots/Maui_Windows_Accounts.png"  title="Accounts list Windows">
<img src="https://docs.siprix-voip.com/screenshots/Maui_Windows_AccountsMini.png" width="50"></a>
<a href="https://docs.siprix-voip.com/screenshots/Maui_Windows_Calls.png"  title="Call in progress Windows">
<img src="https://docs.siprix-voip.com/screenshots/Maui_Windows_CallsMini.png" width="50"></a>
<a href="https://docs.siprix-voip.com/screenshots/Maui_Windows_CallsRecent.png"  title="Recent calls Windows">
<img src="https://docs.siprix-voip.com/screenshots/Maui_Windows_CallsRecentMini.png" width="50"></a>
<a href="https://docs.siprix-voip.com/screenshots/Maui_Windows_Messages.png"  title="Messages Windows">
<img src="https://docs.siprix-voip.com/screenshots/Maui_Windows_MessagesMini.png" width="50"></a>
<a href="https://docs.siprix-voip.com/screenshots/Maui_Windows_Logs.png"  title="Logs Windows">
<img src="https://docs.siprix-voip.com/screenshots/Maui_Windows_LogsMini.png" width="50"></a>