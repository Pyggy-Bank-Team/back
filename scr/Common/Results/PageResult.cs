using Newtonsoft.Json;

namespace Common.Results
{
    public class PageResult<T>
    {
        public int TotalPages { get; set; }
        
        public int CurrentPage { get; set; }

        [JsonIgnore]
        public int CountItemsOnPage { get; set; } = 30;

        public T[] Result { get; set; }
    }
}