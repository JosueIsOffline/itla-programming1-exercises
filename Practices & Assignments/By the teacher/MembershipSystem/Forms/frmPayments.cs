using MembershipSystem.Controllers;
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

namespace MembershipSystem.Forms
{
    public partial class frmPayments : Form
    {

        public Payment oPayment { get; set; }
        public Membership currentMembership { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }

        public frmPayments()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void frmPayments_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigurarColumnas();
                CargarPayments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColumnas()
        {
            dgvdata.Columns.Clear();

            // Configuración básica del DataGridView
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
            dgvdata.Columns.Add("Id", "ID");
            dgvdata.Columns.Add("membership_id", "Membership ID");
            dgvdata.Columns.Add("user_fullname", "Usuario");
            dgvdata.Columns.Add("membership_name", "Membresía");
            dgvdata.Columns.Add("amount", "Monto");
            dgvdata.Columns.Add("payment_date", "Fecha de Pago");
            dgvdata.Columns.Add("status", "Estado");
            dgvdata.Columns.Add("reference", "Referencia");

            // Ocultar columnas de IDs
            dgvdata.Columns["Id"].Visible = false;
            //dgvdata.Columns["membership_id"].Visible = false;
        }

        private void CargarPayments()
        {
            try
            {
                dgvdata.Rows.Clear();
                var payments = PaymentController.Instancia.GetAll();
                foreach (Payment payment in payments)
                {

                    try
                    {
                        dgvdata.Rows.Add(new object[]
                        {
                    "",
                    payment.Id,
                    payment.MembershipId,
                    payment.UserFullName ?? "N/A",
                    payment.MembershipName ?? "N/A",
                    payment.Amount.ToString("C2"),
                    payment.PaymentDate.ToString("dd/MM/yyyy HH:mm"),
                    payment.Status ?? "N/A",
                    payment.Reference ?? "N/A"
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al agregar fila: {ex.Message}");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los pagos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


 

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtMembershipId.Text) ||
                string.IsNullOrWhiteSpace(txtAmount.Text) ||
                string.IsNullOrWhiteSpace(comboStatus.Text))
            {
                MessageBox.Show("Por favor complete los campos requeridos", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal monto))
            {
                MessageBox.Show("El monto debe ser un número válido", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtMembershipId.Text = "";
            txtAmount.Text = "";
            comboStatus.Text = "";
            txtReference.Text = "";
            oPayment = null;
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            try
            {
                
                    string mensaje = string.Empty;

                    if (oPayment == null) // Nuevo Pago
                    {
                        Payment nuevoPago = new Payment()
                        {
                            MembershipId = int.Parse(txtMembershipId.Text),
                            Amount = decimal.Parse(txtAmount.Text),
                            Status = comboStatus.Text,
                            Reference = txtReference.Text,
                            PaymentDate = DateTime.Now
                        };

                        int idGenerado = PaymentController.Instancia.Registrar(nuevoPago, out mensaje);

                        if (idGenerado > 0)
                        {
                            MessageBox.Show("Pago registrado con éxito", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            frmPayments_Load(sender, e);
                        }
                        else
                        {
                            MessageBox.Show($"No se pudo registrar el pago: {mensaje}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else 
                    {
                        DialogResult result = MessageBox.Show("¿Está seguro de actualizar el estado del pago?",
                            "Confirmar Actualización",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            oPayment.Status = comboStatus.Text;
                            // Aquí llamarías al método de actualización en el controlador
                            // Por ahora agregaremos el método al controlador
                            if (PaymentController.Instancia.ActualizarEstado(oPayment, out mensaje))
                            {
                                MessageBox.Show("Estado del pago actualizado con éxito", "Éxito",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LimpiarCampos();
                                frmPayments_Load(sender, e);
                            }
                            else
                            {
                                MessageBox.Show($"No se pudo actualizar el estado: {mensaje}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuscarMembresia()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtMembershipId.Text) &&
                    int.TryParse(txtMembershipId.Text, out int membershipId))
                {
                    LimpiarCamposBusqueda();

                    var memberships = MembershipController.Instancia.GetAll();
                    currentMembership = memberships.FirstOrDefault(m => m.Id == membershipId);

                    if (currentMembership != null)
                    {
                        lblUsername.Text = currentMembership.UserFullName;
                        lblMembershipType.Text = currentMembership.Name;

                        var payments = PaymentController.Instancia.GetByMembership(membershipId);
                        var ultimoPago = payments.OrderByDescending(p => p.PaymentDate).FirstOrDefault();

                        if (ultimoPago != null)
                        {
                            txtMembershipIdRo.Text = currentMembership.Id.ToString();
                            txtAmount.Text = ultimoPago.Amount.ToString("F2");
                            txtReference.Text = ultimoPago.Reference;
                            comboStatus.Text = ultimoPago.Status;
                        }
                        else
                        {
                            txtMembershipIdRo.Text = currentMembership.Id.ToString();
                            txtAmount.Text = currentMembership.Price.ToString("F2");
                            txtReference.Text = "";
                            comboStatus.Text = "PENDIENTE";
                        }

                        CargarHistorialPagos(membershipId);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la membresía", "Advertencia",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor ingrese un ID de membresía válido", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar la membresía: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtMembershipId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscarMembresia();
            }
        }

        private void CargarHistorialPagos(int membershipId)
        {
            try
            {
                dgvdata.Rows.Clear();
                var payments = PaymentController.Instancia.GetByMembership(membershipId);

                foreach (Payment payment in payments)
                {
                    dgvdata.Rows.Add(new object[]
                    {
                    "",
                    payment.Id,
                    payment.MembershipId,
                    payment.UserFullName,
                    payment.MembershipName,
                    payment.Amount.ToString("C2"),
                    payment.PaymentDate.ToString("dd/MM/yyyy HH:mm"),
                    payment.Status,
                    payment.Reference
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el historial de pagos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCamposBusqueda()
        {
            lblUsername.Text = "";
            lblMembershipType.Text = "";
            txtMembershipId.Text = "";
            txtAmount.Text = "";
            currentMembership = null;
            dgvdata.Rows.Clear();
        }

        private void dgvdata_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
            {
                try
                {
                    string montoStr = dgvdata.Rows[e.RowIndex].Cells["amount"].Value.ToString()
                        .Replace("$", "")
                        .Replace(",", "")
                        .Trim();

                    if (decimal.TryParse(montoStr, out decimal monto))
                    {
                        // Crear el objeto Payment con todos los datos de la fila
                        oPayment = new Payment()
                        {
                            Id = Convert.ToInt32(dgvdata.Rows[e.RowIndex].Cells["id"].Value),
                            MembershipId = Convert.ToInt32(dgvdata.Rows[e.RowIndex].Cells["membership_id"].Value),
                            Amount = monto,
                            Status = dgvdata.Rows[e.RowIndex].Cells["status"].Value.ToString(),
                            Reference = dgvdata.Rows[e.RowIndex].Cells["reference"].Value.ToString(),
                            UserFullName = dgvdata.Rows[e.RowIndex].Cells["user_fullname"].Value.ToString(),
                            MembershipName = dgvdata.Rows[e.RowIndex].Cells["membership_name"].Value.ToString()
                        };

                        // Llenar los campos del formulario
                        txtMembershipId.Text = oPayment.MembershipId.ToString();
                        txtAmount.Text = oPayment.Amount.ToString();
                        comboStatus.Text = oPayment.Status;
                        txtReference.Text = oPayment.Reference;

                        // Actualizar los labels con la información de usuario y membresía
                        lblUsername.Text = oPayment.UserFullName;
                        lblMembershipType.Text = oPayment.MembershipName;

                        // También actualizamos el ID de membresía en el campo de solo lectura si existe
                        if (txtMembershipIdRo != null)
                        {
                            txtMembershipIdRo.Text = oPayment.MembershipId.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al seleccionar el pago: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvdata_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
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

        private void dgvdata_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvdata.Columns["status"].Index && e.Value != null)
                {
                    string status = e.Value.ToString().ToLower();

                    switch (status)
                    {
                        case "completed":
                        case "completado":
                            e.CellStyle.ForeColor = Color.Green;
                            e.CellStyle.Font = new Font(dgvdata.DefaultCellStyle.Font, FontStyle.Bold);
                            break;

                        case "pending":
                        case "pendiente":
                            e.CellStyle.ForeColor = Color.Orange;
                            e.CellStyle.Font = new Font(dgvdata.DefaultCellStyle.Font, FontStyle.Bold);
                            break;

                        case "cancelled":
                        case "cancelado":
                            e.CellStyle.ForeColor = Color.Red;
                            e.CellStyle.Font = new Font(dgvdata.DefaultCellStyle.Font, FontStyle.Bold);
                            break;

                        case "processing":
                        case "procesando":
                            e.CellStyle.ForeColor = Color.Blue;
                            e.CellStyle.Font = new Font(dgvdata.DefaultCellStyle.Font, FontStyle.Bold);
                            break;

                        default:
                            e.CellStyle.ForeColor = dgvdata.DefaultCellStyle.ForeColor;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en formato de celda: {ex.Message}");
            }
        }
    }
}
