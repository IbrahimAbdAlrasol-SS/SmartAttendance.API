🎓 نظام الحضور الذكي المتكامل - دليل التطوير الشامل
📋 جدول المحتويات

نظرة عامة على المشروع
التحليل المعماري
التقنيات المختارة
هيكل قاعدة البيانات
الخدمات والأنظمة الفرعية
APIs والواجهات
نظام الأمان والحماية
الذكاء الاصطناعي
التكامل مع التلغرام
إدارة الملفات والفيديو
المراقبة والأداء
خطة التنفيذ


📖 نظرة عامة على المشروع {#overview}
الهدف الأساسي
تطوير نظام حضور ذكي متكامل للجامعات يجمع بين تقنيات التعرف على الوجوه، إدارة الواجبات، والتبليغات الذكية، مع توفير تجربة مستخدم عصرية ونظام إدارة احترافي.
النطاق الوظيفي
للطلاب:

تسجيل بيانات شخصية مع تدريب نموذج التعرف على الوجه محلياً
رفع البيانات المدربة للسيرفر بشكل مشفر
الحصول على QR code مشفر للربط مع بوت التلغرام
تلقي تقارير حضور شخصية
إدارة الواجبات والتسليم عبر التلغرام
الحصول على استشارات AI حول نظام بولونيا

للمحاضرين:

إدارة جلسات الحضور للشعب المخصصة
بدء/إيقاف تسجيل الحضور عبر الكاميرات
عرض تقارير مفصلة لشعبهم
إجراء تعديلات يدوية على الحضور (Manual Override)

للإداريين:

لوحة تحكم شاملة لكافة البيانات
إدارة الطلاب والمحاضرين والشعب
تقارير إحصائية متقدمة
إدارة إعدادات النظام

لأدمن البوت (طلاب مسؤولين):

إدارة واجبات الشعبة
نظام عقوبات ذكي
إرسال تبليغات للطلاب
مراقبة تسليم الواجبات

المتطلبات التقنية الأساسية
الأداء:

دعم 4 كاميرات متزامنة
معالجة فيديو real-time
دعم 1000+ طالب متزامن
استجابة APIs < 200ms
توفر النظام 99.5%

الأمان:

تشفير البيانات الحساسة
JWT tokens مع Refresh mechanism
Rate limiting للAPIs
تشفير QR codes
حماية متقدمة ضد الاختراق

التوافقية:

دعم الهواتف الذكية (Android/iOS)
متصفحات الويب الحديثة
تكامل مع Telegram Bot API
دعم كاميرات USB/IP


🏗️ التحليل المعماري {#architecture}
النمط المعماري المختار: Microservices Architecture
الأسباب:

التعقيد العالي: النظام يحتوي على وظائف متنوعة (Face Recognition, Telegram Bot, File Processing)
القابلية للتوسع: كل خدمة يمكن توسيعها مستقلة
المرونة: تطوير ونشر مستقل لكل خدمة
الموثوقية: فشل خدمة لا يؤثر على الأخريات

الخدمات الأساسية:
1. Identity Service (خدمة الهوية)

إدارة تسجيل الدخول والمصادقة
إدارة JWT tokens
نظام الصلاحيات (RBAC)
API Gateway Authentication

2. Student Management Service (خدمة إدارة الطلاب)

تسجيل الطلاب الجدد
إدارة بيانات الطلاب
ربط الطلاب بالشعب
إدارة المراحل الدراسية

3. Face Recognition Service (خدمة التعرف على الوجوه)

استقبال بيانات التدريب من التطبيق
معالجة فيديوهات الكاميرات
مقارنة الوجوه وتحديد الهوية
إدارة نماذج التعرف

4. Attendance Service (خدمة الحضور)

تسجيل الحضور والغياب
إدارة جلسات الحضور
تقارير الحضور
Manual Override للمحاضرين

5. Camera Management Service (خدمة إدارة الكاميرات)

التحكم في الكاميرات
إدارة جلسات التسجيل
معالجة الفيديو
تنسيق العمليات المتزامنة

6. Telegram Bot Service (خدمة بوت التلغرام)

إدارة الواجبات
نظام العقوبات الذكي
التبليغات والإشعارات
التكامل مع QR codes

7. Homework Management Service (خدمة إدارة الواجبات)

إضافة وإدارة الواجبات
تتبع التسليم
حساب العقوبات
إرسال التذكيرات

8. AI Chat Service (خدمة الدردشة الذكية)

استشارات نظام بولونيا
تحليل سلوك الطلاب
نصائح أكاديمية
تكامل مع بيانات النظام

9. File Management Service (خدمة إدارة الملفات)

تخزين الفيديوهات
ضغط ومعالجة الملفات
النسخ الاحتياطية
إدارة دورة حياة الملفات

10. Notification Service (خدمة الإشعارات)

إرسال الإشعارات المتنوعة
إدارة قوائم الاستلام
تتبع حالة التسليم
جدولة الإشعارات

11. Analytics Service (خدمة التحليلات)

تحليل بيانات الحضور
إحصائيات الأداء
تقارير متقدمة
Dashboard metrics

12. API Gateway

توحيد نقطة الدخول
Load balancing
Rate limiting
Request routing

التواصل بين الخدمات:
Synchronous Communication:

REST APIs للعمليات المباشرة
gRPC للتواصل عالي الأداء بين الخدمات الداخلية

Asynchronous Communication:

Apache Kafka لEvent Streaming
Redis Pub/Sub للرسائل السريعة
Message Queues للمهام الثقيلة

البنية التحتية:
API Gateway: Ocelot (.NET)
Service Discovery: Consul
Configuration Management: Azure Key Vault + appsettings
Monitoring: Prometheus + Grafana
Logging: Serilog + ELK Stack
Caching: Redis Cluster
Message Broker: Apache Kafka + Redis
Database: PostgreSQL Cluster + MongoDB لlogging

⚙️ التقنيات المختارة {#technologies}
Backend Framework: ASP.NET Core 8.0
الأسباب:

الأداء العالي: من أسرع frameworks للويب
Cross-platform: يعمل على Linux/Windows/Mac
Microservices Support: دعم ممتاز للمايكروسيرفيس
Enterprise Ready: مناسب للمشاريع الكبيرة
Rich Ecosystem: مكتبات غنية ودعم كبير
Security: أمان متقدم built-in
Async/Await: معالجة متقدمة للعمليات غير المتزامنة

قواعد البيانات:
PostgreSQL 15 (Primary Database):

أداء عالي للعمليات المعقدة
JSONB support للبيانات المرنة
Full-text search
Advanced indexing
ACID compliance
Extensions support

MongoDB 7.0 (Document Store):

تخزين logs ومعلومات غير منظمة
Chat history
Analytics data
Flexible schema

Redis 7.0 (Cache & Session Store):

Session management
Caching layer
Pub/Sub messaging
Rate limiting data
Temporary data storage

Message Queue & Event Streaming:
Apache Kafka:

Event streaming بين الخدمات
Attendance events
Homework notifications
System-wide events

RabbitMQ:

Task queues
File processing jobs
Email notifications
Background tasks

File Storage:
MinIO (S3-compatible):

تخزين الفيديوهات والصور
Distributed storage
High availability
Cost-effective

AI/ML Stack:
Python Services مع .NET Integration:

OpenCV: معالجة الصور والفيديو
dlib: Face recognition algorithms
TensorFlow/PyTorch: Deep learning models
FastAPI: Python APIs سريعة
Celery: Background tasks

Integration مع .NET:

HTTP APIs للتواصل
Message queues للمهام الثقيلة
Shared data storage

Monitoring & Observability:
Application Monitoring:

Application Insights: .NET monitoring
Prometheus: Metrics collection
Grafana: Visualization
Jaeger: Distributed tracing

Logging:

Serilog: Structured logging
ELK Stack: Log aggregation and search
Seq: .NET-specific log server

Security:
Authentication & Authorization:

JWT: Access tokens
Refresh Tokens: Long-term authentication
IdentityServer4: OAuth2/OpenID Connect
ASP.NET Core Identity: User management

Encryption:

AES-256: Data encryption
RSA: Key exchange
Argon2: Password hashing
TLS 1.3: Transport security

DevOps & Deployment:
Containerization:

Docker: Application containers
Docker Compose: Development environment
Kubernetes: Production orchestration

CI/CD:

Azure DevOps: CI/CD pipelines
GitHub Actions: Alternative CI/CD
Docker Registry: Container storage

Infrastructure as Code:

Terraform: Infrastructure provisioning
Ansible: Configuration management


🗄️ هيكل قاعدة البيانات {#database}
Database Schema Design
Schema Organization:
sql-- Main schemas
CREATE SCHEMA identity;      -- Authentication & authorization
CREATE SCHEMA academic;      -- Academic data (students, instructors, etc.)
CREATE SCHEMA attendance;    -- Attendance related data
CREATE SCHEMA homework;      -- Homework management
CREATE SCHEMA telegram;      -- Telegram bot data
CREATE SCHEMA system;        -- System configuration and logs
CREATE SCHEMA ai;           -- AI related data
CREATE SCHEMA files;        -- File management
الكيانات الأساسية (Core Entities):
1. Identity Schema:
sql-- Users table (base for all user types)
CREATE TABLE identity.users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    salt VARCHAR(255) NOT NULL,
    email_confirmed BOOLEAN DEFAULT FALSE,
    lockout_enabled BOOLEAN DEFAULT TRUE,
    lockout_end TIMESTAMP NULL,
    access_failed_count INTEGER DEFAULT 0,
    two_factor_enabled BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    last_login TIMESTAMP NULL
);

-- Roles
CREATE TABLE identity.roles (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(50) UNIQUE NOT NULL,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- User roles mapping
CREATE TABLE identity.user_roles (
    user_id UUID REFERENCES identity.users(id) ON DELETE CASCADE,
    role_id UUID REFERENCES identity.roles(id) ON DELETE CASCADE,
    assigned_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    assigned_by UUID REFERENCES identity.users(id),
    PRIMARY KEY (user_id, role_id)
);

-- Claims/Permissions
CREATE TABLE identity.claims (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    type VARCHAR(100) NOT NULL,
    value VARCHAR(255) NOT NULL,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Role claims
CREATE TABLE identity.role_claims (
    role_id UUID REFERENCES identity.roles(id) ON DELETE CASCADE,
    claim_id UUID REFERENCES identity.claims(id) ON DELETE CASCADE,
    PRIMARY KEY (role_id, claim_id)
);

-- Refresh tokens
CREATE TABLE identity.refresh_tokens (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES identity.users(id) ON DELETE CASCADE,
    token VARCHAR(500) UNIQUE NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    revoked_at TIMESTAMP NULL,
    replaced_by_token VARCHAR(500) NULL,
    device_info JSONB NULL,
    ip_address INET NULL
);

-- Security logs
CREATE TABLE identity.security_logs (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES identity.users(id) ON DELETE SET NULL,
    action VARCHAR(100) NOT NULL,
    ip_address INET,
    user_agent TEXT,
    success BOOLEAN NOT NULL,
    failure_reason TEXT NULL,
    additional_data JSONB NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
2. Academic Schema:
sql-- Study types (صباحي/مسائي)
CREATE TABLE academic.study_types (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(50) UNIQUE NOT NULL,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Academic stages (المراحل - متغيرة)
CREATE TABLE academic.academic_stages (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(100) NOT NULL,
    level INTEGER NOT NULL,
    study_type_id UUID REFERENCES academic.study_types(id),
    is_active BOOLEAN DEFAULT TRUE,
    academic_year VARCHAR(20) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(name, study_type_id, academic_year)
);

-- Sections (الشعب)
CREATE TABLE academic.sections (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(50) NOT NULL,
    code VARCHAR(20) UNIQUE NOT NULL,
    academic_stage_id UUID REFERENCES academic.academic_stages(id),
    study_type_id UUID REFERENCES academic.study_types(id),
    max_students INTEGER DEFAULT 50,
    current_students INTEGER DEFAULT 0,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Students
CREATE TABLE academic.students (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES identity.users(id) ON DELETE CASCADE,
    student_number VARCHAR(20) UNIQUE NOT NULL,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    father_name VARCHAR(100),
    mother_name VARCHAR(100),
    date_of_birth DATE,
    gender VARCHAR(10) CHECK (gender IN ('male', 'female')),
    phone VARCHAR(20),
    address TEXT,
    section_id UUID REFERENCES academic.sections(id),
    academic_stage_id UUID REFERENCES academic.academic_stages(id),
    study_type_id UUID REFERENCES academic.study_types(id),
    enrollment_date DATE DEFAULT CURRENT_DATE,
    graduation_date DATE NULL,
    status VARCHAR(20) DEFAULT 'active' CHECK (status IN ('active', 'suspended', 'graduated', 'withdrawn')),
    gpa DECIMAL(3,2) DEFAULT 0.00,
    total_credits INTEGER DEFAULT 0,
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Instructors
CREATE TABLE academic.instructors (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES identity.users(id) ON DELETE CASCADE,
    employee_number VARCHAR(20) UNIQUE NOT NULL,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    title VARCHAR(50), -- أستاذ، أستاذ مساعد، مدرس
    department VARCHAR(100),
    phone VARCHAR(20),
    office_location VARCHAR(100),
    specialization TEXT,
    hire_date DATE,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Instructor sections (المحاضر يدرس متعدد شعب)
CREATE TABLE academic.instructor_sections (
    instructor_id UUID REFERENCES academic.instructors(id) ON DELETE CASCADE,
    section_id UUID REFERENCES academic.sections(id) ON DELETE CASCADE,
    academic_year VARCHAR(20) NOT NULL,
    semester VARCHAR(20) NOT NULL,
    assigned_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (instructor_id, section_id, academic_year, semester)
);

-- Face recognition data
CREATE TABLE academic.student_face_data (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    student_id UUID REFERENCES academic.students(id) ON DELETE CASCADE,
    face_encoding BYTEA NOT NULL, -- المشفر
    training_images_count INTEGER DEFAULT 0,
    model_accuracy DECIMAL(5,4) DEFAULT 0.0000,
    last_trained TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    training_device_info JSONB NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
3. Attendance Schema:
sql-- Attendance sessions (جلسات الحضور)
CREATE TABLE attendance.attendance_sessions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    section_id UUID REFERENCES academic.sections(id),
    instructor_id UUID REFERENCES academic.instructors(id),
    session_name VARCHAR(200),
    start_time TIMESTAMP NOT NULL,
    end_time TIMESTAMP NULL,
    status VARCHAR(20) DEFAULT 'active' CHECK (status IN ('scheduled', 'active', 'completed', 'cancelled')),
    camera_ids JSONB NULL, -- array of camera IDs used
    auto_end BOOLEAN DEFAULT TRUE,
    max_duration_minutes INTEGER DEFAULT 120,
    notes TEXT,
    created_by UUID REFERENCES identity.users(id),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Individual attendance records
CREATE TABLE attendance.attendance_records (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    session_id UUID REFERENCES attendance.attendance_sessions(id) ON DELETE CASCADE,
    student_id UUID REFERENCES academic.students(id) ON DELETE CASCADE,
    status VARCHAR(20) DEFAULT 'absent' CHECK (status IN ('present', 'absent', 'late', 'excused')),
    detected_at TIMESTAMP NULL, -- when face was detected
    confidence_score DECIMAL(5,4) NULL, -- face recognition confidence
    detection_method VARCHAR(50) DEFAULT 'face_recognition', -- face_recognition, manual
    manual_override BOOLEAN DEFAULT FALSE,
    override_reason TEXT NULL,
    override_by UUID REFERENCES identity.users(id) NULL,
    override_at TIMESTAMP NULL,
    camera_id VARCHAR(50) NULL,
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(session_id, student_id)
);

-- Attendance statistics (pre-calculated for performance)
CREATE TABLE attendance.attendance_statistics (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    student_id UUID REFERENCES academic.students(id) ON DELETE CASCADE,
    section_id UUID REFERENCES academic.sections(id) ON DELETE CASCADE,
    academic_year VARCHAR(20) NOT NULL,
    semester VARCHAR(20) NOT NULL,
    total_sessions INTEGER DEFAULT 0,
    present_sessions INTEGER DEFAULT 0,
    absent_sessions INTEGER DEFAULT 0,
    late_sessions INTEGER DEFAULT 0,
    excused_sessions INTEGER DEFAULT 0,
    attendance_percentage DECIMAL(5,2) DEFAULT 0.00,
    last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(student_id, section_id, academic_year, semester)
);
4. Homework Schema:
sql-- Homework assignments
CREATE TABLE homework.assignments (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title VARCHAR(255) NOT NULL,
    description TEXT,
    section_id UUID REFERENCES academic.sections(id),
    created_by UUID REFERENCES identity.users(id), -- admin who created it
    due_date TIMESTAMP NOT NULL,
    submission_type VARCHAR(50) DEFAULT 'text' CHECK (submission_type IN ('text', 'file', 'link', 'code')),
    max_score DECIMAL(5,2) DEFAULT 100.00,
    instructions TEXT,
    attachments JSONB NULL, -- file references
    is_active BOOLEAN DEFAULT TRUE,
    late_submission_allowed BOOLEAN DEFAULT TRUE,
    late_penalty_percentage DECIMAL(5,2) DEFAULT 10.00,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Student homework submissions
CREATE TABLE homework.submissions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    assignment_id UUID REFERENCES homework.assignments(id) ON DELETE CASCADE,
    student_id UUID REFERENCES academic.students(id) ON DELETE CASCADE,
    submission_text TEXT NULL,
    submission_files JSONB NULL,
    submission_links JSONB NULL,
    submitted_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(20) DEFAULT 'submitted' CHECK (status IN ('draft', 'submitted', 'graded', 'returned')),
    score DECIMAL(5,2) NULL,
    feedback TEXT NULL,
    graded_by UUID REFERENCES identity.users(id) NULL,
    graded_at TIMESTAMP NULL,
    is_late BOOLEAN DEFAULT FALSE,
    late_penalty_applied DECIMAL(5,2) DEFAULT 0.00,
    resubmission_count INTEGER DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(assignment_id, student_id)
);

-- Homework penalties and warnings
CREATE TABLE homework.student_penalties (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    student_id UUID REFERENCES academic.students(id) ON DELETE CASCADE,
    assignment_id UUID REFERENCES homework.assignments(id) ON DELETE CASCADE,
    penalty_type VARCHAR(50) NOT NULL, -- warning, mute, public_shame
    penalty_count INTEGER DEFAULT 1,
    penalty_duration INTEGER NULL, -- in minutes for mute
    penalty_start TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    penalty_end TIMESTAMP NULL,
    is_active BOOLEAN DEFAULT TRUE,
    reason TEXT,
    applied_by UUID REFERENCES identity.users(id),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Penalty counters for dynamic penalties
CREATE TABLE homework.student_penalty_counters (
    student_id UUID REFERENCES academic.students(id) ON DELETE CASCADE,
    section_id UUID REFERENCES academic.sections(id) ON DELETE CASCADE,
    current_counter INTEGER DEFAULT 0,
    total_warnings INTEGER DEFAULT 0,
    total_mutes INTEGER DEFAULT 0,
    total_public_shames INTEGER DEFAULT 0,
    last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (student_id, section_id)
);
5. Telegram Schema:
sql-- QR codes for telegram linking
CREATE TABLE telegram.qr_codes (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    student_id UUID REFERENCES academic.students(id) ON DELETE CASCADE,
    qr_token VARCHAR(500) UNIQUE NOT NULL, -- JWT encoded
    telegram_user_id BIGINT NULL, -- Telegram user ID after linking
    is_used BOOLEAN DEFAULT FALSE,
    expires_at TIMESTAMP NOT NULL,
    used_at TIMESTAMP NULL,
    device_info JSONB NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(qr_token),
    INDEX(telegram_user_id)
);

-- Telegram bot users
CREATE TABLE telegram.bot_users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    student_id UUID REFERENCES academic.students(id) ON DELETE CASCADE,
    telegram_user_id BIGINT UNIQUE NOT NULL,
    telegram_username VARCHAR(100) NULL,
    first_name VARCHAR(100) NULL,
    last_name VARCHAR(100) NULL,
    language_code VARCHAR(10) DEFAULT 'en',
    is_bot_admin BOOLEAN DEFAULT FALSE,
    is_muted BOOLEAN DEFAULT FALSE,
    mute_until TIMESTAMP NULL,
    joined_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_activity TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    INDEX(telegram_user_id),
    INDEX(student_id)
);

-- Bot admins (section administrators)
CREATE TABLE telegram.bot_admins (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES telegram.bot_users(id) ON DELETE CASCADE,
    section_id UUID REFERENCES academic.sections(id) ON DELETE CASCADE,
    permissions JSONB NOT NULL DEFAULT '[]', -- array of permissions
    assigned_by UUID REFERENCES identity.users(id),
    assigned_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    UNIQUE(user_id, section_id)
);

-- Telegram groups/chats
CREATE TABLE telegram.groups (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    telegram_chat_id BIGINT UNIQUE NOT NULL,
    group_name VARCHAR(255),
    group_type VARCHAR(50) DEFAULT 'group', -- group, supergroup, channel
    section_id UUID REFERENCES academic.sections(id) NULL,
    is_official BOOLEAN DEFAULT FALSE, -- official section group
    created_by UUID REFERENCES telegram.bot_users(id),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    INDEX(telegram_chat_id),
    INDEX(section_id)
);

-- Student group memberships
CREATE TABLE telegram.student_group_memberships (
    student_id UUID REFERENCES academic.students(id) ON DELETE CASCADE,
    group_id UUID REFERENCES telegram.groups(id) ON DELETE CASCADE,
    joined_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    is_active BOOLEAN DEFAULT TRUE,
    PRIMARY KEY (student_id, group_id)
);

-- Bot notifications/messages
CREATE TABLE telegram.notifications (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    message_type VARCHAR(50) NOT NULL, -- homework_warning, penalty, attendance_report
    target_type VARCHAR(50) NOT NULL, -- individual, group, section
    target_student_id UUID REFERENCES academic.students(id) NULL,
    target_group_id UUID REFERENCES telegram.groups(id) NULL,
    target_section_id UUID REFERENCES academic.sections(id) NULL,
    message_content TEXT NOT NULL,
    scheduled_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    sent_at TIMESTAMP NULL,
    status VARCHAR(20) DEFAULT 'pending' CHECK (status IN ('pending', 'sent', 'failed', 'cancelled')),
    telegram_message_id BIGINT NULL,
    failure_reason TEXT NULL,
    retry_count INTEGER DEFAULT 0,
    created_by UUID REFERENCES identity.users(id),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(scheduled_at),
    INDEX(status)
);

-- Chat history for AI analysis
CREATE TABLE telegram.chat_history (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES telegram.bot_users(id) ON DELETE CASCADE,
    message_text TEXT NOT NULL,
    message_type VARCHAR(50) DEFAULT 'text', -- text, command, photo, document
    is_from_bot BOOLEAN DEFAULT FALSE,
    telegram_message_id BIGINT NOT NULL,
    reply_to_message_id BIGINT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(user_id, created_at),
    INDEX(created_at)
);
6. System Schema:
sql-- System configuration
CREATE TABLE system.configurations (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    config_key VARCHAR(255) UNIQUE NOT NULL,
    config_value TEXT NOT NULL,
    config_type VARCHAR(50) DEFAULT 'string', -- string, number, boolean, json
    description TEXT,
    is_encrypted BOOLEAN DEFAULT FALSE,
    updated_by UUID REFERENCES identity.users(id),
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Application logs
CREATE TABLE system.application_logs (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    level VARCHAR(20) NOT NULL, -- Debug, Info, Warning, Error, Critical
    message TEXT NOT NULL,
    template TEXT NULL,
    exception TEXT NULL,
    properties JSONB NULL,
    source_context VARCHAR(255) NULL,
    user_id UUID NULL,
    request_id VARCHAR(100) NULL,
    machine_name VARCHAR(100) NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(level, created_at),
    INDEX(source_context, created_at),
    INDEX(user_id, created_at)
);

-- Audit trails
CREATE TABLE system.audit_logs (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    table_name VARCHAR(100) NOT NULL,
    operation VARCHAR(10) NOT NULL, -- INSERT, UPDATE, DELETE
    primary_key VARCHAR(100) NOT NULL,
    old_values JSONB NULL,
    new_values JSONB NULL,
    user_id UUID REFERENCES identity.users(id) NULL,
    ip_address INET NULL,
    user_agent TEXT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(table_name, created_at),
    INDEX(user_id, created_at),
    INDEX(operation, created_at)
);

-- Background jobs
CREATE TABLE system.background_jobs (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    job_type VARCHAR(100) NOT NULL,
    job_data JSONB NOT NULL,
    status VARCHAR(20) DEFAULT 'pending' CHECK (status IN ('pending', 'running', 'completed', 'failed', 'cancelled')),
    priority INTEGER DEFAULT 0,
    attempts INTEGER DEFAULT 0,
    max_attempts INTEGER DEFAULT 3,
    scheduled_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    started_at TIMESTAMP NULL,
    completed_at TIMESTAMP NULL,
    error_message TEXT NULL,
    result_data JSONB NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(status, priority, scheduled_at),
    INDEX(job_type, status)
);
7. Files Schema:
sql-- File metadata
CREATE TABLE files.file_metadata (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    original_filename VARCHAR(500) NOT NULL,
    stored_filename VARCHAR(500) UNIQUE NOT NULL,
    file_path VARCHAR(1000) NOT NULL,
    file_size BIGINT NOT NULL,
    mime_type VARCHAR(100) NOT NULL,
    file_hash VARCHAR(128) NOT NULL, -- SHA-256
    storage_provider VARCHAR(50) DEFAULT 'minio', -- minio, s3, local
    bucket_name VARCHAR(100) NULL,
    is_encrypted BOOLEAN DEFAULT FALSE,
    encryption_key_id UUID NULL,
    uploaded_by UUID REFERENCES identity.users(id),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(file_hash),
    INDEX(uploaded_by, created_at),
    INDEX(storage_provider, bucket_name)
);

-- Video recordings from cameras
CREATE TABLE files.video_recordings (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    session_id UUID REFERENCES attendance.attendance_sessions(id) ON DELETE CASCADE,
    camera_id VARCHAR(50) NOT NULL,
    file_metadata_id UUID REFERENCES files.file_metadata(id) ON DELETE CASCADE,
    start_time TIMESTAMP NOT NULL,
    end_time TIMESTAMP NOT NULL,
    duration_seconds INTEGER NOT NULL,
    resolution VARCHAR(20) NULL, -- 1920x1080
    fps INTEGER NULL,
    processing_status VARCHAR(20) DEFAULT 'pending' CHECK (processing_status IN ('pending', 'processing', 'completed', 'failed')),
    faces_detected INTEGER DEFAULT 0,
    processing_completed_at TIMESTAMP NULL,
    processing_error TEXT NULL,
    retention_until TIMESTAMP NULL, -- auto-delete date
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(session_id),
    INDEX(camera_id, start_time),
    INDEX(processing_status),
    INDEX(retention_until)
);

-- Face detection results from videos
CREATE TABLE files.face_detections (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    video_recording_id UUID REFERENCES files.video_recordings(id) ON DELETE CASCADE,
    student_id UUID REFERENCES academic.students(id) ON DELETE SET NULL,
    detection_timestamp TIMESTAMP NOT NULL, -- time within video
    confidence_score DECIMAL(5,4) NOT NULL,
    bounding_box JSONB NOT NULL, -- {x, y, width, height}
    face_encoding BYTEA NULL, -- face features for comparison
    is_verified BOOLEAN DEFAULT FALSE, -- manual verification
    verification_notes TEXT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(video_recording_id, detection_timestamp),
    INDEX(student_id, detection_timestamp),
    INDEX(confidence_score)
);
8. AI Schema:
sql-- AI models metadata
CREATE TABLE ai.models (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    model_name VARCHAR(255) NOT NULL,
    model_type VARCHAR(100) NOT NULL, -- face_recognition, chat_bot, behavior_analysis
    version VARCHAR(50) NOT NULL,
    file_path VARCHAR(1000) NOT NULL,
    model_size BIGINT NULL,
    accuracy_metrics JSONB NULL,
    training_data_info JSONB NULL,
    is_active BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deployed_at TIMESTAMP NULL,
    deprecated_at TIMESTAMP NULL,
    UNIQUE(model_name, version)
);

-- AI chat conversations
CREATE TABLE ai.chat_conversations (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES identity.users(id) ON DELETE CASCADE,
    conversation_type VARCHAR(50) DEFAULT 'general', -- general, bologna_system, behavior_analysis
    started_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ended_at TIMESTAMP NULL,
    total_messages INTEGER DEFAULT 0,
    context_data JSONB NULL, -- student data for context
    is_active BOOLEAN DEFAULT TRUE,
    INDEX(user_id, started_at),
    INDEX(conversation_type)
);

-- Individual chat messages
CREATE TABLE ai.chat_messages (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    conversation_id UUID REFERENCES ai.chat_conversations(id) ON DELETE CASCADE,
    message_text TEXT NOT NULL,
    is_from_user BOOLEAN NOT NULL,
    ai_model_used VARCHAR(255) NULL,
    processing_time_ms INTEGER NULL,
    confidence_score DECIMAL(5,4) NULL,
    intent_detected VARCHAR(100) NULL,
    entities_extracted JSONB NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(conversation_id, created_at),
    INDEX(intent_detected)
);

-- Student behavior analytics
CREATE TABLE ai.student_behavior_analytics (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    student_id UUID REFERENCES academic.students(id) ON DELETE CASCADE,
    analysis_period_start DATE NOT NULL,
    analysis_period_end DATE NOT NULL,
    attendance_pattern_score DECIMAL(5,4) DEFAULT 0.0000,
    homework_compliance_score DECIMAL(5,4) DEFAULT 0.0000,
    engagement_score DECIMAL(5,4) DEFAULT 0.0000,
    risk_level VARCHAR(20) DEFAULT 'low' CHECK (risk_level IN ('low', 'medium', 'high', 'critical')),
    recommendations JSONB NULL,
    behavioral_insights JSONB NULL,
    trend_analysis JSONB NULL,
    model_version VARCHAR(50) NOT NULL,
    calculated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(student_id, analysis_period_end),
    INDEX(risk_level),
    UNIQUE(student_id, analysis_period_start, analysis_period_end)
);
Database Indexes Strategy:
Performance Critical Indexes:
sql-- Identity performance indexes
CREATE INDEX CONCURRENTLY idx_users_email_active ON identity.users(email) WHERE is_active = true;
CREATE INDEX CONCURRENTLY idx_refresh_tokens_user_active ON identity.refresh_tokens(user_id) WHERE revoked_at IS NULL;

-- Academic performance indexes
CREATE INDEX CONCURRENTLY idx_students_section_active ON academic.students(section_id) WHERE status = 'active';
CREATE INDEX CONCURRENTLY idx_instructor_sections_year ON academic.instructor_sections(academic_year, semester);

-- Attendance performance indexes
CREATE INDEX CONCURRENTLY idx_attendance_sessions_date ON attendance.attendance_sessions(start_time, section_id);
CREATE INDEX CONCURRENTLY idx_attendance_records_session_student ON attendance.attendance_records(session_id, student_id);
CREATE INDEX CONCURRENTLY idx_attendance_statistics_student_year ON attendance.attendance_statistics(student_id, academic_year, semester);

-- Homework performance indexes
CREATE INDEX CONCURRENTLY idx_assignments_section_due ON homework.assignments(section_id, due_date) WHERE is_active = true;
CREATE INDEX CONCURRENTLY idx_submissions_assignment_student ON homework.submissions(assignment_id, student_id);
CREATE INDEX CONCURRENTLY idx_submissions_due_status ON homework.submissions(assignment_id, status, submitted_at);

-- Telegram performance indexes
CREATE INDEX CONCURRENTLY idx_bot_users_telegram_id ON telegram.bot_users(telegram_user_id) WHERE is_active = true;
CREATE INDEX CONCURRENTLY idx_notifications_scheduled ON telegram.notifications(scheduled_at, status);
CREATE INDEX CONCURRENTLY idx_chat_history_user_date ON telegram.chat_history(user_id, created_at);

-- Files performance indexes
CREATE INDEX CONCURRENTLY idx_video_recordings_session_camera ON files.video_recordings(session_id, camera_id);
CREATE INDEX CONCURRENTLY idx_face_detections_video_time ON files.face_detections(video_recording_id, detection_timestamp);
CREATE INDEX CONCURRENTLY idx_file_metadata_hash ON files.file_metadata(file_hash);

-- System performance indexes
CREATE INDEX CONCURRENTLY idx_application_logs_level_date ON system.application_logs(level, created_at);
CREATE INDEX CONCURRENTLY idx_background_jobs_status_priority ON system.background_jobs(status, priority, scheduled_at);
Database Partitioning Strategy:
Time-based Partitioning for Large Tables:
sql-- Partition attendance_records by month
CREATE TABLE attendance.attendance_records_2024_01 PARTITION OF attendance.attendance_records
FOR VALUES FROM ('2024-01-01') TO ('2024-02-01');

-- Partition application_logs by month
CREATE TABLE system.application_logs_2024_01 PARTITION OF system.application_logs
FOR VALUES FROM ('2024-01-01') TO ('2024-02-01');

-- Partition chat_history by month
CREATE TABLE telegram.chat_history_2024_01 PARTITION OF telegram.chat_history
FOR VALUES FROM ('2024-01-01') TO ('2024-02-01');

🔧 الخدمات والأنظمة الفرعية {#services}
1. Identity Service - خدمة الهوية والمصادقة
المسؤوليات:

إدارة تسجيل الدخول والخروج
إصدار وتحديث JWT tokens
إدارة Refresh tokens
التحقق من الصلاحيات والأدوار
تسجيل العمليات الأمنية
إدارة Two-Factor Authentication

التقنيات المستخدمة:
csharp// ASP.NET Core Identity + IdentityServer4
services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Lockout.MaxFailedAccessAttempts = 6;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
})
.AddEntityFrameworkStores<IdentityDbContext>()
.AddDefaultTokenProviders();

// JWT Configuration
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});
Core Services:
AuthenticationService:
csharppublic interface IAuthenticationService
{
    Task<AuthenticationResult> LoginAsync(LoginRequest request);
    Task<AuthenticationResult> RefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(string userId);
    Task<bool> RevokeAllTokensAsync(string userId);
    Task<TwoFactorResult> SendTwoFactorCodeAsync(string userId);
    Task<AuthenticationResult> VerifyTwoFactorAsync(TwoFactorRequest request);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ISecurityLogger _securityLogger;
    
    public async Task<AuthenticationResult> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            await _securityLogger.LogFailedLoginAsync(request.Email, request.IpAddress);
            return AuthenticationResult.Failed("Invalid credentials");
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            return AuthenticationResult.Failed("Account locked");
        }

        var token = await _tokenService.GenerateTokenAsync(user);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user, request.DeviceInfo);
        
        await _securityLogger.LogSuccessfulLoginAsync(user.Id, request.IpAddress);
        
        return AuthenticationResult.Success(token, refreshToken);
    }
}
TokenService:
csharppublic interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
    Task<RefreshToken> GenerateRefreshTokenAsync(ApplicationUser user, DeviceInfo deviceInfo);
    Task<bool> ValidateRefreshTokenAsync(string token);
    Task<string> RefreshAccessTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string token);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("user_type", user.UserType),
            new Claim("jti", Guid.NewGuid().ToString())
        };

        // Add role claims
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30), // Short-lived access token
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
2. Student Management Service - خدمة إدارة الطلاب
المسؤوليات:

تسجيل الطلاب الجدد
إدارة بيانات الطلاب الشخصية والأكاديمية
ربط الطلاب بالشعب والمراحل
إدارة حالة الطلاب (نشط، متوقف، متخرج)
معالجة البيانات البيومترية (Face Recognition Data)
إنشاء QR Codes للطلاب

Core Models:
csharppublic class Student
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string StudentNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FatherName { get; set; }
    public string MotherName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public Guid SectionId { get; set; }
    public Guid AcademicStageId { get; set; }
    public Guid StudyTypeId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime? GraduationDate { get; set; }
    public StudentStatus Status { get; set; }
    public decimal GPA { get; set; }
    public int TotalCredits { get; set; }
    public string Notes { get; set; }
    
    // Navigation properties
    public ApplicationUser User { get; set; }
    public Section Section { get; set; }
    public AcademicStage AcademicStage { get; set; }
    public StudyType StudyType { get; set; }
    public StudentFaceData FaceData { get; set; }
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
}

public class StudentFaceData
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public byte[] FaceEncoding { get; set; } // Encrypted face embeddings
    public int TrainingImagesCount { get; set; }
    public decimal ModelAccuracy { get; set; }
    public DateTime LastTrained { get; set; }
    public bool IsActive { get; set; }
    public string TrainingDeviceInfo { get; set; }
    
    public Student Student { get; set; }
}
Student Service:
csharppublic interface IStudentService
{
    Task<StudentDto> RegisterStudentAsync(CreateStudentRequest request);
    Task<StudentDto> UpdateStudentAsync(Guid studentId, UpdateStudentRequest request);
    Task<bool> UploadFaceDataAsync(Guid studentId, FaceDataUploadRequest request);
    Task<QrCodeDto> GenerateQrCodeAsync(Guid studentId);
    Task<StudentDto> GetStudentByIdAsync(Guid studentId);
    Task<StudentDto> GetStudentByNumberAsync(string studentNumber);
    Task<PagedResult<StudentDto>> GetStudentsBySectionAsync(Guid sectionId, PagingParameters paging);
    Task<bool> TransferStudentAsync(Guid studentId, Guid newSectionId);
    Task<bool> UpdateStudentStatusAsync(Guid studentId, StudentStatus status);
}

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IFaceRecognitionService _faceRecognitionService;
    private readonly IQrCodeService _qrCodeService;
    private readonly IMapper _mapper;
    private readonly ILogger<StudentService> _logger;

    public async Task<StudentDto> RegisterStudentAsync(CreateStudentRequest request)
    {
        // Validate section capacity
        var section = await _sectionRepository.GetByIdAsync(request.SectionId);
        if (section.CurrentStudents >= section.MaxStudents)
        {
            throw new BusinessException("Section capacity exceeded");
        }

        // Create user account
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = false,
            UserType = UserType.Student
        };

        var userResult = await _userManager.CreateAsync(user, request.Password);
        if (!userResult.Succeeded)
        {
            throw new BusinessException("Failed to create user account");
        }

        // Add student role
        await _userManager.AddToRoleAsync(user, "Student");

        // Create student record
        var student = new Student
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            StudentNumber = await GenerateStudentNumberAsync(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            SectionId = request.SectionId,
            AcademicStageId = request.AcademicStageId,
            StudyTypeId = request.StudyTypeId,
            EnrollmentDate = DateTime.UtcNow,
            Status = StudentStatus.Active
        };

        await _studentRepository.AddAsync(student);

        // Update section count
        section.CurrentStudents++;
        await _sectionRepository.UpdateAsync(section);

        _logger.LogInformation("Student registered successfully: {StudentNumber}", student.StudentNumber);
        
        return _mapper.Map<StudentDto>(student);
    }

    public async Task<bool> UploadFaceDataAsync(Guid studentId, FaceDataUploadRequest request)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
        {
            throw new NotFoundException("Student not found");
        }

        // Validate face data
        var validationResult = await _faceRecognitionService.ValidateFaceDataAsync(request.FaceEncoding);
        if (!validationResult.IsValid)
        {
            throw new BusinessException($"Invalid face data: {validationResult.Error}");
        }

        // Encrypt face encoding
        var encryptedFaceData = await _encryptionService.EncryptAsync(request.FaceEncoding);

        var faceData = new StudentFaceData
        {
            StudentId = studentId,
            FaceEncoding = encryptedFaceData,
            TrainingImagesCount = request.TrainingImagesCount,
            ModelAccuracy = request.ModelAccuracy,
            TrainingDeviceInfo = request.DeviceInfo,
            LastTrained = DateTime.UtcNow,
            IsActive = true
        };

        await _faceDataRepository.UpsertAsync(faceData);

        _logger.LogInformation("Face data uploaded for student: {StudentId}", studentId);
        
        return true;
    }
}
3. Face Recognition Service - خدمة التعرف على الوجوه
Architecture: Hybrid .NET + Python
لماذا Hybrid Architecture؟

