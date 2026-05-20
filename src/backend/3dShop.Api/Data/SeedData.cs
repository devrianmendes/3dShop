using _3dShop.Api.Models.Entities;
using _3dShop.Api.Models.Enums;

namespace _3dShop.Api.Data
{
    public class SeedData
    {
        private readonly AppDbContext _context;
        public SeedData(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Initialize()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    new Category
                    {
                        NamePt = "Eletrônicos",
                        NameEn = "Eletronics"
                    },
                    new Category
                    {
                        NamePt = "Roupas",
                        NameEn = "Clothes"
                    },
                    new Category
                    {
                        NamePt = "Alimentos",
                        NameEn = "Foods"
                    }
                );
            }

            if (!_context.Users.Any())
            {
                _context.Users.Add(new User
                {
                    Email = "rian.damaso@hotmail.com",
                    Name = "Rian",
                    IsActive = true,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("27061991"),
                    UserRole = UserRole.Admin,
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
