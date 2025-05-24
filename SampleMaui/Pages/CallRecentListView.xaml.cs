namespace SampleMaui.Pages;

public partial class CallRecentListView : ContentView
{
    readonly Siprix.ObjModel objModel_;
    readonly Siprix.DestData data_ = new();

    public delegate void CancelHandler();
    public event CancelHandler? OnCancel;

    public CallRecentListView(Siprix.ObjModel om)
	{
		InitializeComponent();
        objModel_ = om;

        lbCdrs.BindingContext = om.Cdrs;
        cbAccounts.BindingContext = om.Accounts;
        lbCdrs.SelectionChanged += CdrsList_SelectionChanged;
        txDestExt.TextChanged += DestExt_TextChanged;

        objModel_.Accounts.Collection.CollectionChanged += (_, _) => OnAccountsList_CollectionChanged();
        OnAccountsList_CollectionChanged();
    }

    private void DestExt_TextChanged(object? sender, TextChangedEventArgs e)
    {
        btnVideoCall.IsEnabled = btnAudioCall.IsEnabled = txDestExt.Text.Length != 0;
    }

    private void OnAccountsList_CollectionChanged()
    {
        bool accountsExists = (objModel_.Accounts.Collection.Count != 0);
        tbErrText.IsVisible = !accountsExists;
        btnAudioCall.IsEnabled = accountsExists;
        btnVideoCall.IsEnabled = accountsExists;
        cbAccounts.IsEnabled = accountsExists;
        txDestExt.IsEnabled = accountsExists;
    }

    private void CdrsList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (lbCdrs.SelectedItem is Siprix.CdrModel cdrModel) {
            txDestExt.Text = cdrModel.RemoteExt;
            cbAccounts.SelectedItem = objModel_.Accounts.FindByUri(cdrModel.AccUri);
        }
    }

    private void ButtonMenu_Click(object sender, EventArgs e)
    {
        Button? menuBtn = sender as Button;
        if (menuBtn?.Parent is not HorizontalStackLayout parentLayout) return;
        foreach (var ctrl in parentLayout.Children)
        {
            if (ctrl is HorizontalStackLayout menuLayout)
                menuLayout.IsVisible = !menuLayout.IsVisible;
        }
    }

    void RecentCallDelete_Click(object sender, EventArgs e)
    {
        Button? editBtn = sender as Button;
        if (editBtn?.BindingContext is Siprix.CdrModel item) 
            objModel_.Cdrs.Delete(item);
    }

    void ShowDialpad_Click(object sender, EventArgs e)
    {
#if ANDROID
        txDestExt.Keyboard = (txDestExt.Keyboard == Keyboard.Default) 
                            ? Keyboard.Numeric : Keyboard.Default;
#else
        gridDialpad.IsVisible = !gridDialpad.IsVisible;
#endif
    }
    void DtmfSend_Click(object sender, EventArgs e)
    {
        if (sender is Button dtmfButton)
            txDestExt.Text += dtmfButton.Text;
    }

    void DestExt_Focused(object sender, EventArgs e)
    {
        gridDialpad.IsVisible = false;
    }

    void AddCall_Click(object sender, EventArgs e)
    {
        Invite(false);
    }
    void AddVideoCall_Click(object sender, EventArgs e)
    {
        Invite(true);
    }
    
    void Invite(bool withVideo)
    {
        //Check empty
        if (string.IsNullOrEmpty(txDestExt.Text) ||
            (cbAccounts.SelectedItem == null))
            return;

        //Get data from controls
        data_.ToExt = txDestExt.Text;
        data_.FromAccId = ((Siprix.AccountModel)cbAccounts.SelectedItem).ID;
        data_.WithVideo = withVideo;

        //Try to make call
        int err = objModel_.Calls.Invite(data_);
        if (err != Siprix.ErrorCode.kNoErr)
        {
            tbErrText.Text = objModel_.ErrorText(err);
            tbErrText.IsVisible = true;
        }
        else
        {
            txDestExt.Text = "";
            tbErrText.IsVisible = false;
            OnCancel?.Invoke();
        }
    }

    void Cancel_Click(object sender, EventArgs e)
    {
        OnCancel?.Invoke();
    }

    public void SetDialogMode(bool dialogMode)
    {
        btnCancel.IsVisible = dialogMode;
        brdShadow.StrokeThickness = dialogMode ? 1 : 0;
        brdShadow.Margin = new Thickness(dialogMode ? 10 : 0);
    }
}