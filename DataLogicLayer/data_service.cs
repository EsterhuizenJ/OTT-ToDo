using ClosedXML.Excel;

namespace OTT_ToDo.DataLogicLayer
{
    public class data_service
    {
        private List<data_class_todo> _tasks = new List<data_class_todo>();
        public string _last_error = "";
        
        public List<data_class_todo> get_all_tasks(bool IncludeCompleted)
        {
            return (IncludeCompleted ? _tasks.ToList() : _tasks.Where(w => w.IsCompleted == IncludeCompleted).ToList());
        }

        public bool create_new_task(data_class_todo task)
        {
            //assign auto id
            try
            {
                task.Id = (_tasks.Count == 0 ? 0 : _tasks.Max(m => m.Id))+1;
                task.IsCompleted = false;
                _tasks.Add(task);
                _last_error = "";
                return true;
            }
            catch (Exception ex)
            {
                _last_error = ex.Message;
                return false;
            }

        }
        public void ToggleTaskCompletion(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
            }
        }

        public bool delete_task(data_class_todo task)
        {
            try
            {
                _tasks.Remove(task);
                _last_error = "";
                return true;
            }
            catch (Exception ex)
            {
                _last_error = ex.Message;
                return false;
            }
        }

        public bool update_task(data_class_todo task)
        {
            try
            {
                _tasks.Remove(_tasks.Where(w=>w.Id == task.Id).FirstOrDefault());
                _tasks.Add(task);
                return true;
            }
            catch (Exception ex)
            {
                _last_error += ex.Message;
                return false;
            }
        }

        public byte[] ExportToExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Tasks");
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "Status";
                worksheet.Cell(1, 5).Value = "Created Date";

                var row = 2;
                foreach (var task in _tasks)
                {
                    worksheet.Cell(row, 1).Value = task.Id;
                    worksheet.Cell(row, 2).Value = task.Title;
                    worksheet.Cell(row, 3).Value = task.Description;
                    worksheet.Cell(row, 4).Value = task.IsCompleted ? "Completed" : "Pending";
                    worksheet.Cell(row, 5).Value = task.CreatedDate;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

    }
}
