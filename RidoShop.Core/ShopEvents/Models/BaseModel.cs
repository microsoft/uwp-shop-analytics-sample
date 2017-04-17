using System.ComponentModel;

namespace ShopEvents.Models
{
    /// <summary>
    /// Used to reduce code redundancy - rather than having each class implement the INotifyPropertyChanged interface, they implement the BaseModel class
    /// </summary>
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}