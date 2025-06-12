using Microsoft.AspNetCore.Mvc;

namespace MovieArchive.Services
{
    public interface IYorumlanabilir
    {
        Task<IActionResult> AddComment(int filmId, string content);
    }

}