.NET: للـ APIs، Business Logic، Database Operations
Python: للـ AI/ML Operations، Image Processing، Face Recognition

Communication Pattern:
csharp// .NET calls Python service via HTTP API
public interface IFaceRecognitionService
{
    Task<FaceRecognitionResult> ProcessVideoAsync(ProcessVideoRequest request);
    Task<ValidationResult> ValidateFaceDataAsync(byte[] faceEncoding);
    Task<IdentificationResult> IdentifyFaceAsync(byte[] faceEncoding, Guid sectionId);
    Task<bool> TrainModelAsync(Guid sectionId);
}

public class FaceRecognitionService : IFaceRecognitionService
{
    private readonly HttpClient _pythonServiceClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FaceRecognitionService> _logger;

    public async Task<FaceRecognitionResult> ProcessVideoAsync(ProcessVideoRequest request)
    {
        var pythonRequest = new
        {
            video_file_path = request.VideoFilePath,
            section_id = request.SectionId,
            session_id = request.SessionId,
            confidence_threshold = 0.8
        };

        var response = await _pythonServiceClient.PostAsJsonAsync("/api/face-recognition/process-video", pythonRequest);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Face recognition service failed: {StatusCode}", response.StatusCode);
            throw new ExternalServiceException("Face recognition service unavailable");
        }

        var result = await response.Content.ReadFromJsonAsync<FaceRecognitionResult>();
        return result;
    }
}
Python Face Recognition Service:
FastAPI Structure:
python# main.py
from fastapi import FastAPI, BackgroundTasks, HTTPException
from fastapi.middleware.cors import CORSMiddleware
import uvicorn
from services.face_recognition_service import FaceRecognitionService
from services.video_processor import VideoProcessor
from models.requests import ProcessVideoRequest, TrainModelRequest
from models.responses import FaceRecognitionResult

app = FastAPI(title="Face Recognition Service", version="1.0.0")

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

face_service = FaceRecognitionService()
video_processor = VideoProcessor()

@app.post("/api/face-recognition/process-video", response_model=FaceRecognitionResult)
async def process_video(request: ProcessVideoRequest, background_tasks: BackgroundTasks):
    try:
        result = await face_service.process_video_async(
            video_path=request.video_file_path,
            section_id=request.section_id,
            session_id=request.session_id,
            confidence_threshold=request.confidence_threshold
        )
        
        # Background cleanup
        background_tasks.add_task(video_processor.cleanup_processed_video, request.video_file_path)
        
        return result
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

@app.post("/api/face-recognition/identify-face")
async def identify_face(face_encoding: bytes, section_id: str):
    try:
        result = await face_service.identify_face_async(face_encoding, section_id)
        return result
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
Face Recognition Core Service:
python# services/face_recognition_service.py
import cv2
import face_recognition
import numpy as np
import asyncio
import logging
from typing import List, Dict, Optional
from dataclasses import dataclass
from database.connection import get_database_connection
from encryption.face_data_encryption import FaceDataEncryption

@dataclass
class FaceDetection:
    student_id: Optional[str]
    confidence: float
    timestamp: float
    bounding_box: Dict[str, int]
    face_encoding: np.ndarray

class FaceRecognitionService:
    def __init__(self):
        self.face_data_cache = {}
        self.encryption_service = FaceDataEncryption()
        self.logger = logging.getLogger(__name__)
        
    async def process_video_async(self, video_path: str, section_id: str, 
                                session_id: str, confidence_threshold: float = 0.8):
        """Process video file for face recognition"""
        
        # Load section face data
        section_face_data = await self._load_section_face_data(section_id)
        
        # Process video frames
        detections = await self._process_video_frames(video_path, section_face_data, confidence_threshold)
        
        # Aggregate results
        attendance_results = self._aggregate_detections(detections)
        
        # Save results to database
        await self._save_detection_results(session_id, detections)
        
        return {
            "session_id": session_id,
            "total_detections": len(detections),
            "unique_students": len(attendance_results),
            "attendance_results": attendance_results,
            "processing_time": self._get_processing_time()
        }
    
    async def _load_section_face_data(self, section_id: str) -> Dict[str, np.ndarray]:
        """Load and decrypt face data for section students"""
        
        # Check cache first
        cache_key = f"section_{section_id}_faces"
        if cache_key in self.face_data_cache:
            return self.face_data_cache[cache_key]
        
        # Load from database
        async with get_database_connection() as conn:
            query = """
                SELECT s.id, sfd.face_encoding 
                FROM academic.students s
                JOIN academic.student_face_data sfd ON s.id = sfd.student_id
                WHERE s.section_id = %s AND sfd.is_active = true
            """
            rows = await conn.fetch(query, section_id)
        
        face_data = {}
        for row in rows:
            # Decrypt face encoding
            decrypted_encoding = await self.encryption_service.decrypt_face_encoding(row['face_encoding'])
            face_data[row['id']] = np.frombuffer(decrypted_encoding, dtype=np.float64)
        
        # Cache for 1 hour
        self.face_data_cache[cache_key] = face_data
        asyncio.create_task(self._expire_cache_entry(cache_key, 3600))
        
        return face_data
    
    async def _process_video_frames(self, video_path: str, known_faces: Dict[str, np.ndarray], 
                                  confidence_threshold: float) -> List[FaceDetection]:
        """Process video frames and detect faces"""
        
        detections = []
        cap = cv2.VideoCapture(video_path)
        
        if not cap.isOpened():
            raise ValueError(f"Cannot open video file: {video_path}")
        
        fps = cap.get(cv2.CAP_PROP_FPS)
        frame_count = 0
        
        # Process every 2nd second (reduce processing load)
        frame_skip = int(fps * 2)
        
        while True:
            ret, frame = cap.read()
            if not ret:
                break
            
            if frame_count % frame_skip == 0:
                # Convert BGR to RGB
                rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                
                # Find faces in frame
                face_locations = face_recognition.face_locations(rgb_frame, model="hog")
                
                if face_locations:
                    face_encodings = face_recognition.face_encodings(rgb_frame, face_locations)
                    
                    for face_encoding, face_location in zip(face_encodings, face_locations):
                        # Compare with known faces
                        student_id, confidence = self._identify_face(face_encoding, known_faces, confidence_threshold)
                        
                        if student_id:
                            detection = FaceDetection(
                                student_id=student_id,
                                confidence=confidence,
                                timestamp=frame_count / fps,
                                bounding_box={
                                    "top": face_location[0],
                                    "right": face_location[1],
                                    "bottom": face_location[2],
                                    "left": face_location[3]
                                },
                                face_encoding=face_encoding
                            )
                            detections.append(detection)
            
            frame_count += 1
        
        cap.release()
        return detections
    
    def _identify_face(self, face_encoding: np.ndarray, known_faces: Dict[str, np.ndarray], 
                      threshold: float) -> tuple[Optional[str], float]:
        """Identify face against known faces"""
        
        if not known_faces:
            return None, 0.0
        
        # Calculate distances
        distances = {}
        for student_id, known_encoding in known_faces.items():
            distance = face_recognition.face_distance([known_encoding], face_encoding)[0]
            distances[student_id] = distance
        
        # Find best match
        best_student_id = min(distances, key=distances.get)
        best_distance = distances[best_student_id]
        
        # Convert distance to confidence (lower distance = higher confidence)
        confidence = 1.0 - best_distance
        
        if confidence >= threshold:
            return best_student_id, confidence
        else:
            return None, confidence
    
    def _aggregate_detections(self, detections: List[FaceDetection]) -> Dict[str, Dict]:
        """Aggregate detections per student"""
        
        results = {}
        for detection in detections:
            if detection.student_id not in results:
                results[detection.student_id] = {
                    "student_id": detection.student_id,
                    "detection_count": 0,
                    "avg_confidence": 0.0,
                    "first_detection": detection.timestamp,
                    "last_detection": detection.timestamp,
                    "status": "present"
                }
            
            student_result = results[detection.student_id]
            student_result["detection_count"] += 1
            student_result["avg_confidence"] = (
                (student_result["avg_confidence"] * (student_result["detection_count"] - 1) + detection.confidence) 
                / student_result["detection_count"]
            )
            student_result["last_detection"] = max(student_result["last_detection"], detection.timestamp)
        
        return results
4. Camera Management Service - خدمة إدارة الكاميرات
المسؤوليات:

