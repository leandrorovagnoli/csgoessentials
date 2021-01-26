using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CsgoEssentials.Infra.Data
{
    public static class DummyData
    {
        public static async Task Initialize(DataContext context)
        {
            await context.Database.EnsureCreatedAsync();
            await GenerateUsers(context, false);

            await context.SaveChangesAsync();
        }

        public static async Task GenerateUsers(DataContext context, bool saveChanges)
        {
            var users = await context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Role == EUserRole.Administrator);

            if (users != null)
                return;

            var adminUserLeo = new User(
                "Leandro",
                "leo@leo.com",
                "leolandrooo",
                MD5Hash.CalculaHash("@123456*"),
                EUserRole.Administrator);

            var adminUserRock = new User(
                "Rock",
                "rock@rock.com",
                "rock",
                MD5Hash.CalculaHash("@123456*"),
                EUserRole.Administrator);

            var adminUserJalaska = new User(
                "Jalaska",
                "jalaska@jalaska.com",
                "jalaska",
                MD5Hash.CalculaHash("@123456*"),
                EUserRole.Administrator);

            var memberUserJoao = new User(
                "Joao",
                "joao@joao.com",
                "joao",
                MD5Hash.CalculaHash("@123456*"),
                EUserRole.Member);

            var editorUserMaria = new User(
                "Maria",
                "maria@maria.com",
                "maria",
                MD5Hash.CalculaHash("@123456*"),
                EUserRole.Editor);

            context.Users.AddRange(adminUserLeo, adminUserRock, adminUserJalaska, memberUserJoao, editorUserMaria);

            if (saveChanges)
                await context.SaveChangesAsync();
        }
    }
}
