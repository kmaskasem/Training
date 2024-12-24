using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingX_2.Areas.Identity.Data;

namespace TrainingX_2.Models.Base
{
    public class Seeding
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDBContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDBContext>>()))
            {
                //is Db has Create?
                context.Database.EnsureCreated();

                if (context.Banks.Any())
                {
                    return;   // DB has been seeded
                }
                var banks = new Bank[]
                {
                    new Bank {Name="กรุงไทย"},
                    new Bank {Name ="กรสิกร"},
                    new Bank {Name="ออมสิน"},
                    new Bank {Name="ทหารไทย"},
                    new Bank {Name="พร้อมเพย์"}
                };
                foreach (Bank b in banks)
                {
                    context.Banks.Add(b);
                }
                context.SaveChanges();

                var categories = new Category[]
                {
                    new Category {Name="การซื้อ-ขาย"},
                    new Category {Name="การตลาด"},
                    new Category {Name = "การบริหาร"},
                    new Category {Name = "การพูด"},
                    new Category {Name="การเงิน"},
                    new Category {Name = "การลงทุน"},
                    new Category {Name="การแพทย์"},
                    new Category {Name = "กฎหมาย"},
                    new Category {Name="เทคโนโลยี"},
                    new Category {Name = "โลจิสติกส์"},
                    new Category {Name="ภาษา"},
                    new Category {Name = "บัญชี"},
                    new Category {Name="ธุรกิจ"},
                    new Category {Name = "อาชีพ"},
                    new Category {Name="สุขภาพ"},
                    new Category {Name = "หุ้น"}
                };
                foreach (Category c in categories)
                {
                    context.Categories.Add(c);
                }
                context.SaveChanges();

                var topics = new TopicAssessment[]
                {
                    new TopicAssessment {Name="ความสะอาด"},
                    new TopicAssessment {Name ="ความรู้"},
                    new TopicAssessment {Name ="ความสะดวก"}
                };
                foreach (TopicAssessment t in topics)
                {
                    context.TopicAssessments.Add(t);
                }
                context.SaveChanges();

                var users = new User[]
                {
                    new User
                    {
                        FirstName = "K",
                        LastName = "Maskasem",
                        role = Role.Admin,
                        Status_user = Status.ใช้งาน,
                        Email = "KMaskasem@example.com",
                        NormalizedEmail = "KMASKASEM@EXAMPLE.COM",
                        UserName = "KMaskasem@example.com",
                        NormalizedUserName = "KMASKASEM@EXAMPLE.COM",
                        PhoneNumber = "+11111111",
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = true,
                        LockoutEnabled = true,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    },
                    new User
                    {
                        FirstName = "A",
                        LastName = "Kasetrin",
                        role = Role.Member,
                        Status_user = Status.ใช้งาน,
                        Email = "AKasetrin@example.com",
                        NormalizedEmail = "AKASETRIN@EXAMPLE.COM",
                        UserName = "AKasetrin@example.com",
                        NormalizedUserName = "AKASETRIN@EXAMPLE.COM",
                        PhoneNumber = "+22222222",
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = true,
                        LockoutEnabled = true,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    }
                };
                foreach (User us in users)
                {
                    if (!context.Users.Any(u => u.UserName == us.UserName))
                    {
                        var password = new PasswordHasher<User>();
                        var hashed = password.HashPassword(us, "secretINW");
                        us.PasswordHash = hashed;

                        var userStore = new UserStore<User>(context);
                        var result = userStore.CreateAsync(us);

                        context.SaveChangesAsync();
                    }
                }
                //AssignRoles(serviceProvider, user1.Email, roles);

            }
        }

        //private static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        //{
        //    UserManager<ApplicationUser> _userManager = services.GetService<UserManager<ApplicationUser>>();
        //    ApplicationUser user = await _userManager.FindByEmailAsync(email);
        //    var result = await _userManager.AddToRolesAsync(user, roles);

        //    return result;
        //}
    }
}
