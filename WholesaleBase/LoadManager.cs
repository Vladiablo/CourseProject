using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WholesaleBase
{
    public interface ILoadManager
    {
        string ReadLine();
        IReadableObject Read(IReadableObjectLoader loader);
    }

    public interface IReadableObject
    { }

    public interface IReadableObjectLoader
    {
        IReadableObject Load(ILoadManager man);
    }
    class LoadManager : ILoadManager
    {
        FileInfo file;
        StreamReader input;
        public LoadManager(string filename)
        {
            file = new FileInfo(filename);
            input = null;
        }

        public IReadableObject Read(IReadableObjectLoader loader)
        {
            return loader.Load(this);
        }

        public void BeginRead()
        {
            if (input != null)
                throw new IOException("Load Error");

            input = file.OpenText();
        }
        public bool IsLoading
        {
            get { return input != null && !input.EndOfStream; }
        }
        public string ReadLine()
        {
            if (input == null)
                throw new IOException("Load Error");

            string line = input.ReadLine();
            return line;
        }

        public void EndRead()
        {
            if (input == null)
                throw new IOException("Load Error");

            input.Close();
        }
    }
}
