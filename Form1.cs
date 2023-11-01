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
            DataSet ds = new DataSet();
            string strselect = "select distinct Ngay from DIEMDANH where MaLopMH = 'LMH101'";
            SqlDataAdapter da = new SqlDataAdapter(strselect, cn);
            da.Fill(ds, "DIEMDANH");

            // Tạo một DataView từ DataTable
            DataView dv = ds.Tables[0].DefaultView;

            // Tạo một bảng mới với cùng cấu trúc như bảng gốc
            DataTable dtFormatted = dv.ToTable();

            // Chuyển đổi định dạng ngày trong bảng mới
            foreach (DataRow row in dtFormatted.Rows)
            {
                DateTime date = DateTime.Parse(row["Ngay"].ToString());
                row["Ngay"] = date.ToString("yyyy-MM-dd");
            }

            // Thêm lựa chọn null vào đầu DataTable
            DataRow newRow = dtFormatted.NewRow();
            newRow["Ngay"] = DBNull.Value;
            dtFormatted.Rows.InsertAt(newRow, 0);

            cbo_ngay.DataSource = dtFormatted;
            cbo_ngay.DisplayMember = "Ngay";
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