إدارة الكاميرات المتعددة (4 كاميرات)
تنسيق جلسات التسجيل
منع التداخل بين الشعب
معالجة الفيديو في الوقت الفعلي
إدارة تخزين الفيديوهات

Camera Management Models:
csharppublic class Camera
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public CameraType Type { get; set; } // USB, IP, Integrated
    public string ConnectionString { get; set; }
    public CameraStatus Status { get; set; }
    public string Resolution { get; set; }
    public int Fps { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastHealthCheck { get; set; }
    public Dictionary<string, object> Settings { get; set; }
}

public class CameraSession
{
    public Guid Id { get; set; }
    public string CameraId { get; set; }
    public Guid AttendanceSessionId { get; set; }
    public Guid SectionId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public CameraSessionStatus Status { get; set; }
    public string VideoFilePath { get; set; }
    public long FileSizeBytes { get; set; }
    public TimeSpan Duration { get; set; }
    public int FacesDetected { get; set; }
}

public enum CameraSessionStatus
{
    Scheduled,
    Recording,
    Processing,
    Completed,
    Failed,
    Cancelled
}
Camera Service:
csharppublic interface ICameraManagementService
{
    Task<IEnumerable<Camera>> GetAvailableCamerasAsync();
    Task<CameraSession> StartRecordingAsync(StartRecordingRequest request);
    Task<bool> StopRecordingAsync(Guid sessionId);
    Task<CameraStatus> GetCameraStatusAsync(string cameraId);
    Task<bool> TestCameraConnectionAsync(string cameraId);
    Task<IEnumerable<CameraSession>> GetActiveSessions();
}

public class CameraManagementService : ICameraManagementService
{
    private readonly ICameraRepository _cameraRepository;
    private readonly ICameraSessionRepository _sessionRepository;
    private readonly IVideoProcessingService _videoProcessingService;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<CameraManagementService> _logger;
    private readonly ConcurrentDictionary<string, CameraController> _activeCameras;

    public async Task<CameraSession> StartRecordingAsync(StartRecordingRequest request)
    {
        // Validate camera availability
        var camera = await _cameraRepository.GetByIdAsync(request.CameraId);
        if (camera == null || !camera.IsActive)
        {
            throw new CameraNotAvailableException($"Camera {request.CameraId} is not available");
        }

        // Check for conflicts
        var activeSession = await _sessionRepository.GetActiveByCameraIdAsync(request.CameraId);
        if (activeSession != null)
        {
            throw new CameraInUseException($"Camera {request.CameraId} is already in use");
        }

        // Create session
        var session = new CameraSession
        {
            Id = Guid.NewGuid(),
            CameraId = request.CameraId,
            AttendanceSessionId = request.AttendanceSessionId,
            SectionId = request.SectionId,
            StartTime = DateTime.UtcNow,
            Status = CameraSessionStatus.Scheduled
        };

        await _sessionRepository.AddAsync(session);

        // Start recording asynchronously
        _taskQueue.QueueBackgroundWorkItem(async token =>
        {
            await StartRecordingInternal(session, token);
        });

        _logger.LogInformation("Recording session started: {SessionId} on camera {CameraId}", 
            session.Id, request.CameraId);

        return session;
    }

    private async Task StartRecordingInternal(CameraSession session, CancellationToken cancellationToken)
    {
        try
        {
            session.Status = CameraSessionStatus.Recording;
            await _sessionRepository.UpdateAsync(session);

            var camera = await _cameraRepository.GetByIdAsync(session.CameraId);
            var controller = _activeCameras.GetOrAdd(session.CameraId, 
                id => new CameraController(camera));

            // Generate unique filename
            var fileName = $"session_{session.Id}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.mp4";
            var filePath = Path.Combine(_configuration["Storage:VideoPath"], fileName);

            // Start recording
            var recordingTask = controller.StartRecordingAsync(filePath, cancellationToken);

            // Update session with file info
            session.VideoFilePath = filePath;
            await _sessionRepository.UpdateAsync(session);

            // Wait for recording to complete
            await recordingTask;

            // Update session status
            session.Status = CameraSessionStatus.Processing;
            session.EndTime = DateTime.UtcNow;
            session.Duration = session.EndTime.Value - session.StartTime;

            var fileInfo = new FileInfo(filePath);
            session.FileSizeBytes = fileInfo.Length;

            await _sessionRepository.UpdateAsync(session);

            // Queue for face recognition processing
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                await _videoProcessingService.ProcessVideoAsync(session.Id, token);
            });

            _logger.LogInformation("Recording completed: {SessionId}", session.Id);
        }
        catch (Exception ex)
        {
            session.Status = CameraSessionStatus.Failed;
            await _sessionRepository.UpdateAsync(session);
            
            _logger.LogError(ex, "Recording failed: {SessionId}", session.Id);
        }
    }
}

public class CameraController
{
    private readonly Camera _camera;
    private Process _ffmpegProcess;
    private readonly ILogger<CameraController> _logger;

    public async Task StartRecordingAsync(string outputPath, CancellationToken cancellationToken)
    {
        var ffmpegArgs = BuildFFmpegArguments(_camera, outputPath);
        
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = ffmpegArgs,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        _ffmpegProcess = new Process { StartInfo = processStartInfo };
        
        _ffmpegProcess.Start();

        // Monitor process
        var tcs = new TaskCompletionSource<bool>();
        
        _ffmpegProcess.Exited += (sender, args) =>
        {
            if (_ffmpegProcess.ExitCode == 0)
                tcs.SetResult(true);
            else
                tcs.SetException(new VideoRecordingException($"FFmpeg failed with exit code {_ffmpegProcess.ExitCode}"));
        };

        cancellationToken.Register(() =>
        {
            if (!_ffmpegProcess.HasExited)
            {
                _ffmpegProcess.Kill();
                tcs.SetCanceled();
            }
        });

        await tcs.Task;
    }

    private string BuildFFmpegArguments(Camera camera, string outputPath)
    {
        return camera.Type switch
        {
            CameraType.USB => $"-f v4l2 -i {camera.ConnectionString} -c:v libx264 -preset fast -crf 23 {outputPath}",
            CameraType.IP => $"-i {camera.ConnectionString} -c:v libx264 -preset fast -crf 23 {outputPath}",
            _ => throw new NotSupportedException($"Camera type {camera.Type} not supported")
        };
    }
}
5. Attendance Service - خدمة الحضور
المسؤوليات:

إدارة جلسات الحضور
تسجيل الحضور من نتائج Face Recognition
Manual Override للمحاضرين
حساب الإحصائيات
تقارير الحضور

csharppublic interface IAttendanceService
{
    Task<AttendanceSession> CreateSessionAsync(CreateAttendanceSessionRequest request);
    Task<bool> StartSessionAsync(Guid sessionId);
    Task<bool> EndSessionAsync(Guid sessionId);
    Task<AttendanceRecord> MarkAttendanceAsync(MarkAttendanceRequest request);
    Task<bool> OverrideAttendanceAsync(Guid recordId, AttendanceOverrideRequest request);
    Task<AttendanceStatistics> GetStudentStatisticsAsync(Guid studentId, string academicYear, string semester);
    Task<SectionAttendanceReport> GetSectionReportAsync(Guid sectionId, DateTime fromDate, DateTime toDate);
}

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly ICameraManagementService _cameraService;
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AttendanceService> _logger;

    public async Task<AttendanceSession> CreateSessionAsync(CreateAttendanceSessionRequest request)
    {
        // Validate instructor permissions
        var instructor = await _instructorRepository.GetByUserIdAsync(request.CreatedBy);
        if (instructor == null)
        {
            throw new UnauthorizedAccessException("Only instructors can create attendance sessions");
        }

        // Validate section assignment
        var hasPermission = await _instructorRepository.HasSectionPermissionAsync(instructor.Id, request.SectionId);
        if (!hasPermission)
        {
            throw new UnauthorizedAccessException("Instructor not assigned to this section");
        }

        var session = new AttendanceSession
        {
            Id = Guid.NewGuid(),
            SectionId = request.SectionId,
            InstructorId = instructor.Id,
            SessionName = request.SessionName ?? $"Session {DateTime.UtcNow:yyyy-MM-dd HH:mm}",
            StartTime = request.StartTime ?? DateTime.UtcNow,
            Status = AttendanceSessionStatus.Scheduled,
            AutoEnd = request.AutoEnd,
            MaxDurationMinutes = request.MaxDurationMinutes ?? 120,
            CreatedBy = request.CreatedBy
        };

        await _attendanceRepository.AddSessionAsync(session);

        _logger.LogInformation("Attendance session created: {SessionId} for section {SectionId}", 
            session.Id, request.SectionId);

        return session;
    }

    public async Task<bool> StartSessionAsync(Guid sessionId)
    {
        var session = await _attendanceRepository.GetSessionByIdAsync(sessionId);
        if (session == null)
        {
            throw new NotFoundException("Attendance session not found");
        }

        if (session.Status != AttendanceSessionStatus.Scheduled)
        {
            throw new InvalidOperationException($"Cannot start session in {session.Status} status");
        }

        // Initialize attendance records for all students in section
        var students = await _studentRepository.GetActiveBySectionIdAsync(session.SectionId);
        var attendanceRecords = students.Select(student => new AttendanceRecord
        {
            Id = Guid.NewGuid(),
            SessionId = sessionId,
            StudentId = student.Id,
            Status = AttendanceStatus.Absent, // Default to absent
            DetectionMethod = DetectionMethod.FaceRecognition
        }).ToList();

        await _attendanceRepository.AddRecordsAsync(attendanceRecords);

        // Start camera recording
        var availableCameras = await _cameraService.GetAvailableCamerasAsync();
        var cameraIds = new List<string>();

        foreach (var camera in availableCameras.Take(4)) // Use up to 4 cameras
        {
            try
            {
                var cameraSession = await _cameraService.StartRecordingAsync(new StartRecordingRequest
                {
                    CameraId = camera.Id,
                    AttendanceSessionId = sessionId,
                    SectionId = session.SectionId
                });
                cameraIds.Add(camera.Id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to start camera {CameraId}", camera.Id);
            }
        }

        // Update session
        session.Status = AttendanceSessionStatus.Active;
        session.StartTime = DateTime.UtcNow;
        session.CameraIds = cameraIds.ToArray();

        await _attendanceRepository.UpdateSessionAsync(session);

        // Schedule auto-end if enabled
        if (session.AutoEnd && session.MaxDurationMinutes.HasValue)
        {
            _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
            {
                await Task.Delay(TimeSpan.FromMinutes(session.MaxDurationMinutes.Value), token);
                await EndSessionAsync(sessionId);
            });
        }

        _logger.LogInformation("Attendance session started: {SessionId} with {CameraCount} cameras", 
            sessionId, cameraIds.Count);

        return true;
    }

    public async Task<AttendanceRecord> MarkAttendanceAsync(MarkAttendanceRequest request)
    {
        var record = await _attendanceRepository.GetRecordAsync(request.SessionId, request.StudentId);
        if (record == null)
        {
            throw new NotFoundException("Attendance record not found");
        }

        // Update attendance based on face recognition result
        if (request.ConfidenceScore >= 0.8) // High confidence threshold
        {
            record.Status = AttendanceStatus.Present;
            record.DetectedAt = request.DetectionTime;
            record.ConfidenceScore = request.ConfidenceScore;
            record.CameraId = request.CameraId;
        }

        await _attendanceRepository.UpdateRecordAsync(record);

        // Update statistics asynchronously
        _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
        {
            await UpdateStudentStatisticsAsync(request.StudentId, record.SessionId);
        });

        return record;
    }

    public async Task<bool> OverrideAttendanceAsync(Guid recordId, AttendanceOverrideRequest request)
    {
        var record = await _attendanceRepository.GetRecordByIdAsync(recordId);
        if (record == null)
        {
            throw new NotFoundException("Attendance record not found");
        }

        // Validate instructor permission
        var session = await _attendanceRepository.GetSessionByIdAsync(record.SessionId);
        var hasPermission = await _instructorRepository.HasSectionPermissionAsync(request.InstructorId, session.SectionId);
        
        if (!hasPermission)
        {
            throw new UnauthorizedAccessException("Instructor not authorized to modify this attendance");
        }

        record.Status = request.NewStatus;
        record.ManualOverride = true;
        record.OverrideReason = request.Reason;
        record.OverrideBy = request.InstructorId;
        record.OverrideAt = DateTime.UtcNow;

        await _attendanceRepository.UpdateRecordAsync(record);

        _logger.LogInformation("Attendance overridden: Record {RecordId} changed to {Status} by instructor {InstructorId}", 
            recordId, request.NewStatus, request.InstructorId);

        return true;
    }
}

🔒 نظام الأمان والحماية {#security}
1. Authentication & Authorization Strategy
Multi-layered Security Architecture:
csharp// Startup.cs - Security Configuration
public void ConfigureServices(IServiceCollection services)
{
    // Identity Configuration
    services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        // Password policy
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        
        // Lockout policy
        options.Lockout.MaxFailedAccessAttempts = 6;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.AllowedForNewUsers = true;
        
        // User policy
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
        
        // Two-factor authentication
        options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    })
    .AddEntityFrameworkStores<SecurityDbContext>()
    .AddDefaultTokenProviders();

    // JWT Configuration
    var jwtSettings = Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                // Additional token validation
                var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
                var isValid = await tokenService.IsTokenValidAsync(context.SecurityToken.RawData);
                
                if (!isValid)
                {
                    context.Fail("Token has been revoked");
                }
            },
            OnAuthenticationFailed = context =>
            {
                // Log authentication failures
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogWarning("JWT Authentication failed: {Error}", context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

    // Authorization Policies
    services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdministrator", policy => 
            policy.RequireRole("Administrator"));
        
        options.AddPolicy("RequireInstructor", policy => 
            policy.RequireRole("Instructor", "Administrator"));
        
        options.AddPolicy("RequireStudent", policy => 
            policy.RequireRole("Student", "Instructor", "Administrator"));
        
        options.AddPolicy("SectionAccess", policy =>
            policy.Requirements.Add(new SectionAccessRequirement()));
        
        options.AddPolicy("CameraControl", policy =>
            policy.Requirements.Add(new CameraControlRequirement()));
    });

    // Rate Limiting
    services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
    services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
    services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    services.AddInMemoryRateLimiting();

    // Security Headers
    services.AddHsts(options =>
    {
        options.Preload = true;
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromDays(365);
    });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Security middleware order is important
    app.UseSecurityHeaders(policies =>
    {
        policies.AddFrameOptionsDeny()
                .AddXssProtectionBlock()
                .AddContentTypeOptionsNoSniff()
                .AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365)
                .AddReferrerPolicyStrictOriginWhenCrossOrigin()
                .RemoveServerHeader()
                .AddContentSecurityPolicy(builder =>
                {
                    builder.AddObjectSrc().None();
                    builder.AddFormAction().Self();
                    builder.AddFrameAncestors().None();
                });
    });

    app.UseIpRateLimiting();
    app.UseAuthentication();
    app.UseAuthorization();
}
2. Data Encryption Strategy
Encryption Service:
csharppublic interface IEncryptionService
{
    Task<string> EncryptAsync(string plainText);
    Task<string> DecryptAsync(string cipherText);
    Task<byte[]> EncryptBytesAsync(byte[] plainBytes);
    Task<byte[]> DecryptBytesAsync(byte[] cipherBytes);
    Task<string> HashPasswordAsync(string password, string salt);
    Task<bool> VerifyPasswordAsync(string password, string hash, string salt);
}

public class EncryptionService : IEncryptionService
{
    private readonly IConfiguration _configuration;
    private readonly IKeyManagementService _keyManagement;
    private readonly ILogger<EncryptionService> _logger;

    public async Task<byte[]> EncryptBytesAsync(byte[] plainBytes)
    {
        var key = await _keyManagement.GetEncryptionKeyAsync();
        
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.GenerateIV();
            
            using (var encryptor = aes.CreateEncryptor())
            using (var msEncrypt = new MemoryStream())
            {
                // Prepend IV to encrypted data
                msEncrypt.Write(aes.IV, 0, aes.IV.Length);
                
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    csEncrypt.FlushFinalBlock();
                }
                
                return msEncrypt.ToArray();
            }
        }
    }

    public async Task<byte[]> DecryptBytesAsync(byte[] cipherBytes)
    {
        var key = await _keyManagement.GetEncryptionKeyAsync();
        
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            
            // Extract IV from the beginning of cipher bytes
            var iv = new byte[aes.BlockSize / 8];
            Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
            aes.IV = iv;
            
            using (var decryptor = aes.CreateDecryptor())
            using (var msDecrypt = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var msPlain = new MemoryStream())
            {
                csDecrypt.CopyTo(msPlain);
                return msPlain.ToArray();
            }
        }
    }

    public async Task<string> HashPasswordAsync(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        
        using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
        {
            argon2.Salt = saltBytes;
            argon2.DegreeOfParallelism = 8;
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 1024; // 1 GB
            
            var hashBytes = argon2.GetBytes(32);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
3. QR Code Security
Secure QR Code Implementation:
csharppublic interface IQrCodeService
{
    Task<QrCodeDto> GenerateSecureQrCodeAsync(Guid studentId);
    Task<QrValidationResult> ValidateQrCodeAsync(string qrToken);
    Task<bool> MarkQrCodeAsUsedAsync(string qrToken, long telegramUserId);
}

public class QrCodeService : IQrCodeService
{
    private readonly ITokenService _tokenService;
    private readonly IQrCodeRepository _qrCodeRepository;
    private readonly IConfiguration _configuration;

    public async Task<QrCodeDto> GenerateSecureQrCodeAsync(Guid studentId)
    {
        // Create JWT token with student info and expiration
        var claims = new[]
        {
            new Claim("student_id", studentId.ToString()),
            new Claim("purpose", "telegram_linking"),
            new Claim("jti", Guid.NewGuid().ToString()),
            new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = _tokenService.GenerateToken(claims, TimeSpan.FromMinutes(15)); // 15 minutes expiry

        var qrCode = new QrCode
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            QrToken = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            IsUsed = false
        };

        await _qrCodeRepository.AddAsync(qrCode);

        // Generate QR code image
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(token, QRCodeGenerator.ECCLevel.Q);
        var qrCodeImage = new PngByteQRCode(qrCodeData);
        var qrCodeBytes = qrCodeImage.GetGraphic(20);

        return new QrCodeDtoRetryااكملEdit2 / 2أكمل مشروع نظام الحضور الذكي بالتفصيل3sالمستخدم يطلب إكمال كتابة ملف markdown الذي كنت أكتبه عن نظام الحضور الذكي. كان آخر ما كتبته هو QR Code Security implementation ولم أكمل الكود، ولم أكمل الأقسام الأخرى التي وعدت بكتابتها في جدول المحتويات.
يجب أن أكمل:

إنهاء QR Code Service
النظام الكامل للـ Telegram Bot
إدارة الملفات والفيديو
نظام الذكاء الاصطناعي
المراقبة والأداء
خطة التنفيذ

المستخدم طلب 5000-10000 سطر، وما كتبته حتى الآن لا يزال أقل من ذلك، لذا يجب أن أكمل بالتفصيل المطلوب.csharp        return new QrCodeDto
        {
            Id = qrCode.Id,
            StudentId = studentId,
            QrToken = token,
            QrCodeImage = Convert.ToBase64String(qrCodeBytes),
            ExpiresAt = qrCode.ExpiresAt,
            Instructions = "Scan this QR code in the Telegram bot to link your account. This code expires in 15 minutes."
        };
    }

    public async Task<QrValidationResult> ValidateQrCodeAsync(string qrToken)
    {
        try
        {
            // Validate JWT token structure
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _tokenService.GetSigningKey(),
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(qrToken, validationParameters, out var validatedToken);
            
            // Extract student ID from claims
            var studentIdClaim = principal.FindFirst("student_id");
            if (studentIdClaim == null || !Guid.TryParse(studentIdClaim.Value, out var studentId))
            {
                return QrValidationResult.Invalid("Invalid student ID in token");
            }

            // Check database record
            var qrCode = await _qrCodeRepository.GetByTokenAsync(qrToken);
            if (qrCode == null)
            {
                return QrValidationResult.Invalid("QR code not found");
            }

            if (qrCode.IsUsed)
            {
                return QrValidationResult.Invalid("QR code already used");
            }

            if (qrCode.ExpiresAt < DateTime.UtcNow)
            {
                return QrValidationResult.Invalid("QR code expired");
            }

            return QrValidationResult.Valid(qrCode.StudentId);
        }
        catch (SecurityTokenExpiredException)
        {
            return QrValidationResult.Invalid("QR code expired");
        }
        catch (SecurityTokenException ex)
        {
            return QrValidationResult.Invalid($"Invalid token: {ex.Message}");
        }
    }

    public async Task<bool> MarkQrCodeAsUsedAsync(string qrToken, long telegramUserId)
    {
        var qrCode = await _qrCodeRepository.GetByTokenAsync(qrToken);
        if (qrCode == null)
        {
            return false;
        }

        qrCode.IsUsed = true;
        qrCode.TelegramUserId = telegramUserId;
        qrCode.UsedAt = DateTime.UtcNow;

        await _qrCodeRepository.UpdateAsync(qrCode);
        return true;
    }
}
4. Rate Limiting & DDoS Protection
Advanced Rate Limiting:
csharppublic class CustomRateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CustomRateLimitingMiddleware> _logger;
    private readonly RateLimitOptions _options;

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var rateLimitAttribute = endpoint?.Metadata.GetMetadata<RateLimitAttribute>();
        
        if (rateLimitAttribute != null)
        {
            var clientId = GetClientIdentifier(context);
            var key = $"rate_limit:{rateLimitAttribute.Policy}:{clientId}";
            
            var requests = _cache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return new RateLimitCounter();
            });

            if (requests.Count >= rateLimitAttribute.Limit)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Rate limit exceeded");
                
                _logger.LogWarning("Rate limit exceeded for client {ClientId} on endpoint {Endpoint}", 
                    clientId, context.Request.Path);
                return;
            }

            requests.Count++;
        }

        await _next(context);
    }

    private string GetClientIdentifier(HttpContext context)
    {
        // Priority: User ID > API Key > IP Address
        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
            return $"user:{userId}";

        var apiKey = context.Request.Headers["X-API-Key"].FirstOrDefault();
        if (!string.IsNullOrEmpty(apiKey))
            return $"api:{apiKey}";

        return $"ip:{context.Connection.RemoteIpAddress}";
    }
}

// Usage in controllers
[RateLimit(Policy = "face_recognition", Limit = 10)] // 10 requests per minute
[HttpPost("process-video")]
public async Task<IActionResult> ProcessVideo([FromBody] ProcessVideoRequest request)
{
    // Implementation
}

[RateLimit(Policy = "authentication", Limit = 6)] // 6 login attempts per minute
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    // Implementation
}

