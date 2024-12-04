using MembershipSystem.DB;
using MembershipSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MembershipSystem.Controllers
{
    public class MembershipController : DbConnection
    {
        private static MembershipController instancia = null;

        public static MembershipController Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new MembershipController();
                }
                return instancia;
            }
        }

        public List<Membership> GetAll()
        {
            List<Membership> oLista = new List<Membership>();
            try
            {
                using (SqlConnection con = GetConnection())
                {
                   con.Open();
                  
                    try
                    {
                        StringBuilder query = new StringBuilder();
                        query.AppendLine("SELECT m.*, CONCAT(u.first_name, ' ', u.last_name) as UserFullName");
                        query.AppendLine("FROM Memberships m");
                        query.AppendLine("INNER JOIN Users u ON m.user_id = u.id");

                        SqlCommand cmd = new SqlCommand(query.ToString(), con);
                        cmd.CommandType = System.Data.CommandType.Text;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            
                            while (dr.Read())
                            {
                                
                                oLista.Add(new Membership()
                                {
                                    Id = Convert.ToInt32(dr["id"]),
                                    UserId = Convert.ToInt32(dr["user_id"]),
                                    Name = dr["name"].ToString(),
                                    Description = dr["description"]?.ToString() ?? "",
                                    DurationMonths = Convert.ToInt32(dr["duration_months"]),
                                    StartDate = Convert.ToDateTime(dr["start_date"]),
                                    EndDate = Convert.ToDateTime(dr["end_date"]),
                                    Active = Convert.ToBoolean(dr["active"]),
                                    Price = Convert.ToDecimal(dr["price"]),
                                    UserFullName = dr["UserFullName"].ToString()
                                });
                            }
                           
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al ejecutar la consulta: {ex.Message}");
                        return oLista;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error general en GetAll: {ex.Message}");
                return oLista;
            }
            return oLista;
        }

        

        public int Registrar(Membership oMembership, out string mensaje)
        {
            mensaje = string.Empty;
            int respuesta = 0;
            SqlTransaction objTransaccion = null;

            using (SqlConnection con = GetConnection())
            {
                try
                {
                    con.Open();
                    objTransaccion = con.BeginTransaction();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("INSERT INTO Memberships (user_id, name, description, duration_months, start_date, end_date, active, price)");
                    query.AppendLine("VALUES (@UserId, @Name, @Description, @DurationMonths, @StartDate, @EndDate, @Active, @Price)");
                    query.AppendLine("SELECT SCOPE_IDENTITY()");

                    SqlCommand cmd = new SqlCommand(query.ToString(), con);
                    cmd.Parameters.AddWithValue("@UserId", oMembership.UserId);
                    cmd.Parameters.AddWithValue("@Name", oMembership.Name);
                    cmd.Parameters.AddWithValue("@Description", oMembership.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DurationMonths", oMembership.DurationMonths);
                    cmd.Parameters.AddWithValue("@StartDate", oMembership.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", oMembership.EndDate);
                    cmd.Parameters.AddWithValue("@Active", oMembership.Active);
                    cmd.Parameters.AddWithValue("@Price", oMembership.Price);

                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Transaction = objTransaccion;
                    respuesta = Convert.ToInt32(cmd.ExecuteScalar());

                    if (respuesta < 1)
                    {
                        objTransaccion.Rollback();
                        mensaje = "No se pudo registrar la membresía";
                    }

                    objTransaccion.Commit();
                }
                catch (Exception ex)
                {
                    objTransaccion?.Rollback();
                    respuesta = 0;
                    mensaje = ex.Message;
                }
            }

            return respuesta;
        }
        public int Update(Membership oMembership, out string message)
        {
            message = string.Empty;
            int respuesta = 0;
            SqlTransaction objTransaccion = null;
            using (SqlConnection con = GetConnection())
            {
                try
                {
                    // Establecer fecha de inicio como la fecha actual
                    oMembership.StartDate = DateTime.Now;
                    // Calcular fecha de fin basada en la duración
                    oMembership.EndDate = oMembership.StartDate.AddMonths(oMembership.DurationMonths);

                    con.Open();
                    objTransaccion = con.BeginTransaction();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("UPDATE Memberships SET ");
                    query.AppendLine("user_id = @UserId, ");
                    query.AppendLine("name = @Name, ");
                    query.AppendLine("description = @Description, ");
                    query.AppendLine("duration_months = @DurationMonths, ");
                    query.AppendLine("start_date = @StartDate, ");
                    query.AppendLine("end_date = @EndDate, ");
                    query.AppendLine("active = @Active, ");
                    query.AppendLine("price = @Price ");
                    query.AppendLine("WHERE id = @Id");

                    SqlCommand cmd = new SqlCommand(query.ToString(), con);
                    cmd.Parameters.AddWithValue("@Id", oMembership.Id);
                    cmd.Parameters.AddWithValue("@UserId", oMembership.UserId);
                    cmd.Parameters.AddWithValue("@Name", oMembership.Name);
                    cmd.Parameters.AddWithValue("@Description", (object)oMembership.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DurationMonths", oMembership.DurationMonths);
                    cmd.Parameters.AddWithValue("@StartDate", oMembership.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", oMembership.EndDate);
                    cmd.Parameters.AddWithValue("@Active", oMembership.Active);
                    cmd.Parameters.AddWithValue("@Price", oMembership.Price);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Transaction = objTransaccion;

                    respuesta = cmd.ExecuteNonQuery();

                    if (respuesta < 1)
                    {
                        objTransaccion.Rollback();
                        message = "No se pudo editar los datos de la membresía";
                    }
                    objTransaccion.Commit();
                }
                catch (Exception ex)
                {
                    objTransaccion?.Rollback();
                    respuesta = 0;
                    message = ex.Message;
                }
            }
            return respuesta;
        }

        public bool TienePagosPendientes(int membershipId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = "SELECT COUNT(*) FROM Payments WHERE membership_id = @id AND status = 'PENDIENTE'";
                    command.Parameters.AddWithValue("@id", membershipId);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public void Delete(int id)
        {
            if (TienePagosPendientes(id))
            {
                throw new Exception("No se puede eliminar la membresía porque tiene pagos pendientes. Resuelva los pagos pendientes antes de eliminar.");
            }

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;

                    try
                    {
                        // Primero eliminamos todos los pagos (que no están pendientes)
                        command.CommandText = "DELETE FROM Payments WHERE membership_id = @id";
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();

                        // Luego eliminamos la membresía
                        command.CommandText = "DELETE FROM Memberships WHERE id = @id";
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception("No se encontró la membresía especificada");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al eliminar: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public Membership GetById(int id)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    string query = @"
                SELECT m.*, CONCAT(u.first_name, ' ', u.last_name) as UserFullName 
                FROM memberships m
                LEFT JOIN users u ON m.user_id = u.id
                WHERE m.id = @Id";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Membership
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Name = reader["name"].ToString(),
                                    Description = reader["description"].ToString(),
                                    UserId = Convert.ToInt32(reader["user_id"]),
                                    UserFullName = reader["UserFullName"]?.ToString(),
                                    DurationMonths = Convert.ToInt32(reader["duration_months"]),
                                    Price = Convert.ToDecimal(reader["price"]),
                                    Active = Convert.ToBoolean(reader["active"]),
                                    StartDate = reader["start_date"] != DBNull.Value
                                        ? Convert.ToDateTime(reader["start_date"])
                                        : DateTime.Now,
                                    EndDate = reader["end_date"] != DBNull.Value
                                        ? Convert.ToDateTime(reader["end_date"])
                                        : DateTime.Now.AddMonths(Convert.ToInt32(reader["duration_months"]))
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en GetById: {ex.Message}");
                throw;
            }
        }

        public void VerificarYActualizarEstadoMembresias()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    string query = @"
                    UPDATE Memberships 
                    SET active = 0 
                    WHERE end_date < GETDATE() 
                    AND active = 1";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al actualizar estados: {ex.Message}");
            }
        }

        public int Renovar(int membershipId, out string mensaje)
        {
            mensaje = string.Empty;
            try
            {
                // Obtener la membresía actual
                var membership = GetById(membershipId);
                if (membership == null)
                {
                    mensaje = "No se encontró la membresía";
                    return 0;
                }

                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 1. Actualizar la membresía
                            string updateQuery = @"
                        UPDATE Memberships 
                        SET start_date = @StartDate,
                            end_date = @EndDate,
                            active = 1
                        WHERE id = @Id";

                            using (var command = new SqlCommand(updateQuery, connection, transaction))
                            {
                                DateTime startDate = DateTime.Now;
                                command.Parameters.AddWithValue("@StartDate", startDate);
                                command.Parameters.AddWithValue("@EndDate", startDate.AddMonths(membership.DurationMonths));
                                command.Parameters.AddWithValue("@Id", membershipId);

                                int result = command.ExecuteNonQuery();

                                if (result <= 0)
                                {
                                    transaction.Rollback();
                                    mensaje = "No se pudo renovar la membresía";
                                    return 0;
                                }
                            }

                            // 2. Insertar el pago pendiente directamente
                            string insertPaymentQuery = @"
                        INSERT INTO Payments (membership_id, amount, status, payment_date, reference)
                        VALUES (@MembershipId, @Amount, @Status, @PaymentDate, @Reference)";

                            using (var command = new SqlCommand(insertPaymentQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@MembershipId", membershipId);
                                command.Parameters.AddWithValue("@Amount", membership.Price);
                                command.Parameters.AddWithValue("@Status", "PENDIENTE");
                                command.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                                command.Parameters.AddWithValue("@Reference", $"RENEW-{DateTime.Now:yyyyMMddHHmmss}");

                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return membershipId;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            mensaje = $"Error al renovar: {ex.Message}";
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
                return 0;
            }
        }


        public bool EstaMembresiaVencida(int membershipId)
        {
            var membership = GetById(membershipId);
            return membership != null && membership.EndDate < DateTime.Now;
        }

    }
}
