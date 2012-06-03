namespace WatchApi
{
    public class Timeouts
    {
        private readonly int _read;
        private readonly int _write;

        public Timeouts(int read, int write)
        {
            this._read = read;
            this._write = write;
        }

        public int Read
        {
            get { return this._read; }
        }

        public int Write
        {
            get { return this._write; }
        }
    }
}