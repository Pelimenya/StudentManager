using System.ComponentModel;

namespace StudentManager.ViewModels;

public class StudentAttendanceViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private int _studentId;
    public int StudentId
    {
        get { return _studentId; }
        set
        {
            _studentId = value;
            OnPropertyChanged(nameof(StudentId));
        }
    }

    private string _fullName;
    public string FullName
    {
        get { return _fullName; }
        set
        {
            _fullName = value;
            OnPropertyChanged(nameof(FullName));
        }
    }

    private bool _isPresent;
    public bool IsPresent
    {
        get { return _isPresent; }
        set
        {
            _isPresent = value;
            OnPropertyChanged(nameof(IsPresent));
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}