using System;
using UserManagementAPI.Models;
using HotChocolate;
using HotChocolate.Data;
using System.Linq;
using UserManagementAPI.Data;

namespace UserManagementAPI
{
    public class UserQuery
    {
        [UseFiltering] // Enables filtering like "where: { firstName: { eq: 'Alice' } }"
        [UseSorting]   // Enables sorting in GraphQL
        public IQueryable<User> GetUsers([Service] UserDbContext context)
        {
            return context.Users;
        }
    }
}