🤖 نظام الذكاء الاصطناعي {#ai}
1. AI Chat Service Architecture
Hybrid Implementation: .NET + Python ML Services
Master AI Service (C#):
csharppublic interface IAiChatService
{
    Task<ChatResponse> ProcessMessageAsync(ChatRequest request);
    Task<StudentAnalysis> AnalyzeStudentBehaviorAsync(Guid studentId, DateTime fromDate, DateTime toDate);
    Task<IEnumerable<Recommendation>> GetStudentRecommendationsAsync(Guid studentId);
    Task<BolognaSystemResponse> QueryBolognaSystemAsync(BolognaSystemQuery query);
}

public class AiChatService : IAiChatService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAiModelRepository _modelRepository;
    private readonly IChatHistoryRepository _chatHistoryRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AiChatService> _logger;

    public async Task<ChatResponse> ProcessMessageAsync(ChatRequest request)
    {
        // Save user message
        var conversation = await GetOrCreateConversationAsync(request.UserId, request.ConversationType);
        
        var userMessage = new ChatMessage
        {
            ConversationId = conversation.Id,
            MessageText = request.Message,
            IsFromUser = true,
            CreatedAt = DateTime.UtcNow
        };
        
        await _chatHistoryRepository.AddMessageAsync(userMessage);

        // Determine intent and route to appropriate AI service
        var intent = await DetermineIntentAsync(request.Message, conversation.ConversationType);
        
        ChatResponse response = intent.Category switch
        {
            IntentCategory.BolognaSystem => await ProcessBolognaQueryAsync(request, conversation),
            IntentCategory.AttendanceInquiry => await ProcessAttendanceQueryAsync(request, conversation),
            IntentCategory.BehaviorAnalysis => await ProcessBehaviorAnalysisAsync(request, conversation),
            IntentCategory.GeneralSupport => await ProcessGeneralSupportAsync(request, conversation),
            _ => new ChatResponse { Message = "I'm sorry, I didn't understand your question. Could you please rephrase it?" }
        };

        // Save AI response
        var aiMessage = new ChatMessage
        {
            ConversationId = conversation.Id,
            MessageText = response.Message,
            IsFromUser = false,
            AiModelUsed = response.ModelUsed,
            ProcessingTimeMs = response.ProcessingTime,
            ConfidenceScore = response.Confidence,
            IntentDetected = intent.Category.ToString(),
            EntitiesExtracted = JsonSerializer.Serialize(intent.Entities),
            CreatedAt = DateTime.UtcNow
        };

        await _chatHistoryRepository.AddMessageAsync(aiMessage);

        return response;
    }

    private async Task<ChatResponse> ProcessBolognaQueryAsync(ChatRequest request, Conversation conversation)
    {
        var httpClient = _httpClientFactory.CreateClient("BolognaAI");
        
        var bolognaRequest = new
        {
            query = request.Message,
            conversation_id = conversation.Id.ToString(),
            user_context = await GetUserContextAsync(request.UserId)
        };

        var response = await httpClient.PostAsJsonAsync("/api/bologna/query", bolognaRequest);
        var result = await response.Content.ReadFromJsonAsync<BolognaAIResponse>();

        return new ChatResponse
        {
            Message = result.Response,
            ModelUsed = "bologna_specialist_v1.0",
            ProcessingTime = result.ProcessingTime,
            Confidence = result.Confidence,
            SuggestedActions = result.SuggestedActions,
            References = result.References
        };
    }

    private async Task<ChatResponse> ProcessBehaviorAnalysisAsync(ChatRequest request, Conversation conversation)
    {
        var httpClient = _httpClientFactory.CreateClient("BehaviorAI");
        
        // Get student data for analysis
        var student = await _studentRepository.GetByUserIdAsync(request.UserId);
        if (student == null)
        {
            return new ChatResponse 
            { 
                Message = "I need your student information to provide behavioral analysis. Please contact your administrator." 
            };
        }

        var analysisRequest = new
        {
            student_id = student.Id.ToString(),
            query = request.Message,
            analysis_period_days = 30
        };

        var response = await httpClient.PostAsJsonAsync("/api/behavior/analyze", analysisRequest);
        var result = await response.Content.ReadFromJsonAsync<BehaviorAnalysisResponse>();

        return new ChatResponse
        {
            Message = result.Analysis,
            ModelUsed = "behavior_analyst_v2.1",
            ProcessingTime = result.ProcessingTime,
            Confidence = result.Confidence,
            Recommendations = result.Recommendations,
            Charts = result.VisualizationData
        };
    }
}
2. Bologna System AI Specialist (Python)
Bologna System Knowledge Base:
python# bologna_ai_service.py
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import List, Optional, Dict
import openai
from langchain.vectorstores import Chroma
from langchain.embeddings import OpenAIEmbeddings
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain.llms import OpenAI
from langchain.chains import RetrievalQA
from langchain.document_loaders import TextLoader
import logging

app = FastAPI(title="Bologna System AI Specialist")

class BolognaQuery(BaseModel):
    query: str
    conversation_id: str
    user_context: Optional[Dict] = None

class BolognaResponse(BaseModel):
    response: str
    confidence: float
    processing_time: int
    suggested_actions: List[str]
    references: List[str]

class BolognaSystemAI:
    def __init__(self):
        self.embeddings = OpenAIEmbeddings()
        self.vectorstore = None
        self.qa_chain = None
        self.logger = logging.getLogger(__name__)
        self._initialize_knowledge_base()
    
    def _initialize_knowledge_base(self):
        """Initialize the Bologna system knowledge base"""
        
        # Load Bologna system documents
        documents = self._load_bologna_documents()
        
        # Split documents into chunks
        text_splitter = RecursiveCharacterTextSplitter(
            chunk_size=1000,
            chunk_overlap=200,
            length_function=len
        )
        texts = text_splitter.split_documents(documents)
        
        # Create vector store
        self.vectorstore = Chroma.from_documents(
            documents=texts,
            embedding=self.embeddings,
            persist_directory="./bologna_vectorstore"
        )
        
        # Create QA chain
        self.qa_chain = RetrievalQA.from_chain_type(
            llm=OpenAI(temperature=0.2),
            chain_type="stuff",
            retriever=self.vectorstore.as_retriever(search_kwargs={"k": 3}),
            return_source_documents=True
        )
    
    def _load_bologna_documents(self):
        """Load Bologna system documentation"""
        
        bologna_content = [
            {
                "title": "نظام بولونيا - مقدمة",
                "content": """
                نظام بولونيا هو نظام تعليمي أوروبي يهدف إلى توحيد معايير التعليم العالي.
                يعتمد على نظام النقاط الأوروبية (ECTS) ويركز على:
                - نتائج التعلم المحددة
                - التقييم المستمر
                - الشفافية في المناهج
                - قابلية النقل بين الجامعات
                """
            },
            {
                "title": "نظام النقاط والدرجات",
                "content": """
                في نظام بولونيا:
                - كل مقرر له عدد محدد من النقاط (Credits)
                - التقدير يعتمد على الأداء النسبي
                - A: 90-100% (ممتاز)
                - B: 80-89% (جيد جداً)
                - C: 70-79% (جيد)
                - D: 60-69% (مقبول)
                - F: أقل من 60% (راسب)
                """
            },
            {
                "title": "متطلبات الحضور",
                "content": """
                متطلبات الحضور في نظام بولونيا:
                - الحد الأدنى للحضور 75%
                - أكثر من 25% غياب = منع من الامتحان
                - الغياب المبرر لا يحتسب ضمن النسبة
                - يمكن للطالب تعويض الغياب بأنشطة إضافية
                """
            },
            {
                "title": "التقييم المستمر",
                "content": """
                التقييم في نظام بولونيا يشمل:
                - امتحانات دورية (30-40%)
                - واجبات ومشاريع (20-30%)
                - مشاركة ونشاط (10-20%)
                - امتحان نهائي (30-50%)
                الهدف هو تقييم شامل لأداء الطالب
                """
            },
            {
                "title": "نتائج التعلم",
                "content": """
                نتائج التعلم المحددة تشمل:
                - المعرفة والفهم
                - المهارات الذهنية
                - المهارات العملية
                - المهارات القابلة للنقل
                كل مقرر يجب أن يحدد نتائج التعلم المتوقعة بوضوح
                """
            }
        ]
        
        documents = []
        for item in bologna_content:
            doc = Document(
                page_content=item["content"],
                metadata={"title": item["title"], "source": "bologna_system"}
            )
            documents.append(doc)
        
        return documents
    
    async def process_query(self, query: BolognaQuery) -> BolognaResponse:
        """Process Bologna system query"""
        
        start_time = time.time()
        
        try:
            # Enhance query with context
            enhanced_query = self._enhance_query_with_context(query.query, query.user_context)
            
            # Get response from QA chain
            result = self.qa_chain({"query": enhanced_query})
            
            # Extract source references
            references = []
            if "source_documents" in result:
                references = [doc.metadata.get("title", "Unknown") for doc in result["source_documents"]]
            
            # Calculate confidence based on source relevance
            confidence = self._calculate_confidence(result)
            
            # Generate suggested actions
            suggested_actions = self._generate_suggested_actions(query.query, result["result"])
            
            processing_time = int((time.time() - start_time) * 1000)
            
            return BolognaResponse(
                response=result["result"],
                confidence=confidence,
                processing_time=processing_time,
                suggested_actions=suggested_actions,
                references=references
            )
            
        except Exception as e:
            self.logger.error(f"Error processing Bologna query: {str(e)}")
            raise HTTPException(status_code=500, detail="Error processing query")
    
    def _enhance_query_with_context(self, query: str, context: Optional[Dict]) -> str:
        """Enhance query with user context"""
        
        if not context:
            return query
        
        enhanced = f"السؤال: {query}\n\n"
        
        if context.get("student_stage"):
            enhanced += f"مرحلة الطالب: {context['student_stage']}\n"
        
        if context.get("study_type"):
            enhanced += f"نوع الدراسة: {context['study_type']}\n"
        
        if context.get("current_gpa"):
            enhanced += f"المعدل الحالي: {context['current_gpa']}\n"
        
        enhanced += f"\nيرجى الإجابة بناءً على نظام بولونيا مع مراعاة سياق الطالب."
        
        return enhanced
    
    def _calculate_confidence(self, result: Dict) -> float:
        """Calculate confidence score based on result quality"""
        
        confidence = 0.5  # Base confidence
        
        # Check if we have source documents
        if "source_documents" in result and result["source_documents"]:
            confidence += 0.3
        
        # Check response length (longer responses might be more detailed)
        response_length = len(result["result"])
        if response_length > 100:
            confidence += 0.1
        if response_length > 200:
            confidence += 0.1
        
        return min(confidence, 1.0)
    
    def _generate_suggested_actions(self, query: str, response: str) -> List[str]:
        """Generate suggested follow-up actions"""
        
        actions = []
        
        query_lower = query.lower()
        
        if "حضور" in query_lower or "غياب" in query_lower:
            actions.extend([
                "تحقق من سجل حضورك الحالي",
                "راجع أسباب الغياب المبررة",
                "تواصل مع مكتب الشؤون الأكاديمية"
            ])
        
        if "درجة" in query_lower or "تقدير" in query_lower:
            actions.extend([
                "راجع نتائج الامتحانات الدورية",
                "تحقق من الواجبات المطلوبة",
                "احسب نقاطك الحالية"
            ])
        
        if "مقرر" in query_lower or "مادة" in query_lower:
            actions.extend([
                "راجع نتائج التعلم للمقرر",
                "تحقق من متطلبات النجاح",
                "راجع جدول التقييم"
            ])
        
        return actions[:3]  # Return max 3 suggestions

# Initialize the Bologna AI service
bologna_ai = BolognaSystemAI()

@app.post("/api/bologna/query", response_model=BolognaResponse)
async def query_bologna_system(query: BolognaQuery):
    """Process Bologna system query"""
    return await bologna_ai.process_query(query)

@app.get("/health")
async def health_check():
    return {"status": "healthy", "service": "Bologna AI Specialist"}
3. Student Behavior Analysis AI (Python)
python# behavior_analysis_service.py
import pandas as pd
import numpy as np
from sklearn.ensemble import IsolationForest
from sklearn.preprocessing import StandardScaler
from sklearn.cluster import KMeans
import matplotlib.pyplot as plt
import seaborn as sns
import io
import base64
from datetime import datetime, timedelta
import asyncpg
import json

class StudentBehaviorAnalyzer:
    def __init__(self):
        self.scaler = StandardScaler()
        self.isolation_forest = IsolationForest(contamination=0.1, random_state=42)
        self.kmeans = KMeans(n_clusters=4, random_state=42)
        
    async def analyze_student_behavior(self, student_id: str, analysis_period_days: int = 30) -> Dict:
        """Comprehensive student behavior analysis"""
        
        # Fetch student data
        student_data = await self._fetch_student_data(student_id, analysis_period_days)
        
        if not student_data:
            return {"error": "Insufficient data for analysis"}
        
        # Perform different types of analysis
        attendance_analysis = self._analyze_attendance_patterns(student_data['attendance'])
        homework_analysis = self._analyze_homework_patterns(student_data['homework'])
        engagement_analysis = self._analyze_engagement_patterns(student_data['engagement'])
        risk_assessment = self._assess_risk_level(attendance_analysis, homework_analysis, engagement_analysis)
        
        # Generate recommendations
        recommendations = self._generate_recommendations(risk_assessment, attendance_analysis, homework_analysis)
        
        # Create visualizations
        charts = self._create_visualizations(student_data, attendance_analysis, homework_analysis)
        
        return {
            "student_id": student_id,
            "analysis_period": analysis_period_days,
            "attendance_score": attendance_analysis['score'],
            "homework_score": homework_analysis['score'],
            "engagement_score": engagement_analysis['score'],
            "overall_risk_level": risk_assessment['level'],
            "risk_factors": risk_assessment['factors'],
            "recommendations": recommendations,
            "detailed_insights": {
                "attendance": attendance_analysis,
                "homework": homework_analysis,
                "engagement": engagement_analysis
            },
            "visualizations": charts,
            "generated_at": datetime.utcnow().isoformat()
        }
    
    async def _fetch_student_data(self, student_id: str, period_days: int) -> Dict:
        """Fetch comprehensive student data for analysis"""
        
        end_date = datetime.utcnow()
        start_date = end_date - timedelta(days=period_days)
        
        conn = await asyncpg.connect("postgresql://user:pass@localhost/db")
        
        try:
            # Attendance data
            attendance_query = """
                SELECT ar.status, ar.detected_at, ar.confidence_score, ar.manual_override,
                       ases.start_time, ases.end_time
                FROM attendance.attendance_records ar
                JOIN attendance.attendance_sessions ases ON ar.session_id = ases.id
                WHERE ar.student_id = $1 
                AND ases.start_time >= $2 
                AND ases.start_time <= $3
                ORDER BY ases.start_time
            """
            attendance_records = await conn.fetch(attendance_query, student_id, start_date, end_date)
            
            # Homework data
            homework_query = """
                SELECT ha.title, ha.due_date, hs.submitted_at, hs.is_late, 
                       hs.score, hs.status, ha.max_score
                FROM homework.assignments ha
                LEFT JOIN homework.submissions hs ON ha.id = hs.assignment_id AND hs.student_id = $1
                JOIN academic.sections sec ON ha.section_id = sec.id
                JOIN academic.students s ON s.section_id = sec.id
                WHERE s.id = $1 
                AND ha.due_date >= $2 
                AND ha.due_date <= $3
                ORDER BY ha.due_date
            """
            homework_records = await conn.fetch(homework_query, student_id, start_date, end_date)
            
            # Engagement data (chat interactions, bot usage)
            engagement_query = """
                SELECT ch.message_text, ch.is_from_bot, ch.created_at,
                       cc.conversation_type
                FROM telegram.chat_history ch
                JOIN ai.chat_conversations cc ON ch.conversation_id = cc.id
                JOIN telegram.bot_users bu ON cc.user_id = bu.id
                JOIN academic.students s ON bu.student_id = s.id
                WHERE s.id = $1 
                AND ch.created_at >= $2 
                AND ch.created_at <= $3
                ORDER BY ch.created_at
            """
            engagement_records = await conn.fetch(engagement_query, student_id, start_date, end_date)
            
            return {
                "attendance": [dict(record) for record in attendance_records],
                "homework": [dict(record) for record in homework_records],
                "engagement": [dict(record) for record in engagement_records]
            }
            
        finally:
            await conn.close()
    
    def _analyze_attendance_patterns(self, attendance_data: List[Dict]) -> Dict:
        """Analyze attendance patterns and trends"""
        
        if not attendance_data:
            return {"score": 0.0, "pattern": "insufficient_data"}
        
        df = pd.DataFrame(attendance_data)
        
        # Calculate basic metrics
        total_sessions = len(df)
        present_sessions = len(df[df['status'] == 'present'])
        absent_sessions = len(df[df['status'] == 'absent'])
        late_sessions = len(df[df['status'] == 'late'])
        
        attendance_rate = present_sessions / total_sessions if total_sessions > 0 else 0
        
        # Analyze patterns
        df['start_time'] = pd.to_datetime(df['start_time'])
        df['weekday'] = df['start_time'].dt.dayofweek
        df['hour'] = df['start_time'].dt.hour
        
        # Weekly pattern analysis
        weekly_attendance = df.groupby('weekday')['status'].apply(
            lambda x: (x == 'present').sum() / len(x) if len(x) > 0 else 0
        ).to_dict()
        
        # Time-based pattern analysis
        hourly_attendance = df.groupby('hour')['status'].apply(
            lambda x: (x == 'present').sum() / len(x) if len(x) > 0 else 0
        ).to_dict()
        
        # Trend analysis (last 7 days vs previous 7 days)
        df_sorted = df.sort_values('start_time')
        if len(df_sorted) >= 14:
            recent_half = df_sorted.tail(7)
            previous_half = df_sorted.iloc[-14:-7]
            
            recent_rate = (recent_half['status'] == 'present').sum() / len(recent_half)
            previous_rate = (previous_half['status'] == 'present').sum() / len(previous_half)
            trend = "improving" if recent_rate > previous_rate else "declining" if recent_rate < previous_rate else "stable"
        else:
            trend = "insufficient_data"
        
        # Calculate confidence scores
        confidence_scores = [r.get('confidence_score', 0) for r in attendance_data if r.get('confidence_score')]
        avg_confidence = np.mean(confidence_scores) if confidence_scores else 0
        
        # Risk indicators
        risk_indicators = []
        if attendance_rate < 0.75:
            risk_indicators.append("low_attendance_rate")
        if absent_sessions > 3:
            risk_indicators.append("frequent_absences")
        if trend == "declining":
            risk_indicators.append("declining_trend")
        if avg_confidence < 0.8:
            risk_indicators.append("low_recognition_confidence")
        
        return {
            "score": attendance_rate,
            "total_sessions": total_sessions,
            "present_sessions": present_sessions,
            "absent_sessions": absent_sessions,
            "late_sessions": late_sessions,
            "weekly_pattern": weekly_attendance,
            "hourly_pattern": hourly_attendance,
            "trend": trend,
            "avg_confidence": avg_confidence,
            "risk_indicators": risk_indicators
        }
    
    def _analyze_homework_patterns(self, homework_data: List[Dict]) -> Dict:
        """Analyze homework submission patterns"""
        
        if not homework_data:
            return {"score": 0.0, "pattern": "insufficient_data"}
        
        df = pd.DataFrame(homework_data)
        
        # Calculate basic metrics
        total_assignments = len(df)
        submitted_assignments = len(df[df['submitted_at'].notna()])
        late_submissions = len(df[df['is_late'] == True])
        
        submission_rate = submitted_assignments / total_assignments if total_assignments > 0 else 0
        on_time_rate = (submitted_assignments - late_submissions) / total_assignments if total_assignments > 0 else 0
        
        # Score analysis
        scored_assignments = df[df['score'].notna()]
        if len(scored_assignments) > 0:
            avg_score = scored_assignments['score'].mean()
            max_possible = scored_assignments['max_score'].mean()
            score_percentage = avg_score / max_possible if max_possible > 0 else 0
        else:
            avg_score = 0
            score_percentage = 0
        
        # Submission timing analysis
        df['due_date'] = pd.to_datetime(df['due_date'])
        df['submitted_at'] = pd.to_datetime(df['submitted_at'])
        
        # Calculate submission timing (hours before/after due date)
        submitted_df = df[df['submitted_at'].notna()].copy()
        if len(submitted_df) > 0:
            submitted_df['timing'] = (submitted_df['due_date'] - submitted_df['submitted_at']).dt.total_seconds() / 3600
            avg_submission_timing = submitted_df['timing'].mean()  # Positive = early, Negative = late
        else:
            avg_submission_timing = 0
        
        # Trend analysis
        if len(df) >= 4:
            recent_assignments = df.tail(2)
            previous_assignments = df.iloc[-4:-2] if len(df) >= 4 else df.head(2)
            
            recent_submission_rate = (recent_assignments['submitted_at'].notna()).sum() / len(recent_assignments)
            previous_submission_rate = (previous_assignments['submitted_at'].notna()).sum() / len(previous_assignments)
            
            trend = "improving" if recent_submission_rate > previous_submission_rate else "declining" if recent_submission_rate < previous_submission_rate else "stable"
        else:
            trend = "insufficient_data"
        
        # Risk indicators
        risk_indicators = []
        if submission_rate < 0.8:
            risk_indicators.append("low_submission_rate")
        if late_submissions > total_assignments * 0.3:
            risk_indicators.append("frequent_late_submissions")
        if score_percentage < 0.6:
            risk_indicators.append("low_average_score")
        if trend == "declining":
            risk_indicators.append("declining_trend")
        if avg_submission_timing < -24:  # Submitting more than 24 hours late on average
            risk_indicators.append("consistently_late")
        
        return {
            "score": (submission_rate + on_time_rate + score_percentage) / 3,  # Composite score
            "total_assignments": total_assignments,
            "submitted_assignments": submitted_assignments,
            "submission_rate": submission_rate,
            "on_time_rate": on_time_rate,
            "late_submissions": late_submissions,
            "average_score": avg_score,
            "score_percentage": score_percentage,
            "avg_submission_timing": avg_submission_timing,
            "trend": trend,
            "risk_indicators": risk_indicators
        }
    
    def _analyze_engagement_patterns(self, engagement_data: List[Dict]) -> Dict:
        """Analyze student engagement through bot interactions"""
        
        if not engagement_data:
            return {"score": 0.0, "pattern": "no_engagement"}
        
        df = pd.DataFrame(engagement_data)
        
        # Basic metrics
        total_messages = len(df)
        user_messages = len(df[df['is_from_bot'] == False])
        bot_responses = len(df[df['is_from_bot'] == True])
        
        # Conversation analysis
        conversation_types = df['conversation_type'].value_counts().to_dict()
        
        # Engagement frequency
        df['created_at'] = pd.to_datetime(df['created_at'])
        df['date'] = df['created_at'].dt.date
        daily_interactions = df.groupby('date').size().to_dict()
        
        # Calculate engagement score
        engagement_frequency = len(daily_interactions) / 30  # Interactions per day over 30 days
        question_complexity = user_messages / total_messages if total_messages > 0 else 0
        
        engagement_score = min((engagement_frequency * 0.6 + question_complexity * 0.4), 1.0)
        
        # Identify engagement patterns
        patterns = []
        if engagement_frequency > 0.5:
            patterns.append("highly_engaged")
        elif engagement_frequency > 0.2:
            patterns.append("moderately_engaged")
        else:
            patterns.append("low_engagement")
        
        if 'bologna_system' in conversation_types:
            patterns.append("seeks_academic_guidance")
        if 'behavior_analysis' in conversation_types:
            patterns.append("self_aware")
        
        return {
            "score": engagement_score,
            "total_messages": total_messages,
            "user_messages": user_messages,
            "engagement_frequency": engagement_frequency,
            "conversation_types": conversation_types,
            "daily_interactions": {str(k): v for k, v in daily_interactions.items()},
            "patterns": patterns
        }
    
    def _assess_risk_level(self, attendance_analysis: Dict, homework_analysis: Dict, engagement_analysis: Dict) -> Dict:
        """Assess overall student risk level"""
        
        # Weight the different factors
        attendance_weight = 0.4
        homework_weight = 0.4
        engagement_weight = 0.2
        
        # Calculate weighted score
        overall_score = (
            attendance_analysis['score'] * attendance_weight +
            homework_analysis['score'] * homework_weight +
            engagement_analysis['score'] * engagement_weight
        )
        
        # Determine risk level
        if overall_score >= 0.8:
            risk_level = "low"
        elif overall_score >= 0.6:
            risk_level = "medium"
        elif overall_score >= 0.4:
            risk_level = "high"
        else:
            risk_level = "critical"
        
        # Collect risk factors
        risk_factors = []
        risk_factors.extend(attendance_analysis.get('risk_indicators', []))
        risk_factors.extend(homework_analysis.get('risk_indicators', []))
        
        if engagement_analysis['score'] < 0.3:
            risk_factors.append("low_engagement")
        
        # Override risk level if critical indicators present
        critical_indicators = ['low_attendance_rate', 'frequent_absences', 'low_submission_rate']
        if any(indicator in risk_factors for indicator in critical_indicators):
            if risk_level in ['low', 'medium']:
                risk_level = 'high'
        
        return {
            "level": risk_level,
            "score": overall_score,
            "factors": list(set(risk_factors)),  # Remove duplicates
            "breakdown": {
                "attendance": attendance_analysis['score'],
                "homework": homework_analysis['score'],
                "engagement": engagement_analysis['score']
            }
        }
    
    def _generate_recommendations(self, risk_assessment: Dict, attendance_analysis: Dict, homework_analysis: Dict) -> List[str]:
        """Generate personalized recommendations"""
        
        recommendations = []
        risk_factors = risk_assessment['factors']
        risk_level = risk_assessment['level']
        
        # Attendance-based recommendations
        if 'low_attendance_rate' in risk_factors:
            recommendations.append("حضورك أقل من المطلوب (75%). حاول حضور المحاضرات القادمة بانتظام.")
        
        if 'frequent_absences' in risk_factors:
            recommendations.append("لديك غيابات متكررة. تواصل مع مكتب الشؤون الأكاديمية لمناقشة وضعك.")
        
        if 'declining_trend' in risk_factors:
            recommendations.append("يبدو أن أداءك في تراجع مؤخراً. راجع أهدافك الدراسية وخطط لتحسين أدائك.")
        
        # Homework-based recommendations
        if 'low_submission_rate' in risk_factors:
            recommendations.append("معدل تسليم واجباتك منخفض. ضع تذكيرات لمواعيد التسليم واطلب المساعدة عند الحاجة.")
        
        if 'frequent_late_submissions' in risk_factors:
            recommendations.append("لديك تسليمات متأخرة متكررة. حاول التخطيط بشكل أفضل وابدأ الواجبات مبكراً.")
        
        if 'low_average_score' in risk_factors:
            recommendations.append("درجاتك في الواجبات منخفضة. راجع المواد مع زملائك أو اطلب مساعدة المدرس.")
        
        # Engagement-based recommendations
        if 'low_engagement' in risk_factors:
            recommendations.append("تفاعلك مع النظام محدود. استخدم البوت للحصول على نصائح أكاديمية وتابع تقدمك.")
        
        # Risk level-based recommendations
        if risk_level == 'critical':
            recommendations.append("وضعك الأكاديمي يتطلب تدخل فوري. تواصل مع المرشد الأكاديمي بأسرع وقت.")
        elif risk_level == 'high':
            recommendations.append("تحتاج لتحسين أدائك لتجنب المشاكل الأكاديمية. ضع خطة عمل واضحة.")
        elif risk_level == 'medium':
            recommendations.append("أداؤك مقبول لكن يمكن تحسينه. ركز على النقاط الضعيفة.")
        else:
            recommendations.append("أداؤك ممتاز! حافظ على هذا المستوى واستمر في التطوير.")
        
        # Specific actionable recommendations
        if attendance_analysis.get('trend') == 'declining':
            recommendations.append("ضع تنبيهات يومية لمواعيد المحاضرات وجهز مستلزماتك من الليلة السابقة.")
        
        if homework_analysis.get('avg_submission_timing', 0) < -12:
            recommendations.append("أنشئ تقويماً للواجبات وابدأ العمل عليها قبل 3 أيام من موعد التسليم.")
        
        return recommendations[:6]  # Return max 6 recommendations
    
    def _create_visualizations(self, student_data: Dict, attendance_analysis: Dict, homework_analysis: Dict) -> Dict:
        """Create visualizations for the analysis"""
        
        charts = {}
        
        # Attendance trend chart
        if attendance_analysis.get('total_sessions', 0) > 0:
            attendance_df = pd.DataFrame(student_data['attendance'])
            attendance_df['start_time'] = pd.to_datetime(attendance_df['start_time'])
            attendance_df['date'] = attendance_df['start_time'].dt.date
            
            # Daily attendance
            daily_attendance = attendance_df.groupby('date')['status'].apply(
                lambda x: 1 if (x == 'present').any() else 0
            ).reset_index()
            
            plt.figure(figsize=(12, 6))
            plt.plot(daily_attendance['date'], daily_attendance['status'], marker='o')
            plt.title('Attendance Trend Over Time')
            plt.xlabel('Date')
            plt.ylabel('Present (1) / Absent (0)')
            plt.xticks(rotation=45)
            plt.tight_layout()
            
            # Convert to base64
            buffer = io.BytesIO()
            plt.savefig(buffer, format='png')
            buffer.seek(0)
            charts['attendance_trend'] = base64.b64encode(buffer.getvalue()).decode()
            plt.close()
        
        # Homework submission pattern
        if homework_analysis.get('total_assignments', 0) > 0:
            homework_df = pd.DataFrame(student_data['homework'])
            homework_df['due_date'] = pd.to_datetime(homework_df['due_date'])
            homework_df['status'] = homework_df['submitted_at'].apply(
                lambda x: 'Submitted' if pd.notna(x) else 'Not Submitted'
            )
            
            plt.figure(figsize=(10, 6))
            status_counts = homework_df['status'].value_counts()
            plt.pie(status_counts.values, labels=status_counts.index, autopct='%1.1f%%')
            plt.title('Homework Submission Rate')
            
            buffer = io.BytesIO()
            plt.savefig(buffer, format='png')
            buffer.seek(0)
            charts['homework_pie'] = base64.b64encode(buffer.getvalue()).decode()
            plt.close()
        
        # Weekly attendance pattern
        if 'weekly_pattern' in attendance_analysis:
            days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
            weekly_data = [attendance_analysis['weekly_pattern'].get(i, 0) for i in range(7)]
            
            plt.figure(figsize=(10, 6))
            plt.bar(days, weekly_data)
            plt.title('Weekly Attendance Pattern')
            plt.xlabel('Day of Week')
            plt.ylabel('Attendance Rate')
            plt.ylim(0, 1)
            
            buffer = io.BytesIO()
            plt.savefig(buffer, format='png')
            buffer.seek(0)
            charts['weekly_pattern'] = base64.b64encode(buffer.getvalue()).decode()
            plt.close()
        
        return charts

# Initialize the behavior analyzer
behavior_analyzer = StudentBehaviorAnalyzer()

@app.post("/api/behavior/analyze")
async def analyze_student_behavior(request: BehaviorAnalysisRequest):
    """Analyze student behavior and provide insights"""
    
    try:
        result = await behavior_analyzer.analyze_student_behavior(
            request.student_id,
            request.analysis_period_days
        )
        
        return BehaviorAnalysisResponse(
            analysis=generate_analysis_summary(result),
            recommendations=result.get('recommendations', []),
            processing_time=100,  # Placeholder
            confidence=0.9,  # Placeholder
            visualization_data=result.get('visualizations', {})
        )
        
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

def generate_analysis_summary(result: Dict) -> str:
    """Generate human-readable analysis summary"""
    
    risk_level = result.get('overall_risk_level', 'unknown')
    attendance_score = result.get('attendance_score', 0)
    homework_score = result.get('homework_score', 0)
    engagement_score = result.get('engagement_score', 0)
    
    summary = f"""
    تحليل السلوك الأكاديمي:
    
    📊 النتيجة الإجمالية: {risk_level.upper()}
    
    🎯 التفاصيل:
    • معدل الحضور: {attendance_score:.1%}
    • أداء الواجبات: {homework_score:.1%}
    • مستوى التفاعل: {engagement_score:.1%}
    
    📈 الملاحظات:
    """
    
    if risk_level == 'low':
        summary += "أداؤك ممتاز! تواصل بانتظام وتلتزم بالواجبات. حافظ على هذا المستوى."
    elif risk_level == 'medium':
        summary += "أداؤك جيد لكن يحتاج لبعض التحسين. ركز على النقاط الضعيفة."
    elif risk_level == 'high':
        summary += "هناك مؤشرات تستدعي الانتباه. تحتاج لتحسين الحضور أو أداء الواجبات."
    else:
        summary += "وضعك الأكاديمي يتطلب تدخل فوري. تواصل مع المرشد الأكاديمي."
    
    return summary

🤖 التكامل مع التلغرام {#telegram}
1. Telegram Bot Architecture
Main Bot Service (C#):
csharppublic interface ITelegramBotService
{
    Task HandleUpdateAsync(Update update);
    Task SendMessageAsync(long chatId, string message, IReplyMarkup replyMarkup = null);
    Task SendNotificationAsync(NotificationRequest request);
    Task<bool> ValidateUserAsync(long telegramUserId);
    Task RegisterStudentAsync(long telegramUserId, string qrToken);
}

public class TelegramBotService : ITelegramBotService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IStudentService _studentService;
    private readonly IHomeworkService _homeworkService;
    private readonly IAiChatService _aiChatService;
    private readonly IQrCodeService _qrCodeService;
    private readonly IBotUserRepository _botUserRepository;
    private readonly ILogger<TelegramBotService> _logger;
    private readonly Dictionary<string, IBotCommand> _commands;

    public TelegramBotService(
        ITelegramBotClient botClient,
        IServiceProvider serviceProvider,
        ILogger<TelegramBotService> logger)
    {
        _botClient = botClient;
        _logger = logger;
        _commands = InitializeCommands(serviceProvider);
    }

    public async Task HandleUpdateAsync(Update update)
    {
        try
        {
            var handler = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(update.Message),
                UpdateType.CallbackQuery => HandleCallbackQueryAsync(update.CallbackQuery),
                UpdateType.InlineQuery => HandleInlineQueryAsync(update.InlineQuery),
                _ => Task.CompletedTask
            };

            await handler;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling telegram update: {UpdateType}", update.Type);
        }
    }

    private async Task HandleMessageAsync(Message message)
    {
        if (message.Text == null) return;

        var chatId = message.Chat.Id;
        var userId = message.From.Id;
        var messageText = message.Text;

        // Log user activity
        await LogUserActivityAsync(userId, messageText);

        // Check if user is registered
        var botUser = await _botUserRepository.GetByTelegramIdAsync(userId);
        
        if (botUser == null && !messageText.StartsWith("/start"))
        {
            await SendUnregisteredUserMessageAsync(chatId);
            return;
        }

        // Handle commands
        if (messageText.StartsWith("/"))
        {
            await HandleCommandAsync(message, botUser);
        }
        else
        {
            // Handle regular chat (AI interaction)
            await HandleChatMessageAsync(message, botUser);
        }
    }

    private async Task HandleCommandAsync(Message message, BotUser botUser)
    {
        var commandText = message.Text.Split(' ')[0].ToLower();
        var args = message.Text.Split(' ').Skip(1).ToArray();

        if (_commands.TryGetValue(commandText, out var command))
        {
            var context = new BotCommandContext
            {
                Message = message,
                BotUser = botUser,
                Arguments = args,
                BotClient = _botClient
            };

            await command.ExecuteAsync(context);
        }
        else
        {
            await SendMessageAsync(message.Chat.Id, 
                "❌ أمر غير معروف. استخدم /help لعرض القائمة الكاملة للأوامر.");
        }
    }

    private async Task HandleChatMessageAsync(Message message, BotUser botUser)
    {
        if (botUser == null) return;

        // Check if user is muted
        if (botUser.IsMuted && botUser.MuteUntil > DateTime.UtcNow)
        {
            var remainingTime = botUser.MuteUntil.Value - DateTime.UtcNow;
            await SendMessageAsync(message.Chat.Id, 
                $"🔇 أنت مكتوم لمدة {remainingTime.TotalMinutes:F0} دقيقة أخرى.");
            return;
        }

        // Send typing indicator
        await _botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

        // Process with AI
        var aiRequest = new ChatRequest
        {
            UserId = botUser.StudentId,
            Message = message.Text,
            ConversationType = ChatConversationType.General
        };

        var aiResponse = await _aiChatService.ProcessMessageAsync(aiRequest);
        
        await SendMessageAsync(message.Chat.Id, aiResponse.Message);
    }

    private Dictionary<string, IBotCommand> InitializeCommands(IServiceProvider serviceProvider)
    {
        return new Dictionary<string, IBotCommand>
        {
            ["/start"] = serviceProvider.GetRequiredService<StartCommand>(),
            ["/help"] = serviceProvider.GetRequiredService<HelpCommand>(),
            ["/my_attendance"] = serviceProvider.GetRequiredService<MyAttendanceCommand>(),
            ["/my_homework"] = serviceProvider.GetRequiredService<MyHomeworkCommand>(),
            ["/submit_homework"] = serviceProvider.GetRequiredService<SubmitHomeworkCommand>(),
            ["/add_homework"] = serviceProvider.GetRequiredService<AddHomeworkCommand>(),
            ["/list_homework"] = serviceProvider.GetRequiredService<ListHomeworkCommand>(),
            ["/check_homework"] = serviceProvider.GetRequiredService<CheckHomeworkCommand>(),
            ["/warn_late_students"] = serviceProvider.GetRequiredService<WarnLateStudentsCommand>(),
            ["/mute_student"] = serviceProvider.GetRequiredService<MuteStudentCommand>(),
            ["/unmute_student"] = serviceProvider.GetRequiredService<UnmuteStudentCommand>(),
            ["/broadcast"] = serviceProvider.GetRequiredService<BroadcastCommand>(),
            ["/section_stats"] = serviceProvider.GetRequiredService<SectionStatsCommand>(),
            ["/add_admin"] = serviceProvider.GetRequiredService<AddAdminCommand>(),
            ["/remove_admin"] = serviceProvider.GetRequiredService<RemoveAdminCommand>(),
            ["/system_stats"] = serviceProvider.GetRequiredService<SystemStatsCommand>(),
            ["/my_warnings"] = serviceProvider.GetRequiredService<MyWarningsCommand>()
        };
    }
}
2. Bot Commands Implementation
Base Command Structure:
csharppublic interface IBotCommand
{
    Task ExecuteAsync(BotCommandContext context);
    string[] RequiredRoles { get; }
    string Description { get; }
    string Usage { get; }
}

public class BotCommandContext
{
    public Message Message { get; set; }
    public BotUser BotUser { get; set; }
    public string[] Arguments { get; set; }
    public ITelegramBotClient BotClient { get; set; }
}

public abstract class BaseBotCommand : IBotCommand
{
    protected readonly ILogger Logger;
    
    public abstract string[] RequiredRoles { get; }
    public abstract string Description { get; }
    public abstract string Usage { get; }

    protected BaseBotCommand(ILogger logger)
    {
        Logger = logger;
    }

    public async Task ExecuteAsync(BotCommandContext context)
    {
        try
        {
            // Check permissions
            if (!await HasPermissionAsync(context))
            {
                await SendNoPermissionMessageAsync(context);
                return;
            }

            await ExecuteCommandAsync(context);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing command {CommandType}", GetType().Name);
            await SendErrorMessageAsync(context, "حدث خطأ أثناء تنفيذ الأمر. حاول مرة أخرى.");
        }
    }

    protected abstract Task ExecuteCommandAsync(BotCommandContext context);

    protected virtual async Task<bool> HasPermissionAsync(BotCommandContext context)
    {
        if (RequiredRoles == null || RequiredRoles.Length == 0)
            return true;

        if (context.BotUser == null)
            return false;

        // Check if user has required role
        return RequiredRoles.Contains("Student") || 
               (RequiredRoles.Contains("Admin") && context.BotUser.IsBotAdmin) ||
               RequiredRoles.Contains("Owner"); // Owner check implementation needed
    }

    protected async Task SendNoPermissionMessageAsync(BotCommandContext context)
    {
        await context.BotClient.SendTextMessageAsync(
            context.Message.Chat.Id,
            "❌ ليس لديك صلاحية لتنفيذ هذا الأمر.");
    }

    protected async Task SendErrorMessageAsync(BotCommandContext context, string message)
    {
        await context.BotClient.SendTextMessageAsync(
            context.Message.Chat.Id,
            message);
    }
}
Student Commands:
csharppublic class StartCommand : BaseBotCommand
{
    private readonly IQrCodeService _qrCodeService;
    private readonly IBotUserRepository _botUserRepository;
    private readonly IStudentRepository _studentRepository;

    public override string[] RequiredRoles => new[] { "Any" };
    public override string Description => "بدء استخدام البوت وربط الحساب";
    public override string Usage => "/start [QR_CODE]";

    public StartCommand(
        IQrCodeService qrCodeService,
        IBotUserRepository botUserRepository,
        IStudentRepository studentRepository,
        ILogger<StartCommand> logger) : base(logger)
    {
        _qrCodeService = qrCodeService;
        _botUserRepository = botUserRepository;
        _studentRepository = studentRepository;
    }

    protected override async Task ExecuteCommandAsync(BotCommandContext context)
    {
        var telegramUserId = context.Message.From.Id;
        
        // Check if user is already registered
        var existingUser = await _botUserRepository.GetByTelegramIdAsync(telegramUserId);
        if (existingUser != null)
        {
            await context.BotClient.SendTextMessageAsync(
                context.Message.Chat.Id,
                $"مرحباً {existingUser.FirstName}! أنت مسجل بالفعل في النظام.\n\n" +
                "استخدم /help لعرض الأوامر المتاحة.");
            return;
        }

        // Check if QR code is provided
        if (context.Arguments.Length == 0)
        {
            await context.BotClient.SendTextMessageAsync(
                context.Message.Chat.Id,
                "مرحباً! لاستخدام هذا البوت، تحتاج لمسح رمز QR من التطبيق.\n\n" +
                "📱 افتح التطبيق → انتقل لإعدادات الحساب → امسح رمز QR\n" +
                "ثم ارسل الأمر: `/start [QR_CODE]`");
            return;
        }

        var qrToken = context.Arguments[0];
        
        // Validate QR code
        var validation = await _qrCodeService.ValidateQrCodeAsync(qrToken);
        if (!validation.IsValid)
        {
            await context.BotClient.SendTextMessageAsync(
                context.Message.Chat.Id,
                $"❌ رمز QR غير صحيح: {validation.Error}\n\n" +
                "تأكد من أن الرمز صحيح ولم تنته صلاحيته.");
            return;
        }

        // Get student information
        var student = await _studentRepository.GetByIdAsync(validation.StudentId);
        if (student == null)
        {
            await context.BotClient.SendTextMessageAsync(
                context.Message.Chat.Id,
                "❌ لم يتم العثور على بيانات الطالب. تواصل مع الإدارة.");
            return;
        }

        // Register bot user
        var botUser = new BotUser
        {
            StudentId = student.Id,
            TelegramUserId = telegramUserId,
            TelegramUsername = context.Message.From.Username,
            FirstName = context.Message.From.FirstName,
            LastName = context.Message.From.LastName,
            LanguageCode = context.Message.From.LanguageCode ?? "en",
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _botUserRepository.AddAsync(botUser);
        await _qrCodeService.MarkQrCodeAsUsedAsync(qrToken, telegramUserId);

        // Send welcome message
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "📊 حضوري", "📝 واجباتي" },
            new KeyboardButton[] { "⚠️ إنذاراتي", "💬 مساعدة" }
        })
        {
            ResizeKeyboard = true
        };

        await context.BotClient.SendTextMessageAsync(
            context.Message.Chat.Id,
            $"🎉 مرحباً {student.FirstName}!\n\n" +
            $"تم ربط حسابك بنجاح.\n" +
            $"📚 الشعبة: {student.Section.Name}\n" +
            $"🎓 المرحلة: {student.AcademicStage.Name}\n\n" +
            $"استخدم الأزرار أدناه أو اكتب /help لعرض جميع الأوامر.",
            replyMarkup: keyboard);
    }
}

