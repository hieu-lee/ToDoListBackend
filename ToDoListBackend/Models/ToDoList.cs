using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToDoListBackend.Models
{
    public class ToDoList
    {
        [Key]
        public string listId { get; set; } = Guid.NewGuid().ToString();
        public string name { get; set; }
        public DateTime timeCreate { get; set; } = DateTime.UtcNow.ToLocalTime();
        [ForeignKey("Owner")]
        public string ownerUsername { get; set; }
        [JsonIgnore]
        public Account owner { get; set; }
        public List<ToDoItem> items { get; set; } = new List<ToDoItem>();
        public string deleteHeight { get; set; } = "0";
    }
}
