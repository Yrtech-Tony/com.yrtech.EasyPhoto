namespace XHX.View
{
    partial class Shop
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
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnModule = new DevExpress.XtraEditors.ButtonEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnPassword = new DevExpress.XtraEditors.SimpleButton();
            this.txtShopName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.grcShop = new DevExpress.XtraGrid.GridControl();
            this.grvShop = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcShopCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcShopName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUseChk = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkUseChk = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gcPassword = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboSaleBigAreaInGrid = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.cboAfterBigAreaInGrid = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdShop)).BeginInit();
            this.grdShop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtShopName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcShop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvShop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseChk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSaleBigAreaInGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAfterBigAreaInGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // grdShop
            // 
            this.grdShop.Controls.Add(this.labelControl5);
            this.grdShop.Controls.Add(this.btnModule);
            this.grdShop.Controls.Add(this.simpleButton1);
            this.grdShop.Controls.Add(this.btnPassword);
            this.grdShop.Controls.Add(this.txtShopName);
            this.grdShop.Controls.Add(this.labelControl4);
            this.grdShop.Dock = System.Windows.Forms.DockStyle.Top;
            this.grdShop.Location = new System.Drawing.Point(5, 5);
            this.grdShop.Margin = new System.Windows.Forms.Padding(0);
            this.grdShop.Name = "grdShop";
            this.grdShop.Size = new System.Drawing.Size(1064, 80);
            this.grdShop.TabIndex = 10;
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
            this.simpleButton1.Text = "上传经销商信息";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btnPassword
            // 
            this.btnPassword.Location = new System.Drawing.Point(534, 41);
            this.btnPassword.Name = "btnPassword";
            this.btnPassword.Size = new System.Drawing.Size(116, 36);
            this.btnPassword.TabIndex = 43;
            this.btnPassword.Text = "生成随机密码";
            this.btnPassword.Click += new System.EventHandler(this.btnPassword_Click);
            // 
            // txtShopName
            // 
            this.txtShopName.Location = new System.Drawing.Point(90, 17);
            this.txtShopName.Name = "txtShopName";
            this.txtShopName.Size = new System.Drawing.Size(219, 21);
            this.txtShopName.TabIndex = 3;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(19, 20);
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
            this.gcUseChk,
            this.gcPassword});
            this.grvShop.GridControl = this.grcShop;
            this.grvShop.Name = "grvShop";
            this.grvShop.OptionsView.ShowGroupPanel = false;
            this.grvShop.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView1_CustomDrawCell);
            this.grvShop.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridView1_ShowingEditor);
            // 
            // gcShopCode
            // 
            this.gcShopCode.AppearanceHeader.Options.UseTextOptions = true;
            this.gcShopCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcShopCode.Caption = "经销商代码";
            this.gcShopCode.FieldName = "ShopCode";
            this.gcShopCode.Name = "gcShopCode";
            this.gcShopCode.Visible = true;
            this.gcShopCode.VisibleIndex = 0;
            this.gcShopCode.Width = 74;
            // 
            // gcShopName
            // 
            this.gcShopName.AppearanceHeader.Options.UseTextOptions = true;
            this.gcShopName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcShopName.Caption = "经销商名称";
            this.gcShopName.FieldName = "ShopName";
            this.gcShopName.Name = "gcShopName";
            this.gcShopName.Visible = true;
            this.gcShopName.VisibleIndex = 1;
            this.gcShopName.Width = 82;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "区域";
            this.gridColumn6.FieldName = "AreaCode";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            this.gridColumn6.Width = 70;
            // 
            // gcUseChk
            // 
            this.gcUseChk.AppearanceHeader.Options.UseTextOptions = true;
            this.gcUseChk.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcUseChk.Caption = "使用与否";
            this.gcUseChk.ColumnEdit = this.chkUseChk;
            this.gcUseChk.FieldName = "UseChk";
            this.gcUseChk.Name = "gcUseChk";
            this.gcUseChk.Visible = true;
            this.gcUseChk.VisibleIndex = 3;
            this.gcUseChk.Width = 70;
            // 
            // chkUseChk
            // 
            this.chkUseChk.AutoHeight = false;
            this.chkUseChk.Name = "chkUseChk";
            // 
            // gcPassword
            // 
            this.gcPassword.AppearanceHeader.Options.UseTextOptions = true;
            this.gcPassword.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcPassword.Caption = "密码";
            this.gcPassword.FieldName = "Password";
            this.gcPassword.Name = "gcPassword";
            this.gcPassword.Visible = true;
            this.gcPassword.VisibleIndex = 4;
            this.gcPassword.Width = 89;
            // 
            // cboSaleBigAreaInGrid
            // 
            this.cboSaleBigAreaInGrid.AutoHeight = false;
            this.cboSaleBigAreaInGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSaleBigAreaInGrid.Name = "cboSaleBigAreaInGrid";
            // 
            // cboAfterBigAreaInGrid
            // 
            this.cboAfterBigAreaInGrid.AutoHeight = false;
            this.cboAfterBigAreaInGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAfterBigAreaInGrid.Name = "cboAfterBigAreaInGrid";
            // 
            // Shop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.grcShop);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.grdShop);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Shop";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(1074, 613);
            this.Load += new System.EventHandler(this.Shop_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdShop)).EndInit();
            this.grdShop.ResumeLayout(false);
            this.grdShop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtShopName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcShop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvShop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseChk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSaleBigAreaInGrid)).EndInit();
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
        private DevExpress.XtraGrid.Columns.GridColumn gcUseChk;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkUseChk;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cboAfterBigAreaInGrid;
        private DevExpress.XtraEditors.SimpleButton btnPassword;
        private DevExpress.XtraGrid.Columns.GridColumn gcPassword;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ButtonEdit btnModule;


    }
}