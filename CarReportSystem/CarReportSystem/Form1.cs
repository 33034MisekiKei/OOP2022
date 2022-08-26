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
        //設定情報保存用オブジェクト
        Settings settings = Settings.getInstance();

        //カーレポート管理用リスト
        BindingList<CarReport> listCarReports = new BindingList<CarReport>();

        int mode = 0;
        public Form1() 
        {
            InitializeComponent();
            dgvPersons.DataSource = listCarReports;
        }

        private void btExit_Click(object sender, EventArgs e) 
        {
            //アプリケーションの終了
            Application.Exit();
        }
        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            //色設定ダイアログ
            if (cdColorSelect.ShowDialog() == DialogResult.OK) 
            {
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
            finally 
            {
                EnabledCheck();//マスク処理
            }
        }

        private void btAddPerson_Click(object sender, EventArgs e) 
        {
            if (String.IsNullOrWhiteSpace(cbAuther.Text)) 
            {
                MessageBox.Show("記録者を入力してください");
                return;
            }

            CarReport carReport = new CarReport 
            {
                Date = dtpDate.Value,
                Auther = cbAuther.Text,
                Maker = GetRadioButtonMaker(),
                CarName = cbCarName.Text,
                Report = tbReport.Text,
                Picture = pbPicture.Image,
            };
            listCarReports.Add(carReport);

            //EnabledCheck(); //マスク処理呼び出し
            setCbAuther(cbAuther.Text);
            setCbCarName(cbCarName.Text);
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
            listCarReports[dgvPersons.CurrentRow.Index].Date = dtpDate.Value;
            listCarReports[dgvPersons.CurrentRow.Index].Auther = cbAuther.Text;
            listCarReports[dgvPersons.CurrentRow.Index].Maker = GetRadioButtonMaker();
            listCarReports[dgvPersons.CurrentRow.Index].CarName = cbCarName.Text;
            listCarReports[dgvPersons.CurrentRow.Index].Report = tbReport.Text;
            listCarReports[dgvPersons.CurrentRow.Index].Picture = pbPicture.Image;
            dgvPersons.Refresh();//データグリッドビュー更新
        }
        private void btDeleteReport_Click(object sender, EventArgs e) 
        {
            listCarReports.RemoveAt(dgvPersons.CurrentRow.Index);
            EnabledCheck(); //マスク処理呼び出し
        }

        private void EnabledCheck() {
            btUpdate.Enabled = btDelete.Enabled = listCarReports.Count() > 0 ? true : false;
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
            if (ofdFileOpenDialog.ShowDialog() == DialogResult.OK) 
            {
                try {
                    //バイナリ形式で逆シリアル化
                    var bf = new BinaryFormatter();
                    using (FileStream fs = File.Open(ofdFileOpenDialog.FileName, FileMode.Open, FileAccess.Read)) 
                    {
                        //逆シリアル化して読み込む
                        listCarReports = (BindingList<CarReport>)bf.Deserialize(fs);
                        dgvPersons.DataSource = null;
                        dgvPersons.DataSource = listCarReports;
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
                cbAuther.Items.Clear();//コンボボックスのアイテム消去
                cbCarName.Items.Clear();//コンボボックスのアイテム消去
                //コンボボックスへ登録
                foreach (var item in listCarReports.Select(p => p.Auther)) 
                {
                    setCbCarName(item);//登録
                }
                foreach (var item in listCarReports.Select(p => p.CarName)) 
                {
                    setCbCarName(item);//登録
                }
            }
            EnabledCheck(); //マスク処理呼び出し
        }

        private void btSaveReport_Click(object sender, EventArgs e) 
        {
            if (ofdFileOpenDialog.ShowDialog() == DialogResult.OK) 
            {
                try 
                {
                    //バイナリ形式でシリアル化
                    var bf = new BinaryFormatter();
                    using (FileStream fs = File.Open(ofdFileOpenDialog.FileName, FileMode.Create))
                    {
                        bf.Serialize(fs, listCarReports);
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
