using System;
using System.ComponentModel;
using System.Windows.Input;


namespace ClientProgram
{
    /// <summary>
    /// This class implements ICommand. Used for creating a new ItemObject.
    /// </summary>
    public class ItemCreationCommand : ICommand
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
        public ItemCreationCommand(FridgeVM viewModel)
        {
            ViewModel = viewModel;
            ViewModel.NewItemNameTextBoxChanged += NewItemName_PropertyChanged;
            Info = Info.GetInstance;
            ClientCallsInstance = ClientCalls.GetInstance;
            ClientCallsInstance.PropertyChanged += IsConnected_PropertyChanged;
        }

        private void NewItemName_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.NewItemName))
                CanExecuteChanged?.Invoke(this, e);
        }

        private void IsConnected_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ClientCallsInstance.IsConnected))
                CanExecuteChanged?.Invoke(this, e);
        }
        
        /// <summary>
        /// Commands can only be executed, if they fulfil some conditions.
        /// </summary>
        /// <param name="parameter">Parameter object.</param>
        /// <returns>Boolean if the textbox contains anything, and the simulator is connected.</returns>
        public bool CanExecute(object parameter)
        {
            return (ViewModel.NewItemName != "") && ClientCallsInstance.IsConnected;
        }

        /// <summary>
        /// On ItemCreation button click, this function is invoked.
        /// </summary>
        /// <param name="parameter">Parameter object</param>
        public void Execute(object parameter)
        {
            if(ClientCalls.GetInstance.IsConnected)
            {
                string name = ViewModel.NewItemName;

                if (ClientCalls.GetInstance.PostItemAsync(new Item(name, 1)) != null)
                {
                    ViewModel.Items.Add(new Item(name, 1));
                    Info.Log($"[INFO] Created item sent to the server: {name}");
                }
                ViewModel.NewItemName = "";
            }
        }
    }
}
