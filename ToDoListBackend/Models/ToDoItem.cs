using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListBackend.Models
{
    public class ToDoItem
    {
        [Key]
        public string itemId { get; set; } = Guid.NewGuid().ToString();
        public string parentListId { get; set; }
        public string owner { get; set; }
        public DateTime timeCreate { get; set; }
        public DateTime? timeRemind { get; set; }
        public bool important { get; set; } = false;
        public bool completed { get; set; } = false;
        public string content { get; set; } = string.Empty;
        public string title { get; set; }
        public string contentHeight { get; set; } = "0";
        public string deleteHeight { get; set; } = "0";
        public DateTime? lastNotified { get; set; }
    }
}
