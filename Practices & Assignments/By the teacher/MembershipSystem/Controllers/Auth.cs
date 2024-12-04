using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MembershipSystem.Models;
using MembershipSystem.DB;

namespace MembershipSystem.Controllers
{
    public class Auth : DbConnection
    {
        private static readonly object _lock = new object();
        private static Auth _instance;

        // Private constructor that calls the base constructor
        private Auth() : base()
        {
        }

        public static Auth Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Auth();
                        }
                    }
                }
                return _instance;
            }
        }

        public async Task<Employee> GetUserAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Username and password are required.");
            }

            Employee user = null;

            try
            {
                using (var connection = GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_GetUser", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password); // Password sin hash

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                user = new Employee
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AuthenticationException("Error in authentication.", ex);
            }

            return user;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}