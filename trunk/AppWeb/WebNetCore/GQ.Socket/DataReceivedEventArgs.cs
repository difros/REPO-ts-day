using System.Text;

namespace GQ.Socket
{
    public class DataReceivedEventArgs
    {
        public int Size { get; private set; }
        public byte[] Data { get; private set; }

        public DataReceivedEventArgs(int size, byte[] data)
        {
            Size = size;
            Data = data;
        }

        public override string ToString()
        {
            return Encoding.ASCII.GetString(Data, 0, Size);
        }
    }
}
