using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TalanUserAccessLayer.DTO;
using TalanUserAccessLayer.Models;

namespace TalanUserAccessLayer.BusinessLogic
{
    public class UsersBL : IUsersBL
    {
        private IDbConnection db;

        public UsersBL(string connectionString)
        {
            db = new SqlConnection(connectionString);
        }

        public async Task<User> LogIn(AuthenticationUser authenticationUser)
        {
            try
            {
                string userSaltedAndHashedPassword = FindUserSaltedAndHashedPassword(authenticationUser);

                (string salt, string hash) = SeperatingSaltAndHash(userSaltedAndHashedPassword);

                string userSaltedPassword = authenticationUser.Password + salt;

                string userHashedPassword = HashPassword(userSaltedPassword);

                if (!hash.Equals(userHashedPassword, StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Invalid username/password");

                User user = await RetrieveUserInfo(userSaltedAndHashedPassword);

                return user;

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
             
        }

        private string FindUserSaltedAndHashedPassword(AuthenticationUser authenticationUser)
        {
            var saltedQuery = $"Select [SaltedAndHashedPassword] from [Users] " +
                $" Where [Username] = '{authenticationUser.Login}'" +
                $" Or [Email] = '{authenticationUser.Login}'";

            string userSaltedAndHashedPassword = db.QueryFirstOrDefault<string>(saltedQuery);

            if (userSaltedAndHashedPassword == null)
                throw new Exception("Invalid login");

            return userSaltedAndHashedPassword;
        }


        private static Tuple<string, string> SeperatingSaltAndHash(string userSaltedAndHashedPassword)
        {
            int numberOfHexadecimalBitsInSHA256Hash = 64;
            string salt = userSaltedAndHashedPassword.Substring(numberOfHexadecimalBitsInSHA256Hash);
            string hash = userSaltedAndHashedPassword.Remove(numberOfHexadecimalBitsInSHA256Hash);
            return Tuple.Create(salt, hash);
        }

        private static string HashPassword(string saltedPassword)
        {
            var hashProvider = SHA256.Create();

            var bytesOfSaltedPassword = Encoding.UTF8.GetBytes(saltedPassword);
            var bytesHashed = hashProvider.ComputeHash(bytesOfSaltedPassword);
            var bytesHashedConvertedtoString = Array.ConvertAll(bytesHashed, h => h.ToString("X2"));
            var hashedPassword = string.Concat(bytesHashedConvertedtoString);

            return hashedPassword;
        }

        private async Task<User> RetrieveUserInfo(string userSaltedAndHashedPassword)
        {
            var sql = $"Select [{nameof(User.FirstName)}], [LastName], [Email], [Username], [Admin]" +
                " From [Users]" +
                $" Where [SaltedAndHashedPassword] = '{userSaltedAndHashedPassword}'";

            var queryUser = await db.QueryFirstOrDefaultAsync<User>(sql);

            return queryUser;
        }

        
        public async Task<User> SignUp(CreationUser creationUser)
        {
            try
            {
                if (creationUser.Email == "" || creationUser.FirstName == "" || creationUser.Password == "")
                    throw new Exception("Your profile needs at least a first name, an e-mail and a password !");

                User userToAdd = TransformCreationUserIntoUser(creationUser);

                var sql = "Insert Into Users (FirstName, LastName, Email, Username, SaltedAndHashedPassword, Admin)" +
                   $" Values('{userToAdd.FirstName}', '{userToAdd.LastName}', '{userToAdd.Email}', " +
                   $"'{userToAdd.Username}', '{userToAdd.SaltedAndHashedPassword}', '{userToAdd.Admin}');" +
                   " Select Cast(Scope_Identity() as int);";
                await db.ExecuteAsync(sql, userToAdd);

                userToAdd.SaltedAndHashedPassword = "";

                return userToAdd;

            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        private static User TransformCreationUserIntoUser(CreationUser creationUser)
        {
            string saltedAndHashedPassword = HashingProcess(creationUser);

            User user = new User
            {
                FirstName = creationUser.FirstName,
                LastName = creationUser.LastName,
                Email = creationUser.Email,
                Username = creationUser.Username,
                SaltedAndHashedPassword = saltedAndHashedPassword,
                Admin = creationUser.Admin
            };

            return user;
        }

        private static string HashingProcess(CreationUser creationUser)
        {
            string saltForPassword = ProvideSaltForThePassword();
            string saltedPassword = creationUser.Password + saltForPassword;
            string hashedPassword = HashPassword(saltedPassword);
            string saltedAndHashedPassword = hashedPassword + saltForPassword;

            return saltedAndHashedPassword;
        }

        private static string ProvideSaltForThePassword()
        {
            byte[] arrayOfBytes = new byte[15];
            var randomProvider = RandomNumberGenerator.Create();
            randomProvider.GetBytes(arrayOfBytes);
            var saltedBytes = String.Concat(arrayOfBytes);

            return saltedBytes;

        }


    }
}
