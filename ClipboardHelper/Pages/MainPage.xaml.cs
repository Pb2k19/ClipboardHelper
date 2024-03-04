using ClipboardHelper.ViewModels;
using System.Windows.Controls;

namespace ClipboardHelper.Pages;
/// <summary>
/// Interaction logic for MainPage.xaml
/// </summary>
public partial class MainPage : Page
{
    public MainPage(MainPageViewModel mainPageViewModel)
    {
        InitializeComponent();
        DataContext = mainPageViewModel;
    }
}