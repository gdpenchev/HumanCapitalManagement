using AuthenticationAPI.Services;

namespace AuthenticationAPI.Tests
{
    public class UserServiceTests
    {
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userService = new UserService();
        }

        [Test]
        public void ValidateUser_ValidCredentials_Return_User()
        {
            //Arrange
            var user = _userService.ValidateUser("Pesho", "12345");

            //Assert
            Assert.IsNotNull(user);
            Assert.That(user.Username, Is.EqualTo("Pesho"));
        }

        [Test]
        public void ValidateUser_InvalidCredentials_Return_Null()
        {
            //Arrange
            var user = _userService.ValidateUser("Gosho", "12345");

            //Assert
            Assert.IsNull(user);
        }
    }
}
