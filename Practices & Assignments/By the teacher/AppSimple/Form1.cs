namespace AppSimple
{
    public partial class Form1 : Form
    {
        private int? num1 { get; set; }
        private int? num2 { get; set; }

        public Form1()
        {
            InitializeComponent();
            ConfigurarListView();
        }
        private void ConfigurarListView()
        {
            // Configurar las columnas del ListView
            listView1.View = View.Details;
            listView1.Columns.Add("Número 1", 100);
            listView1.Columns.Add("Operación", 70);
            listView1.Columns.Add("Número 2", 100);
            listView1.Columns.Add("Resultado", 100);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text))
            {
                num1 = null;
                return;
            }

            if(!int.TryParse(textBox1.Text, out int result))
            {
                MessageBox.Show("Ingrese un número válido");
                textBox1.Text = "";
                return;
            }

            num1 = result;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                num2 = null;
                return;
            }

            if (!int.TryParse(textBox2.Text, out int result))
            {
                MessageBox.Show("Ingrese un número válido");
                textBox2.Text = "";
                num2 = null;
                return;
            }

            num2 = result;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btn_suma_Click(object sender, EventArgs e)
        {
            if(!num1.HasValue || !num2.HasValue)
            {
                MessageBox.Show("Ingrese ambos números");
                return;
            }

            int result = num1.Value + num2.Value;
            label_result.Text = result.ToString();

            ListViewItem item = new ListViewItem(num1.ToString());
            item.SubItems.Add("+");
            item.SubItems.Add(num2.ToString());
            item.SubItems.Add(result.ToString());
            listView1.Items.Add(item);
        }

        private void btn_restar_Click(object sender, EventArgs e)
        {
            if (!num1.HasValue || !num2.HasValue)
            {
                MessageBox.Show("Por favor ingrese ambos números");
                return;
            }

            int result = num1.Value - num2.Value;
            label_result.Text = result.ToString();

         
            ListViewItem item = new ListViewItem(num1.ToString());
            item.SubItems.Add("-");
            item.SubItems.Add(num2.ToString());
            item.SubItems.Add(result.ToString());
            listView1.Items.Add(item);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            label_result.Text = "0";
            num1 = null;
            num2 = null;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
