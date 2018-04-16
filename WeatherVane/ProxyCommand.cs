using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WeatherVane
{
    public class ProxyCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the proxy function for CanExecute.
        /// </summary>
        /// <value>
        /// The proxy can execute.
        /// </value>
        public Func<object, bool> ProxyCanExecute { get; set; }

        /// <summary>
        /// Gets or sets the proxy function for Execute.
        /// </summary>
        /// <value>
        /// The proxy execute.
        /// </value>
        public Action<object> ProxyExecute { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyCommand"/> class.
        /// </summary>
        public ProxyCommand()
        {
            ProxyCanExecute = o => true;
            ProxyExecute = o => { };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyCommand"/> class.
        /// </summary>
        /// <param name="proxyCanExecute">The proxy can execute.</param>
        /// <param name="proxyExecute">The proxy execute.</param>
        public ProxyCommand(Func<object, bool> proxyCanExecute, Action<object> proxyExecute)
        {
            ProxyCanExecute = proxyCanExecute;
            ProxyExecute = proxyExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyCommand"/> class.
        /// </summary>
        /// <param name="proxyExecute">The proxy execute.</param>
        public ProxyCommand(Action<object> proxyExecute)
        {
            ProxyCanExecute = o => true;
            ProxyExecute = proxyExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        /// <returns>
        ///   <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return ProxyCanExecute.Invoke(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            ProxyExecute.Invoke(parameter);
        }

        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;
    }
}
