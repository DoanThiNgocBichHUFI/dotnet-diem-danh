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

        private void btn_loaddata_Click(object sender, EventArgs e)
        {

            // Kiểm tra xem người dùng đã chọn một ngày trong cbo_ngay chưa
            if (cbo_ngay.SelectedItem == null || cbo_ngay.SelectedItem.ToString() == "")
            {
                // Nếu chưa, hiển thị thông báo và thoát khỏi sự kiện
                MessageBox.Show("Vui lòng chọn ngày.");
                dataGridView1.Hide();
                return;
            }
            else
            {
                dataGridView1.Show();

                string constr = @"Data Source=DESKTOP-0MNS8CK\SQLEXPRESS;Initial Catalog=qldiemdanh4ll11;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string query = "SELECT distinct MaSV, VangCoPhep, VangKhongPhep FROM DIEMDANH WHERE MaLopMH = @MaLopMH";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaLopMH", txt_malop.Text); // Giả sử textBox1 chứa giá trị bạn muốn truy vấn

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dataGridView1.DataSource = dt; // Giả sử dataGridView1 là DataGridView của bạn
                            dataGridView1.AllowUserToAddRows = false; // Thêm dòng này để loại bỏ dòng trống dư

                        }
                    }

                    connection.Close();
                }

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
                string loadQuery = "SELECT DISTINCT MaSV, VangCoPhep, VangKhongPhep FROM DIEMDANH where Ngay=@selectedDate and MaLopMH = @MaLopMH";

                
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
    }
}
