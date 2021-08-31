using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JBKClubs1.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JBKClubs1.Controllers
{
    //make controller
    public class JBKRoleController : Controller
    {

            UserManager<IdentityUser> userManager;
            RoleManager<IdentityRole> roleManager;
        //make constructor
        public JBKRoleController(UserManager<IdentityUser> user, RoleManager<IdentityRole> role)
        {
            userManager = user;
            roleManager = role;
        }
        // make index for role
        public IActionResult Index()
            {
            var role = roleManager.Roles;
            List<RoleList> roleLists = new List<RoleList>();
            foreach (var item in role)
            {
                RoleList roles = new RoleList();
                roles.RoleId = item.Id;
                roles.RoleName = item.Name;
                roles.Normalized = item.NormalizedName;
                roleLists.Add(roles);
            }
                return View(roleLists.OrderBy(a=>a.RoleName));
            }
        //make action to add role
        public async Task<IActionResult> AddRole(string roleName)
        {
            try
            {
                if (string.IsNullOrEmpty(roleName))
                    throw new Exception($"role Name is empty!");

                if (await roleManager.RoleExistsAsync(roleName))
                        throw new Exception($"roleName : {roleName} is already on file");
                IdentityResult identityResult = await roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
                if (!identityResult.Succeeded)
                    throw new Exception(identityResult.Errors.FirstOrDefault().Description);
                TempData["message"] = $"roleName : {roleName} is created";
            }
            catch (Exception ex)
            {
                TempData["message"] = $"exception creation role: {ex.GetBaseException().Message}";
            }

            return RedirectToAction("Index");
        }
        //make confirm button to delete
             public async Task<IActionResult> DeleteUserConfirm(string RoleName, string confirm)
            {
            try
            {
                if (confirm != null)
                {
                    IdentityRole identityRole = await roleManager.FindByNameAsync(RoleName);
                    await roleManager.DeleteAsync(identityRole);
                    TempData["message"] = $"roleName : {RoleName} is deleted";
                    ViewBag.RoleName = RoleName;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.RoleName = RoleName;
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = $"exception delete role: {ex.GetBaseException().Message}";
            }
            return RedirectToAction("Index");
        }
        //make button to delete user
            public async Task<IActionResult> DeleteUser(string RoleName,string confirm)
            {
            int count = 0; 
            var users = userManager.Users;
            List<UserList> userList = new List<UserList>();
            try
            {            

                if (RoleName == "administrators")
                {
                    throw new Exception("administrators can not be deleted");
                }



                foreach (var item in users)
                {
                    if (await userManager.IsInRoleAsync(item, RoleName))
                    {
                        UserList user = new UserList();
                        user.UserName = item.UserName;
                        user.UserEmail = item.Email;
                        userList.Add(user);
                        ViewBag.UserName = user;
                        count++;
                    }
                }
                if (count == 0)
                {
                    if (!await roleManager.RoleExistsAsync(RoleName))
                        throw new Exception($"roleName : {RoleName} is not on file");
                    IdentityRole identityRole = await roleManager.FindByNameAsync(RoleName);
                    await roleManager.DeleteAsync(identityRole);
                    TempData["message"] = $"roleName : {RoleName} is deleted";
                    ViewBag.RoleName = RoleName;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.RoleName = RoleName;
                    ViewBag.UserName = userList;
                    if (confirm != null)
                    {
                        IdentityRole identityRole = await roleManager.FindByNameAsync(RoleName);
                        await roleManager.DeleteAsync(identityRole);
                        TempData["message"] = $"roleName : {RoleName} is deleted";
                        ViewBag.RoleName = RoleName;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.RoleName = RoleName;
                        return View();
                    }
                }



            }
            catch (Exception ex)
            {
                TempData["message"] = $"exception delete role: {ex.GetBaseException().Message}";
            }
            ViewBag.RoleName = RoleName;
            ViewBag.UserName = userList;
            return View(userList);
        }
        //make button to add user
        public async Task<IActionResult> AddUser(string RoleName)
        {
            var users = userManager.Users;
            var role = roleManager.Roles;
            RoleList roles = new RoleList();
            List<UserList> userList = new List<UserList>();
            List<UserList> notUserList = new List<UserList>();
            if (RoleName != null)
            {
                foreach (var item in users)
                {
                    if (await userManager.IsInRoleAsync(item, RoleName))
                    {
                        UserList user = new UserList();
                        user.UserName = item.UserName;
                        user.UserEmail = item.Email;
                        userList.Add(user);
                        ViewBag.roleName = RoleName;
                    }
                    else
                    {
                        UserList user = new UserList();
                        user.UserName = item.UserName;
                        user.UserEmail = item.Email;
                        notUserList.Add(user);
                        ViewBag.roleName = RoleName;

                    }
                }
            }
            ViewData["UserName"] = new SelectList(notUserList, "UserName", "UserName");

            return View(userList.OrderBy(a => a.UserName));
        }
        //add user from dropdown
        public async Task<IActionResult> AddUserInList(string RoleName, string UserName)
        {
            IdentityUser user = await userManager.FindByNameAsync(UserName);
            try
            {
                if (!await userManager.IsInRoleAsync(user, RoleName))
                {
                    await userManager.AddToRoleAsync(user, RoleName);
                    TempData["message"] = $"userName : {UserName} is added";
                }

                else
                    throw new Exception($"Cant add!");
            }
            catch (Exception ex)
            {
                TempData["message"] = $"exception add user: {ex.GetBaseException().Message}";
                throw;
            }


            return RedirectToAction("Index");
        }
        //remove user from role
        public async Task<IActionResult> removeUser(string RoleName, string UserName)
        {
            IdentityUser user = await userManager.FindByNameAsync(UserName);
            try
            {
                if (await userManager.IsInRoleAsync(user, "administrators"))
                {
                    TempData["message"] = $"administrator can not be deleted";
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(user, RoleName);
                    TempData["message"] = $"userName : {UserName} is removed";
                }


            }
            catch (Exception ex)
            {
                TempData["message"] = $"exception remove user: {ex.GetBaseException().Message}";
                throw;
            }

            return RedirectToAction("Index");
        }
    }
    }


