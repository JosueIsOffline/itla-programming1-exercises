using System;
using System.Collections.Generic;
using MembershipSystem.DB;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MembershipSystem.Models;
using System.Data.SqlClient;

namespace MembershipSystem.Controllers
{
    public class UserController : DbConnection
    {

        private static UserController instancia = null;

        public static UserController Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new UserController();
                }
                return instancia;
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> oLista = new List<User>();
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT id, first_name, last_name, email, phone, address, registration_date, active FROM Users");

                    SqlCommand cmd = new SqlCommand(query.ToString(), con);
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new User()
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                FirstName = dr["first_name"].ToString(),
                                LastName = dr["last_name"].ToString(),
                                Email = dr["email"].ToString(),
                                Phone = dr["phone"].ToString(),
                                Address = dr["address"].ToString(),
                                RegistrationDate = Convert.ToDateTime(dr["registration_date"]),
                                Active = Convert.ToBoolean(dr["active"])
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                oLista = new List<User>();
            }
            return oLista;
        }

        public int Registrar(User oUser, out string mensaje)
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

                    query.AppendLine("INSERT INTO Users (first_name, last_name, email, phone, address, registration_date, active) ");
                    query.AppendLine("VALUES (@FirstName, @LastName, @Email, @Phone, @Address, @RegistrationDate, @Active)");
                    query.AppendLine("SELECT SCOPE_IDENTITY()");

                    SqlCommand cmd = new SqlCommand(query.ToString(), con);
                    cmd.Parameters.AddWithValue("@FirstName", oUser.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", oUser.LastName);
                    cmd.Parameters.AddWithValue("@Email", oUser.Email);
                    cmd.Parameters.AddWithValue("@Phone", oUser.Phone);
                    cmd.Parameters.AddWithValue("@Address", oUser.Address);
                    cmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Active", true);

                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Transaction = objTransaccion;
                    respuesta = Convert.ToInt32(cmd.ExecuteScalar());

                    if (respuesta < 1)
                    {
                        objTransaccion.Rollback();
                        mensaje = "No se pudo registrar el usuario";
                    }

                    objTransaccion.Commit();
                }
                catch (Exception ex)
                {
                    objTransaccion.Rollback();
                    respuesta = 0;
                    mensaje = ex.Message;
                }
            }

            return respuesta;
        }

        public int Editar(User oUser, out string mensaje)
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

                    query.AppendLine("UPDATE Users SET ");
                    query.AppendLine("first_name = @FirstName, ");
                    query.AppendLine("last_name = @LastName, ");
                    query.AppendLine("email = @Email, ");
                    query.AppendLine("phone = @Phone, ");
                    query.AppendLine("address = @Address, ");
                    query.AppendLine("active = @Active ");
                    query.AppendLine("WHERE id = @Id");

                    SqlCommand cmd = new SqlCommand(query.ToString(), con);
                    cmd.Parameters.AddWithValue("@Id", oUser.Id);
                    cmd.Parameters.AddWithValue("@FirstName", oUser.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", oUser.LastName);
                    cmd.Parameters.AddWithValue("@Email", oUser.Email);
                    cmd.Parameters.AddWithValue("@Phone", oUser.Phone);
                    cmd.Parameters.AddWithValue("@Address", oUser.Address);
                    cmd.Parameters.AddWithValue("@Active", oUser.Active);

                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Transaction = objTransaccion;
                    respuesta = cmd.ExecuteNonQuery();

                    if (respuesta < 1)
                    {
                        objTransaccion.Rollback();
                        mensaje = "No se pudo editar los datos del usuario";
                    }

                    objTransaccion.Commit();
                }
                catch (Exception ex)
                {
                    objTransaccion.Rollback();
                    respuesta = 0;
                    mensaje = ex.Message;
                }
            }

            return respuesta;
        }

        public int Eliminar(int id)
        {
            int respuesta = 0;

            using (SqlConnection con = GetConnection())
            {
                try
                {
                    con.Open();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("DELETE FROM Users WHERE id = @id");
                    SqlCommand cmd = new SqlCommand(query.ToString(), con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = System.Data.CommandType.Text;

                    respuesta = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    respuesta = 0;
                }
            }
            return respuesta;
        }
    }
}
