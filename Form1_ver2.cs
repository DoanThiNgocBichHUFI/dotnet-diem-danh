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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem cột được nhấn có phải là cột checkbox hay không
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                // Duyệt qua tất cả các cột checkbox
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i] is DataGridViewCheckBoxColumn && i != e.ColumnIndex)
                    {
                        // Đặt giá trị của các ô checkbox khác trong cùng một dòng thành false
                        dataGridView1.Rows[e.RowIndex].Cells[i].Value = false;
                    }
                }
            }
        }

        private void cbo_ngay_SelectedIndexChanged(object sender, EventArgs e) 
        { 
        
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            load_gridview();
            ///dataGridView1.Hide();
            load_cbo();
            txt_malop.Text = "LMH101";
            if (btn_loaddata.Enabled)
            {
                themCotCoMat();
            }

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

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["checkBoxColumn"].Value = true; // Thay "checkBoxColumn" bằng tên cột checkbox của bạn
            }

        }

        //void load_gridview()
        //{
        //    // Bước 1: Tạo đối tượng SqlConnection để kết nối với CSDL
        //    SqlConnection connection = new SqlConnection(constr);

        //    // Bước 2: Tạo đối tượng SqlDataAdapter
        //    SqlDataAdapter adapter = new SqlDataAdapter("SELECT MaSV, VangCoPhep, VangKhongPhep FROM DIEMDANH", connection);

        //    // Bước 3: Tạo DataSet
        //    DataSet dsDiemDanh = new DataSet();

        //    // Bước 4: Đổ dữ liệu vào DataSet
        //    adapter.Fill(dsDiemDanh, "DIEMDANH");

        //    // Bước 5: Đổ dữ liệu vào DataGridView
        //    dataGridView1.DataSource = dsDiemDanh.Tables["DIEMDANH"];

        //}
        void load_gridview()
        {
            // Bước 1: Tạo đối tượng SqlConnection để kết nối với CSDL
            SqlConnection connection = new SqlConnection(constr);

            // Bước 2: Thêm sự kiện SelectedIndexChanged cho ComboBox
            cbo_ngay.SelectedIndexChanged += (s, e) => {
                // Bước 3: Lấy ngày được chọn từ ComboBox
                string selectedDate = cbo_ngay.SelectedItem.ToString();

                // Bước 4: Chuyển đổi chuỗi thành DateTime
                DateTime dateValue;
                if (DateTime.TryParseExact(selectedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                {
                    // Bước 5: Tạo đối tượng SqlDataAdapter
                    SqlDataAdapter adapter = new SqlDataAdapter($"SELECT MaSV, VangCoPhep, VangKhongPhep FROM DIEMDANH WHERE Ngay = '{dateValue.ToString("yyyy-MM-dd")}'", connection);

                    // Bước 6: Tạo DataSet
                    DataSet dsDiemDanh = new DataSet();

                    // Bước 7: Đổ dữ liệu vào DataSet
                    adapter.Fill(dsDiemDanh, "DIEMDANH");

                    // Bước 8: Đổ dữ liệu vào DataGridView
                    cbo_ngay.DataSource = dsDiemDanh.Tables["DIEMDANH"];
                }
                else
                {
                    // Xử lý lỗi nếu chuỗi không phải là ngày và thời gian hợp lệ
                    MessageBox.Show("Ngày không hợp lệ: " + selectedDate);
                }
            };



        }
        private void btn_loaddata_Click(object sender, EventArgs e)
        {
            

        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            // Kết nối với cơ sở dữ liệu
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                // Tạo câu lệnh SQL để cập nhật dữ liệu
                string query = "UPDATE DIEMDANH SET VangCoPhep = @newVangCoPhep, VangKhongPhep = @newVangKhongPhep WHERE VangCoPhep = 1 OR VangKhongPhep = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Thêm tham số cho câu lệnh SQL
                    cmd.Parameters.AddWithValue("@newVangCoPhep", 1);
                    cmd.Parameters.AddWithValue("@newVangKhongPhep", 1);

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
