using Microsoft.AspNetCore.Mvc;

namespace MovieArchive.Services
{
    public interface IPuanlanabilir
    {
         Task<IActionResult> AddPoint(int filmId, int puan);
    }

}
