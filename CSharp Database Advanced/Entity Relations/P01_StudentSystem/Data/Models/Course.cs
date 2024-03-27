namespace P01_StudentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Course
    {
        public Course()
        {
            this.Resources = new List<Resource>();
            this.Homeworks = new List<Homework>();
            this.StudentsCourses = new List<StudentCourse>();
        }

        public int CourseId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public ICollection<Resource> Resources { get; set; }

        public ICollection<Homework> Homeworks { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; }
    }
}