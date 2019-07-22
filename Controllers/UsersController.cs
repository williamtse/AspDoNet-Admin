using System.Linq;
using System.Threading.Tasks;
using BootstrapHtmlHelper.FormHelper;
using BootstrapHtmlHelper.FormHelper.Fields;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin.Extensions;
using Admin.IForms;
using Admin.Interface;
using Admin.IService;
using Admin.Models;
using Admin.Utils;
using Admin.ViewModels;
using Admin.IViewModels;
using System.IO;

namespace Admin.Controllers
{
    public class UsersController : AController
    {
        //RBAC权限控制接口
        private readonly IRbacService rbac;
        private IUserForm _userForm;
        private readonly IStorageService _storage;
        public UsersController(IHttpContextAccessor _httpContextAccessor,
            AdminContext context,
            IRbacService rbacService,
            IUserViewModel userViewModel,
            IUserForm userForm,
            IStorageService storage)
        {
            httpContextAccessor = _httpContextAccessor;
            _context = context;
            rbac = rbacService;
            _userForm = userForm;
            _storage = storage;
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        private Form Form()
        {
            _userForm.SetRoles(Option.GetOptions<Role>(
                        _context.Role.ToList<Role>(),
                        (r) => r.ID.ToString(),
                        (r) => r.Name
                        ));
            _userForm.SetPermissions(Option.GetOptions<Permission>(
                        _context.Permission.ToList<Permission>(),
                        (r) => r.ID.ToString(),
                        (r) => r.Name
                        ));
            return _userForm.GetForm();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            Form form = Form();
            UserViewModel _userViewModel = new UserViewModel();
            form.Model(_userViewModel, "ID");
            ViewData["formHtml"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Username,Password,ConfirmPassword, Name,Avatar,RememberToken, Roles, Permissions")] UserViewModel userViewModel, IFormFile Avatar)
        {
            User user = userViewModel.GetEntity();
            

            if (ModelState.IsValid)
            {
                //上传头像
                long size = Avatar.Length;
                await _storage.Put(Avatar, "images/avatar");
                string url = _storage.GetFileUrl();
                if (url.Length > 0)
                {
                    user.Avatar = url;
                }
                
                HashPair hashPair = Encrypt.Password(userViewModel.Password);
                user.Password = hashPair.Hashed;
                user.Salt = hashPair.Salt;
                _context.Add(user);
                rbac.AddUserRoles(user, userViewModel.Roles);
                rbac.AddUserPermissions(user, userViewModel.Permissions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                GetErrorListFromModelState(ModelState);
            }
            
            return Create();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var uvm = new UserViewModel();
            BindObject.CopyModel(uvm, user);
            uvm.Permissions = rbac.GetUserPermissions(user);
            uvm.Roles = rbac.GetUserRoles(user);
            uvm.ConfirmPassword = user.Password;
            Form form = Form();
            form.Model(uvm, "ID");
            ViewData["formHtml"] = form.GetContent();
            ViewData["script"] = form.GetScript();
            return View();
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Username,Password,ConfirmPassword, Name,Avatar,RememberToken, Roles, Permissions")] UserViewModel userViewModel, IFormFile Avatar)
        {
            User user = userViewModel.GetEntity();
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldUser = _context.User.Find(id);
                    //修改头像
                    if (Avatar != null)
                    {
                        //上传头像
                        long size = Avatar.Length;
                        await _storage.Put(Avatar, "images/avatar");
                        string url = _storage.GetFileUrl();
                        if (url.Length > 0)
                        {
                            user.Avatar = url;
                        }
                    }
                    else
                    {
                        user.Avatar = oldUser.Avatar;
                    }
                    //修改密码
                    if (oldUser.Password != user.Password)
                    {
                        HashPair hash = Encrypt.Password(user.Password);
                        user.Password = hash.Hashed;
                        user.Salt = hash.Salt;
                    }
                    _context.Entry(oldUser).CurrentValues.SetValues(user); //更新用户信息
                    //编辑用户权限
                    rbac.RemoveUserAuthorities(user); //删除旧权限
                    rbac.AddUserRoles(user, userViewModel.Roles);//增添新用户角色对应关系
                    rbac.AddUserPermissions(user, userViewModel.Permissions);//添加新用户权限对应关系
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                GetErrorListFromModelState(ModelState);
            }
            return await Edit(id);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            rbac.RemoveUserAuthorities(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }
    }
}
