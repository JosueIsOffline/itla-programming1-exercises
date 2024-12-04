using MembershipSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MembershipSystem.Controllers;
using System.Windows.Input;

namespace MembershipSystem.Forms
{
    public partial class frmMemberShips : Form
    {

        public string IdUser { get; set; }
        public string _name { get; set; }
        public string _description { get; set; }
        public string _duration { get; set; }
        public string _price { get; set; }
        public Boolean _IsActive { get; set; }

        public Membership oMembership { get; set; }
        public bool Editar { get; set; }

        public frmMemberShips()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmMemberShips_Load(object sender, EventArgs e)
        {

            try
            {
                System.Diagnostics.Debug.WriteLine("Iniciando carga del formulario");

                ConfigurarColumnas();
                System.Diagnostics.Debug.WriteLine("Columnas configuradas");

                var memberships = MembershipController.Instancia.GetAll();

                if (memberships.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No se encontraron membresías");
                    return;
                }

                foreach (Membership membership in memberships)
                {
                    try
                    {
                        
                        dgvdata.Rows.Add(new object[]
                        {
                    "",
                    membership.Id,
                    membership.Name,
                    membership.Description,
                    membership.UserFullName,
                    membership.UserId,
                    membership.DurationMonths,
                    membership.Price.ToString("C2"),
                    membership.Active,
                    membership.EndDate
                        });

                        
                        if(oMembership != null)
                        {
                            txtName.Text = oMembership.Name;
                            textDescription.Text = oMembership.Description;
                            txtUserId.Text = oMembership.UserId.ToString();
                            textDuration.Text = oMembership.DurationMonths.ToString();
                            textPrice.Text = oMembership.Price.ToString();
                            radioState.Checked = oMembership.Active;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al agregar fila: {ex.Message}");
                    }
                }

                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en Load: {ex.Message}");
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Configurar la columna del botón seleccionar
            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.Name = "btnseleccionar";
            btnColumn.HeaderText = "";
            btnColumn.Width = 30;

            // Agregar las columnas
            dgvdata.Columns.Add(btnColumn);
            dgvdata.Columns.Add("id", "ID");
            dgvdata.Columns.Add("name", "Nombre");
            dgvdata.Columns.Add("description", "Descripción");
            dgvdata.Columns.Add("user_fullname", "Usuario");
            dgvdata.Columns.Add("user_id", "UserId");
            dgvdata.Columns.Add("duration_months", "Duración (meses)");
            dgvdata.Columns.Add("price", "Precio");
            dgvdata.Columns.Add("active", "Estado");
            dgvdata.Columns.Add("end_date", "Fecha de Vencimiento");

            // Configuraciones adicionales
            //dgvdata.Columns["id"].Visible = false; 
            dgvdata.Columns["user_id"].Visible = false;
            
        }

        private void radioState_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioState_Click(object sender, EventArgs e)
        {
            if (radioState.Text == "Activa")
            {
                radioState.Text = "Inactiva";
                radioState.Checked = false;
            }
            else
            {
                radioState.Text = "Activa";
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
                    string precioStr = dgvdata.Rows[index].Cells["price"].Value.ToString()
                   .Replace("$", "")
                   .Replace(",", "")
                   .Replace(" ", "")
                   .Trim();

                    decimal precio;
                    if (!decimal.TryParse(precioStr, out precio))
                    {
                        MessageBox.Show("Error al convertir el precio", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    oMembership = new Membership()
                    {
                        Id = Convert.ToInt32(dgvdata.Rows[index].Cells["id"].Value),
                        Name = dgvdata.Rows[index].Cells["name"].Value.ToString(),
                        Description = dgvdata.Rows[index].Cells["description"].Value.ToString(),
                        UserId = Convert.ToInt32(dgvdata.Rows[index].Cells["user_id"].Value),
                        DurationMonths = Convert.ToInt32(dgvdata.Rows[index].Cells["duration_months"].Value.ToString()),
                        Price = precio,
                        Active = Convert.ToInt32(dgvdata.Rows[index].Cells["active"].Value) == 1
                    };

                    txtName.Text = oMembership.Name;
                    textDescription.Text = oMembership.Description;
                    txtUserId.Text = oMembership.UserId.ToString();
                    textDuration.Text = oMembership.DurationMonths.ToString();
                    textPrice.Text = precio.ToString();
                    radioState.Checked = oMembership.Active;

                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {

                if (oMembership != null)
                {
                    MessageBox.Show("No se puede registrar una nueva membresía mientras hay una seleccionada. " +
                        "Por favor, limpie el formulario antes de registrar una nueva.",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mensaje = string.Empty;

                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(txtName.Text) ||
                    string.IsNullOrWhiteSpace(textDescription.Text) ||
                    string.IsNullOrWhiteSpace(textDuration.Text) ||
                    string.IsNullOrWhiteSpace(textPrice.Text))
                {
                    MessageBox.Show("Por favor complete todos los campos requeridos", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar que duración y precio sean números válidos
                if (!int.TryParse(textDuration.Text, out int duracion))
                {
                    MessageBox.Show("La duración debe ser un número válido", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(textPrice.Text, out decimal precio))
                {
                    MessageBox.Show("El precio debe ser un número válido", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crear objeto Membership
                Membership nuevaMembership = new Membership()
                {
                    Name = txtName.Text,
                    Description = textDescription.Text,
                    UserId = int.Parse(txtUserId.Text),
                    DurationMonths = duracion,
                    Price = precio,
                    Active = radioState.Checked,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(duracion)
                };

                // Llamar al método de registro
                int idGenerado = MembershipController.Instancia.Registrar(nuevaMembership, out mensaje);

                if (idGenerado > 0)
                {
                    Payment nuevoPago = new Payment()
                    {
                        MembershipId = idGenerado,
                        Amount = precio,
                        Status = "PENDIENTE",
                        PaymentDate = DateTime.Now,
                        Reference = $"NEW-{DateTime.Now:yyyyMMddHHmmss}"
                    };

                    string mensajePago;
                    PaymentController.Instancia.Registrar(nuevoPago, out mensajePago);

                    MessageBox.Show("Membresía registrada con éxito y pago pendiente generado",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    frmMemberShips_Load(sender, e);
                }
                else
                {
                    MessageBox.Show($"No se pudo registrar la membresía: {mensaje}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar la membresía: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtUserId.Text = "";
            txtName.Text = "";
            textDescription.Text = "";
            textDuration.Text = "";
            textPrice.Text = "";
            radioState.Checked = true;
            radioState.Text = "Active";
            oMembership = null;
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            int _idTemp = oMembership.Id;
            int nroperacion = MembershipController.Instancia.Update(new Membership()
            {
                Id = _idTemp,
                Name = txtName.Text,
                Description = textDescription.Text,
                UserId = int.Parse(txtUserId.Text),
                DurationMonths = int.Parse(textDuration.Text),
                Price = decimal.Parse(textPrice.Text),
                Active = radioState.Checked

            }, out message);

            if (nroperacion < 1)
            {
                MessageBox.Show(message, "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                oMembership = new Membership()
                {
                    Id = _idTemp,
                    Name = txtName.Text,
                    Description = textDescription.Text,
                    UserId = int.Parse(txtUserId.Text),
                    DurationMonths = int.Parse(textDuration.Text),
                    Price = decimal.Parse(textPrice.Text),
                    Active = radioState.Checked
                };
                MessageBox.Show("Usuario actualizado con éxito", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                frmMemberShips_Load(sender, e);
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (oMembership == null)
                {
                    MessageBox.Show("Por favor seleccione una membresía para eliminar",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verificar si hay pagos pendientes
                var payments = PaymentController.Instancia.GetByMembership(oMembership.Id);
                if (payments.Any(p => p.Status.ToUpper() == "PENDIENTE"))
                {
                    MessageBox.Show("No se puede eliminar la membresía porque tiene pagos pendientes",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "¿Está seguro de eliminar esta membresía? Esta acción también eliminará todos los pagos asociados.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        MembershipController.Instancia.Delete(oMembership.Id);
                        MessageBox.Show("Membresía eliminada con éxito",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LimpiarCampos();
                        dgvdata.Rows.Clear();
                        frmMemberShips_Load(sender, EventArgs.Empty);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("registros relacionados"))
                        {
                            MessageBox.Show("No se puede eliminar la membresía porque tiene registros relacionados. " +
                                "Por favor, elimine primero los registros dependientes.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show($"Error al eliminar la membresía: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnRenovar_Click(object sender, EventArgs e)
        {
            try
            {
                if (oMembership == null)
                {
                    MessageBox.Show("Por favor seleccione una membresía para renovar",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (oMembership.Active && !MembershipController.Instancia.EstaMembresiaVencida(oMembership.Id))
                {
                    MessageBox.Show("Esta membresía aún está activa y no ha vencido",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "¿Desea renovar esta membresía? Se actualizará con un nuevo período y se generará un pago pendiente.",
                    "Confirmar Renovación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string mensaje;
                    int idRenovado = MembershipController.Instancia.Renovar(oMembership.Id, out mensaje);

                    if (idRenovado > 0)
                    {
                        MessageBox.Show("Membresía renovada con éxito. Se ha generado un pago pendiente.",
                            "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        frmMemberShips_Load(sender, EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show($"Error al renovar la membresía: {mensaje}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvdata_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvdata.Columns["active"].Index && e.Value != null)
            {
                bool isActive = Convert.ToBoolean(e.Value);
                var row = dgvdata.Rows[e.RowIndex];
                DateTime endDate = Convert.ToDateTime(row.Cells["end_date"].Value);

                if (!isActive)
                {
                    e.Value = "INACTIVA";
                    e.CellStyle.ForeColor = Color.Red;
                }
                else if (endDate < DateTime.Now)
                {
                    e.Value = "EXPERIDA";
                    e.CellStyle.ForeColor = Color.Orange;
                }
                else
                {
                    e.Value = "ACTIVA";
                    e.CellStyle.ForeColor = Color.Green;
                }
                e.CellStyle.Font = new Font(dgvdata.DefaultCellStyle.Font, FontStyle.Bold);
                e.FormattingApplied = true;
            }
        }
    }
}