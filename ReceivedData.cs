using System;

namespace BackendExam
{
    class ReceivedDataItem
    {
        public ReceivedDataItem(string i_Data)
        {
            Data = i_Data;
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; set; }
        public string Data { get; set; }
    }
}
