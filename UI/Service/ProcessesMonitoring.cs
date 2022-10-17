using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class ProcessesMonitoring
    {
        private Client client = new Client();

        private List<int> currentProcessesId;

        public async Task StartMonitoring()
        {
            currentProcessesId = GetIdOfCurrentProcesses();
            await client.SendTheNewProcessList(GetTheProcessList());

            while (true)
            {
                Thread.Sleep(1000);
                var processesId = GetIdOfCurrentProcesses();
                if (!Enumerable.SequenceEqual(processesId, currentProcessesId))
                {
                    currentProcessesId = processesId;
                    await client.SendTheNewProcessList(GetTheProcessList());
                }
            }
        }

        private List<int> GetIdOfCurrentProcesses()
        {
            var list = new List<int>();
            foreach (var item in Process.GetProcesses())
            {
                list.Add(item.Id);
            }
            return list;
        }

        private List<(int, string)> GetTheProcessList()
        {
            var list = new List<(int, string)>();
            foreach (var process in Process.GetProcesses())
            {
                list.Add((process.Id, process.ProcessName));
            }
            return list;
        }
    }
}
