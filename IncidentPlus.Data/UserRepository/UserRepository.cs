using IncidentPlus.Entity.Entities;
using System.Linq;
using System.Data.Entity;

namespace IncidentPlus.Data.UserRepository
{
    public class UserRepository : GenericRepository<User>
    {
        private UserRepository() { }


        public static UserRepository NewInstance()
        {
            return new UserRepository();
        }

        public User ValidateCredential(User user)
        {
            using(var db = new ContextDB.IncidencPlusDBContext())
            {
                var userTemp = db.Users.Include(_=>_.Rol).Where(_ => _.UserName.ToUpper() == user.UserName.ToUpper() && _.Password.ToUpper() == user.Password.ToUpper()).FirstOrDefault();
                if (userTemp != null)
                    return userTemp;
                return null;
            }
        }
    }
}
