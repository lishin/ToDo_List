using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Task> tasks;
        private DatabaseHelper dbHelper;

        public MainWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadTasks();
        }

        private void LoadTasks()
        {
            tasks = new ObservableCollection<Task>(dbHelper.GetAllTasks());
            TaskList.ItemsSource = tasks;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string content = TaskInput.Text.Trim();
            if (!string.IsNullOrEmpty(content))
            {
                var newTask = new Task { Content = content };
                dbHelper.AddTask(newTask);
                tasks.Add(newTask);
                TaskInput.Clear();
                StatusText.Text = "Task added successfully.";
            }
            else
            {
                MessageBox.Show("Please enter a task.", "Empty Task", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            var task = ((FrameworkElement)sender).DataContext as Task;
            if (task != null)
            {
                string newContent = Microsoft.VisualBasic.Interaction.InputBox("Edit task:", "Edit Task", task.Content);
                if (!string.IsNullOrEmpty(newContent))
                {
                    task.Content = newContent;
                    dbHelper.UpdateTask(task);
                    TaskList.Items.Refresh();
                    StatusText.Text = "Task updated successfully.";
                }
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var task = ((FrameworkElement)sender).DataContext as Task;
            if (task != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    dbHelper.DeleteTask(task.Id);
                    tasks.Remove(task);
                    StatusText.Text = "Task deleted successfully.";
                }
            }
        }

        private void SearchTasks_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchInput.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var searchResults = dbHelper.SearchTasks(searchTerm);
                TaskList.ItemsSource = new ObservableCollection<Task>(searchResults);
                StatusText.Text = $"Found {searchResults.Count} task(s) matching '{searchTerm}'.";
            }
            else
            {
                LoadTasks();
                StatusText.Text = "Showing all tasks.";
            }
        }
    }
}