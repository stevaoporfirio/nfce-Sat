namespace invoiceServerApp
{
    partial class FormStatus
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnInutilizar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.txtDtEmissao = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtNumRecibo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtStatusNFCe = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtWSID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNumConta = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNumNFCe = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtChaveAcesso = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(14, 131);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(573, 222);
            this.dataGridView1.TabIndex = 0;
            // 
            // btnInutilizar
            // 
            this.btnInutilizar.Enabled = false;
            this.btnInutilizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInutilizar.Location = new System.Drawing.Point(207, 355);
            this.btnInutilizar.Name = "btnInutilizar";
            this.btnInutilizar.Size = new System.Drawing.Size(182, 55);
            this.btnInutilizar.TabIndex = 1;
            this.btnInutilizar.Text = "Inutilizar NFCE";
            this.btnInutilizar.UseVisualStyleBackColor = true;
            this.btnInutilizar.Click += new System.EventHandler(this.btnInutilizar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Enabled = false;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(401, 354);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(182, 56);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "Cancelar NFCE";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtDtEmissao
            // 
            this.txtDtEmissao.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDtEmissao.ForeColor = System.Drawing.Color.Black;
            this.txtDtEmissao.Location = new System.Drawing.Point(441, 100);
            this.txtDtEmissao.Name = "txtDtEmissao";
            this.txtDtEmissao.ReadOnly = true;
            this.txtDtEmissao.Size = new System.Drawing.Size(142, 21);
            this.txtDtEmissao.TabIndex = 34;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(438, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 15);
            this.label7.TabIndex = 33;
            this.label7.Text = "Data de Emissão:";
            // 
            // txtNumRecibo
            // 
            this.txtNumRecibo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumRecibo.ForeColor = System.Drawing.Color.Black;
            this.txtNumRecibo.Location = new System.Drawing.Point(309, 100);
            this.txtNumRecibo.Name = "txtNumRecibo";
            this.txtNumRecibo.ReadOnly = true;
            this.txtNumRecibo.Size = new System.Drawing.Size(123, 21);
            this.txtNumRecibo.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(309, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 15);
            this.label6.TabIndex = 31;
            this.label6.Text = "N° Recibo:";
            // 
            // txtStatusNFCe
            // 
            this.txtStatusNFCe.ForeColor = System.Drawing.Color.Black;
            this.txtStatusNFCe.Location = new System.Drawing.Point(445, 65);
            this.txtStatusNFCe.Name = "txtStatusNFCe";
            this.txtStatusNFCe.ReadOnly = true;
            this.txtStatusNFCe.Size = new System.Drawing.Size(142, 20);
            this.txtStatusNFCe.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(442, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Status NFC-e:";
            // 
            // txtWSID
            // 
            this.txtWSID.ForeColor = System.Drawing.Color.Black;
            this.txtWSID.Location = new System.Drawing.Point(313, 65);
            this.txtWSID.Name = "txtWSID";
            this.txtWSID.ReadOnly = true;
            this.txtWSID.Size = new System.Drawing.Size(123, 20);
            this.txtWSID.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(313, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Workstation ID:";
            // 
            // txtNumConta
            // 
            this.txtNumConta.ForeColor = System.Drawing.Color.Black;
            this.txtNumConta.Location = new System.Drawing.Point(208, 65);
            this.txtNumConta.Name = "txtNumConta";
            this.txtNumConta.ReadOnly = true;
            this.txtNumConta.Size = new System.Drawing.Size(99, 20);
            this.txtNumConta.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(208, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "N° Conta:";
            // 
            // txtNumNFCe
            // 
            this.txtNumNFCe.ForeColor = System.Drawing.Color.Black;
            this.txtNumNFCe.Location = new System.Drawing.Point(14, 65);
            this.txtNumNFCe.Name = "txtNumNFCe";
            this.txtNumNFCe.ReadOnly = true;
            this.txtNumNFCe.Size = new System.Drawing.Size(185, 20);
            this.txtNumNFCe.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(14, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "N° NFC-e:";
            // 
            // txtChaveAcesso
            // 
            this.txtChaveAcesso.ForeColor = System.Drawing.Color.Black;
            this.txtChaveAcesso.Location = new System.Drawing.Point(14, 104);
            this.txtChaveAcesso.Name = "txtChaveAcesso";
            this.txtChaveAcesso.ReadOnly = true;
            this.txtChaveAcesso.Size = new System.Drawing.Size(293, 20);
            this.txtChaveAcesso.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Chave NFCE";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtNumRecibo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtDtEmissao);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnCancelar);
            this.groupBox1.Controls.Add(this.btnInutilizar);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(599, 416);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Historico NFCE";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(13, 354);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 56);
            this.button1.TabIndex = 35;
            this.button1.Text = "Imprimir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 432);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStatusNFCe);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtWSID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNumConta);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNumNFCe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtChaveAcesso);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormStatus";
            this.Text = "Histórico NFCE";
            this.Load += new System.EventHandler(this.FormStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnInutilizar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox txtDtEmissao;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNumRecibo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtStatusNFCe;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtWSID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNumConta;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNumNFCe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtChaveAcesso;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
    }
}