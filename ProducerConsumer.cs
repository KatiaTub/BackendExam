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
        //Used blocking queue without uppper bound because GenerateSampleData is slower than DecodeData 
        private BlockingCollection<ReceivedDataItem> DataQueue = new BlockingCollection<ReceivedDataItem>(new ConcurrentQueue<ReceivedDataItem>());

        public ProducerConsumer(CancellationToken i_Token)
        {
            try
            {
                m_ProducerTask = Task.Run(async () =>
                {
                    while (!i_Token.IsCancellationRequested)
                    {
                        var data = await GenerateSampleData(i_Token);
                        DataQueue.Add(new ReceivedDataItem(data), i_Token);
                    }
                }, i_Token);

                m_ConsumerTask = Task.Run(() =>
                {
                    while (!i_Token.IsCancellationRequested)
                    {
                        var data = DataQueue.Take(i_Token);
                        DecodeData(data.Data, i_Token);
                    }
                }, i_Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Stopping");
            }
        }

        //Added cancelation token to fuction so cancelation can stop mid process
        public async Task<string> GenerateSampleData(CancellationToken i_Token)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMilliseconds(m_Faker.Random.Int(300, 1000)), i_Token); //Simulate text generator in random interval between 300 ms to 1 sec
                return m_Faker.Lorem.Sentence(m_Faker.Random.Int(0, 100), m_Faker.Random.Int(0, 100));
                            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Stopping");
                return null;
            }
        }

        //Added cancelation token to fuction so cancelation can stop mid process
        public void DecodeData(string i_Data, CancellationToken i_Token)
        {
            try
            {
                var processingDelay = TimeSpan.FromMilliseconds(m_Faker.Random.Int(10, 300)); //Simulated text decoder that completed in 10 to 300 ms
                i_Token.WaitHandle.WaitOne(processingDelay);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Stopping");
            }
        }

        public Task WaitForCompletion() => Task.WhenAll(m_ProducerTask ?? Task.CompletedTask, m_ConsumerTask ?? Task.CompletedTask);
    }
}
