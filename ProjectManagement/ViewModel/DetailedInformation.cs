using DrawerTest;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectManagement.ViewModel;

public class DetailedInformation:INotifyPropertyChanged
{
    private ObservableCollection<DrawerUIVM> _dataCollection;
    public ObservableCollection<DrawerUIVM> DataCollection
    {
        get => _dataCollection;
        set
        {
            _dataCollection = value;
            OnPropChanged("DataCollection");
        }
    }



    public DetailedInformation()
    {

    }


    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropChanged(String Prop)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Prop));
    }

}