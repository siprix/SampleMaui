namespace SampleMaui.Pages;

public partial class MessagesPage : ContentPage
{
    readonly Siprix.ObjModel objModel_;
    public MessagesPage(Siprix.ObjModel om)
	{
		InitializeComponent();
        objModel_ = om;

        lbMessages.BindingContext = om.Messages;
        lbMessages.SelectionChanged += MessageList_SelectionChanged;

        cbMsgAccounts.BindingContext = om.Accounts;
    }

    private void MessageSend_Click(object sender, EventArgs e)
    {
        //Check empty
        if (string.IsNullOrEmpty(txMsgBody.Text) ||
            string.IsNullOrEmpty(txMsgDestExt.Text) ||
            (cbMsgAccounts.SelectedItem == null)) return;

        //Get data from controls
        Siprix.MsgData msgData = new();
        msgData.ToExt = txMsgDestExt.Text;
        msgData.FromAccId = ((Siprix.AccountModel)cbMsgAccounts.SelectedItem).ID;
        msgData.Body = txMsgBody.Text;

        txMsgBody.Text = "";

        //Try to send
        objModel_.Messages.Send(msgData);
    }

    async private void MessageDelete_Click(object sender, EventArgs e)
    {
        Button? editBtn = sender as Button;
        if (editBtn?.BindingContext is not Siprix.MessageModel msg) return;

        //Confirm deleting
        bool answerYes = await DisplayAlert("Confirmation", "Confirm deleting message?", "Yes", "No");
        if (!answerYes) return;

        //Delete
        int err = objModel_.Messages.Delete(msg);
        if (err != Siprix.ErrorCode.kNoErr)
        {
            await DisplayAlert("Alert", objModel_.ErrorText(err), "OK");
        }
    }

    private void MessageList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (lbMessages.SelectedItem is Siprix.MessageModel msgModel)
        {
            txMsgDestExt.Text = msgModel.ToExt;
            cbMsgAccounts.SelectedItem = objModel_.Accounts.FindByUri(msgModel.AccUri);
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
}