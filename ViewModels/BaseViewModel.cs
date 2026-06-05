using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FitTrackGymApp.ViewModels;

public partial class BaseViewModel : INotifyPropertyChanged
{
    bool isBusy;
    string title;

    public bool IsBusy
    {
        get => isBusy;
        set
        {
            if (isBusy == value) return;
            isBusy = value;
            OnPropertyChanged();
        }
    }

    public string Title
    {
        get => title;
        set
        {
            if (title == value) return;
            title = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
