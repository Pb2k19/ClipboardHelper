using WindowsClipboard = Windows.ApplicationModel.DataTransfer.Clipboard;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using Windows.ApplicationModel.DataTransfer;
using CommunityToolkit.Mvvm.Input;

namespace ClipboardHelper.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    public const char SpaceChar = ' ';
    private bool removeUnnecessarySpaces = true, removeAllSpaces, trim = true, toUpper, toLower;

    [ObservableProperty]
    private bool removeNewLines = true;

    public bool Trim
    {
        get => trim;
        set
        {
            if (value != trim)
            {
                trim = value;
                if (!value)
                    RemoveAllSpaces = false;

                OnPropertyChanged(nameof(Trim));
            }
        }
    }

    public bool RemoveUnnecessarySpaces
    {
        get => removeUnnecessarySpaces;
        set
        {
            if (value != removeUnnecessarySpaces)
            {
                removeUnnecessarySpaces = value;
                OnPropertyChanged(nameof(RemoveUnnecessarySpaces));
            }
        }
    }

    public bool RemoveAllSpaces
    {
        get => removeAllSpaces;
        set
        {
            if (value != removeAllSpaces)
            {
                removeAllSpaces = value;

                if (value)
                {
                    Trim = true;
                    RemoveUnnecessarySpaces = true;
                }

                OnPropertyChanged(nameof(RemoveAllSpaces));
            }
        }
    }

    public bool ToUpper
    {
        get => toUpper;
        set
        {
            if (value != toUpper)
            {
                toUpper = value;

                if (value && ToLower)
                    ToLower = false;

                OnPropertyChanged(nameof(ToUpper));
            }
        }
    }

    public bool ToLower
    {
        get => toLower;
        set
        {
            if (value != toLower)
            {
                toLower = value;

                if (value && ToUpper)
                    ToUpper = false;

                OnPropertyChanged(nameof(ToLower));
            }
        }
    }

    public MainPageViewModel()
    {
        WindowsClipboard.ContentChanged += new EventHandler<object>(OnClipboardChanged);
    }

    protected async void OnClipboardChanged(object? sender, object? e)
    {
        await ChangeClipboardAsync();
    }

    [RelayCommand]
    public static void ClearClipboard()
    {
        WindowsClipboard.Clear();
        WindowsClipboard.ClearHistory();
    }

    public async Task ChangeClipboardAsync()
    {
        DataPackageView dataPackageView = WindowsClipboard.GetContent();
        if (dataPackageView.Contains(StandardDataFormats.Text))
        {
            try
            {
                string text = await dataPackageView.GetTextAsync();
                string newText = EditText(text);

                if (text.Equals(newText))
                    return;

                DataPackage dataPackage = new();
                dataPackage.SetText(newText);
                WindowsClipboard.SetContent(dataPackage);
            }
            catch (COMException ex)
            {
                MessageBox.Show(ex.Message, "Error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }

    public string EditText(string input)
    {
        Span<char> newTextSpan = input.Length * sizeof(char) <= 1024 ? stackalloc char[input.Length] : new char[input.Length];

        if (ToLower)
            MemoryExtensions.ToLowerInvariant(input, newTextSpan);
        else if (ToUpper)
            MemoryExtensions.ToUpperInvariant(input, newTextSpan);
        else
            input.CopyTo(newTextSpan);

        if (Trim)
            newTextSpan = newTextSpan.Trim();

        if (RemoveNewLines)
        {
            newTextSpan.Replace('\r', ' ');
            newTextSpan.Replace('\n', ' ');
        }

        if (RemoveUnnecessarySpaces || RemoveAllSpaces)
            newTextSpan = RemoveUnnecessarySpacesFromSpan(newTextSpan, RemoveAllSpaces);

        return new(newTextSpan);
    }

    public static Span<char> RemoveUnnecessarySpacesFromSpan(Span<char> chars, bool removeAllSpaces = false)
    {
        bool isPerviousCharSpace = false;
        int length = 0;
        
        for (int i = 0; i < chars.Length; i++)
        {
            char current = chars[i];
            if (current == SpaceChar)
            {
                if (isPerviousCharSpace || removeAllSpaces)
                    continue;

                isPerviousCharSpace = true;
            }
            else
                isPerviousCharSpace = false;

            chars[length] = current;
            length++;
        }

        return chars[..length];
    }
}