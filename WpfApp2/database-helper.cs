using System.Collections.Generic;
using System.Data.SQLite;

namespace WpfApp2
{
    public class DatabaseHelper
    {
        private const string ConnectionString = "Data Source=tasks.db;Version=3;";

        public DatabaseHelper()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(
                    "CREATE TABLE IF NOT EXISTS Tasks (Id INTEGER PRIMARY KEY AUTOINCREMENT, Content TEXT NOT NULL);",
                    connection);
                command.ExecuteNonQuery();
            }
        }

        public void AddTask(Task task)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(
                    "INSERT INTO Tasks (Content) VALUES (@Content);",
                    connection);
                command.Parameters.AddWithValue("@Content", task.Content);
                command.ExecuteNonQuery();
                task.Id = (int)connection.LastInsertRowId;
            }
        }

        public void UpdateTask(Task task)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(
                    "UPDATE Tasks SET Content = @Content WHERE Id = @Id;",
                    connection);
                command.Parameters.AddWithValue("@Content", task.Content);
                command.Parameters.AddWithValue("@Id", task.Id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteTask(int taskId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(
                    "DELETE FROM Tasks WHERE Id = @Id;",
                    connection);
                command.Parameters.AddWithValue("@Id", taskId);
                command.ExecuteNonQuery();
            }
        }

        public List<Task> GetAllTasks()
        {
            var tasks = new List<Task>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT Id, Content FROM Tasks;", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        {
                            Id = reader.GetInt32(0),
                            Content = reader.GetString(1)
                        });
                    }
                }
            }
            return tasks;
        }

        public List<Task> SearchTasks(string searchTerm)
        {
            var tasks = new List<Task>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(
                    "SELECT Id, Content FROM Tasks WHERE Content LIKE @SearchTerm;",
                    connection);
                command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        {
                            Id = reader.GetInt32(0),
                            Content = reader.GetString(1)
                        });
                    }
                }
            }
            return tasks;
        }
    }
}
