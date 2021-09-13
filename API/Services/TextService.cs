using System;
using System.Linq;
using API.DTO;
using API.Extensions;

namespace API.Services
{
    public class TextService : ITextService
    {
        public TextService()
        {
        }
        public TextInfoDTO PrepareText(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new ArgumentNullException(inputString);
            }
            return new TextInfoDTO
            {
                UniqueCharCount = inputString.UniqueCharCount(),
                CharCount = inputString.Count(),
                WordCount = inputString.WordCount()
            };
        }
    }
}