using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Admin.Extensions;
using Admin.Models;
using Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Attributes
{
    public class UniqueUserName : ValidationAttribute, IClientModelValidator
    {
        private readonly AdminContext _context;
        public UniqueUserName()
        {
            _context = XWFHttpContext.ServiceProvider.GetService(typeof(AdminContext)) as AdminContext;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            UserViewModel user = (UserViewModel)validationContext.ObjectInstance;
            User oldUser = _context.User.Find(user.ID);
            string userName = value.ToString();
            if ((user.ID > 0 && userName!= oldUser.Username) || user.ID==0)
            {
                if (_context.User.Where((u) => u.Username == userName).Count() > 0)
                {
                    return new ValidationResult(GetErrorMessage(userName));
                }
            }

            return ValidationResult.Success;
        }
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
        }

        private string GetErrorMessage(string userName)
        {
            return $"用户名{userName}已存在";
        }
    }
}
