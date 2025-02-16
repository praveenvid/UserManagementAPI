using System;
using HotChocolate;
using HotChocolate.Data;
using UserManagementAPI.Data;
using UserManagementAPI.Models;
using System.Linq;

namespace UserManagementAPI.GraphQL
{
    public class Query
    {
        //[UseD(typeof(UserDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<User> GetUsers([ScopedState] UserDbContext context) => context.Users;
    }
}
