using System.Net.NetworkInformation;
//using System.Net.Sockets;

namespace SampleMaui.Pages;

public partial class AccountAddPage : ContentPage
{
    readonly bool addNew_;
    readonly Siprix.AccData data_;
    readonly Siprix.ObjModel objModel_;
    public AccountAddPage(Siprix.ObjModel om, Siprix.AccData? data = null)
	{
		InitializeComponent();

        objModel_ = om;
        addNew_ = (data == null);
        data_ = data ?? new Siprix.AccData();        

        //Set data to controls
        tbSipServer.Text    = data_.SipServer;
        tbSipExtension.Text = data_.SipExtension;
        tbSipPassword.Text  = data_.SipPassword;
        tbExpireTime.Text   = data_.ExpireTime.ToString();
        tbDisplayName.Text  = data_.DisplayName;

        cbTransport.ItemsSource  = Enum.GetValues(typeof(Siprix.SipTransport));
        cbTransport.SelectedItem = data_.TranspProtocol;

        cbSecureMedia.ItemsSource = Enum.GetValues(typeof(Siprix.SecureMedia));
        cbSecureMedia.SelectedItem = data_.SecureMediaMode;

        AddItemsToBindAddressCombobox();

        //Set controls state
        this.Title = addNew_ ? "Add account" : "Edit account";
        tbSipServer.IsEnabled = addNew_;
        tbSipExtension.IsEnabled = addNew_;
        tbExpireTime.IsEnabled = addNew_;
        cbTransport.IsEnabled = addNew_;

        tbSipServer.Focus();
    }

	private void AddLocalAccount_Click(object sender, EventArgs e)
	{
	}

    private void ShowPassword_Click(object sender, EventArgs e)
    {
        tbSipPassword.IsPassword = !tbSipPassword.IsPassword;
    }

    async private void btnOK_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbSipServer.Text) ||
            string.IsNullOrEmpty(tbSipExtension.Text))
        {
            tbErrText.Text = "Fields `Server (PBX)` and `Extension` can't be empty";
            tbErrText.IsVisible = true;
            return;
        }

        //Get data from controls
        data_.SipServer = tbSipServer.Text;
        data_.SipExtension = tbSipExtension.Text;
        data_.SipPassword = tbSipPassword.Text;
        data_.ExpireTime = string.IsNullOrEmpty(tbExpireTime.Text) ? 0 : uint.Parse(tbExpireTime.Text);
        data_.DisplayName = tbDisplayName.Text;

        data_.RingToneFile = await Siprix.ObjModel.WriteAssetAndGetFilePath("ringtone.mp3");

        if (cbTransport.SelectedItem != null)
            data_.TranspProtocol = (Siprix.SipTransport)cbTransport.SelectedItem;

        if (cbSecureMedia.SelectedItem != null)
            data_.SecureMediaMode = (Siprix.SecureMedia)cbSecureMedia.SelectedItem;

        string? selectedBindAddr = cbBindAddr.SelectedItem?.ToString();
        if (!string.IsNullOrEmpty(selectedBindAddr))
            data_.TranspBindAddr = selectedBindAddr;

        //Try to add/update account
        int err = addNew_ ? objModel_.Accounts.Add(data_)
                          : objModel_.Accounts.Update(data_);
        if (err != Siprix.ErrorCode.kNoErr)
        {
            tbErrText.Text = objModel_.ErrorText(err);
            tbErrText.IsVisible = true;
            return;
        }

        await Navigation.PopModalAsync();
    }

    async private void btnCancel_Click(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    public void AddItemsToBindAddressCombobox()
    {/*
        cbBindAddr.Items.Add("");

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if ((item.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                (item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)) &&
                item.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        cbBindAddr.Items.Add(ip.Address.ToString());
                    }
                }
            }
        }*/
    }
}