using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography.Xml;

namespace WebApplication1.Pages.Todo_List
{
    public class IndexModel : PageModel
    {
        public List<TodoInfo> listtodo = new List<TodoInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=todolist;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM todo";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TodoInfo todoInfo = new TodoInfo();
                                todoInfo.id = "" + reader.GetInt32(0);
                                todoInfo.task = reader.GetString(1);
                                todoInfo.description = reader.GetString(2);
                                if (!reader.IsDBNull(3))
                                {
                                    todoInfo.endtime = reader.GetTimeSpan(3);
                                }


                                DateTime endDateTime = DateTime.Today.Add(todoInfo.endtime);
                                todoInfo.remainingTime = endDateTime > DateTime.Now ? endDateTime - DateTime.Now : TimeSpan.Zero; 

                                listtodo.Add(todoInfo);
                            } 
                        }
                    }                
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class TodoInfo
    {
        public string id;
        public string task;
        public string description;
        public TimeSpan endtime;
        public TimeSpan remainingTime;
    }
}
