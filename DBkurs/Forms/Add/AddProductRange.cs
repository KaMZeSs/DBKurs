using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms.Add
{
    delegate void Shown();
    public partial class AddProductRange : Form
    {
        private readonly string connectString;
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dtShops, dtAlbums;
        private readonly DataGridViewRow row;
        private async Task InitializeShops()
        {
            cmd = new NpgsqlCommand($"SELECT shop_id, shop_name FROM Shops", conn);

            dtShops = new DataTable();
            dtShops.Load(await cmd.ExecuteReaderAsync());

            listBox1.Items.Clear();
            for (int i = 0; i < dtShops.Rows.Count; i++)
            {
                listBox1.Items.Add(dtShops.Rows[i][1] + " - " + dtShops.Rows[i][0]);
            }
        }
        private async Task InitializeAlbums()
        {
            cmd = new NpgsqlCommand($"SELECT album_id, album_name FROM albums", conn);

            dtAlbums = new DataTable();
            dtAlbums.Load(await cmd.ExecuteReaderAsync());

            listBox2.Items.Clear();
            for (int i = 0; i < dtAlbums.Rows.Count; i++)
            {
                listBox2.Items.Add(dtAlbums.Rows[i][1] + " - " + dtAlbums.Rows[i][0]);
            }
        }

        private async void AddProductRange_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            try
            {
                conn.Open();
                await this.InitializeShops();
                await this.InitializeAlbums();
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
                listBox1.SelectedIndex = listBox1.FindString(row.Cells["Магазин"].Value.ToString() + " - ");
                listBox2.SelectedIndex = listBox2.FindString(row.Cells["Альбом"].Value.ToString() + " - ");
                dateTimePicker1.Value = DateTime.Parse(row.Cells["Дата поступления"].Value.ToString());
                textBox1.Text = row.Cells["Количество единиц"].Value.ToString();
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string[] temp;
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите необходимый магазин\nЕсли вашего магазина нет, его можно добавить, нажав на кнопку\nДобавить новый магазин");
                return;
            }
            temp = listBox1.SelectedItem.ToString().Split('-');
            int shop_id = Int32.Parse(temp.Last());
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Выберите необходимый альбом\nЕсли вашего альбома нет, его можно добавить, нажав на кнопку\nДобавить новый альбом");
                return;
            }
            temp = listBox2.SelectedItem.ToString().Split('-');
            int album_id = Int32.Parse(temp.Last());

            if (dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Дата поступления не может быть позже текущей даты");
                return;
            }
            if (dateTimePicker1.Value < new DateTime(1900, 1, 1))
            {
                MessageBox.Show("Дата поступления не может быть меньше 1900 года");
                return;
            }

            if (!Int32.TryParse(textBox1.Text, out int amount))
            {
                MessageBox.Show("Невозможно считать количество единиц в магазине");
                return;
            }

            DateTime time = dateTimePicker1.Value;

            try
            {
                conn.Open();

                if (row == null)
                {
                    cmd = new NpgsqlCommand("INSERT INTO ProductRanges (shop_id, album_id, dateOfReceipt, amount) " +
                    $"VALUES ({shop_id}, {album_id}, \'{time}\', {amount})", conn);
                }
                else
                {
                    int mId = (int)row.Cells["id"].Value;
                    cmd = new NpgsqlCommand($"UPDATE ProductRanges SET shop_id = {shop_id}, album_id = {album_id}, dateOfReceipt = '{time}', amount = {amount}  WHERE productrange_id = {mId}", conn);
                }


                await cmd.ExecuteNonQueryAsync();
                DialogResult = DialogResult.OK;

                if (row == null)
                    MessageBox.Show("Ассортимент успешно добавлен");
                else
                    MessageBox.Show("Ассортимент успешно изменен");

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
            if (new AddShop(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    await this.InitializeShops();
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
            if (new AddAlbum(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    await this.InitializeAlbums();
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

        public AddProductRange(string conn, DataGridViewRow row = null)
        {
            this.InitializeComponent();
            connectString = conn;
            this.row = row;

        }
    }
}
