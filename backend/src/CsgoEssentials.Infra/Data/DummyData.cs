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
            await GenerateUsers(context);
            await GenerateArticles(context);
            await GenerateMaps(context);
            await GenerateVideos(context);
        }

        public static async Task GenerateUsers(DataContext context)
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
            await context.SaveChangesAsync();
        }

        public static async Task GenerateArticles(DataContext context)
        {
            var articles = await context
                .Articles
                .AsNoTracking()
                .ToListAsync();

            if (articles != null && articles.Any())
                return;

            var userMaria = await context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == "maria");

            if (userMaria == null)
                return;

            var _dust2Article = new Article(
               "Best Grenade Spots for Dust2 - Must Know!",
               new DateTime(2021, 01, 10),
               "Dust2 is probably one of the most recognized maps in CS:GO. If you enter a competitive queue, " +
               "it's a very high chance you will be playing on Dust2. The map is also one of the most beginner-friendly " +
               "maps with a simple layout. So many new players will be starting their journey into Counter-Strike Global " +
               "Offensive on this map. So let's jump straight in and see the essential utility you will need to excel on Dust2.",
                userMaria.Id);

            var _smokeLineupCrosshairArticle = new Article(
                "Smoke Lineup Crosshair Bind for CS:GO",
                new DateTime(2021, 01, 02),
                "Create a fullscreen crosshair to line up smokes and other nades. For particular smokes, it can be hard to find " +
                "something natural to place your crosshair at to hit that perfect smoke. This key bind will help you align those hard nades.",
                userMaria.Id);

            context.Articles.AddRange(_dust2Article, _smokeLineupCrosshairArticle);
            await context.SaveChangesAsync();
        }

        public static async Task GenerateMaps(DataContext context)
        {
            var maps = await context
                .Maps
                .AsNoTracking()
                .ToListAsync();

            if (maps != null && maps.Any())
                return;

            var _dust2Map = new Map(
                "Dust2",
                "As melhores smokes do Mapa Dust2");

            var _infernoMap = new Map(
                "Inferno",
                "As melhores smokes do Mapa Inferno");

            context.Maps.AddRange(_dust2Map, _infernoMap);
            await context.SaveChangesAsync();
        }

        public static async Task GenerateVideos(DataContext context)
        {
            var videos = await context
                .Videos
                .AsNoTracking()
                .ToListAsync();

            if (videos != null && videos.Any())
                return;

            var user = await context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == "leolandrooo");

            if (user == null)
                return;

            var map = await context
                .Maps
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == "Dust2");

            if (map == null)
                return;

            var _video1 = new Video(
                "Smoke Fundo D2",
                new DateTime(2021, 01, 10),
                EGrenadeType.Smoke,
                ETick.Tick128,
                "Video demonstracao de uma smoke fundo na d2",
                user.Id,
                map.Id);

            var _video2 = new Video(
                "Smoke Mirage",
                new DateTime(2021, 01, 10),
                EGrenadeType.Smoke,
                ETick.Tick128,
                "Video demonstracao de uma smoke fundo na Mirage",
                user.Id,
                map.Id);

            context.Videos.AddRange(_video1, _video2);
            await context.SaveChangesAsync();
        }
    }
}
