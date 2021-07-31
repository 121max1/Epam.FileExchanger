using Epam.FileExchanger.BLL.Interfaces;
using Epam.FileExchanger.Common.Entities;
using Epam.FileExchanger.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.FileExchanger.BLL
{
    public class FileService : IFileService
    {
        private readonly IFileDAO _fileDAO;
        public FileService(IFileDAO fileDAO)
        {
            _fileDAO = fileDAO;
        }
        public void Add(File file)
        {
            _fileDAO.Add(file);
        }


        public void Delete(File file)
        {
            _fileDAO.Delete(file);
        }

        public IEnumerable<File> GetAll()
        {
            return _fileDAO.GetAll();
        }

        public File GetById(long id)
        {
            return _fileDAO.GetById(id);
        }

        public string GetShortUrl(File file)
        {
            //return new HashidsNet.Hashids(salt: file.Path)
            return "хер";
        }
        public string DecodeFromShortUrl(string path)
        {
            throw new NotImplementedException();
        }

        public void Update(File file)
        {
            _fileDAO.Update(file);
        }
    }
}
