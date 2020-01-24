using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WholesaleBase
{
    public interface ISaveManager
    {
        void WriteLine(string line);
        void WriteObject(IWritableObject obj);
    }

    public interface IWritableObject
    {
        void Write(ISaveManager man);
    }
    class SaveManager: ISaveManager
    {
        FileInfo file;

        public SaveManager(string filename)
        {
            file = new FileInfo(filename);
        }

        public void CreateFile()
        {
            if (file.Exists) file.Delete();
            FileStream fileStream = file.Create();
            fileStream.Close();
        }

        public void WriteLine(string line)
        {
            var output = file.AppendText();
            output.WriteLine(line);
            output.Close();
        }

        public void WriteObject(IWritableObject obj)
        {
            obj.Write(this);
        }
    }
}