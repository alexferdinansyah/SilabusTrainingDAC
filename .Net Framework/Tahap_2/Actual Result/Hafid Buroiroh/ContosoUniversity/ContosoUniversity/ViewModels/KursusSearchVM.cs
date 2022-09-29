using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.ViewModels
{
    public class KursusSearchVM : IValidatableObject
    {
        
        [Display(Name = "ID Kursus")]
        public string CourseID { get; set; }

        [Display(Name = "Judul")]
        public string Title { get; set; }

        [Display(Name = "Mata Ujian")]
        public string Credits { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (String.IsNullOrEmpty(CourseID))
            {
                yield return new ValidationResult("Masukkan ID Kursus Yang Ingin Dicari ");
                //Memberi validation ke variable yang dituju!
            } if (String.IsNullOrEmpty(Title))
            {
                yield return new ValidationResult("Masukan Judul Kursus Yang Ingin Dicari ");
                //Memberi validation ke variable yang dituju!
            }if (String.IsNullOrEmpty(Credits))
            {
                yield return new ValidationResult("Masukan Mata Ujian Yang Ingin Dicari ");
                //Memberi validation ke variable yang dituju!
            }
        }
    }
}