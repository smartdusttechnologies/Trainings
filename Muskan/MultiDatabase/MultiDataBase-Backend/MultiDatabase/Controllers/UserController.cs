using Microsoft.AspNetCore.Mvc;
using MultiDatabase.Data;
using MultiDatabase.Models;
using MultiDatabase.Models.Entities;
using Microsoft.EntityFrameworkCore;
using TestProject.DbContexts;




namespace MultiDatabase.Controllers
{
    public class UserController : Controller
    {
        private readonly Application2DbContext dbContext;
        private readonly UserTestDbContext _ucontext;
      

        public UserController(Application2DbContext dbContext)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                var options = new DbContextOptionsBuilder<UserTestDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDb")
                    .Options;

                _ucontext = new UserTestDbContext(options);
            }
            else
            {
                this.dbContext = dbContext;
            }

            
        }

        // Add user
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Add(AddViewModel2 viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            byte[]? fileData = null;
            string? fileName = null;

            if (viewModel.File != null && viewModel.File.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await viewModel.File.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                    fileName = viewModel.File.FileName;
                }
            }
            else
            {
                ModelState.AddModelError("File", "No valid file provided.");
                return View(viewModel);
            }

            var user = new User
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Address = viewModel.Address,
                FileData = fileData,
                FileName = fileName
            };

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                await _ucontext.Users.AddAsync(user);
                await _ucontext.SaveChangesAsync();
            }
            else
            {
                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // Get all users
        [HttpGet]
       
        public async Task<IActionResult> Index()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {

                var users = await _ucontext.Users.ToListAsync();
                return View(users);
            }
            else
            {
                var users = await dbContext.Users.ToListAsync();
                return View(users);
            }
           
        }


        // Edit user
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                var user = await _ucontext.Users.FindAsync(id);
                if (user == null) return NotFound();
                return View(user);
            }
            else
            {
                var user = await dbContext.Users.FindAsync(id);
                if (user == null) return NotFound();
                return View(user);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user, IFormFile file)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                var existingUser = await _ucontext.Users.FindAsync(user.Id);
                if (existingUser == null) return NotFound();

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.Address = user.Address;

                if (file != null && file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        existingUser.FileData = memoryStream.ToArray();
                        existingUser.FileName = file.FileName;
                    }
                }

                _ucontext.Users.Update(existingUser);
                await _ucontext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                var existingUser = await dbContext.Users.FindAsync(user.Id);
                if (existingUser == null) return NotFound();

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.Address = user.Address;

                if (file != null && file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        existingUser.FileData = memoryStream.ToArray();
                        existingUser.FileName = file.FileName;
                    }
                }

                dbContext.Users.Update(existingUser);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
        }

        // Download file
        public async Task<IActionResult> DownloadFile(int id)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                var user = await _ucontext.Users.FindAsync(id);
                if (user == null || user.FileData == null) return NotFound();
                return File(user.FileData, "application/octet-stream", user.FileName);
            }
            else
            {
                var user = await dbContext.Users.FindAsync(id);
                if (user == null || user.FileData == null) return NotFound();
                return File(user.FileData, "application/octet-stream", user.FileName);
            }
        }

        // Delete user
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                var user = await _ucontext.Users.FindAsync(id);
                if (user == null) return NotFound();
                _ucontext.Users.Remove(user);
                await _ucontext.SaveChangesAsync();
                return NoContent(); 
            }
            else
            {
                var user = await dbContext.Users.FindAsync(id);
                if (user == null) return NotFound();
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
                return NoContent(); 
            }
        }

    }
}
