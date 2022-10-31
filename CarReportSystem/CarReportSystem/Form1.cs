using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CarReportSystem 
{
    public partial class Form1 : Form 
    {
        

        // バイト配列をImageオブジェクトに変換
        public static Image ByteArrayToImage(byte[] b) {
            ImageConverter imgconv = new ImageConverter();
            Image img = (Image)imgconv.ConvertFrom(b);
            return img;
        }

        // Imageオブジェクトをバイト配列に変換
        public static byte[] ImageToByteArray(Image img) {
            ImageConverter imgconv = new ImageConverter();
            byte[] b = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return b;
        }



        //設定情報保存用オブジェクト
        Settings settings = Settings.getInstance();

        //カーレポート管理用リスト
        BindingList<CarReport> listCarReports = new BindingList<CarReport>();

        int mode = 0;
        public Form1() 
        {
            InitializeComponent();
        }

        private void btExit_Click(object sender, EventArgs e) 
        {
            //アプリケーションの終了
            Application.Exit();
        }

        private void 設定ToolStripMenuItem_Click_1(object sender, EventArgs e) {

            //色設定ダイアログ
            if (cdColorSelect.ShowDialog() == DialogResult.OK) {
                BackColor = cdColorSelect.Color;
                settings.MainFormColor = cdColorSelect.Color.ToArgb();//;cdColorSelect.Color;   //設定オブジェクトへセット
            }
        }

        private void pbPicture_Click(object sender, EventArgs e) {
            pbPicture.SizeMode = (PictureBoxSizeMode)mode;
            mode = mode < 4 ? ++mode : 0;
        }

        private void Form1_FormClased(object sender, FormClosedEventArgs e) 
        {
            //設定ファイルをシリアル化
            using (var writr = XmlWriter.Create("settings.xml")) 
            {
                var serializer = new XmlSerializer(settings.GetType());
                serializer.Serialize(writr,settings);
            }
        }

        private void Form1_Load_1(object sender, EventArgs e) {
            // TODO: このコード行はデータを 'infosys202204DataSet.CarReportDB' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
            this.carReportDBTableAdapter.Fill(this.infosys202204DataSet.CarReportDB);
            try {
                //設定ファイルを逆シリアル化して背景の色に設定
                using (var reader = XmlReader.Create("settings.xml")) 
                {
                    var serializer = new XmlSerializer(settings.GetType());
                    settings = serializer.Deserialize(reader) as Settings;
                    BackColor = Color.FromArgb(settings.MainFormColor);
                }
            }
            catch(Exception)
            {

            }

        }

        private void btAddPerson_Click(object sender, EventArgs e) 
        {
            //氏名が未入力なら登録しない
            if (String.IsNullOrWhiteSpace(cbAuther.Text)) {
                MessageBox.Show("氏名が入力されていません");
                return;
            }

            CarReport carReport = new CarReport 
            {
                Date = dtpDate.Value,
                Auther = cbAuther.Text,
                CarName = cbCarName.Text,
                Report = tbReport.Text,
                Picture = pbPicture.Image,
            };
            listCarReports.Add(carReport);

            //EnabledCheck(); //マスク処理呼び出し
            setCbAuther(cbAuther.Text);
            setCbCarName(cbCarName.Text);

            DataRow newRow = infosys202204DataSet.AddressTable.NewRow();
            newRow[1] = dtpDate.Text;
            newRow[2] = cbAuther.Text;
            newRow[3] = cbCarName.Text;
            newRow[4] = tbReport.Text;
            newRow[5] = pbPicture.Text;
            //データセットへ新しいレコードを追加
            infosys202204DataSet.AddressTable.Rows.Add(newRow);
            //データベース更新
            this.Validate();
            this.carReportDBBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.infosys202204DataSet);
        }

        //チェックボックスにセットされている値をリストとして取り出す
        private CarReport.MakerGroup GetRadioButtonMaker() 
        {
            if (rbToyota.Checked) 
            {
                return CarReport.MakerGroup.トヨタ;
            }
            if (rbNissan.Checked) 
            {
                return CarReport.MakerGroup.日産;
            }
            if (rbHonda.Checked) 
            {
                return CarReport.MakerGroup.ホンダ;
            }
            if (rbSubaru.Checked) 
            {
                return CarReport.MakerGroup.スバル;
            }
            if (rbGaisha.Checked) 
            {
                return CarReport.MakerGroup.外国車;
            }

            return CarReport.MakerGroup.その他;

        }

        private void btUpdate_Click(object sender, EventArgs e) 
        {
            carReportDBDataGridView.CurrentRow.Cells[1].Value = GetRadioButtonMaker();
            carReportDBDataGridView.CurrentRow.Cells[2].Value = GetRadioButtonMaker();
            carReportDBDataGridView.CurrentRow.Cells[3].Value = GetRadioButtonMaker();
            carReportDBDataGridView.CurrentRow.Cells[4].Value = GetRadioButtonMaker();
            carReportDBDataGridView.CurrentRow.Cells[5].Value = GetRadioButtonMaker();
        }


        //コンボボックスに記録者を登録する（重複なし）
        private void setCbAuther(string company) 
        {
            if (!cbAuther.Items.Contains(company)) 
            {
                //まだ登録されていなければ登録処理
                cbAuther.Items.Add(company);
            }
        }
        //コンボボックスに車名を登録する（重複なし）
        private void setCbCarName(string company) 
        {
            if (!cbCarName.Items.Contains(company)) 
            {
                //まだ登録されていなければ登録処理
                cbCarName.Items.Add(company);
            }
        }

        private void btPictureOpen_Click(object sender, EventArgs e) 
        {
            if (ofdFileOpenDialog.ShowDialog() == DialogResult.OK) 
            {
                pbPicture.Image = Image.FromFile(ofdFileOpenDialog.FileName);
            }
        }

        private void btPictureClear_Click(object sender, EventArgs e) 
        {
            pbPicture.Image = null;
        }

        private void btOpen_Click(object sender, EventArgs e) 
        {
           
        }

        private void btSaveReport_Click(object sender, EventArgs e) 
        {
            
        }

        private void CarReportDBBindingNavigatorSaveItem_Click(object sender, EventArgs e) {
            this.Validate();
            this.carReportDBBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.infosys202204DataSet);

        }
        //データグリッドビューのクリックイベント
        private void carReportDBDataGridView_Click(object sender, EventArgs e) {

            setMakerRadioSet(carReportDBDataGridView.CurrentRow.Cells[3].Value.ToString());

        }

        private void setMakerRadioSet(string maker) {
            switch (maker) {
                case "トヨタ":
                    rbToyota.Checked = true;
                    break;

                case "日産":
                    rbNissan.Checked = true;
                    break;

                case "ホンダ":
                    rbNissan.Checked = true;
                    break;

                case "スバル":
                    rbNissan.Checked = true;
                    break;

                case "外国車":
                    rbNissan.Checked = true;
                    break;

                case "その他":
                    rbNissan.Checked = true;
                    break;
            }
        }

        private void carReportDBBindingNavigatorSaveItem_Click(object sender, EventArgs e) {
            this.Validate();
            this.carReportDBBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.infosys202204DataSet);
        }

        private void 接続ToolStripMenuItem_Click(object sender, EventArgs e) {
            this.carReportDBTableAdapter.Fill(this.infosys202204DataSet.CarReportDB);


        }
    }
}
