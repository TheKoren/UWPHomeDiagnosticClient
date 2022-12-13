using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;


namespace ClientProgram
{
    /// <summary>
    /// This class implements ICommand. Used for saving old data to a json file.
    /// </summary>
    public class SaveValuesCommand : ICommand
    {
        /// <summary>
        /// EventHandler for command changing.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Commands can only be executed, if they fulfil some conditions.
        /// </summary>
        /// <param name="parameter">Parameter object</param>
        /// <returns>Always true.</returns>
        public bool CanExecute(object parameter) => true;

        /// <summary>
        /// On SaveValues button click, this function is invoked.
        /// </summary>
        /// <param name="parameter">Parameter object.</param>
        public void Execute(object parameter)
        {
            List<Data> valueList = new List<Data>();
            foreach (var data in SensorValuesVM.GetInstance.Values)
            {
                valueList.Add(data);
            }
            var task = Task.Run(async () => await FileUtils.WriteToFile(valueList, "history.json"));
            _ = task.IsCompletedSuccessfully;
        }
    }
}