public class MyAttendanceCommand : BaseBotCommand
{
    private readonly IAttendanceService _attendanceService;
    private readonly IStudentRepository _studentRepository;

    public override string[] RequiredRoles => new[] { "Student" };
    public override string Description => "عرض سجل الحضور الشخصي";
    public override string Usage => "/my_attendance [عدد الأيام]";

    public MyAttendanceCommand(
        IAttendanceService attendanceService,
        IStudentRepository studentRepository,
        ILogger<MyAttendanceCommand> logger) : base(logger)
    {
        _attendanceService = attendanceService;
        _studentRepository = studentRepository;
    }

    protected override async Task ExecuteCommandAsync(BotCommandContext context)
    {
        var student = await _studentRepository.GetByIdAsync(context.BotUser.StudentId);
        
        // Parse days parameter (default 30 days)
        var days = 30;
        if (context.Arguments.Length > 0 && int.TryParse(context.Arguments[0], out var parsedDays))
        {
            days = Math.Min(parsedDays, 90); // Max 90 days
        }

        var fromDate = DateTime.UtcNow.AddDays(-days);
        var toDate = DateTime.UtcNow;

        // Get attendance statistics
        var stats = await _attendanceService.GetStudentStatisticsAsync(
            student.Id, 
            DateTime.UtcNow.Year.ToString(), 
            GetCurrentSemester());

        // Get recent attendance records
        var recentRecords = await _attendanceService.GetStudentAttendanceAsync(
            student.Id, fromDate, toDate);

        var message = $"📊 **سجل الحضور - {student.FirstName}**\n\n";
        
        message += $"📈 **الإحصائيات العامة:**\n";
        message += $"• إجمالي الجلسات: {stats.TotalSessions}\n";
        message += $"• الحضور: {stats.PresentSessions} ({stats.AttendancePercentage:F1}%)\n";
        message += $"• الغياب: {stats.AbsentSessions}\n";
        message += $"• التأخير: {stats.LateSessions}\n\n";

        // Attendance status indicator
        var statusEmoji = stats.AttendancePercentage >= 75 ? "✅" : 
                         stats.AttendancePercentage >= 60 ? "⚠️" : "❌";
        
        message += $"{statusEmoji} **حالة الحضور:** ";
        message += stats.AttendancePercentage >= 75 ? "مقبول" :
                   stats.AttendancePercentage >= 60 ? "تحذير" : "خطر";
        
        message += $" ({75 - stats.AttendancePercentage:F1}% للوصول للحد الأدنى)\n\n";

        // Recent sessions
        if (recentRecords.Any())
        {
            message += $"📅 **آخر {Math.Min(5, recentRecords.Count())} جلسات:**\n";
            foreach (var record in recentRecords.Take(5))
            {
                var statusIcon = record.Status switch
                {
                    AttendanceStatus.Present => "✅",
                    AttendanceStatus.Late => "🟡",
                    AttendanceStatus.Absent => "❌",
                    AttendanceStatus.Excused => "🔵",
                    _ => "❓"
                };
                
                message += $"{statusIcon} {record.SessionDate:MM/dd} - {record.Status}\n";
            }
        }

        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("📊 إحصائيات مفصلة", "detailed_attendance"),
                InlineKeyboardButton.WithCallbackData("📈 الاتجاه", "attendance_trend")
            }
        });

        await context.BotClient.SendTextMessageAsync(
            context.Message.Chat.Id,
            message,
            parseMode: ParseMode.Markdown,
            replyMarkup: keyboard);
    }

    private string GetCurrentSemester()
    {
        var month = DateTime.UtcNow.Month;
        return month >= 9 || month <= 1 ? "Fall" : "Spring";
    }
}

public class MyHomeworkCommand : BaseBotCommand
{
    private readonly IHomeworkService _homeworkService;

    public override string[] RequiredRoles => new[] { "Student" };
    public override string Description => "عرض الواجبات والمهام";
    public override string Usage => "/my_homework";

    public MyHomeworkCommand(
        IHomeworkService homeworkService,
        ILogger<MyHomeworkCommand> logger) : base(logger)
    {
        _homeworkService = homeworkService;
    }

