using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using ConcurrencySandbox.Work;
using Newtonsoft.Json;
using NLog;

namespace ConcurrencySandbox.Controllers
{
    public class ComputeController : ApiController
    {
        public async Task<Dictionary<string, string>> Get([FromUri] long cycles)
        {
            var computations = new Computations();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            stopwatch.Restart();
            computations.DoWork(cycles);
            var inOrderSpan = stopwatch.Elapsed;

            stopwatch.Restart();
            await computations.DoWorkAsync(cycles);
            var asyncSpan = stopwatch.Elapsed;

            var returnDictionary = new Dictionary<string, string>
            {
                {"processors", Environment.ProcessorCount.ToString()},
                {"inOrderSpan", inOrderSpan.ToString()},
                {"asyncSpan", asyncSpan.ToString()}
            };

            var results = JsonConvert.SerializeObject(returnDictionary);
            LogManager.GetCurrentClassLogger().Log(LogLevel.Info, results);

            return returnDictionary;
        }
    }
}
