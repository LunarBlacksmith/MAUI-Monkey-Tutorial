namespace MonkeyFinder.ViewModel;

#region With CommunityToolkit.Mvvm
// This allows code (written in the below region) to be automatically generated
// using community-driven, Microsoft-supported libraries that do cool stuff.


public partial class BaseViewModel : ObservableObject
//<-- has to be partial class since the full code generated automatically using the toolkit can be shared in the class
{
    /* Check in your solution!
     * "MonkeyFinder\Dependencies\netX.X-android\Analyzers\CommunityToolkit.Mvvm.SourceGenerators\
     * CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator\"
     * Open the .g.cs files and see the generated code!
     */

    // Implementing the class then gives us access to all the generated code methods, such as SetProperty()
    public BaseViewModel()
    {
        //SetProperty(ref _isBusy, true);
    }

    [ObservableProperty] // allows us to get our accessible public property back
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool _isBusy;

    [ObservableProperty]
    string _title;

    public bool IsNotBusy => !IsBusy;
}
#endregion

/* #region Without CommunityToolkit.Mvvm
 *  
    * Check in your solution!
    * "MonkeyFinder\Dependencies\netX.X-android\Analyzers\CommunityToolkit.Mvvm.SourceGenerators\
    * CommunityToolkit.Mvvm.SourceGenerators.INotifyPropertyChangedGenerator\"
    * Open the .g.cs file and see the generated code!
     
//[INotifyPropertyChanged] <-- Outdated and now instead should inherit from ObservableObject
public class BaseViewModel : INotifyPropertyChanged
{
    bool    _isBusy;
    string  _title;

    // public methods and properties allow MAUI to use DATA BINDING (e.g: Title="{Binding Title}")
    // in the XAML instead of hard coding values or instead of using coupled code (this way of doing
    // it abstracts each part of the code, such as View, ViewModel, and Model.) A benefit of this is
    // unit testing each of these specific parts without needing the other.
    public bool IsBusy
    {
        // fancy way of saying get {return _isBusy;}
        get => _isBusy;
        set
        {
            // if _isBusy is already equal to the value then don't do anything
            if (_isBusy == value)
            { return; }

            // otherwise set its value
            _isBusy = value;
            // then notify the user interface that this property has changed
            OnPropertyChanged();
            //OnPropertyChanged(nameof(IsBusy)); // <-- returns the string version of IsBusy property ("IsBusy")
            //OnPropertyChanged("IsBusy"); <-- same as above except above renames whenever the property name changes

            // since IsBusy is changed, so will IsNotBusy, and this is the easy, less-code-dense way of
            // notifying the change of the other property.
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }

    // easy way of implementing this same property that is the opposite of IsBusy without doing
    // the whole code block. Its OnPropertyChanged() method is also called in IsBusy since this property
    // is always the opposite of it - when it changes, so does IsNotBusy, by nature.
    public bool IsNotBusy => !IsBusy;

    public string Title
    {
        get => _title;
        set
        {
            if (_title == value) 
            { return; }
            _title = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    // [CallerMemberName]string name = null <-- when you don't pass the method a parameter, its
    // default gets the Caller's Name as a string and provides the method that instead. (e.g: If 
    // IsBusy called OnPropertyChanged(); then the default would return the same as OnPropertyChanged("IsBusy");
    // This 'search' for the caller's name happens at compile time, not runtime.
    public void OnPropertyChanged([CallerMemberName]string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
#endregion
*/