using System;

namespace BackendExam
{
    class ReceivedData
    {
        public ReceivedData(string data)
        {
            Data = data;
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; set; }
        public string Data { get; set; }
    }
}
