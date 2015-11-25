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
    public class WaitController : ApiController
    {
        public async Task<Dictionary<string, string>> Get([FromUri] int ms)
        {
            //Sanitize
            ms = ms > 2000 ? 1 : ms;

            var waiting = new Waiter();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            stopwatch.Restart();
            waiting.DoWork(ms);
            var inOrderSpan = stopwatch.Elapsed;

            stopwatch.Restart();
            await waiting.DoWorkAsync(ms);
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
