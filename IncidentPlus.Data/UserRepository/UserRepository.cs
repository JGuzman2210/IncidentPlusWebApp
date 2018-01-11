using IncidentPlus.Entity.Entities;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System;

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

        public override List<User> GetAll()
        {
            using(var db = new ContextDB.IncidencPlusDBContext())
            {
                var result = db.Users.Include(_=>_.Rol).ToList();

                return result.Select(_ => new User() {
                    Id = _.Id,
                    Name = _.Name,
                    Email = _.Email,
                    UserName = _.UserName,
                    Rol = new Rol()
                    {
                        Id = _.Rol.Id,
                        Name = _.Rol.Name,
                        Description = _.Rol.Description
                    }
                }).ToList(); ;
            }
        }

    }
}
