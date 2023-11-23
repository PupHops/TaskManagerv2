using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Taskmanager.Class;

using Task = Taskmanager.Class.Task;

namespace Taskmanager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Maincontext _context;

        public ObservableCollection<TypeTask> TaskTypes { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _context = new Maincontext();
            // Инициализация и загрузка данных
            TaskTypes = new ObservableCollection<TypeTask>();
            LoadData();
        }
        private void LoadData()
        {
            // Загрузка данных из базы данных
            using (var context = new Maincontext())
            {
                TaskTypes = new ObservableCollection<TypeTask>(context.TypeTasks.Include("Tasks").ToList());

                // Вам нужно убедиться, что задачи связаны с соответствующими TypeTask
                foreach (var typeTask in TaskTypes)
                {
                    typeTask.Tasks = new ObservableCollection<Task>(context.Tasks.Where(t => t.TaskTypeId == typeTask.Id));
                }
            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            var task = ((Button)sender).DataContext as Task;
            var currentTaskType = TaskTypes.FirstOrDefault(tt => tt.Tasks.Contains(task));

            if (currentTaskType != null)
            {
                var index = currentTaskType.Tasks.IndexOf(task);
                if (index > 0)
                {
                    var targetTask = currentTaskType.Tasks[index - 1];
                    currentTaskType.Tasks[index - 1] = task;
                    currentTaskType.Tasks[index] = targetTask;

                    using (var context = new Maincontext())
                    {
                        context.Tasks.Find(task.Id).TaskTypeId = currentTaskType.Id;
                        context.Tasks.Find(targetTask.Id).TaskTypeId = currentTaskType.Id;
                        context.SaveChanges();
                    }
                }
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var task = ((Button)sender).DataContext as Task;
            var currentTaskType = TaskTypes.FirstOrDefault(tt => tt.Tasks.Contains(task));

            if (currentTaskType != null)
            {
                var index = currentTaskType.Tasks.IndexOf(task);
                if (index < currentTaskType.Tasks.Count - 1)
                {
                    var targetTask = currentTaskType.Tasks[index + 1];
                    currentTaskType.Tasks[index + 1] = task;
                    currentTaskType.Tasks[index] = targetTask;

                    using (var context = new Maincontext())
                    {
                        context.Tasks.Find(task.Id).TaskTypeId = currentTaskType.Id;
                        context.Tasks.Find(targetTask.Id).TaskTypeId = currentTaskType.Id;
                        context.SaveChanges();
                    }
                }
            }
        }

        private void MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            var task = ((Button)sender).DataContext as Task;
            var currentTaskType = TaskTypes.FirstOrDefault(tt => tt.Tasks.Contains(task));

            if (currentTaskType != null)
            {
                var index = currentTaskType.Tasks.IndexOf(task);
                if (index >= 0)
                {
                    var currentIndex = TaskTypes.IndexOf(currentTaskType);

                    var targetTaskType = currentIndex > 0 ? TaskTypes[currentIndex - 1] : null;

                    if (targetTaskType != null && targetTaskType.Tasks.Any())
                    {
                        currentTaskType.Tasks.Remove(task);
                        targetTaskType.Tasks.Add(task);

                        using (var context = new Maincontext())
                        {
                            context.Tasks.Find(task.Id).TaskTypeId = targetTaskType.Id;
                            context.SaveChanges();
                        }
                    }
                }
            }
        }

        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            var task = ((Button)sender).DataContext as Task;
            var currentTaskType = TaskTypes.FirstOrDefault(tt => tt.Tasks.Contains(task));

            if (currentTaskType != null)
            {
                var index = currentTaskType.Tasks.IndexOf(task);
                if (index >= 0)
                {
                    var currentIndex = TaskTypes.IndexOf(currentTaskType);
                    var maxIndex = TaskTypes.Count - 1;

                    var targetTaskType = currentIndex < maxIndex ? TaskTypes[currentIndex + 1] : null;

                    if (targetTaskType != null)
                    {
                        currentTaskType.Tasks.Remove(task);
                        targetTaskType.Tasks.Add(task);

                        using (var context = new Maincontext())
                        {
                            context.Tasks.Find(task.Id).TaskTypeId = targetTaskType.Id;
                            context.SaveChanges();
                        }
                    }
                }
            }
        }





    }
}
