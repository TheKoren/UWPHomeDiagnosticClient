using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;


namespace ClientProgram
{
    /// <summary>
    /// This class implements ICommand. Used for decreasing the amount property of an Item object.
    /// </summary>
    public class DecreaseItemAmountCommand : ICommand
    {
        private FridgeVM ViewModel; 
        private Info Info;
        private ClientCalls ClientCallsInstance;

        /// <summary>
        /// EventHandler for command changing.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="viewModel">FridgeVM viewModel, so it can access its properties.</param>
        public DecreaseItemAmountCommand(FridgeVM viewModel)
        {
            ViewModel = viewModel;
            Info = Info.GetInstance;
            ClientCallsInstance = ClientCalls.GetInstance;
            ClientCallsInstance.PropertyChanged += IsConnected_PropertyChanged;
        }

        private void IsConnected_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ClientCallsInstance.IsConnected))
                CanExecuteChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Commands can only be executed, if they fulfil some conditions.
        /// </summary>
        /// <param name="parameter">Parameter object</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return ClientCallsInstance.IsConnected;
        }

        /// <summary>
        /// On DecreaseItem button click, this function is invoked.
        /// </summary>
        /// <param name="parameter">Parameter object.</param>
        public void Execute(object parameter)
        {
            if(ClientCalls.GetInstance.IsConnected)
            {
                Item item = ViewModel.Items.SingleOrDefault(element => element.Name == (string)parameter);
                item.Amount -= 1;
                if (ClientCalls.GetInstance.PutItemAsync(item) != null)
                {
                    if (item.Amount == 0)
                    {
                        ViewModel.Items.Remove(item);
                        Info.Log($"[INFO] Item removed from Fridge List: {item.Name}");
                    }
                }
            }   
        }
    }
}
