using Microsoft.AspNetCore.Mvc;
using MultiDatabase.Data;
using MultiDatabase.Models;
using MultiDatabase.Models.Entities;
using Microsoft.EntityFrameworkCore;
using TestProject.DbContexts;
using MultiDatabase.Repository.Interface;




namespace MultiDatabase.Controllers
{
    public class UserController : Controller
    {
        //private readonly DbContext dbContext;
        //private readonly UserTestDbContext _ucontext;
        private readonly IUserRepository _userRepository; //data access layer


        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        //{
        //    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
        //    {
        //        var options = new DbContextOptionsBuilder<UserTestDbContext>()
        //            .UseInMemoryDatabase(databaseName: "TestDb")
        //            .Options;

        //        this.dbContext = new UserTestDbContext(options);
        //    }
        //    else
        //    {
        //        this.dbContext = dbContext;
        //    }
        //}

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

            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            //{
            //    await _context.Users.AddAsync(user);
            //    await _ucontext.SaveChangesAsync();
            //}
            //else
            //{
            //await _userRepository.Users.AddAsync(user); //repository pattern
            await _userRepository.AddUserAsync(user);

            return RedirectToAction("Index");
        }

        // Get all users
        [HttpGet]
       
        public async Task<IActionResult> Index()
        {
            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            //{

            //    var users = await _ucontext.Users.ToListAsync();
            //    return View(users);
            //}
            //else
            //{
            var users = await _userRepository.GetAllUsersAsync();
            return View(users);
            //}

        }


        // Edit user
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            //{
            //    var user = await _ucontext.Users.FindAsync(id);
            //    if (user == null) return NotFound();
            //    return View(user);
            //}
            //else
            //{
            var user = await _userRepository.GetUserById(id);
            if (user == null) return NotFound();

            return View(user);
            //}
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user, IFormFile file)
        {
            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            //{
            //    var existingUser = await _ucontext.Users.FindAsync(user.Id);
            //    if (existingUser == null) return NotFound();

            //    existingUser.Name = user.Name;
            //    existingUser.Email = user.Email;
            //    existingUser.Phone = user.Phone;
            //    existingUser.Address = user.Address;

            //    if (file != null && file.Length > 0)
            //    {
            //        using (var memoryStream = new MemoryStream())
            //        {
            //            await file.CopyToAsync(memoryStream);
            //            existingUser.FileData = memoryStream.ToArray();
            //            existingUser.FileName = file.FileName;
            //        }
            //    }

            //    _ucontext.Users.Update(existingUser);
            //    await _ucontext.SaveChangesAsync();

            //    return RedirectToAction("Index");
            //}
            //else
            //{
                var existingUser = await _userRepository.GetUserById(user.Id);
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

            _userRepository.UpdateUserAsync(existingUser);
                //await _userRepository.AddUserAsync(user);

                return RedirectToAction("Index");
            //}
        }

        // Download file
        public async Task<IActionResult> DownloadFile(int id)
        {
            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            //{
            //    var user = await _ucontext.Users.FindAsync(id);
            //    if (user == null || user.FileData == null) return NotFound();
            //    return File(user.FileData, "application/octet-stream", user.FileName);
            //}
            //else
            //{
                var user = await _userRepository.GetUserById(id);
                if (user == null || user.FileData == null) return NotFound();
                return File(user.FileData, "application/octet-stream", user.FileName);
            //}
        }

        // Delete user
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            //{
            //    var user = await _ucontext.Users.FindAsync(id);
            //    if (user == null) return NotFound();
            //    _ucontext.Users.Remove(user);
            //    await _ucontext.SaveChangesAsync();
            //    return NoContent(); 
            //}
            //else
            //{
                var user = await _userRepository.GetUserById(id);
                if (user == null) return NotFound();
            await _userRepository.DeleteUserAsync(id);
            return NoContent();
         
            //}
        }

    }
}
