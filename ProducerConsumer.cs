using Bogus;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BackendExam
{
    class ProducerConsumer
    {
        private Task m_ProducerTask;
        private Task m_ConsumerTask;
        private Faker m_Faker = new Faker();

        private ConcurrentQueue<ReceivedDataItem> DataQueue = new ConcurrentQueue<ReceivedDataItem>();

        public ProducerConsumer(CancellationToken i_Token)
        {
            m_ProducerTask = Task.Run(async () =>
            {
                while (!i_Token.IsCancellationRequested)
                {
                    var data = await GenerateSampleData();
                    DataQueue.Enqueue(new ReceivedDataItem(data));
                }
            }, i_Token);

            m_ConsumerTask = Task.Run(() =>
            {
                while (!i_Token.IsCancellationRequested)
                {
                    //TODO: Implement
                    //Call DecodeData Method
                }
            }, i_Token);
        }

        public async Task<string> GenerateSampleData()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(m_Faker.Random.Int(300, 1000))); //Simulate text generator in random interval between 300 ms to 1 sec
            return m_Faker.Lorem.Sentence(m_Faker.Random.Int(0, 100), m_Faker.Random.Int(0, 100));
        }

        public void DecodeData(string i_Data)
        {
            var processingDelay = TimeSpan.FromMilliseconds(m_Faker.Random.Int(10, 300)); //Simulated text decoder that completed in 10 to 300 ms
            Thread.Sleep(processingDelay);
        }

        public Task WaitForCompletion() => Task.WhenAll(m_ProducerTask ?? Task.CompletedTask, m_ConsumerTask ?? Task.CompletedTask);
    }
}
