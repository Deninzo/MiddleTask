using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.DependencyInjection;
using MiddleTask.Data;
using MiddleTask.Logging;
using MiddleTask.Logging.Writers;
using Newtonsoft.Json;
using Ninject;

namespace MiddleTask
{
    class Program
    {
        public static IKernel Kernel;

        static void Main(string[] args)
        {
            CompositionRoot();

            var importer = new Importer();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (Console.ReadKey().Key != ConsoleKey.Escape) continue;
                    importer.StopImport();
                    Environment.Exit(0);
                    break;
                }
            });

            importer.StartImport("data.txt");
        }

        public static void CompositionRoot()
        {
            Kernel = new StandardKernel();

            Kernel.Bind<IWriter>().To<ConsoleWriter>();
            Kernel.Bind<IWriter>().To<FileWriter>();
            Kernel.Bind<Logger>().ToSelf();
            Kernel.Bind<PeopleRepository>().ToSelf();
        }
    }
}
