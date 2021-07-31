using Epam.FileExchanger.BLL;
using Epam.FileExchanger.BLL.Interfaces;
using Epam.FileExchanger.DAL;
using Epam.FileExchanger.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.FileExchanger.Common.Dependencies
{
    public class DependenciesResolver
    {
        public static IServiceProvider Kernel { get; private set; }
        private static IServiceCollection _services { get; set; }

        static DependenciesResolver()
        {
            _services = new ServiceCollection();
            Kernel = Configurates();
        }

        private static IServiceProvider Configurates()
        {
            _services.AddTransient<IFileDAO, FileDAO>();
            _services.AddTransient<IUserDAO, UserDAO>();

            _services.AddTransient<IFileService, FileService>();
            _services.AddTransient<IUserService, UserService>();

            return _services.BuildServiceProvider();
        }
    }
}
