using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace WebApplication1.Pages.Todo_List
{
    public class CreateModel : PageModel
    {
        public TodoInfo todoInfo = new TodoInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            todoInfo.task = Request.Form["task"];
            todoInfo.description = Request.Form["description"];
            todoInfo.endtime = Request.Form["endtime"];

            if (todoInfo.task.Length == 0 || todoInfo.description.Length == 0 || todoInfo.endtime.Length == 0)
            {
                errorMessage = "All the fields are required.";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=todolist;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO todo " +
                                "(task, description, endtime) VALUES " +
                                "(@task, @description, @endtime);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@task", todoInfo.task);
                        command.Parameters.AddWithValue("@description", todoInfo.description);
                        command.Parameters.AddWithValue("@endtime", todoInfo.endtime);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            todoInfo.task = ""; todoInfo.description = ""; todoInfo.endtime = "";
            successMessage = "New task added correctly";

            Response.Redirect("/Todo List/Index");
        }
    }
}
