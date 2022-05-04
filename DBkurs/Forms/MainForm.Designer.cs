namespace DBKurs.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.данныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.запросыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.таблицыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.основныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ассортиментToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.альбомToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.магазинToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справочникиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.странаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.городToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.фирмаЗвукозаписиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.жанрИсполненияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.типЗаписиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.языкИсполненияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.исполнительToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.владелецToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.типСобственностиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.районГородаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обновитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new DBKurs.Forms.DoubleBufferedDataGridView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.menuStrip1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.данныеToolStripMenuItem,
            this.запросыToolStripMenuItem,
            this.таблицыToolStripMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.MinimumSize = new System.Drawing.Size(0, 40);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Size = new System.Drawing.Size(915, 40);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(53, 40);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // данныеToolStripMenuItem
            // 
            this.данныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьToolStripMenuItem,
            this.удалитьToolStripMenuItem,
            this.изменитьToolStripMenuItem});
            this.данныеToolStripMenuItem.Name = "данныеToolStripMenuItem";
            this.данныеToolStripMenuItem.Size = new System.Drawing.Size(71, 40);
            this.данныеToolStripMenuItem.Text = "Данные";
            // 
            // добавитьToolStripMenuItem
            // 
            this.добавитьToolStripMenuItem.Name = "добавитьToolStripMenuItem";
            this.добавитьToolStripMenuItem.Size = new System.Drawing.Size(140, 24);
            this.добавитьToolStripMenuItem.Text = "Добавить";
            this.добавитьToolStripMenuItem.Click += new System.EventHandler(this.добавитьToolStripMenuItem_Click);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(140, 24);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            // 
            // изменитьToolStripMenuItem
            // 
            this.изменитьToolStripMenuItem.Name = "изменитьToolStripMenuItem";
            this.изменитьToolStripMenuItem.Size = new System.Drawing.Size(140, 24);
            this.изменитьToolStripMenuItem.Text = "Изменить";
            // 
            // запросыToolStripMenuItem
            // 
            this.запросыToolStripMenuItem.Name = "запросыToolStripMenuItem";
            this.запросыToolStripMenuItem.Size = new System.Drawing.Size(76, 40);
            this.запросыToolStripMenuItem.Text = "Запросы";
            // 
            // таблицыToolStripMenuItem
            // 
            this.таблицыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.основныеToolStripMenuItem,
            this.справочникиToolStripMenuItem,
            this.обновитьToolStripMenuItem});
            this.таблицыToolStripMenuItem.Name = "таблицыToolStripMenuItem";
            this.таблицыToolStripMenuItem.Size = new System.Drawing.Size(76, 40);
            this.таблицыToolStripMenuItem.Text = "Таблицы";
            // 
            // основныеToolStripMenuItem
            // 
            this.основныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ассортиментToolStripMenuItem,
            this.альбомToolStripMenuItem,
            this.магазинToolStripMenuItem});
            this.основныеToolStripMenuItem.Name = "основныеToolStripMenuItem";
            this.основныеToolStripMenuItem.Size = new System.Drawing.Size(164, 24);
            this.основныеToolStripMenuItem.Text = "Основные";
            // 
            // ассортиментToolStripMenuItem
            // 
            this.ассортиментToolStripMenuItem.Name = "ассортиментToolStripMenuItem";
            this.ассортиментToolStripMenuItem.Size = new System.Drawing.Size(160, 24);
            this.ассортиментToolStripMenuItem.Text = "Ассортимент";
            this.ассортиментToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // альбомToolStripMenuItem
            // 
            this.альбомToolStripMenuItem.Name = "альбомToolStripMenuItem";
            this.альбомToolStripMenuItem.Size = new System.Drawing.Size(160, 24);
            this.альбомToolStripMenuItem.Text = "Альбом";
            this.альбомToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // магазинToolStripMenuItem
            // 
            this.магазинToolStripMenuItem.Name = "магазинToolStripMenuItem";
            this.магазинToolStripMenuItem.Size = new System.Drawing.Size(160, 24);
            this.магазинToolStripMenuItem.Text = "Магазин";
            this.магазинToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // справочникиToolStripMenuItem
            // 
            this.справочникиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.странаToolStripMenuItem,
            this.городToolStripMenuItem,
            this.фирмаЗвукозаписиToolStripMenuItem,
            this.жанрИсполненияToolStripMenuItem,
            this.типЗаписиToolStripMenuItem,
            this.языкИсполненияToolStripMenuItem,
            this.исполнительToolStripMenuItem,
            this.toolStripSeparator1,
            this.владелецToolStripMenuItem,
            this.типСобственностиToolStripMenuItem,
            this.районГородаToolStripMenuItem});
            this.справочникиToolStripMenuItem.Name = "справочникиToolStripMenuItem";
            this.справочникиToolStripMenuItem.Size = new System.Drawing.Size(164, 24);
            this.справочникиToolStripMenuItem.Text = "Справочники";
            // 
            // странаToolStripMenuItem
            // 
            this.странаToolStripMenuItem.Name = "странаToolStripMenuItem";
            this.странаToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.странаToolStripMenuItem.Text = "Страна";
            this.странаToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // городToolStripMenuItem
            // 
            this.городToolStripMenuItem.Name = "городToolStripMenuItem";
            this.городToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.городToolStripMenuItem.Text = "Город";
            this.городToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // фирмаЗвукозаписиToolStripMenuItem
            // 
            this.фирмаЗвукозаписиToolStripMenuItem.Name = "фирмаЗвукозаписиToolStripMenuItem";
            this.фирмаЗвукозаписиToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.фирмаЗвукозаписиToolStripMenuItem.Text = "Фирма звукозаписи";
            this.фирмаЗвукозаписиToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // жанрИсполненияToolStripMenuItem
            // 
            this.жанрИсполненияToolStripMenuItem.Name = "жанрИсполненияToolStripMenuItem";
            this.жанрИсполненияToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.жанрИсполненияToolStripMenuItem.Text = "Жанр исполнения";
            this.жанрИсполненияToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // типЗаписиToolStripMenuItem
            // 
            this.типЗаписиToolStripMenuItem.Name = "типЗаписиToolStripMenuItem";
            this.типЗаписиToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.типЗаписиToolStripMenuItem.Text = "Тип записи";
            this.типЗаписиToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // языкИсполненияToolStripMenuItem
            // 
            this.языкИсполненияToolStripMenuItem.Name = "языкИсполненияToolStripMenuItem";
            this.языкИсполненияToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.языкИсполненияToolStripMenuItem.Text = "Язык исполнения";
            this.языкИсполненияToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // исполнительToolStripMenuItem
            // 
            this.исполнительToolStripMenuItem.Name = "исполнительToolStripMenuItem";
            this.исполнительToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.исполнительToolStripMenuItem.Text = "Исполнитель";
            this.исполнительToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(200, 6);
            // 
            // владелецToolStripMenuItem
            // 
            this.владелецToolStripMenuItem.Name = "владелецToolStripMenuItem";
            this.владелецToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.владелецToolStripMenuItem.Text = "Владелец";
            this.владелецToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // типСобственностиToolStripMenuItem
            // 
            this.типСобственностиToolStripMenuItem.Name = "типСобственностиToolStripMenuItem";
            this.типСобственностиToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.типСобственностиToolStripMenuItem.Text = "Тип собственности";
            this.типСобственностиToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // районГородаToolStripMenuItem
            // 
            this.районГородаToolStripMenuItem.Name = "районГородаToolStripMenuItem";
            this.районГородаToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.районГородаToolStripMenuItem.Text = "Район города";
            this.районГородаToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // обновитьToolStripMenuItem
            // 
            this.обновитьToolStripMenuItem.Name = "обновитьToolStripMenuItem";
            this.обновитьToolStripMenuItem.Size = new System.Drawing.Size(164, 24);
            this.обновитьToolStripMenuItem.Text = "Обновить";
            this.обновитьToolStripMenuItem.Click += new System.EventHandler(this.обновитьToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(24, 6, 5, 6);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(181)))), ((int)(((byte)(204)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 40);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(181)))), ((int)(((byte)(204)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(181)))), ((int)(((byte)(204)))));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(915, 434);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.DataSourceChanged += new System.EventHandler(this.dataGridView1_DataSourceChanged);
            this.dataGridView1.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellMouseEnter);
            this.dataGridView1.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellMouseLeave);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 474);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Opacity = 0.96D;
            this.ShowIcon = false;
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem данныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem запросыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem таблицыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem основныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справочникиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ассортиментToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem альбомToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem магазинToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem странаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem городToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem фирмаЗвукозаписиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem жанрИсполненияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem типЗаписиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem языкИсполненияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem исполнительToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem владелецToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem типСобственностиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem районГородаToolStripMenuItem;
        private DoubleBufferedDataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem обновитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}