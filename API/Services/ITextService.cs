using API.DTO;

namespace API.Services
{
    public interface ITextService
    {
        TextInfoDTO PrepareText(string inputString);
    }
}