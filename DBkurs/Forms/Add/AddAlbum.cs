using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DBKurs.Forms.Add
{
    public partial class AddAlbum : Form
    {
        private readonly String connectString;
        private NpgsqlConnection conn;
        private String sql;
        private NpgsqlCommand cmd;
        private DataTable dtCountries, dtCities, dtRecordFirms,
            RecordTypes, Executors, Genres, Languages;
        private Image img;
        private DataGridViewRow row;

        private async Task InitializeCountries()
        {
            cmd = new NpgsqlCommand($"SELECT * FROM Countries", conn);

            dtCountries = new DataTable();
            dtCountries.Load(await cmd.ExecuteReaderAsync());

            listBox8.Items.Clear();
            for (int i = 0; i < dtCountries.Rows.Count; i++)
            {
                listBox8.Items.Add(dtCountries.Rows[i][1] + " - " + dtCountries.Rows[i][0]);
            }
        }

        private async Task InitializeCities(int country_id)
        {
            cmd = new NpgsqlCommand($"SELECT * FROM Cities WHERE country_id = {country_id}", conn);

            dtCities = new DataTable();
            dtCities.Load(await cmd.ExecuteReaderAsync());

            listBox7.Enabled = true;
            listBox7.Items.Clear();
            for (int i = 0; i < dtCities.Rows.Count; i++)
            {
                listBox7.Items.Add(dtCities.Rows[i][2] + " - " + dtCities.Rows[i][0]);
            }
        }

        private async Task InitializeRecordFirms(int city_id = -1)
        {
            if (city_id == -1)
                cmd = new NpgsqlCommand($"SELECT * FROM RecordFirms", conn);
            else
                cmd = new NpgsqlCommand($"SELECT * FROM RecordFirms WHERE city_id = {city_id}", conn);
            dtRecordFirms = new DataTable();
            dtRecordFirms.Load(await cmd.ExecuteReaderAsync());

            listBox1.Items.Clear();
            for (int i = 0; i < dtRecordFirms.Rows.Count; i++)
            {
                listBox1.Items.Add(dtRecordFirms.Rows[i][2] + " - " + dtRecordFirms.Rows[i][0]);
            }
        }
        
        private async Task InitializeRecordTypes()
        {
            cmd = new NpgsqlCommand($"SELECT * FROM RecordTypes", conn);

            RecordTypes = new DataTable();
            RecordTypes.Load(await cmd.ExecuteReaderAsync());

            listBox2.Items.Clear();
            for (int i = 0; i < RecordTypes.Rows.Count; i++)
            {
                listBox2.Items.Add(RecordTypes.Rows[i][1] + " - " + RecordTypes.Rows[i][0]);
            }
        }

        private async Task InitializeExecutors()
        {
            cmd = new NpgsqlCommand($"SELECT * FROM Executors", conn);

            Executors = new DataTable();
            Executors.Load(await cmd.ExecuteReaderAsync());

            listBox3.Items.Clear();
            for (int i = 0; i < Executors.Rows.Count; i++)
            {
                listBox3.Items.Add(Executors.Rows[i][1] + " - " + Executors.Rows[i][0]);
            }
        }

        private async Task InitializeGenres()
        {
            cmd = new NpgsqlCommand($"SELECT * FROM Genres", conn);

            Genres = new DataTable();
            Genres.Load(await cmd.ExecuteReaderAsync());

            listBox4.Items.Clear();
            for (int i = 0; i < Genres.Rows.Count; i++)
            {
                listBox4.Items.Add(Genres.Rows[i][1] + " - " + Genres.Rows[i][0]);
            }
        }

        private async Task InitializeLanguages()
        {
            cmd = new NpgsqlCommand($"SELECT * FROM Languages", conn);

            Languages = new DataTable();
            Languages.Load(await cmd.ExecuteReaderAsync());

            listBox5.Items.Clear();
            for (int i = 0; i < Languages.Rows.Count; i++)
            {
                listBox5.Items.Add(Languages.Rows[i][1] + " - " + Languages.Rows[i][0]);
            }
        }

        private async void AddAlbum_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectString);
            try
            {
                conn.Open();
                await this.InitializeCountries();
                await this.InitializeRecordFirms();
                await this.InitializeRecordTypes();
                await this.InitializeExecutors();
                await this.InitializeGenres();
                await this.InitializeLanguages();
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
                listBox1.SelectedIndex = listBox1.FindString(row.Cells["Фирма звукозаписи"].Value.ToString() + " - ");
                listBox2.SelectedIndex = listBox2.FindString(row.Cells["Тип записи"].Value.ToString() + " - ");
                listBox5.SelectedIndex = listBox5.FindString(row.Cells["Язык исполнения"].Value.ToString() + " - ");
                listBox4.SelectedIndex = listBox4.FindString(row.Cells["Жанр"].Value.ToString() + " - ");
                dateTimePicker1.Value = DateTime.Parse(row.Cells["Дата выпуска альбома"].Value.ToString());
                pictureBox1.Image = (Image)row.Cells["Титул альбома"].Value;
                textBox1.Text = row.Cells["Название альбома"].Value.ToString();
                textBox3.Text = row.Cells["Количество песен"].Value.ToString();
                textBox4.Text = row.Cells["Тираж албома"].Value.ToString();
                textBox5.Text = row.Cells["Время звучания"].Value.ToString();
                if ((bool)row.Cells["Альбом сборник"].Value)
                {
                    String[] executors = row.Cells["Информация"].Value.ToString().
                        Substring(12).Split('\n')[0].Trim().
                        Split(new String[] {", "}, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < executors.Length; i++)
                    {
                        int id = listBox3.FindString(executors[i] + " - ");
                        if (id == -1)
                            continue;
                        listBox3.SetItemChecked(id, true);
                        listBox3.SelectedIndex = id;
                    }
                }
                else
                {
                    int id = listBox3.FindString(row.Cells["Исполнитель"].Value.ToString() + " - ");
                    listBox3.SetItemChecked(id, true);
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (new AddRecordFirm(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    await InitializeRecordFirms();
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
            if (new AddRecordType(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    await InitializeRecordTypes();
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
            if (new AddExecutor(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    await InitializeExecutors();
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

        private async void button5_Click(object sender, EventArgs e)
        {
            if (new AddGenre(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    await InitializeGenres();
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

        private void button9_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            if (new AddLanguage(connectString).ShowDialog() == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    await InitializeLanguages();
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

        private void button8_Click(object sender, EventArgs e)
        {
            label16.Visible = false;
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|All files (*.*)|*.*";
            fileDialog.Multiselect = false;
            
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    img = Image.FromFile(fileDialog.FileName);
                    label16.Visible = true;
                    pictureBox1.Image = img;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Название альбома должно быть больше 0");
                return;
            }
            String name = textBox1.Text;

            int songsCount = -1;
            if (!Int32.TryParse(textBox3.Text, out songsCount))
            {
                MessageBox.Show("Невозможно считать количество песен в альбоме");
                return;
            }

            int amount = -1;
            if (!Int32.TryParse(textBox4.Text, out amount))
            {
                MessageBox.Show("Невозможно считать тираж альбома");
                return;
            }

            int time = -1;
            if (!Int32.TryParse(textBox5.Text, out time))
            {
                MessageBox.Show("Невозможно считать время звучания альбома");
                return;
            }

            if (dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Дата выпуска альбома не может быть больше текущей даты");
                return;
            }
            if (dateTimePicker1.Value < new DateTime(1900, 1, 1))
            {
                MessageBox.Show("Дата выпуска альбома не может быть меньше 1900 года");
                return;
            }

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите фирму звукозаписи\nЕсли необходимой фирмы нет - добавьте ее с помощью кнопки\nДобавить новую фирму");
                return;
            }
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип звукозаписи\nЕсли необходимого нет - добавьте его с помощью кнопки\nДобавить новый тип записи");
                return;
            }
            if (listBox3.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите необходимого исполнителя(-ей)\nЕсли необходимого исполнителя нет - добавьте его с помощью кнопки\nДобавить нового исполнителя");
                return;
            }
            if (listBox4.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр\nЕсли необходимого жанра нет - добавьте его с помощью кнопки\nДобавить новый жанр");
                return;
            }
            if (listBox5.SelectedItem == null)
            {
                MessageBox.Show("Выберите язык\nЕсли необходимого языка нет - добавьте его с помощью кнопки\nДобавить новый язык");
                return;
            }
            if (img == null)
            {
                if (pictureBox1.Image == null)
                {
                    if (MessageBox.Show("Вы уверены, что хотите продолжить без изображения?",
                    "Изображение не установлено",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        img = new Bitmap(1, 1);
                        img.Save($".{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.jpg");
                        img = Image.FromFile($".{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.jpg");
                        File.Delete($".{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.jpg");
                    }
                    else
                        return;
                }
                else
                {
                    img = new Bitmap(pictureBox1.Image);
                    img.Save($"temp.jpg");
                    using (var reader = new StreamReader("temp.jpg"))
                    {
                        img = Image.FromStream(reader.BaseStream);
                    }
                    File.Delete($"temp.jpg");
                }
                
            }

            String[] temp;

            temp = listBox1.SelectedItem.ToString().Split('-');
            int recordFirm_id = int.Parse(temp.Last());

            temp = listBox4.SelectedItem.ToString().Split('-');
            int genre_id = int.Parse(temp.Last());

            temp = listBox3.SelectedItem.ToString().Split('-');
            int executor_id = int.Parse(temp.Last());

            temp = listBox5.SelectedItem.ToString().Split('-');
            int language_id = int.Parse(temp.Last());

            temp = listBox2.SelectedItem.ToString().Split('-');
            int recordType_id = int.Parse(temp.Last());

            bool isCompilation = false;
            String info = string.Empty;
            String executors = string.Empty;
            foreach (var el in listBox3.CheckedItems)
            {
                executors += el.ToString().Split('-').First().Trim() + ", ";
            }
            executors = executors.Remove(executors.Length - 2);
            if (listBox3.CheckedItems.Count > 1)
            {
                isCompilation = true;
                info = $"Исполнители: {executors}\nЖанр: {listBox4.SelectedItem.ToString().Split('-').First().Trim()}\nЯзык: {listBox5.SelectedItem.ToString().Split('-').First().Trim()}";
            }

            String release = dateTimePicker1.Value.ToString("dd-MM-yyyy");
            String isCol = isCompilation ? "true" : "false";

            byte[] arr;

            using (var ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                arr = ms.ToArray();
            }

            String image = Convert.ToBase64String(arr);


            try
            {
                conn.Open();
                if (row == null)
                {
                    if (isCompilation)
                    {
                        cmd = new NpgsqlCommand("INSERT INTO Albums (recordFirm_id, genre_id, executor_id, language_id, recordType_id, album_name, releaseDate, albumCount, songsCount, isCollection, albumInfo, Photo, albumTime) " +
                        $"VALUES ({recordFirm_id}, {genre_id}, NULL, {language_id}, {recordType_id}, \'{name}\', \'{release}\', {amount}, {songsCount}, \'{isCol}\', \'{info}\', \'{image}\', {time})", conn);
                    }
                    else
                    {
                        cmd = new NpgsqlCommand("INSERT INTO Albums (recordFirm_id, genre_id, executor_id, language_id, recordType_id, album_name, releaseDate, albumCount, songsCount, isCollection, albumInfo, Photo, albumTime) " +
                        $"VALUES ({recordFirm_id}, {genre_id}, {executor_id}, {language_id}, {recordType_id}, \'{name}\', \'{release}\', {amount}, {songsCount}, \'{isCol}\', NULL, \'{image}\', {time})", conn);
                    }
                }
                else
                {
                    int mId = (int)row.Cells["id"].Value;
                    if (isCompilation)
                    {
                        cmd = new NpgsqlCommand($"UPDATE Albums SET recordFirm_id = {recordFirm_id}, genre_id = {genre_id}, executor_id = NULL, language_id = {language_id}, " +
                            $"recordType_id = {recordType_id}, album_name = \'{name}\', releaseDate = \'{release}\', albumCount = {amount}, songsCount = {songsCount}, " +
                            $"isCollection = \'{isCol}\', albumInfo = \'{info}\', Photo = \'{image}\', albumTime = {time} WHERE album_id = {mId}", conn);
                    }
                    else
                    {
                        cmd = new NpgsqlCommand($"UPDATE Albums SET recordFirm_id = {recordFirm_id}, genre_id = {genre_id}, executor_id = {executor_id}, language_id = {language_id}, " +
                            $"recordType_id = {recordType_id}, album_name = \'{name}\', releaseDate = \'{release}\', albumCount = {amount}, songsCount = {songsCount}, " +
                            $"isCollection = \'{isCol}\', albumInfo = NULL, Photo = \'{image}\', albumTime = {time} WHERE album_id = {mId}", conn);
                    }
                }
                
                await cmd.ExecuteNonQueryAsync();
                this.DialogResult = DialogResult.OK;
                if (row == null)
                {
                    MessageBox.Show("Альбом успешно добавлен");
                }
                else
                {
                    MessageBox.Show("Альбом успешно изменен");
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

        private async void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                String[] temp = listBox8.SelectedItem.ToString().Split('-');
                int id = int.Parse(temp.Last());
                await this.InitializeCities(id);
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

        private async void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                String[] temp = listBox7.SelectedItem.ToString().Split('-');
                int id = int.Parse(temp.Last());
                await this.InitializeRecordFirms(id);
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

        public AddAlbum(String conn, DataGridViewRow row = null)
        {
            InitializeComponent();
            connectString = conn;
            this.row = row;
            if (row == null)
            {
                this.Text = "Добавление альбома";
            }
            else
            {
                this.Text = "Изменение альбома";
            }
        }
    }
}
