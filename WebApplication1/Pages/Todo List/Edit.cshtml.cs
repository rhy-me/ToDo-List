using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace WebApplication1.Pages.Todo_List
{
    public class EditModel : PageModel
    {
        public TodoInfo todoInfo = new TodoInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=todolist;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM todo WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                todoInfo.id = "" + reader.GetInt32(0);
                                todoInfo.task = reader.GetString(1);
                                todoInfo.description = reader.GetString(2);
                                todoInfo.endtime = reader.GetString(3);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
        public void OnPost()
        {
            todoInfo.id = Request.Form["id"];
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
                    String sql = "UPDATE todo " +
                    "SET task = @task, description = @description, endtime = @endtime " +
                    "WHERE id = @id ";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@task", todoInfo.task);
                        command.Parameters.AddWithValue("@description", todoInfo.description);
                        command.Parameters.AddWithValue("@endtime", todoInfo.endtime);
                        command.Parameters.AddWithValue("@id", todoInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Todo List/Index");
        }
    }
}
