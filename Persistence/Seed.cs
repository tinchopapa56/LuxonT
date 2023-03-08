using Domain;

namespace Persistence
{
    public class Seed
    {
        private readonly LuxonDB _initialDB;
        public Seed(LuxonDB initialContext)
        {
            this._initialDB = initialContext;
        }
        // public void SeedDataContext()
        // {
        //     //if (!_initialDB.Tasks.Any())
        //     if (!_initialDB.Tasks.Any())
        //     {
        //         var initialTasks = new List<Tasky>()
        //         {
        //             new Tasky
        //             {
        //                 Title = "Navbar",
        //                 Description = "tremenda descripcion",
        //                 Importance = "HIGH",
        //                 Status = "DONE",
        //             },
        //            new Tasky
        //             {
        //                 Title = "Navbar2",
        //                 Description = "tremenda descripcion",
        //                 Importance = "LOW",
        //                 Status = "DONE",
        //             },
        //              new Tasky
        //             {
        //                 Title = "Navbar3",
        //                 Description = "tremenda descripcion",
        //                 Importance = "HIGH",
        //                 Status = "DONE",
        //             },
        //         };
        //         _initialDB.Tasks.AddRange(initialTasks);
        //         _initialDB.SaveChanges();
        //         //await _initialDB.Tasks.AddRange(initialTasks);
        //         //await _initialDB.SaveChanges();
        //     }
        // }
        
        public static async Task SeedData(LuxonDB context)
        {
           //if (!_initialDB.Tasks.Any())
           if (!context.Tasks.Any())
           {
               var initialTasks = new List<Tasky>
               {
                   new Tasky
                   {
                       Title = "Navbar",
                       OwnerId = "fsa3f3afas",
                       Description = "tremenda descripcion",
                       Importance = "HIGH",
                       Status = "DONE",
                   },
                  new Tasky
                   {
                       Title = "Navbar2",
                       OwnerId = "fsa3f3afas",
                       Description = "tremenda descripcion",
                       Importance = "LOW",
                       Status = "DONE",
                   },
               };
               await context.Tasks.AddRangeAsync(initialTasks);
               await context.SaveChangesAsync();
           }
        }
    }
}
            //        if (!userManager.Users.Any())
            //        {
            //            var users = new List<AppUser>
            //            {
            //                new AppUser
            //                {
            //                    DisplayName = "Bob",
            //                    UserName = "bob",
            //                    Email = "bob@test.com"
            //                },
            //                new AppUser
            //                {
            //                    DisplayName = "Jane",
            //                    UserName = "jane",
            //                    Email = "jane@test.com"
            //                },
            //                new AppUser
            //                {
            //                    DisplayName = "Tom",
            //                    UserName = "tom",
            //                    Email = "tom@test.com"
            //                },


            //            foreach (var user in users)
            //            {
            //                await userManager.CreateAsync(user, "Pa$$w0rd");
            //            }
            //        }
            // }