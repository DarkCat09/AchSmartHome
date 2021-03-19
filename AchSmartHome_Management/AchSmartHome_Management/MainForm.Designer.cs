namespace AchSmartHome_Management
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.панельToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.соединитьсяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.регистрацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.управлениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.главнаяСтраницаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пользовательскиеДатчикиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.светToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.умныйЗвонокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьЛогфайлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.панельНавигацииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПроектеASHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panelNameLabel = new System.Windows.Forms.Label();
            this.VSeparator2 = new System.Windows.Forms.Label();
            this.VSeparator1 = new System.Windows.Forms.Label();
            this.goNextLabel = new System.Windows.Forms.Label();
            this.goBackLabel = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.панельToolStripMenuItem,
            this.видToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(381, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // панельToolStripMenuItem
            // 
            this.панельToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.соединитьсяToolStripMenuItem,
            this.регистрацияToolStripMenuItem,
            this.управлениеToolStripMenuItem,
            this.копироватьЛогфайлToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.панельToolStripMenuItem.Name = "панельToolStripMenuItem";
            this.панельToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.панельToolStripMenuItem.Tag = "PanelText";
            this.панельToolStripMenuItem.Text = "Панель";
            // 
            // соединитьсяToolStripMenuItem
            // 
            this.соединитьсяToolStripMenuItem.Name = "соединитьсяToolStripMenuItem";
            this.соединитьсяToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.соединитьсяToolStripMenuItem.Tag = "Connect";
            this.соединитьсяToolStripMenuItem.Text = "Подключиться";
            this.соединитьсяToolStripMenuItem.Click += new System.EventHandler(this.соединитьсяToolStripMenuItem_Click);
            // 
            // регистрацияToolStripMenuItem
            // 
            this.регистрацияToolStripMenuItem.Name = "регистрацияToolStripMenuItem";
            this.регистрацияToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.регистрацияToolStripMenuItem.Text = "Регистрация";
            this.регистрацияToolStripMenuItem.Click += new System.EventHandler(this.регистрацияToolStripMenuItem_Click);
            // 
            // управлениеToolStripMenuItem
            // 
            this.управлениеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.главнаяСтраницаToolStripMenuItem,
            this.пользовательскиеДатчикиToolStripMenuItem,
            this.светToolStripMenuItem,
            this.умныйЗвонокToolStripMenuItem,
            this.настройкиToolStripMenuItem});
            this.управлениеToolStripMenuItem.Name = "управлениеToolStripMenuItem";
            this.управлениеToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.управлениеToolStripMenuItem.Tag = "Management";
            this.управлениеToolStripMenuItem.Text = "Управление";
            // 
            // главнаяСтраницаToolStripMenuItem
            // 
            this.главнаяСтраницаToolStripMenuItem.Name = "главнаяСтраницаToolStripMenuItem";
            this.главнаяСтраницаToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.главнаяСтраницаToolStripMenuItem.Tag = "Homepage";
            this.главнаяСтраницаToolStripMenuItem.Text = "Главная страница";
            this.главнаяСтраницаToolStripMenuItem.Click += new System.EventHandler(this.главнаяСтраницаToolStripMenuItem_Click);
            // 
            // пользовательскиеДатчикиToolStripMenuItem
            // 
            this.пользовательскиеДатчикиToolStripMenuItem.Name = "пользовательскиеДатчикиToolStripMenuItem";
            this.пользовательскиеДатчикиToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.пользовательскиеДатчикиToolStripMenuItem.Text = "Пользовательские датчики";
            this.пользовательскиеДатчикиToolStripMenuItem.Click += new System.EventHandler(this.пользовательскиеДатчикиToolStripMenuItem_Click);
            // 
            // светToolStripMenuItem
            // 
            this.светToolStripMenuItem.Name = "светToolStripMenuItem";
            this.светToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.светToolStripMenuItem.Tag = "Light";
            this.светToolStripMenuItem.Text = "Свет";
            this.светToolStripMenuItem.Click += new System.EventHandler(this.светToolStripMenuItem_Click);
            // 
            // умныйЗвонокToolStripMenuItem
            // 
            this.умныйЗвонокToolStripMenuItem.Name = "умныйЗвонокToolStripMenuItem";
            this.умныйЗвонокToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.умныйЗвонокToolStripMenuItem.Text = "Умный звонок";
            this.умныйЗвонокToolStripMenuItem.Click += new System.EventHandler(this.умныйЗвонокToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.настройкиToolStripMenuItem.Tag = "Settings";
            this.настройкиToolStripMenuItem.Text = "Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
            // 
            // копироватьЛогфайлToolStripMenuItem
            // 
            this.копироватьЛогфайлToolStripMenuItem.Name = "копироватьЛогфайлToolStripMenuItem";
            this.копироватьЛогфайлToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.копироватьЛогфайлToolStripMenuItem.Text = "Копировать лог-файл";
            this.копироватьЛогфайлToolStripMenuItem.Click += new System.EventHandler(this.копироватьЛогфайлToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.выходToolStripMenuItem.Tag = "Exit";
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // видToolStripMenuItem
            // 
            this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.панельНавигацииToolStripMenuItem});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.видToolStripMenuItem.Text = "Вид";
            // 
            // панельНавигацииToolStripMenuItem
            // 
            this.панельНавигацииToolStripMenuItem.CheckOnClick = true;
            this.панельНавигацииToolStripMenuItem.Name = "панельНавигацииToolStripMenuItem";
            this.панельНавигацииToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.панельНавигацииToolStripMenuItem.Text = "Панель навигации";
            this.панельНавигацииToolStripMenuItem.CheckedChanged += new System.EventHandler(this.панельНавигацииToolStripMenuItem_CheckedChanged);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem,
            this.оПроектеASHToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Tag = "Help";
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.оПрограммеToolStripMenuItem.Tag = "AboutProg";
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // оПроектеASHToolStripMenuItem
            // 
            this.оПроектеASHToolStripMenuItem.Name = "оПроектеASHToolStripMenuItem";
            this.оПроектеASHToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.оПроектеASHToolStripMenuItem.Tag = "AboutProj";
            this.оПроектеASHToolStripMenuItem.Text = "О проекте ASH";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panelNameLabel);
            this.panel3.Controls.Add(this.VSeparator2);
            this.panel3.Controls.Add(this.VSeparator1);
            this.panel3.Controls.Add(this.goNextLabel);
            this.panel3.Controls.Add(this.goBackLabel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 24);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(381, 29);
            this.panel3.TabIndex = 2;
            // 
            // panelNameLabel
            // 
            this.panelNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNameLabel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelNameLabel.Location = new System.Drawing.Point(50, 0);
            this.panelNameLabel.Name = "panelNameLabel";
            this.panelNameLabel.Size = new System.Drawing.Size(281, 29);
            this.panelNameLabel.TabIndex = 2;
            this.panelNameLabel.Text = "Главная панель";
            this.panelNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelNameLabel.Click += new System.EventHandler(this.panelNameLabel_Click);
            // 
            // VSeparator2
            // 
            this.VSeparator2.Dock = System.Windows.Forms.DockStyle.Right;
            this.VSeparator2.Font = new System.Drawing.Font("Lucida Sans Unicode", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.VSeparator2.ForeColor = System.Drawing.Color.Silver;
            this.VSeparator2.Location = new System.Drawing.Point(331, 0);
            this.VSeparator2.Name = "VSeparator2";
            this.VSeparator2.Size = new System.Drawing.Size(18, 29);
            this.VSeparator2.TabIndex = 4;
            this.VSeparator2.Text = "|";
            this.VSeparator2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VSeparator1
            // 
            this.VSeparator1.Dock = System.Windows.Forms.DockStyle.Left;
            this.VSeparator1.Font = new System.Drawing.Font("Lucida Sans Unicode", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.VSeparator1.ForeColor = System.Drawing.Color.Silver;
            this.VSeparator1.Location = new System.Drawing.Point(32, 0);
            this.VSeparator1.Name = "VSeparator1";
            this.VSeparator1.Size = new System.Drawing.Size(18, 29);
            this.VSeparator1.TabIndex = 3;
            this.VSeparator1.Text = "|";
            this.VSeparator1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // goNextLabel
            // 
            this.goNextLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.goNextLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.goNextLabel.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.goNextLabel.Location = new System.Drawing.Point(349, 0);
            this.goNextLabel.Name = "goNextLabel";
            this.goNextLabel.Size = new System.Drawing.Size(32, 29);
            this.goNextLabel.TabIndex = 1;
            this.goNextLabel.Text = ">";
            this.goNextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.goNextLabel.Click += new System.EventHandler(this.goNextLabel_Click);
            // 
            // goBackLabel
            // 
            this.goBackLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.goBackLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.goBackLabel.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.goBackLabel.Location = new System.Drawing.Point(0, 0);
            this.goBackLabel.Name = "goBackLabel";
            this.goBackLabel.Size = new System.Drawing.Size(32, 29);
            this.goBackLabel.TabIndex = 0;
            this.goBackLabel.Text = "<";
            this.goBackLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.goBackLabel.Click += new System.EventHandler(this.goBackLabel_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "log";
            this.saveFileDialog1.FileName = "achsmarthome.log";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "AchSmartHome";
            this.notifyIcon1.Visible = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(381, 185);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Tag = "FormTitle";
            this.Text = "Управление умным домом AchSmartHome";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem панельToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem соединитьсяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem управлениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem главнаяСтраницаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem светToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПроектеASHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem регистрацияToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label goNextLabel;
        private System.Windows.Forms.Label goBackLabel;
        private System.Windows.Forms.Label panelNameLabel;
        private System.Windows.Forms.Label VSeparator2;
        private System.Windows.Forms.Label VSeparator1;
        private System.Windows.Forms.ToolStripMenuItem копироватьЛогфайлToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem пользовательскиеДатчикиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem умныйЗвонокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem панельНавигацииToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Timer timer1;
    }
}

