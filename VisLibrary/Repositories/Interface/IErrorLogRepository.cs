using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Models;

namespace VisLibrary.Repositories.Interface
{
    public interface IErrorLogRepository: IGenericRepositoryBase<ErrorLog>
    {
        Task LogErrorAsync(ErrorLog errorLog);
    }
}
