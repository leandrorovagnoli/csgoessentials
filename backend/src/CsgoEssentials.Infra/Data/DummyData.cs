using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace CsgoEssentials.Infra.Data
{
    public static class DummyData
    {
        public static async Task Initialize(DataContext context)
        {
            await context.Database.EnsureCreatedAsync();
            await GenerateUsers(context, false);
            await GenerateArticles(context, false);

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

        public static async Task GenerateArticles(DataContext context, bool saveChanges)
        {
            var articles = await context
                .Articles
                .AsNoTracking()
                .ToListAsync();

            if (articles != null && articles.Count() > 0)
                return;

            var _dust2Article = new Article(
               "Best Grenade Spots for Dust2 - Must Know!",
               new DateTime(2021, 01, 10),
               "Dust2 is probably one of the most recognized maps in CS:GO. If you enter a competitive queue, " +
               "it's a very high chance you will be playing on Dust2. The map is also one of the most beginner-friendly " +
               "maps with a simple layout. So many new players will be starting their journey into Counter-Strike Global " +
               "Offensive on this map. So let's jump straight in and see the essential utility you will need to excel on Dust2.",
                5);

            var _smokeLineupCrosshairArticle = new Article(
                "Smoke Lineup Crosshair Bind for CS:GO",
                new DateTime(2021, 01, 02),
                "Create a fullscreen crosshair to line up smokes and other nades. For particular smokes, it can be hard to find " +
                "something natural to place your crosshair at to hit that perfect smoke. This key bind will help you align those hard nades.",
                5);
             
            context.Articles.AddRange(_dust2Article, _smokeLineupCrosshairArticle);

            if (saveChanges)
                await context.SaveChangesAsync();
        }
    }
}
