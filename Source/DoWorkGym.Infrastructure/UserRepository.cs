using System;
using System.Linq;
using DoWorkGym.Model;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace DoWorkGym.Infrastructure
{
    public class UserRepository : Repository<User>
    {
        public void Add(User user)
        {
            base.Insert(user);
        }


        public User GetById(string id)
        {
            return base.GetById(ObjectId.Parse(id));
        }


        public User ByEmail(string email)
        {
            var user = base.GetCollection().AsQueryable<User>()
                .FirstOrDefault(e => e.Email == email);

            return user;
        }


        public void UpdateLastLoginById(string userId, DateTime date)
        {
            var query = Query<User>.EQ(x => x.Id, ObjectId.Parse(userId));
            var update = Update<User>.Set(x => x.LastLogin, date); // update modifiers, ToUniversalTime()

            base.GetCollection().Update(query, update);
        }
    }
}
