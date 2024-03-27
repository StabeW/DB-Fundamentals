namespace P01_StudentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Student
    {
        public Student()
        {
            this.RegisteredOn = DateTime.Now;

            this.Homeworks = new List<Homework>();
            this.StudentsCourses = new List<StudentCourse>();
        }

        public int StudentId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public ICollection<Homework> Homeworks { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; }
    }
}