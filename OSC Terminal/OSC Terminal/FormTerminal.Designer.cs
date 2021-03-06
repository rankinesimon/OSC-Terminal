﻿namespace OSC_Terminal
{
    partial class FormTerminal
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemReceivePort = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTerminal = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSourceCode = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelPacketsReceived = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelPacketRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBox = new System.Windows.Forms.TextBox();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemReceivePort,
            this.toolStripMenuItemTerminal,
            this.toolStripMenuItemHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(584, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // toolStripMenuItemReceivePort
            // 
            this.toolStripMenuItemReceivePort.Name = "toolStripMenuItemReceivePort";
            this.toolStripMenuItemReceivePort.Size = new System.Drawing.Size(84, 20);
            this.toolStripMenuItemReceivePort.Text = "Receive Port";
            this.toolStripMenuItemReceivePort.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMenuItemReceivePort_DropDownItemClicked);
            // 
            // toolStripMenuItemTerminal
            // 
            this.toolStripMenuItemTerminal.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemEnabled,
            this.toolStripMenuItemClear});
            this.toolStripMenuItemTerminal.Name = "toolStripMenuItemTerminal";
            this.toolStripMenuItemTerminal.Size = new System.Drawing.Size(66, 20);
            this.toolStripMenuItemTerminal.Text = "Terminal";
            // 
            // toolStripMenuItemEnabled
            // 
            this.toolStripMenuItemEnabled.Checked = true;
            this.toolStripMenuItemEnabled.CheckOnClick = true;
            this.toolStripMenuItemEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemEnabled.Name = "toolStripMenuItemEnabled";
            this.toolStripMenuItemEnabled.Size = new System.Drawing.Size(116, 22);
            this.toolStripMenuItemEnabled.Text = "Enabled";
            this.toolStripMenuItemEnabled.CheckStateChanged += new System.EventHandler(this.toolStripMenuItemEnabled_CheckStateChanged);
            // 
            // toolStripMenuItemClear
            // 
            this.toolStripMenuItemClear.Name = "toolStripMenuItemClear";
            this.toolStripMenuItemClear.Size = new System.Drawing.Size(116, 22);
            this.toolStripMenuItemClear.Text = "Clear";
            this.toolStripMenuItemClear.Click += new System.EventHandler(this.toolStripMenuItemClear_Click);
            // 
            // toolStripMenuItemHelp
            // 
            this.toolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAbout,
            this.toolStripMenuItemSourceCode});
            this.toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
            this.toolStripMenuItemHelp.Size = new System.Drawing.Size(44, 20);
            this.toolStripMenuItemHelp.Text = "Help";
            // 
            // toolStripMenuItemAbout
            // 
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Size = new System.Drawing.Size(141, 22);
            this.toolStripMenuItemAbout.Text = "About";
            this.toolStripMenuItemAbout.Click += new System.EventHandler(this.toolStripMenuItemAbout_Click);
            // 
            // toolStripMenuItemSourceCode
            // 
            this.toolStripMenuItemSourceCode.Name = "toolStripMenuItemSourceCode";
            this.toolStripMenuItemSourceCode.Size = new System.Drawing.Size(141, 22);
            this.toolStripMenuItemSourceCode.Text = "Source Code";
            this.toolStripMenuItemSourceCode.Click += new System.EventHandler(this.toolStripMenuItemSourceCode_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelPacketsReceived,
            this.toolStripStatusLabelPacketRate});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip.Location = new System.Drawing.Point(0, 342);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(584, 20);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabelPacketsReceived
            // 
            this.toolStripStatusLabelPacketsReceived.Name = "toolStripStatusLabelPacketsReceived";
            this.toolStripStatusLabelPacketsReceived.Size = new System.Drawing.Size(199, 15);
            this.toolStripStatusLabelPacketsReceived.Text = "toolStripStatusLabelPacketsReceived";
            // 
            // toolStripStatusLabelPacketRate
            // 
            this.toolStripStatusLabelPacketRate.Name = "toolStripStatusLabelPacketRate";
            this.toolStripStatusLabelPacketRate.Size = new System.Drawing.Size(170, 15);
            this.toolStripStatusLabelPacketRate.Text = "toolStripStatusLabelPacketRate";
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.Color.Black;
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.textBox.ForeColor = System.Drawing.SystemColors.Info;
            this.textBox.Location = new System.Drawing.Point(0, 24);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(584, 318);
            this.textBox.TabIndex = 1;
            // 
            // FormTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormTerminal";
            this.Text = "FormTerminal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTerminal_FormClosing);
            this.Load += new System.EventHandler(this.FormTerminal_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPacketsReceived;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPacketRate;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSourceCode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTerminal;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEnabled;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClear;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemReceivePort;
    }
}

