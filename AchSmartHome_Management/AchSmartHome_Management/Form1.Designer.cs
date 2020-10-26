namespace AchSmartHome_Management
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.панельToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.соединитьсяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.регистрацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.управлениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.главнаяСтраницаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.светToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПроектеASHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.панельToolStripMenuItem,
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
            this.выходToolStripMenuItem});
            this.панельToolStripMenuItem.Name = "панельToolStripMenuItem";
            this.панельToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.панельToolStripMenuItem.Tag = "PanelText";
            this.панельToolStripMenuItem.Text = "Панель";
            // 
            // соединитьсяToolStripMenuItem
            // 
            this.соединитьсяToolStripMenuItem.Name = "соединитьсяToolStripMenuItem";
            this.соединитьсяToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.соединитьсяToolStripMenuItem.Tag = "Connect";
            this.соединитьсяToolStripMenuItem.Text = "Подключиться";
            this.соединитьсяToolStripMenuItem.Click += new System.EventHandler(this.соединитьсяToolStripMenuItem_Click);
            // 
            // регистрацияToolStripMenuItem
            // 
            this.регистрацияToolStripMenuItem.Name = "регистрацияToolStripMenuItem";
            this.регистрацияToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.регистрацияToolStripMenuItem.Text = "Регистрация";
            this.регистрацияToolStripMenuItem.Click += new System.EventHandler(this.регистрацияToolStripMenuItem_Click);
            // 
            // управлениеToolStripMenuItem
            // 
            this.управлениеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.главнаяСтраницаToolStripMenuItem,
            this.светToolStripMenuItem,
            this.настройкиToolStripMenuItem});
            this.управлениеToolStripMenuItem.Name = "управлениеToolStripMenuItem";
            this.управлениеToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.управлениеToolStripMenuItem.Tag = "Management";
            this.управлениеToolStripMenuItem.Text = "Управление";
            // 
            // главнаяСтраницаToolStripMenuItem
            // 
            this.главнаяСтраницаToolStripMenuItem.Name = "главнаяСтраницаToolStripMenuItem";
            this.главнаяСтраницаToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.главнаяСтраницаToolStripMenuItem.Tag = "Homepage";
            this.главнаяСтраницаToolStripMenuItem.Text = "Главная страница";
            this.главнаяСтраницаToolStripMenuItem.Click += new System.EventHandler(this.главнаяСтраницаToolStripMenuItem_Click);
            // 
            // светToolStripMenuItem
            // 
            this.светToolStripMenuItem.Name = "светToolStripMenuItem";
            this.светToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.светToolStripMenuItem.Tag = "Light";
            this.светToolStripMenuItem.Text = "Свет";
            this.светToolStripMenuItem.Click += new System.EventHandler(this.светToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.настройкиToolStripMenuItem.Tag = "Settings";
            this.настройкиToolStripMenuItem.Text = "Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.выходToolStripMenuItem.Tag = "Exit";
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
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
            // 
            // оПроектеASHToolStripMenuItem
            // 
            this.оПроектеASHToolStripMenuItem.Name = "оПроектеASHToolStripMenuItem";
            this.оПроектеASHToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.оПроектеASHToolStripMenuItem.Tag = "AboutProj";
            this.оПроектеASHToolStripMenuItem.Text = "О проекте ASH";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(381, 185);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Tag = "FormTitle";
            this.Text = "Управление умным домом AchSmartHome";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
    }
}

