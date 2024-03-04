using ClipboardHelper.Pages;
using ClipboardHelper.Services.ConfigService;
using ClipboardHelper.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace ClipboardHelper;

public partial class MainWindow : Window
{
    private readonly MainPageViewModel mainPageViewModel;
    private readonly IConfigService configService;

    public MainWindow(MainPage mainPage, MainPageViewModel mainPageViewModel, IConfigService configService)
    {
        InitializeComponent();

        MainFrame.Navigate(mainPage);
        this.mainPageViewModel = mainPageViewModel;
        this.configService = configService;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        string serialized = System.Text.Json.JsonSerializer.Serialize(mainPageViewModel);
        configService.SetConfigProperty(Const.PreferencesKeys.MainPageViewModelSerializedKey, serialized);
        base.OnClosing(e);
    }
}