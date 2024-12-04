using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MembershipSystem.DB
{
    public abstract class DbConnection : IDisposable
    {
        private readonly string _connectionString;
        private bool _disposed;

        protected DbConnection()
        {
            try
            {
                _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new Exception("Error getting connection string. Please verify configuration file.", ex);
            }
        }

        protected SqlConnection GetConnection()
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                return connection;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error creating database connection.", ex);
            }
        }

        // Clean up managed and unmanaged resources
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Clean up managed resources
                }
                _disposed = true;
            }
        }

        // Implementation of IDisposable pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}