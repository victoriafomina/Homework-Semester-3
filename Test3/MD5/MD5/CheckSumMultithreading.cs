using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CheckSum
{
    public class CheckSumMultithreading
    {
        private static string GetHash(byte[] data)
        {
            var md5 = MD5.Create();
            var checkSum = md5.ComputeHash(data);
            var result = BitConverter.ToString(checkSum);

            return result;
        }

        public static string HashFile(string path)
        {
            using (FileStream fStream = File.OpenRead(path))
            {
                var data = new byte[fStream.Length];
                fStream.Read(data, 0, (int)fStream.Length);

                return GetHash(data);
            }
        }

        public static string HashDirectory(string path)
        {
            var filesTasks = new List<Task<string>>();
            var directoriesTasks = new List<Task<string>>();

            var files = Directory.EnumerateFiles(path);
            var directories = Directory.EnumerateDirectories(path);

            var result = path;

            foreach (var file in files)
            {
                var task = new Task<string>(() => HashFile(file));
                task.Start();
                filesTasks.Add(task);
            }

            foreach (var task in filesTasks)
            {
                result += task.Result;
            }

            foreach (var directory in directories)
            {
                var task = new Task<string>(() => HashDirectory(directory));
                task.Start();
                directoriesTasks.Add(task);
            }

            foreach (var task in directoriesTasks)
            {
                result += task.Result;
            }

            var data = Encoding.UTF8.GetBytes(result);

            return GetHash(data);
        }
    }
}
