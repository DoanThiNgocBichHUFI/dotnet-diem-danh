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
        SqlDataAdapter da_diemdanh;
        DataSet ds_diemdanh;
        DataColumn[] key = new DataColumn[1];

        public Form1()
        {
            InitializeComponent();
            cn = new SqlConnection(@"Data Source=DESKTOP-0MNS8CK\SQLEXPRESS;Initial Catalog=qlddanh28ll10;Integrated Security=True");
            
            load_cbo(); // Gọi hàm load_cbo() ở đây để khởi tạo cbo_ngay

            // Chuyển đổi cbo_ngay.Text thành DateTime
            DateTime ngay;
            if (DateTime.TryParse(cbo_ngay.Text, out ngay))
            {
                string strSelect = "SELECT distinct MaSV, VangCoPhep, VangKhongPhep FROM DIEMDANH where Ngay= '" + ngay.ToString("yyyy-MM-dd") + "'";

                da_diemdanh = new SqlDataAdapter(strSelect, cn);
                ds_diemdanh = new DataSet();
                da_diemdanh.Fill(ds_diemdanh, "DIEMDANH");

                // Tạo một mảng DataColumn
                DataColumn[] key = new DataColumn[3];

                // Thêm các cột vào mảng
                key[0] = ds_diemdanh.Tables["DIEMDANH"].Columns["MaSV"];
                //key[1] = ds_diemdanh.Tables["DIEMDANH"].Columns["MaLopMH"];
                //key[2] = ds_diemdanh.Tables["DIEMDANH"].Columns["Ngay"];

                // Đặt khóa chính cho bảng DIEMDANH
                ds_diemdanh.Tables["DIEMDANH"].PrimaryKey = key;
            }
            else
            {
                // Xử lý lỗi nếu cbo_ngay.Text không phải là một chuỗi ngày hợp lệ
            }
        }

        //void Databingding(DataTable pDT)
        //{
        //    txt_malop.DataBindings.Clear();
        //    cbo_ngay.DataBindings.Clear();
        //    //lien ket du lieu tren textbox voi truong du lieu tuong ung trong dataTable
        //    txt_malop.DataBindings.Add("Text", pDT, "MaLopMH");

        //}

        void DataBinding(DataTable ds)
        {
            try
            {
                // Kiểm tra xem ct_map có phải là null không
                if (txt_malop != null)
                {
                    txt_malop.DataBindings.Clear();
                    txt_malop.DataBindings.Add("text", ds, "MaLopMH");
                    //Lieu là tên cột trong bảng ds
                }
                else
                {
                    MessageBox.Show("loi khi load data ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void load_grid()
        {
            // Hoặc bạn có thể đặt mục được chọn bằng cách sử dụng SelectedItem
            // Điều này sẽ chọn mục có giá trị là "Item 3"
            cbo_ngay.SelectedItem = "03-01-2023";

            // Kiểm tra xem có mục nào được chọn trong ComboBox không
            if (cbo_ngay.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một ngày.");
                return;
            }
            

            string selectedDate = cbo_ngay.SelectedValue.ToString(); // Giả sử comboBox1 chứa các giá trị ngày

            DateTime date;
            try
            {
                if (!DateTime.TryParseExact(selectedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    MessageBox.Show("Ngày không hợp lệ. Vui lòng nhập ngày theo định dạng yyyy-MM-dd");
                    return;
                }
                string constr = @"Data Source=DESKTOP-0MNS8CK\SQLEXPRESS;Initial Catalog=qlddanh28ll10;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string query = "SELECT MaSV, VangCoPhep, VangKhongPhep FROM DIEMDANH WHERE Ngay = @Ngay";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Ngay", date); // Sử dụng date thay vì selectedDate

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dataGridView1.DataSource = dt; // Giả sử dataGridView1 là DataGridView của bạn
                        }
                    }

                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //void load_cbo()
        //{
        //    DataSet ds = new DataSet();
        //    string strselect = "select distinct Ngay from DIEMDANH";
        //    SqlDataAdapter da = new SqlDataAdapter(strselect, cn);
        //    da.Fill(ds, "DIEMDANH");
        //    cbo_ngay.DataSource = ds.Tables[0];
        //    cbo_ngay.DisplayMember = "Ngay";
        //    //cbo_ngay.ValueMember = "MaSV";
        //}

        void load_cbo()
        {
            string constr = @"Data Source=DESKTOP-0MNS8CK\SQLEXPRESS;Initial Catalog=qlddanh28ll10;Integrated Security=True"; // Thay đổi thông tin kết nối phù hợp với cơ sở dữ liệu của bạn

            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();

                string query = "SELECT distinct Ngay FROM DIEMDANH";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Chuyển đổi Date sang DateTime
                            DateTime dateTime = Convert.ToDateTime(reader["Ngay"]);

                            // Thêm vào ComboBox
                            cbo_ngay.Items.Add(dateTime);
                        }
                    }
                }

                connection.Close();
            }

            // Đặt mục được chọn trong ComboBox
            // Ở đây, tôi giả định rằng bạn muốn chọn mục đầu tiên trong danh sách.
            // Bạn có thể thay đổi giá trị này để chọn một mục khác.
            if (cbo_ngay.Items.Count > 0)
            {
                cbo_ngay.SelectedIndex = 1;
            }
        }


        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load_cbo();
            load_grid();
        }
    }
}
