using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToDoListBackend.Models
{
    public class Account
    {
        [Key]
        public string username { get; set; }
        public string password { get; set; }
        [JsonIgnore]
        public HashSet<ToDoList> lists { get; set; } = new();
    }
}
