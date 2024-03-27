using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        public Patient()
        {
            Prescriptions = new HashSet<PatientMedicament>();
            Visitations = new HashSet<Visitation>();
            Diagnoses = new HashSet<Diagnose>();
        }

        public int PatientId { get; set; }

        public string FirstName { get; set; }

        
        public string LastName { get; set; }

       
        public string Address { get; set; }

        
        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        public ICollection<PatientMedicament> Prescriptions { get; set; }

        public ICollection<Visitation> Visitations { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }



    }
}
