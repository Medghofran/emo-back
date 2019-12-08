using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
// project dependencies
using emo_back.Models;
using Microsoft.AspNetCore.Identity;
// framework dependencies
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace emo_back.Data
{
    public class DataGenerator
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new EmoDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<EmoDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;
                }
                var applicationUser = new ApplicationUser
                {
                    UserName = "default",
                    Email = "default@emo.ee"
                };
                string hash = new PasswordHasher<ApplicationUser>().HashPassword(applicationUser, "DeFaUlTpAsSwOrD");
                applicationUser.PasswordHash = hash;
                applicationUser.NormalizedEmail = applicationUser.Email.ToUpper();
                applicationUser.NormalizedUserName = applicationUser.UserName.ToUpper();
                context.Entry(applicationUser).State = EntityState.Added;
                context.SaveChanges();

                // Look for any board games.
                if (context.Channels.Any())
                {
                    return;   // Data was already seeded
                }

                context.Channels.AddRange(
                    new Channel
                    {
                        ConversationName = "default",
                        CreatedAt = DateTime.Now,
                        IsDeleted = false,
                        Messages = new List<Message>{
                            new Message{
                                Emotion = "peepee",
                                MessageBody = "oh my peepee",
                                UserId = applicationUser.Id
                            }
                        }
                    });

                context.SaveChanges();
            }
        }
    }
}