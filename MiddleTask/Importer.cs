using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using MiddleTask.Data;
using MiddleTask.Logging;
using Newtonsoft.Json;
using Ninject;

namespace MiddleTask
{
    public class Importer
    {
        public Importer()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _logger = Program.Kernel.Get<Logger>();

            _batchBlock = new BatchBlock<string>(100, new GroupingDataflowBlockOptions
            {
                BoundedCapacity = 200,
                CancellationToken = _cancellationTokenSource.Token
            });

            var dataflowBlockOptions = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = int.Parse(
                    File.ReadAllLines("config.txt")[0].Split("=")[1]),
                CancellationToken = _cancellationTokenSource.Token
            };

            _actionBlock = new ActionBlock<string[]>(strings =>
            {

                var peoples = strings.Select(JsonConvert.DeserializeObject<People>);
                var peopleRepository = new PeopleRepository();

                foreach (var people in peoples)
                {
                    var existPeople = peopleRepository.GetPeople(people.Id);

                    if (existPeople == null)
                    {
                        peopleRepository.AddPeople(people);
                        _logger.Log($" [{DateTime.Now:F}] INSERT row with Id {people.Id}");
                    }
                    else
                    {
                        if ((existPeople.DateChange - people.DateChange).Seconds < 0)
                        {
                            peopleRepository.UpdatePeople(people);
                            _logger.Log($" [{DateTime.Now:F}] UPDATE row with Id {people.Id}");
                        }
                    }
                }
            }, dataflowBlockOptions);

            _batchBlock.LinkTo(_actionBlock);

            _batchBlock.Completion.ContinueWith(task => _actionBlock.Complete());
        }

        private readonly BatchBlock<string> _batchBlock;
        private readonly ActionBlock<string[]> _actionBlock;
        private readonly Logger _logger;

        private readonly CancellationTokenSource _cancellationTokenSource;

        public void StartImport(string pathToFileData)
        {
            var fileStream = File.OpenRead(pathToFileData);
            var reader = new StreamReader(fileStream);

            while (!reader.EndOfStream)
            {
                var nextLine = reader.ReadLine();
                while (!_batchBlock.Post(nextLine))
                {
                }
            }

            _batchBlock.Complete();
            _actionBlock.Completion.Wait();
        }

        public void StopImport()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
