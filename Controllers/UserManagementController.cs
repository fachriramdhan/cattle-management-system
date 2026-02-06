using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimSapi.Data;
using SimSapi.Models;
using SimSapi.ViewModels;

namespace SimSapi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserManagementController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .Include(u => u.Peternak)
                .OrderBy(u => u.NamaLengkap)
                .ToListAsync();

            var userViewModels = new List<(ApplicationUser User, string Role)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add((user, roles.FirstOrDefault() ?? "User"));
            }

            return View(userViewModels);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["PeternakId"] = new SelectList(
                await _context.Peternak.Where(p => p.UserId == null).ToListAsync(), 
                "Id", 
                "NamaLengkap"
            );
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NamaLengkap = model.NamaLengkap,
                    EmailConfirmed = true,
                    IsActive = true,
                    PeternakId = model.PeternakId
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);

                    // Update Peternak UserId jika ada
                    if (model.PeternakId.HasValue)
                    {
                        var peternak = await _context.Peternak.FindAsync(model.PeternakId.Value);
                        if (peternak != null)
                        {
                            peternak.UserId = user.Id;
                            await _context.SaveChangesAsync();
                        }
                    }

                    TempData["SuccessMessage"] = "User berhasil dibuat";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewData["PeternakId"] = new SelectList(
                await _context.Peternak.Where(p => p.UserId == null).ToListAsync(), 
                "Id", 
                "NamaLengkap",
                model.PeternakId
            );
            return View(model);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                NamaLengkap = user.NamaLengkap,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? "User",
                IsActive = user.IsActive,
                PeternakId = user.PeternakId
            };

            ViewData["PeternakId"] = new SelectList(
                await _context.Peternak.Where(p => p.UserId == null || p.UserId == id).ToListAsync(), 
                "Id", 
                "NamaLengkap",
                model.PeternakId
            );

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                // Update basic info
                user.NamaLengkap = model.NamaLengkap;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.IsActive = model.IsActive;

                // Update Peternak link
                var oldPeternakId = user.PeternakId;
                user.PeternakId = model.PeternakId;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Update password if provided
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        await _userManager.ResetPasswordAsync(user, token, model.Password);
                    }

                    // Update role
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, model.Role);

                    // Update Peternak UserId
                    if (oldPeternakId.HasValue && oldPeternakId != model.PeternakId)
                    {
                        var oldPeternak = await _context.Peternak.FindAsync(oldPeternakId.Value);
                        if (oldPeternak != null)
                        {
                            oldPeternak.UserId = null;
                        }
                    }

                    if (model.PeternakId.HasValue)
                    {
                        var peternak = await _context.Peternak.FindAsync(model.PeternakId.Value);
                        if (peternak != null)
                        {
                            peternak.UserId = user.Id;
                        }
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "User berhasil diupdate";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewData["PeternakId"] = new SelectList(
                await _context.Peternak.Where(p => p.UserId == null || p.UserId == id).ToListAsync(), 
                "Id", 
                "NamaLengkap",
                model.PeternakId
            );

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Remove link from Peternak if exists
            if (user.PeternakId.HasValue)
            {
                var peternak = await _context.Peternak.FindAsync(user.PeternakId.Value);
                if (peternak != null)
                {
                    peternak.UserId = null;
                    await _context.SaveChangesAsync();
                }
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User berhasil dihapus";
            }
            else
            {
                TempData["ErrorMessage"] = "Gagal menghapus user";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}