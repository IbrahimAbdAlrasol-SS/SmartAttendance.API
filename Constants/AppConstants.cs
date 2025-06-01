namespace SmartAttendance.API.Constants
{
    public static class UserRoles
    {
        public const string Student = "Student";
        public const string Professor = "Professor";
        public const string Admin = "Admin";
    }

    public static class AttendanceStatus
    {
        public const string Present = "Present";
        public const string Absent = "Absent";
        public const string Late = "Late";
        public const string Excused = "Excused";
    }

    public static class DetectionMethods
    {
        public const string FaceRecognition = "FaceRecognition";
        public const string Manual = "Manual";
        public const string QRCode = "QRCode";
    }

    public static class SessionStatus
    {
        public const string Scheduled = "Scheduled";
        public const string Active = "Active";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
    }

    public static class AcademicInfo
    {
        public static readonly string[] Stages = { "First", "Second", "Third", "Fourth" };
        public static readonly string[] StudyTypes = { "Morning", "Evening" };
        public static readonly string[] Sections = { "A", "B", "C", "D" };
        public static readonly string[] Semesters = { "الفصل الأول", "الفصل الثاني" };
    }

    public static class ApiMessages
    {
        public const string Success = "تمت العملية بنجاح";
        public const string Error = "حدث خطأ في العملية";
        public const string NotFound = "العنصر غير موجود";
        public const string Unauthorized = "غير مخول للوصول";
        public const string ValidationError = "خطأ في البيانات المدخلة";
    }
}