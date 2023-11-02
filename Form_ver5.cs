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
            // Tạo một cột mới kiểu checkbox
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "Co mat"; // Đặt tiêu đề cho cột
            checkBoxColumn.Width = 50; // Đặt chiều rộng cho cột
            checkBoxColumn.Name = "checkBoxColumn"; // Đặt tên cho cột

            // Thêm cột vào DataGridView
            dataGridView1.Columns.Add(checkBoxColumn); // Thêm vào cuối danh sách
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem cột được nhấn có phải là cột checkbox hay không
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                // Kiểm tra xem cột được nhấn có phải là một trong các cột VangCoPhep, VangKhongPhep hay không
                if (dataGridView1.Columns[e.ColumnIndex].Name == "VangCoPhep" || dataGridView1.Columns[e.ColumnIndex].Name == "VangKhongPhep" || dataGridView1.Columns[e.ColumnIndex].Name == "checkBoxColumn")
                {
                    // Duyệt qua các cột VangCoPhep, VangKhongPhep
                    foreach (string colName in new string[] { "VangCoPhep", "VangKhongPhep", "checkBoxColumn" })
                    {
                        if (dataGridView1.Columns.Contains(colName) && colName != dataGridView1.Columns[e.ColumnIndex].Name)
                        {
                            // Đặt giá trị của các ô checkbox khác trong cùng một dòng thành false
                            dataGridView1.Rows[e.RowIndex].Cells[colName].Value = false;
                        }
                    }
                }
            }
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

                    // Cập nhật DataGridView
                    dataGridView1.Refresh();
                }

                conn.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load_gridview();
            dataGridView1.Hide();
            load_cbo();
            txt_malop.Text = "LMH101";
            if (btn_loaddata.Enabled == true)
            {
                themCotCoMat();
            }
            cbo_ngay.SelectedIndexChanged += cbo_ngay_SelectedIndexChanged;
        }

        void load_gridview()
        {
            // Bước 1: Tạo đối tượng SqlConnection để kết nối với CSDL
            SqlConnection connection = new SqlConnection(constr);
            dataGridView1.AllowUserToAddRows = false;

            // Bước 2: Thêm sự kiện SelectedIndexChanged cho ComboBox
            cbo_ngay.SelectedIndexChanged += (s, e) => {
                // Bước 3: Kiểm tra xem có mục nào được chọn trong ComboBox không
                if (cbo_ngay.SelectedItem != null)
                {
                    // Bước 4: Lấy ngày được chọn từ ComboBox
                    string selectedDate = cbo_ngay.SelectedItem.ToString();

                    // Bước 5: Tạo đối tượng SqlDataAdapter
                    SqlDataAdapter adapter = new SqlDataAdapter($"SELECT MaSV, VangCoPhep, VangKhongPhep FROM DIEMDANH WHERE Ngay = '{selectedDate}'", connection);

                    // Bước 6: Tạo DataSet
                    DataSet dsDiemDanh = new DataSet();

                    // Bước 7: Đổ dữ liệu vào DataSet
                    adapter.Fill(dsDiemDanh, "DIEMDANH");

                    // Bước 8: Đổ dữ liệu vào DataGridView
                    dataGridView1.DataSource = dsDiemDanh.Tables["DIEMDANH"];
                }
            };

            // Bước 9: Gán mặc định cho ComboBox là dòng đầu tiên
            if (cbo_ngay.Items.Count > 0)
            {
                cbo_ngay.SelectedIndex = 0;
            }
        }



        private void btn_loaddata_Click(object sender, EventArgs e)
        {
            dataGridView1.Show();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Kết nối với cơ sở dữ liệu
            // Kết nối với cơ sở dữ liệu
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                // Lấy ngày được chọn từ combobox
                string selectedDate = cbo_ngay.SelectedItem.ToString();

                // Tạo câu lệnh SQL để cập nhật dữ liệu
                string query1 = "UPDATE DIEMDANH SET VangCoPhep = @newVangCoPhep1, VangKhongPhep = @newVangKhongPhep1 WHERE (VangCoPhep = 1 OR VangKhongPhep = 1) and MaLopMH= 'LMH101' AND Ngay= @selectedDate and MaSV= 103";
                string query2 = "UPDATE DIEMDANH SET VangCoPhep = @newVangCoPhep2, VangKhongPhep = @newVangKhongPhep2 WHERE (VangCoPhep = 0 OR VangKhongPhep = 0) and MaLopMH= 'LMH101' AND Ngay= @selectedDate and MaSV= 103";

                using (SqlCommand cmd = new SqlCommand(query1, conn))
                {
                    // Thêm tham số cho câu lệnh SQL
                    cmd.Parameters.AddWithValue("@newVangCoPhep1", 1);
                    cmd.Parameters.AddWithValue("@newVangKhongPhep1", 1);
                    cmd.Parameters.AddWithValue("@selectedDate", selectedDate);

                    // Thực thi câu lệnh SQL
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand(query2, conn))
                {
                    // Thêm tham số cho câu lệnh SQL
                    cmd.Parameters.AddWithValue("@newVangCoPhep2", 0);
                    cmd.Parameters.AddWithValue("@newVangKhongPhep2", 0);
                    cmd.Parameters.AddWithValue("@selectedDate", selectedDate);

                    // Thực thi câu lệnh SQL
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
