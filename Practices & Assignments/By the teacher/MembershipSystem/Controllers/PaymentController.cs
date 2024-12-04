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
    public class PaymentController : DbConnection
    {
        private static PaymentController instancia = null;

        public static PaymentController Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new PaymentController();
                }
                return instancia;
            }
        }

        public List<Payment> GetAll()
        {
            List<Payment> oLista = new List<Payment>();
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    
                    con.Open();
                    

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT p.*, m.name as MembershipName,");
                    query.AppendLine("CONCAT(u.first_name, ' ', u.last_name) as UserFullName");
                    query.AppendLine("FROM Payments p");
                    query.AppendLine("INNER JOIN Memberships m ON p.membership_id = m.id");
                    query.AppendLine("INNER JOIN Users u ON m.user_id = u.id");

                    SqlCommand cmd = new SqlCommand(query.ToString(), con);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        
                        while (dr.Read())
                        {
                            Payment payment = new Payment();
                            payment.Id = Convert.ToInt32(dr["id"]);
                            payment.MembershipId = Convert.ToInt32(dr["membership_id"]);
                            payment.Amount = Convert.ToDecimal(dr["amount"]);
                            payment.PaymentDate = Convert.ToDateTime(dr["payment_date"]);
                            payment.Status = dr["status"].ToString();
                            payment.Reference = dr["reference"]?.ToString();
                            payment.MembershipName = dr["MembershipName"].ToString();
                            payment.UserFullName = dr["UserFullName"].ToString();

                            oLista.Add(payment);
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en GetAll: {ex.Message}\n\nStack Trace: {ex.StackTrace}");
                return new List<Payment>();
            }

            return oLista;
        }

        public int Registrar(Payment oPayment, out string mensaje)
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

                    query.AppendLine("INSERT INTO Payments (membership_id, amount, payment_date, status, reference)");
                    query.AppendLine("VALUES (@MembershipId, @Amount, @PaymentDate, @Status, @Reference)");
                    query.AppendLine("SELECT SCOPE_IDENTITY()");

                    SqlCommand cmd = new SqlCommand(query.ToString(), con);
                    cmd.Parameters.AddWithValue("@MembershipId", oPayment.MembershipId);
                    cmd.Parameters.AddWithValue("@Amount", oPayment.Amount);
                    cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Status", oPayment.Status);
                    cmd.Parameters.AddWithValue("@Reference", $"NEW-{DateTime.Now:yyyyMMddHHmmss}" ?? (object)DBNull.Value);

                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Transaction = objTransaccion;
                    respuesta = Convert.ToInt32(cmd.ExecuteScalar());

                    if (respuesta < 1)
                    {
                        objTransaccion.Rollback();
                        mensaje = "No se pudo registrar el pago";
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

        public List<Payment> GetByMembership(int membershipId)
        {
            List<Payment> oLista = new List<Payment>();
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT p.*, m.name as MembershipName,");
                    query.AppendLine("CONCAT(u.first_name, ' ', u.last_name) as UserFullName");
                    query.AppendLine("FROM Payments p");
                    query.AppendLine("INNER JOIN Memberships m ON p.membership_id = m.id");
                    query.AppendLine("INNER JOIN Users u ON m.user_id = u.id");
                    query.AppendLine("WHERE p.membership_id = @MembershipId");

                    SqlCommand cmd = new SqlCommand(query.ToString(), con);
                    cmd.Parameters.AddWithValue("@MembershipId", membershipId);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Payment()
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                MembershipId = Convert.ToInt32(dr["membership_id"]),
                                Amount = Convert.ToDecimal(dr["amount"]),
                                PaymentDate = Convert.ToDateTime(dr["payment_date"]),
                                Status = dr["status"].ToString(),
                                Reference = dr["reference"]?.ToString() ?? "",
                                MembershipName = dr["MembershipName"].ToString(),
                                UserFullName = dr["UserFullName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener pagos por membresía: {ex.Message}");
            }
            return oLista;
        }

        public bool ActualizarEstado(Payment payment, out string mensaje)
        {
            mensaje = string.Empty;
            bool respuesta = false;
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("UPDATE Payments SET");
                    query.AppendLine("status = @status");
                    query.AppendLine("WHERE id = @id");

                    SqlCommand cmd = new SqlCommand(query.ToString(), con);
                    cmd.Parameters.AddWithValue("@status", payment.Status);
                    cmd.Parameters.AddWithValue("@id", payment.Id);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        respuesta = true;
                    }
                    else
                    {
                        mensaje = "No se pudo actualizar el estado del pago";
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}
