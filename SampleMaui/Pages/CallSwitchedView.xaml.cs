using System.ComponentModel;

namespace SampleMaui.Pages;

public partial class CallSwitchedView : ContentView
{
    enum UiMode { eUndef, eMain, eDtmf, eTransferBlind, eTranferAtt };
    readonly Dictionary<Siprix.CallState, UiMode> uiModes_ = [];

    Siprix.CallModel? callModel_;
    readonly Siprix.ObjModel objModel_;
    readonly Siprix.CallsListModel calls_;
    readonly IDispatcherTimer callDurationTimer_;

    public delegate void AddCallHandler();
    public event AddCallHandler? OnAddCall;
    public CallSwitchedView(Siprix.ObjModel om)
    {
        InitializeComponent();

        objModel_ = om;
        calls_ = om.Calls;
        BindingContext = null;

        calls_.Collection.CollectionChanged += (_,_) => OnCallsList_CollectionChanged();
        calls_.PropertyChanged += OnCallsList_PropertyChanged;

        callDurationTimer_ = Dispatcher.CreateTimer();
        callDurationTimer_.Tick += OnCallDurationTimer_Tick;
        callDurationTimer_.Interval = TimeSpan.FromSeconds(1);

        //Android: Handle case when activity started by notification
        if (callModel_ != calls_.SwitchedCall)
        {
            OnCallsList_PropertyChanged(calls_,
                new PropertyChangedEventArgs(nameof(Siprix.CallsListModel.SwitchedCall)));

            OnCallsList_CollectionChanged();
        }
    }

    private void OnCallsList_CollectionChanged()
    {
        if(callDurationTimer_.IsRunning && (calls_.Collection.Count == 0)) 
            callDurationTimer_.Stop();else
        if (!callDurationTimer_.IsRunning && (calls_.Collection.Count > 0))
            callDurationTimer_.Start();

        ConferenceButton.IsEnabled = (calls_.Collection.Count > 1);//when have 2 or more calls
    }

