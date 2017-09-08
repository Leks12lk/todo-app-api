using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LearnToDo.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsDone { get; set; }

        public DateTime? DateAdded { get; set; }

		[JsonIgnore]
		[ForeignKey("User")]
		public string UserId { get; set; }

		//public string UserName { get; set; }

		[JsonIgnore]
		public virtual ApplicationUser User { get; set; }

        public Todo()
        {
            IsDone = false;
            DateAdded = DateTime.Now;
        }
    }
}