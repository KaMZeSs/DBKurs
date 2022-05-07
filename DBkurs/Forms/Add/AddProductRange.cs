using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms.Add
{
    delegate void Shown();
    public partial class AddProductRange : Form
    {
        private readonly String connectString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataTable dtShops, dtAlbums;
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
                await InitializeShops();
                await InitializeAlbums();
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

        private async void button3_Click(object sender, EventArgs e)
        {
            String[] temp;
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

            int amount = -1;
            if (!Int32.TryParse(textBox1.Text, out amount))
            {
                MessageBox.Show("Невозможно считать количество единиц в магазине");
                return;
            }

            DateTime time = dateTimePicker1.Value;

            try
            {
                conn.Open();
                cmd = new NpgsqlCommand("INSERT INTO ProductRanges (shop_id, album_id, dateOfReceipt, amount) " +
                    $"VALUES ({shop_id}, {album_id}, \'{time}\', {amount})", conn);
                await cmd.ExecuteNonQueryAsync();
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Ассортимент успешно добавлен");
                this.Close();
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

        private async void button1_Click(object sender, EventArgs e)
        {
            if (new AddShop(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    await InitializeShops();
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
                    await InitializeAlbums();
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

        public AddProductRange(String conn)
        {
            InitializeComponent();
            connectString = conn;
        }
    }
}
