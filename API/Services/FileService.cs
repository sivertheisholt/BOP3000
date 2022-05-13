using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Interfaces.IServices;

namespace API.Services
{
    public class FileService : IFileService
    {
        public FileService()
        {
        }

        public async Task CreateUserData()
        {
            string[] lines =
            {
                "First line", "Second line", "Third line"
            };

            await File.WriteAllLinesAsync("WriteLines.txt", lines);
        }
    }
}