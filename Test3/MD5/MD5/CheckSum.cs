using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CheckSum
{
    public static class CheckSum
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
            var directories = Directory.EnumerateDirectories(path);
            var files = Directory.EnumerateFiles(path);

            var result = path;

            foreach (var file in files)
            {
                result += HashFile(file);
            }

            foreach (var directory in directories)
            {
                result += HashDirectory(directory);
            }

            var data = Encoding.UTF8.GetBytes(result);

            return GetHash(data);
        }
    }
}
