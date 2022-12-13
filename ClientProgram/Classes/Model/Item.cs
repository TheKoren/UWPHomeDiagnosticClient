namespace ClientProgram
{
    /// <summary>
    /// Wrapper for items from the Fridge.
    /// </summary>
    public class Item : ObservableObject
    {
        private string name;
        /// <summary>
        /// The Name shall be unique for every Item, no need for ID.
        /// </summary>
        public string Name
        { 
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    Notify(nameof(Name));
                }
            }
        }

        private int amount;
        /// <summary>
        /// The amount of the given item.
        /// </summary>
        public int Amount
        {
            get => amount;
            set
            {
                if (amount != value)
                {
                    amount = value;
                    Notify(nameof(Amount));
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Item's name</param>
        /// <param name="amount">Item's amount</param>
        public Item(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
