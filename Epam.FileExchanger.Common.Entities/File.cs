using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.FileExchanger.Common.Entities
{
    public enum PrivacyType
    {
        Public = 0,
        Private = 1

    }
    public class File
    {
        public File(string path, User publisher, DateTime addTime, PrivacyType privacyType,long id = -1)
        {
            Id = id;
            Path = path;
            Publisher = publisher;
            AddTime = addTime;
            PrivacyType = privacyType;
        }
        public long Id { get; private set; }

        public string Path { get; private set; }

        public User Publisher { get; private set; }

        public DateTime AddTime { get; private set; }

        public PrivacyType PrivacyType { get; private set; }

    }
}
