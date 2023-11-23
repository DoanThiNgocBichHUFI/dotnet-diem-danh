using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Net.NetworkInformation;
using Microsoft.VisualBasic;

namespace QLSV
{
    public partial class Form6 : Form
    {
        SqlConnection con;
        public string MaLopMH { get; set; }
        string constr = "Data Source=DESKTOP-0MNS8CK\\SQLEXPRESS;Initial Catalog=qldiemdanh4ll11;Integrated Security=True";
        public Form6()
        {
            con = new SqlConnection("Data Source=DESKTOP-0MNS8CK\\SQLEXPRESS;Initial Catalog=qldiemdanh4ll11;Integrated Security=True");
            InitializeComponent();
        }

        public class Student
        {
            public string MaSV { get; set; }
            public string GhiChu { get; set; }
            
        }

        void load_cbo()
        {
            // Bước 1: Tạo đối tượng SqlConnection để kết nối với CSDL
            SqlConnection connection = new SqlConnection(constr);

            // Bước 2: Mở kết nối
            connection.Open();

            // Bước 3: Tạo đối tượng SqlCommand

            SqlCommand command = new SqlCommand("SELECT DISTINCT Ngay FROM DIEMDANH where MaLopMH= @MaLopMH", connection);
            command.Parameters.AddWithValue("@MaLopMH", MaLopMH);

            // Bước 4: Thực thi câu lệnh và lấy dữ liệu
            SqlDataReader reader = command.ExecuteReader();

            // Bước 5: Đọc và thêm dữ liệu vào ComboBox
            while (reader.Read())
            {
                //cbo_ngay.Items.Add(reader.GetDateTime(0).ToString("yyyy-MM-dd"));
                cbo_ngay.Items.Add(reader.GetDateTime(0).ToString("yyyy-MM-dd"));
            }
        }
        private void Form6_Load(object sender, EventArgs e)
        {
            dataGridView1.Hide();
            load_cbo();
            txt_malop.Text = MaLopMH;
            load_txt_diachi();
            load_txt_tenmh();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }


        private void cbo_ngay_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy ngày được chọn từ ComboBox
            string selectedDate = cbo_ngay.SelectedItem.ToString();

            // Kết nối với cơ sở dữ liệu
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                // Tạo câu lệnh SQL để tải lại dữ liệu
                string loadQuery = "SELECT DISTINCT MaSV, VangCoPhep, VangKhongPhep, GhiChu FROM DIEMDANH where Ngay=@selectedDate and MaLopMH = @MaLopMH";


                using (SqlCommand cmd = new SqlCommand(loadQuery, conn))
                {
                    // Thêm tham số cho câu lệnh SQL
                    cmd.Parameters.AddWithValue("@selectedDate", selectedDate);
                    cmd.Parameters.AddWithValue("@MaLopMH", MaLopMH);

                    // Tạo một SqlDataAdapter để tải dữ liệu
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    // Tạo một DataTable để chứa dữ liệu
                    DataTable dt = new DataTable();

                    // Đổ dữ liệu vào DataTable
                    da.Fill(dt);

                    // Đặt DataTable làm nguồn dữ liệu cho DataGridView
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = dt;

                    // Cập nhật DataGridView
                    dataGridView1.Refresh();
                }