    protected override async Task ExecuteCommandAsync(BotCommandContext context)
    {
        var studentId = context.BotUser.StudentId;
        
        // Get pending homework
        var pendingHomework = await _homeworkService.GetPendingHomeworkAsync(studentId);
        var submittedHomework = await _homeworkService.GetRecentSubmissionsAsync(studentId, 10);

        var message = "📝 **واجباتي**\n\n";

        // Pending homework
        if (pendingHomework.Any())
        {
            message += "⏳ **الواجبات المطلوبة:**\n";
            foreach (var hw in pendingHomework.OrderBy(h => h.DueDate))
            {
                var timeRemaining = hw.DueDate - DateTime.UtcNow;
                var urgencyIcon = timeRemaining.TotalHours < 24 ? "🔴" :
                                timeRemaining.TotalDays < 3 ? "🟡" : "🟢";
                
                message += $"{urgencyIcon} **{hw.Title}**\n";
                message += $"   📅 موعد التسليم: {hw.DueDate:MM/dd HH:mm}\n";
                message += $"   ⏰ متبقي: {FormatTimeRemaining(timeRemaining)}\n\n";
            }
        }
        else
        {
            message += "✅ **لا توجد واجبات مطلوبة حالياً**\n\n";
        }

        // Recent submissions
        if (submittedHomework.Any())
        {
            message += "📋 **آخر التسليمات:**\n";
            foreach (var submission in submittedHomework.Take(3))
            {
                var statusIcon = submission.IsLate ? "🟡" : "✅";
                var scoreText = submission.Score.HasValue ? 
                    $" ({submission.Score:F1}/{submission.MaxScore:F1})" : "";
                
                message += $"{statusIcon} {submission.AssignmentTitle}{scoreText}\n";
                message += $"   📅 سُلم: {submission.SubmittedAt:MM/dd HH:mm}\n";
            }
        }

        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("📝 تسليم واجب", "submit_homework"),
                InlineKeyboardButton.WithCallbackData("📊 إحصائيات", "homework_stats")
            }
        });

        await context.BotClient.SendTextMessageAsync(
            context.Message.Chat.Id,
            message,
            parseMode: ParseMode.Markdown,
            replyMarkup: keyboard);
    }

    private string FormatTimeRemaining(TimeSpan timeSpan)
    {
        if (timeSpan.TotalDays >= 1)
            return $"{timeSpan.Days} يوم";
        else if (timeSpan.TotalHours >= 1)
            return $"{timeSpan.Hours} ساعة";
        else
            return $"{timeSpan.Minutes} دقيقة";
    }
}
Admin Commands:
csharppublic class AddHomeworkCommand : BaseBotCommand
{
    private readonly IHomeworkService _homeworkService;
    private readonly IBotAdminRepository _botAdminRepository;

    public override string[] RequiredRoles => new[] { "Admin" };
    public override string Description => "إضافة واجب جديد للشعبة";
    public override string Usage => "/add_homework [عنوان] [وصف] [تاريخ_التسليم]";

    public AddHomeworkCommand(
        IHomeworkService homeworkService,
        IBotAdminRepository botAdminRepository,
        ILogger<AddHomeworkCommand> logger) : base(logger)
    {
        _homeworkService = homeworkService;
        _botAdminRepository = botAdminRepository;
    }

    protected override async Task ExecuteCommandAsync(BotCommandContext context)
    {
        if (context.Arguments.Length < 3)
        {
            await context.BotClient.SendTextMessageAsync(
                context.Message.Chat.Id,
                "❌ صيغة الأمر غير صحيحة.\n\n" +
                "الاستخدام: `/add_homework [عنوان] [وصف] [تاريخ_التسليم]`\n\n" +
                "مثال: `/add_homework \"تقرير المشروع\" \"تقرير عن تطبيق الجوال\" \"2024-12-15 23:59\"`",
                parseMode: ParseMode.Markdown);
            return;
        }

        // Get admin's section
        var botAdmin = await _botAdminRepository.GetByUserIdAsync(context.BotUser.Id);
        if (botAdmin == null)
        {
            await context.BotClient.SendTextMessageAsync(
                context.Message.Chat.Id,
                "❌ ليس لديك صلاحيات إدارية.");
            return;
        }

        var title = context.Arguments[0];
        var description = context.Arguments[1];
        
        if (!DateTime.TryParse(context.Arguments[2], out var dueDate))
        {
            await context.BotClient.SendTextMessageAsync(
                context.Message.Chat.Id,
                "❌ تاريخ التسليم غير صحيح. استخدم الصيغة: YYYY-MM-DD HH:MM");
            return;
        }

        var request = new CreateHomeworkRequest
        {
            Title = title,
            Description = description,
            SectionId = botAdmin.SectionId,
            DueDate = dueDate,
            CreatedBy = context.BotUser.StudentId
        };

        var homework = await _homeworkService.CreateHomeworkAsync(request);

        // Send confirmation to admin
        await context.BotClient.SendTextMessageAsync(
            context.Message.Chat.Id,
            $"✅ **تم إضافة الواجب بنجاح**\n\n" +
            $"📝 العنوان: {homework.Title}\n" +
            $"📄 الوصف: {homework.Description}\n" +
            $"📅 موعد التسليم: {homework.DueDate:yyyy-MM-dd HH:mm}\n" +
            $"🆔 معرف الواجب: `{homework.Id}`",
            parseMode: ParseMode.Markdown);

        // Notify all students in section
        await NotifyStudentsAboutNewHomeworkAsync(botAdmin.SectionId, homework);
    }

    private async Task NotifyStudentsAboutNewHomeworkAsync(Guid sectionId, HomeworkDto homework)
    {
        var notification = new NotificationRequest
        {
            MessageType = "homework_assigned",
            TargetType = "section",
            TargetSectionId = sectionId,
            MessageContent = $"📝 **واجب جديد: {homework.Title}**\n\n" +
                           $"📄 {homework.Description}\n\n" +
                           $"📅 موعد التسليم: {homework.DueDate:yyyy-MM-dd HH:mm}\n" +
                           $"⏰ متبقي: {FormatTimeRemaining(homework.DueDate - DateTime.UtcNow)}\n\n" +
                           $"استخدم `/submit_homework {homework.Id}` للتسليم.",
            ScheduledAt = DateTime.UtcNow
        };

        // Queue notification for sending
        await _homeworkService.QueueNotificationAsync(notification);
    }

    private string FormatTimeRemaining(TimeSpan timeSpan)
    {
        if (timeSpan.TotalDays >= 1)
            return $"{timeSpan.Days} يوم";
        else if (timeSpan.TotalHours >= 1)
            return $"{timeSpan.Hours} ساعة";
        else
            return $"{timeSpan.Minutes} دقيقة";
    }
}

public class WarnLateStudentsCommand : BaseBotCommand
{
    private readonly IHomeworkService _homeworkService;
    private readonly IPenaltyService _penaltyService;

    public override string[] RequiredRoles => new[] { "Admin" };
    public override string Description => "تحذير الطلاب المتأخرين في التسليم";
    public override string Usage => "/warn_late_students [homework_id]";

    protected override async Task ExecuteCommandAsync(BotCommandContext context)
    {
        if (context.Arguments.Length == 0)
        {
            await context.BotClient.SendTextMessageAsync(
                context.Message.Chat.Id,
                "❌ يرجى تحديد معرف الواجب.\n\n" +
                "الاستخدام: `/warn_late_students [homework_id]`");
            return;
        }

        if (!Guid.TryParse(context.Arguments[0], out var homeworkId))
        {
            await context.BotClient.SendTextMessageAsync(
                context.Message.Chat.Id,
                "❌ معرف الواجب غير صحيح.");
            return;
        }

        var homework = await _homeworkService.GetHomeworkByIdAsync(homeworkId);
        if (homework == null)
        {
            await context.BotClient.SendTextMessageAsync(
                context.Message.Chat.Id,
                "❌ لم يتم العثور على الواجب.");
            return;
        }

        // Get late students based on penalty system rules
        var lateStudents = await _penaltyService.GetStudentsForWarningAsync(homeworkId);

        if (!lateStudents.Any())
        {
            await context.BotClient.SendTextMessageAsync(
                context.Message.Chat.Id,
                "✅ لا يوجد طلاب يحتاجون لتحذير في هذا الواجب.");
            return;
        }

        // Process warnings based on timing
        var results = new List<string>();

        foreach (var student in lateStudents)
        {
            var timeRemaining = homework.DueDate - DateTime.UtcNow;
            var timeElapsedPercent = 100 - (timeRemaining.TotalMinutes / (homework.DueDate - homework.CreatedAt).TotalMinutes * 100);

            PenaltyAction action = timeElapsedPercent switch
            {
                >= 90 => PenaltyAction.PublicShame,
                >= 75 => PenaltyAction.Mute,
                >= 50 => PenaltyAction.Warning,
                _ => PenaltyAction.None
            };

            if (action != PenaltyAction.None)
            {
                var penalty = await _penaltyService.ApplyPenaltyAsync(new ApplyPenaltyRequest
                {
                    StudentId = student.Id,
                    HomeworkId = homeworkId,
                    PenaltyType = action,
                    Reason = $"تأخر في تسليم واجب: {homework.Title}",
                    AppliedBy = context.BotUser.StudentId
                });

                results.Add($"{GetActionEmoji(action)} {student.FirstName} {student.LastName}");
            }
        }

        var message = $"⚠️ **تم تطبيق العقوبات على {results.Count} طالب**\n\n";
        message += string.Join("\n", results);

        await context.BotClient.SendTextMessageAsync(
            context.Message.Chat.Id,
            message,
            parseMode: ParseMode.Markdown);
    }

    private string GetActionEmoji(PenaltyAction action)
    {
        return action switch
        {
            PenaltyAction.Warning => "⚠️",
            PenaltyAction.Mute => "🔇",
            PenaltyAction.PublicShame => "📢",
            _ => "ℹ️"
        };
    }
}
3. Smart Penalty System
Dynamic Penalty Calculator:
csharppublic interface IPenaltyService
{
    Task<PenaltyResult> ApplyPenaltyAsync(ApplyPenaltyRequest request);
    Task<IEnumerable<StudentPenalty>> GetStudentPenaltiesAsync(Guid studentId);
    Task<PenaltyCounter> UpdatePenaltyCounterAsync(Guid studentId, Guid sectionId);
    Task<IEnumerable<Student>> GetStudentsForWarningAsync(Guid homeworkId);
    Task<bool> MuteStudentAsync(Guid studentId, TimeSpan duration, string reason);
    Task<bool> UnmuteStudentAsync(Guid studentId);
}

public class PenaltyService : IPenaltyService
{
    private readonly IPenaltyRepository _penaltyRepository;
    private readonly IHomeworkRepository _homeworkRepository;
    private readonly ITelegramBotService _telegramBotService;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<PenaltyService> _logger;

    public async Task<PenaltyResult> ApplyPenaltyAsync(ApplyPenaltyRequest request)
    {
        var homework = await _homeworkRepository.GetByIdAsync(request.HomeworkId);
        var student = await _studentRepository.GetByIdAsync(request.StudentId);
        
        if (homework == null || student == null)
        {
            return PenaltyResult.Failed("Student or homework not found");
        }

        // Calculate penalty level based on timing
        var penaltyLevel = CalculatePenaltyLevel(homework, request.PenaltyType);
        
        // Update penalty counter
        var counter = await UpdatePenaltyCounterAsync(request.StudentId, student.SectionId);
        
        // Create penalty record
        var penalty = new StudentPenalty
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            HomeworkId = request.HomeworkId,
            PenaltyType = request.PenaltyType.ToString(),
            PenaltyCount = 1,
            PenaltyStart = DateTime.UtcNow,
            IsActive = true,
            Reason = request.Reason,
            AppliedBy = request.AppliedBy
        };

        // Set penalty duration and end time
        switch (request.PenaltyType)
        {
            case PenaltyAction.Mute:
                var muteDuration = CalculateMuteDuration(counter.CurrentCounter);
                penalty.PenaltyDuration = (int)muteDuration.TotalMinutes;
                penalty.PenaltyEnd = DateTime.UtcNow.Add(muteDuration);
                
                // Apply mute to telegram user
                await MuteStudentAsync(request.StudentId, muteDuration, request.Reason);
                break;
                
            case PenaltyAction.PublicShame:
                // Send public notification
                await SendPublicShameNotificationAsync(student, homework);
                break;
        }

        await _penaltyRepository.AddAsync(penalty);

        // Send penalty notification to student
        await SendPenaltyNotificationAsync(student, penalty, homework);

        _logger.LogInformation(
            "Penalty applied: {PenaltyType} to student {StudentId} for homework {HomeworkId}",
            request.PenaltyType, request.StudentId, request.HomeworkId);

        return PenaltyResult.Success(penalty);
    }

    private PenaltyLevel CalculatePenaltyLevel(HomeworkDto homework, PenaltyAction action)
    {
        var totalDuration = homework.DueDate - homework.CreatedAt;
        var timeElapsed = DateTime.UtcNow - homework.CreatedAt;
        var progressPercent = (timeElapsed.TotalMinutes / totalDuration.TotalMinutes) * 100;

        return progressPercent switch
        {
            >= 90 => PenaltyLevel.Critical,
            >= 75 => PenaltyLevel.High,
            >= 50 => PenaltyLevel.Medium,
            _ => PenaltyLevel.Low
        };
    }

    private TimeSpan CalculateMuteDuration(int currentCounter)
    {
        // Progressive mute duration based on penalty count
        return currentCounter switch
        {
            1 => TimeSpan.FromMinutes(15),
            2 => TimeSpan.FromMinutes(30),
            3 => TimeSpan.FromHours(1),
            4 => TimeSpan.FromHours(2),
            >= 5 => TimeSpan.FromHours(6),
            _ => TimeSpan.FromMinutes(10)
        };
    }

    private async Task SendPublicShameNotificationAsync(StudentDto student, HomeworkDto homework)
    {
        // Find official section groups
        var sectionGroups = await _groupRepository.GetOfficialGroupsBySectionIdAsync(student.SectionId);
        
        foreach (var group in sectionGroups)
        {
            var message = $"📢 **تنبيه للجميع**\n\n" +
                         $"الطالب {student.FirstName} {student.LastName} لم يسلم الواجب:\n" +
                         $"📝 {homework.Title}\n" +
                         $"📅 كان مطلوب التسليم: {homework.DueDate:yyyy-MM-dd HH:mm}\n\n" +
                         $"⚠️ تذكروا: التأخير في التسليم يؤثر على درجاتكم!";

            await _telegramBotService.SendMessageAsync(group.TelegramChatId, message);
        }
    }

    private async Task SendPenaltyNotificationAsync(StudentDto student, StudentPenalty penalty, HomeworkDto homework)
    {
        var botUser = await _botUserRepository.GetByStudentIdAsync(student.Id);
        if (botUser == null) return;

        var message = penalty.PenaltyType switch
        {
            "Warning" => $"⚠️ **تحذير**\n\nلم تسلم الواجب: {homework.Title}\nيرجى التسليم قبل انتهاء الموعد.",
            "Mute" => $"🔇 **تم كتمك**\n\nالسبب: تأخر في تسليم {homework.Title}\nمدة الكتم: {penalty.PenaltyDuration} دقيقة",
            "PublicShame" => $"📢 **تنبيه عام**\n\nتم الإعلان عن تأخرك في تسليم {homework.Title} في مجموعة الشعبة.",
            _ => $"ℹ️ تم تطبيق إجراء بسبب: {penalty.Reason}"
        };

        await _telegramBotService.SendMessageAsync(botUser.TelegramUserId, message);
    }

    public async Task<IEnumerable<Student>> GetStudentsForWarningAsync(Guid homeworkId)
    {
        var homework = await _homeworkRepository.GetByIdAsync(homeworkId);
        if (homework == null) return Enumerable.Empty<Student>();

        var totalDuration = homework.DueDate - homework.CreatedAt;
        var timeElapsed = DateTime.UtcNow - homework.CreatedAt;
        var progressPercent = (timeElapsed.TotalMinutes / totalDuration.TotalMinutes) * 100;

        // Only warn students if we've passed the 50% mark
        if (progressPercent < 50) return Enumerable.Empty<Student>();

        // Get students who haven't submitted
        var studentsWithoutSubmission = await _homeworkRepository.GetStudentsWithoutSubmissionAsync(homeworkId);
        
        // Filter students who haven't been warned recently for this homework
        var studentsNeedingWarning = new List<Student>();
        
        foreach (var student in studentsWithoutSubmission)
        {
            var recentPenalty = await _penaltyRepository.GetRecentPenaltyAsync(student.Id, homeworkId);
            
            // Check if enough time has passed since last penalty
            var shouldWarn = progressPercent switch
            {
                >= 90 when (recentPenalty == null || recentPenalty.PenaltyType != "PublicShame") => true,
                >= 75 when (recentPenalty == null || recentPenalty.PenaltyType == "Warning") => true,
                >= 50 when recentPenalty == null => true,
                _ => false
            };

            if (shouldWarn)
            {
                studentsNeedingWarning.Add(student);
            }
        }

        return studentsNeedingWarning;
    }

    public async Task<PenaltyCounter> UpdatePenaltyCounterAsync(Guid studentId, Guid sectionId)
    {
        var counter = await _penaltyRepository.GetPenaltyCounterAsync(studentId, sectionId);
        
        if (counter == null)
        {
            counter = new PenaltyCounter
            {
                StudentId = studentId,
                SectionId = sectionId,
                CurrentCounter = 1,
                TotalWarnings = 0,
                TotalMutes = 0,
                TotalPublicShames = 0,
                LastUpdated = DateTime.UtcNow
            };
        }
        else
        {
            counter.CurrentCounter++;
            counter.LastUpdated = DateTime.UtcNow;
        }

        // Auto-decrement counter for completed homeworks
        _taskQueue.QueueBackgroundWorkItem(async token =>
        {
            await AutoDecrementCounterAsync(studentId, sectionId);
        });

        await _penaltyRepository.UpsertPenaltyCounterAsync(counter);
        return counter;
    }

    private async Task AutoDecrementCounterAsync(Guid studentId, Guid sectionId)
    {
        // This runs in background and decrements counter when homeworks are submitted
        var completedHomeworks = await _homeworkRepository.GetCompletedHomeworksAsync(studentId);
        var counter = await _penaltyRepository.GetPenaltyCounterAsync(studentId, sectionId);
        
        if (counter != null && completedHomeworks.Any())
        {
            // Decrement counter for each completed homework (max decrement per day)
            var decrementAmount = Math.Min(completedHomeworks.Count(), counter.CurrentCounter);
            counter.CurrentCounter = Math.Max(0, counter.CurrentCounter - decrementAmount);
            counter.LastUpdated = DateTime.UtcNow;
            
            await _penaltyRepository.UpsertPenaltyCounterAsync(counter);
        }
    }
}

📁 إدارة الملفات والفيديو {#files}
1. File Management Service
File Storage Architecture:
csharppublic interface IFileManagementService
{
    Task<FileUploadResult> UploadVideoAsync(UploadVideoRequest request);
    Task<FileMetadata> GetFileMetadataAsync(Guid fileId);
    Task<Stream> DownloadFileAsync(Guid fileId);
    Task<bool> DeleteFileAsync(Guid fileId);
    Task<IEnumerable<FileMetadata>> GetSessionFilesAsync(Guid sessionId);
    Task<CleanupResult> CleanupOldFilesAsync(TimeSpan retentionPeriod);
    Task<CompressionResult> CompressVideoAsync(Guid videoId, CompressionSettings settings);
}

public class FileManagementService : IFileManagementService
{
    private readonly IFileMetadataRepository _fileRepository;
    private readonly IMinioStorageService _storageService;
    private readonly IVideoProcessingService _videoProcessingService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FileManagementService> _logger;

    public async Task<FileUploadResult> UploadVideoAsync(UploadVideoRequest request)
    {
        var fileId = Guid.NewGuid();
        var fileName = $"{fileId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.mp4";
        var bucketName = _configuration["Storage:VideoBucket"];
        
        // Calculate file hash for deduplication
        var fileHash = await CalculateFileHashAsync(request.FileStream);
        
        // Check for duplicate files
        var existingFile = await _fileRepository.GetByHashAsync(fileHash);
        if (existingFile != null)
        {
            _logger.LogInformation("Duplicate file detected, returning existing file: {FileId}", existingFile.Id);
            return FileUploadResult.Success(existingFile);
        }

        // Upload to MinIO
        var uploadResult = await _storageService.UploadFileAsync(
            bucketName, 
            fileName, 
            request.FileStream, 
            "video/mp4");

        if (!uploadResult.Success)
        {
            return FileUploadResult.Failed(uploadResult.Error);
        }

        // Create metadata record
        var metadata = new FileMetadata
        {
            Id = fileId,
            OriginalFilename = request.OriginalFilename,
            StoredFilename = fileName,
            FilePath = $"{bucketName}/{fileName}",
            FileSize = request.FileStream.Length,
            MimeType = "video/mp4",
            FileHash = fileHash,
            StorageProvider = "minio",
            BucketName = bucketName,
            IsEncrypted = false,
            UploadedBy = request.UploadedBy
        };

        await _fileRepository.AddAsync(metadata);

        // Create video recording record
        var videoRecording = new VideoRecording
        {
            Id = Guid.NewGuid(),
            SessionId = request.SessionId,
            CameraId = request.CameraId,
            FileMetadataId = fileId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            DurationSeconds = (int)(request.EndTime - request.StartTime).TotalSeconds,
            Resolution = request.Resolution,
            Fps = request.Fps,
            ProcessingStatus = ProcessingStatus.Pending,
            RetentionUntil = DateTime.UtcNow.AddDays(30) // 30 days retention
        };

        await _videoRepository.AddAsync(videoRecording);

        // Queue for face recognition processing
        await QueueVideoProcessingAsync(videoRecording.Id);

        _logger.LogInformation("Video uploaded successfully: {FileId}, Size: {FileSize} bytes", 
            fileId, request.FileStream.Length);

        return FileUploadResult.Success(metadata);
    }

    public async Task<CleanupResult> CleanupOldFilesAsync(TimeRetryااكمل  الدليلEdit2 / 2أكمل الدليل التقني وركز على إدارة الملفات4sالمستخدم يطلب مني إكمال الدليل التقني الشامل. لقد وصلنا إلى جزء إدارة الملفات والفيديو ولم أكمله بعد. يجب أن أكمل هذا الجزء وأتابع باقي أجزاء الدليل حسب جدول المحتويات الذي وضعته في البداية:

✅ نظرة عامة على المشروع
✅ التحليل المعماري
✅ التقنيات المختارة
✅ هيكل قاعدة البيانات
✅ الخدمات والأنظمة الفرعية (جزئياً)
✅ نظام الأمان والحماية
✅ الذكاء الاصطناعي
✅ التكامل مع التلغرام (جزئياً)
🔄 إدارة الملفات والفيديو (لم أكمله)
❌ المراقبة والأداء
❌ خطة التنفيذ

يجب أن أكمل إدارة الملفات أولاً ثم أنتقل لباقي الأجزاء.csharp    public async Task<CleanupResult> CleanupOldFilesAsync(TimeSpan retentionPeriod)
    {
        var cutoffDate = DateTime.UtcNow - retentionPeriod;
        var oldVideos = await _videoRepository.GetVideosOlderThanAsync(cutoffDate);
        
        var cleanupResult = new CleanupResult
        {
            TotalFilesScanned = oldVideos.Count(),
            FilesDeleted = 0,
            SpaceFreed = 0,
            Errors = new List<string>()
        };

        foreach (var video in oldVideos)
        {
            try
            {
                var metadata = await _fileRepository.GetByIdAsync(video.FileMetadataId);
                if (metadata != null)
                {
                    // Delete from storage
                    await _storageService.DeleteFileAsync(metadata.BucketName, metadata.StoredFilename);
                    
                    // Delete metadata records
                    await _videoRepository.DeleteAsync(video.Id);
                    await _fileRepository.DeleteAsync(metadata.Id);
                    
                    cleanupResult.FilesDeleted++;
                    cleanupResult.SpaceFreed += metadata.FileSize;
                    
                    _logger.LogInformation("Deleted old video file: {FileName}, Size: {FileSize} bytes", 
                        metadata.StoredFilename, metadata.FileSize);
                }
            }
            catch (Exception ex)
            {
                var error = $"Failed to delete video {video.Id}: {ex.Message}";
                cleanupResult.Errors.Add(error);
                _logger.LogError(ex, "Error deleting old video: {VideoId}", video.Id);
            }
        }

        _logger.LogInformation("Cleanup completed: {FilesDeleted} files deleted, {SpaceFreed} bytes freed", 
            cleanupResult.FilesDeleted, cleanupResult.SpaceFreed);

        return cleanupResult;
    }

    public async Task<CompressionResult> CompressVideoAsync(Guid videoId, CompressionSettings settings)
    {
        var video = await _videoRepository.GetByIdAsync(videoId);
        if (video == null)
        {
            return CompressionResult.Failed("Video not found");
        }

        var metadata = await _fileRepository.GetByIdAsync(video.FileMetadataId);
        if (metadata == null)
        {
            return CompressionResult.Failed("File metadata not found");
        }

        try
        {
            // Download original file
            var originalStream = await _storageService.DownloadFileAsync(metadata.BucketName, metadata.StoredFilename);
            
            // Compress video
            var compressedStream = await _videoProcessingService.CompressVideoAsync(originalStream, settings);
            
            // Upload compressed version
            var compressedFileName = $"compressed_{metadata.StoredFilename}";
            var uploadResult = await _storageService.UploadFileAsync(
                metadata.BucketName, 
                compressedFileName, 
                compressedStream, 
                metadata.MimeType);

            if (!uploadResult.Success)
            {
                return CompressionResult.Failed(uploadResult.Error);
            }

            // Update metadata
            var originalSize = metadata.FileSize;
            metadata.StoredFilename = compressedFileName;
            metadata.FilePath = $"{metadata.BucketName}/{compressedFileName}";
            metadata.FileSize = compressedStream.Length;
            
            await _fileRepository.UpdateAsync(metadata);

            // Delete original file
            await _storageService.DeleteFileAsync(metadata.BucketName, metadata.StoredFilename);

            var compressionRatio = (double)(originalSize - compressedStream.Length) / originalSize * 100;

            _logger.LogInformation("Video compressed successfully: {VideoId}, Compression: {Ratio:F1}%", 
                videoId, compressionRatio);

            return CompressionResult.Success(originalSize, compressedStream.Length, compressionRatio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error compressing video: {VideoId}", videoId);
            return CompressionResult.Failed(ex.Message);
        }
    }

    private async Task<string> CalculateFileHashAsync(Stream fileStream)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashBytes = await sha256.ComputeHashAsync(fileStream);
            return Convert.ToBase64String(hashBytes);
        }
    }

    private async Task QueueVideoProcessingAsync(Guid videoRecordingId)
    {
        var job = new BackgroundJob
        {
            JobType = "video_face_recognition",
            JobData = JsonSerializer.Serialize(new { VideoRecordingId = videoRecordingId }),
            Priority = 1,
            MaxAttempts = 3
        };

        await _backgroundJobRepository.AddAsync(job);
    }
}
2. MinIO Storage Service
csharppublic interface IMinioStorageService
{
    Task<UploadResult> UploadFileAsync(string bucketName, string fileName, Stream fileStream, string contentType);
    Task<Stream> DownloadFileAsync(string bucketName, string fileName);
    Task<bool> DeleteFileAsync(string bucketName, string fileName);
    Task<bool> FileExistsAsync(string bucketName, string fileName);
    Task<IEnumerable<string>> ListFilesAsync(string bucketName, string prefix = null);
    Task<bool> CreateBucketAsync(string bucketName);
    Task<FileInfo> GetFileInfoAsync(string bucketName, string fileName);
}

public class MinioStorageService : IMinioStorageService
{
    private readonly MinioClient _minioClient;
    private readonly ILogger<MinioStorageService> _logger;

    public MinioStorageService(IConfiguration configuration, ILogger<MinioStorageService> logger)
    {
        _logger = logger;
        
        var endpoint = configuration["MinIO:Endpoint"];
        var accessKey = configuration["MinIO:AccessKey"];
        var secretKey = configuration["MinIO:SecretKey"];
        var useSSL = configuration.GetValue<bool>("MinIO:UseSSL");

        _minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey);

        if (useSSL)
        {
            _minioClient = _minioClient.WithSSL();
        }
    }

    public async Task<UploadResult> UploadFileAsync(string bucketName, string fileName, Stream fileStream, string contentType)
    {
        try
        {
            // Ensure bucket exists
            var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
            if (!bucketExists)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
            }

            // Upload file
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType));

            _logger.LogInformation("File uploaded successfully to MinIO: {BucketName}/{FileName}", bucketName, fileName);
            
            return UploadResult.Success($"{bucketName}/{fileName}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file to MinIO: {BucketName}/{FileName}", bucketName, fileName);
            return UploadResult.Failed(ex.Message);
        }
    }

    public async Task<Stream> DownloadFileAsync(string bucketName, string fileName)
    {
        try
        {
            var memoryStream = new MemoryStream();
            
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream)));

            memoryStream.Position = 0;
            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file from MinIO: {BucketName}/{FileName}", bucketName, fileName);
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string bucketName, string fileName)
    {
        try
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName));

            _logger.LogInformation("File deleted from MinIO: {BucketName}/{FileName}", bucketName, fileName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file from MinIO: {BucketName}/{FileName}", bucketName, fileName);
            return false;
        }
    }
}
3. Video Processing Service
csharppublic interface IVideoProcessingService
{
    Task<ProcessingResult> ProcessVideoAsync(Guid videoRecordingId, CancellationToken cancellationToken = default);
    Task<Stream> CompressVideoAsync(Stream inputStream, CompressionSettings settings);
    Task<VideoMetadata> ExtractVideoMetadataAsync(Stream videoStream);
    Task<IEnumerable<FrameExtraction>> ExtractFramesAsync(Stream videoStream, TimeSpan interval);
}

