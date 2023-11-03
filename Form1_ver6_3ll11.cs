using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization;
using System.Windows.Forms;

namespace Diem_Danh_SV
{
    public partial class Form1 : Form
    {
        SqlConnection cn;
       
         string constr = "Data Source=DESKTOP-0MNS8CK\\SQLEXPRESS;Initial Catalog=qlddanh28ll10;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
            cn = new SqlConnection(@"Data Source=DESKTOP-0MNS8CK\SQLEXPRESS;Initial Catalog=qlddanh28ll10;Integrated Security=True");


        }

        void load_cbo()
        {
            // Bước 1: Tạo đối tượng SqlConnection để kết nối với CSDL
            SqlConnection connection = new SqlConnection(constr);

            // Bước 2: Mở kết nối
            connection.Open();
            
            // Bước 3: Tạo đối tượng SqlCommand
            SqlCommand command = new SqlCommand("SELECT DISTINCT Ngay FROM DIEMDANH where MaLopMH= 'LMH101'", connection);

            // Bước 4: Thực thi câu lệnh và lấy dữ liệu
            SqlDataReader reader = command.ExecuteReader();

            // Bước 5: Đọc và thêm dữ liệu vào ComboBox
            while (reader.Read())
            {
                //cbo_ngay.Items.Add(reader.GetDateTime(0).ToString("yyyy-MM-dd"));
                cbo_ngay.Items.Add(reader.GetDateTime(0).ToString("yyyy-MM-dd"));
            }
            // Bước 6: Đóng kết nối
            connection.Close();
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }
        void themCotCoMat()
        {
            if (!dataGridView1.Columns.Contains("checkBoxColumn"))
            {
                // Tạo một cột mới kiểu checkbox
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                checkBoxColumn.HeaderText = "Co mat"; // Đặt tiêu đề cho cột
                checkBoxColumn.Width = 70; // Đặt chiều rộng cho cột
                checkBoxColumn.Name = "checkBoxColumn"; // Đặt tên cho cột
                checkBoxColumn.ThreeState = false; // Đảm bảo rằng checkbox chỉ có hai trạng thái
                checkBoxColumn.IndeterminateValue = false; // Đặt giá trị không xác định thành false
                checkBoxColumn.FalseValue = false; // Đặt giá trị false thành false
                checkBoxColumn.TrueValue = true; // Đặt giá trị true thành true
                checkBoxColumn.CellTemplate.Value = true; // Đặt giá trị mặc định cho tất cả các ô trong cột là true

                // Thêm cột vào DataGridView
                dataGridView1.Columns.Add(checkBoxColumn); // Thêm vào cuối danh sách

                // Đặt giá trị của tất cả các ô trong cột checkbox là true

            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["checkBoxColumn"].Value = true; // Thay "checkBoxColumn" bằng tên cột checkbox của bạn
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }


    // Thêm sự kiện SelectedIndexChanged vào ComboBox
    private void cbo_ngay_SelectedIndexChanged(object sender, EventArgs e) 
       {
            // Lấy ngày được chọn từ ComboBox
            string selectedDate = cbo_ngay.SelectedItem.ToString();

            // Kết nối với cơ sở dữ liệu
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                // Tạo câu lệnh SQL để tải lại dữ liệu
                string loadQuery = "SELECT DISTINCT MaSV, VangCoPhep, VangKhongPhep FROM DIEMDANH where Ngay=@selectedDate and MaLopMH = 'LMH101' ";

                using (SqlCommand cmd = new SqlCommand(loadQuery, conn))
                {
                    // Thêm tham số cho câu lệnh SQL
                    cmd.Parameters.AddWithValue("@selectedDate", selectedDate);

                    // Tạo một SqlDataAdapter để tải dữ liệu
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    // Tạo một DataTable để chứa dữ liệu
                    DataTable dt = new DataTable();

                    // Đổ dữ liệu vào DataTable
                    da.Fill(dt);

                    // Đặt DataTable làm nguồn dữ liệu cho DataGridView
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = dt;
                    dataGridView1.AllowUserToAddRows = false;

                    // Cập nhật DataGridView
                    dataGridView1.Refresh();
                }

                conn.Close();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            load_gridview();
            // dataGridView1.Hide();
            load_cbo();
            txt_maLop.Text = "LMH101";
            load_txt_tenmh();
            load_txt_diachi();
            cbo_ngay.SelectedIndexChanged += cbo_ngay_SelectedIndexChanged;
        }
        void load_gridview()
        {
            //dataGridView1.AllowUserToAddRows = false;

            //cbo_ngay.SelectedIndexChanged += (s, e) => {
            //    if (cbo_ngay.SelectedItem != null)
            //    {
            //        string selectedDate = cbo_ngay.SelectedItem.ToString();

            //        // Chuyển đổi selectedDate thành DateTime
            //        DateTime date;
            //        if (DateTime.TryParse(selectedDate, out date))
            //        {
            //            using (SqlConnection connection = new SqlConnection(constr))
            //            {
            //                connection.Open();

            //                // Sử dụng tham số để tránh SQL Injection
            //                string query = "SELECT MaSV, VangCoPhep, VangKhongPhep FROM DIEMDANH WHERE Ngay = @Ngay";
            //                using (SqlCommand command = new SqlCommand(query, connection))
            //                {
            //                    command.Parameters.AddWithValue("@Ngay", date);

            //                    SqlDataAdapter adapter = new SqlDataAdapter(command);
            //                    DataSet dsDiemDanh = new DataSet();
            //                    adapter.Fill(dsDiemDanh, "DIEMDANH");

            //                    dataGridView1.DataSource = dsDiemDanh.Tables["DIEMDANH"];

            //                }
            //            }
            //        }
            //        else
            //        {
            //            // Xử lý lỗi nếu không thể chuyển đổi ngày
            //            MessageBox.Show("Không thể chuyển đổi ngày đã chọn. Vui lòng kiểm tra lại định dạng ngày.");
            //        }
            //    }
            //};

            //if (cbo_ngay.Items.Count > 0)
            //{
            //    cbo_ngay.SelectedIndex = 0;
            //}
        }

        private void btn_loaddata_Click(object sender, EventArgs e)
        {
            if (cbo_ngay.SelectedItem == null || cbo_ngay.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Vui lòng chọn ngày.");
                dataGridView1.Hide();
                return;
            }
            else
            {
                dataGridView1.Show();

                string constr = "Data Source=DESKTOP-0MNS8CK\\SQLEXPRESS;Initial Catalog=qlddanh28ll10;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string query = "SELECT distinct MaSV, VangCoPhep, VangKhongPhep FROM DIEMDANH WHERE MaLopMH = @MaLopMH";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaLopMH", txt_maLop.Text);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dataGridView1.DataSource = dt;
                            dataGridView1.AllowUserToAddRows = false;
                        }
                    }
                    connection.Close();
                }

                if (!dataGridView1.Columns.Contains("checkBoxColumn"))
                {
                    DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                    checkBoxColumn.HeaderText = "Co mat";
                    checkBoxColumn.Width = 70;
                    checkBoxColumn.Name = "checkBoxColumn";
                    checkBoxColumn.ThreeState = false;
                    checkBoxColumn.IndeterminateValue = false;
                    checkBoxColumn.FalseValue = false;
                    checkBoxColumn.TrueValue = true;
                    checkBoxColumn.CellTemplate.Value = true;

                    dataGridView1.Columns.Add(checkBoxColumn);
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells["checkBoxColumn"].Value = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kết nối với cơ sở dữ liệu
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                // Lấy ngày được chọn từ combobox
                string selectedDate = cbo_ngay.SelectedItem.ToString();

                // Tạo câu lệnh SQL để cập nhật dữ liệu
                string query1 = "UPDATE DIEMDANH SET VangCoPhep = @newVangCoPhep1, VangKhongPhep = @newVangKhongPhep1 WHERE (VangCoPhep = 1 OR VangKhongPhep = 1 or VangCoPhep = 0 OR VangKhongPhep = 0) and MaLopMH= 'LMH101' AND Ngay= @selectedDate and MaSV= 103";
                using (SqlCommand cmd = new SqlCommand(query1, conn))
                {
                    // Thêm tham số cho câu lệnh SQL
                    cmd.Parameters.AddWithValue("@newVangCoPhep1", 1);
                    cmd.Parameters.AddWithValue("@newVangKhongPhep1", 1);
                    cmd.Parameters.AddWithValue("@newVangCoPhep1", 0);
                    cmd.Parameters.AddWithValue("@newVangKhongPhep1", 0);
                    cmd.Parameters.AddWithValue("@selectedDate", selectedDate);
                    // Thực thi câu lệnh SQL
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e){ }
        void load_txt_tenmh()
        {
            // Câu truy vấn SQL
            string query = "select TenMH, mh.MaMH from dbo.LOPMONHOC lopmh join dbo.MONHOC mh on lopmh.MaMH= mh.MaMH where MaLopMH= 'LMH101' ";

            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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
            string query = "select distinct DiaChi from PHONGHOC p , SUDUNGPHONGHOC sd, MONHOC mh where p.MaPhongHoc= sd.MaPhongHoc and MaLopMH= 'LMH101' ";
            try
            {
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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


    }
}
