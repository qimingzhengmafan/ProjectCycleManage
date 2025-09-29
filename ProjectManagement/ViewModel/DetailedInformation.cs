using DrawerTest;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectManagement.ViewModel;

public class DetailedInformation:INotifyPropertyChanged
{

    private ObservableCollection<DrawerUIVM> _yourDataCollection;
    public ObservableCollection<DrawerUIVM> YourDataCollection
    {
        get => _yourDataCollection;
        set
        {
            _yourDataCollection = value;
            OnPropChanged("YourDataCollection");
        }
    }

    public DetailedInformation()
    {
        // 初始化数据
        YourDataCollection = new ObservableCollection<DrawerUIVM>();

        // 添加示例数据
        for (int i = 0; i < 5; i++)
        {
            YourDataCollection.Add(new DrawerUIVM()
            {


            });

        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropChanged(String Prop)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Prop));
    }

}