public class VideoProcessingService : IVideoProcessingService
{
    private readonly IFaceRecognitionService _faceRecognitionService;
    private readonly IVideoRecordingRepository _videoRepository;
    private readonly IFileManagementService _fileService;
    private readonly ILogger<VideoProcessingService> _logger;

    public async Task<ProcessingResult> ProcessVideoAsync(Guid videoRecordingId, CancellationToken cancellationToken = default)
    {
        var video = await _videoRepository.GetByIdAsync(videoRecordingId);
        if (video == null)
        {
            return ProcessingResult.Failed("Video recording not found");
        }

        try
        {
            // Update status to processing
            video.ProcessingStatus = ProcessingStatus.Processing;
            await _videoRepository.UpdateAsync(video);

            // Get video file stream
            var videoStream = await _fileService.DownloadFileAsync(video.FileMetadataId);
            
            // Save to temporary file for face recognition processing
            var tempFilePath = Path.GetTempFileName() + ".mp4";
            using (var fileStream = File.Create(tempFilePath))
            {
                await videoStream.CopyToAsync(fileStream, cancellationToken);
            }

            // Process with face recognition service
            var session = await _attendanceSessionRepository.GetByIdAsync(video.SessionId);
            var faceRecognitionRequest = new ProcessVideoRequest
            {
                VideoFilePath = tempFilePath,
                SessionId = video.SessionId,
                SectionId = session.SectionId,
                ConfidenceThreshold = 0.8
            };

            var faceRecognitionResult = await _faceRecognitionService.ProcessVideoAsync(faceRecognitionRequest);

            // Save face detection results
            var detections = new List<FaceDetection>();
            foreach (var result in faceRecognitionResult.AttendanceResults)
            {
                var detection = new FaceDetection
                {
                    Id = Guid.NewGuid(),
                    VideoRecordingId = videoRecordingId,
                    StudentId = Guid.Parse(result.Key),
                    DetectionTimestamp = video.StartTime.AddSeconds(result.Value.FirstDetection),
                    ConfidenceScore = (decimal)result.Value.AvgConfidence,
                    BoundingBox = JsonSerializer.Serialize(new { x = 0, y = 0, width = 100, height = 100 }), // Placeholder
                    IsVerified = result.Value.AvgConfidence >= 0.8
                };
                detections.Add(detection);
            }

            await _faceDetectionRepository.AddRangeAsync(detections);

            // Update attendance records
            await UpdateAttendanceRecordsAsync(video.SessionId, faceRecognitionResult.AttendanceResults);

            // Update video processing status
            video.ProcessingStatus = ProcessingStatus.Completed;
            video.FacesDetected = detections.Count;
            video.ProcessingCompletedAt = DateTime.UtcNow;
            
            await _videoRepository.UpdateAsync(video);

            // Cleanup temporary file
            File.Delete(tempFilePath);

            _logger.LogInformation("Video processing completed: {VideoId}, Faces detected: {FaceCount}", 
                videoRecordingId, detections.Count);

            return ProcessingResult.Success(detections.Count, faceRecognitionResult.ProcessingTime);
        }
        catch (Exception ex)
        {
            // Update status to failed
            video.ProcessingStatus = ProcessingStatus.Failed;
            video.ProcessingError = ex.Message;
            await _videoRepository.UpdateAsync(video);

            _logger.LogError(ex, "Error processing video: {VideoId}", videoRecordingId);
            return ProcessingResult.Failed(ex.Message);
        }
    }

    private async Task UpdateAttendanceRecordsAsync(Guid sessionId, Dictionary<string, dynamic> attendanceResults)
    {
        foreach (var result in attendanceResults)
        {
            var studentId = Guid.Parse(result.Key);
            var attendanceData = result.Value;

            var attendanceRecord = await _attendanceRecordRepository.GetBySessionAndStudentAsync(sessionId, studentId);
            if (attendanceRecord != null)
            {
                attendanceRecord.Status = AttendanceStatus.Present;
                attendanceRecord.DetectedAt = DateTime.UtcNow.AddSeconds(-attendanceData.FirstDetection);
                attendanceRecord.ConfidenceScore = (decimal)attendanceData.AvgConfidence;
                attendanceRecord.DetectionMethod = DetectionMethod.FaceRecognition;

                await _attendanceRecordRepository.UpdateAsync(attendanceRecord);
            }
        }
    }

    public async Task<Stream> CompressVideoAsync(Stream inputStream, CompressionSettings settings)
    {
        var inputPath = Path.GetTempFileName() + ".mp4";
        var outputPath = Path.GetTempFileName() + ".mp4";

        try
        {
            // Save input stream to temporary file
            using (var fileStream = File.Create(inputPath))
            {
                await inputStream.CopyToAsync(fileStream);
            }

            // Build FFmpeg command
            var ffmpegArgs = $"-i \"{inputPath}\" -c:v libx264 -preset {settings.Preset} -crf {settings.CRF} -c:a aac -b:a 128k \"{outputPath}\"";
            
            var processInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processInfo))
            {
                await process.WaitForExitAsync();
                
                if (process.ExitCode != 0)
                {
                    var error = await process.StandardError.ReadToEndAsync();
                    throw new VideoProcessingException($"FFmpeg compression failed: {error}");
                }
            }

            // Return compressed video stream
            var compressedStream = new MemoryStream();
            using (var fileStream = File.OpenRead(outputPath))
            {
                await fileStream.CopyToAsync(compressedStream);
            }
            
            compressedStream.Position = 0;
            return compressedStream;
        }
        finally
        {
            // Cleanup temporary files
            if (File.Exists(inputPath)) File.Delete(inputPath);
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }
}

public class CompressionSettings
{
    public string Preset { get; set; } = "fast"; // ultrafast, superfast, veryfast, faster, fast, medium, slow, slower, veryslow
    public int CRF { get; set; } = 23; // 0-51, lower = better quality
    public string Resolution { get; set; } = "1280x720"; // Optional resolution change
    public int? Fps { get; set; } = null; // Optional FPS change
}

📊 المراقبة والأداء {#monitoring}
1. Application Monitoring Setup
Performance Monitoring Service:
csharppublic interface IPerformanceMonitoringService
{
    Task LogRequestAsync(RequestMetrics metrics);
    Task LogSystemMetricsAsync(SystemMetrics metrics);
    Task<HealthCheckResult> CheckSystemHealthAsync();
    Task<PerformanceReport> GeneratePerformanceReportAsync(DateTime fromDate, DateTime toDate);
    Task SendAlertAsync(AlertRequest alert);
}

public class PerformanceMonitoringService : IPerformanceMonitoringService
{
    private readonly IMetricsLogger _metricsLogger;
    private readonly IHealthCheckService _healthCheckService;
    private readonly INotificationService _notificationService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PerformanceMonitoringService> _logger;

    public async Task LogRequestAsync(RequestMetrics metrics)
    {
        // Log to structured logging system
        _logger.LogInformation("API Request: {Method} {Path} - {StatusCode} in {Duration}ms", 
            metrics.Method, metrics.Path, metrics.StatusCode, metrics.DurationMs);

        // Send to metrics aggregation
        await _metricsLogger.LogAsync("api_request", new
        {
            method = metrics.Method,
            path = metrics.Path,
            status_code = metrics.StatusCode,
            duration_ms = metrics.DurationMs,
            user_id = metrics.UserId,
            timestamp = metrics.Timestamp
        });

        // Check for performance issues
        if (metrics.DurationMs > 5000) // Requests taking more than 5 seconds
        {
            await SendAlertAsync(new AlertRequest
            {
                AlertType = AlertType.Performance,
                Severity = AlertSeverity.Warning,
                Message = $"Slow API request detected: {metrics.Method} {metrics.Path} took {metrics.DurationMs}ms",
                Metadata = metrics
            });
        }
    }

    public async Task<HealthCheckResult> CheckSystemHealthAsync()
    {
        var healthChecks = new Dictionary<string, bool>();
        var details = new Dictionary<string, object>();

        try
        {
            // Database connectivity
            healthChecks["database"] = await CheckDatabaseHealthAsync();
            
            // Redis connectivity
            healthChecks["redis"] = await CheckRedisHealthAsync();
            
            // MinIO storage
            healthChecks["storage"] = await CheckStorageHealthAsync();
            
            // Face recognition service
            healthChecks["face_recognition"] = await CheckFaceRecognitionServiceAsync();
            
            // Telegram bot
            healthChecks["telegram_bot"] = await CheckTelegramBotAsync();
            
            // System resources
            var systemMetrics = await GetSystemMetricsAsync();
            healthChecks["system_resources"] = systemMetrics.IsHealthy;
            details["system_metrics"] = systemMetrics;

            // Overall health
            var isHealthy = healthChecks.Values.All(h => h);
            
            var result = new HealthCheckResult
            {
                IsHealthy = isHealthy,
                CheckedAt = DateTime.UtcNow,
                Services = healthChecks,
                Details = details
            };

            // Log health status
            _logger.LogInformation("Health check completed: {IsHealthy}, Services: {ServiceCount}", 
                isHealthy, healthChecks.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during health check");
            return new HealthCheckResult
            {
                IsHealthy = false,
                CheckedAt = DateTime.UtcNow,
                Services = healthChecks,
                Error = ex.Message
            };
        }
    }

    private async Task<bool> CheckDatabaseHealthAsync()
    {
        try
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            
            var command = new NpgsqlCommand("SELECT 1", connection);
            var result = await command.ExecuteScalarAsync();
            
            return result != null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Database health check failed");
            return false;
        }
    }

    private async Task<bool> CheckRedisHealthAsync()
    {
        try
        {
            var redis = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis"));
            var database = redis.GetDatabase();
            
            await database.PingAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis health check failed");
            return false;
        }
    }

    private async Task<SystemMetrics> GetSystemMetricsAsync()
    {
        var process = Process.GetCurrentProcess();
        
        return new SystemMetrics
        {
            CpuUsagePercent = await GetCpuUsageAsync(),
            MemoryUsageMB = process.WorkingSet64 / 1024 / 1024,
            DiskUsagePercent = await GetDiskUsageAsync(),
            ActiveConnections = GetActiveConnectionsCount(),
            ThreadCount = process.Threads.Count,
            IsHealthy = true // Will be calculated based on thresholds
        };
    }
}

// Middleware for automatic request monitoring
public class RequestMonitoringMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IPerformanceMonitoringService _monitoringService;
    private readonly ILogger<RequestMonitoringMiddleware> _logger;

    public RequestMonitoringMiddleware(
        RequestDelegate next, 
        IPerformanceMonitoringService monitoringService,
        ILogger<RequestMonitoringMiddleware> logger)
    {
        _next = next;
        _monitoringService = monitoringService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var startTime = DateTime.UtcNow;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in request pipeline");
            throw;
        }
        finally
        {
            stopwatch.Stop();

            var metrics = new RequestMetrics
            {
                Method = context.Request.Method,
                Path = context.Request.Path,
                StatusCode = context.Response.StatusCode,
                DurationMs = stopwatch.ElapsedMilliseconds,
                UserId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault(),
                Timestamp = startTime
            };

            // Log metrics asynchronously
            _ = Task.Run(async () =>
            {
                try
                {
                    await _monitoringService.LogRequestAsync(metrics);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to log request metrics");
                }
            });
        }
    }
}
2. Custom Metrics and Alerting
csharppublic interface ICustomMetricsService
{
    Task IncrementCounterAsync(string metricName, Dictionary<string, string> tags = null);
    Task RecordGaugeAsync(string metricName, double value, Dictionary<string, string> tags = null);
    Task RecordHistogramAsync(string metricName, double value, Dictionary<string, string> tags = null);
    Task<MetricSummary> GetMetricSummaryAsync(string metricName, TimeSpan period);
}

public class CustomMetricsService : ICustomMetricsService
{
    private readonly ILogger<CustomMetricsService> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _prometheusClient;

    // Application-specific metrics
    private readonly Counter _faceRecognitionAttempts;
    private readonly Counter _attendanceMarked;
    private readonly Counter _homeworkSubmissions;
    private readonly Counter _telegramMessages;
    private readonly Histogram _videoProcessingDuration;
    private readonly Gauge _activeUsers;
    private readonly Gauge _systemLoad;

    public CustomMetricsService(ILogger<CustomMetricsService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        // Initialize Prometheus metrics
        _faceRecognitionAttempts = Metrics.CreateCounter(
            "face_recognition_attempts_total", 
            "Total number of face recognition attempts",
            new[] { "camera_id", "section_id", "result" });

        _attendanceMarked = Metrics.CreateCounter(
            "attendance_marked_total", 
            "Total attendance records marked",
            new[] { "section_id", "status", "method" });

        _homeworkSubmissions = Metrics.CreateCounter(
            "homework_submissions_total", 
            "Total homework submissions",
            new[] { "section_id", "is_late", "submission_type" });

        _telegramMessages = Metrics.CreateCounter(
            "telegram_messages_total", 
            "Total Telegram bot messages",
            new[] { "message_type", "user_type" });

        _videoProcessingDuration = Metrics.CreateHistogram(
            "video_processing_duration_seconds", 
            "Duration of video processing operations",
            new[] { "camera_id", "processing_type" });

        _activeUsers = Metrics.CreateGauge(
            "active_users", 
            "Number of currently active users",
            new[] { "user_type" });

        _systemLoad = Metrics.CreateGauge(
            "system_load", 
            "Current system load metrics",
            new[] { "metric_type" });
    }

    public async Task IncrementCounterAsync(string metricName, Dictionary<string, string> tags = null)
    {
        try
        {
            switch (metricName)
            {
                case "face_recognition_attempts":
                    _faceRecognitionAttempts
                        .WithLabels(tags?.GetValueOrDefault("camera_id", "unknown"),
                                   tags?.GetValueOrDefault("section_id", "unknown"),
                                   tags?.GetValueOrDefault("result", "unknown"))
                        .Inc();
                    break;

                case "attendance_marked":
                    _attendanceMarked
                        .WithLabels(tags?.GetValueOrDefault("section_id", "unknown"),
                                   tags?.GetValueOrDefault("status", "unknown"),
                                   tags?.GetValueOrDefault("method", "unknown"))
                        .Inc();
                    break;

                case "homework_submissions":
                    _homeworkSubmissions
                        .WithLabels(tags?.GetValueOrDefault("section_id", "unknown"),
                                   tags?.GetValueOrDefault("is_late", "unknown"),
                                   tags?.GetValueOrDefault("submission_type", "unknown"))
                        .Inc();
                    break;

                case "telegram_messages":
                    _telegramMessages
                        .WithLabels(tags?.GetValueOrDefault("message_type", "unknown"),
                                   tags?.GetValueOrDefault("user_type", "unknown"))
                        .Inc();
                    break;
            }

            _logger.LogDebug("Incremented counter: {MetricName} with tags: {@Tags}", metricName, tags);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to increment counter: {MetricName}", metricName);
        }
    }

    public async Task RecordHistogramAsync(string metricName, double value, Dictionary<string, string> tags = null)
    {
        try
        {
            switch (metricName)
            {
                case "video_processing_duration":
                    _videoProcessingDuration
                        .WithLabels(tags?.GetValueOrDefault("camera_id", "unknown"),
                                   tags?.GetValueOrDefault("processing_type", "unknown"))
                        .Observe(value);
                    break;
            }

            _logger.LogDebug("Recorded histogram: {MetricName} = {Value} with tags: {@Tags}", metricName, value, tags);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to record histogram: {MetricName}", metricName);
        }
    }
}

// Background service for system metrics collection
public class SystemMetricsCollectorService : BackgroundService
{
    private readonly ICustomMetricsService _metricsService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SystemMetricsCollectorService> _logger;

    public SystemMetricsCollectorService(
        ICustomMetricsService metricsService,
        IServiceScopeFactory scopeFactory,
        ILogger<SystemMetricsCollectorService> logger)
    {
        _metricsService = metricsService;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CollectSystemMetricsAsync();
                await CollectApplicationMetricsAsync();
                
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error collecting system metrics");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }

    private async Task CollectSystemMetricsAsync()
    {
        var process = Process.GetCurrentProcess();
        
        // Memory usage
        var memoryUsageMB = process.WorkingSet64 / 1024.0 / 1024.0;
        await _metricsService.RecordGaugeAsync("system_load", memoryUsageMB, 
            new Dictionary<string, string> { ["metric_type"] = "memory_mb" });

        // Thread count
        await _metricsService.RecordGaugeAsync("system_load", process.Threads.Count, 
            new Dictionary<string, string> { ["metric_type"] = "thread_count" });

        // CPU usage (simplified)
        var cpuUsage = await GetCpuUsageAsync();
        await _metricsService.RecordGaugeAsync("system_load", cpuUsage, 
            new Dictionary<string, string> { ["metric_type"] = "cpu_percent" });
    }

    private async Task CollectApplicationMetricsAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        
        try
        {
            // Active users count
            var userRepository = scope.ServiceProvider.GetRequiredService<IBotUserRepository>();
            var activeUsers = await userRepository.GetActiveUsersCountAsync(TimeSpan.FromMinutes(15));
            
            await _metricsService.RecordGaugeAsync("active_users", activeUsers.Students, 
                new Dictionary<string, string> { ["user_type"] = "students" });
            
            await _metricsService.RecordGaugeAsync("active_users", activeUsers.Instructors, 
                new Dictionary<string, string> { ["user_type"] = "instructors" });

            // Pending background jobs
            var backgroundJobRepository = scope.ServiceProvider.GetRequiredService<IBackgroundJobRepository>();
            var pendingJobs = await backgroundJobRepository.GetPendingJobsCountAsync();
            
            await _metricsService.RecordGaugeAsync("system_load", pendingJobs, 
                new Dictionary<string, string> { ["metric_type"] = "pending_jobs" });

            // Database connection pool metrics
            // This would depend on your connection pool implementation
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error collecting application metrics");
        }
    }

    private async Task<double> GetCpuUsageAsync()
    {
        // Simplified CPU usage calculation
        // In production, you might want to use more sophisticated methods
        var startTime = DateTime.UtcNow;
        var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
        
        await Task.Delay(1000);
        
        var endTime = DateTime.UtcNow;
        var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
        
        var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
        var totalMsPassed = (endTime - startTime).TotalMilliseconds;
        var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
        
        return cpuUsageTotal * 100;
    }
}
3. Alerting System
csharppublic interface IAlertingService
{
    Task ProcessAlertAsync(AlertRequest alert);
    Task<IEnumerable<Alert>> GetActiveAlertsAsync();
    Task ResolveAlertAsync(Guid alertId, string resolvedBy, string resolution);
    Task ConfigureAlertRuleAsync(AlertRule rule);
}

public class AlertingService : IAlertingService
{
    private readonly IAlertRepository _alertRepository;
    private readonly INotificationService _notificationService;
    private readonly ILogger<AlertingService> _logger;
    private readonly Dictionary<AlertType, AlertHandler> _alertHandlers;

    public AlertingService(
        IAlertRepository alertRepository,
        INotificationService notificationService,
        ILogger<AlertingService> logger)
    {
        _alertRepository = alertRepository;
        _notificationService = notificationService;
        _logger = logger;
        _alertHandlers = InitializeAlertHandlers();
    }

