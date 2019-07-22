using Admin.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.IService
{
    public interface IStorageService
    {
        Task Put(IFormFile avatar, string v);
        string GetFileUrl();
    }
}
