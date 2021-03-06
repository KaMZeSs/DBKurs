using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms.Add
{
    public partial class AddShop : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dtDistricts, dtPropertyTypes, dtOwners;
        private readonly DataGridViewRow row;

        private async void AddShop_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);

            try
            {
                conn.Open();
                await this.InitializeDistricts();
                await this.InitializeOwners();
                await this.InitializePropertyTypes();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }

            if (row != null)
            {
                listBox1.SelectedIndex = listBox1.FindString(row.Cells["Район города"].Value.ToString() + " - ");
                listBox2.SelectedIndex = listBox2.FindString(row.Cells["Тип собственности"].Value.ToString() + " - ");
                listBox3.SelectedIndex = listBox3.FindString(row.Cells["Владелец"].Value.ToString() + " - ");
                textBox1.Text = row.Cells["Название магазина"].Value.ToString();
                textBox2.Text = row.Cells["Адрес"].Value.ToString();
                textBox3.Text = row.Cells["Лицензия"].Value.ToString();
                textBox4.Text = row.Cells["Год открытия"].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(row.Cells["Дата окончания лицензии"].Value.ToString());
            }
        }

        public AddShop(string conn, DataGridViewRow row = null)
        {
            this.InitializeComponent();
            connectString = conn;
            this.row = row;

        }
        private async Task InitializeDistricts()
        {
            cmd = new NpgsqlCommand($"SELECT * FROM Districts", conn);

            dtDistricts = new DataTable();
            dtDistricts.Load(await cmd.ExecuteReaderAsync());

            listBox1.Items.Clear();
            for (int i = 0; i < dtDistricts.Rows.Count; i++)
            {
                listBox1.Items.Add(dtDistricts.Rows[i][1] + " - " + dtDistricts.Rows[i][0]);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Длина названия магазина должна быть больше 0");
                return;
            }
            if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("Длина адреса должна быть больше 0");
                return;
            }
            if (textBox3.Text.Length == 0)
            {
                MessageBox.Show("Длина номера лицензии должна быть больше 0");
                return;
            }
            if (textBox4.Text.Length == 0)
            {
                MessageBox.Show("Длина года открытия фирмы должна быть больше 0");
                return;
            }
            if (!Int32.TryParse(textBox4.Text, out int yearOpened))
            {
                MessageBox.Show("Невозможно считать год открытия");
                return;
            }
            if (yearOpened < 1900)
            {
                MessageBox.Show("Год открытия магазина должна быть больше 1900 года");
            }
            if (yearOpened > DateTime.Now.Year)
            {
                MessageBox.Show("Год открытия магазина не может быть больше текущего года");
            }
            if (dateTimePicker1.Value <= DateTime.Now)
            {
                MessageBox.Show("Дата окончании лицензии должна быть больше текущей даты");
            }

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите район, в котором находится данный магазин\nЕсли данного района нет - создайте его, нажав на кнопку\n\"Добавить новый район\"");
                return;
            }
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип собственности данного магазина\nЕсли данного типа собственности нет - создайте его, нажав на кнопку\n\"Добавить новый тип собственности\"");
                return;
            }
            if (listBox3.SelectedItem == null)
            {
                MessageBox.Show("Выберите владельца магазина\nЕсли данного владельца нет - добавьте его, нажав на кнопку\n\"Добавить нового владельца\"");
                return;
            }
            string[] temp = listBox1.SelectedItem.ToString().Split('-');
            int district_id = Int32.Parse(temp.Last());
            temp = listBox2.SelectedItem.ToString().Split('-');
            int PropertyType_id = Int32.Parse(temp.Last());
            temp = listBox3.SelectedItem.ToString().Split('-');
            int Owner_id = Int32.Parse(temp.Last());


            try
            {
                conn.Open();

                if (row == null)
                {
                    cmd = new NpgsqlCommand("INSERT INTO Shops (district_id, propertyType_id, owner_id, shop_name, addres, license, expiryDate, yearOpened) " +
                    $"VALUES ({district_id}, {PropertyType_id}, {Owner_id}, \'{textBox1.Text}\', \'{textBox2.Text}\', \'{textBox3.Text}\', \'{dateTimePicker1.Value}\', {yearOpened})", conn);
                }
                else
                {
                    int mId = (int)row.Cells["id"].Value;
                    cmd = new NpgsqlCommand($"UPDATE Shops SET district_id = {district_id}, propertyType_id = {PropertyType_id}, " +
                        $"owner_id = {Owner_id}, shop_name = '{textBox1.Text}', addres = '{textBox2.Text}', " +
                        $"license = '{textBox3.Text}', expiryDate = '{dateTimePicker1.Value}', yearOpened = {yearOpened}" +
                        $" WHERE shop_id = {mId}", conn);
                }

                await cmd.ExecuteNonQueryAsync();
                DialogResult = DialogResult.OK;

                if (row == null)
                {
                    MessageBox.Show("Магазин успешно добавлен");
                }
                else
                {
                    MessageBox.Show("Магазин успешно изменен");
                }

                this.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (new AddDistrict(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    await this.InitializeDistricts();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (new AddPropertyType(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    await this.InitializePropertyTypes();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (new AddOwner(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    await this.InitializeOwners();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private async Task InitializePropertyTypes()
        {
            cmd = new NpgsqlCommand($"SELECT * FROM PropertyTypes", conn);

            dtPropertyTypes = new DataTable();
            dtPropertyTypes.Load(await cmd.ExecuteReaderAsync());

            listBox2.Items.Clear();
            for (int i = 0; i < dtPropertyTypes.Rows.Count; i++)
            {
                listBox2.Items.Add(dtPropertyTypes.Rows[i][1] + " - " + dtPropertyTypes.Rows[i][0]);
            }
        }

        private async Task InitializeOwners()
        {
            cmd = new NpgsqlCommand($"SELECT * FROM Owners", conn);

            dtOwners = new DataTable();
            dtOwners.Load(await cmd.ExecuteReaderAsync());

            listBox3.Items.Clear();
            for (int i = 0; i < dtOwners.Rows.Count; i++)
            {
                listBox3.Items.Add(dtOwners.Rows[i][1] + " - " + dtOwners.Rows[i][0]);
            }
        }
    }
}
