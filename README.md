# MuseoAR: Augmented Reality Museum Experience

## 🎓 Thesis Project - BSCS 411 Group 2

**Academic Year:** 2024-2025  
**Course:** BSCS 411 (Thesis)  
**Institution:** STI College Las Piñas

### 👥 Group Members
- **Prinze Mikhail Sadsad**
- **Jerald Kyle Ordaz**
- **James Vincent Bade**
- **Julianne Cruz**

---

## 📱 Project Overview

**MuseoAR** is an innovative Augmented Reality (AR) mobile application that revolutionizes the museum experience by bringing historical artworks to life through immersive 3D interactions. The application allows users to scan museum paintings and sculptures to unlock interactive content, educational quizzes, and detailed information about Philippine art and history.

### 🎯 Key Features

- **🔍 AR Image Recognition**: Scan museum artworks to trigger 3D models and interactive content
- **📚 Educational Quizzes**: Interactive quizzes about each artwork with localized content
- **🌍 Multi-language Support**: English and Filipino language options
- **👤 User Authentication**: Secure Firebase-based user registration and login
- **📊 Analytics Tracking**: Real-time scan tracking and user engagement metrics
- **🎨 3D Model Integration**: Interactive 3D representations of historical artworks
- **📱 Cross-platform**: Android and iOS support

### 🏛️ Featured Artworks

The application includes interactive content for famous Philippine artworks:

- **Basi Revolt II** - Historical painting by Esteban Villanueva
- **Basi Revolt VI** - Revolutionary artwork depicting the Basi Revolt
- **Bataan** - War memorial artwork
- **Parisian Life** - Juan Luna's masterpiece
- **Spoliarium** - Juan Luna's most famous work
- **The Assassination of Governor Bustamante** - Historical painting
- **Una Bulaquena** - Traditional Filipino artwork

---

## 🛠️ Technical Specifications

### **Development Environment**
- **Unity Version**: 6000.0.28f1
- **Platform**: Android/iOS
- **AR Framework**: AR Foundation 6.0.3
- **Backend**: Firebase (Authentication, Realtime Database)
- **Localization**: Unity Localization Package

### **Key Technologies**
- **AR Foundation**: ARCore (Android) & ARKit (iOS)
- **Firebase**: Authentication, Realtime Database, Analytics
- **Unity XR**: Cross-platform AR development
- **TextMeshPro**: Advanced text rendering
- **Unity Localization**: Multi-language support

### **Architecture Components**

#### **Core Scripts**
- `TrackedImageSpawner.cs` - AR image recognition and 3D model spawning
- `FirebaseAuthManager.cs` - User authentication and session management
- `Database.cs` - User data management and Firebase integration
- `Quiz.cs` - Interactive quiz system with localized content
- `UIManager.cs` - User interface management
- `FeedbackSpawner.cs` - User feedback and interaction systems

#### **Scene Structure**
- **Startup.unity** - Terms of service and initial setup
- **Authentication.unity** - User login and registration
- **User Data.unity** - User profile data collection
- **Camera.unity** - Main AR experience and artwork scanning

---

## 🚀 Installation & Setup

### **Prerequisites**
- Unity 6000.0.28f1 or later
- Android SDK (for Android builds)
- Xcode (for iOS builds)
- Firebase project with Authentication and Realtime Database enabled

### **Setup Instructions**

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Satsuna/MuseoAR.git
   cd MuseoAR
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Open the MuseoAR project folder
   - Ensure Unity version 6000.0.28f1 is selected

3. **Firebase Configuration**
   - Replace `google-services.json` (Android) and `google-services-desktop.json` with your Firebase project files
   - Update Firebase Database URL in `Database.cs` (line 21)
   - Configure Firebase Authentication in Unity

4. **Build Settings**
   - Set target platform (Android/iOS)
   - Configure AR Foundation settings
   - Build and deploy to target device

### **Dependencies**
- AR Foundation 6.0.3
- ARCore XR Plugin 6.0.3
- ARKit XR Plugin 6.0.3
- Firebase Unity SDK
- Unity Localization Package
- TextMeshPro

---

## 📱 User Experience Flow

1. **Startup** → Terms of Service acceptance
2. **Authentication** → User registration/login
3. **User Data** → Profile information collection
4. **Camera** → AR scanning and interaction
5. **Quiz System** → Educational content engagement
6. **Analytics** → Usage tracking and feedback

---

## 🎨 Design Philosophy

**MuseoAR** follows a user-centric design approach that prioritizes:
- **Accessibility**: Multi-language support for diverse users
- **Education**: Interactive learning through AR technology
- **Engagement**: Gamified quiz system to enhance learning
- **Performance**: Optimized AR rendering for smooth mobile experience

---

## 📊 Analytics & Data Collection

The application tracks:
- Total artwork scans
- Individual painting scan counts
- User engagement metrics
- Quiz completion rates
- User demographics (age, nationality)

---

## 🔮 Future Enhancements

- **Social Features**: User-generated content and sharing
- **Advanced AR**: 3D model manipulation and interaction
- **Museum Integration**: Real-time museum data synchronization
- **Accessibility**: Voice narration and haptic feedback
- **Offline Mode**: Cached content for areas with poor connectivity

---

## 📄 Academic Context

This project serves as the capstone thesis for BSCS 411, demonstrating the practical application of:
- **Augmented Reality Development**
- **Mobile Application Architecture**
- **User Experience Design**
- **Database Management**
- **Cross-platform Development**

---

## 📞 Contact Information

**Thesis Group 2 - BSCS 411**
- **Email**: museoar2024@gmail.com
- **Repository**: https://github.com/Satsuna/MuseoAR
<!-- - **Documentation**: [where link]-->

---

## 📜 License

This project is developed for academic purposes as part of BSCS 411 Thesis requirements. All rights reserved to the development team and academic institution.

---

*Built with ❤️ by BSCS 411 Group 2 - Bringing Philippine Art to Life Through Augmented Reality*
