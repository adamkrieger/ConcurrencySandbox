using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencySandbox.Work
{
    public class Waiter
    {
        public int DoWork(int ms)
        {
            var a = Wait(ms);

            var b = Wait(ms);

            return a + b;
        }

        public async Task<int> DoWorkAsync(int ms)
        {
            var a = Task.Run(() => Wait(ms));

            var b = Task.Run(() => Wait(ms));

            return await a + await b;
        }

        private static int Wait(int ms)
        {
            Thread.Sleep(ms);

            return 21;
        }
    }
}