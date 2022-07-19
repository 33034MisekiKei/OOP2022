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
        Settings settings = new Settings();

        //データ管理用リスト
        BindingList<CarReport> listCarReport = new BindingList<CarReport>();

        int mode = 0;
        public Form1() 
        {
            InitializeComponent();
            dgvPersons.DataSource = listCarReport;
        }

        private void btPictureOpen_Click(object sender, EventArgs e) 
        {
            if (ofdFileOpenDialog.ShowDialog() == DialogResult.OK) 
            {
                pbPicture.Image = Image.FromFile(ofdFileOpenDialog.FileName);
            }
        }

        private void btAddPerson_Click(object sender, EventArgs e) 
        {
            if (String.IsNullOrWhiteSpace(cbAuther.Text)) 
            {
                MessageBox.Show("記録者を入力してください");
                return;
            }
            CarReport newPerson = new CarReport 
            {
                Date = dtpDate.Value,
                Auther = cbAuther.Text,
                CarName = cbCarName.Text,
                Report = tbReport.Text,
                Picture = pbPicture.Image,
            };
            listCarReport.Add(newPerson);
            dgvPersons.Rows[dgvPersons.RowCount - 1].Selected = true;

            EnabledCheck(); //マスク処理呼び出し

            setCbCarName(cbCarName.Text);
        }

        private void ファイルToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            //色設定ダイアログ
            if (cdColorSelect.ShowDialog() == DialogResult.OK) {
                BackColor = cdColorSelect.Color;
                settings.MainFormColor = cdColorSelect.Color;
            }
        }
    
    private void pbPicture_Click(object sender, EventArgs e) 
        {
            pbPicture.SizeMode = (PictureBoxSizeMode)mode;
            mode = mode < 4 ? ++mode : 0;
        }

        private void Form1_FormClased(object sender, FormClosedEventArgs e) 
        {
            //設定ファイルをシリアル化
            using (var writr = XmlWriter.Create("settings.xml")) {
                var serializer = new XmlSerializer(settings.GetType());
              //serializer.Serialize(writr.Settings);
            }
            
        }

        private void Form1_Load_1(object sender, EventArgs e) 
        {
            //設定ファイルを逆シリアル化して背景の色に設定
            using (var reader = XmlReader.Create("settings.xml")) 
            {
                var serializer = new XmlSerializer(typeof(Settings));
                settings = serializer.Deserialize(reader) as Settings;
            }
            EnabledCheck();
        }

        //チェックボックスにセットされている値をリストとして取り出す
        private List<CarReport.MakerGroup> GetCheckBoxGroup() 
        {
            var listGroup = new List<CarReport.MakerGroup>();
            if (rbToyota.Checked) 
            {
                listGroup.Add(CarReport.MakerGroup.トヨタ);
            }
            if (rbNissan.Checked) 
            {
                listGroup.Add(CarReport.MakerGroup.日産);
            }
            if (rbHonda.Checked) 
            {
                listGroup.Add(CarReport.MakerGroup.ホンダ);
            }
            if (rbSubaru.Checked) 
            {
                listGroup.Add(CarReport.MakerGroup.スバル);
            }
            if (rbGaisha.Checked) 
            {
                listGroup.Add(CarReport.MakerGroup.外国車);
            }
            if (rbOther.Checked) 
            {
                listGroup.Add(CarReport.MakerGroup.その他);
            }
            return listGroup;
        }

        private void setCbCarName(string carname) 
        {
            if (!cbCarName.Items.Contains(carname)) 
            {
                //まだ登録されていなければ登録処理
                cbCarName.Items.Add(carname);
            }
        }
        private void btPictureClear_Click(object sender, EventArgs e) 
        {
            pbPicture.Image = null;
        }

        //データグリッドビューをクリックした時のイベントハンドラ
        private void dgvPersons_Click(object sender, EventArgs e) 
        {
            if (dgvPersons.CurrentRow == null) return;

            int index = dgvPersons.CurrentRow.Index;
            //データグリッドビューのインデックス０番の名前をテキストボックスに収納
            dtpDate.Value = listCarReport[index].Date;
            cbAuther.Text = listCarReport[index].Auther;
            cbCarName.Text = listCarReport[index].CarName;
            tbReport.Text = listCarReport[index].Report;
            pbPicture.Image = listCarReport[index].Picture;

        }
        //グループのチェックボックスをオールクリア
        private void groupCheckBoxAllClear() 
        {
            rbHonda.Checked = rbNissan.Checked = rbOther.Checked 
                = rbSubaru.Checked = rbToyota.Checked = rbGaisha.Checked = false;
        }
        //更新ボタンが押された時の処理
        private void btUpdate_Click(object sender, EventArgs e) 
        {
            listCarReport[dgvPersons.CurrentRow.Index].Date = dtpDate.Value;
            listCarReport[dgvPersons.CurrentRow.Index].Auther = cbAuther.Text;
            listCarReport[dgvPersons.CurrentRow.Index].CarName = cbCarName.Text;
            listCarReport[dgvPersons.CurrentRow.Index].Report = tbReport.Text;
            listCarReport[dgvPersons.CurrentRow.Index].Picture = pbPicture.Image;
            dgvPersons.Refresh();//データグリッドビュー更新
        }
        //更新・削除ボタンが押された時の処理
        private void btDeletion_Click(object sender, EventArgs e) 
        {
            listCarReport.RemoveAt(dgvPersons.CurrentRow.Index);
            EnabledCheck(); //マスク処理呼び出し
        }

        //更新・削除ボタンのマスク処理行う（マスク判定含む）
        private void EnabledCheck() 
        {
            btUpdate.Enabled = btDelete.Enabled = listCarReport.Count() > 0 ? true : false;
        }

        private void Form1_Load(object sender, EventArgs e) 
        {
            EnabledCheck(); //マスク処理呼び出し
            //背景色
            //dgvPersons.RowsDefaultCellStyle.BackColor = Color.Bisque;
            //奇数行
            dgvPersons.AlternatingRowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
        }

        //保存ボタンのイベントハンドラ
        private void btSave_Click(object sender, EventArgs e) 
        {
            if (sfdSaveDialog.ShowDialog() == DialogResult.OK) 
            {
                try {
                    //バイナリー形式でシリアル化
                    var bf = new BinaryFormatter();
                    using (FileStream fs = File.Open(sfdSaveDialog.FileName, FileMode.Create)) 
                    {
                        bf.Serialize(fs, listCarReport);
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
                        listCarReport = (BindingList<CarReport>)bf.Deserialize(fs);
                        dgvPersons.DataSource = null;
                        dgvPersons.DataSource = listCarReport;
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
                cbAuther.Items.Clear();//コンボボックスのアイテム消去
                cbCarName.Items.Clear();//コンボボックスのアイテム消去
                //コンボボックスへ登録
                foreach (var item in listCarReport.Select(p => p.Auther)) 
                {
                    setCbCarName(item);//登録
                }
                foreach (var item in listCarReport.Select(p => p.CarName)) 
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
                
            }
        }

        
    }
}

