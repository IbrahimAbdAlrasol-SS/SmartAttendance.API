📊 فلوش تشارت مفصل للعمليات الرئيسية
🔄 Student Registration Flow
mermaidflowchart TD
    A[👤 Student opens Mobile App] --> B{📱 First time user?}
    B -->|Yes| C[📝 Fill Registration Form]
    B -->|No| D[🔑 Login with credentials]
    
    C --> E[📧 Email validation]
    E --> F[🤳 Face Training Process]
    F --> G[📷 Capture 50+ photos]
    G --> H[🧠 Local AI processing]
    H --> I{✅ Training successful?}
    
    I -->|No| J[❌ Show error & retry]
    J --> G
    
    I -->|Yes| K[🔐 Encrypt face data]
    K --> L[☁️ Upload to server]
    L --> M[🔍 Server validation]
    M --> N{✅ Data valid?}
    
    N -->|No| O[❌ Reject & notify]
    N -->|Yes| P[💾 Save to database]
    
    P --> Q[🔢 Generate QR Code]
    Q --> R[📱 Show QR to student]
    R --> S[📲 Student scans with Telegram]
    S --> T[🤖 Bot links account]
    T --> U[✅ Registration Complete]
    
    D --> V[🔐 Authenticate user]
    V --> W{✅ Valid credentials?}
    W -->|No| X[❌ Show login error]
    W -->|Yes| Y[📱 Main app interface]
🎥 Attendance Session Flow
mermaidflowchart TD
    A[👨‍🏫 Instructor logs in] --> B[📋 Select section]
    B --> C[▶️ Start attendance session]
    C --> D[📡 Check camera availability]
    D --> E{📹 4 cameras available?}
    
    E -->|No| F[⚠️ Show available cameras]
    F --> G[🔧 Allocate available cameras]
    
    E -->|Yes| H[📹 Allocate all 4 cameras]
    
    G --> I[🎬 Start recording]
    H --> I
    
    I --> J[👁️ Real-time face detection]
    J --> K[🧠 Face recognition processing]
    K --> L[📊 Update attendance records]
    
    L --> M{⏰ Session ended?}
    M -->|No| N[🔄 Continue processing]
    N --> J
    
    M -->|Yes| O[⏹️ Stop recording]
    O --> P[💾 Save video files]
    P --> Q[📈 Generate statistics]
    Q --> R[📧 Send notifications]
    R --> S[✅ Session complete]
    
    subgraph "🔄 Parallel Processing"
        T[📹 Camera 1 processing]
        U[📹 Camera 2 processing]
        V[📹 Camera 3 processing]
        W[📹 Camera 4 processing]
    end
    
    J --> T
    J --> U
    J --> V
    J --> W
📝 Homework & Penalty Flow
mermaidflowchart TD
    A[👥 Bot Admin creates homework] --> B[📝 Set parameters]
    B --> C[📅 Set deadline]
    C --> D[📢 Broadcast to section]
    D --> E[⏰ Start monitoring]
    
    E --> F{📊 Check submission status}
    F --> G{⏱️ 50% time passed?}
    
    G -->|Yes| H{📝 Student submitted?}
    H -->|No| I[⚠️ Send warning message]
    I --> J[📊 Increment warning counter]
    
    G -->|No| K[⏰ Continue monitoring]
    K --> F
    
    H -->|Yes| L[✅ Mark as submitted]
    L --> K
    
    J --> M{⏱️ 75% time passed?}
    M -->|Yes| N{📝 Still not submitted?}
    N -->|Yes| O[🔇 Mute student]
    O --> P[⏰ Calculate mute duration]
    P --> Q[📊 Update penalty counter]
    
    M -->|No| K
    N -->|No| K
    
    Q --> R{⏱️ 90% time passed?}
    R -->|Yes| S{📝 Still not submitted?}
    S -->|Yes| T[📢 Public shame in groups]
    T --> U[🚨 Final warning]
    
    R -->|No| K
    S -->|No| K
    
    U --> V{⏱️ Deadline reached?}
    V -->|Yes| W[❌ Mark as late/missing]
    W --> X[📊 Apply final penalties]
    X --> Y[📈 Update analytics]
    
    V -->|No| K
🤖 AI Chat & Analysis Flow
mermaidflowchart TD
    A[👤 Student asks question] --> B[📱 Message received by bot]
    B --> C[🧠 Analyze message intent]
    C --> D{🎯 Intent category?}
    
    D -->|Bologna System| E[📚 Query knowledge base]
    D -->|Academic Performance| F[📊 Analyze student data]
    D -->|Behavioral| G[📈 Behavioral analysis]
    D -->|General| H[💬 General response]
    
    E --> I[🔍 Search relevant info]
    I --> J[📝 Generate contextualized response]
    
    F --> K[📊 Get attendance data]
    K --> L[📝 Get homework data]
    L --> M[🧮 Calculate performance metrics]
    M --> N[📝 Generate personalized insights]
    
    G --> O[📈 Analyze patterns]
    O --> P[🚨 Check risk indicators]
    P --> Q[💡 Generate recommendations]
    
    H --> R[💬 Standard AI response]
    
    J --> S[📤 Send response to student]
    N --> S
    Q --> S
    R --> S
    
    S --> T[📊 Log interaction]
    T --> U[🧠 Update behavioral model]
    U --> V{🚨 Intervention needed?}
    
    V -->|Yes| W[📧 Alert counselor/instructor]
    V -->|No| X[✅ Continue monitoring]
    
    W --> Y[👨‍🏫 Human intervention]
    X --> Z[📈 Update analytics]
    Y --> Z