                conn.Close();
            }

        }

        void load_txt_tenmh()
        {
            // Câu truy vấn SQL
            string query = "select TenMH, mh.MaMH from dbo.LOPMONHOC lopmh join dbo.MONHOC mh on lopmh.MaMH= mh.MaMH where MaLopMH= @MaLopMH ";

            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaLopMH", MaLopMH);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // Lấy giá trị từ cột "ColumnName"
                                string value = reader.GetString(reader.GetOrdinal("TenMH"));

                                // Hiển thị kết quả trên TextBox
                                txt_tenmh.Text = value;
                            }
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        void load_txt_diachi()
        {
            // Câu truy vấn SQL
            string query = "select distinct DiaChi from PHONGHOC p , SUDUNGPHONGHOC sd, MONHOC mh where p.MaPhongHoc= sd.MaPhongHoc and MaLopMH= @MaLopMH ";
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaLopMH", MaLopMH);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // Lấy giá trị từ cột "ColumnName"
                                string value = reader.GetString(reader.GetOrdinal("DiaChi"));

                                // Hiển thị kết quả trên TextBox
                                txt_diachi.Text = value;
                            }
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void btn_luu_Click(object sender, EventArgs e)
        {
            // Tạo kết nối đến cơ sở dữ liệu.
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                // Lấy ngày được chọn từ ComboBox
                string selectedDate = cbo_ngay.SelectedItem.ToString();

                // Duyệt qua từng dòng trong DataGridView.
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        // Lấy giá trị từ các ô.
                        string maSV = row.Cells["MaSV"].Value.ToString();
                        string notes = row.Cells["GhiChu"].Value.ToString();

                        // Tạo câu lệnh SQL để cập nhật dữ liệu
                        string query1 = "UPDATE DIEMDANH SET VangCoPhep = @newVangCoPhep1, VangKhongPhep = @newVangKhongPhep1 WHERE VangCoPhep = 1 OR VangKhongPhep = 1 AND MaSV = @MaSV AND Ngay= @selectedDate AND MaLopMH= @MaLopMH";
                        string query2 = "UPDATE DIEMDANH SET VangCoPhep = @newVangCoPhep2, VangKhongPhep = @newVangKhongPhep2 WHERE VangCoPhep = 0 OR VangKhongPhep = 0 AND MaSV = @MaSV AND Ngay= @selectedDate AND MaLopMH= @MaLopMH";

                        using (SqlCommand cmd = new SqlCommand(query1, connection))
                        {
                            // Thêm tham số cho câu lệnh SQL
                            cmd.Parameters.AddWithValue("@newVangCoPhep1", 1);
                            cmd.Parameters.AddWithValue("@newVangKhongPhep1", 1);
                            cmd.Parameters.AddWithValue("@MaSV", maSV);
                            cmd.Parameters.AddWithValue("@selectedDate", selectedDate);
                            cmd.Parameters.AddWithValue("@MaLopMH", txt_malop.Text);

                            // Thực thi câu lệnh SQL
                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand(query2, connection))
                        {
                            // Thêm tham số cho câu lệnh SQL
                            cmd.Parameters.AddWithValue("@newVangCoPhep2", 0);
                            cmd.Parameters.AddWithValue("@newVangKhongPhep2", 0);
                            cmd.Parameters.AddWithValue("@MaSV", maSV);
                            cmd.Parameters.AddWithValue("@selectedDate", selectedDate);
                            cmd.Parameters.AddWithValue("@MaLopMH", txt_malop.Text);

                            // Thực thi câu lệnh SQL
                            cmd.ExecuteNonQuery();
                        }

                        // Tạo câu lệnh SQL để cập nhật dữ liệu.
                        string sql = "UPDATE DIEMDANH SET GhiChu = @Notes WHERE MaSV = @MaSV AND Ngay= @selectedDate AND MaLopMH= @MaLopMH";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@MaSV", maSV);
                            command.Parameters.AddWithValue("@Notes", notes);
                            command.Parameters.AddWithValue("@selectedDate", selectedDate);
                            command.Parameters.AddWithValue("@MaLopMH", txt_malop.Text);
                            // Thực hiện câu lệnh SQL.
                            command.ExecuteNonQuery();
                        }
                    }

                }

                MessageBox.Show("Đã lưu thay đổi vào cơ sở dữ liệu.");
            }
            
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        
        private void btn_loaddata_Click(object sender, EventArgs e)
        {
            //// Kiểm tra xem người dùng đã chọn một ngày trong cbo_ngay chưa
            //if (cbo_ngay.SelectedItem == null || cbo_ngay.SelectedItem.ToString() == "")
            //{
            //    // Nếu chưa, hiển thị thông báo và thoát khỏi sự kiện
            //    MessageBox.Show("Vui lòng chọn ngày.");
            //    dataGridView1.Hide();
            //    return;
            //}
            //else
            //{
            //    dataGridView1.Show();

            //    using (SqlConnection connection = new SqlConnection(constr))
            //    {
            //        connection.Open();

            //        string query = "SELECT distinct MaSV, VangCoPhep, VangKhongPhep,GhiChu FROM DIEMDANH WHERE MaLopMH = @MaLopMH AND Ngay= @selectedDate";
            //        string selectedDate = cbo_ngay.SelectedItem?.ToString();
            //        using (SqlCommand command = new SqlCommand(query, connection))
            //        {
            //            command.Parameters.AddWithValue("@MaLopMH", txt_malop.Text);
            //            command.Parameters.AddWithValue("@selectedDate", selectedDate);
            //            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            //            {
            //                DataTable dt = new DataTable();
            //                adapter.Fill(dt);
            //                dataGridView1.DataSource = dt;
            //                dataGridView1.AllowUserToAddRows = false;

            //                // Tìm cột "GhiChu" và thiết lập thuộc tính ReadOnly thành false.
            //                if (dataGridView1.Columns.Contains("GhiChu"))
            //                {
            //                    dataGridView1.Columns["GhiChu"].ReadOnly = false;
            //                }
            //            }
            //        }

            //        connection.Close();
            //    }
            //}
        }


        private void txt_tenmh_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem người dùng có thay đổi một ô hợp lệ không
            if (e.RowIndex >= 0)
            {
                // Kiểm tra xem cột có phải là "VangCoPhep" hoặc "VangKhongPhep" không.
                if (dataGridView1.Columns[e.ColumnIndex].Name == "VangCoPhep" || dataGridView1.Columns[e.ColumnIndex].Name == "VangKhongPhep")
                {
                    // Lấy giá trị mới từ ô đã thay đổi.
                    var newValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                    // Hiển thị thông báo cho người dùng.
                    this.BeginInvoke(new Action(() => MessageBox.Show($"Giá trị của cột {dataGridView1.Columns[e.ColumnIndex].Name} đã được thay đổi thành: {newValue}")));
                }
            }
        }

        private void txt_tenGV_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
