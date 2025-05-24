namespace SampleMaui.Pages;

public partial class CallsListPage : ContentPage
{
    readonly Siprix.ObjModel objModel_;
    readonly CallRecentListView callRecentListView_;
    readonly CallSwitchedView callSwitchedView_;
    public CallsListPage(Siprix.ObjModel om)
    {
		InitializeComponent();
        BindingContext = om.Calls;
        objModel_ = om;

        callSwitchedView_ = new CallSwitchedView(objModel_);
        callSwitchedView_.OnAddCall += OnCallAdd_Click;
        MainGrid.Children.Add(callSwitchedView_);
        MainGrid.SetRow(callSwitchedView_, 1);

        callRecentListView_ = new CallRecentListView(om);
        callRecentListView_.OnCancel += OnCallAddCancel_Click;
        MainGrid.Children.Add(callRecentListView_);
        MainGrid.SetRowSpan(callRecentListView_, 2);

        objModel_.Calls.Collection.CollectionChanged += (_, _) => OnCallsList_CollectionChanged();
        OnCallsList_CollectionChanged();
    }

    private void OnCallsList_CollectionChanged()
    {
        bool callsListEmpty = (objModel_.Calls.Collection.Count == 0);
        callRecentListView_.IsVisible = callsListEmpty;
        callRecentListView_.SetDialogMode(!callsListEmpty);

        callSwitchedView_.IsVisible = !callsListEmpty;
    }

    private void OnCallAdd_Click()
    {
        callRecentListView_.IsVisible = true;
    }

    private void OnCallAddCancel_Click()
    {
        callRecentListView_.IsVisible = false;
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