using BookStoreShoppingCartUI.Constants;

namespace BookStoreShoppingCartUI.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMng = service.GetService<UserManager<IdentityUser>>();
            var roleMng = service.GetService<RoleManager<IdentityRole>>();
            //add roles to database
            await roleMng.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMng.CreateAsync(new IdentityRole(Roles.User.ToString()));

            //create admin user
            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };

            var isUserInDB = await userMng.FindByEmailAsync(admin.Email);
            if (isUserInDB is null)
            {
                await userMng.CreateAsync(admin, "2000.Admin");
                await userMng.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }
    }
}