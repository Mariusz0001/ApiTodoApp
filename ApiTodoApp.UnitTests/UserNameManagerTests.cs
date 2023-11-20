using ApiTodoApp.Infrastructure.Authentication;
using ApiTodoApp.Model.User;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ApiTodoApp.UnitTests
{
    public class UserNameManagerTests
    {
        [Fact]
        public void GenerateNewUserNameWhenExists()
        {
            var userManager = Substitute.For<MyUserManager>(Substitute.For<IUserStore<User>>(),
                 null, null, null, null, null, null, null, null
             );

            var userNameManager = new UserNameManager(userManager);

            userManager.FindByNameAsync("John").Returns(Task.FromResult(Substitute.For<User>()));
            userManager.FindByNameAsync("John1").Returns(Task.FromResult(Substitute.For<User>()));

            userManager.FindByNameAsync("John2").Returns(Task.FromResult<User>(null));

            var newUserName = userNameManager.GenerateNewUsernameAsync("John");

            newUserName.Result.Should().Be("John2");
        }

    }

    public class MyUserManager : UserManager<User>
    {
        public MyUserManager(IUserStore<User> store,
                             IOptions<IdentityOptions> optionsAccessor,
                             IPasswordHasher<User> passwordHasher,
                             IEnumerable<IUserValidator<User>> userValidators,
                             IEnumerable<IPasswordValidator<User>> passwordValidators,
                             ILookupNormalizer keyNormalizer,
                             IdentityErrorDescriber errors,
                             IServiceProvider services,
                             ILogger<UserManager<User>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators,
                   passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}