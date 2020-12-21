using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.CoSFE.DatabaseUtils;
using IIG.PasswordHashingUtils;

namespace IntegrationTesting
{
    [TestClass]
    public class CredentialsTest
    {
        private const string Server = @"DESKTOP-5K2F4JH";
        private const string Database = @"IIG.CoSWE.AuthDB";
        private const bool IsTrusted = false;
        private const string Login = @"sa";
        private const string Password = @"123456";
        private const int ConnectionTime = 75;

        AuthDatabaseUtils authDatabaseUtils = new AuthDatabaseUtils(Server, Database, IsTrusted, Login, Password, ConnectionTime);

        [TestMethod]
        public void AddCreadentialsTest()
        {
            Assert.IsTrue(authDatabaseUtils.AddCredentials("login", PasswordHasher.GetHash("password")));
           
            Assert.IsTrue(authDatabaseUtils.AddCredentials("another login 1", PasswordHasher.GetHash("password")));
            Assert.IsTrue(authDatabaseUtils.AddCredentials("another login 2", PasswordHasher.GetHash("pass")));
            Assert.IsTrue(authDatabaseUtils.AddCredentials("another login 3", PasswordHasher.GetHash("")));

            Assert.IsFalse(authDatabaseUtils.AddCredentials("login", PasswordHasher.GetHash("password")));
            Assert.IsFalse(authDatabaseUtils.AddCredentials("another login again", "password"));
        }

        [TestMethod]
        public void UpdateCreadentialsTest()
        {
            authDatabaseUtils.AddCredentials("login", PasswordHasher.GetHash("password"));
            Assert.IsFalse(authDatabaseUtils.UpdateCredentials(
                "login1", PasswordHasher.GetHash("password1"),
                "login", PasswordHasher.GetHash("password")
                ));

            Assert.IsFalse(authDatabaseUtils.UpdateCredentials(
                "login1", PasswordHasher.GetHash("password"),
                "login", PasswordHasher.GetHash("password")
                ));

            Assert.IsFalse(authDatabaseUtils.UpdateCredentials(
                "login", PasswordHasher.GetHash("password1"),
                "login", PasswordHasher.GetHash("password")
                ));

           Assert.IsFalse(authDatabaseUtils.UpdateCredentials(
                "login", PasswordHasher.GetHash("password"),
                "another login 1", PasswordHasher.GetHash("password")
                ));

            Assert.IsFalse(authDatabaseUtils.UpdateCredentials(
                "login", PasswordHasher.GetHash("password"),
                "login", "password"
                ));

            Assert.IsTrue(authDatabaseUtils.UpdateCredentials(
                "login", PasswordHasher.GetHash("password"),
                "login", PasswordHasher.GetHash("password")
                ));

            Assert.IsTrue(authDatabaseUtils.UpdateCredentials(
                "login", PasswordHasher.GetHash("password"),
                "update", PasswordHasher.GetHash("update")
                ));
        }

        [TestMethod]
        public void CheckCreadentialsTest()
        {
            Assert.IsTrue(authDatabaseUtils.CheckCredentials("login", PasswordHasher.GetHash("password")));

            Assert.IsTrue(authDatabaseUtils.CheckCredentials("another login 1", PasswordHasher.GetHash("password")));
            Assert.IsTrue(authDatabaseUtils.CheckCredentials("another login 2", PasswordHasher.GetHash("pass")));
            Assert.IsTrue(authDatabaseUtils.CheckCredentials("another login 3", PasswordHasher.GetHash("")));

            Assert.IsFalse(authDatabaseUtils.CheckCredentials("another login 1", PasswordHasher.GetHash("password password")));
            Assert.IsFalse(authDatabaseUtils.CheckCredentials("another login 5", PasswordHasher.GetHash("pass")));
        }

        [TestMethod]
        public void DeleteCreadentialsTest()
        {
            Assert.IsTrue(authDatabaseUtils.DeleteCredentials("login", PasswordHasher.GetHash("password")));
            Assert.IsTrue(authDatabaseUtils.DeleteCredentials("login", PasswordHasher.GetHash("password")));

            Assert.IsTrue(authDatabaseUtils.DeleteCredentials("another login 1", PasswordHasher.GetHash("password password")));
            Assert.IsTrue(authDatabaseUtils.DeleteCredentials("another login 5", PasswordHasher.GetHash("pass")));
        }

    }
}
