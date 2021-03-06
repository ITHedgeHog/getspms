﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SPMS.Application.Common.Interfaces;
using SPMS.Common;
using SPMS.Domain.Models;

namespace SPMS.Application.System.Commands
{
    public class BasicDataSeeder
    {
        private readonly ISpmsContext _db;

        public BasicDataSeeder(ISpmsContext db)
        {
            _db = db;
        }

        public async Task SeedAllAsync(CancellationToken cancellationToken)
        {
            await SeedPlayerRoleAsync(_db, cancellationToken);

            SeedDefaults(_db);


            await SeedBeyondTheDarknessAsync(_db, cancellationToken);
            //await SeedAquariusAsync(_db, cancellationToken);
            await SeedRing0Async(_db, cancellationToken);
        }

        public async Task SeedPlayerRoleAsync(ISpmsContext db, CancellationToken cancellationToken)
        {
            // Player Role
            if (!db.PlayerRole.Any(r => r.Name == StaticValues.AdminRole))
                await db.PlayerRole.AddAsync(new PlayerRole() { Name = StaticValues.AdminRole }, cancellationToken);
            if (!db.PlayerRole.Any(r => r.Name == StaticValues.PlayerRole))
                await db.PlayerRole.AddAsync(new PlayerRole() { Name = StaticValues.PlayerRole }, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task SeedBiographyStatus(ISpmsContext db, int game, CancellationToken cancellationToken)
        {



            try
            {
                foreach (var status in await db.BiographyStatus.Where(x => x.GameId == null).ToListAsync(cancellationToken))
                {
                    status.GameId = game;
                    await db.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            if (await db.BiographyStatus.AnyAsync(x => !x.Default && x.GameId == game, cancellationToken: cancellationToken))
            {
                var entity = db.BiographyStatus.First(x => x.Name == StaticValues.BioStatusAlive && x.GameId == game);
                entity.Default = true;
                await db.SaveChangesAsync(cancellationToken);
            }

            if (!db.BiographyStatus.Any(n => n.Name == StaticValues.BioStatusAlive && n.GameId == game))
                await db.BiographyStatus.AddAsync(new BiographyStatus() { Name = StaticValues.BioStatusAlive, Default = true, GameId = game }, cancellationToken);
            if (!db.BiographyStatus.Any(n => n.Name == StaticValues.BioStatusDeceased && n.GameId == game))
                await db.BiographyStatus.AddAsync(new BiographyStatus() { Name = StaticValues.BioStatusDeceased, GameId = game }, cancellationToken);
            if (!db.BiographyStatus.Any(n => n.Name == StaticValues.BioStatusMia && n.GameId == game))
                await db.BiographyStatus.AddAsync(new BiographyStatus() { Name = StaticValues.BioStatusMia, GameId = game }, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task SeedBiographyState(ISpmsContext db, int game, CancellationToken cancellationToken)
        {



            foreach (var state in await db.BiographyState.Where(x => x.GameId == null).ToListAsync(cancellationToken))
            {
                state.GameId = game;
                await db.SaveChangesAsync(cancellationToken);
            }

            if (await db.BiographyState.AnyAsync(x => !x.Default && x.GameId == game, cancellationToken: cancellationToken))
            {
                var entity = db.BiographyState.First(x => x.Name == StaticValues.Published && x.GameId == game);
                entity.Default = true;
                await db.SaveChangesAsync(cancellationToken);
            }

            // Biography Status
            if (!db.BiographyState.Any(n => n.Name == StaticValues.Draft && n.GameId == game))
                await db.BiographyState.AddAsync(new BiographyState() { Name = StaticValues.Draft, GameId = game }, cancellationToken);
            if (!db.BiographyState.Any(n => n.Name == StaticValues.Pending && n.GameId == game))
                await db.BiographyState.AddAsync(new BiographyState() { Name = StaticValues.Pending, GameId = game }, cancellationToken);
            if (!db.BiographyState.Any(n => n.Name == StaticValues.Published && n.GameId == game))
                await db.BiographyState.AddAsync(new BiographyState() { Name = StaticValues.Published, Default = true, GameId = game }, cancellationToken);
            if (!db.BiographyState.Any(n => n.Name == StaticValues.Archived && n.GameId == game))
                await db.BiographyState.AddAsync(new BiographyState() { Name = StaticValues.Archived, GameId = game }, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);


        }

        public async Task SeedPostings(ISpmsContext db, int game, CancellationToken cancellationToken)
        {
            foreach (var posting in await db.Posting.Where(x => x.GameId == null).ToListAsync(cancellationToken))
            {
                posting.GameId = game;
                await db.SaveChangesAsync(cancellationToken);
            }

            if (await db.Posting.AnyAsync(x => !x.Default, cancellationToken: cancellationToken))
            {
                var state = db.Posting.First(x => x.Name == "Starbase Gamma" && x.GameId == game);
                state.Default = true;
                await db.SaveChangesAsync(cancellationToken);
            }

            // Posting
            if (!db.Posting.Any(p => p.Name == "Starbase Gamma" && p.GameId == game))
                await db.Posting.AddAsync(new Posting() { Name = "Starbase Gamma", Default = true, GameId = game, IsPlayable = true}, cancellationToken);
            if (!db.Posting.Any(p => p.Name == "USS Sovereign" && p.GameId == game))
                await db.Posting.AddAsync(new Posting() { Name = "USS Sovereign", GameId = game, IsPlayable = true}, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task SeedBiographyTypes(ISpmsContext db, int gameId, CancellationToken cancellationToken)
        {
            try
            {
                if (!db.BiographyTypes.Any(x => x.GameId == gameId && x.Name == StaticValues.BioTypePlayer))
                    await db.BiographyTypes.AddAsync(
                        new BiographyType() { Default = true, GameId = gameId, Name = StaticValues.BioTypePlayer },
                        cancellationToken);
                if (!db.BiographyTypes.Any(x => x.GameId == gameId && x.Name == StaticValues.BioTypeNpc))
                    await db.BiographyTypes.AddAsync(
                        new BiographyType() { Default = true, GameId = gameId, Name = StaticValues.BioTypeNpc },
                        cancellationToken);
                if (!db.BiographyTypes.Any(x => x.GameId == gameId && x.Name == StaticValues.BioTypePoc))
                    await db.BiographyTypes.AddAsync(
                        new BiographyType() { Default = true, GameId = gameId, Name = StaticValues.BioTypePoc },
                        cancellationToken);

                await db.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SeedBeyondTheDarknessAsync(ISpmsContext db, CancellationToken cancellationToken)
        {
            var game = new Game() { Name = StaticValues.BtdGame, Description = "BtD Simulation", SiteTitle = "Beyond the Darkness a Star Trek RPG", Disclaimer = "<p>Star Trek, Star Trek TAS, Star Trek: The Next Generation, Star Trek: Deep Space 9, Star Trek: Voyager, Star Trek Enterprise, and all Star Trek Movies are registered trademarks of Paramount Pictures and their respective owners; no copyright violation is intended or desired.</p><p>All material contained within this site is the property of Dan Taylor, Evan Scown &amp; Beyond the Darkness.</p>", SiteAnalytics = @"<!-- Global site tag (gtag.js) - Google Analytics -->
<script async src='https://www.googletagmanager.com/gtag/js?id=UA-167297746-1'></script>
                <script>
                window.dataLayer = window.dataLayer || [];
                function gtag() { dataLayer.push(arguments); }
            gtag('js', new Date());

            gtag('config', 'UA-167297746-1');
                </script> " };
            if (!db.Game.Any(g => g.Name == StaticValues.BtdGame))
            {
                await db.Game.AddAsync(game, cancellationToken);

                await db.SaveChangesAsync(cancellationToken);

                // Add Game URL's

                await db.GameUrl.AddAsync(new GameUrl() { GameId = game.Id, Url = "www.beyond-the-darkness.com" }, cancellationToken);
                //await db.GameUrl.AddAsync(new GameUrl() { GameId = game.Id, Url = "spms0.rpg-hosting.net" }, cancellationToken);
                await db.GameUrl.AddAsync(new GameUrl() { GameId = game.Id, Url = "btd.beyond-the-darkness.com" }, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

            }
            else
            {


                try
                {
                    game = await db.Game.Include(x => x.Url).FirstOrDefaultAsync(g => g.Name == StaticValues.BtdGame,
                        cancellationToken: cancellationToken);


                    if (game.Url.Any(x => x.Url == "spms0.rpg-hosting.net"))
                    {
                        var urlToRemove = await db.GameUrl.FirstAsync(x => x.GameId == game.Id && x.Url == "spms0.rpg-hosting.net", cancellationToken: cancellationToken);
                        db.GameUrl.Remove(urlToRemove);
                        await db.SaveChangesAsync(cancellationToken);
                    }

                    if (game.Url.Any(x => x.Url == "localhost"))
                    {
                        var urlToRemove = await db.GameUrl.FirstAsync(x => x.GameId == game.Id && x.Url == "localhost", cancellationToken: cancellationToken);
                        db.GameUrl.Remove(urlToRemove);
                        await db.SaveChangesAsync(cancellationToken);
                    }

                }
                catch (Exception ex)
                {
                    var i = 1;
                }

            }

            
            await SeedBiographyStatus(_db, game.Id, cancellationToken);
            await SeedBiographyState(_db, game.Id, cancellationToken);
            await SeedPostings(_db, game.Id, cancellationToken);
            await SeedBiographyTypes(_db, game.Id, cancellationToken);


            // Add Biographies

            // Character
            if (!await db.Biography.AnyAsync(b => b.Firstname == "Marcus" && b.Surname == "Brightstar", cancellationToken: cancellationToken))
                await db.Biography.AddAsync(new Domain.Models.Biography()
                {
                    Firstname = "Marcus",
                    Surname = "Brightstar",
                    Born = "Earth",
                    Gender = "Male",
                    Assignment = "Starbase Gamma",
                    PostingId = db.Posting.First(p => p.Name == "Starbase Gamma").Id,
                    Rank = "Captain",
                    DateOfBirth = "Sometime in 2351",
                    StateId = (await db.BiographyState.FirstAsync(x => x.GameId == game.Id && x.Name == StaticValues.Published, cancellationToken)).Id,
                    StatusId = (await db.BiographyState.FirstAsync(x => x.GameId == game.Id && x.Name == StaticValues.BioStatusAlive, cancellationToken)).Id
                }, cancellationToken);
            if (!db.Biography.Any(b => b.Firstname == "Jessica" && b.Surname == "Darkly"))
                await db.Biography.AddAsync(new Domain.Models.Biography()
                {
                    Firstname = "Jessica",
                    Surname = "Darkly",
                    Born = "Earth",
                    Gender = "Female",
                    Assignment = "Starbase Gamma",
                    PostingId = db.Posting.First(p => p.Name == "Starbase Gamma").Id,
                    Rank = "Admiral",
                    DateOfBirth = "Sometime in 2332",
                    StateId = (await db.BiographyState.FirstAsync(x => x.GameId == game.Id && x.Name == StaticValues.Published, cancellationToken)).Id,
                    StatusId = (await db.BiographyState.FirstAsync(x => x.GameId == game.Id && x.Name == StaticValues.BioStatusAlive, cancellationToken)).Id,
                }, cancellationToken);
            if (!db.Biography.Any(b => b.Firstname == "Nigel" && b.Surname == "Adisa"))
                await db.Biography.AddAsync(new Domain.Models.Biography()
                {
                    Firstname = "Nigel",
                    Surname = "Adisa",
                    Born = "Earth",
                    Gender = "Male",
                    Assignment = "Starbase Gamma",
                    PostingId = db.Posting.First(p => p.Name == "Starbase Gamma").Id,
                    Rank = "Lieutenant",
                    DateOfBirth = "",
                    StateId = (await db.BiographyState.FirstAsync(x => x.GameId == game.Id && x.Name == StaticValues.Published, cancellationToken)).Id,
                    StatusId = (await db.BiographyState.FirstAsync(x => x.GameId == game.Id && x.Name == StaticValues.BioStatusAlive, cancellationToken)).Id,
                    History = @"A black male of African/British decent, 6'0 in height and weighing in at 196 pounds. He has short cropped black hair and usually wears a short beard.

General Overview		Doctor Adisa, a specialist in neurology possesses a seemingly easygoing manner which he generally uses to mask his borderline OCD issues. He is very witty however his humor can sometimes become overly Sharp. His hobbies include playing various jazz instruments, long distance running, chess, and baking.



Personal History		graduated from Oxford University, attended st. George's medical University and graduated in the top 5% of his class.

Married at the age of 22, and divorced at the age of 30, the union producing one child, a daughter.

He enrolled in Starfleet directly after graduating the University against his father's wishes. who vehemently opposes Starfleet policies."
                }, cancellationToken);

            if (!db.Biography.Any(b => b.Firstname == "Vars" && b.Surname == "Qiratt"))
                await db.Biography.AddAsync(new Domain.Models.Biography()
                {
                    Firstname = "Vars",
                    Surname = "Qiratt",
                    Born = "Bolia",
                    Species = "Bolian",
                    Gender = "Male",
                    Assignment = "",
                    PostingId = db.Posting.First(p => p.Name == "Starbase Gamma").Id,
                    Rank = "Lieutenant Commander",
                    DateOfBirth = "",
                    PlayerId = db.Player.First(p => p.DisplayName == "Dan Taylor").Id,
                    StateId = (await db.BiographyState.FirstAsync(x => x.GameId == game.Id && x.Name == StaticValues.Published, cancellationToken)).Id,
                    StatusId = (await db.BiographyState.FirstAsync(x => x.GameId == game.Id && x.Name == StaticValues.BioStatusAlive, cancellationToken)).Id,
                    History = @"Vars is best described as carrying extra weight. A rotund Bolian Male with puffy facial features and a noticeable double chin. His height is on the slightly shorter side, coming in at around 5'9&quot;.
Vars is a vibrant individual living up to the term, 'Jolly Fat Man'.He has a distinctive laugh that can be considered quite obnoxious, not helped by his flavorful personality.Wearing his emotions like a badge on his sleeve,
Vars rarely shy's away from expressing his opinion. To the same extent, a withdrawn Vars is often the sign of an insecurity or fear.

He keeps with him a Hair piece that he wears on 'special occasions' or on Thursdays.Part of his quirky nature.

* 2383 - Current - USS Ronald Reagan - Chief of Operations(Lt Cmdr)
* 2383(Late) - USS Ronald Reagan - Acting Chief of Operations(Lt Cmdr)
* 2383(Early) - USS Ronald Reagan - Assistant Chief of Engineering / Acting Chief of Operations(Lt)
* 2380 - 2382 - USS Midway - Assistant Chief of Engineering(Lt)
* 2379 - USS Midway - Supervising Engineering Officer(Lt JG)
* 2376 - 2378 - Starbase 386 - Star Ship Maintenance Engineering Team Lead(Lt JG)
* 2374 - 2375 - USS Gettysburg - Engineering Officer / Trainee Supervisor(Lt JG)
* 2372 - 2373 - USS Melbourne - Engineering Officer(Ens / Lt JG)
* 2368 - 2371 - USS Galloway - Engineering Officer(Ens)
* 2363 - 2367 - Starfleet Academy - Officer Cadet[Engineering Stream]"
                }, cancellationToken);


            // Series
            var series = new Series() { Title = "Series 1", GameId = game.Id, IsActive = true };
            if (!db.Series.Any(x => x.Title == "Series 1" && x.GameId == game.Id))
            {
                await db.Series.AddAsync(series, cancellationToken);

                await db.SaveChangesAsync(cancellationToken);
            }
            else
            {
                series = await _db.Series.FirstAsync(x => x.Title == "Series 1" && x.GameId == game.Id,
                    cancellationToken: cancellationToken);

                if (!series.IsActive)
                {
                    series.IsActive = true;
                    await db.SaveChangesAsync(cancellationToken);
                }
            }

            if (!await db.Episode.AnyAsync(x => x.Title == "Prologue", cancellationToken: cancellationToken))
            {
                EpisodeStatus episodeStatus = await db.EpisodeStatus.AsNoTracking().FirstAsync(x => x.Name == StaticValues.Published, cancellationToken: cancellationToken);
                await db.Episode.AddAsync(new Episode()
                {
                    Title = "Prologue",
                    SeriesId = series.Id,
                    StatusId = episodeStatus.Id
                }, cancellationToken);


            }

            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task SeedRing0Async(ISpmsContext db, CancellationToken cancellationToken)
        {
            var game = new Game()
            {
                Name = StaticValues.TestGame,
                Description = "USS Voyager Test SPMS Site",
                SiteTitle = "SPMS Example Site",
                Disclaimer =
                    "<p>Star Trek, Star Trek TAS, Star Trek: The Next Generation, Star Trek: Deep Space 9, Star Trek: Voyager, Star Trek Enterprise, and all Star Trek Movies are registered trademarks of Paramount Pictures and their respective owners; no copyright violation is intended or desired.</p><p>All material contained within this site is the property of Dan Taylor, Evan Scown &amp; Beyond the Darkness.</p>",
                SiteAnalytics = "",
                IsSpiderable = false,
                Author = "Dan Taylor & Evan Scown",
                RobotsText = "User-Agent: *\n\rAllow: /"
            };
            if (!db.Game.Any(g => g.Name == StaticValues.TestGame))
            {
                await db.Game.AddAsync(game, cancellationToken);

                await db.SaveChangesAsync(cancellationToken);

                // Add Game URL's

                await db.GameUrl.AddAsync(new GameUrl() { GameId = game.Id, Url = "localhost" }, cancellationToken);
                await db.GameUrl.AddAsync(new GameUrl() { GameId = game.Id, Url = "spms0.rpg-hosting.net" },
                    cancellationToken);

                await db.SaveChangesAsync(cancellationToken);
            }

            game = await db.Game.Include(x => x.Url)
                .FirstAsync(g => g.Name == StaticValues.TestGame, cancellationToken);


            if (game.IsSpiderable.HasValue == false || game.IsSpiderable.Value)
            {
                game.IsSpiderable = false;
                await db.SaveChangesAsync(cancellationToken);
            }

            if (string.IsNullOrEmpty(game.RobotsText))
            {
                game.RobotsText = "User-Agent: *\n\rAllow: /";
                await db.SaveChangesAsync(cancellationToken);
            }

            if (string.IsNullOrEmpty(game.Author))
            {
                game.Author = "Dan Taylor & Evan Scown";
                await db.SaveChangesAsync(cancellationToken);
            }

            await SeedBiographyStatus(_db, game.Id, cancellationToken);
            await SeedBiographyState(_db, game.Id, cancellationToken);
            //await SeedPostings(_db, game.Id, cancellationToken);

            // Posting
            if (!db.Posting.Any(p => p.Name == "USS Voyager" && p.GameId == game.Id))
                await db.Posting.AddAsync(new Posting() { Name = "USS Voyager", Default = true, GameId = game.Id, IsPlayable = true}, cancellationToken);
            

            await db.SaveChangesAsync(cancellationToken);
            await SeedBiographyTypes(_db, game.Id, cancellationToken);

            if (await db.Posting.AnyAsync(x => x.Name == "USS Voyager" && x.GameId == game.Id,
                cancellationToken: cancellationToken))
            {
                await db.Posting.AddAsync(new Posting()
                {
                    GameId = game.Id,
                    Default = true,
                    Name = "USS Voyager"
                }, cancellationToken);

                await db.SaveChangesAsync(cancellationToken);
            }

            var posting = await db.Posting.FirstAsync(x => x.GameId == game.Id && x.Name == "USS Voyager", cancellationToken);


            // Add Biographies

            // Character
            if (!await db.Biography.AnyAsync(b => b.Firstname == "Kathryn" && b.Surname == "Janeway", cancellationToken: cancellationToken))
                await db.Biography.AddAsync(new Domain.Models.Biography()
                {
                    Firstname = "Kathryn",
                    Surname = "Janeway",
                    Born = "Earth",
                    Gender = "Female",
                    Assignment = "Starbase Gamma",
                    PostingId = db.Posting.First(p => p.Name == "Starbase Gamma").Id,
                    Rank = "Captain",
                    StateId = (await db.BiographyState.FirstAsync(x => x.GameId == game.Id && x.Name == StaticValues.Published, cancellationToken)).Id,
                    StatusId = (await db.BiographyState.FirstAsync(x => x.GameId == game.Id && x.Name == StaticValues.BioStatusAlive, cancellationToken)).Id
                }, cancellationToken);


            // Series
            var series = new Series() { Title = "Series 1", GameId = game.Id, IsActive = true };
            if (!db.Series.Any(x => x.Title == "Series 1" && x.GameId == game.Id))
            {
                await db.Series.AddAsync(series, cancellationToken);

                await db.SaveChangesAsync(cancellationToken);
            }
            else
            {
                series = await _db.Series.FirstAsync(x => x.Title == "Series 1" && x.GameId == game.Id,
                    cancellationToken: cancellationToken);

                if (!series.IsActive)
                {
                    series.IsActive = true;
                    await db.SaveChangesAsync(cancellationToken);
                }
            }

            if (!await db.Episode.Include(x => x.Series).AnyAsync(x => x.Title == "Caretaker" && x.Series.GameId == game.Id, cancellationToken: cancellationToken))
            {
                EpisodeStatus episodeStatus = await db.EpisodeStatus.AsNoTracking().FirstAsync(x => x.Name == StaticValues.Published, cancellationToken: cancellationToken);
                await db.Episode.AddAsync(new Episode()
                {
                    Title = "Caretaker",
                    SeriesId = series.Id,
                    StatusId = episodeStatus.Id
                }, cancellationToken);


            }

            await db.SaveChangesAsync(cancellationToken);
        }



        public static void SeedDefaults(ISpmsContext context)
        {

            // Episode Status
            if (!context.EpisodeStatus.Any(n => n.Name == StaticValues.Draft))
                context.EpisodeStatus.Add(new EpisodeStatus() { Name = StaticValues.Draft });
            if (!context.EpisodeStatus.Any(n => n.Name == StaticValues.Pending))
                context.EpisodeStatus.Add(new EpisodeStatus() { Name = StaticValues.Pending });
            if (!context.EpisodeStatus.Any(n => n.Name == StaticValues.Published))
                context.EpisodeStatus.Add(new EpisodeStatus() { Name = StaticValues.Published });
            if (!context.EpisodeStatus.Any(n => n.Name == StaticValues.Archived))
                context.EpisodeStatus.Add(new EpisodeStatus() { Name = StaticValues.Archived });

            // EpisodeEntryType
            if (!context.EpisodeEntryType.Any(e => e.Name == StaticValues.Post))
                context.EpisodeEntryType.Add(new EpisodeEntryType() { Name = StaticValues.Post });

            if (!context.EpisodeEntryType.Any(e => e.Name == StaticValues.PersonalLog))
                context.EpisodeEntryType.Add(new EpisodeEntryType() { Name = StaticValues.PersonalLog });
            if (!context.EpisodeEntryType.Any(e => e.Name == StaticValues.Fiction))
                context.EpisodeEntryType.Add(new EpisodeEntryType() { Name = StaticValues.Fiction });

            // Episode Status
            if (!context.EpisodeEntryStatus.Any(n => n.Name == StaticValues.Draft))
                context.EpisodeEntryStatus.Add(new EpisodeEntryStatus() { Name = StaticValues.Draft });
            if (!context.EpisodeEntryStatus.Any(n => n.Name == StaticValues.Pending))
                context.EpisodeEntryStatus.Add(new EpisodeEntryStatus() { Name = StaticValues.Pending });
            if (!context.EpisodeEntryStatus.Any(n => n.Name == StaticValues.Published))
                context.EpisodeEntryStatus.Add(new EpisodeEntryStatus() { Name = StaticValues.Published });
            if (!context.EpisodeEntryStatus.Any(n => n.Name == StaticValues.Archived))
                context.EpisodeEntryStatus.Add(new EpisodeEntryStatus() { Name = StaticValues.Archived });


            context.SaveChanges();


        }

        public async Task SeedAquariusAsync(ISpmsContext context, CancellationToken cancellationToken)
        {
            var game = new Game() { Name = "USS Aquarius", Description = "Aqua  Simulation", SiteTitle = "Beyond the Darkness a Star Trek RPG", Disclaimer = "<p>Star Trek, Star Trek TAS, Star Trek: The Next Generation, Star Trek: Deep Space 9, Star Trek: Voyager, Star Trek Enterprise, and all Star Trek Movies are registered trademarks of Paramount Pictures and their respective owners; no copyright violation is intended or desired.</p><p>All material contained within this site is the property of Dan Taylor, Evan Scown &amp; Beyond the Darkness.</p>" };
            if (!context.Game.Any(g => g.Name == "USS Aquarius"))
            {
                await context.Game.AddAsync(game, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);

                // Add Game URL's

                await context.GameUrl.AddAsync(new GameUrl() { GameId = game.Id, Url = "aqua.beyond-the-darkness.com" }, cancellationToken);
                await context.GameUrl.AddAsync(new GameUrl() { GameId = game.Id, Url = "spms1.beyond-the-darkness.com" }, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                game = context.Game.First(g => g.Name == "USS Aqua");
            }

            await SeedBiographyStatus(_db, game.Id, cancellationToken);
            await SeedBiographyState(_db, game.Id, cancellationToken);
            await SeedPostings(_db, game.Id, cancellationToken);
            await SeedBiographyTypes(_db, game.Id, cancellationToken);

            //var bioCount = context.Biography.Include(x => x.Posting)
            //    .Count(b => b.Posting.Name == "Starbase Gamma");
            //if (bioCount <= 10)
            //{
            //    for (var i = bioCount; i <= 10; i++)
            //    {
            //        var rnd = new Random();
            //        context.Biography.Add(new Biography()
            //        {
            //            Firstname = "Random" + rnd.Next(),
            //            Surname = "Character" + rnd.Next(),
            //            Born = "Earth",
            //            Gender = "Female",
            //            Assignment = "Starbase Gamma",
            //            PostingId = context.Posting.First(p => p.Name == "Starbase Gamma").Id,
            //            Rank = "Admiral",
            //            DateOfBirth = "Sometime in 2332",
            //            PlayerId = context.Player.First(p => p.DisplayName == "Dan Taylor").Id,
            //            StatusId = 3
            //        });
            //    }
            //}
            //context.SaveChanges();
        }
    }
}
