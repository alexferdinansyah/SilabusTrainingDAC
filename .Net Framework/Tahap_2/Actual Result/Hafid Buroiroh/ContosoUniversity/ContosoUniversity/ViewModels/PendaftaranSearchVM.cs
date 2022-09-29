using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContosoUniversity.ViewModels
{
    public class PendaftaranSearchVM : IValidatableObject
    {

        [Display(Name = "Judul Mata Kuliah")]
        public string CourseID { get; set; } 
        
        [Display(Name = "Nama Mahasiswa")]
        public string StudentID { get; set; } 
        
        [Display(Name = "Nilai")]
        public string Grade { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(CourseID))
            {
                yield return new ValidationResult("Masukkan Mata Kuliah Yang Ingin Dicari ");
                //Memberi validation ke variable yang dituju!
            }
            if (String.IsNullOrEmpty(StudentID))
            {
                yield return new ValidationResult("Masukan Nama Mahasiswa Yang Ingin Dicari ");
                //Memberi validation ke variable yang dituju!
            }
            if (String.IsNullOrEmpty(Grade))
            {
                yield return new ValidationResult("Masukan Nilai Ujian Yang Ingin Dicari ");
                //Memberi validation ke variable yang dituju!
            }
        }
    }
   
}
