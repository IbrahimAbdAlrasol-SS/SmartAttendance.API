namespace SmartAttendance.API.Models.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public int? StatusCode { get; set; }

        public static ApiResponse<T> SuccessResult(T data, string message = "تمت العملية بنجاح")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = 200
            };
        }

        public static ApiResponse<T> ErrorResult(string message, List<string>? errors = null, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                StatusCode = statusCode
            };
        }
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }

    public class SelectListItem
    {
        public string Value { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool Selected { get; set; }
    }

    public class ValidationErrorResponse
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}