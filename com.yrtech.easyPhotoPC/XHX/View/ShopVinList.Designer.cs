namespace XHX.View
{
    partial class ShopVinList
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
            this.grdShop = new DevExpress.XtraEditors.PanelControl();
            this.txtVin = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cboProjects = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnModule = new DevExpress.XtraEditors.ButtonEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.txtShopName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.grcShop = new DevExpress.XtraGrid.GridControl();
            this.grvShop = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcShopCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcShopName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPassword = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboSaleBigAreaInGrid = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.chkUseChk = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.cboAfterBigAreaInGrid = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdShop)).BeginInit();
            this.grdShop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProjects.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtShopName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcShop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvShop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSaleBigAreaInGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseChk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAfterBigAreaInGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // grdShop
            // 
            this.grdShop.Controls.Add(this.txtVin);
            this.grdShop.Controls.Add(this.labelControl1);
            this.grdShop.Controls.Add(this.cboProjects);
            this.grdShop.Controls.Add(this.labelControl3);
            this.grdShop.Controls.Add(this.labelControl5);
            this.grdShop.Controls.Add(this.btnModule);
            this.grdShop.Controls.Add(this.simpleButton1);
            this.grdShop.Controls.Add(this.txtShopName);
            this.grdShop.Controls.Add(this.labelControl4);
            this.grdShop.Dock = System.Windows.Forms.DockStyle.Top;
            this.grdShop.Location = new System.Drawing.Point(5, 5);
            this.grdShop.Margin = new System.Windows.Forms.Padding(0);
            this.grdShop.Name = "grdShop";
            this.grdShop.Size = new System.Drawing.Size(1064, 80);
            this.grdShop.TabIndex = 10;
            // 
            // txtVin
            // 
            this.txtVin.Location = new System.Drawing.Point(652, 9);
            this.txtVin.Name = "txtVin";
            this.txtVin.Size = new System.Drawing.Size(219, 21);
            this.txtVin.TabIndex = 99;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(572, 9);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(29, 14);
            this.labelControl1.TabIndex = 98;
            this.labelControl1.Text = "Vin码";
            // 
            // cboProjects
            // 
            this.cboProjects.Location = new System.Drawing.Point(90, 9);
            this.cboProjects.Name = "cboProjects";
            this.cboProjects.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.cboProjects.Properties.Appearance.Options.UseBackColor = true;
            this.cboProjects.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProjects.Size = new System.Drawing.Size(100, 21);
            this.cboProjects.TabIndex = 97;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Options.UseTextOptions = true;
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl3.Location = new System.Drawing.Point(42, 9);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 14);
            this.labelControl3.TabIndex = 96;
            this.labelControl3.Text = "项目名";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Options.UseTextOptions = true;
            this.labelControl5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl5.Location = new System.Drawing.Point(31, 59);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 95;
            this.labelControl5.Text = "模板路径";
            // 
            // btnModule
            // 
            this.btnModule.EditValue = "";
            this.btnModule.Location = new System.Drawing.Point(90, 56);
            this.btnModule.Name = "btnModule";
            this.btnModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.btnModule.Size = new System.Drawing.Size(287, 21);
            this.btnModule.TabIndex = 94;
            this.btnModule.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnModule_ButtonClick);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(412, 41);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(116, 36);
            this.simpleButton1.TabIndex = 44;
            this.simpleButton1.Text = "上传清单数据";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // txtShopName
            // 
            this.txtShopName.Location = new System.Drawing.Point(323, 9);
            this.txtShopName.Name = "txtShopName";
            this.txtShopName.Size = new System.Drawing.Size(219, 21);
            this.txtShopName.TabIndex = 3;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(243, 9);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 2;
            this.labelControl4.Text = "经销商名称";
            // 
            // labelControl2
            // 
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl2.Location = new System.Drawing.Point(5, 85);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(1064, 5);
            this.labelControl2.TabIndex = 11;
            // 
            // grcShop
            // 
            this.grcShop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grcShop.Location = new System.Drawing.Point(5, 90);
            this.grcShop.MainView = this.grvShop;
            this.grcShop.Name = "grcShop";
            this.grcShop.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboSaleBigAreaInGrid,
            this.chkUseChk,
            this.cboAfterBigAreaInGrid});
            this.grcShop.Size = new System.Drawing.Size(1064, 518);
            this.grcShop.TabIndex = 12;
            this.grcShop.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvShop});
            // 
            // grvShop
            // 
            this.grvShop.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcShopCode,
            this.gcShopName,
            this.gridColumn6,
            this.gcPassword,
            this.gridColumn1});
            this.grvShop.GridControl = this.grcShop;
            this.grvShop.Name = "grvShop";
            this.grvShop.OptionsView.ShowGroupPanel = false;
            this.grvShop.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView1_CustomDrawCell);
            this.grvShop.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridView1_ShowingEditor);
            // 
            // gcShopCode
            // 
            this.gcShopCode.AppearanceCell.Options.UseTextOptions = true;
            this.gcShopCode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcShopCode.AppearanceHeader.Options.UseTextOptions = true;
            this.gcShopCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcShopCode.Caption = "期号";
            this.gcShopCode.FieldName = "ProjectCode";
            this.gcShopCode.Name = "gcShopCode";
            this.gcShopCode.Visible = true;
            this.gcShopCode.VisibleIndex = 0;
            this.gcShopCode.Width = 74;
            // 
            // gcShopName
            // 
            this.gcShopName.AppearanceCell.Options.UseTextOptions = true;
            this.gcShopName.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcShopName.AppearanceHeader.Options.UseTextOptions = true;
            this.gcShopName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcShopName.Caption = "经销商名称";
            this.gcShopName.FieldName = "ShopCode";
            this.gcShopName.Name = "gcShopName";
            this.gcShopName.Visible = true;
            this.gcShopName.VisibleIndex = 1;
            this.gcShopName.Width = 82;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "区域";
            this.gridColumn6.FieldName = "AreaCode";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            this.gridColumn6.Width = 70;
            // 
            // gcPassword
            // 
            this.gcPassword.AppearanceCell.Options.UseTextOptions = true;
            this.gcPassword.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcPassword.AppearanceHeader.Options.UseTextOptions = true;
            this.gcPassword.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcPassword.Caption = "Vin码";
            this.gcPassword.FieldName = "VinCode";
            this.gcPassword.Name = "gcPassword";
            this.gcPassword.Visible = true;
            this.gcPassword.VisibleIndex = 3;
            this.gcPassword.Width = 89;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "类型";
            this.gridColumn1.FieldName = "Type";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 4;
            // 
            // cboSaleBigAreaInGrid
            // 
            this.cboSaleBigAreaInGrid.AutoHeight = false;
            this.cboSaleBigAreaInGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSaleBigAreaInGrid.Name = "cboSaleBigAreaInGrid";
            // 
            // chkUseChk
            // 
            this.chkUseChk.AutoHeight = false;
            this.chkUseChk.Name = "chkUseChk";
            // 
            // cboAfterBigAreaInGrid
            // 
            this.cboAfterBigAreaInGrid.AutoHeight = false;
            this.cboAfterBigAreaInGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAfterBigAreaInGrid.Name = "cboAfterBigAreaInGrid";
            // 
            // ShopVinList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.grcShop);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.grdShop);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ShopVinList";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(1074, 613);
            this.Load += new System.EventHandler(this.Shop_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdShop)).EndInit();
            this.grdShop.ResumeLayout(false);
            this.grdShop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProjects.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtShopName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcShop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvShop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSaleBigAreaInGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseChk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAfterBigAreaInGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl grdShop;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraGrid.GridControl grcShop;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtShopName;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cboSaleBigAreaInGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView grvShop;
        private DevExpress.XtraGrid.Columns.GridColumn gcShopCode;
        private DevExpress.XtraGrid.Columns.GridColumn gcShopName;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkUseChk;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cboAfterBigAreaInGrid;
        private DevExpress.XtraGrid.Columns.GridColumn gcPassword;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ButtonEdit btnModule;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.ComboBoxEdit cboProjects;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtVin;
        private DevExpress.XtraEditors.LabelControl labelControl1;


    }
}