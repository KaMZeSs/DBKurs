using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Npgsql;

namespace DBKurs.Forms.Add
{
    public partial class AddShop : Form
    {
        private readonly String connectString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataTable dtDistricts, dtPropertyTypes, dtOwners;

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
        }

        public AddShop(String conn)
        {
            InitializeComponent();
            connectString = conn;
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
            this.DialogResult = DialogResult.Cancel;
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
            int yearOpened = -1;
            if (!Int32.TryParse(textBox4.Text, out yearOpened))
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
            String[] temp = listBox1.SelectedItem.ToString().Split('-');
            int district_id = Int32.Parse(temp.Last());
            temp = listBox2.SelectedItem.ToString().Split('-');
            int PropertyType_id = Int32.Parse(temp.Last());
            temp = listBox3.SelectedItem.ToString().Split('-');
            int Owner_id = Int32.Parse(temp.Last());


            try
            {
                conn.Open();
                cmd = new NpgsqlCommand("INSERT INTO Shops (district_id, propertyType_id, owner_id, shop_name, addres, license, expiryDate, yearOpened) " +
                $"VALUES ({district_id}, {PropertyType_id}, {Owner_id}, \'{textBox1.Text}\', \'{textBox2.Text}\', \'{textBox3.Text}\', \'{dateTimePicker1.Value}\', {yearOpened})", conn);
                await cmd.ExecuteNonQueryAsync();
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Магазин успешно добавлен");
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
                catch(Exception exc)
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
