using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MembershipSystem.Models;
using MembershipSystem.DB;

namespace MembershipSystem.Controllers
{
    public class DashboardController : DbConnection
    {
        public Dashboard GetDashboardData()
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    var dashboard = new Dashboard();

                    try
                    {
                        using (var command = new SqlCommand("SELECT COUNT(*) FROM Users", connection))
                        {
                            dashboard.TotalMembers = (int)command.ExecuteScalar();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error en consulta de Total Members: " + ex.Message);
                    }

                    try
                    {
                        using (var command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE active = 1", connection))
                        {
                            dashboard.ActiveMembers = (int)command.ExecuteScalar();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error en consulta de Active Members: " + ex.Message);
                    }

                    try
                    {
                        using (var command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE active = 0", connection))
                        {
                            dashboard.InactiveMembers = (int)command.ExecuteScalar();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error en consulta de Inactive Members: " + ex.Message);
                    }

                    try
                    {
                        using (var command = new SqlCommand("SELECT ISNULL(SUM(amount), 0) FROM Payments WHERE status = 'COMPLETADO'", connection))
                        {
                            dashboard.TotalRevenue = (decimal)command.ExecuteScalar();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error en consulta de Total Revenue: " + ex.Message);
                    }

                    try
                    {
                        using (var command = new SqlCommand(@"
                        SELECT COUNT(*) 
                        FROM Users 
                        WHERE MONTH(registration_date) = MONTH(GETDATE()) 
                        AND YEAR(registration_date) = YEAR(GETDATE())", connection))
                        {
                            dashboard.NewMembersThisMonth = (int)command.ExecuteScalar();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error en consulta de New Members This Month: " + ex.Message);
                    }

                    return dashboard;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error específico: {ex.Message}", ex);
                }
            }
        }

        private List<string> GetRecentActivitiesWithConnection(SqlConnection connection)
        {
            var activities = new List<string>();
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = @"
                    SELECT TOP 10
                        CONCAT(u.first_name, ' ', u.last_name, ' ', 
                            CASE 
                                WHEN p.id IS NOT NULL THEN 'realizó un pago de $' + CAST(p.amount AS VARCHAR)
                                WHEN m.id IS NOT NULL THEN 'adquirió una membresía ' + mt.name
                            END,
                            ' el ', FORMAT(COALESCE(p.payment_date, m.start_date), 'dd/MM/yyyy')
                        ) as activity
                    FROM Users u
                    LEFT JOIN Memberships m ON u.id = m.user_id
                    LEFT JOIN Membership_Types mt ON m.membership_type_id = mt.id
                    LEFT JOIN Payments p ON m.id = p.membership_id
                    WHERE COALESCE(p.payment_date, m.start_date) IS NOT NULL
                    ORDER BY COALESCE(p.payment_date, m.start_date) DESC";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        activities.Add(reader.GetString(0));
                    }
                }
            }
            return activities;
        }

        public int RefreshTotalMembers()
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT COUNT(*) FROM Users", connection))
                    {
                        return (int)command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar total de miembros", ex);
                }
            }
        }

        public int RefreshActiveMembers()
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE active = 1", connection))
                    {
                        return (int)command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar miembros activos", ex);
                }
            }
        }

        public decimal RefreshTotalRevenue()
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT ISNULL(SUM(amount), 0) FROM Payments WHERE status = 'COMPLETADO'", connection))
                    {
                        return (decimal)command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar ingresos totales", ex);
                }
            }
        }

        public List<string> RefreshRecentActivities()
        {
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    return GetRecentActivitiesWithConnection(connection);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar actividades recientes", ex);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Liberar recursos manejados si es necesario
            }
            base.Dispose(disposing);
        }
    }
}