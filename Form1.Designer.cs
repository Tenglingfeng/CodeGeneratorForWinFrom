namespace CodeGenerator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.connString = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tablesChecked = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.connection = new System.Windows.Forms.Button();
            this.Selected = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "请输入数据库连接地址";
            // 
            // connString
            // 
            this.connString.Location = new System.Drawing.Point(12, 38);
            this.connString.Name = "connString";
            this.connString.Size = new System.Drawing.Size(405, 23);
            this.connString.TabIndex = 1;
            this.connString.Text = "Server=localhost;database=dictdb;uid=root;pwd=;";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(444, 458);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "生成";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_ClickAsync);
            // 
            // tablesChecked
            // 
            this.tablesChecked.BackColor = System.Drawing.SystemColors.Window;
            this.tablesChecked.CheckOnClick = true;
            this.tablesChecked.FormattingEnabled = true;
            this.tablesChecked.Location = new System.Drawing.Point(12, 99);
            this.tablesChecked.Name = "tablesChecked";
            this.tablesChecked.Size = new System.Drawing.Size(405, 382);
            this.tablesChecked.TabIndex = 4;
            this.tablesChecked.SelectedValueChanged += new System.EventHandler(this.tablesChecked_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "请选择表名";
            // 
            // connection
            // 
            this.connection.Location = new System.Drawing.Point(444, 38);
            this.connection.Name = "connection";
            this.connection.Size = new System.Drawing.Size(75, 23);
            this.connection.TabIndex = 5;
            this.connection.Text = "连接";
            this.connection.UseVisualStyleBackColor = true;
            this.connection.Click += new System.EventHandler(this.connection_Click);
            // 
            // Selected
            // 
            this.Selected.Location = new System.Drawing.Point(154, 75);
            this.Selected.Name = "Selected";
            this.Selected.Size = new System.Drawing.Size(75, 23);
            this.Selected.TabIndex = 6;
            this.Selected.Text = "全选";
            this.Selected.UseVisualStyleBackColor = true;
            this.Selected.Click += new System.EventHandler(this.Selected_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 523);
            this.Controls.Add(this.Selected);
            this.Controls.Add(this.connection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tablesChecked);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.connString);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = ".Net Code Generator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox connString;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox tablesChecked;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button connection;
        private System.Windows.Forms.Button Selected;
    }
}

