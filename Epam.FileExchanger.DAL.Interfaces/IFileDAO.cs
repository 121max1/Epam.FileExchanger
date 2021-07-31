using Epam.FileExchanger.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.FileExchanger.DAL.Interfaces
{
    public interface IFileDAO
    {
        void Add(File file);

        void Delete(File file);

        void Update(File file);

        File GetById(long id);

        IEnumerable<File> GetAll();

    }
}
