using System.Threading.Tasks;

namespace ConcurrencySandbox.Work
{
    public class Computations
    {
        public int DoWork(long cycles)
        {
            var a = Compute(cycles);

            var b = Compute(cycles);

            return a + b;
        }

        public async Task<int> DoWorkAsync(long cycles)
        {
            var a = Task.Run(() => Compute(cycles));

            var b = Task.Run(() => Compute(cycles));

            return await a + await b;
        }

        private static int Compute(long cycles)
        {
            for (var i = 0; i < cycles; i++) { }

            return 21;
        }
    }
}