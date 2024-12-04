using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MembershipSystem.Controllers;
using MembershipSystem.Models;

namespace MembershipSystem.Forms
{
    public partial class frmMembers : Form
    {

        

        public string IdUser { get; set; }
        public string _first_name { get; set; }
        public string _last_name { get; set; }
        public string _email { get; set; }
        public string _phone { get; set; }
        public string _address { get; set; }
        public Boolean _IsActive { get; set; }

        public User oUser { get; set; }
        public bool Editar { get; set; }

        public frmMembers()
        {
            InitializeComponent();
            
        }

        private void frmMembers_Load(object sender, EventArgs e)
        {

            ConfigurarColumnas();

            var users = UserController.Instancia.GetAllUsers();
            foreach ( User user in users)
            {
                dgvdata.Rows.Add(new object[]
                    {
                        "",
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.Phone,
                        user.Address,
                        user.Active
                    });
            }

            if(oUser != null)
            {
                txtFirstName.Text = oUser.FirstName;
                txtLastName.Text = oUser.LastName;
                txtEmail.Text = oUser.Email;
                txtPhone.Text = oUser.Phone;
                txtAddress.Text = oUser.Address;
                radioState.Checked = oUser.Active;
            }


        }

        private void ConfigurarColumnas()
        {
            dgvdata.Columns.Clear();
            dgvdata.EnableHeadersVisualStyles = false;
            dgvdata.BackgroundColor = Color.White;
            dgvdata.BorderStyle = BorderStyle.None;
            dgvdata.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvdata.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Estilos del encabezado
            dgvdata.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
            dgvdata.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvdata.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            dgvdata.ColumnHeadersHeight = 30;

            // Estilos de las celdas
            dgvdata.DefaultCellStyle.BackColor = Color.White;
            dgvdata.DefaultCellStyle.ForeColor = Color.Black;
            dgvdata.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dgvdata.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvdata.DefaultCellStyle.SelectionForeColor = Color.FromArgb(45, 66, 91);

            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.Name = "btnseleccionar";
            btnColumn.HeaderText = "";
            btnColumn.Width = 30;

            
            dgvdata.Columns.Add(btnColumn);
            dgvdata.Columns.Add("id", "ID");
            dgvdata.Columns.Add("first_name", "Nombre");
            dgvdata.Columns.Add("last_name", "Apellido");
            dgvdata.Columns.Add("email", "Email");
            dgvdata.Columns.Add("phone", "Teléfono");
            dgvdata.Columns.Add("address", "Dirección");
            dgvdata.Columns.Add("active", "Activo");

            
            //dgvdata.Columns["id"].Visible = false; 
        }

        //private void LoadUsers()
        //{
        //    try
        //    {
        //        var users = _userController.GetAllUsers();

        //        dgvdata.Rows.Clear();
        //        foreach (User user in users)
        //        {
        //            dgvdata.Rows.Add(new object[]
        //            {
        //                "",
        //                user.Id,
        //                user.FirstName,
        //                user.LastName,
        //                user.Email,
        //                user.Phone,
        //                user.Address,
        //                user.Active
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}



        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void radioState_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioState_Click(object sender, EventArgs e)
        {
            if (radioState.Text == "Active")
            {
                radioState.Text = "Inactive";
                radioState.Checked = false;
            }
            else
            {
                radioState.Text = "Active";
                radioState.Checked = true;
            }
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;

            if (index >= 0)
            {
                if (dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
                {
                    oUser = new User()
                    {
                        Id = Convert.ToInt32(dgvdata.Rows[index].Cells["id"].Value),
                        FirstName = dgvdata.Rows[index].Cells["first_name"].Value.ToString(),
                        LastName = dgvdata.Rows[index].Cells["last_name"].Value.ToString(),
                        Email = dgvdata.Rows[index].Cells["email"].Value.ToString(),
                        Phone = dgvdata.Rows[index].Cells["phone"].Value.ToString(),
                        Address = dgvdata.Rows[index].Cells["address"].Value.ToString(),
                        Active = Convert.ToInt32(dgvdata.Rows[index].Cells["active"].Value) == 1
                    };

                    txtFirstName.Text = oUser.FirstName;
                    txtLastName.Text = oUser.LastName;
                    txtEmail.Text = oUser.Email;
                    txtPhone.Text = oUser.Phone;
                    txtAddress.Text = oUser.Address;
                    radioState.Checked = oUser.Active;



                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Debe ingresar los campos obligatorios", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (oUser != null)
            {
                int _idtemp = oUser.Id;
                int nroperacion = UserController.Instancia.Editar(new User()
                {
                    Id = _idtemp,
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text,
                    Address = txtAddress.Text,
                    Active = radioState.Checked
                }, out mensaje);

                if (nroperacion < 1)
                {
                    MessageBox.Show(mensaje, "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    oUser = new User()
                    {
                        Id = _idtemp,
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        Email = txtEmail.Text,
                        Phone = txtPhone.Text,
                        Address = txtAddress.Text,
                        Active = radioState.Checked
                    };
                    MessageBox.Show("Usuario actualizado con éxito", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    CargarDatos();
                }
            }
            else // Modo Nuevo Registro
            {
                int idgenerado = UserController.Instancia.Registrar(new User()
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text,
                    Address = txtAddress.Text,
                    Active = radioState.Checked
                }, out mensaje);

                if (idgenerado < 1)
                {
                    MessageBox.Show(mensaje, "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    oUser = new User()
                    {
                        Id = idgenerado,
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        Email = txtEmail.Text,
                        Phone = txtPhone.Text,
                        Address = txtAddress.Text,
                        Active = radioState.Checked
                    };
                    MessageBox.Show("Usuario registrado con éxito", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    CargarDatos();
                }
            }
        }

        private void LimpiarFormulario()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            radioState.Checked = true;
            oUser = null;
        }

        private void CargarDatos()
        {
            try
            {
                dgvdata.Rows.Clear();
                var users = UserController.Instancia.GetAllUsers();
                foreach (User user in users)
                {
                    dgvdata.Rows.Add(new object[]
                    {
                "",
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Phone,
                user.Address,
                user.Active
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvdata_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvdata.Columns["active"].Index && e.Value != null)
            {
                bool isActive = Convert.ToBoolean(e.Value);
                var row = dgvdata.Rows[e.RowIndex];

                if (!isActive)
                {
                    e.Value = "INACTIVO";
                    e.CellStyle.ForeColor = Color.Red;
                }
                else
                {
                    e.Value = "ACTIVO";
                    e.CellStyle.ForeColor = Color.Green;
                }
                e.CellStyle.Font = new Font(dgvdata.DefaultCellStyle.Font, FontStyle.Bold);
                e.FormattingApplied = true;
            }
        }
    }
}
