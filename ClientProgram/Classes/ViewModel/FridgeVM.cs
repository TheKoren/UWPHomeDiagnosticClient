using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ClientProgram
{
    /// <summary>
    /// ViewModel for representing the working of the Fridge.
    /// </summary>
    public class FridgeVM : ObservableCollection<Item>
    {
        /// <summary>
        /// Model for the fridge items.
        /// </summary>
        public Item Model { get; internal set; }

        /// <summary>
        /// Command for decreasing item amount.
        /// </summary>
        public DecreaseItemAmountCommand DecreaseItemAmountCommand { get; internal set; }

        /// <summary>
        /// Command for increasing item amount.
        /// </summary>
        public IncreaseItemAmountCommand IncreaseItemAmountCommand { get; internal set; }

        /// <summary>
        /// Command for creating new items.
        /// </summary>
        public ItemCreationCommand ItemCreationCommand { get; internal set; }

        /// <summary>
        /// Event for signalling when the new item textbox content has changed.
        /// </summary>
        public event PropertyChangedEventHandler NewItemNameTextBoxChanged;

        private ObservableCollection<Item> items;
        /// <summary>
        /// ObservableCollection for Items.
        /// </summary>
        public ObservableCollection<Item> Items
        {
            get => items;
            set
            {
                if (items != value)
                {
                    items = value;
                }
            }
        }

        private string newItemName;
        /// <summary>
        /// Name of the new Item.
        /// </summary>
        public string NewItemName
        {
            get => newItemName;
            set
            {
                if (newItemName != value)
                {
                    newItemName = value;
                    NewItemNameTextBoxChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewItemName)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(NewItemName)));
                }
            }
        }

        private static FridgeVM instance;
        /// <summary>
        /// Getter for the singleton instance.
        /// </summary>
        public static FridgeVM GetInstance
        {
            get => instance ?? (instance = new FridgeVM());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private FridgeVM()
        {
            ItemCreationCommand = new ItemCreationCommand(this);
            NewItemName = "";
            DecreaseItemAmountCommand = new DecreaseItemAmountCommand(this);
            IncreaseItemAmountCommand = new IncreaseItemAmountCommand(this);

            Items = new ObservableCollection<Item>();
            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }
        }
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }
    }
}
