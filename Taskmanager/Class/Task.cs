using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskmanager.Class
{
    public class Task  
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public int UserId { get; set; }
        public int TaskTypeId { get; set; }
        public User User { get; set; }
        public TypeTask TaskType { get; set; }
    }
}
