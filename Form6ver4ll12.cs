private void ExportDataToExcel()
{
    // Tạo một ứng dụng Excel mới
    Excel.Application excelApp = new Excel.Application();

    // Tạo một Workbook mới
    Excel._Workbook workbook = excelApp.Workbooks.Add(Type.Missing);

    // Tạo một Worksheet mới và đặt nó làm Worksheet đầu tiên
    Excel._Worksheet worksheet = null;
    worksheet = workbook.Sheets["Sheet1"];
    worksheet = workbook.ActiveSheet;

    // Đặt tiêu đề cho các cột trong Worksheet
    for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
    {
        worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
    }

    // Điền dữ liệu vào các ô trong Worksheet
    for (int i = 0; i < dataGridView1.Rows.Count; i++)
    {
        for (int j = 0; j < dataGridView1.Columns.Count; j++)
        {
            worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
        }
    }

    // Lưu Workbook
    workbook.SaveAs("output.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

}
private void btn_in_Click(object sender, EventArgs e)
{
    ExportDataToExcel();
    // Tạo một ứng dụng Excel mới
    Excel.Application excelApp = new Excel.Application();

    // Tạo một Workbook mới
    Excel._Workbook workbook = excelApp.Workbooks.Add(Type.Missing);

    // Tạo một Worksheet mới và đặt nó làm Worksheet đầu tiên
    Excel._Worksheet worksheet = null;
    worksheet = workbook.Sheets["Sheet1"];
    worksheet = workbook.ActiveSheet;

    // Đặt tiêu đề cho các cột trong Worksheet
    for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
    {
        worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
    }

    // Điền dữ liệu vào các ô trong Worksheet
    for (int i = 0; i < dataGridView1.Rows.Count; i++)
    {
        for (int j = 0; j < dataGridView1.Columns.Count; j++)
        {
            worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
        }
    }

    // Mở hộp thoại SaveFileDialog để người dùng chọn nơi lưu tệp
    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
    {
        saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
        saveFileDialog.FilterIndex = 1;
        saveFileDialog.RestoreDirectory = true;

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            // Lưu Workbook tại đường dẫn mà người dùng đã chọn
            workbook.SaveAs(saveFileDialog.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }
    }

    // Đóng Workbook và ứng dụng Excel
    workbook.Close();
    excelApp.Quit();
}

/*chỉ thực hiện lưu dữ liệu trong datagridview vào excel*/
