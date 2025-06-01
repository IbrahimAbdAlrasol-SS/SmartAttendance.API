namespace SmartAttendance.API.Models.DTOs
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    public class TokenRefreshRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class StudentFilterRequest
    {
        public string? SearchTerm { get; set; }
        public string? Stage { get; set; }
        public string? StudyType { get; set; }
        public string? Section { get; set; }
        public bool? IsActive { get; set; }
        public bool? HasFaceData { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "FullName";
        public string? SortDirection { get; set; } = "asc";
    }

    public class StudentStatisticsDto
    {
        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }
        public int InactiveStudents { get; set; }
        public int StudentsWithFaceData { get; set; }
        public int StudentsWithoutFaceData { get; set; }
        public Dictionary<string, int> StudentsByStage { get; set; } = new();
        public Dictionary<string, int> StudentsByStudyType { get; set; } = new();
        public Dictionary<string, int> StudentsBySection { get; set; } = new();
    }
}