using System;
namespace KuberOrderApp.Interfaces
{
    public interface ISQLiteDBFilePath
    {
        string GetLocalDBFilePath(string filename);
    }
}
