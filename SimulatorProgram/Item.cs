using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorProgram
{
    /// <summary>
    /// Wrapper for items from the Fridge.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The Name shall be unique for every Item, no need for ID.
        /// </summary>
        public string Name;
        public int Amount;

        public Item(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
