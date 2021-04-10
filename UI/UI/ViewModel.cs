using Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace UI
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ProcessesMonitoring monitoring = new ProcessesMonitoring();

        private Server server;

        public ObservableCollection<string> Processes { get; } = new ObservableCollection<string>();

        private AsyncCommand connectCommand;

        public AsyncCommand ConnectCommand
        {
            get
            {
                return connectCommand ??
                    (connectCommand = new AsyncCommand(async (object parameter) =>
                    {
                        await ShowProcesses();
                    }));
            }
        }

        private Task ShowProcesses()
        {
            server = new Server(this);
            _ = Task.Run(() => server.Start());
            _ = Task.Run(() => monitoring.StartMonitoring());
            return Task.CompletedTask;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
