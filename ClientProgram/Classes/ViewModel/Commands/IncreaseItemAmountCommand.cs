using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;


namespace ClientProgram
{
    /// <summary>
    /// This class implements ICommand. Used for increasing the amount property of an Item object.
    /// </summary>
    public class IncreaseItemAmountCommand : ICommand
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
        public IncreaseItemAmountCommand(FridgeVM viewModel)
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
        /// <returns>Boolean if the simulator is connected.</returns>
        public bool CanExecute(object parameter)
        {
            return ClientCallsInstance.IsConnected;
        }

        /// <summary>
        /// On IncreaseItem button click, this function is invoked.
        /// </summary>
        /// <param name="parameter">Parameter object</param>
        public void Execute(object parameter)
        {
            if(ClientCallsInstance.IsConnected)
            {
                Item item = ViewModel.Items.SingleOrDefault(element => element.Name == (string)parameter);
                item.Amount += 1;
                if (ClientCallsInstance.PutItemAsync(item) != null)
                {
                    Info.Log($"[INFO] Item updated in Fridge List: {item.Name}");
                }
            }
        }
    }
}
