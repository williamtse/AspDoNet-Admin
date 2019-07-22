using Admin.IService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Services
{
    public class LocalStorage : IStorageService
    {
        private string baseFolder = @"wwwroot\";
        private string url = "/";
        private string path="";
        private string prefix="";

        public string GetFileUrl()
        {
            return url;
        }

        public async Task Put(IFormFile avatar, string prefix)
        {
            // 临时文件的路径
            var filePath = Path.GetTempFileName();
            //取后缀名
            var fileN = avatar.FileName.ToString();
            var fileLastName = fileN.Substring(fileN.LastIndexOf(".") + 1,
                (fileN.Length - fileN.LastIndexOf(".") - 1));

            string fileFolder = baseFolder + prefix;
            if (!Directory.Exists(fileFolder))
                Directory.CreateDirectory(fileFolder);

            filePath = fileFolder + "\\"+ fileN;//保存文件的路径
            url = url + prefix + "/" + fileN;
            if (avatar.Length > 0)
            {
                //根据路径创建文件
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }
            }
        }
    }
}