    private void OnCallsList_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Siprix.CallsListModel.SwitchedCall))
        {
            if (callModel_ != null)
            {
                callModel_.PropertyChanged -= OnCall_PropertyChanged;
                callModel_.SetVideoWindow(IntPtr.Zero);
            }

            callModel_ = calls_.SwitchedCall;
            BindingContext = callModel_;

            ResetUiModes();
            UpdateVisibility();

            if (callModel_ != null)
            {
                callModel_.PropertyChanged += OnCall_PropertyChanged;
                //callModel_.SetVideoWindow(receivedVideoHost_[0].Hwnd);
            }

            //calls_.SetPreviowVideoWindow(previewVideoHost_.Hwnd);
        }

        if (e.PropertyName == nameof(Siprix.CallsListModel.ConfModeStarted))
        {
            ConferenceButton.Text = calls_.ConfModeStarted ? "End\nconference" : "Make\nconference";
            if (ConferenceButton.ImageSource is FontImageSource imgSrc)
                imgSrc.Color = calls_.ConfModeStarted ? Colors.Red : Colors.White;

            SetVideoWindowConfMode();
        }
    }

    private void OnCall_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Siprix.CallModel.CallState) ||
           (e.PropertyName == nameof(Siprix.CallModel.HoldState)))
        {
            UpdateVisibility();
        }
    }

    void ResetUiModes()
    {
        uiModes_[Siprix.CallState.Ringing] = UiMode.eUndef;
        uiModes_[Siprix.CallState.Connected] = UiMode.eMain;
        uiModes_[Siprix.CallState.Held] = UiMode.eMain;
    }

    UiMode GetUiMode(Siprix.CallState? state)
    {
        UiMode mode = UiMode.eUndef;
        if (state == null) return mode;

        uiModes_.TryGetValue(state.Value, out mode);
        return mode;
    }

    void SetUiMode(Siprix.CallState? state, UiMode mode)
    {
        if (state == null) return;

        if ((state == Siprix.CallState.Ringing) && (mode == UiMode.eMain))
            mode = UiMode.eUndef;

        uiModes_[state.Value] = mode;
    }
    
    void UpdateVisibility()
    {
        tbNameAndExt.IsVisible = (callModel_ != null);
        spDetails.IsVisible = (callModel_ != null);

        //Ringing
        bool isConnected = (callModel_ != null) && callModel_.IsConnected;
        bool isRinging = (callModel_ != null) && callModel_.IsRinging;
        bool isVideo = (callModel_ != null) && callModel_.WithVideo;
        spAcceptReject.IsVisible = isRinging;
        bnDtmfMode.IsEnabled = isConnected;

        //Video
        gridVideo.IsVisible = isVideo;

        //Connected display depending on input mode
        UiMode uiMode = GetUiMode(callModel_?.CallState);
        bnRedirect.IsVisible  = (uiMode == UiMode.eUndef) && isRinging;
        gridDtmf.IsVisible    = (uiMode == UiMode.eDtmf);
        gridMain.IsVisible    = (uiMode == UiMode.eMain);
        gridTransfer.IsVisible= (uiMode == UiMode.eTransferBlind);

        //bnMakeCall.IsVisible = (callModel_ == null);
        bnHangup.IsVisible = (callModel_ != null) && !isRinging;

        if (callModel_ != null)
            bnTransfer.Text = callModel_.IsRinging ? "Redirect" : "Transfer";
    }

    private void OnCallDurationTimer_Tick(object? sender, EventArgs e)
    {
        calls_.CalcDuration();
    }

    private void DtmfMode_Click(object sender, EventArgs e)
    {
        UiMode uiMode = GetUiMode(callModel_?.CallState);
        if (uiMode == UiMode.eDtmf)
        {
            //Hide DTMF keys if displayed                
            SetUiMode(callModel_?.CallState, UiMode.eMain);
        }
        else
        {
            //Show DTMF keys
            tbSentDtmf.Text = "";
            SetUiMode(callModel_?.CallState, UiMode.eDtmf);
        }
        UpdateVisibility();
    }

    private void DtmfSend_Click(object sender, EventArgs e)
    {
        if(sender is not Button btnSender) return;

        string tone = btnSender.Text;
        tbSentDtmf.Text += tone;
        callModel_?.SendDtmf(tone);
    }

    private void TransferBlindMode_Click(object sender, EventArgs e)
    {
        UiMode uiMode = GetUiMode(callModel_?.CallState);
        if (uiMode == UiMode.eTransferBlind)
        {
            //Hide transfer/redirect edit if displayed
            tbTransferToExt.Text = "";
            SetUiMode(callModel_?.CallState, UiMode.eMain);
        }
        else
        {
            //Show transfer/redirect edit if displayed
            SetUiMode(callModel_?.CallState, UiMode.eTransferBlind);
        }
        UpdateVisibility();
    }

    private void TransferBlind_Click(object sender, EventArgs e)
    {
        callModel_?.TransferBlind(tbTransferToExt.Text);
    }
    private void OnTransferTextChanged(object sender, TextChangedEventArgs e)
    {
        bnTransfer.IsEnabled = !string.IsNullOrWhiteSpace(tbTransferToExt.Text);
    }

    private async void PlayFile_Click(object sender, EventArgs e)
    {
        if (callModel_ == null) return;

        if (callModel_.IsFilePlaying) 
        {
            callModel_.StopPlayFile();
        }
        else
        {
            string pathToDemoFile = await Siprix.ObjModel.WriteAssetAndGetFilePath("music.mp3");
            callModel_.PlayFile(pathToDemoFile, false);
        }
    }

    private void RecordFile_Click(object sender, EventArgs e)
    {
        if (callModel_ == null) return;

        if (callModel_.IsFileRecording)
        {
            callModel_.StopRecordFile();
        }
        else
        {
            string recFile = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyyyMMdd_hhmmss.mp3");
            callModel_.RecordFile(recFile);
        }
    }

    private void Conference_Click(object sender, EventArgs e)
    {
        calls_.MakeConference();
    }

    private void SetVideoWindowConfMode()
    {/*
        //Set/unset video windows for other calls
        for (int i = 1; i < Math.Min(calls_.Collection.Count, receivedVideoHost_.Length); ++i)
        {
            calls_.Collection[i].SetVideoWindow(calls_.ConfModeStarted ? receivedVideoHost_[i].Hwnd : IntPtr.Zero);
            receivedVideoBorders_[i].Visibility = calls_.ConfModeStarted ? Visibility.Visible : Visibility.Collapsed;
        }

        //Hide rest video windows 
        for (int j = calls_.Collection.Count; j < receivedVideoHost_.Length; ++j)
        {
            receivedVideoBorders_[j].Visibility = Visibility.Collapsed;
        }

        if (!calls_.ConfModeStarted && calls_.SwitchedCall != null)
            calls_.SwitchedCall.SetVideoWindow(receivedVideoHost_[0].Hwnd);

        calls_.SetPreviowVideoWindow(previewVideoHost_.Hwnd);*/
    }


    private void ButtonMenu_Click(object sender, EventArgs e)
    {
        bool newVisible = !CloseMoreButtons.IsVisible;
        ConferenceButton.IsVisible    = newVisible;
        TransferAttButton.IsVisible   = newVisible;
        TransferBlindButton.IsVisible = newVisible;
        PlayFileButton.IsVisible      = newVisible;
        RecFileButton.IsVisible       = newVisible;
        SeparatorMore.IsVisible       = newVisible;
        CloseMoreButtons.IsVisible    = newVisible;
    }

    private void AddCall_Click(object sender, EventArgs e)
    {
        OnAddCall?.Invoke();
    }
}