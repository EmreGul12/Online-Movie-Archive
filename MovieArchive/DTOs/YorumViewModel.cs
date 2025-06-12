using System.ComponentModel.DataAnnotations;

namespace MovieArchive.DTOs
{
    // ViewModels/EkleYorumViewModel.
    public class EkleYorumViewModel
    {
        [Required(ErrorMessage = "Yorum başlığı zorunludur.")]
        [StringLength(200)]
        public string Baslik { get; set; }

        [Required(ErrorMessage = "Yorum içeriği zorunludur.")]
        [StringLength(1000)]
        public string Icerik { get; set; }

    }
}


