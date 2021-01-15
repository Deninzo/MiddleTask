using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MiddleTask.Data
{
    public class People
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public DateTime DateChange { get; set; }

        public void CopyValues(People people)
        {
            FullName = people.FullName;
            PhoneNumber = people.PhoneNumber;
            City = people.City;
            Email = people.Email;
            DateChange = people.DateChange;
        }

    }
}
