using Bogus;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BackendExam
{
    class ProducerConsumer
    {
        private Task m_FetcherTask;
        private Task m_DecoderTask;
        private Faker m_Faker = new Faker();
        private bool m_FastDataDecoder = true;
        private ConcurrentQueue<ReceivedData> DataQueue = new ConcurrentQueue<ReceivedData>();

        public ProducerConsumer(CancellationToken i_Token)
        {
            m_FetcherTask = Task.Run(async () =>
            {
                while (!i_Token.IsCancellationRequested)
                {
                    var dataToProcess = new ReceivedData(await GenerateSampleData());
                    DataQueue.Enqueue(dataToProcess);
                }
            }, i_Token);

            m_DecoderTask = Decoder(i_Token);
        }

        public async Task<string> GenerateSampleData()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(m_Faker.Random.Int(10, 300))); //Simulate text generator
            return m_Faker.Lorem.Sentence(m_Faker.Random.Int(0, 100), m_Faker.Random.Int(0, 100));
        }

        private async Task Decoder(CancellationToken i_Token)
        {
            //TODO: Implement
            //Call DecodeData Method
        }

        public async Task DecodeData(string i_Data)
        {
            await Task.Delay(GetDecoderDelay()); //Simulate text Processing
        }

        private TimeSpan GetDecoderDelay() => 
            TimeSpan.FromMilliseconds(m_FastDataDecoder ? m_Faker.Random.Int(0, 10) : m_Faker.Random.Int(10, 300));

        public Task WaitForCompletion() => Task.WhenAll(m_FetcherTask ?? Task.CompletedTask, m_DecoderTask ?? Task.CompletedTask);
    }
}