    public async Task ProcessAlertAsync(AlertRequest alert)
    {
        try
        {
            // Check for duplicate alerts (avoid spam)
            var existingAlert = await _alertRepository.GetActiveAlertAsync(alert.AlertType, alert.Message);
            if (existingAlert != null && existingAlert.CreatedAt > DateTime.UtcNow.AddMinutes(-5))
            {
                _logger.LogDebug("Duplicate alert suppressed: {AlertType}", alert.AlertType);
                return;
            }

            // Create alert record
            var alertEntity = new Alert
            {
                Id = Guid.NewGuid(),
                AlertType = alert.AlertType,
                Severity = alert.Severity,
                Message = alert.Message,
                Metadata = JsonSerializer.Serialize(alert.Metadata),
                Status = AlertStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            await _alertRepository.AddAsync(alertEntity);

            // Process with appropriate handler
            if (_alertHandlers.TryGetValue(alert.AlertType, out var handler))
            {
                await handler.HandleAsync(alertEntity);
            }

            // Send notifications based on severity
            await SendAlertNotificationsAsync(alertEntity);

            _logger.LogWarning("Alert created: {AlertType} - {Severity} - {Message}", 
                alert.AlertType, alert.Severity, alert.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing alert: {AlertType}", alert.AlertType);
        }
    }

    private async Task SendAlertNotificationsAsync(Alert alert)
    {
        var notifications = new List<Task>();

        switch (alert.Severity)
        {
            case AlertSeverity.Critical:
                // Immediate notifications to all administrators
                notifications.Add(SendEmailAlertAsync(alert, "admin@university.edu"));
                notifications.Add(SendSMSAlertAsync(alert, "+1234567890"));
                notifications.Add(SendSlackAlertAsync(alert, "#critical-alerts"));
                break;

            case AlertSeverity.Warning:
                // Email notifications to administrators
                notifications.Add(SendEmailAlertAsync(alert, "admin@university.edu"));
                notifications.Add(SendSlackAlertAsync(alert, "#alerts"));
                break;

            case AlertSeverity.Info:
                // Slack notification only
                notifications.Add(SendSlackAlertAsync(alert, "#monitoring"));
                break;
        }

        await Task.WhenAll(notifications);
    }

    private Dictionary<AlertType, AlertHandler> InitializeAlertHandlers()
    {
        return new Dictionary<AlertType, AlertHandler>
        {
            [AlertType.Performance] = new PerformanceAlertHandler(_logger),
            [AlertType.SystemHealth] = new SystemHealthAlertHandler(_logger),
            [AlertType.Security] = new SecurityAlertHandler(_logger),
            [AlertType.FaceRecognition] = new FaceRecognitionAlertHandler(_logger),
            [AlertType.Storage] = new StorageAlertHandler(_logger)
        };
    }
}

// Specific alert handlers
public abstract class AlertHandler
{
    protected readonly ILogger Logger;

    protected AlertHandler(ILogger logger)
    {
        Logger = logger;
    }

    public abstract Task HandleAsync(Alert alert);
}

public class PerformanceAlertHandler : AlertHandler
{
    public PerformanceAlertHandler(ILogger logger) : base(logger) { }

    public override async Task HandleAsync(Alert alert)
    {
        // Analyze performance issues and suggest solutions
        var metadata = JsonSerializer.Deserialize<RequestMetrics>(alert.Metadata);
        
        // Check for common performance issues
        var suggestions = new List<string>();
        
        if (metadata.DurationMs > 10000)
        {
            suggestions.Add("Consider adding caching for this endpoint");
            suggestions.Add("Review database queries for optimization");
        }
        
        if (metadata.Path.Contains("/api/face-recognition/"))
        {
            suggestions.Add("Face recognition service may be overloaded");
            suggestions.Add("Consider scaling the AI processing service");
        }

        Logger.LogInformation("Performance alert processed with {SuggestionCount} suggestions", 
            suggestions.Count);
    }
}

public class SecurityAlertHandler : AlertHandler
{
    public SecurityAlertHandler(ILogger logger) : base(logger) { }

    public override async Task HandleAsync(Alert alert)
    {
        // Handle security-related alerts
        var metadata = JsonSerializer.Deserialize<Dictionary<string, object>>(alert.Metadata);
        
        // Log security incident
        Logger.LogWarning("Security alert: {AlertType} from IP {IpAddress}", 
            alert.AlertType, metadata.GetValueOrDefault("ip_address"));
        
        // Implement automatic responses for certain security threats
        if (alert.Message.Contains("brute force"))
        {
            // Trigger automatic IP blocking
            await BlockSuspiciousIPAsync(metadata.GetValueOrDefault("ip_address")?.ToString());
        }
    }

    private async Task BlockSuspiciousIPAsync(string ipAddress)
    {
        // Implementation would depend on your firewall/security system
        Logger.LogInformation("Automatically blocked suspicious IP: {IpAddress}", ipAddress);
    }
}
4. Performance Optimization
csharppublic interface IPerformanceOptimizationService
{
    Task OptimizeDatabaseQueriesAsync();
    Task CleanupOldDataAsync();
    Task OptimizeCacheStrategyAsync();
    Task AnalyzeSlowQueriesAsync();
}

public class PerformanceOptimizationService : IPerformanceOptimizationService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<PerformanceOptimizationService> _logger;

    public async Task OptimizeDatabaseQueriesAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        
        try
        {
            // Analyze and rebuild indexes
            await RebuildIndexesAsync(context);
            
            // Update table statistics
            await UpdateTableStatisticsAsync(context);
            
            // Optimize query plans
            await OptimizeQueryPlansAsync(context);
            
            _logger.LogInformation("Database optimization completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during database optimization");
        }
    }

    private async Task RebuildIndexesAsync(ApplicationDbContext context)
    {
        var indexRebuildQueries = new[]
        {
            "REINDEX INDEX CONCURRENTLY idx_attendance_records_session_student;",
            "REINDEX INDEX CONCURRENTLY idx_students_section_active;",
            "REINDEX INDEX CONCURRENTLY idx_homework_submissions_assignment_student;",
            "REINDEX INDEX CONCURRENTLY idx_chat_history_user_date;"
        };

        foreach (var query in indexRebuildQueries)
        {
            try
            {
                await context.Database.ExecuteSqlRawAsync(query);
                _logger.LogInformation("Rebuilt index: {Query}", query);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to rebuild index: {Query}", query);
            }
        }
    }

    public async Task CleanupOldDataAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        
        var cutoffDate = DateTime.UtcNow.AddMonths(-6); // Keep 6 months of data
        
        try
        {
            // Cleanup old logs
            var deletedLogs = await context.Database.ExecuteSqlRawAsync(
                "DELETE FROM system.application_logs WHERE created_at < {0}", cutoffDate);
            
            // Cleanup old chat history
            var deletedChatHistory = await context.Database.ExecuteSqlRawAsync(
                "DELETE FROM telegram.chat_history WHERE created_at < {0}", cutoffDate);
            
            // Cleanup old video recordings (keep metadata, delete files)
            var oldVideos = await context.VideoRecordings
                .Where(v => v.CreatedAt < cutoffDate)
                .ToListAsync();
            
            foreach (var video in oldVideos)
            {
                // Delete actual video file
                // Implementation depends on your file storage service
                video.RetentionUntil = DateTime.UtcNow; // Mark for deletion
            }
            
            await context.SaveChangesAsync();
            
            _logger.LogInformation("Cleanup completed: {LogsDeleted} logs, {ChatHistoryDeleted} chat messages, {VideosMarked} videos marked for deletion", 
                deletedLogs, deletedChatHistory, oldVideos.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during data cleanup");
        }
    }

    public async Task OptimizeCacheStrategyAsync()
    {
        try
        {
            // Clear cold cache entries
            await ClearColdCacheEntriesAsync();
            
            // Preload hot data
            await PreloadHotDataAsync();
            
            // Optimize cache hit ratios
            await AnalyzeCachePerformanceAsync();
            
            _logger.LogInformation("Cache optimization completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during cache optimization");
        }
    }

    private async Task PreloadHotDataAsync()
    {
        // Preload frequently accessed data
        var hotDataTasks = new[]
        {
            PreloadActiveSectionsAsync(),
            PreloadActiveStudentsAsync(),
            PreloadSystemConfigurationAsync()
        };

        await Task.WhenAll(hotDataTasks);
    }

    private async Task PreloadActiveSectionsAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        
        var activeSections = await context.Sections
            .Where(s => s.IsActive)
            .Include(s => s.StudyType)
            .Include(s => s.AcademicStage)
            .ToListAsync();

        foreach (var section in activeSections)
        {
            var cacheKey = $"section:{section.Id}";
            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(section), 
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
        }

        _logger.LogInformation("Preloaded {SectionCount} active sections to cache", activeSections.Count);
    }
}

🚀 خطة التنفيذ {#implementation}
المرحلة الأولى: الإعداد الأساسي (الأسبوع 1-2)
الأسبوع الأول:
أولوية عليا:

إعداد البنية التحتية:
bash# إعداد قاعدة البيانات
docker-compose up -d postgresql redis minio

# إنشاء قواعد البيانات والمخططات
dotnet ef database update --context IdentityDbContext
dotnet ef database update --context AcademicDbContext
dotnet ef database update --context AttendanceDbContext

إنشاء المشروع الأساسي:
bash# إنشاء Solution
dotnet new sln -n SmartAttendanceSystem

# إنشاء المشاريع الفرعية
dotnet new webapi -n SmartAttendance.API
dotnet new classlib -n SmartAttendance.Core
dotnet new classlib -n SmartAttendance.Infrastructure
dotnet new classlib -n SmartAttendance.Services

تكوين Identity والأمان:
csharp// Program.cs
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddJwtAuthentication(builder.Configuration);


الأهداف المحددة:

 إعداد قواعد البيانات والمخططات
 تكوين Identity Service
 إنشاء Basic JWT Authentication
 تطبيق Basic Rate Limiting
 إعداد Logging الأساسي

الأسبوع الثاني:
أولوية عليا:

Student Management Service:
csharp// تطبيق Basic CRUD للطلاب
[HttpPost("register")]
public async Task<IActionResult> RegisterStudent([FromBody] CreateStudentRequest request)
{
    var result = await _studentService.RegisterStudentAsync(request);
    return Ok(result);
}

إعداد MinIO Storage:
csharpbuilder.Services.AddSingleton<IMinioStorageService, MinioStorageService>();
builder.Services.Configure<MinioSettings>(builder.Configuration.GetSection("MinIO"));


الأهداف المحددة:

 تطبيق Student Registration API
 إعداد File Storage الأساسي
 إنشاء Basic Section Management
 تطبيق QR Code Generation
 Basic Unit Tests

المرحلة الثانية: Face Recognition والكاميرات (الأسبوع 3-4)
الأسبوع الثالث:
أولوية عليا:

إعداد Python Face Recognition Service:
python# requirements.txt
fastapi==0.104.1
opencv-python==4.8.1.78
face-recognition==1.3.0
dlib==19.24.2
numpy==1.24.3
pillow==10.0.1
asyncpg==0.29.0

# main.py
from fastapi import FastAPI
app = FastAPI(title="Face Recognition Service")

@app.post("/api/face-recognition/process-video")
async def process_video(request: ProcessVideoRequest):
    # Implementation

Camera Management Service:
csharppublic class CameraManagementService : ICameraManagementService
{
    public async Task<CameraSession> StartRecordingAsync(StartRecordingRequest request)
    {
        // تطبيق إدارة الكاميرات
    }
}


الأهداف المحددة:

 تطبيق Python Face Recognition Service
 إنشاء Camera Management APIs
 تطبيق Video Upload والـ Processing
 إعداد Integration بين .NET و Python
 Basic Face Recognition Tests

الأسبوع الرابع:
أولوية عليا:

Attendance Service Implementation:
csharppublic class AttendanceService : IAttendanceService
{
    public async Task<AttendanceSession> CreateSessionAsync(CreateAttendanceSessionRequest request)
    {
        // تطبيق جلسات الحضور
    }
}

Integration Testing:
csharp[Test]
public async Task Should_Process_Video_And_Mark_Attendance()
{
    // End-to-end test للنظام كاملاً
}


الأهداف المحددة:

 تطبيق Attendance Session Management
 ربط Face Recognition مع Attendance Records
 تطبيق Manual Override للمحاضرين
 Integration Tests شاملة
 Performance Testing الأولي

المرحلة الثالثة: Telegram Bot والذكاء الاصطناعي (الأسبوع 5-6)
الأسبوع الخامس:
أولوية عليا:

Telegram Bot Service:
csharppublic class TelegramBotService : ITelegramBotService
{
    public async Task HandleUpdateAsync(Update update)
    {
        // معالجة رسائل التلغرام
    }
}

Bot Commands Implementation:
csharppublic class MyAttendanceCommand : BaseBotCommand
{
    protected override async Task ExecuteCommandAsync(BotCommandContext context)
    {
        // تطبيق أوامر البوت
    }
}


الأهداف المحددة:

 تطبيق Telegram Bot Basic Structure
 إنشاء Student Commands (حضوري، واجباتي)
 تطبيق QR Code Linking
 Basic Homework Management
 User Registration عبر QR

الأسبوع السادس:
أولوية عليا:

AI Chat Service:
python# Bologna AI Specialist
class BolognaSystemAI:
    async def process_query(self, query: BolognaQuery) -> BolognaResponse:
        # AI processing for Bologna system queries

Smart Penalty System:
csharppublic class PenaltyService : IPenaltyService
{
    public async Task<PenaltyResult> ApplyPenaltyAsync(ApplyPenaltyRequest request)
    {
        // تطبيق نظام العقوبات الذكي
    }
}


الأهداف المحددة:

 تطبيق AI Chat للاستشارات الأكاديمية
 إنشاء Smart Penalty System
 تطبيق Admin Commands للبوت
 Student Behavior Analysis الأساسي
 Notification System

المرحلة الرابعة: Web Dashboard والتحسينات (الأسبوع 7-8)
الأسبوع السابع:
أولوية عليا:

Web Dashboard للإداريين:
csharp[Authorize(Roles = "Administrator")]
public class DashboardController : ControllerBase
{
    [HttpGet("overview")]
    public async Task<IActionResult> GetDashboardOverview()
    {
        // Dashboard APIs
    }
}

Advanced Reporting:
csharppublic class ReportingService : IReportingService
{
    public async Task<AttendanceReport> GenerateAttendanceReportAsync(ReportRequest request)
    {
        // تقارير متقدمة
    }
}


الأهداف المحددة:

 تطبيق Web Dashboard الأساسي
 إنشاء Advanced Reporting APIs
 تطبيق Data Visualization
 Excel/PDF Export Functionality
 Real-time Dashboard Updates

الأسبوع الثامن:
أولوية عليا:

Performance Optimization:
csharp// تحسين الأداء والذاكرة
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = connectionString;
});

Monitoring والـ Alerting:
csharppublic class PerformanceMonitoringService : IPerformanceMonitoringService
{
    public async Task<HealthCheckResult> CheckSystemHealthAsync()
    {
        // مراقبة الأداء
    }
}


الأهداف المحددة:

 تطبيق Caching Strategy شامل
 إنشاء Health Checks
 تطبيق Performance Monitoring
 Advanced Alerting System
 Load Testing والتحسين

خطة التحقق والاختبار:
Testing Strategy:
Unit Tests (كل أسبوع):
csharppublic class StudentServiceTests
{
    [Test]
    public async Task RegisterStudent_ShouldCreateStudent_WhenValidRequest()
    {
        // Arrange
        var request = new CreateStudentRequest { /* test data */ };
        
        // Act
        var result = await _studentService.RegisterStudentAsync(request);
        
        // Assert
        Assert.IsTrue(result.Success);
    }
}
Integration Tests (نهاية كل مرحلة):
csharppublic class AttendanceIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task CompleteAttendanceFlow_ShouldWork_EndToEnd()
    {
        // Test complete attendance marking flow
    }
}
Load Testing (الأسبوع الأخير):
bash# k6 load testing
k6 run --vus 100 --duration 30s load-test.js
المراقبة المستمرة:
Daily Checklist:

 Build Status ✅
 Test Coverage > 80% ✅
 Performance Metrics ✅
 Security Scans ✅
 Database Health ✅

Weekly Reviews:

 Code Review Sessions
 Performance Analysis
 Security Assessment
 Progress vs Timeline
 Risk Assessment

Deployment Strategy:
Development Environment:
yaml# docker-compose.dev.yml
version: '3.8'
services:
  api:
    build: .
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
    ports:
      - "5000:80"
  
  postgres:
    image: postgres:15
    environment:
      POSTGRES_DB: smartattendance_dev
  
  redis:
    image: redis:7-alpine
  
  minio:
    image: minio/minio
Production Deployment:
bash# Production deployment steps
docker build -t smartattendance:latest .
docker tag smartattendance:latest registry.company.com/smartattendance:latest
docker push registry.company.com/smartattendance:latest

# Deploy with Kubernetes or Docker Swarm
kubectl apply -f k8s/production/
Risk Mitigation:
Technical Risks:

Face Recognition Accuracy:

Risk: منخفضة الدقة في ظروف الإضاءة المختلفة
Mitigation: اختبار مكثف في ظروف متنوعة + backup manual override


System Performance:

Risk: بطء المعالجة مع 4 كاميرات متزامنة
Mitigation: Load testing مبكر + horizontal scaling capability


Data Privacy:

Risk: مخاوف الخصوصية للبيانات البيومترية
Mitigation: تشفير قوي + compliance with privacy laws



Project Risks:

Timeline Pressure:

Risk: شهرين قد لا يكفي للتطبيق الكامل
Mitigation: أولوية للمميزات الأساسية + agile approach


Integration Complexity:

Risk: تعقيد في ربط النظم المختلفة
Mitigation: Integration testing مبكر + modular architecture



Success Criteria:
Technical Metrics:

 Face Recognition Accuracy > 90%
 API Response Time < 200ms
 System Uptime > 99.5%
 Video Processing < 2x real-time
 Zero Critical Security Vulnerabilities

Business Metrics:

 100% Student Registration Success
 95% Attendance Accuracy
 90% User Satisfaction
 80% Reduction in Manual Work
 Zero Data Loss Incidents

Quality Metrics:

 Code Coverage > 80%
 All Unit Tests Pass
 All Integration Tests Pass
 Performance Tests Pass
 Security Scans Clean


📋 الخلاصة والتوصيات النهائية
ملخص النظام:
تم تصميم نظام الحضور الذكي المتكامل كحل شامل وعصري لإدارة الحضور في الجامعات باستخدام أحدث التقنيات. النظام يجمع بين:

تقنيات الذكاء الاصطناعي للتعرف على الوجوه وتحليل السلوك
أتمتة كاملة لعملية تسجيل الحضور والإدارة
تكامل ذكي مع Telegram Bot للتفاعل المباشر مع الطلاب
أمان متقدم لحماية البيانات الحساسة
مراقبة شاملة للأداء والنظام

المميزات الرئيسية المحققة:
للطلاب:
✅ تسجيل آلي للحضور دون تدخل يدوي
✅ تقارير شخصية فورية عبر Telegram
✅ نظام إنذار مبكر للغيابات
✅ مساعد ذكي للاستشارات الأكاديمية
✅ إدارة الواجبات والتذكيرات
للمحاضرين:
✅ بدء/إيقاف جلسات الحضور بسهولة
✅ تقارير مفصلة عن أداء الطلاب
✅ إمكانية التعديل اليدوي للحضور
✅ إشعارات فورية عن حالات الغياب
للإداريين:
✅ لوحة تحكم شاملة للنظام
✅ تقارير إحصائية متقدمة
✅ مراقبة الأداء الأكاديمي
✅ إدارة مركزية للطلاب والمحاضرين
التقييم التقني:
نقاط القوة:

معمارية Microservices قابلة للتوسع والصيانة
تكامل هجين .NET + Python يجمع بين القوة والمرونة
أمان متعدد الطبقات يحمي البيانات الحساسة
ذكاء اصطناعي متقدم للتحليل والتوقعات
مراقبة شاملة للأداء والصحة

التحديات المحتملة:

تعقيد النظام يتطلب خبرة تقنية عالية
استهلاك الموارد للمعالجة المتزامنة لـ4 كاميرات
دقة التعرف في ظروف الإضاءة المختلفة
أمان البيانات البيومترية والامتثال للقوانين

التوصيات للتطوير:
المرحلة الأولى (الإنتاج):

التركيز على الـ MVP: تطبيق المميزات الأساسية أولاً
اختبار مكثف: خاصة لدقة Face Recognition
أمان أولاً: تطبيق جميع متطلبات الأمان من البداية
مراقبة مستمرة: إعداد monitoring شامل من اليوم الأول

المرحلة الثانية (التحسين):

تحسين الأداء: optimization مستمر للاستعلامات والخوارزميات
مميزات متقدمة: إضافة Analytics أكثر تفصيلاً
تكامل أوسع: ربط مع أنظمة الجامعة الأخرى
ذكاء أكثر: تطوير AI models أكثر دقة

المرحلة الثالثة (التوسع):

دعم متعدد الكليات: توسيع النظام ليشمل كليات أخرى
تطبيق جوال متقدم: إضافة مميزات أكثر للتطبيق
تحليلات تنبؤية: استخدام Machine Learning للتنبؤ بالأداء
تكامل IoT: ربط مع أجهزة إضافية في القاعات

متطلبات التشغيل الموصى بها:
للإنتاج:
yamlServer Specifications:
  CPU: 16 cores minimum
  RAM: 32GB minimum  
  Storage: 1TB SSD
  Network: 1Gbps
  GPU: NVIDIA RTX 4070 (للـ AI processing)

Database:
  PostgreSQL 15+ cluster
  Redis 7+ cluster
  MinIO distributed storage

Monitoring:
  Prometheus + Grafana
  ELK Stack for logging
  Custom alerting system
الاعتبارات القانونية والأخلاقية:
خصوصية البيانات:

تشفير كامل للبيانات البيومترية
صلاحيات محدودة لكل مستوى مستخدم
شفافية كاملة في كيفية استخدام البيانات
حق المسح للطلاب المتخرجين

الامتثال للقوانين:

GDPR compliance للبيانات الشخصية
قوانين الخصوصية المحلية
سياسات الجامعة للبيانات الأكاديمية

خطة الصيانة والدعم:
صيانة دورية:

نسخ احتياطية يومية للبيانات الحرجة
تحديثات أمنية شهرية
مراجعة الأداء أسبوعية
تدريب المستخدمين ربع سنوي

دعم تقني:

فريق دعم 24/7 للمشاكل الحرجة
دليل مستخدم شامل وتدريب
نظام ticketing لمتابعة المشاكل
تحديثات منتظمة للمميزات

النظرة المستقبلية:
هذا النظام يمثل نقلة نوعية في إدارة الحضور الجامعي، ويضع الأساس لتطوير حلول تعليمية ذكية أكثر تقدماً. مع التطوير المستمر، يمكن أن يصبح النموذج المعياري للجامعات الحديثة.
الهدف النهائي: جامعة ذكية بالكامل حيث التقنية تخدم التعليم بأعلى مستويات الكفاءة والشفافية.

هذه أكواد توضيحية - ليست للتجريب المباشر! ⚠️
الهدف منها:

دليل للمبرمج: لفهم بنية النظام وكيفية تصميمه
مرجع للتطوير: كل كلاس وميثود يوضح الوظيفة المطلوبة
خارطة طريق: لتسهيل كتابة الكود الفعلي


المطلوب لجعله قابل للتشغيل:

إكمال كتابة الـ 5000+ سطر المتبقية
إضافة ملفات الإعدادات
إعداد قواعد البيانات
تكامل المكتبات الخارجية
اختبار وتصحيح الأخطاء
لماذا ليست جاهزة للتشغيل؟

مفقود: Connection strings وإعدادات قواعد البيانات
مفقود: Dependencies والمكتبات المطلوبة
مفقود: Configuration files و appsettings.json
مفقود: Database migrations و Entity Framework setup
مفقود: Error handling تفصيلي
مفقود: Unit tests و Integration tests

كيف تستخدم هذا الدليل؟

اقرأ البنية العامة → افهم النظام
خذ الكلاسات والواجهات → كقالب للتطوير
اتبع التصميم المقترح → لبناء النظام
أضف التفاصيل المفقودة → أثناء التطوير

هذه أكواد توضيحية - ليست للتجريب المباشر! ⚠️
الهدف منها:

دليل للمبرمج: لفهم بنية النظام وكيفية تصميمه
مرجع للتطوير: كل كلاس وميثود يوضح الوظيفة المطلوبة
خارطة طريق: لتسهيل كتابة الكود الفعلي


المطلوب لجعله قابل للتشغيل:

إكمال كتابة الـ 5000+ سطر المتبقية
إضافة ملفات الإعدادات
إعداد قواعد البيانات
تكامل المكتبات الخارجية
اختبار وتصحيح الأخطاء
لماذا ليست جاهزة للتشغيل؟

مفقود: Connection strings وإعدادات قواعد البيانات
مفقود: Dependencies والمكتبات المطلوبة
مفقود: Configuration files و appsettings.json
مفقود: Database migrations و Entity Framework setup
مفقود: Error handling تفصيلي
مفقود: Unit tests و Integration tests

كيف تستخدم هذا الدليل؟

اقرأ البنية العامة → افهم النظام
خذ الكلاسات والواجهات → كقالب للتطوير
اتبع التصميم المقترح → لبناء النظام
أضف التفاصيل المفقودة → أثناء التطوير