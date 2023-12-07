/*hàm lưu dữ liệu vào file excel*/
        private void ExportDataToExcel()
        {
            // Tạo một ứng dụng Excel mới
            Excel.Application excelApp = new Excel.Application();

            // Tạo một Workbook mới
            Excel._Workbook workbook = excelApp.Workbooks.Add(Type.Missing);

            // Tạo một Worksheet mới và đặt nó làm Worksheet đầu tiên
            Excel._Worksheet worksheet = workbook.Sheets["Sheet1"];

            // Thêm dữ liệu từ TextBox và Label vào Worksheet
            int startRow = dataGridView1.Rows.Count +1;
            
            worksheet.Cells[startRow, 2] = "Tên giảng viên";
            worksheet.Cells[startRow++, 3] = txt_tenGV.Text;

            // Thêm giá trị đang được chọn từ ComboBox vào Worksheet
            worksheet.Cells[startRow, 2] = "Ngày điểm danh";
            worksheet.Cells[startRow++, 3] = cbo_ngay.SelectedItem.ToString();

            worksheet.Cells[startRow, 2] = "Mã lớp";
            worksheet.Cells[startRow++, 3] = txt_malop.Text;
            worksheet.Cells[startRow, 2] = "Địa chỉ";
            worksheet.Cells[startRow++, 3] = textBox3.Text;
            worksheet.Cells[startRow, 2] = "Tên môn";
            worksheet.Cells[startRow++, 3] = textBox4.Text;
            worksheet.Cells[startRow, 2] = "Hiện diện";
            worksheet.Cells[startRow++, 3] = hiendien.Text;
            worksheet.Cells[startRow, 2] = "Vắng";
            worksheet.Cells[startRow++, 3] = vang.Text;
            // Đặt tiêu đề cho các cột trong Worksheet
           
            dataGridView1.Columns[0].HeaderText = " STT";
            dataGridView1.Columns[1].HeaderText = "Mã SV";
            dataGridView1.Columns[2].HeaderText = "Tên SV";
            dataGridView1.Columns[3].HeaderText = "Vắng có phép";
            dataGridView1.Columns[4].HeaderText = "Vắng KHÔNG phép";
            dataGridView1.Columns[5].HeaderText = "Ghi chú";

            // Đặt tiêu đề cho các cột trong Worksheet
            for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
            {
                worksheet.Cells[11, i] = dataGridView1.Columns[i - 1].HeaderText;
            }

            // Điền dữ liệu vào các ô trong Worksheet
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    // Kiểm tra xem giá trị của ô có phải là 'true' hay 'false'
                    if (dataGridView1.Rows[i].Cells[j].Value is bool)
                    {
                        if ((bool)dataGridView1.Rows[i].Cells[j].Value)
                        {
                            // Nếu đúng, thay thế giá trị bằng ký tự tượng trưng cho checkbox được chọn
                            worksheet.Cells[i + 12, j + 1] = "[x]";
                        }
                        else
                        {
                            // Nếu đúng, thay thế giá trị bằng ký tự tượng trưng cho checkbox không được chọn
                            worksheet.Cells[i + 12, j + 1] = "";
                        }
                    }
                    else
                    {
                        // Nếu không, giữ nguyên giá trị
                        worksheet.Cells[i + 12, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
            }

            // Đóng Workbook và ứng dụng Excel
            workbook.Close();
            excelApp.Quit();
        }
